using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Mvp.Xml
{
	/// <summary>
	/// Helper class that parses expressions into a corresponding 
	/// list of <see cref="XmlMatch"/>. Supports a subset of XPath 1.0.
	/// </summary>
	/// <remarks>
	/// Path expressions can contain
	/// <a href="http://www.w3.org/TR/xpath#NT-RelativeLocationPath">relative</a> or 
	/// <a href="http://www.w3.org/TR/xpath#NT-AbsoluteLocationPath">absolute</a> location paths.
	/// <para>
	/// The only supported <a href="http://www.w3.org/TR/xpath#node-tests">node test</a> in a 
	/// location path is a <a href="http://www.w3.org/TR/xpath#NT-NameTest">name test</a> with the 
	/// following slight change to allow for more flexible tests:
	/// <code>
	/// 1. NameTest ::= '*'	
	/// 2. 			| NCName ':' '*'	
	/// 3. 			| QName
	/// 4. 			| '*' ':' NCName
	/// 5. 			| '*' ':' '*'
	/// </code>
	/// The new entries in addition to lines 1-3 from the XPath specification mean:
	/// 4. Match the given NCName matches with any prefix (including no prefix)
	/// 5. Match any element in any namespace. Equal to 1.
	/// <para>
	/// Being able to match elements by name regardless of their namespace is valuable 
	/// in scenarios where only elements in a known namespace are expected, it's a 
	/// controlled vocabulary and the input is also controlled.
	/// </para>
	/// </para>
	/// The following list shows the supported, which must be used in their abbreviated form:
	/// <list type="table">
	/// <listheader>
	/// <term>XPath Axes</term>
	/// <description>Syntax Example</description>
	/// </listheader>
	/// <item>
	/// <term>child (/)</term>
	/// <description><c>item/title</c>: the <c>title</c> child element of <c>item</c>.</description>
	/// </item>
	/// <item>
	/// <term>attribute (@)</term>
	/// <description><c>item/@id</c>: the <c>id</c> attribute of <c>item</c> element.</description>
	/// </item>
	/// <item>
	/// <term>descendant-or-self (//)</term>
	/// <description><c>//item</c>: the <c>item</c> element in the current node or any descendent.
	/// This axis is only valid at the beginning of the expression.
	/// </description>
	/// </item>
	/// </list>
	/// Predicates are not supported.
	/// </remarks>
	/// <example>
	/// Some valid sample expressions are:
	/// <list type="table">
	/// <listheader>
	/// <term>Expression</term>
	/// <description>Description</description>
	/// </listheader>
	/// <item>
	/// <term>*/@id</term>
	/// <description>All <c>id</c> attributes in the document.</description>
	/// </item>
	/// <item>
	/// <term>*:data/*:item</term>
	/// <description>All elements named <c>item</c> that are children of 
	/// <c>data</c>, regardless of their namespace.</description>
	/// </item>
	/// <item>
	/// <term>//item</term>
	/// <description>All <c>item</c> elements in the document which are in the empty namespace.</description>
	/// </item>
	/// </list>
	/// </example>
	public class PathExpressionParser
	{
		public static List<XmlMatch> Parse(string expression)
		{
			return Parse(expression, MatchMode.StartElement);
		}

		public static List<XmlMatch> Parse(string expression, MatchMode mode)
		{
			Guard.ArgumentNotNullOrEmptyString(expression, "expression");

			bool isRoot = false;

			List<XmlMatch> names = new List<XmlMatch>();
			string normalized = expression;
			if (normalized.StartsWith("//", StringComparison.Ordinal))
			{
				normalized = normalized.Substring(2);
			}
			else if (normalized.StartsWith("/", StringComparison.Ordinal))
			{
				isRoot = true;
				normalized = normalized.Substring(1);
			}

			string[] paths = normalized.Split('/');

			try
			{
				for (int i = 0; i < paths.Length; i++)
				{
					string path = paths[i];
					if (path.Length == 0)
						ThrowInvalidPath(expression);

					// Attribute match can only be the last in the expression.
					if (path.StartsWith("@", StringComparison.Ordinal) && 
						i != paths.Length - 1)
					{
						throw new ArgumentException(String.Format(
							CultureInfo.CurrentCulture, 
							Properties.Resources.AttributeAxisInvalid, 
							expression));
					}

					XmlMatch match;

					string[] xmlName = path.Split(':');
					string prefix, name;

					if (xmlName.Length == 2)
					{
						prefix = xmlName[0];
						name = xmlName[1];

						if (prefix.Length == 0)
							ThrowInvalidPath(expression);
						else if (name.Length == 0)
							ThrowInvalidPath(expression);
					}
					else
					{
						name = xmlName[0];
						prefix = String.Empty;
					}

					match = CreateMatch(
						isRoot && i == 0,
						/* Only pass the actual match mode when we're at the last segment */
						(i == paths.Length - 1) ? mode : MatchMode.StartElement,
						prefix, name);

					if (match is AttributeMatch && names.Count > 0)
					{
						// Build a composite that matches element with the given attribute.
						ElementMatch parent = names[names.Count - 1] as ElementMatch;

						if (mode == MatchMode.EndElement)
							throw new ArgumentException(Properties.Resources.AttributeMatchCannotBeEndElement);
						
						names.RemoveAt(names.Count - 1);
						names.Add(new ElementAttributeMatch(parent, (AttributeMatch)match));
					}
					else
					{
						names.Add(match);
					}
				}
			}
			catch (ArgumentException aex)
			{
				throw new ArgumentException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.InvalidPath,
					expression), aex);
			}


			return names;
		}

		private static XmlMatch CreateMatch(bool isRootMatch, MatchMode mode, string prefix, string name)
		{
			if (name.StartsWith("@", StringComparison.Ordinal))
			{
				return new AttributeMatch(prefix, name.Substring(1));
			}
			else if (prefix.StartsWith("@", StringComparison.Ordinal))
			{
				return new AttributeMatch(prefix.Substring(1), name);
			}
			else
			{
				if (isRootMatch)
				{
					return new RootElementMatch(prefix, name, mode);
				}
				else
				{
					return new ElementMatch(prefix, name, mode);
				}
			}
		}

		private static void ThrowInvalidPath(string path)
		{
			throw new ArgumentException(String.Format(
				CultureInfo.CurrentCulture,
				Properties.Resources.InvalidPath,
				path));
		}
	}
}
