#region using statements

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using RedCorona.Net;
using BuckRogers;
using BuckRogers.Networking;
using System.Xml;
using System.Text.RegularExpressions;
using Azuria.Controls.ColorPicker;
using System.IO;
#endregion

namespace BuckRogers.Interface
{
	public partial class GameLobbyForm : Form
	{
		#region private members
		private BuckRogersClient m_gameClient;

		private Regex m_rePrivateMessage;
		private bool m_connected;

		private Hashtable m_otherClients;
		private Hashlist m_players;
		private List<ClientLobbyPanel> m_lobbyPanels;
		private Dictionary<Player, ClientLobbyPanel> m_playerPanels;

		#endregion

		#region properties

		public bool Connected
		{
			get { return m_connected; }
			set { m_connected = value; }
		}

		public BuckRogersClient GameClient
		{
			get { return m_gameClient; }
			set { m_gameClient = value; }
		}

		#endregion

		#region constructor

		public GameLobbyForm()
		{
			InitializeComponent();

			m_gameClient = new BuckRogersClient();
			m_gameClient.ClientStatusUpdate += new EventHandler<ClientUpdateEventArgs>(OnClientStatusUpdate);

			m_lobbyPanels = new List<ClientLobbyPanel>();
			m_otherClients = new Hashtable();
			m_players = new Hashlist();
			m_playerPanels = new Dictionary<Player, ClientLobbyPanel>();

			tabControl1.TabPages.Clear();
			AddPlayerPage();

			m_rePrivateMessage = new Regex(@"Private Message\r\nSender: (?<sender>.+)\r\nRecipient: (?<recipient>.+)\r\n(?<messageText>.+)");

			m_txtAddress.Text = GetIP();
			m_txtPort.Text = "4242";
		}

		#endregion

		#region utility

		public String GetIP()
		{
			String strHostName = Dns.GetHostName();

			// Find host by name
			IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

			// Grab the first IP addresses
			String IPStr = "";
			foreach (IPAddress ipaddress in iphostentry.AddressList)
			{
				if (ipaddress.AddressFamily != AddressFamily.InterNetwork)
				{
					continue;
				}
				IPStr = ipaddress.ToString();
				return IPStr;
			}

			return IPStr;
		}

		private void SetControlEnabled(Control c, bool enabled)
		{
			c.Invoke((MethodInvoker)delegate
			{
				c.Enabled = enabled;
			});
		}

		#endregion

		#region player pages

		private void AddPlayerPage()
		{
			TabPage tp1 = new TabPage();
			tabControl1.TabPages.Add(tp1);
			tp1.Text = "Player " + tabControl1.TabPages.Count;

			ClientLobbyPanel clp1 = new ClientLobbyPanel(tabControl1.TabPages.IndexOf(tp1));
			clp1.Location = new Point(0, 0);
			clp1.Parent = tp1;

			m_lobbyPanels.Add(clp1);

			if (m_connected)
			{
				//clp1.Connection = m_connection;
				clp1.Client = m_gameClient;
			}

			clp1.ClientConnectionStatusChanged(m_connected);

			m_btnRemovePlayer.Enabled = true;
		}

		private void RemovePlayerPage()
		{
			if (m_lobbyPanels.Count <= 1)
			{
				return;
			}

			ClientLobbyPanel clp = m_lobbyPanels[m_lobbyPanels.Count - 1];
			if (clp.PlayerConnected)
			{
				clp.Client = null;

				m_gameClient.SendMessageToServer(GameMessage.PlayerRemoved, clp.Player.Name);
			}

			tabControl1.TabPages.RemoveAt(tabControl1.TabPages.Count - 1);

			m_lobbyPanels.Remove(clp);

			if (tabControl1.TabPages.Count == 1)
			{
				m_btnRemovePlayer.Enabled = false;
			}
		}

		#endregion

		#region connect / disconnect

		private void m_btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				int port = int.Parse(m_txtPort.Text);

				m_gameClient.ConnectToServer(m_txtAddress.Text, port);

				m_connected = true;
				m_btnConnect.Enabled = false;
				m_btnDisconnect.Enabled = true;

			}
			catch (SocketException se)
			{
				MessageBox.Show("Connection failed: " + se.Message);
			}
		}

		private void OnConnectionClosed(ClientInfo client)
		{
			Disconnect(client);
		}

		private void Disconnect(ClientInfo client)
		{

			m_lbMessages.Invoke((MethodInvoker)delegate
			{
				m_lbMessages.Items.Clear();
			});

			SetControlEnabled(m_btnDisconnect, false);
			SetControlEnabled(m_btnConnect, true);

			m_lblConnectionStatus.Invoke((MethodInvoker)delegate
			{
				m_lblConnectionStatus.Text = "No";
			});

			foreach (ClientLobbyPanel clp in m_lobbyPanels)
			{
				clp.Invoke((MethodInvoker)delegate
				{
					clp.Client = m_gameClient;
					clp.ClientConnectionStatusChanged(false);
				});
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			m_gameClient.ClientStatusUpdate -= new EventHandler<ClientUpdateEventArgs>(OnClientStatusUpdate);
		}

		#endregion

		#region message handling stuff


		void OnClientStatusUpdate(object sender, ClientUpdateEventArgs e)
		{
			switch (e.MessageType)
			{
				case GameMessage.ConnectionAcknowledged:
					{
						m_lblConnectionStatus.Invoke((MethodInvoker)delegate
						{
							m_lblConnectionStatus.Text = "Yes";
						});

						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								//clp.Connection = m_connection;
								clp.Client = m_gameClient;
								clp.ClientConnectionStatusChanged(true);
							});

						}
						break;
					}
				case GameMessage.PublicChatMessage:
					{
						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								clp.AddMessage(e.MessageText);
							});

						}
						break;
					}
				case GameMessage.PrivateChatMessage:
					{
						Player p = e.Players[0];
						ClientLobbyPanel clp = m_playerPanels[p];

						clp.Invoke((MethodInvoker)delegate
						{
							clp.AddMessage(e.MessageText);
						});
						break;
					}
				case GameMessage.OtherClientsList:
					{
						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								clp.RefreshClientsList(e.MessageText);
							});

						}
						break;
					}
				case GameMessage.PlayerAdded:
					{
						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								clp.AddMessage(e.MessageText);
							});
						}
						break;
					}
				case GameMessage.PlayerRemoved:
					{
						string fullMessage = "Player left: " + e.MessageText;
						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								clp.AddMessage(fullMessage);
							});
						}

						if (m_players.ContainsKey(e.MessageText))
						{
							m_players.Remove(e.MessageText);
						}

						break;
					}
				case GameMessage.PlayerDenied:
					{
						MessageBox.Show(e.MessageText);
						break;
					}
				case GameMessage.PlayerAccepted:
					{
						ClientLobbyPanel acceptedCLP = null;
						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							if (clp.Player.Name == e.MessageText)
							{
								acceptedCLP = clp;
								break;
							}
						}

						if (acceptedCLP != null)
						{
							m_playerPanels[acceptedCLP.Player] = acceptedCLP;
							//m_playerPanels.Add(acceptedCLP.Player, acceptedCLP);
							m_players.Add(acceptedCLP.Player.Name, acceptedCLP.Player);

							acceptedCLP.Invoke((MethodInvoker)delegate
							{
								acceptedCLP.PlayerConnectionStatusChanged(true);
							});
						}
						break;
					}
				case GameMessage.GameSettings:
					{
						m_txtGameSettings.Invoke((MethodInvoker)delegate
						{
							m_txtGameSettings.Text = e.MessageText;
						});
						break;
					}
				case GameMessage.StatusUpdate:
					{
						if (e.MessageText != string.Empty)
						{
							m_lbMessages.Invoke((MethodInvoker)delegate
							{
								m_lbMessages.Items.Add(e.MessageText);
							});
						}
						break;
					}
				case GameMessage.ServerDisconnected:
					{
						m_lbMessages.Invoke((MethodInvoker)delegate
						{
							m_lbMessages.Items.Clear();
						});

						SetControlEnabled(m_btnDisconnect, false);
						SetControlEnabled(m_btnConnect, true);

						m_lblConnectionStatus.Invoke((MethodInvoker)delegate
						{
							m_lblConnectionStatus.Text = "No";
						});

						foreach (ClientLobbyPanel clp in m_lobbyPanels)
						{
							clp.Invoke((MethodInvoker)delegate
							{
								//clp.Connection = m_connection;
								clp.Client = m_gameClient;
								clp.ClientConnectionStatusChanged(false);
							});
						}

						m_players.Clear();
						break;
					}
				case GameMessage.GameStarted:
					{
						//MessageBox.Show("Game starting!");

						this.Close();
						break;
					}

			}
		}

		#endregion

		#region info sending functions

		private void SendPlayersList()
		{
			MemoryStream stream = new MemoryStream();
			XmlWriter xw = XmlWriter.Create(stream);

			xw.WriteStartDocument();
			xw.WriteStartElement("Players");

			foreach (Player p in m_players)
			{
				xw.WriteStartElement("Player");
				xw.WriteAttributeString("name", p.Name);
				xw.WriteAttributeString("color", p.Color.Name);
				xw.WriteEndElement();
			}

			xw.WriteEndElement();
			xw.WriteEndDocument();
			xw.Flush();

			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);
			string xml = sr.ReadToEnd();

			m_gameClient.SendMessageToServer(GameMessage.PlayerListing, xml);
		}

		#endregion

		#region button handlers

		private void m_btnDisconnect_Click(object sender, EventArgs e)
		{
			if (m_gameClient.Connection != null)
			{
				m_gameClient.Connection.Close();
			}
		}

		private void m_btnAddPlayer_Click(object sender, EventArgs e)
		{
			AddPlayerPage();
		}

		private void m_btnRemovePlayer_Click(object sender, EventArgs e)
		{
			RemovePlayerPage();
		}

		#endregion

	}
}
