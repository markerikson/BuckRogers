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
	public class ClientSideGameManager
	{
		#region private members
		public event EventHandler<ClientUpdateEventArgs> ClientUpdateMessage = delegate { };
		public event EventHandler<StatusUpdateEventArgs> TransportLanded = delegate { };

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

		public static bool IsLocalOrActive
		{
			get
			{
				bool isLocalOrActive = (!GameController.Options.IsNetworkGame || ClientSideGameManager.IsActiveClient);
				return isLocalOrActive;
			}
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
				case GameMessage.PlacementPhaseStarted:
				{
					CheckIfActiveClient();
					RaiseSimpleUpdateEvent(GameMessage.PlacementPhaseStarted, string.Empty, null);
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
					if(p.IsLocal)
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
				case GameMessage.PlacementPhaseEnded:
				{
					if(e.MessageText != "GameLoaded")
					{
						m_controller.NextPlayer();
					}
					
					RaiseSimpleUpdateEvent(GameMessage.PlacementPhaseEnded, string.Empty, null);
					break;
				}
				case GameMessage.MovementPhaseStarted:
				{
					if(e.MessageText != "GameLoaded")
					{
						string[] playerOrderNames = e.MessageText.Split('|');
						List<Player> playerOrder = new List<Player>();

						foreach (string s in playerOrderNames)
						{
							Player p = m_controller.GetPlayer(s);
							playerOrder.Add(p);
						}

						// if we got this message, we KNOW there will be another turn (the one
						// we're about to start)
						m_controller.NextTurn(playerOrder.ToArray());
						m_battleController.InitGameLog();
					}
					
					CheckIfActiveClient();
					RaiseSimpleUpdateEvent(GameMessage.MovementPhaseStarted, string.Empty, null);
					break;
				}
				case GameMessage.PlayerTransportedUnits:
				{
					if(!m_controller.CurrentPlayer.IsLocal)
					{
						ParseUnitTransportMessage(e.MessageText);
					}
					
					break;
				}
				case GameMessage.PlayerMovedUnits:
				{
					if (!m_controller.CurrentPlayer.IsLocal)
					{
						ParseUnitMoveMessage(e.MessageText);
					}
					break;
				}
				case GameMessage.PlayerUndidMove:
				{
					if (!m_controller.CurrentPlayer.IsLocal)
					{
						m_controller.UndoAction();
					}
					break;
				}
				case GameMessage.PlayerRedidMove:
				{
					if (!m_controller.CurrentPlayer.IsLocal)
					{
						m_controller.RedoAction();
					}
					break;
				}
				case GameMessage.PlayerFinishedMoving:
				{
					if (!m_controller.CurrentPlayer.IsLocal)
					{
						m_controller.FinalizeCurrentPlayerMoves();
					}
					break;
				}
				case GameMessage.MovementPhaseEnded:
				{
					m_controller.StartNextPhase();

					if(m_controller.Battles.Count == 0)
					{
						goto case GameMessage.CombatPhaseEnded;
					}
					else
					{
						m_battleController.Battles = m_controller.Battles;
						RaiseSimpleUpdateEvent(GameMessage.CombatPhaseStarted, string.Empty, null);
					}
					break;
				}
				case GameMessage.NextBattle:
				{
					if(!m_battleController.NextBattle())
					{
						RaiseSimpleUpdateEvent(GameMessage.CombatPhaseEnded, string.Empty, null);
					}
					
					break;
				}
				case GameMessage.CombatPhaseEnded:
				{
					m_controller.StartNextPhase();

					RaiseSimpleUpdateEvent(GameMessage.CombatPhaseEnded, string.Empty, null);
					break;
				}
				case GameMessage.CombatAttack:
				{
					if(!m_battleController.CurrentPlayer.IsLocal)
					{
						ParseCombatAttackMessage(e.MessageText);
					}
					break;
				}
			}
		}

		#endregion

		#region private utility

		private void CheckIfActiveClient()
		{
			m_isActiveClient = m_controller.CurrentPlayer.IsLocal;
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

		#region message parsing / creation

		private void ParseInitialSetupInformation(string messageText)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(messageText);

			// Check if loading from saved game

			XmlElement xeGameInfo = (XmlElement)xd.GetElementsByTagName("GameInformation")[0];

			bool fromSavedGame = bool.Parse(xeGameInfo.Attributes["savedgame"].Value);

			if(fromSavedGame)
			{
				//XmlElement xeGameOptions = (XmlElement)xeGameInfo.GetElementsByTagName("GameOptions");
				//m_gameClient.ParseGameOptionsMessage(messageText);//xeGameOptions.OuterXml);
				//GameController.Options = m_gameClient.GameOptions;

				XmlDocument savedgame = new XmlDocument();
				XmlElement xeSavedGame = (XmlElement)xd.GetElementsByTagName("SavedGame")[0];
				savedgame.LoadXml(xeSavedGame.OuterXml);

				m_controller.LoadGame(savedgame);

				GameController.Options.IsNetworkGame = true;

				// if loading from save game, do a foreach over the players, create them,
				// and overwrite the previously set players

				foreach(Player p in m_gameClient.Players)
				{
					Player loadedPlayer = m_controller.GetPlayer(p.Name);
					loadedPlayer.Location = p.Location;
				}
			}
			else
			{
				List<Player> initialPlayerOrder = new List<Player>();

				XmlElement xePlayers = (XmlElement)xd.GetElementsByTagName("Players")[0];
				XmlNodeList xnlPlayers = xePlayers.GetElementsByTagName("Player");



				foreach (XmlElement xePlayer in xnlPlayers)
				{
					string playerName = xePlayer.Attributes["name"].Value;

					Player p = m_controller.GetPlayer(playerName);

					XmlElement xeTerritories = (XmlElement)xePlayer.GetElementsByTagName("Territories")[0];
					XmlNodeList xnlTerritories = xePlayer.GetElementsByTagName("Territory");

					List<Territory> territories = new List<Territory>();

					foreach (XmlElement xeTerritory in xnlTerritories)
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

						if (territoryName != "None")
						{
							Territory t = m_controller.Map[territoryName];
							u.CurrentTerritory = t;
						}

					}
				}

				XmlElement xePlayerOrder = (XmlElement)xd.GetElementsByTagName("InitialOrder")[0];
				string playerOrderString = xePlayerOrder.Attributes["order"].Value;
				string[] playerOrderNames = playerOrderString.Split('|');

				foreach (string playerOrderName in playerOrderNames)
				{
					Player p = m_controller.GetPlayer(playerOrderName);
					initialPlayerOrder.Add(p);
				}

				m_controller.PlayerOrder = initialPlayerOrder;
			}

			
		}

		private string CreateTransportMessage(List<TransportAction> actions)
		{
			MemoryStream stream = new MemoryStream();
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.OmitXmlDeclaration = true;
			XmlWriter xw = XmlWriter.Create(stream, xws);

			xw.WriteStartElement("TransportActions");
			
			Player p = actions[0].Owner;

			xw.WriteAttributeString("player", p.Name);
			
			foreach(TransportAction ta in actions)
			{
				xw.WriteStartElement("TransportAction");

				xw.WriteAttributeString("territory", ta.StartingTerritory.Name);
				xw.WriteAttributeString("load", ta.Load.ToString());
				xw.WriteAttributeString("transport", ta.Transport.ID.ToString());

				xw.WriteStartElement("Units");

				foreach(Unit u in ta.Units)
				{
					xw.WriteStartElement("Unit");

					xw.WriteAttributeString("type", u.Type.ToString());
					xw.WriteAttributeString("id", u.ID.ToString());
					xw.WriteAttributeString("moves", u.MovesLeft.ToString());

					xw.WriteEndElement();
				}

				xw.WriteEndElement();
				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteEndDocument();

			xw.Flush();

			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);

			return sr.ReadToEnd();
		}

		private string CreateMoveMessage(MoveAction ma)
		{
			MemoryStream stream = new MemoryStream();
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.OmitXmlDeclaration = true;
			XmlWriter xw = XmlWriter.Create(stream, xws);

			xw.WriteStartElement("MoveAction");

			xw.WriteAttributeString("player", ma.Owner.Name);
			xw.WriteAttributeString("territory", ma.StartingTerritory.Name);

			xw.WriteStartElement("Territories");

			foreach (Territory t in ma.Territories)
			{
				xw.WriteStartElement("Territory");
				xw.WriteAttributeString("name", t.Name);
				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteStartElement("Units");

			foreach (Unit u in ma.Units)
			{
				xw.WriteStartElement("Unit");

				xw.WriteAttributeString("type", u.Type.ToString());
				xw.WriteAttributeString("id", u.ID.ToString());
				xw.WriteAttributeString("moves", u.MovesLeft.ToString());

				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteEndElement();


			xw.WriteEndDocument();

			xw.Flush();

			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);

			return sr.ReadToEnd();
		}

		private void ParseUnitTransportMessage(string xml)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(xml);

			XmlElement xeTransportActions = (XmlElement)xd.GetElementsByTagName("TransportActions")[0];

			/*
			string playerName = xeTransportActions.Attributes["player"].Value;

			if (m_controller.GetPlayer(playerName).IsLocal)
			{
				return;
			}
			*/

			XmlNodeList xnlActions = xeTransportActions.GetElementsByTagName("TransportAction");

			foreach (XmlElement xeAction in xnlActions)
			{
				TransportAction ta = new TransportAction();

				string territoryName = xeAction.Attributes["territory"].Value;
				Territory t = m_controller.Map[territoryName];
				ta.StartingTerritory = t;

				ta.Load = bool.Parse(xeAction.Attributes["load"].Value);

				int transportID = int.Parse(xeAction.Attributes["transport"].Value);
				Transport transport = (Transport)t.Units.GetUnitByID(transportID);
				ta.Transport = transport;

				XmlElement xeUnits = (XmlElement)xeAction.GetElementsByTagName("Units")[0];
				XmlNodeList xnlUnits = xeUnits.GetElementsByTagName("Unit");

				foreach (XmlElement xeUnit in xnlUnits)
				{
					UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), xeUnit.Attributes["type"].Value);
					int id = int.Parse(xeUnit.Attributes["id"].Value);
					int movesLeft = int.Parse(xeUnit.Attributes["moves"].Value);

					Unit u = t.Units.GetUnitByID(id);
					ta.Units.AddUnit(u);

					//UnitCollection uc1 = t.Units.GetUnitsWithMoves(movesLeft);
					//UnitCollection uc2 = uc1.GetUnits(ut, p, t, 1);

					//ta.Units.AddUnit(uc2[0]);
				}

				m_controller.AddAction(ta);

			}
		}

		private void ParseUnitMoveMessage(string xml)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(xml);

			MoveAction ma = new MoveAction();

			XmlElement xeMoveAction = (XmlElement)xd.GetElementsByTagName("MoveAction")[0];
			string playerName = xeMoveAction.Attributes["player"].Value;
			string territoryName = xeMoveAction.Attributes["territory"].Value;

			Player p = m_controller.GetPlayer(playerName);

			/*
			if (p.IsLocal)
			{
				return;
			}
			*/

			Territory t = m_controller.Map[territoryName];
			ma.Owner = p;
			ma.StartingTerritory = t;

			XmlElement xeTerritories = (XmlElement)xeMoveAction.GetElementsByTagName("Territories")[0];
			XmlNodeList xnlTerritories = xeTerritories.GetElementsByTagName("Territory");

			foreach (XmlElement xeTerritory in xnlTerritories)
			{
				string moveTerritoryName = xeTerritory.Attributes["name"].Value;

				Territory moveTerritory = m_controller.Map[moveTerritoryName];
				ma.Territories.Add(moveTerritory);
			}


			XmlElement xeUnits = (XmlElement)xeMoveAction.GetElementsByTagName("Units")[0];
			XmlNodeList xnlUnits = xeUnits.GetElementsByTagName("Unit");

			foreach (XmlElement xeUnit in xnlUnits)
			{
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), xeUnit.Attributes["type"].Value);
				int id = int.Parse(xeUnit.Attributes["id"].Value);
				int movesLeft = int.Parse(xeUnit.Attributes["moves"].Value);

				Unit u = t.Units.GetUnitByID(id);
				ma.Units.AddUnit(u);
			}

			m_controller.AddAction(ma);
		}

		private string CreateCombatAttackMessage(CombatInfo ci)
		{
			MemoryStream stream = new MemoryStream();
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.OmitXmlDeclaration = true;
			XmlWriter xw = XmlWriter.Create(stream, xws);

			xw.WriteStartElement("CombatInfo");
			xw.WriteAttributeString("type", ci.Type.ToString());
			xw.WriteAttributeString("attackingLeader", ci.AttackingLeader.ToString());

			xw.WriteStartElement("Attackers");

			foreach(Unit u in ci.Attackers)
			{
				xw.WriteStartElement("Unit");

				xw.WriteAttributeString("type", u.Type.ToString());
				xw.WriteAttributeString("id", u.ID.ToString());

				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteStartElement("Defenders");

			foreach (Unit u in ci.Defenders)
			{
				xw.WriteStartElement("Unit");

				xw.WriteAttributeString("type", u.Type.ToString());
				xw.WriteAttributeString("id", u.ID.ToString());

				xw.WriteEndElement();
			}

			xw.WriteEndElement();

			xw.WriteEndDocument();

			xw.Flush();

			stream.Position = 0;
			StreamReader sr = new StreamReader(stream);

			return sr.ReadToEnd();
		}

		private CombatInfo ParseCombatAttackMessage(string xml)
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml(xml);

			XmlElement xeCombatInfo = (XmlElement)xd.GetElementsByTagName("CombatInfo")[0];

			CombatInfo ci = new CombatInfo();
			ci.Type = (BattleType)Enum.Parse(typeof(BattleType), xeCombatInfo.Attributes["type"].Value);
			ci.AttackingLeader = bool.Parse(xeCombatInfo.Attributes["attackingLeader"].Value);

			XmlElement xeAttackers = (XmlElement)xeCombatInfo.GetElementsByTagName("Attackers")[0];
			XmlNodeList xnlAttackers = xeAttackers.GetElementsByTagName("Unit");

			foreach (XmlElement xeUnit in xnlAttackers)
			{
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), xeUnit.Attributes["type"].Value);
				int id = int.Parse(xeUnit.Attributes["id"].Value);

				Unit u = Unit.AllUnits.GetUnitByID(id);
				ci.Attackers.AddUnit(u);
			}

			XmlElement xeDefenders = (XmlElement)xeCombatInfo.GetElementsByTagName("Defenders")[0];
			XmlNodeList xnlDefenders = xeDefenders.GetElementsByTagName("Unit");

			foreach (XmlElement xeUnit in xnlDefenders)
			{
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), xeUnit.Attributes["type"].Value);
				int id = int.Parse(xeUnit.Attributes["id"].Value);

				Unit u = Unit.AllUnits.GetUnitByID(id);
				ci.Defenders.AddUnit(u);
			}

			return ci;
		}

		#endregion

		#region external API

		public void ReadyToStartPlacement()
		{
			// if it's a network game, then the ClientReady message should be sent from
			// OnGameMessageReceived once this call stack returns
			if (!m_isNetworkGame)
			{
				RaiseSimpleUpdateEvent(GameMessage.PlacementPhaseStarted, string.Empty, null);
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
				bool morePlayersLeft = m_controller.NextPlayer();
				RaiseSimpleUpdateEvent(GameMessage.NextPlayer, string.Empty, null);
				CheckIfActiveClient();

				if(!morePlayersLeft)
				{
					m_controller.StartNextPhase();
					RaiseSimpleUpdateEvent(GameMessage.PlacementPhaseEnded, string.Empty, null);

					m_controller.NextTurn();
					m_battleController.InitGameLog();
					RaiseSimpleUpdateEvent(GameMessage.MovementPhaseStarted, string.Empty, null);
				}
				//RaiseSimpleUpdateEvent(GameMessage.PlayerPlacedUnits, string.Empty, player);
			}
		}

		public void PlayerTransportedUnits(List<TransportAction> actions)
		{
			if(!m_isNetworkGame)
			{
				return;
			}

			string xml = CreateTransportMessage(actions);

			m_gameClient.SendMessageToServer(GameMessage.PlayerTransportedUnits, xml);
		}

		public void PlayerMovedUnits(MoveAction ma)
		{
			if (m_isNetworkGame)
			{
				string xml = CreateMoveMessage(ma);

				m_gameClient.SendMessageToServer(GameMessage.PlayerMovedUnits, xml);
			}

			Territory destination = (Territory)ma.Territories[ma.Territories.Count - 1];
			if (destination.Type == TerritoryType.Ground)
			{
				bool unloadTransports = false;

				UnitCollection transports = ma.Units.GetUnits(UnitType.Transport);
				if (transports.Count > 0)
				{
					bool askToUnload = false;

					foreach (Transport tr in transports)
					{
						if (tr.Transportees.Count > 0)
						{
							askToUnload = true;
							break;
						}
					}
					if (askToUnload)
					{
						StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
						suea.Territories.Add(destination);// = destination;//(Player)m_currentPlayerOrder[m_idxCurrentPlayer];

						suea.StatusInfo = StatusInfo.TransportLanded;

						EventsHelper.Fire(TransportLanded, this, suea);

						//unloadTransports = StatusUpdate(this, suea);
						unloadTransports = suea.Result;

					}
				}

				if (unloadTransports)
				{
					List<TransportAction> actions = new List<TransportAction>();
					foreach (Transport tr in transports)
					{
						TransportAction ta = new TransportAction();
						ta.Load = false;
						ta.Owner = tr.Owner;
						ta.MaxTransfer = tr.Transportees.Count;
						ta.StartingTerritory = destination;
						ta.Transport = tr;
						ta.UnitType = tr.Transportees[0].Type;

						m_controller.AddAction(ta);
						actions.Add(ta);
					}

					if(m_isNetworkGame)
					{
						string xml = CreateTransportMessage(actions);

						m_gameClient.SendMessageToServer(GameMessage.PlayerTransportedUnits, xml);
					}
					
				}
			}
		}

		public void PlayerUndidMove()
		{
			Action a = m_controller.UndoAction();

			if(m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.PlayerUndidMove, m_controller.CurrentPlayer.Name);
			}
		}

		public void PlayerRedidMove()
		{
			Action a = m_controller.RedoAction();

			if (m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.PlayerRedidMove, m_controller.CurrentPlayer.Name);
			}
		}

		public void PlayerFinishedMoving()
		{
			m_controller.FinalizeCurrentPlayerMoves();

			if(m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.PlayerFinishedMoving, m_controller.CurrentPlayer.Name);
			}
			else
			{
				m_controller.FinalizeCurrentPlayerMoves();

				bool morePlayers = m_controller.NextPlayer();
				RaiseSimpleUpdateEvent(GameMessage.NextPlayer, string.Empty, null);

				if (!morePlayers)
				{
					RaiseLocalLoopbackEvent(GameMessage.MovementPhaseEnded, string.Empty, null);
				}
			}
		}

		public void ReadyToBeginCombat()
		{
			if(m_isNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.ClientReadyForCombat, string.Empty);
			}
			else
			{
				RaiseLocalLoopbackEvent(GameMessage.NextBattle, string.Empty, null);
			}
		}

		public void ExecuteCombatAttack(CombatInfo ci)
		{
			m_battleController.DoCombat(ci);

			if(m_isNetworkGame)
			{
				string message = CreateCombatAttackMessage(ci);
				m_gameClient.SendMessageToServer(GameMessage.CombatAttack, message);
			}
		}

		public void CombatCompleted()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

	}
}
