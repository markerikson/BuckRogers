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
	public partial class ClientLobbyPanel : UserControl
	{
		#region private members

		private string[] m_nameList = {"Mark", "Chris", "Hannah", "Stu", "Kathryn", "Jake",
										"Kevin", "Joel", "KC", "Adam", "Emanuel", "Fred"};

		//public event EventHandler<PlayerStatusEventArgs> PlayerConnectStatusChanged = delegate { };
		private GameLobbyForm m_lobbyForm;
		//private ClientInfo m_connection;
		private BuckRogersClient m_client;

		
		private Player m_player;

		#endregion

		#region properties

		public bool PlayerConnected
		{
			get
			{
				return m_chkPlayerConnected.Checked;
			}
		}

		public Player Player
		{
			get { return m_player; }
			set { m_player = value; }
		}

		public BuckRogersClient Client
		{
			get { return m_client; }
			set { m_client = value; }
		}

		public GameLobbyForm ClientForm
		{
			get { return m_lobbyForm; }
			set { m_lobbyForm = value; }
		}

		#endregion

		#region constructor

		public ClientLobbyPanel(int tabIndex)
		{
			InitializeComponent();

			m_player = new Player(string.Empty);

			Random r = new Random();
			int index = r.Next(m_nameList.Length);
			m_txtName.Text = m_nameList[index];

			CreateColorSelector();

			m_cbColorPicker.SelectedIndex = tabIndex;
		}

		#endregion

		#region connnection status stuff

		public void ClientConnectionStatusChanged(bool connected)
		{
			if(connected)
			{
				m_chkPlayerConnected.Enabled = true;
			}
			else
			{
//				m_lbMessages.Items.Clear();
				m_txtMessagesDisplay.Text = string.Empty;

				m_chkPlayerConnected.Checked = false;
				m_chkPlayerConnected.Enabled = false;

				m_btnSendAll.Enabled = false;
				m_btnSendPlayer.Enabled = false;

				m_txtName.Enabled = true;

				m_lbOtherClients.Items.Clear();
			}
		}

		public void PlayerConnectionStatusChanged(bool connected)
		{
			m_btnSendPlayer.Enabled = connected;
			m_btnSendAll.Enabled = connected;

			if(connected)
			{
				
			}
			else
			{
				//m_btnSendClient.Enabled = false;
				//m_btnSendAll.Enabled = false;
			}
		}

		#endregion

		/*
		#region info sending stuff
				
		private void SendPlayerInfo(Player p)
		{
			string message = p.Name + "|" + p.Color.Name;
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);

			m_connection.SendMessage((uint)NetworkMessages.PlayerAdded, messageBytes);
		}

		#endregion
		*/

		#region utility

		private void CreateColorSelector()
		{
			CustomColorCollection ccc = new CustomColorCollection();
			int firstNamedColorIndex = 28;
			int lastNamedColorIndex = 166;
			Color[] defaultPlayerColors = {Color.CornflowerBlue, Color.Yellow, Color.Teal,  
											Color.Violet, Color.Tan, Color.MediumVioletRed};

			foreach (Color defaultColor in defaultPlayerColors)
			{
				ccc.Add(defaultColor);
			}

			KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));

			for (int i = firstNamedColorIndex; i <= lastNamedColorIndex; i++)
			{
				KnownColor kc = colors[i];
				Color c = Color.FromKnownColor(kc);

				if (Array.IndexOf(defaultPlayerColors, c) != -1
					|| c == Color.Transparent)
				{
					continue;
				}

				ccc.Add(c);
			}

			m_cbColorPicker.Items = ccc;

			m_cbColorPicker.SelectedIndex = 0;

		}

		#endregion

		#region external event handlers

		public void AddMessage(string message)
		{
			//m_lbMessages.Items.Add(message);
			m_txtMessagesDisplay.AppendText(message);
			m_txtMessagesDisplay.AppendText("\n");
		}

		public void RefreshClientsList(string message)
		{
			m_lbOtherClients.Invoke((MethodInvoker)delegate
			{
				m_lbOtherClients.Items.Clear();
			});

			XmlDocument xd = new XmlDocument();
			xd.LoadXml(message);

			XmlNodeList xnlClients = xd.GetElementsByTagName("Player");

			foreach (XmlElement xeClient in xnlClients)
			{
				string playerName = xeClient.Attributes["name"].Value;

				if(playerName == m_player.Name)
				{
					continue;
				}

				string colorName = xeClient.Attributes["color"].Value;
				Color playerColor = Color.FromName(colorName);
				Player p = new Player(playerName, playerColor);
				p.ID = int.Parse(xeClient.Attributes["id"].Value);

				m_lbOtherClients.Invoke((MethodInvoker)delegate
				{
					m_lbOtherClients.Items.Add(p);
				});

			}
		}

		#endregion

		#region local event handlers

		private void m_cbColorPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_client == null || m_client.Connection.Closed || !m_chkPlayerConnected.Checked)
			{
				return;
			}

			string colorName = "White";

			m_cbColorPicker.Invoke((MethodInvoker)delegate
			{
				colorName = m_cbColorPicker.SelectedText;
			});

			string message = m_player.Name + "|" + colorName;
			m_client.SendMessageToServer(GameMessage.PlayerColorUpdated, message);

			/*
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);
			m_connection.SendMessage((uint)NetworkMessages.PlayerColorUpdated, messageBytes);
			*/
		}

		private void OnPlayerConnectedClicked(object sender, EventArgs e)
		{
			bool connecting = m_chkPlayerConnected.Checked;

			if (connecting)
			{
				m_player = new Player(m_txtName.Text);
				string colorName = m_cbColorPicker.SelectedText;
				m_player.Color = Color.FromName(colorName);

				m_txtName.Enabled = false;
				//SendPlayerInfo(m_player);
				string message = m_player.Name + "|" + m_player.Color.Name;
				m_client.SendMessageToServer(GameMessage.PlayerAdded, message);
				
			}
			else
			{
				try
				{
					//byte[] bytes = Encoding.UTF8.GetBytes(m_player.Name);
					//m_connection.SendMessage((uint)NetworkMessages.PlayerRemoved, bytes);
					m_client.SendMessageToServer(GameMessage.PlayerRemoved, m_player.Name);
				}
				catch (NullReferenceException)
				{
					// might get a null exception here if we're disconnecting?
				}

			}


			/*
			PlayerStatusEventArgs psea = new PlayerStatusEventArgs();
			psea.Connecting = connecting;

			EventsHelper.Fire(PlayerConnectStatusChanged, this, psea);
			*/
		}

		private void m_btnSendPlayer_Click(object sender, EventArgs e)
		{
			if (m_txtMessage.Text.Length == 0
				|| m_lbOtherClients.SelectedItem == null)
			{
				return;
			}

			Player selectedPlayer = (Player)m_lbOtherClients.SelectedItem;
			string clientName = selectedPlayer.Name;

			string message = m_txtMessage.Text;
			string fullMessage = string.Format("Private Message\r\nSender: {0}\r\nRecipient: {1}\r\n{2}",
												m_player.Name, clientName, message);

			m_client.SendMessageToServer(GameMessage.PrivateChatMessage, fullMessage);

			/*
			byte[] messageBytes = Encoding.UTF8.GetBytes(fullMessage);


			m_connection.SendMessage((uint)NetworkMessages.PrivateChatMessage, messageBytes);
			*/

			m_txtMessage.Text = string.Empty;
		}

		private void m_btnSendAll_Click(object sender, EventArgs e)
		{
			if(m_txtMessage.Text == string.Empty)
			{
				return;
			}

			string message = m_txtMessage.Text;
			string fullMessage = string.Format("Public Message\r\nSender: {0}\r\n{1}",
												m_player.Name, message);
			m_client.SendMessageToServer(GameMessage.PublicChatMessage, fullMessage);

			/*
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fullMessage);

			m_connection.SendMessage((uint)NetworkMessages.PublicChatMessage, bytes);
			*/
			m_txtMessage.Text = string.Empty;
		}

		#endregion

	}
}
