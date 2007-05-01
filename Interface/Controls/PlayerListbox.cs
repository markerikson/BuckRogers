using System;
using System.Windows.Forms;
using System.Drawing;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for PlayerListbox.
	/// </summary>
	public class PlayerListBox : UnclickableListBox
	{
		private const int RECTCOLOR_LEFT = 4;
		private const int RECTCOLOR_TOP = 2;
		private const int RECTCOLOR_WIDTH = 20;
		private const int RECTTEXT_MARGIN = 10;
		private const int RECTTEXT_LEFT = RECTCOLOR_LEFT + RECTCOLOR_WIDTH + RECTTEXT_MARGIN;

		private bool m_showPlayerLocation;

		public bool ShowPlayerLocation
		{
			get { return m_showPlayerLocation; }
			set { m_showPlayerLocation = value; }
		}

		public PlayerListBox()
		{
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.SelectionMode = SelectionMode.One;
			
		}


		protected override void OnDrawItem(DrawItemEventArgs e) 
		{
			if(this.DesignMode)
			{
				base.OnDrawItem(e);
				return;
			}
			Graphics g = e.Graphics;
			Color blockColor = Color.Empty;
			Color textColor = Color.Black;
			int left = RECTCOLOR_LEFT;

			string text = "";

			if( (e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				using ( Brush b = new SolidBrush(System.Drawing.SystemColors.Highlight) )
				{
					// Fill background;
					e.Graphics.FillRectangle(b, e.Bounds);
				}					
				textColor = Color.White;				
			}

			if(e.Index == -1)
			{
				blockColor = BackColor;
			}
			else
			{
				Player p = (Player)this.Items[e.Index];
				blockColor = p.Color;

				if (m_showPlayerLocation)
				{
					string extraText;

					if (p.IsLocal)
					{
						extraText = " (L)";
					}
					else
					{
						extraText = " (R)";
					}

					text = p.Name + extraText;
				}
				else
				{
					text = p.Name;
				}
			}

			g.FillRectangle(new SolidBrush(blockColor),left, e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,
							ItemHeight - 2 * RECTCOLOR_TOP);

			g.DrawRectangle(Pens.Black,left,e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,ItemHeight - 2 * RECTCOLOR_TOP);

			g.DrawString(text,e.Font,new SolidBrush(textColor),
						new Rectangle(RECTTEXT_LEFT,e.Bounds.Top,e.Bounds.Width-RECTTEXT_LEFT,ItemHeight));

		}

		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged (e);
			Refresh();
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged (e);
			Refresh();
		}
	}
}
