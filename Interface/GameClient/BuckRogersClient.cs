#region using statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

using RedCorona.Net;
using Lambda.Collections.Generic;
using CommonGenius.Collections;

using BuckRogers;
using BuckRogers.Networking;
using ISquared.Debugging;
#endregion

namespace BuckRogers.Networking
{
	public class BuckRogersClient
	{
		#region private members
		public event EventHandler<ClientUpdateEventArgs> ClientStatusUpdate = delegate { };
		public event EventHandler<ClientUpdateEventArgs> GameMessageReceived = delegate { };

		private ClientInfo m_connection;

		private GameOptions m_options;

		private Regex m_rePrivateMessage;

		private bool m_connected;
		private bool m_gameStarted;

		private int m_clientID;
		
		private Hashtable m_otherClients;
		private Hashlist m_players;
		private Set<int> m_acknowledgedMessages;
		private OrderedDictionary<int, NetworkMessageInfo> m_sentMessages;

		#endregion

		#region properties

		public ClientInfo Connection
		{
			get { return m_connection; }
			set { m_connection = value; }
		}

		public bool Connected
		{
			get { return m_connected; }
			set { m_connected = value; }
		}

		public bool GameStarted
		{
			get { return m_gameStarted; }
			set { m_gameStarted = value; }
		}

		public GameOptions GameOptions
		{
			get { return m_options; }
			set { m_options = value; }
		}

		public Hashlist Players
		{
			get { return m_players; }
			set { m_players = value; }
		}

		#endregion

		#region constructor

		public BuckRogersClient()
		{
			m_otherClients = new Hashtable();
			m_players = new Hashlist();
			m_acknowledgedMessages = new Set<int>();
			m_sentMessages = new OrderedDictionary<int, NetworkMessageInfo>();

			m_options = new GameOptions();
			m_options.IsNetworkGame = true;

			m_clientID = 0;

			m_rePrivateMessage = new Regex(@"Private Message\r\nSender: (?<sender>.+)\r\nRecipient: (?<recipient>.+)\r\n(?<messageText>.+)");

			TextTraceListener.Prefix = "Buck Rogers Client";
			TextTraceListener.FlushType = TextTraceListener.LogFlushType.AutoClose;

			TextTraceListener.InstallListener();
		}

		#endregion

		#region connect / disconnect
		public void ConnectToServer(string address, int port)
		{
			Socket sock = Sockets.CreateTCPSocket(address, port);

			m_connection = new ClientInfo(sock, false);
			m_connection.MessageType = MessageType.CodeAndLength;
			m_connection.OnReadMessage += new ConnectionReadMessage(OnReadMessage);
			m_connection.OnClose += new ConnectionClosed(OnConnectionClosed);

			m_connection.BeginReceive();

			m_connected = true;
		}

		private void OnConnectionClosed(ClientInfo client)
		{
			Disconnect(client);
		}

		private void Disconnect(ClientInfo client)
		{
			m_connected = false;

			m_connection = null;
			m_players.Clear();

			RaiseSimpleUpdateEvent(string.Empty, GameMessage.ServerDisconnected, null);
		}

		#endregion

		#region message handling stuff

		private void OnReadMessage(ClientInfo ci, uint code, byte[] bytes, int len)
		{
			// 1 byte for parameter type, 4 bytes for length

			string message = Encoding.UTF8.GetString(bytes);
			string logMessage = string.Empty;

			int messageID = Utility.High24Bits((int)code);
			int messageCode = Utility.Low8Bits((int)code);
			GameMessage netMessage = (GameMessage)messageCode;

			LogNetworkMessage(netMessage, messageID, message);

			if(netMessage == GameMessage.MessageAcknowledged)
			{
				m_acknowledgedMessages.Add(messageID);
				return;
			}
			else
			{
				AcknowledgeMessage(messageID, netMessage);
			}

			if(messageCode >= (int)GameMessage.GameplayMessagesFirst)
			{
				ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
				cuea.MessageType = netMessage;
				cuea.MessageText = message;

				EventsHelper.Fire(GameMessageReceived, this, cuea);
				return;
			}

			switch (netMessage)
			{
				case GameMessage.ConnectionAcknowledged:
				{
					logMessage = "Connection acknowledged.";

					OnConnectionAcknowledged(message);
					break;
				}
				case GameMessage.PublicChatMessage:
				{
					logMessage = string.Format("Received public chat message : {0}", message);

					RaiseSimpleUpdateEvent(message, netMessage, null);
					break;
				}
				case GameMessage.PrivateChatMessage:
				{
					string sender = m_rePrivateMessage.Match(message).Groups["sender"].Value;
					string recipient = m_rePrivateMessage.Match(message).Groups["recipient"].Value;
					string privateMessage = m_rePrivateMessage.Match(message).Groups["messageText"].Value;

					logMessage = string.Format("Received private message from {0} to {1}.  Message: {2}",
													sender, recipient, privateMessage);

					Player p = (Player)m_players[recipient];
					string formattedMessage = sender + ": " + privateMessage;
					RaiseSimpleUpdateEvent(formattedMessage, netMessage, p);
					break;
				}
				case GameMessage.PlayerNameRequested:
				{
					logMessage = "Received PlayerNameRequested.  Isn't this obsolete now?";
					//SendPlayerName();
					break;
				}
				case GameMessage.OtherClientsList:
				{
					logMessage = "Received OtherClientsList";

					RaiseSimpleUpdateEvent(message, netMessage, null);
					break;
				}
				case GameMessage.PlayerColorUpdated:
				case GameMessage.PlayerNameUpdated:
				case GameMessage.OtherClientConnected:
				case GameMessage.OtherClientDisconnected:
				case GameMessage.LoginCompleted:
				{
					string messageName = Enum.GetName(typeof(GameMessage), code);
					logMessage = string.Format("Received message ({0}) causing client list request", messageName);
					//m_connection.SendMessage((uint)GameMessage.ClientListRequested, new byte[0]);
					SendMessageToServer(GameMessage.ClientListRequested, string.Empty);
					break;
				}
				case GameMessage.PlayerAdded:
				{
					logMessage = string.Format("Server reports Player Added: {0}", message);


					string fullMessage = "Player joined: " + message;

					RaiseSimpleUpdateEvent(fullMessage, netMessage, null);

					if(!m_players.ContainsKey(message))
					{
						m_connection.SendMessage((uint)GameMessage.ClientListRequested, new byte[0]);
					}
					
					break;
				}
				case GameMessage.PlayerRemoved:
				{
					logMessage = string.Format("Server reports Player Removed: {0}", message);
					//string fullMessage = "Player left: " + message;

					RaiseSimpleUpdateEvent(message, netMessage, null);

					if(m_players.ContainsKey(message))
					{
						m_players.Remove(message);
					}

					m_connection.SendMessage((uint)GameMessage.ClientListRequested, new byte[0]);
					break;
				}
				case GameMessage.PlayerDenied:
				{
					logMessage = string.Format("Player was denied: {0}", message);

					RaiseSimpleUpdateEvent(message, netMessage, null);
					break;
				}
				case GameMessage.PlayerAccepted:
				{
					logMessage = string.Format("Player was accepted: {0}", message);

					RaiseSimpleUpdateEvent(message, netMessage, null);

					Player p = new Player(message);

					m_players.Add(message, p);
					m_connection.SendMessage((uint)GameMessage.ClientListRequested, new byte[0]);
					break;
				}
				case GameMessage.GameSettings:
				{
					logMessage = "Game settings received";
					ParseGameOptionsMessage(message);

					StringBuilder sb = new StringBuilder();

					//string a = "Number of players: 6\r\nVictory Condition: Total Annihilation\r\n\r\nGame Options:\r\nOnly Ground Troops Conquer\r\nAll Territories Owned\r\nControl Markers Fight";
					sb.AppendFormat("Number of players: {0}\r\n", m_options.NumPlayers);
					sb.AppendFormat("\r\nVictory Condition:\r\n{0}\r\n", m_options.WinningConditions.ToString());
					sb.AppendFormat("\r\nGame Options:\r\n");

					foreach (GameOption option in m_options.OptionalRules)
					{
						if (option.Value)
						{
							sb.AppendFormat("{0}\r\n", option.Name);
						}
					}

					RaiseSimpleUpdateEvent(sb.ToString(), GameMessage.GameSettings, null);
					break;
				}
				case GameMessage.GameStarted:
				{
					m_gameStarted = true;

					ParseGameInformation(message);

					RaiseSimpleUpdateEvent(string.Empty, netMessage, null);
					break;
				}
			}

			if(logMessage != string.Empty)
			{
				RaiseSimpleUpdateEvent(logMessage, GameMessage.StatusUpdate, null);
			}
			
		}

		

		private void LogNetworkMessage(GameMessage netMessage, int messageID, string message)
		{
			string logMessage = string.Format("Client ID: {0}, GameMessage: {1}, Message ID: {2}, Message text: {3}", 
												m_clientID, netMessage.ToString(), messageID, message);
			Debug.WriteLine(logMessage);
		}

		private void OnConnectionAcknowledged(string message)
		{
			int index = message.IndexOf("#");
			string idString = message.Substring(index + 1);
			m_clientID = int.Parse(idString);

			
			TextTraceListener.Prefix = "Buck Rogers Client "  + idString;
			RaiseSimpleUpdateEvent(string.Empty, GameMessage.ConnectionAcknowledged, null);

			//m_connection.SendMessage((uint)GameMessage.ClientListRequested, new byte[0]);
			//m_connection.SendMessage((uint)GameMessage.GameSettings, new byte[0]);
			SendMessageToServer(GameMessage.ClientListRequested, string.Empty);
			SendMessageToServer(GameMessage.GameSettings, string.Empty);
		}

		private void ParseGameInformation(string message)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(message);

			XmlElement xePlayers = (XmlElement)xd.GetElementsByTagName("Players")[0];
			XmlNodeList xnlPlayers = xePlayers.GetElementsByTagName("Player");

			List<Player> tempPlayers = new List<Player>();

			foreach(XmlElement xePlayer in xnlPlayers)
			{
				string playerName = xePlayer.Attributes["name"].Value;
				string colorName = xePlayer.Attributes["color"].Value;
				int id = int.Parse(xePlayer.Attributes["id"].Value);

				Player p;
				if(m_players.ContainsKey(playerName))
				{
					p = (Player)m_players[playerName];
					p.Location = PlayerLocation.Local;					
				}
				else
				{
					p = new Player(playerName);					
					p.Location = PlayerLocation.Remote;					
				}

				p.Color = Color.FromName(colorName);
				p.ID = id;
				tempPlayers.Add(p);
			}

			m_players.Clear();

			foreach(Player p in tempPlayers)
			{
				m_players.Add(p.Name, p);				
			}
		}

		public void ParseGameOptionsMessage(string message)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(message);

			XmlElement xeGameOptions = (XmlElement)xd.GetElementsByTagName("GameOptions")[0];

			string victoryString = xeGameOptions.Attributes["victoryCondition"].Value;
			string numPlayersString = xeGameOptions.Attributes["numberOfPlayers"].Value;

			VictoryConditions vc = (VictoryConditions)Enum.Parse(typeof(VictoryConditions), victoryString);
			int numPlayers = int.Parse(numPlayersString);

			m_options.NumPlayers = numPlayers;
			m_options.WinningConditions = vc;

			XmlNodeList xnlOptions = xeGameOptions.GetElementsByTagName("OptionalRule");

			foreach (XmlElement xeOption in xnlOptions)
			{
				string optionName = xeOption.Attributes["name"].Value;
				//string optionDescription = xeOption.Attributes["description"].Value;

				GameOption currentOption = (GameOption)m_options.OptionalRules.Get(optionName);
				//m_options.OptionalRules.Add(optionName, currentOption);

				// only receiving enabled options anyway
				currentOption.Value = true;

				XmlNodeList xnlValues = xeOption.GetElementsByTagName("Value");

				foreach (XmlElement xeValue in xnlValues)
				{
					string valueName = xeValue.Attributes["name"].Value;
					//string valueDescription = xeValue.Attributes["description"].Value;

					string sMin = xeValue.Attributes["min"].Value;
					string sMax = xeValue.Attributes["max"].Value;
					string sStart = xeValue.Attributes["start"].Value;
					string sValue = xeValue.Attributes["value"].Value;

					//OptionValue ov = new OptionValue();
					OptionValue ov = (OptionValue)currentOption.Values.Get(valueName);
					ov.Name = valueName;
					//ov.Description = valueDescription;

					ov.Min = int.Parse(sMin);
					ov.Max = int.Parse(sMax);
					ov.Start = int.Parse(sStart);
					ov.Value = int.Parse(sValue);

					//currentOption.Values.Add(valueName, ov);
				}
			}

			
		}

		

		#endregion

		#region events and message sending

		private void RaiseSimpleUpdateEvent(string text, GameMessage message, Player p)
		{
			ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
			cuea.MessageText = text;
			cuea.MessageType = message;

			if (p != null)
			{
				cuea.Players.Add(p);
			}

			EventsHelper.Fire(ClientStatusUpdate, this, cuea);
		}

		public void SendMessageToServer(GameMessage messageType, string messageText)
		{
			byte[] messageBytes = Encoding.UTF8.GetBytes(messageText);

			int encodedMessage = Utility.MakeReallyLong((int)messageType, 0);

			m_connection.SendMessage((uint)encodedMessage, messageBytes);
		}

		private void AcknowledgeMessage( int messageID, GameMessage netMessage )
		{
			int encodedMessage = Utility.MakeReallyLong((int)GameMessage.MessageAcknowledged, messageID);

			string message = string.Format("{0} (message ID: {1})", netMessage.ToString(), messageID);
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);
			m_connection.SendMessage((uint)encodedMessage, messageBytes);
		}

		#endregion

	}
}