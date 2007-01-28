using System;
using System.ComponentModel;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for GameOptions.
	/// </summary>
	/// 
	
	
	public enum VictoryConditions
	{
        [Description("Total Annihilation")]
		TotalAnnihilation, 

        [Description("Last Leader Left")]
		LastLeaderLeft,

        [Description("Number of Territories")]
        NumberOfTerritories,

        [Description("One Earth Year")]
		OneEarthYear,

        [Description("Own Three Planets")]
		ThreePlanets,

        [Description("Own Earth, Moon, and Mars")]
		EarthMoonMars,

		HomePlanetPlusTwo,
	}

	[Flags]
	public enum StartingScenarios
	{
		[Browsable(false)]
		Normal = 0,
		[Description("Planets do not move")]
		NoPlanetaryMovement = 1,
		[Description("Players can pick their starting units")]
		PickStartingUnits = 2,
	}

	public class GameOptions
	{
		private OptionsHashlist m_optionalRules;

		private VictoryConditions m_victoryConditions;
		private StartingScenarios m_startingScenarios;

		private int m_productionMultiplier;
		private int m_increasedProductionTurn;
		private int m_numTerritoriesNeeded;
		private string[] m_playerNames;

		public GameOptions()
		{
            string[,] optionDetails = new string[,]
			{
				{"UseTestingSetup",				"Use the sample player setup and deployment"},
				{"LimitedTwoPlayerSetup",		"No extra units or territories in two/three player games"},
				{"ConquerWithGround",				"Only ground units can conquer planetary territories"},
                {"AllTerritoriesOwned",          "All ground territories have an owner at the start"},
				
				/*
				{"RandomTurnOrder",				"Turn order is completely random"},
				{"DifferentShipSpeeds",			"Spaceships have different speeds"},
				{"DifferentTroopSpeeds",			"Gennies and troopers have different speeds"},
				{"PassingFire",					"Moving units can be fired at"},
				{"ControlMarkersFight",			"Control markers must be killed to conquer the territory"},				
				{"CombatRetreat",					"Players can retreat from combat"},
				{"LimitedFactories",				"Limited number of factories per planet"},
				{"IncreasedProduction",			"Increased production after turn N"},
				{"CombineFactories",				"Factories may combine output to produce faster"},
				{"PartialPlanetControl",			"Players only need a majority of territories to own a planet"},
				{"SpecializedGennies",			"Gennies on different planets have different abilities"},
				{"MergeFarOrbits",				"No cost to move from far orbit to the solar system"},
				{"SlingshotEffect",				"Ships can slingshot around planets or the Sun",},
				{"FactoryDefense",				"Factories provide a defensive bonus",},
				{"DeployAnywhere",				"Newly produced units may be deployed anywhere",},
				{"TransportedFactoriesBuild",	"Factories in transports can build fighters"},
				{"FreePlanetaryFactory",			"First player to own each planet gets a free factory"},
				{"KillerAsteroids",				"Asteroids may be launched towards the sun"},
				*/
			};

			m_optionalRules = new OptionsHashlist();
			for(int i = 0; i <= optionDetails.GetUpperBound(0); i++)
			{
				m_optionalRules.Add(optionDetails[i, 0], new GameOption(optionDetails[i, 0],  false, optionDetails[i, 1]));
			}

			m_victoryConditions = VictoryConditions.TotalAnnihilation;
			m_startingScenarios = StartingScenarios.Normal;

			m_productionMultiplier = 1;
			m_increasedProductionTurn = 1;
		}


		public BuckRogers.VictoryConditions WinningConditions
		{
			get { return this.m_victoryConditions; }
			set { this.m_victoryConditions = value; }
		}

		public int NumTerritoriesNeeded
		{
			get { return this.m_numTerritoriesNeeded; }
			set { this.m_numTerritoriesNeeded = value; }
		}

		public int ProductionMultiplier
		{
			get { return this.m_productionMultiplier; }
			set { this.m_productionMultiplier = value; }
		}

		public int IncreasedProductionTurn
		{
			get { return this.m_increasedProductionTurn; }
			set { this.m_increasedProductionTurn = value; }
		}

		public BuckRogers.OptionsHashlist OptionalRules
		{
			get { return this.m_optionalRules; }
			set { this.m_optionalRules = value; }
		}

		public BuckRogers.StartingScenarios SetupOptions
		{
			get { return this.m_startingScenarios; }
			set { this.m_startingScenarios = value; }
		}

		public string[] PlayerNames
		{
			get { return this.m_playerNames; }
			set { this.m_playerNames = value; }
		}
	}

	public class GameOption
	{
		public GameOption(string name, bool val, string description)
		{
			m_name = name;
			m_description = description;
			m_value = val;
		}
		public string Description
		{
			get { return this.m_description; }
			set { this.m_description = value; }
		}

		public string Name
		{
			get { return this.m_name; }
			set { this.m_name = value; }
		}

		public bool Value
		{
			get { return this.m_value; }
			set { this.m_value = value; }
		}
	
		private string m_name;
		private bool m_value;
		private string m_description;
		
	}

}
