using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	class EmptyMessageListView : ListView
	{
		public EmptyMessageListView()
		{

			SetStyle(ControlStyles.ResizeRedraw, true);
		}
		 
		private bool _gridLines = false;
		/// 
		/// Handle GridLines on our own because base.GridLines has to be switched on
		/// and off depending on the amount of items in the ListView.
		/// 
		[DefaultValue(false)]
		public new bool GridLines
		{
			get {  return _gridLines; }
			set { _gridLines = value; Invalidate(); }
		}
		 
		private string _noItemsMessage = "There are no items to show in this view";
		/// 
		/// To be able to localize the message it must not be hardcoded
		/// 
		[DefaultValue("There are no items to show in this view")]
		public string NoItemsMessage
		{
			get {  return _noItemsMessage; }
			set { _noItemsMessage = value; Invalidate(); }    
		}
		 
		const int WM_ERASEBKGND = 0x14;
		protected override void WndProc(ref Message m)
		{
			base.WndProc (ref m);
		 
			if (m.Msg == WM_ERASEBKGND)
			{
				#region Handle drawing of "no items" message
				if (Items.Count==0 && Columns.Count>0)
				{
					if (this.GridLines)
					{
						base.GridLines = false;
					}
		 
					int w = 0;
					foreach (ColumnHeader h in this.Columns)
						w += h.Width;
		 
					StringFormat sf = new StringFormat();
					sf.Alignment = StringAlignment.Center;
		 
					Rectangle rc = new Rectangle(0, (int)(this.Font.Height * 3), w, this.Height);
		 
					using (Graphics g = this.CreateGraphics())
					{
						g.FillRectangle(SystemBrushes.Window, 0, 0, this.Width, this.Height);
						g.DrawString(NoItemsMessage, this.Font, SystemBrushes.ControlText, rc, sf);
					}
				} 
				else
				{
					base.GridLines = this.GridLines;
				}
				#endregion
			}
		}
	}
}
