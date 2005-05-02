using System;
using UMD.HCIL.Piccolo;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Components;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for RefreshingScrollableControl.
	/// </summary>
	public class RefreshingScrollableControl : PScrollableControl
	{
		public RefreshingScrollableControl()
		{
		}

		public RefreshingScrollableControl(PCanvas view) 
			: base(view)
		{
		}

		protected override void hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			base.hScrollBar_ValueChanged (sender, e);
			Canvas.Refresh();
		}

		protected override void vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			base.vScrollBar_ValueChanged (sender, e);
			Canvas.Refresh();
		}


	}
}
