using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for GameOptions.
	/// </summary>
	/// 
	
	
	public enum VictoryConditions
	{
		TotalAnnihilation, 
		LastLeaderLeft,
        NumberOfTerritories,
		OneEarthYear,
		ThreePlanets,
		EarthMoon,
		HomePlanetPlusTwo,
	}

	[Flags]
	public enum StartingScenarios
	{
		Normal = 0,
		NoPlanetaryMovement = 1,
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

		public GameOptions()
		{
			string[] optionShortNames = {"DifferentShipSpeeds", "DifferentTroopSpeeds", "PassingFire", "ControlMarkersFight",
											"CombatRetreat", "LimitedFactories", "DoubledProduction", "CombineFactories",
											"PartialPlanetControl", "SpecializedGennies", "MergeFarOrbits", "SlingshotEffect",
											"FactoryDefense", "DeployAnywhere", "TransportedFactoriesBuild", "FreePlanetaryFactory",
											"ConquerWithGround"};
			string[] optionDescriptions = {"Spaceships have different speeds", 
											  "Gennies and troopers have different speeds",
											  "Moving units can be fired at",
											  "Control markers must be killed to conquer the territory",
											  "Players can retreat from combat",
											  "Limited number of factories per planet",
											  "Double production after turn 3",
											  "Factories may combine output to produce faster",
											  "Players only need a majority of territories to own a planet",
											  "Gennies on different planets have different abilities",
											  "No cost to move from far orbit to the solar system",
											  "Ships can slingshot around planets or the Sun",
											  "Factories provide a defensive bonus",
											  "Newly produced units may be deployed anywhere",
											  "Factories in transports can build fighters",
											  "First player to own each planet gets a free factory",
											  "Only ground units can conquer planetary territories"};

			m_optionalRules = new OptionsHashlist();
			for(int i = 0; i < optionDescriptions.Length; i++)
			{
				m_optionalRules.Add(optionShortNames[i], new GameOption(optionShortNames[i],  false, optionDescriptions[i]));
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

		public BuckRogers.StartingScenarios StartingScenarios
		{
			get { return this.m_startingScenarios; }
			set { this.m_startingScenarios = value; }
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
