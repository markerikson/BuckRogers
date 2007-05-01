using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace BuckRogers.Interface
{
	public partial class CombatPreviewForm : Form
	{
		private System.Timers.Timer m_timer;
		private bool m_finalCountdown;
		private int m_secondsRemaining;

		public CombatPreviewForm()
		{
			InitializeComponent();

			m_timer = new System.Timers.Timer();

			m_timer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);

			m_lvBattles.NoItemsMessage = "There are no battles this turn.";
		}
		
		private void m_btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		public void DisplayBattles(Hashlist battles)
		{
			m_lvBattles.Items.Clear();

			int battleNumber = 0;

			foreach(BattleInfo bi in battles)
			{
				ListViewItem lvi = new ListViewItem();

				battleNumber++;
				lvi.Text = battleNumber.ToString();

				lvi.SubItems.Add(bi.Territory.Name);
				lvi.SubItems.Add(bi.Territory.System.Name);
				lvi.SubItems.Add(bi.Type.ToString());

				StringBuilder sb = new StringBuilder();
				List<Player> playersInBattle = new List<Player>();

				if(bi.Player != Player.NONE)
				{
					playersInBattle.Add(bi.Player);
				}

				if(bi.Type == BattleType.KillerSatellite ||
					bi.Type == BattleType.Normal)
				{
					ArrayList players = bi.Territory.Units.GetPlayersWithUnits();

					foreach(Player p in players)
					{
						if(p == bi.Player)
						{
							continue;
						}

						playersInBattle.Add(p);
					}
				}

				bool firstPlayer = true;
				foreach(Player p in playersInBattle)
				{
					if(firstPlayer)
					{
						sb.Append(p.Name);
						firstPlayer = false;
					}
					else
					{
						sb.AppendFormat(", {0}", p.Name);
					}
				}
				lvi.SubItems.Add(sb.ToString());

				m_lvBattles.Items.Add(lvi);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Text = "Battles";

			m_timer.Interval = 10000;

			m_timer.Start();
		}

		void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if(m_finalCountdown)
			{
				if(m_secondsRemaining > 0)
				{
					string formatString;

					if(m_secondsRemaining == 1)
					{
						formatString = "Battles (closing in 1 second)";
					}
					else
					{
						formatString = "Battles (closing in {0} seconds)";
					}

					this.Invoke((MethodInvoker)delegate
					{
						this.Text = string.Format(formatString, m_secondsRemaining);
					});

					m_secondsRemaining--;
				}
				else
				{
					m_timer.Stop();

					this.Invoke((MethodInvoker)delegate
					{
						this.Text = "Battles";
						this.DialogResult = DialogResult.OK;
						this.Close();
					});

					
				}
				
			}
			else
			{
				m_timer.Stop();

				m_timer.Interval = 1000;
				m_finalCountdown = true;
				m_secondsRemaining = 5;

				m_timer.Start();
			}
		}


	}
}