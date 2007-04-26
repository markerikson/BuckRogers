using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;

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

	/*
	[Flags]
	public enum StartingScenarios
	{
		[Browsable(false)]
		Normal = 0,

		[Description("Planets do not move")]
		NoPlanetaryMovement = 1,

		[Description("Players can pick their starting units")]
		PickStartingUnits = 2,

		[Description("Players can pick their starting territories")]
		PickStartingTerritories = 4,
	}
	*/

	public class GameOptions
	{
		private OptionsHashlist m_optionalRules;
		private Hashlist m_categories;



		private VictoryConditions m_victoryConditions;
		//private StartingScenarios m_startingScenarios;

		private int m_numPlayers;
		
		private int m_productionMultiplier;
		private int m_increasedProductionTurn;
		private int m_numTerritoriesNeeded;
		private string[] m_playerNames;

		private bool m_isNetworkGame;

		public bool IsNetworkGame
		{
			get { return m_isNetworkGame; }
			set { m_isNetworkGame = value; }
		}

		public Hashlist Categories
		{
			get { return m_categories; }
			set { m_categories = value; }
		}

		public int NumPlayers
		{
			get { return m_numPlayers; }
			set { m_numPlayers = value; }
		}

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
			m_categories = new Hashlist();


			/*
			for(int i = 0; i <= optionDetails.GetUpperBound(0); i++)
			{
				m_optionalRules.Add(optionDetails[i, 0], new GameOption(optionDetails[i, 0], optionDetails[i, 1],  false));
			}
			*/

			LoadOptionsXML();
			m_victoryConditions = VictoryConditions.TotalAnnihilation;
			//m_startingScenarios = StartingScenarios.Normal;

			m_productionMultiplier = 1;
			m_increasedProductionTurn = 1;
		}

		private void LoadOptionsXML()
		{
			Assembly a = Assembly.GetExecutingAssembly();

			string resourceName = "BuckRogers.Engine.OptionalRules.xml";
			Stream stream = a.GetManifestResourceStream(resourceName);

			XmlDocument xd = new XmlDocument();
			xd.Load(stream);

			XmlElement xeOptionalRules = (XmlElement)xd.GetElementsByTagName("OptionalRules")[0];

			XmlNodeList xnlCategories = xeOptionalRules.GetElementsByTagName("Category");

			foreach (XmlElement xeCategory in xnlCategories)
			{
				string categoryName = xeCategory.Attributes["name"].Value;
				//Hashlist categoryList = new Hashlist();
				OptionCategory category = new OptionCategory();
				category.Name = categoryName;
				m_categories.Add(categoryName, category);

				XmlNodeList xnlOptions = xeCategory.GetElementsByTagName("OptionalRule");

				foreach (XmlElement xeOption in xnlOptions)
				{
					string optionName = xeOption.Attributes["name"].Value;
					string optionDescription = xeOption.Attributes["description"].Value;

					GameOption option = new GameOption(optionName, optionDescription, false);
					option.Category = categoryName;
					m_optionalRules.Add(optionName, option);
					category.Options.Add(optionName, option);

					if(xeOption.Attributes["excludes"] != null)
					{
						option.Excludes = xeOption.Attributes["excludes"].Value;
					}

					XmlNodeList xnlValues = xeOption.GetElementsByTagName("Value");

					foreach(XmlElement xeValue in xnlValues)
					{
						string valueName = xeValue.Attributes["name"].Value;
						string valueDescription = xeValue.Attributes["description"].Value;
						
						string sMin = xeValue.Attributes["min"].Value;
						string sMax = xeValue.Attributes["max"].Value;
						string sStart = xeValue.Attributes["start"].Value;
						
						OptionValue ov = new OptionValue();
						ov.Name = valueName;
						ov.Description = valueDescription;

						ov.Min = int.Parse(sMin);
						ov.Max = int.Parse(sMax);
						ov.Start = int.Parse(sStart);


						option.Values.Add(valueName, ov);
					}
				}
			}


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

		/*
		public BuckRogers.StartingScenarios SetupOptions
		{
			get { return this.m_startingScenarios; }
			set { this.m_startingScenarios = value; }
		}
		*/

		public string[] PlayerNames
		{
			get { return this.m_playerNames; }
			set { this.m_playerNames = value; }
		}
	}

	public class GameOption
	{
		private string m_name;
		private bool m_value;
		private string m_description;
		private string m_category;
		private Hashlist m_values;
		private string m_excludes;

		public GameOption(string name, string description, bool val)
		{
			m_name = name;
			m_description = description;
			m_value = val;
			m_values = new Hashlist();
			m_excludes = string.Empty;
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

		public string Category
		{
			get { return m_category; }
			set { m_category = value; }
		}

		public Hashlist Values
		{
			get { return m_values; }
			set { m_values = value; }
		}

		public string Excludes
		{
			get { return m_excludes; }
			set { m_excludes = value; }
		}
	}

	public class OptionCategory
	{
		private string m_name;
		private Hashlist m_options;

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}		

		public Hashlist Options
		{
			get { return m_options; }
			set { m_options = value; }
		}

		public OptionCategory()
		{
			m_options = new Hashlist();
		}
	};

	public class OptionValue
	{
		private int m_value;
		private string m_name;
		private string m_description;
		private int m_min;
		private int m_start;
		private int m_max;

		public int Min
		{
			get { return m_min; }
			set { m_min = value; }
		}		

		public int Max
		{
			get { return m_max; }
			set { m_max = value; }
		}		

		public int Start
		{
			get { return m_start; }
			set { m_start = value; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}		

		public string Description
		{
			get { return m_description; }
			set { m_description = value; }
		}		

		public int Value
		{
			get { return m_value; }
			set { m_value = value; }
		}
	};
}
