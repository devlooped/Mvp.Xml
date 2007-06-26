using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	internal class XmlSharingWriter : XmlWrappingWriter
	{
		bool writeSSE = true;

		public XmlSharingWriter(XmlWriter baseWriter)
			: base(baseWriter)
		{
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement(prefix, localName, ns);

			if (writeSSE)
			{
				WriteAttributeString(
					XmlNamespaces.XmlNsPrefix, Schema.DefaultPrefix,
					XmlNamespaces.XmlNs, Schema.Namespace);

				writeSSE = false;
			}
		}
	}
}
