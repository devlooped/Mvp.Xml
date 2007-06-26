using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class XmlNamespaceFilteringReader : XmlWrappingReader
	{
		string includeNamespaceUri;

		public XmlNamespaceFilteringReader(XmlReader baseReader, string includeNamespaceUri)
			: base(baseReader)
		{
			this.includeNamespaceUri = includeNamespaceUri;
		}

		public override bool Read()
		{
			bool read = base.Read();

			if (!read) return false;

			while ((NodeType == XmlNodeType.Element ||
				NodeType == XmlNodeType.EndElement) &&
				NamespaceURI != includeNamespaceUri)
			{
				Skip();
			}

			return !this.EOF;
		}
	}
}
