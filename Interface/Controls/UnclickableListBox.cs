using System;
using System.Windows.Forms;
using System.Drawing;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for PlayerListbox.
	/// </summary>
	public class UnclickableListBox : ListBox
	{

		public UnclickableListBox()
		{
			this.SelectionMode = SelectionMode.One;			
		}

		const int WM_LBUTTONDOWN = 0x0201; 
  
		protected override void WndProc( ref Message m ) 
		{ 
			if ( m.Msg != WM_LBUTTONDOWN ) 
				base.WndProc( ref m ); 
		} 
	}
}
