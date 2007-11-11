using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Mvp.Xml.Template
{
	public static class KeyValueParser
	{
		static Regex AttributesExpression = new Regex(
			@"\s*(?<name>\w+(?=\W))\s*=\s*[""'](?<value>[^""']*)[""']",
			RegexOptions.Compiled | RegexOptions.Singleline);

		public static IDictionary<string, string> Parse(string attributeValues)
		{
			Guard.ArgumentNotNull(attributeValues, "attributeValues");

			// Directive attributes are case-insensitive. This is compatible with ASP.NET and DSL Tools T4 syntax.
			Dictionary<string, string> props = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

			for (Match attr = AttributesExpression.Match(attributeValues.Trim());
				attr.Success; attr = attr.NextMatch())
			{
				if (props.ContainsKey(attr.Groups["name"].Value))
				{
					throw new ArgumentException(String.Format(
						CultureInfo.CurrentCulture,
						Properties.Resources.Template_DuplicateAttribute,
						attr.Groups["name"].Value));
				}

				props.Add(
					attr.Groups["name"].Value,
					attr.Groups["value"].Value);
			}

			return props;
		}
	}
}
