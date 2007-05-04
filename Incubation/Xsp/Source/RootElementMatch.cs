using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml
{
	/// <summary>
	/// A <see cref="ElementMatch"/> that only matches root elements 
	/// (<c>XmlReader.Depth == 0</c>).
	/// </summary>
	public class RootElementMatch : ElementMatch
	{
		/// <summary>
		/// Constructs the <see cref="RootElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public RootElementMatch(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Constructs the <see cref="RootElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public RootElementMatch(string name, MatchMode mode)
			: base(name, mode)
		{
		}

		/// <summary>
		/// Constructs the <see cref="RootElementMatch"/> with the given name and 
		/// and no prefix.
		/// </summary>
		public RootElementMatch(string prefix, string name)
			: base(prefix, name)
		{
		}

		/// <summary>
		/// Constructs the <see cref="RootElementMatch"/> with the given name and prefix.
		/// </summary>
		public RootElementMatch(string prefix, string name, MatchMode mode)
			: base(prefix, name, mode)
		{
		}

		public override bool Matches(XmlReader reader, IXmlNamespaceResolver resolver)
		{
			return reader.Depth == 0 && base.Matches(reader, resolver);
		}

	}
}
