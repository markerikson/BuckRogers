using System;
using System.Collections.Generic;

namespace BuckRogers
{
	public enum StatusInfo
	{
		NextPlayer,
		NextPhase,
		FactoryConquered,
		SabotageResult,
		LeaderKilled,
		TransportLanded,
		PlayerKilled,
		GameOver,
		ActionAdded,
		ActionUndone,
		PlayersCreated,
		UpdateTerritory,
		BattleStatusUpdated,
	};
	/// <summary>
	/// Summary description for StatusUpdateEventArgs.
	/// </summary>
	public class StatusUpdateEventArgs : System.EventArgs
	{
		private Player m_player;
		//private Territory m_territory;
		private StatusInfo m_statusInfo;
		private Action m_action;
		private BattleStatus m_battleStatus;
		private List<Territory> m_territories;
		private CombatResult m_combatResult;
		private UnitCollection m_units;

		

		private bool m_result;
		private bool m_isLocal;		

		public StatusUpdateEventArgs()
		{
			m_territories = new List<Territory>();
			m_units = new UnitCollection();
		}

		public BuckRogers.Player Player
		{
			get { return this.m_player; }
			set { this.m_player = value; }
		}

		public BuckRogers.StatusInfo StatusInfo
		{
			get { return this.m_statusInfo; }
			set { this.m_statusInfo = value; }
		}

		/*
		public BuckRogers.Territory Territory
		{
			get { return this.m_territory; }
			set { this.m_territory = value; }
		}
		*/

		public Action Action
		{
			get { return m_action; }
			set { m_action = value; }
		}

		public BattleStatus BattleStatus
		{
			get { return m_battleStatus; }
			set { m_battleStatus = value; }
		}

		public List<Territory> Territories
		{
			get { return m_territories; }
			set { m_territories = value; }
		}

		public CombatResult CombatResult
		{
			get { return m_combatResult; }
			set { m_combatResult = value; }
		}

		public UnitCollection Units
		{
			get { return m_units; }
			set { m_units = value; }
		}

		public bool Result
		{
			get { return this.m_result; }
			set { this.m_result = value; }
		}

		public bool IsLocal
		{
			get { return m_isLocal; }
			set { m_isLocal = value; }
		}
	}
}
