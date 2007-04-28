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
				case NetworkMessages.InitialSetupInformation:
				{
					ParseInitialSetupInformation(e.MessageText);

					ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
					cuea.MessageType = NetworkMessages.InitialSetupInformation;

					// synchronous, to make sure things complete before we report ready
					EventsHelper.Fire(ClientUpdateMessage, this, cuea);

					m_gameClient.SendMessageToServer(NetworkMessages.ClientReady, string.Empty);
					break;
				}
				case NetworkMessages.PlayerPlacementStarted:
				{
					CheckIfActiveClient();
					RaiseSimpleUpdateEvent(string.Empty, NetworkMessages.PlayerPlacementStarted, null);
					break;
				}
				case NetworkMessages.PlayerChoseUnits:
				{
					string[] messageParts = e.MessageText.Split('|');
					Player p = m_controller.GetPlayer(messageParts[0]);

					if(p.Location == PlayerLocation.Local)
					{
						break;
					}

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

					RaiseSimpleUpdateEvent(string.Empty, NetworkMessages.PlayerChoseUnits, p);
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

		private void RaiseSimpleUpdateEvent(string text, NetworkMessages message, Player p)
		{
			ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
			cuea.MessageText = text;
			cuea.MessageType = message;

			if (p != null)
			{
				cuea.Players.Add(p);
			}

			EventsHelper.FireAsync(ClientUpdateMessage, this, cuea);
		}

		#endregion

		#region message creation

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
				initialPlayerOrder.Add(p);

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

			m_controller.PlayerOrder = initialPlayerOrder;
		}

		#endregion

		#region external API

		public void ReadyToStartPlacement()
		{
			if(m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(NetworkMessages.ClientReady, string.Empty);
			}
			else
			{
				RaiseSimpleUpdateEvent(string.Empty, NetworkMessages.PlayerPlacementStarted, null);
			}
		}

		public void PlayerChoseUnits(Player p, int numTroopers, int numFighters, int numGennies, int numTransports)
		{
			m_controller.CreateUnits(p, UnitType.Fighter, numFighters);
			m_controller.CreateUnits(p, UnitType.Transport, numTransports);
			m_controller.CreateUnits(p, UnitType.Trooper, numTroopers);
			m_controller.CreateUnits(p, UnitType.Gennie,numGennies);

			if(m_isNetworkGame)
			{
				string message = string.Format("{0}|{1}|{2}|{3}|{4}", p.Name, 
												numTroopers, numFighters, numGennies, numTransports);
				m_gameClient.SendMessageToServer(NetworkMessages.PlayerChoseUnits, message);
			}
		}

		#endregion



		
	}
}
