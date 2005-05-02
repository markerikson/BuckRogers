using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace EeekSoft.WinForms.Controls
{
	/// <summary>
	/// Type of enumeration editor
	/// </summary>
	public enum EnumEditorType
	{
		#region Members

		/// <summary>Automaticly detect type of control (using FlagsAttribute)</summary>
		Auto,
		/// <summary>Allow combination of more flags (uses checkbox)</summary>
		Flags,
		/// <summary>Allow only one selected item (radiobutton)</summary>
		Options

		#endregion
	}


	/// <summary>
	/// WinForms enumeration editor control
	/// </summary>
	public class EnumEditor : System.Windows.Forms.UserControl
	{
		#region Local variables
		
		System.Type _type;
		long _value=0;
		EnumEditorType _edType=EnumEditorType.Auto;
		string _lblFormat="{0}";
		int _ctrlSpacing=32;

		#endregion

		#region Public properties

		/// <summary>
		/// Type of enumeration to edit
		/// </summary>
		/// <example>
		/// <code>enumEditor1.EnumType=typeof(MyEnum);</code>
		/// </example>
		[Description("Type of enumeration to edit - cannot be set in design mode!"),
		Category("Enum editor")]
		public System.Type EnumType
		{
			get 
			{
				return _type;
			}
			set
			{
				_type=value;
				Controls.Clear();
				if (_type!=null)
					AddEnumControls();
			}
		}


		/// <summary>
		/// Value of enumeration converted to integer
		/// </summary>
		/// <example>
		/// <code>
		/// MyEnum me=MyEnum.First;
		/// 
		/// // Set value
		/// enumEditor1.EnumValue=(long)me;
		/// 
		/// // Get value
		/// me=(MyEnum)enumEditor.EnumValue;
		/// </code>
		/// </example>
		[Description("Value of enumeration converted to integer"),
		DefaultValue(0),Category("Enum editor")]		public long EnumValue
		{
			get 
			{
				return _value;
			}
			set
			{
				_value=value;
				foreach(ButtonBase btn in Controls)
				{
					if (FlagsEditor)
					{
						((CheckBox)btn).Checked=(_value&(long)btn.Tag)!=0;
					}
					else
					{
						if (_value==(long)btn.Tag) 
							((RadioButton)btn).Checked=true;
					}
				}
			}
		}


		/// <summary>
		/// Gets value specifiing whether editor mode is set to Flags
		/// </summary>
		[Browsable(false)]
		public bool FlagsEditor
		{
			get
			{
				bool bChecks=(EnumEditorType.Flags==_edType);
				if (_edType==EnumEditorType.Auto)
				{
					bChecks=(_type.GetCustomAttributes(typeof(FlagsAttribute),false).Length>0);
				}
				return bChecks;
			}
		}

		/// <summary>
		/// Format of checkbox or radiobutton label. Use {0} for name of field 
		/// and {1} for its description (added by DescriptionAttribute)
		/// </summary>
		/// <remarks>Default value is {0}</remarks>
		[DefaultValue("{0}{1}"),Description("Format of checkbox or radiobutton label. "+
			"Use {0} for name of field and {1} for its description (added by DescriptionAttribute)")]
		public string LableFormat
		{
			get { return _lblFormat; }
			set { _lblFormat=value; }
		}


		/// <summary>
		/// Type of editor (Flags editor/Options editor)
		/// <seealso cref="EnumEditorType"/>
		/// </summary>
		[DefaultValue(EnumEditorType.Auto),Category("Enum editor"),
		Description("Type of editor (Flags editor/Options editor)")]
		public EnumEditorType EditorType
		{
			get { return _edType; }
			set { _edType=value; }
		}


		/// <summary>
		/// Space in pixels between each two generated controls (checkboxes/radiobuttons)
		/// </summary>
		[DefaultValue(32),Category("Enum editor"),
			Description("Space in pixels between each two generated controls (checkboxes/radiobuttons)")]
		public int ControlSpacing
		{
			get { return _ctrlSpacing; }
			set { _ctrlSpacing=value; }
		}

		#endregion
		#region Events

		/// <summary>
		/// Is raised after postback when user changed enumeration value
		/// </summary>
		[Category("Action"),
		Description("Is raised after postback when user changed enumeration value")]
		public event EventHandler Change;

		#endregion

		#region Implementation


		/// <summary>
		/// Helper: Get long value from object of unknown integer data type
		/// </summary>
		/// <param name="val">Any integer: int/long/byte.. etc..</param>
		/// <returns>Value converted to long</returns>
		private long GetLong(object val)
		{
			try { return (int)val;	} catch {}
			try { return (long)val;	} catch {}
			try { return (byte)val;	} catch {}
			try { return (short)val;	} catch {}
			try { return (int)val;	} catch {}
			try { return (long)((ulong)val);	} catch {}
			try { return (byte)val;	} catch {}
			try { return (short)val;	} catch {}
			throw new Exception("Invalid value.");
		}


		/// <summary>
		/// Generate web controls for editing
		/// </summary>
		private void AddEnumControls()
		{
			// get fields
			int y=0;
			FieldInfo[] fields=_type.GetFields();
			foreach(FieldInfo field in fields)
			{
				if (field.IsSpecialName) continue;

				// skip field?
				object[] attrs;
				attrs=field.GetCustomAttributes(typeof(BrowsableAttribute),false);
				if (attrs.Length>0&&((BrowsableAttribute)attrs[0]).Browsable==false) continue;

				// get description & value
				string description="";
				attrs=field.GetCustomAttributes(typeof(DescriptionAttribute),false);
				if (attrs.Length>0) description=((DescriptionAttribute)attrs[0]).Description;

				long val=GetLong(field.GetValue(0));

				// add radiobutton/checkbox
				ButtonBase ctrl;
				if (FlagsEditor)
				{
					CheckBox btn=new CheckBox();
					ctrl=btn;
					if ((val&_value)>0)
						btn.Checked=true;				
					ctrl.Click+=new EventHandler(checkBox_Click);
				}
				else
				{
					RadioButton btn=new RadioButton();
					ctrl=btn;
					if (val==_value)
						btn.Checked=true;
					ctrl.Click+=new EventHandler(radioButton_Click);
				}

				// set other properties & add control
				ctrl.Tag=val;
				ctrl.Text=string.Format(_lblFormat,field.Name,description);

				ctrl.Top=y;
				ctrl.Width=Width;
				ctrl.Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right;
				Controls.Add(ctrl);

				y+=ControlSpacing==0?ctrl.Height:ControlSpacing;
			}
		}


		/// <summary>
		/// Value of radiobutton changed - update enum value
		/// </summary>
		private void radioButton_Click(object sender, EventArgs e)
		{
			long old=EnumValue;
      _value=(long)((RadioButton)sender).Tag;
			
			if (EnumValue!=old) OnChange();
		}


		/// <summary>
		/// Value of checkbox changed - update enum value
		/// </summary>
		private void checkBox_Click(object sender, EventArgs e)
		{
			long old=EnumValue;
			if (((CheckBox)sender).Checked)
				_value|=(long)((CheckBox)sender).Tag;
			else
				_value&=~(long)((CheckBox)sender).Tag;
			
			if (EnumValue!=old) OnChange();
		}


		/// <summary>
		/// Raise change event
		/// </summary>
		protected virtual void OnChange()
		{
			if (Change!=null)
				Change(this,EventArgs.Empty);
		}

		#endregion
	}
}
