using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Mvp.Xml.TypedTemplate
{
	public class TemplateException : InvalidOperationException
	{
		public TemplateException(string message, int line, int column)
			: base(BuildMessage(message, line, column))
		{
		}

		private static string BuildMessage(string message, int line, int column)
		{
			Guard.ArgumentNotNull(message, "message");

			return String.Format(CultureInfo.CurrentCulture,
				Properties.Resources.Template_Exception,
				line,
				column,
				message);
		}
	}
}
