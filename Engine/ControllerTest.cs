using System;
using NUnit.Framework;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for ControllerTest.
	/// </summary>
	[TestFixture]
	public class ControllerTest
	{
		private GameController m_controller;
		private BattleController m_battleController;
		public ControllerTest()
		{
		}

		[SetUp]
		public void Init()
		{
			string[] players = {"Mark", "Chris", "Stu", "Hannah", "Jake", "Kathryn"};
			m_controller = new GameController(players);
			m_battleController = new BattleController(m_controller);

			m_controller.AssignTerritories();
			m_controller.CreateInitialUnits();

			m_controller.RollForInitiative(false);

			string[,] territories = { {"Deimos", "Psyche", "Urban Reservations", "Space Elevator", 
										   "Thule", "Aurora", "Tolstoi"},
										{"Mt. Maxwell", "Moscoviense", "Tycho", "Aerostates", "Mariposas", 
											"African Regency", "Boreal Sea"},
										{"Pallas", "Eurasian Regency", "Fortuna", "Coprates Chasm", "Hielo", 
											"Sobkou Plains", "The Warrens"},
										{"Lowlanders", "L-4 Colony", "Hygeia", "Wreckers", "Beta Regio", 
											"L-5 Colony", "Arcadia"},
										{"Juno", "Independent Arcologies", "Tranquility", "Vesta", "American Regency", 
											"Ceres", "Pavonis"},
										{"Ram HQ", "Bach", "Elysium", "Antarctic Testing Zone", "Aphrodite", 
											"Australian Development Facility", "Farside"},
									 };

			ArrayList al = new ArrayList();
			for (int i = 0; i < players.Length; i++)
			{
				for (int j = 0; j < territories.GetLength(1); j++)
				{
					Territory t = m_controller.Map[territories[i, j]];
					t.Owner = m_controller.Players[i];
					al.Add(t);
				}
			}

			#region Mark's unit setup

			Player mark = m_controller.Players[0];

			UnitCollection units = mark.Units;
			
			Territory thule = m_controller.Map["Thule"];
			Territory elevator = m_controller.Map["Space Elevator"];
			Territory deimos = m_controller.Map["Deimos"];

			UnitCollection uc = units.GetUnits(UnitType.Fighter);

			for (int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = deimos;
			}

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = thule;

			uc = mark.Units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = thule;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = deimos;
			uc[1].CurrentTerritory = elevator;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = elevator;
			uc[1].CurrentTerritory = elevator;


			uc = units.GetUnits(UnitType.Trooper);
			uc[0].CurrentTerritory = deimos;

			for(int i = 1; i < 4; i++)
			{
				uc[i].CurrentTerritory = elevator;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = thule;
			}

			#endregion

			#region Chris's unit setup

			Territory africa = m_controller.Map["African Regency"];
			Territory mosco = m_controller.Map["Moscoviense"];
			Territory tycho = m_controller.Map["Tycho"];

			Player chris = m_controller.Players[1];
			units = chris.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = africa;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = mosco;
			uc[1].CurrentTerritory = tycho;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = mosco;
			uc[1].CurrentTerritory = tycho;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = africa;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = tycho;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = mosco;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = africa;
			}

			#endregion

			#region Stu's unit setup

			Territory hielo = m_controller.Map["Hielo"];
			Territory warrens = m_controller.Map["The Warrens"];
			Territory sobkou = m_controller.Map["Sobkou Plains"];

			Player stu = m_controller.Players[2];
			units = stu.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = hielo;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = warrens;
			uc[1].CurrentTerritory = sobkou;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = warrens;
			uc[1].CurrentTerritory = sobkou;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = hielo;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = sobkou;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = warrens;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = hielo;
			}

			#endregion

			#region Hannah's unit setup

			Territory wreckers = m_controller.Map["Wreckers"];
			Territory beta = m_controller.Map["Beta Regio"];
			Territory lowlanders = m_controller.Map["Lowlanders"];

			Player hannah = m_controller.Players[3];
			units = hannah.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = wreckers;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = beta;
			uc[1].CurrentTerritory = lowlanders;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = beta;
			uc[1].CurrentTerritory = lowlanders;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = wreckers;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = lowlanders;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = beta;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = wreckers;
			}

			#endregion

			#region Jake's unit setup

			Territory tranquility = m_controller.Map["Tranquility"];
			Territory arcologies = m_controller.Map["Independent Arcologies"];
			Territory america = m_controller.Map["American Regency"];

			Player jake = m_controller.Players[4];
			units = jake.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = tranquility;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = arcologies;
			uc[1].CurrentTerritory = america;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = arcologies;
			uc[1].CurrentTerritory = america;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = tranquility;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = america;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = arcologies;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = tranquility;
			}

			#endregion

			#region Kathryn's unit setup

			Territory farside = m_controller.Map["Farside"];
			Territory antarctica = m_controller.Map["Antarctic Testing Zone"];
			Territory australia = m_controller.Map["Australian Development Facility"];

			Player kathryn = m_controller.Players[5];
			units = kathryn.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = farside;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = antarctica;
			uc[1].CurrentTerritory = australia;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = antarctica;
			uc[1].CurrentTerritory = australia;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = farside;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = australia;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = antarctica;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = farside;
			}

			#endregion


			

		}

		#region Movement tests
		[Test]
		public void AddValidMoves()
		{
			Territory thule = m_controller.Map["Thule"];
			Player mark = m_controller.GetPlayer("Mark");

			MoveAction move = new MoveAction();
			move.Owner = mark;
			move.StartingTerritory = thule;
			move.Units.AddAllUnits(thule.Units);

			ArrayList territories = new ArrayList();
			territories.Add(m_controller.Map["Thule Orbit"]);
			territories.Add(m_controller.Map["Asteroid Orbit: 19"]);
			territories.Add(m_controller.Map["Asteroid Orbit: 20"]);
			Territory tmo20 = m_controller.Map["Trans-Mars Orbit: 20"];
			territories.Add(tmo20);
			
			m_controller.AddAction(move);



			//Assert.IsFalse(result);
		}

		[Test]
		public void TransportTroopers()
		{
			Territory thule = m_controller.Map["Thule"];

			Transport transport = thule.Units.GetUnits(UnitType.Transport)[0] as Transport;

			UnitCollection troopers = thule.Units.GetUnits(UnitType.Trooper);
			TransportAction ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			ta.Units = troopers;
			ta.UnitType = UnitType.Trooper;
			ta.Load = true;
			m_controller.AddAction(ta);
			//m_controller.LoadTransport(transport, troopers, UnitType.Trooper);

			Assert.AreEqual(transport.Transportees.Count, 4);

			Territory arcadia = m_controller.Map["Arcadia"];
			transport.CurrentTerritory = arcadia;


			ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			//ta.Units = factories;
			//ta.UnitType = UnitType.Factory;
			ta.Load = false;
			m_controller.AddAction(ta);
			//m_controller.UnloadTransport(transport, 5);


			Assert.AreEqual(0, transport.Transportees.Count );
			Assert.AreEqual(5, arcadia.Units.Count);
		}

		[Test]
		public void TransportFactory()
		{
			Territory thule = m_controller.Map["Thule"];
			Territory deimos = m_controller.Map["Deimos"];
			Transport transport = thule.Units.GetUnits(UnitType.Transport)[0] as Transport;

			transport.CurrentTerritory = deimos;

			UnitCollection factories = deimos.Units.GetUnits(UnitType.Factory);
			TransportAction ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			ta.Units = factories;
			ta.UnitType = UnitType.Factory;
			ta.Load = true;
			m_controller.AddAction(ta);
			//m_controller.LoadTransport(transport, factories, UnitType.Factory);

			Assert.AreEqual(transport.Transportees.Count, 1);
			Territory arcadia = m_controller.Map["Arcadia"];
			transport.CurrentTerritory = arcadia;


			ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			//ta.Units = factories;
			//ta.UnitType = UnitType.Factory;
			ta.Load = false;
			m_controller.AddAction(ta);
			//m_controller.UnloadTransport(transport, 5);

			Assert.AreEqual(0, transport.Transportees.Count );
			Assert.AreEqual(arcadia.Units.Count, 2);
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void LoadFactoryInFullTransport()
		{
			Territory thule = m_controller.Map["Thule"];
			Territory deimos = m_controller.Map["Deimos"];
			Transport transport = thule.Units.GetUnits(UnitType.Transport)[0] as Transport;

			UnitCollection troopers = thule.Units.GetUnits(UnitType.Trooper);

			TransportAction ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			ta.Units = troopers;
			ta.UnitType = UnitType.Trooper;
			ta.Load = true;

			m_controller.AddAction(ta);
			//m_controller.LoadTransport(transport, troopers, UnitType.Trooper);

			transport.CurrentTerritory = deimos;

			UnitCollection factories = deimos.Units.GetUnits(UnitType.Factory);
			
			ta = new TransportAction();
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			ta.Units = factories;
			ta.UnitType = UnitType.Factory;
			ta.Load = true;

			m_controller.AddAction(ta);
			//m_controller.LoadTransport(transport, factories, UnitType.Factory);
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void OverLoadTransport()
		{
			Territory thule = m_controller.Map["Thule"];
			Territory elevator = m_controller.Map["Space Elevator"];
			Transport transport = thule.Units.GetUnits(UnitType.Transport)[0] as Transport;

			UnitCollection troopers = thule.Units.GetUnits(UnitType.Trooper);
			TransportAction ta = new TransportAction();
			ta.Units = troopers;
			ta.Transport = transport;
			ta.Owner = transport.Owner;
			ta.UnitType = UnitType.Trooper;
			ta.Load = true;
			//m_controller.LoadTransport(transport, troopers, UnitType.Trooper);
			//m_controller.LoadTransport(ta);
			m_controller.AddAction(ta);

			transport.CurrentTerritory = elevator;

			UnitCollection factories = elevator.Units.GetUnits(UnitType.Trooper);
			ta = new TransportAction();
			ta.Owner = transport.Owner;
			ta.Transport = transport;
			ta.Units = factories;
			ta.UnitType = UnitType.Trooper;
			ta.Load = true;

			m_controller.AddAction(ta);
			//m_controller.LoadTransport(transport, factories, UnitType.Trooper);
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void MoveFactory()
		{
			Territory elevator = m_controller.Map["Space Elevator"];
			Territory nearOrbit = m_controller.Map["Near Mars Orbit"];

			UnitCollection factories = elevator.Units.GetUnits(UnitType.Factory);
			MoveAction move = new MoveAction();
			move.Owner = elevator.Owner;
			move.Territories.Add(nearOrbit);
			move.Units = factories;
			move.StartingTerritory = elevator;

			m_controller.AddAction(move);			
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void MoveGroundUnitIntoSpace()
		{
			Territory elevator = m_controller.Map["Space Elevator"];
			Territory nearOrbit = m_controller.Map["Near Mars Orbit"];

			UnitCollection gennies = elevator.Units.GetUnits(UnitType.Gennie);
			MoveAction move = new MoveAction();
			move.Owner = elevator.Owner;
			move.Territories.Add(nearOrbit);
			move.Units = gennies;
			move.StartingTerritory = elevator;

			m_controller.AddAction(move);			
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void MoveUnitNonAdjacent()
		{
			Territory deimos = m_controller.Map["Deimos"];
			Territory nearOrbit = m_controller.Map["Near Mars Orbit"];

			UnitCollection fighters = deimos.Units.GetUnits(UnitType.Fighter);
			MoveAction move = new MoveAction();
			move.Owner = deimos.Owner;
			move.Territories.Add(nearOrbit);
			move.Units = fighters;
			move.StartingTerritory = deimos;

			m_controller.AddAction(move);			
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void MoveUnitPastLimit()
		{
			Territory elevator = m_controller.Map["Space Elevator"];
			Territory pavonis = m_controller.Map["Pavonis"];
			Territory arcadia = m_controller.Map["Arcadia"];

			UnitCollection gennies = elevator.Units.GetUnits(UnitType.Gennie);
			MoveAction move = new MoveAction();
			move.Owner = elevator.Owner;
			move.Territories.Add(pavonis);
			move.Territories.Add(arcadia);
			move.Units = gennies;
			move.StartingTerritory = elevator;

			m_controller.AddAction(move);			
		}

		[Test, ExpectedException(typeof(ActionException))]
		public void MoveUnitThroughEnemyTerritory()
		{
			Territory tranquility = m_controller.Map["Tranquility"];
			Territory farside = m_controller.Map["Farside"];
			Territory orbit = m_controller.Map["Near Moon Orbit"];

			UnitCollection fighters = tranquility.Units.GetUnits(UnitType.Fighter);
			MoveAction move = new MoveAction();
			move.Owner = tranquility.Owner;
			move.Territories.Add(farside);
			move.Territories.Add(orbit);
			move.Units = fighters;
			move.StartingTerritory = tranquility;

			m_controller.AddAction(move);			
		}

		[Test]
		public void MakeMultipleMoves()
		{
			Territory thule = m_controller.Map["Thule"];
			Player mark = m_controller.GetPlayer("Mark");

			UnitCollection troopers = thule.Units.GetUnits(UnitType.Trooper);
			Unit leader = thule.Units.GetUnits(UnitType.Leader)[0] as Unit;
			Transport transport = thule.Units.GetUnits(UnitType.Transport)[0] as Transport;
			TransportAction ta = new TransportAction();
			ta.Transport = transport;
			ta.Units = troopers;
			ta.Load = true;
			ta.UnitType = UnitType.Trooper;
			
			m_controller.AddAction(ta);

			
			MoveAction move = new MoveAction();
			move.Owner = mark;
			move.StartingTerritory = thule;
			//Unit
			move.Units.AddAllUnits(thule.Units);

			//ArrayList territories = new ArrayList();
			move.Territories.Add(m_controller.Map["Thule Orbit"]);
			move.Territories.Add(m_controller.Map["Asteroid Orbit: 19"]);
			move.Territories.Add(m_controller.Map["Asteroid Orbit: 20"]);
			Territory tmo20 = m_controller.Map["Trans-Mars Orbit: 20"];
			move.Territories.Add(tmo20);
			
			m_controller.AddAction(move);

			

			for(int i = 0; i < move.Units.Count; i++)
			{
				Unit u = move.Units[i];
				//Console.WriteLine("Current territory: " + u.CurrentTerritory.Name);
				Assert.AreEqual(tmo20, u.CurrentTerritory);
			}

			m_controller.EndMovePhase();
			m_controller.Map.AdvancePlanets();

			move = new MoveAction();
			move.Owner = mark;
			move.StartingTerritory = tmo20;
			move.Units.AddAllUnits(tmo20.Units);
			move.Territories.Add(m_controller.Map["Mars Orbit: 10"]);
			Territory farOrbit = m_controller.Map["Far Mars Orbit"];
			move.Territories.Add(farOrbit);
			Territory deimos = m_controller.Map["Deimos"];
			move.Territories.Add(deimos);

			m_controller.AddAction(move);

			ta = new TransportAction();
			ta.Owner = mark;
			ta.MaxTransfer = 3;
			ta.Load = false;
			ta.Transport = deimos.Units.GetUnits(UnitType.Transport)[0] as Transport;
			ta.StartingTerritory = deimos;

			m_controller.AddAction(ta);

			move = new MoveAction();
			move.Owner = mark;
			move.StartingTerritory = deimos;
			UnitCollection fighters = deimos.Units.GetUnits(UnitType.Fighter);
			move.Units.AddUnit(fighters[0]);
			UnitCollection transports = deimos.Units.GetUnits(UnitType.Transport);
			move.Units.AddUnit(transports[0]);
			move.Territories.Add(farOrbit);

			m_controller.AddAction(move);
			

			Assert.AreEqual(transport.CurrentTerritory, farOrbit);
			Assert.AreEqual(1, transport.Transportees.Count);
			Assert.AreEqual(deimos, leader.CurrentTerritory);

			m_controller.UndoAction();
			m_controller.UndoAction();
			m_controller.UndoAction();

			Console.WriteLine("Expecting Trans-Mars Orbit: 20, got: " + leader.CurrentTerritory.Name);
			Assert.AreEqual(tmo20, leader.CurrentTerritory);
			Assert.AreEqual(tmo20, transport.CurrentTerritory);
			Assert.AreEqual(4, transport.Transportees.Count);
			Assert.AreEqual(4, deimos.Units.GetUnits(UnitType.Fighter).Count);

			m_controller.RedoAction();
			m_controller.RedoAction();
			m_controller.RedoAction();

			Assert.AreEqual(transport.CurrentTerritory, farOrbit);
			Assert.AreEqual(1, transport.Transportees.Count);
			Assert.AreEqual(deimos, leader.CurrentTerritory);
		}

		#endregion

		#region Combat tests
		[Test]
		public void FindBattles()
		{
			Territory africa = m_controller.Map["African Regency"];
			Territory antarctica = m_controller.Map["Antarctic Testing Zone"];
			UnitCollection uc = africa.Units.GetUnits(UnitType.Fighter);
			
            MoveAction ma = new MoveAction();
			ma.Owner = africa.Owner;
			ma.StartingTerritory = africa;
			ma.Territories.Add(antarctica);
			ma.Units.AddAllUnits(uc);

			m_controller.AddAction(ma);


			Territory farside = m_controller.Map["Farside"];
			uc = farside.Units.GetUnits(UnitType.Fighter);

			ma = new MoveAction();
			ma.Owner = farside.Owner;
			ma.StartingTerritory = farside;
			ma.Units.AddAllUnits(uc);
			ma.Territories.Add(m_controller.Map["Near Moon Orbit"]);
			ma.Territories.Add(m_controller.Map["Near Earth Orbit"]);
			Territory arcologies = m_controller.Map["Independent Arcologies"];
			ma.Territories.Add(arcologies);

			m_controller.AddAction(ma);

			Territory tranquility = m_controller.Map["Tranquility"];
			uc = tranquility.Units;

			ma = new MoveAction();
			ma.Owner = tranquility.Owner;
			ma.StartingTerritory = tranquility;
			ma.Units.AddAllUnits(uc);
			Territory moscoviense = m_controller.Map["Moscoviense"];
			ma.Territories.Add(moscoviense);

			m_controller.AddAction(ma);

			Territory mercuryOrbit1 = m_controller.Map["Mercury Orbit: 1"];
			Territory transMarsOrbit15 = m_controller.Map["Trans-Mars Orbit: 15"];

			UnitCollection uc2 = m_controller.Map["Deimos"].Units.GetUnits(UnitType.Fighter);
			UnitCollection uc3 = m_controller.Map["Hielo"].Units.GetUnits(UnitType.Fighter);

			uc2[0].CurrentTerritory = mercuryOrbit1;
			uc2[1].CurrentTerritory = transMarsOrbit15;

			uc3[0].CurrentTerritory = mercuryOrbit1;
			uc3[1].CurrentTerritory = transMarsOrbit15;

			Player stu = m_controller.GetPlayer("Stu");
			Planet mercury = (Planet)m_controller.Map.Planets["Mercury"];
			foreach(Territory t in mercury.Surface)
			{
				t.Owner = stu;
			}
			mercury.CheckControl();
			Unit satellite = Unit.CreateNewUnit(stu, UnitType.KillerSatellite);
			satellite.CurrentTerritory = mercury.NearOrbit;
			
			Player mark = m_controller.GetPlayer("Mark");
			Unit battler = Unit.CreateNewUnit(mark, UnitType.Battler);
			battler.CurrentTerritory = mercury.NearOrbit;
			for(int i = 0; i < 5; i++)
			{
				Unit fighter = Unit.CreateNewUnit(mark, UnitType.Fighter);
				fighter.CurrentTerritory = mercury.NearOrbit;
			}

			Unit battler2 = Unit.CreateNewUnit(mark, UnitType.Battler);
			Planet venus = (Planet)m_controller.Map.Planets["Venus"];
			battler2.CurrentTerritory = venus.NearOrbit;

			Unit battler3 = Unit.CreateNewUnit(mark, UnitType.Battler);
			Planet earth = (Planet)m_controller.Map.Planets["Earth"];
			battler3.CurrentTerritory = earth.NearOrbit;

			Player chris = m_controller.GetPlayer("Chris");
			Unit battler4 = Unit.CreateNewUnit(chris, UnitType.Battler);
			Planet mars = (Planet)m_controller.Map.Planets["Mars"];
			battler4.CurrentTerritory = mars.FarOrbit;

			Unit battler5 = Unit.CreateNewUnit(chris, UnitType.Battler);
			battler5.CurrentTerritory = earth.NearOrbit;

			Hashlist battles = m_controller.FindBattles();


			Territory[] battleSites = {mercury.NearOrbit, mercury.NearOrbit, mercury.NearOrbit, mercuryOrbit1,
										  venus.NearOrbit, earth.NearOrbit, earth.NearOrbit, earth.NearOrbit, antarctica, 
										  arcologies, moscoviense,  mars.FarOrbit, transMarsOrbit15};
			BattleType[] battleTypes = {BattleType.KillerSatellite, BattleType.Bombing, BattleType.Normal, BattleType.Normal,
										   BattleType.Bombing, BattleType.Bombing, BattleType.Bombing, BattleType.Normal, 
										   BattleType.Normal, 
										   BattleType.Normal, BattleType.Normal, BattleType.Bombing, BattleType.Normal};
			//foreach(BattleInfo bi in battles)
			for(int i = 0; i < battles.Count; i++)
			{
				BattleInfo bi = (BattleInfo)battles[i];
				Console.WriteLine(bi.ToString());
				Assert.AreEqual(battleSites[i], bi.Territory);
				Assert.AreEqual(battleTypes[i], bi.Type);
			}
		}

		[Test]
		public void KillerSatelliteBattle()
		{
			FindBattles();

			//Unit leader = m_controller.GetPlayer("Stu").Units.GetUnits(UnitType.Leader)[0];
			//leader.CurrentTerritory = m_controller.Map["Near Mercury Orbit"];
			Hashlist battles = m_controller.Battles;

			BattleInfo bi = (BattleInfo)battles[0];

			UnitCollection units = bi.Territory.Units;
			UnitCollection satellites = units.GetUnits(UnitType.KillerSatellite);

			// Set up a predictable series of combat results
			// Next six numbers are 4, 8, 10, 2, 8, 8
			Utility.Twister.Initialize(42);
			CombatResult cr = m_battleController.DoCombat(bi);

			Assert.AreEqual(4, cr.Casualties.Count);
			Assert.AreEqual(2, cr.Survivors.Count);

			Assert.AreEqual(UnitType.Battler, cr.Survivors[0].UnitType);
			Assert.AreEqual(UnitType.Fighter, cr.Survivors[1].UnitType);

			Assert.AreEqual(UnitType.Fighter, cr.Casualties[0].UnitType);
			Assert.AreEqual(UnitType.Fighter, cr.Casualties[1].UnitType);
			Assert.AreEqual(UnitType.Fighter, cr.Casualties[2].UnitType);
			Assert.AreEqual(UnitType.Fighter, cr.Casualties[3].UnitType);
		}

		[Test]
		public void BombingAttacks()
		{
			//KillerSatelliteBattle();

			FindBattles();

			Hashlist battles = m_controller.Battles;

			Utility.Twister.Initialize(5);

			ArrayList results = new ArrayList();

			for(int i = 0; i < battles.Count; i++)
			{
				BattleInfo bi = (BattleInfo)battles[i];

				CombatResult cr = null;
				switch(bi.Type)
				{
					case BattleType.KillerSatellite:
					{
						cr = m_battleController.DoKillerSatelliteCombat(bi);
						results.Add(cr);
						break;
					}
					case BattleType.Bombing:
					{
						if(!m_controller.CheckForBombing(bi.Territory, bi.Player))
						{
							continue;
						}

						UnitCollection targets = m_controller.GetBombingTargets(bi.Territory, bi.Player);

						ArrayList territories = targets.GetUnitTerritories();

						// Give the user a chance to select his targets here

						CombatInfo ci = new CombatInfo();
						ci.Type = bi.Type;

						ci.Defenders.AddAllUnits(targets);

						UnitCollection battlers = bi.Territory.Units.GetUnits(UnitType.Battler);
						UnitCollection playerBattlers = battlers.GetUnits(bi.Player);

						ci.Attackers.AddAllUnits(playerBattlers);
						ci.AttackingLeader = false;

						cr = m_battleController.DoBombingCombat(ci);

						results.Add(cr);

						break;
					}
					case BattleType.Normal:
					{
						// Need to get the combatants in current turn order

						ArrayList combatants = new ArrayList();
						ArrayList al = bi.Territory.Units.GetPlayersWithUnits();
						
						CombatResult finalResult = new CombatResult();
						CombatResult turnResult = new CombatResult();

						foreach(Player p in m_controller.PlayerOrder)
						{
							if(al.Contains(p))
							{
								combatants.Add(p);
							}
						}




						break;
					}
				}
			}

			/*
			foreach(CombatResult cr in results)
			{
				int q = 42;
				int z = q;
			}
			*/
			
		}

		#endregion

		[Test]
		public void TestProduction()
		{
			Player hannah = m_controller.GetPlayer("Hannah");
			UnitCollection factories = hannah.Units.GetUnits(UnitType.Factory);
			Territory lowlanders = m_controller.Map["Lowlanders"];
			Territory beta = m_controller.Map["Beta Regio"];
			Territory orbit = m_controller.Map["Near Venus Orbit"];
			Territory wreckers = m_controller.Map["Wreckers"];

			Factory lf = (Factory)factories.GetUnits(lowlanders)[0];
			Factory bf = (Factory)factories.GetUnits(beta)[0];

			lf.StartProduction(UnitType.Trooper);
			bf.StartProduction(UnitType.Fighter);

			int startingTroopers = lowlanders.Units.GetUnits(UnitType.Trooper).Count;
			int startingFighters = beta.Units.GetUnits(UnitType.Fighter).Count;

			lf.ExecuteProduction();
			bf.ExecuteProduction();

			int currentTroopers = lowlanders.Units.GetUnits(UnitType.Trooper).Count;
			int currentFighters = beta.Units.GetUnits(UnitType.Fighter).Count;

			Assert.AreEqual(startingTroopers + 2, currentTroopers);
			Assert.AreEqual(startingFighters + 1, currentFighters);

			lf.StartProduction(UnitType.Battler, orbit);
			bf.StartProduction(UnitType.KillerSatellite, orbit);
	
			lf.ExecuteProduction();
			bf.ExecuteProduction();

			Assert.AreEqual(true, lf.UnitHalfProduced);
			Assert.AreEqual(true, bf.UnitHalfProduced);

			Assert.AreEqual(0, orbit.Units.GetUnits(UnitType.Battler).Count);
			Assert.AreEqual(0, orbit.Units.GetUnits(UnitType.KillerSatellite).Count);

			lf.ExecuteProduction();
			bf.ExecuteProduction();

			Assert.AreEqual(1, orbit.Units.GetUnits(UnitType.Battler).Count);
			Assert.AreEqual(1, orbit.Units.GetUnits(UnitType.KillerSatellite).Count);

			startingFighters = currentFighters;
			startingTroopers = currentTroopers;

			Factory.ProductionPerTurn = 4;

			lf.StartProduction(UnitType.Trooper);
			bf.StartProduction(UnitType.Fighter);

			lf.ExecuteProduction();
			bf.ExecuteProduction();

			currentTroopers = lowlanders.Units.GetUnits(UnitType.Trooper).Count;
			currentFighters = beta.Units.GetUnits(UnitType.Fighter).Count;

			Assert.AreEqual(startingTroopers + 4, currentTroopers);
			Assert.AreEqual(startingFighters + 2, currentFighters);
		}
	}
}