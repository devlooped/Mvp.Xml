using System;
using System.Xml;
using System.Globalization;

namespace Mvp.Xml
{
	/// <summary>
	/// A <see cref="XmlNameMatch"/> that only matches elements, optionally 
	/// only at the root of the document (<c>XmlReader.Depth == 0</c>).
	/// </summary>
	public class ElementMatch : XmlNameMatch
	{
		MatchMode mode = MatchMode.Default;

		/// <summary>
		/// Constructs the <see cref="ElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public ElementMatch(string name)
			: base(name)
		{
		}


		/// <summary>
		/// Constructs the <see cref="ElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public ElementMatch(string name, MatchMode mode)
			: base(name)
		{
			if (!Enum.IsDefined(typeof(MatchMode), mode))
				throw new ArgumentException(Properties.Resources.InvalidMode, "mode");
			this.mode = mode;
		}

		/// <summary>
		/// Constructs the <see cref="ElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public ElementMatch(string prefix, string name)
			: base(prefix, name)
		{
		}

		/// <summary>
		/// Constructs the <see cref="ElementMatch"/> with the given name and prefix.
		/// </summary>
		public ElementMatch(string prefix, string name, MatchMode mode)
			: base(prefix, name)
		{
			this.mode = mode;
		}

		public override bool Matches(XmlReader reader, IXmlNamespaceResolver resolver)
		{
			bool preCondition = false;

			switch (mode)
			{
				case MatchMode.StartElement:
					preCondition = reader.NodeType == XmlNodeType.Element;
					break;
				case MatchMode.StartElementClosed:
					if (reader.NodeType == XmlNodeType.Attribute)
					{
						string name = reader.LocalName;
						string ns = reader.NamespaceURI;
						if (reader.MoveToNextAttribute())
						{
							// If we moved, we didn't match and 
							// we restore the cursor.
							reader.MoveToAttribute(name, ns);
							preCondition = false;
						}
						else
						{
							// We need to evaluate the element name/ns match from 
							// the base here, moving to the element first
							reader.MoveToElement();
							preCondition = base.Matches(reader, resolver);
							// Restore cursor always.
							reader.MoveToAttribute(name, ns);
							return preCondition;
						}
					}
					else if (reader.NodeType == XmlNodeType.Element)
					{
						// Matches the same as StartElement if there are no attributes
						preCondition = !reader.HasAttributes;
					}
					break;
				case MatchMode.EndElement:
					preCondition = reader.NodeType == XmlNodeType.EndElement;
					break;
			}

			return preCondition && base.Matches(reader, resolver);
		}

		public MatchMode Mode
		{
			get { return mode; }
		}
	}
}
