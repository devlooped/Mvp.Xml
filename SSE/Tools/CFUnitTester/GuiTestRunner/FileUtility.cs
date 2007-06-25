using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Microsoft.Practices.Mobile.GuiTestRunner
{
	class FileUtility
	{
		private FileUtility()
		{
		}

		public static string ExePath
		{
			get
			{
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
			}
		}
	}
}
