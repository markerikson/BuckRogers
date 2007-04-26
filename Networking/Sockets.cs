// Client-server helpers for TCP/IP
// ClientInfo: wrapper for a socket which throws
//  Receive events, and allows for messaged communication (using
//  a specified character as end-of-message)
// Server: simple TCP server that throws Connect events
// ByteBuilder: utility class to manage byte arrays built up
//  in multiple transactions

// (C) Richard Smith 2005-7
//   bobjanova@gmail.com
// You can use this for free and give it to people as much as you like
// as long as you leave a credit to me :).

// Code to connect to a SOCKS proxy modified from
//   http://www.thecodeproject.com/csharp/ZaSocks5Proxy.asp

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.Sockets;

namespace RedCorona.Net
{
	public delegate void ConnectionRead(ClientInfo ci, String text);
	public delegate void ConnectionClosed(ClientInfo ci);
	public delegate void ConnectionReadBytes(ClientInfo ci, byte[] bytes, int len);
	public delegate void ConnectionReadMessage(ClientInfo ci, uint code, byte[] bytes, int len);

	public enum ClientDirection { In, Out, Left, Right, Both };
	public enum MessageType { Unmessaged, EndMarker, Length, CodeAndLength };

	// OnReadBytes: catch the raw bytes as they arrive
	// OnRead: for text ended with a marker character
	// OnReadMessage: for binary info on a messaged client
	public class ClientInfo
	{
		internal Server server = null;
		private Socket sock;
		private String buffer;
		public ConnectionRead OnRead;
		public ConnectionClosed OnClose;
		public ConnectionReadBytes OnReadBytes;
		public ConnectionReadMessage OnReadMessage;
		public MessageType MessageType;
		private ClientDirection dir;
		int id;
		bool alreadyclosed = false;
		public static int NextID = 1;
		//private ClientThread t;
		public object Data = null;

		private char delim;
		public const int BUFSIZE = 1024;
		byte[] buf = new byte[BUFSIZE];
		ByteBuilder bytes = new ByteBuilder(10);

		byte[] msgheader = new byte[8];
		byte headerread = 0;

		public char Delimiter
		{
			get { return delim; }
			set { delim = value; }
		}

		public ClientDirection Direction { get { return dir; } }
		public Socket Socket { get { return sock; } }
		public Server Server { get { return server; } }
		public int ID { get { return id; } }

		public bool Closed
		{
			get { return !sock.Connected; }
		}

		public ClientInfo(Socket cl, bool StartNow) : this(cl, null, null, ClientDirection.Both, StartNow) { }
		public ClientInfo(Socket cl, ConnectionRead read, ConnectionReadBytes readevt, ClientDirection d) : this(cl, read, readevt, d, true) { }
		public ClientInfo(Socket cl, ConnectionRead read, ConnectionReadBytes readevt, ClientDirection d, bool StartNow)
		{
			sock = cl; buffer = ""; OnReadBytes = readevt;
			OnRead = read;
			MessageType = MessageType.EndMarker;
			dir = d; delim = '\n';
			id = NextID; // Assign each client an application-unique ID
			unchecked { NextID++; }
			//t = new ClientThread(this);
			if (StartNow) BeginReceive();
		}

		public void BeginReceive()
		{
			//			t.t.Start();
			sock.BeginReceive(buf, 0, BUFSIZE, 0, new AsyncCallback(ReadCallback), this);
		}

		public String Send(String text)
		{
			byte[] ba = Encoding.UTF8.GetBytes(text);
			String s = "";
			for (int i = 0; i < ba.Length; i++) s += ba[i] + " ";
			sock.Send(ba);
			return s;
		}

		public void SendMessage(uint code, byte[] bytes) { SendMessage(code, bytes, 0, bytes.Length); }
		public void SendMessage(uint code, byte[] bytes, ParameterType paramType) { SendMessage(code, bytes, paramType, bytes.Length); }
		public void SendMessage(uint code, byte[] bytes, ParameterType paramType, int len)
		{
			if (paramType > 0)
			{
				ByteBuilder b = new ByteBuilder(3);
				b.AddParameter(bytes, paramType);
				bytes = b.Read(0, b.Length);
				len = bytes.Length;
			}

			lock (sock)
			{
				switch (MessageType)
				{
					case MessageType.CodeAndLength:
						sock.Send(UintToBytes(code));
						sock.Send(IntToBytes(len));
						break;
					case MessageType.Length:
						sock.Send(IntToBytes(len));
						break;
				}
				sock.Send(bytes, len, SocketFlags.None);
			}
		}

		public bool MessageWaiting()
		{
			FillBuffer(sock);
			return buffer.IndexOf(delim) >= 0;
		}

		public String Read()
		{
			//FillBuffer(sock);
			int p = buffer.IndexOf(delim);
			if (p >= 0)
			{
				String res = buffer.Substring(0, p);
				buffer = buffer.Substring(p + 1);
				return res;
			}
			else return "";
		}

		private void FillBuffer(Socket sock)
		{
			byte[] buf = new byte[256];
			int read;
			while (sock.Available != 0)
			{
				read = sock.Receive(buf);
				if (OnReadBytes != null) OnReadBytes(this, buf, read);
				buffer += Encoding.UTF8.GetString(buf, 0, read);
			}
		}

		void ReadCallback(IAsyncResult ar)
		{
			try
			{
				int read = sock.EndReceive(ar);
				//Console.WriteLine("Socket "+ID+" read "+read+" bytes");
				if (read > 0)
				{
					DoRead(buf, read);
					BeginReceive();
				}
				else
				{
					Close();
				}
			}
			catch (SocketException) { Close(); }
			catch (ObjectDisposedException) { Close(); }
		}

		internal void DoRead(byte[] buf, int read)
		{
			if (read > 0)
			{
				if (OnReadBytes != null) OnReadBytes(this, buf, read);
				if (OnRead != null)
				{ // Simple text mode
					buffer += Encoding.UTF8.GetString(buf, 0, read);
					while (buffer.IndexOf(delim) >= 0) OnRead(this, Read());
				}
			}
			ReadInternal(buf, read);
		}

		void ReadInternal(byte[] buf, int read)
		{
			if ((OnReadMessage != null) && (MessageType != MessageType.Unmessaged))
			{
				// Messaged mode
				int copied;
				uint code = 0;
				switch (MessageType)
				{
					case MessageType.CodeAndLength:
					case MessageType.Length:
						int length;
						if (MessageType == MessageType.Length)
						{
							copied = FillHeader(ref buf, 4, read);
							if (headerread < 4) break;
							length = GetInt(msgheader, 0, 4);
						}
						else
						{
							copied = FillHeader(ref buf, 8, read);
							if (headerread < 8) break;
							code = (uint)GetInt(msgheader, 0, 4);
							length = GetInt(msgheader, 4, 4);
						}
						bytes.Add(buf, 0, read - copied);
						if (bytes.Length >= length)
						{
							// A message was received!
							headerread = 0;
							byte[] msg = bytes.Read(0, length);
							OnReadMessage(this, code, msg, length);
							// Don't forget to put the rest through the mill
							byte[] whatsleft = bytes.Read(length, bytes.Length - length);
							bytes.Clear();
							if (whatsleft.Length > 0) ReadInternal(whatsleft, whatsleft.Length);
						}
						//if(OnStatus != null) OnStatus(this, bytes.Length, length);
						break;
				}
			}
		}

		int FillHeader(ref byte[] buf, int to, int read)
		{
			int copied = 0;
			if (headerread < to)
			{
				// First copy the header into the header variable.
				for (int i = 0; (i < read) && (headerread < to); i++, headerread++, copied++)
				{
					msgheader[headerread] = buf[i];
				}
			}
			if (copied > 0)
			{
				// Take the header bytes off the 'message' section
				byte[] newbuf = new byte[read - copied];
				for (int i = 0; i < newbuf.Length; i++) newbuf[i] = buf[i + copied];
				buf = newbuf;
			}
			return copied;
		}

		public static int GetInt(byte[] ba, int from, int len)
		{
			int r = 0;
			for (int i = 0; i < len; i++)
				r += ba[from + i] << ((len - i - 1) * 8);
			return r;
		}

		public static int[] GetIntArray(byte[] ba)
		{
			int[] res = new int[ba.Length / 4];
			for (int i = 0; i < res.Length; i++)
			{
				res[i] = GetInt(ba, i * 4, 4);
			}
			return res;
		}

		public static uint[] GetUintArray(byte[] ba)
		{
			uint[] res = new uint[ba.Length / 4];
			for (int i = 0; i < res.Length; i++)
			{
				res[i] = (uint)GetInt(ba, i * 4, 4);
			}
			return res;
		}

		public static byte[] IntToBytes(int val) { return UintToBytes((uint)val); }
		public static byte[] UintToBytes(uint val)
		{
			byte[] res = new byte[4];
			for (int i = 3; i >= 0; i--)
			{
				res[i] = (byte)val; val >>= 8;
			}
			return res;
		}

		public static byte[] IntArrayToBytes(int[] val)
		{
			byte[] res = new byte[val.Length * 4];
			for (int i = 0; i < val.Length; i++)
			{
				byte[] vb = IntToBytes(val[i]);
				res[(i * 4)] = vb[0];
				res[(i * 4) + 1] = vb[1];
				res[(i * 4) + 2] = vb[2];
				res[(i * 4) + 3] = vb[3];
			}
			return res;
		}

		public static byte[] UintArrayToBytes(uint[] val)
		{
			byte[] res = new byte[val.Length * 4];
			for (uint i = 0; i < val.Length; i++)
			{
				byte[] vb = IntToBytes((int)val[i]);
				res[(i * 4)] = vb[0];
				res[(i * 4) + 1] = vb[1];
				res[(i * 4) + 2] = vb[2];
				res[(i * 4) + 3] = vb[3];
			}
			return res;
		}

		public void Close()
		{
			if (!alreadyclosed)
			{
				if (server != null) server.ClientClosed(this);
				if (OnClose != null) OnClose(this);
				alreadyclosed = true;
			}
			sock.Close();
		}
	}

	public class Sockets
	{
		// Socks proxy inspired by http://www.thecodeproject.com/csharp/ZaSocks5Proxy.asp
		public static SocksProxy SocksProxy;
		public static bool UseSocks = false;

		public static Socket CreateTCPSocket(String address, int port) { return CreateTCPSocket(address, port, UseSocks, SocksProxy); }
		public static Socket CreateTCPSocket(String address, int port, bool useSocks, SocksProxy proxy)
		{
			Socket sock;
			if (useSocks) sock = ConnectToSocksProxy(proxy.host, proxy.port, address, port, proxy.username, proxy.password);
			else
			{
				//IPAddress host = Dns.GetHostByName(address).AddressList[0];
				IPAddress host = Dns.GetHostEntry(address).AddressList[0];
				IPAddress newHost = IPAddress.Parse(address);
				sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sock.Connect(new IPEndPoint(newHost, port));
			}
			return sock;
		}

		private static string[] errorMsgs =	{
			"Operation completed successfully.",
			"General SOCKS server failure.",
			"Connection not allowed by ruleset.",
			"Network unreachable.",
			"Host unreachable.",
			"Connection refused.",
			"TTL expired.",
			"Command not supported.",
			"Address type not supported.",
			"Unknown error."
		};

		public static Socket ConnectToSocksProxy(IPAddress proxyIP, int proxyPort, String destAddress, int destPort, string userName, string password)
		{
			byte[] request = new byte[257];
			byte[] response = new byte[257];
			ushort nIndex;

			IPAddress destIP = null;

			try { destIP = IPAddress.Parse(destAddress); }
			catch { }

			IPEndPoint proxyEndPoint = new IPEndPoint(proxyIP, proxyPort);

			// open a TCP connection to SOCKS server...
			Socket s;
			s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			s.Connect(proxyEndPoint);
			/*			} catch(SocketException){
							throw new SocketException(0, "Could not connect to proxy server.");
						}*/

			nIndex = 0;
			request[nIndex++] = 0x05; // Version 5.
			request[nIndex++] = 0x02; // 2 Authentication methods are in packet...
			request[nIndex++] = 0x00; // NO AUTHENTICATION REQUIRED
			request[nIndex++] = 0x02; // USERNAME/PASSWORD
			// Send the authentication negotiation request...
			s.Send(request, nIndex, SocketFlags.None);

			// Receive 2 byte response...
			int nGot = s.Receive(response, 2, SocketFlags.None);
			if (nGot != 2)
				throw new ConnectionException("Bad response received from proxy server.");

			if (response[1] == 0xFF)
			{	// No authentication method was accepted close the socket.
				s.Close();
				throw new ConnectionException("None of the authentication method was accepted by proxy server.");
			}

			byte[] rawBytes;

			if (/*response[1]==0x02*/true)
			{//Username/Password Authentication protocol
				nIndex = 0;
				request[nIndex++] = 0x05; // Version 5.

				// add user name
				request[nIndex++] = (byte)userName.Length;
				rawBytes = Encoding.Default.GetBytes(userName);
				rawBytes.CopyTo(request, nIndex);
				nIndex += (ushort)rawBytes.Length;

				// add password
				request[nIndex++] = (byte)password.Length;
				rawBytes = Encoding.Default.GetBytes(password);
				rawBytes.CopyTo(request, nIndex);
				nIndex += (ushort)rawBytes.Length;

				// Send the Username/Password request
				s.Send(request, nIndex, SocketFlags.None);
				// Receive 2 byte response...
				nGot = s.Receive(response, 2, SocketFlags.None);
				if (nGot != 2)
					throw new ConnectionException("Bad response received from proxy server.");
				if (response[1] != 0x00)
					throw new ConnectionException("Bad Usernaem/Password.");
			}
			// This version only supports connect command. 
			// UDP and Bind are not supported.

			// Send connect request now...
			nIndex = 0;
			request[nIndex++] = 0x05;	// version 5.
			request[nIndex++] = 0x01;	// command = connect.
			request[nIndex++] = 0x00;	// Reserve = must be 0x00

			if (destIP != null)
			{// Destination adress in an IP.
				switch (destIP.AddressFamily)
				{
					case AddressFamily.InterNetwork:
						// Address is IPV4 format
						request[nIndex++] = 0x01;
						rawBytes = destIP.GetAddressBytes();
						rawBytes.CopyTo(request, nIndex);
						nIndex += (ushort)rawBytes.Length;
						break;
					case AddressFamily.InterNetworkV6:
						// Address is IPV6 format
						request[nIndex++] = 0x04;
						rawBytes = destIP.GetAddressBytes();
						rawBytes.CopyTo(request, nIndex);
						nIndex += (ushort)rawBytes.Length;
						break;
				}
			}
			else
			{// Dest. address is domain name.
				request[nIndex++] = 0x03;	// Address is full-qualified domain name.
				request[nIndex++] = Convert.ToByte(destAddress.Length); // length of address.
				rawBytes = Encoding.Default.GetBytes(destAddress);
				rawBytes.CopyTo(request, nIndex);
				nIndex += (ushort)rawBytes.Length;
			}

			// using big-edian byte order
			byte[] portBytes = BitConverter.GetBytes((ushort)destPort);
			for (int i = portBytes.Length - 1; i >= 0; i--)
				request[nIndex++] = portBytes[i];

			// send connect request.
			s.Send(request, nIndex, SocketFlags.None);
			s.Receive(response);	// Get variable length response...
			if (response[1] != 0x00)
				throw new ConnectionException(errorMsgs[response[1]]);
			// Success Connected...
			return s;
		}
	}

	public struct SocksProxy
	{
		public IPAddress host;
		public ushort port;
		public string username, password;

		public SocksProxy(String hostname, ushort port, String username, String password)
		{
			this.port = port;
			host = Dns.GetHostEntry(hostname).AddressList[0];
			this.username = username; this.password = password;
		}
	}

	public class ConnectionException : Exception
	{
		public ConnectionException(string message) : base(message) { }
	}

	// Server code cribbed from Framework Help
	public delegate bool ClientConnect(Server serv, ClientInfo new_client); // whether to accept the client


	public class Server
	{
		class ClientState
		{
			// To hold the state information about a client between transactions
			internal Socket Socket = null;
			internal const int BufferSize = 1024;
			internal byte[] buffer = new byte[BufferSize];
			internal StringBuilder sofar = new StringBuilder();

			internal ClientState(Socket sock)
			{
				Socket = sock;
			}
		}

		ArrayList clients = new ArrayList();
		Socket ss;

		public ClientConnect Connect;
		public IEnumerable Clients
		{
			get { return clients; }
		}

		
		public Socket ServerSocket
		{
			get { return ss; }
		}
		

		public ClientInfo this[int id]
		{
			get
			{
				foreach (ClientInfo ci in Clients)
					if (ci.ID == id) return ci;
				return null;
			}
		}

		public int Port
		{
			get { return ((IPEndPoint)ss.LocalEndPoint).Port; }
		}

		public Server(int port) : this(port, null) { }
		public Server(int port, ClientConnect connDel)
		{
			Connect = connDel;

			ss = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			ss.Bind(new IPEndPoint(IPAddress.Any, port));
			ss.Listen(100);

			// Start the accept process. When a connection is accepted, the callback
			// must do this again to accept another connection
			ss.BeginAccept(new AsyncCallback(AcceptCallback), ss);
		}

		internal void ClientClosed(ClientInfo ci)
		{
			clients.Remove(ci);
		}

		public void Broadcast(byte[] bytes)
		{
			foreach (ClientInfo ci in clients) ci.Socket.Send(bytes);
		}

		public void BroadcastMessage(uint code, byte[] bytes) { BroadcastMessage(code, bytes, 0); }
		public void BroadcastMessage(uint code, byte[] bytes, ParameterType paramType)
		{
			foreach (ClientInfo ci in clients) ci.SendMessage(code, bytes, paramType);
		}

		// ASYNC CALLBACK CODE
		void AcceptCallback(IAsyncResult ar)
		{
			try
			{
				Socket server = (Socket)ar.AsyncState;
				Socket cs = server.EndAccept(ar);

				// Start the thing listening again
				server.BeginAccept(new AsyncCallback(AcceptCallback), server);

				// Allow the new client to be rejected by the application
				ClientInfo c = new ClientInfo(cs, null, null, ClientDirection.Both);
				c.server = this;
				if (Connect != null)
				{
					if (!Connect(this, c))
					{
						// Rejected
						cs.Close();
						return;
					}
				}

				clients.Add(c);
			}
			catch (ObjectDisposedException) { }
			catch (SocketException) { }
		}

		~Server() { Close(); }
		public void Close()
		{
			ArrayList cl2 = new ArrayList();
			foreach (ClientInfo c in clients) cl2.Add(c);
			foreach (ClientInfo c in cl2) c.Close();

			ss.Close();
		}
	}



	public class ByteBuilder
	{
		byte[][] data;
		int packsize, used;

		public int Length
		{
			get
			{
				int len = 0;
				for (int i = 0; i < used; i++) len += data[i].Length;
				return len;
			}
		}

		public ByteBuilder() : this(10) { }
		public ByteBuilder(int packsize)
		{
			this.packsize = packsize; used = 0;
			data = new byte[packsize][];
		}

		public ByteBuilder(byte[] data)
		{
			packsize = 1;
			used = 1;
			this.data = new byte[][] { data };
		}

		public void Add(byte[] moredata) { Add(moredata, 0, moredata.Length); }
		public void Add(byte[] moredata, int from, int len)
		{
			//Console.WriteLine("Getting "+from+" to "+(from+len-1)+" of "+moredata.Length);
			if (used < packsize)
			{
				data[used] = new byte[len];
				for (int j = from; j < from + len; j++)
					data[used][j - from] = moredata[j];
				used++;
			}
			else
			{
				// Compress the existing items into the first array
				byte[] newdata = new byte[Length + len];
				int np = 0;
				for (int i = 0; i < used; i++)
					for (int j = 0; j < data[i].Length; j++)
						newdata[np++] = data[i][j];
				for (int j = from; j < from + len; j++)
					newdata[np++] = moredata[j];
				data[0] = newdata;
				for (int i = 1; i < used; i++) data[i] = null;
				used = 1;
			}
		}

		public byte[] Read(int from, int len)
		{
			if (len == 0) return new byte[0];
			byte[] res = new byte[len];
			int done = 0, start = 0;

			for (int i = 0; i < used; i++)
			{
				if ((start + data[i].Length) <= from)
				{
					start += data[i].Length; continue;
				}
				// Now we're in the data block
				for (int j = 0; j < data[i].Length; j++)
				{
					if ((j + start) < from) continue;
					res[done++] = data[i][j];
					if (done == len) return res;
				}
			}

			throw new ArgumentException("Datapoints " + from + " and " + (from + len) + " must be less than " + Length);
		}

		public void Clear()
		{
			used = 0;
			for (int i = 0; i < used; i++) data[i] = null;
		}

		public Parameter GetParameter(ref int index)
		{
			Parameter res = new Parameter();
			res.Type = (ParameterType)Read(index++, 1)[0];
			byte[] lenba = Read(index, 4);
			index += 4;
			int len = ClientInfo.GetInt(lenba, 0, 4);
			res.content = Read(index, len);
			index += len;
			return res;
		}

		public void AddParameter(Parameter param) { AddParameter(param.content, param.Type); }
		public void AddParameter(byte[] content, ParameterType Type)
		{
			Add(new byte[] { (byte)Type });
			Add(ClientInfo.IntToBytes(content.Length));
			Add(content);
		}

		public static String FormatParameter(Parameter p)
		{
			switch (p.Type)
			{
				case ParameterType.Int:
					int[] ia = ClientInfo.GetIntArray(p.content);
					StringBuilder sb = new StringBuilder();
					foreach (int i in ia) sb.Append(i + " ");
					return sb.ToString();
				case ParameterType.Uint:
					ia = ClientInfo.GetIntArray(p.content);
					sb = new StringBuilder();
					foreach (int i in ia) sb.Append(i.ToString("X8") + " ");
					return sb.ToString();
				case ParameterType.String:
					return Encoding.UTF8.GetString(p.content);
				case ParameterType.Byte:
					sb = new StringBuilder();
					foreach (int b in p.content) sb.Append(b.ToString("X2") + " ");
					return sb.ToString();
				default: return "??";
			}
		}
	}

	public struct Parameter
	{
		public ParameterType Type;
		public byte[] content;

		public Parameter(byte[] content, ParameterType type)
		{
			this.content = content; Type = type;
		}
	}
	/*
	public struct ParameterType {
		public const byte Unparameterised = 0;
	 public const byte Int = 1;
	 public const byte Uint = 2;
	 public const byte String = 3;
	 public const byte Byte = 4;
	}
	*/

	public enum ParameterType : byte
	{
		Unparameterised = 0,
		Int = 1,
		Uint = 2,
		String = 3,
		Byte = 4,
	}
}
