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
using RedCorona.Net;
using BuckRogers;
using BuckRogers.Networking;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
#endregion

namespace BuckRogers.Networking
{
	class ClientSideGameManager
	{
		public event EventHandler<ClientUpdateEventArgs> ClientUpdateMessage = delegate { };

		private GameController m_controller;
		private BattleController m_battleController;
		private BuckRogersClient m_gameClient;

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

		public ClientSideGameManager(BuckRogersClient client, GameController controller, BattleController battleController)
		{
			m_gameClient = client;
			m_controller = controller;
			m_battleController = battleController;

			m_gameClient.GameMessageReceived += new EventHandler<ClientUpdateEventArgs>(OnGameMessageReceived);
		}

		void OnGameMessageReceived(object sender, ClientUpdateEventArgs e)
		{
			switch (e.MessageType)
			{
				case NetworkMessages.InitialSetupInformation:
				{
					ParseInitialSetupInformation(e.MessageText);

					ClientUpdateEventArgs cuea = new ClientUpdateEventArgs();
					cuea.MessageType = NetworkMessages.InitialSetupInformation;

					EventsHelper.FireAsync(ClientUpdateMessage, this, cuea);
					break;
				}
			}
		}

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
				XmlNodeList xnlTerritories = xePlayers.GetElementsByTagName("Territory");

				foreach(XmlElement xeTerritory in xnlTerritories)
				{
					string territoryName = xeTerritory.Attributes["name"].Value;

					Territory t = m_controller.Map[territoryName];
					t.Owner = p;
				}

				XmlElement xeUnits = (XmlElement)xePlayer.GetElementsByTagName("Units")[0];
				XmlNodeList xnlUnits = xePlayers.GetElementsByTagName("Unit");

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
	}
}
