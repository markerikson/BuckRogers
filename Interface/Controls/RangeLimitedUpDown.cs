using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace BuckRogers.Interface
{
	public class RangeLimitedUpDown : NumericUpDown
	{
		public RangeLimitedUpDown()
		{
			this.TextAlign = HorizontalAlignment.Center;
		}

		private bool limitRange = true;
		[Description("Forces typed values to stay within the Minimum and Maximum range."), Category("Behavior"), DefaultValue(true)]
		public bool LimitToRange
		{
			get
			{
				return limitRange;
			}
			set
			{
				limitRange = value;
			}
		}

		protected override void OnValidating(CancelEventArgs e)
		{
			if (LimitToRange) { this.Text = this.Value.ToString(); }
			base.OnValidating(e);
		}
	}
}
