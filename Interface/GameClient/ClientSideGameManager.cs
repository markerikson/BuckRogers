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

using RedCorona.Net;
using BuckRogers;
using BuckRogers.Networking;
using CommandManagement;
#endregion

namespace BuckRogers.Networking
{
	class ClientSideGameManager
	{
		#region private members
		public event EventHandler<ClientUpdateEventArgs> ClientUpdateMessage = delegate { };

		private GameController m_controller;
		private BattleController m_battleController;
		private BuckRogersClient m_gameClient;
		private GameOptions m_options;
		private bool m_isNetworkGame;

		private static bool m_isActiveClient;

		private static CommandManager m_commandManager = new CommandManager();

		#endregion

		#region properties

		public static CommandManager CommandManager
		{
			get { return m_commandManager; }
			set { m_commandManager = value; }
		}

		public static bool IsActiveClient
		{
			get { return m_isActiveClient; }
			set { m_isActiveClient = value; }
		}

		public GameController GameController
		{
			get { return m_controller; }
			set { m_controller = value; }
		}

		public BattleController BattleController
		{
			get { return m_battleController; }
			set { m_battleController = value; }
		}

		#endregion

		#region constructor

		public ClientSideGameManager(BuckRogersClient client, GameController controller, BattleController battleController)
		{
			m_gameClient = client;
			m_controller = controller;
			m_battleController = battleController;
			m_options = GameController.Options;

			m_isNetworkGame = m_options.IsNetworkGame;

			
			if(m_gameClient != null)
			{
				m_gameClient.GameMessageReceived += new EventHandler<ClientUpdateEventArgs>(OnGameMessageReceived);
			}
			

		}

		#endregion

		#region OnGameMessageReceived

		void OnGameMessageReceived(object sender, ClientUpdateEventArgs e)
		{
			switch (e.MessageType)
			{
				case GameMessage.InitialSetupInformation:
				{
					ParseInitialSetupInformation(e.MessageText);

					ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
					cuea.MessageType = GameMessage.InitialSetupInformation;

					// synchronous, to make sure things complete before we report ready
					EventsHelper.Fire(ClientUpdateMessage, this, cuea);

					m_gameClient.SendMessageToServer(GameMessage.ClientReady, string.Empty);
					break;
				}
				case GameMessage.PlayerPlacementStarted:
				{
					CheckIfActiveClient();
					RaiseSimpleUpdateEvent(GameMessage.PlayerPlacementStarted, string.Empty, null);
					break;
				}
				case GameMessage.PlayerChoseUnits:
				{
					string[] messageParts = e.MessageText.Split('|');
					Player p = m_controller.GetPlayer(messageParts[0]);

					/*
					if(p.Location == PlayerLocation.Local)
					{
						break;
					}
					*/

					int[] numUnits = new int[4];
					UnitType[] unitTypes = new UnitType[] { UnitType.Trooper, UnitType.Fighter, 
															UnitType.Gennie, UnitType.Transport };

					for (int i = 0; i <= 3; i++)
					{
						numUnits[i] = int.Parse(messageParts[i + 1]);
					}

					for (int i = 0; i < unitTypes.Length; i++)
					{
						m_controller.CreateUnits(p, unitTypes[i], numUnits[i]);
					}

					RaiseSimpleUpdateEvent(GameMessage.PlayerChoseUnits, string.Empty, p);
					break;
				}
				case GameMessage.PlayerPlacedUnits:
				{
					string[] messageInfo = e.MessageText.Split('|');
					Player p = m_controller.GetPlayer(messageInfo[0]);

					if(p.Location == PlayerLocation.Local)
					{
						break;
					}

					Territory t = m_controller.Map[messageInfo[1]];

					for (int i = 2; i < messageInfo.Length; i++)
					{
						string[] unitCountInfo = messageInfo[i].Split(':');
						UnitType ut = (UnitType)Enum.Parse( typeof(UnitType), unitCountInfo[0]);
						int numUnits = int.Parse(unitCountInfo[1]);

						UnitCollection unitsToPlace = p.Units.GetUnits(ut, p, Territory.NONE, numUnits);

						m_controller.PlaceUnits(unitsToPlace, t);
					}

					break;
				}
				case GameMessage.NextPlayer:
				{
					if(m_controller.NextPlayer())
					{
						CheckIfActiveClient();
						RaiseSimpleUpdateEvent(GameMessage.NextPlayer, string.Empty, null);
					}					
					
					break;
				}
			}
		}

		#endregion

		#region private utility

		private void CheckIfActiveClient()
		{
			m_isActiveClient = (m_controller.CurrentPlayer.Location == PlayerLocation.Local);
		}

		private void RaiseSimpleUpdateEvent(GameMessage message, string text, Player p)
		{
			ClientUpdateEventArgs cuea = CreateClientUpdateEvent(text, message, p);

			EventsHelper.Fire(ClientUpdateMessage, this, cuea);
		}

		private void RaiseLocalLoopbackEvent(GameMessage message, string text, Player p)
		{
			ClientUpdateEventArgs cuea = CreateClientUpdateEvent(text, message, p);

			OnGameMessageReceived(this, cuea);
		}

		private ClientUpdateEventArgs CreateClientUpdateEvent(string text, GameMessage message, Player p)
		{
			ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
			cuea.MessageText = text;
			cuea.MessageType = message;

			if (p != null)
			{
				cuea.Players.Add(p);
			}

			return cuea;
		}

		#endregion

		#region message parsing

		private void ParseInitialSetupInformation(string messageText)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(messageText);

			ArrayList initialPlayerOrder = new ArrayList();

			XmlElement xePlayers = (XmlElement)xd.GetElementsByTagName("Players")[0];
			XmlNodeList xnlPlayers = xePlayers.GetElementsByTagName("Player");

			foreach (XmlElement xePlayer in xnlPlayers)
			{
				string playerName = xePlayer.Attributes["name"].Value;

				Player p = m_controller.GetPlayer(playerName);

				XmlElement xeTerritories = (XmlElement)xePlayer.GetElementsByTagName("Territories")[0];
				XmlNodeList xnlTerritories = xePlayer.GetElementsByTagName("Territory");

				List<Territory> territories = new List<Territory>();

				foreach(XmlElement xeTerritory in xnlTerritories)
				{
					string territoryName = xeTerritory.Attributes["name"].Value;

					Territory t = m_controller.Map[territoryName];
					territories.Add(t);
				}

				m_controller.SetTerritoriesOwner(p, territories);

				XmlElement xeUnits = (XmlElement)xePlayer.GetElementsByTagName("Units")[0];
				XmlNodeList xnlUnits = xePlayer.GetElementsByTagName("Unit");

				foreach (XmlElement xeUnit in xnlUnits)
				{
					int id = int.Parse(xeUnit.Attributes["id"].Value);
					UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), xeUnit.Attributes["type"].Value);

					string territoryName = xeUnit.Attributes["territory"].Value;

					Unit u = Unit.CreateNewUnit(p, ut);

					if(territoryName != "None")
					{
						Territory t = m_controller.Map[territoryName];
						u.CurrentTerritory = t;
					}

				}
			}

			XmlElement xePlayerOrder = (XmlElement)xd.GetElementsByTagName("InitialOrder")[0];
			string playerOrderString = xePlayerOrder.Attributes["order"].Value;
			string[] playerOrderNames = playerOrderString.Split('|');

			foreach(string playerOrderName in playerOrderNames)
			{
				Player p = m_controller.GetPlayer(playerOrderName);
				initialPlayerOrder.Add(p);
			}
			
			m_controller.PlayerOrder = initialPlayerOrder;
		}

		#endregion

		#region external API

		public void ReadyToStartPlacement()
		{
			// if it's a network game, then the ClientReady message should be sent from
			// OnGameMessageReceived once this call stack returns
			if (!m_isNetworkGame)
			{
				RaiseSimpleUpdateEvent(GameMessage.PlayerPlacementStarted, string.Empty, null);
				//m_gameClient.SendMessageToServer(NetworkMessages.ClientReady, string.Empty);
			}
		}

		public void PlayerChoseUnits(Player p, int numTroopers, int numFighters, int numGennies, int numTransports)
		{
			/*
			m_controller.CreateUnits(p, UnitType.Fighter, numFighters);
			m_controller.CreateUnits(p, UnitType.Transport, numTransports);
			m_controller.CreateUnits(p, UnitType.Trooper, numTroopers);
			m_controller.CreateUnits(p, UnitType.Gennie,numGennies);
			*/

			string messageText = string.Format("{0}|{1}|{2}|{3}|{4}", p.Name,
												numTroopers, numFighters, numGennies, numTransports);

			if(m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.PlayerChoseUnits, messageText);
			}
			else
			{
				//RaiseSimpleUpdateEvent(string.Empty, NetworkMessages.PlayerChoseUnits, p);
				RaiseLocalLoopbackEvent(GameMessage.PlayerChoseUnits, messageText, p);
			}
		}

		public void PlayerPlacedUnits(Player player, Territory territory, UnitCollection placedUnits)
		{
			if(m_isNetworkGame)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(player.Name);
				sb.Append("|");
				sb.Append(territory.Name);

				Hashtable ht = placedUnits.GetUnitTypeCount();

				foreach(DictionaryEntry de in ht)
				{
					UnitType ut = (UnitType)de.Key;
					int numUnits = (int)de.Value;

					sb.Append("|");
					sb.AppendFormat("{0}:{1}", ut, numUnits);
				}

				m_gameClient.SendMessageToServer(GameMessage.PlayerPlacedUnits, sb.ToString());
			}
			else
			{
				RaiseSimpleUpdateEvent(GameMessage.PlayerPlacedUnits, string.Empty, player);
			}
		}

		#endregion





		
	}
}
