using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace BuckRogers.Interface
{
	public class InterfaceUtility
	{
		public static Icon GetApplicationIcon()
		{
			Assembly asm = Assembly.GetCallingAssembly();
			string resourceName = "BuckRogers.Interface.Other.Graphics.bucklogo.ico";
			Stream stream = asm.GetManifestResourceStream(resourceName);

			Icon icon = new Icon(stream);

			return icon;			
		}
	}

}