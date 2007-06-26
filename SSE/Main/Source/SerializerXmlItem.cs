using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class SerializerXmlItem<TData> : DynamicXmlItem
	{
		static readonly XmlSerializer serializer = new XmlSerializer(typeof(TData));

		private TData data;

		public SerializerXmlItem(string id,
			string titleExpression,
			string descriptionExpression,
			DateTime timestamp,
			TData data)
			: base(id, titleExpression, descriptionExpression, timestamp, Serialize(data), data)
		{
			this.data = data;
		}

		private static XmlElement Serialize(TData data)
		{
			Guard.ArgumentNotNull(data, "data");

			MemoryStream mem = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(mem);
			NoXsiXsdWriter cleanWriter = new NoXsiXsdWriter(writer);
			cleanWriter.WriteStartElement("payload");
			serializer.Serialize(cleanWriter, data);
			cleanWriter.WriteEndElement();
			cleanWriter.Close();

			mem.Position = 0;

			XmlDocument doc = new XmlDocument();
			doc.Load(mem);
			return doc.DocumentElement;
		}

		public TData Data
		{
			get { return data; }
		}

		class NoXsiXsdWriter : XmlWrappingWriter
		{
			bool skip = false;

			public NoXsiXsdWriter(XmlWriter baseWriter) : base(baseWriter) { }

			public override void WriteStartAttribute(string prefix, string localName, string ns)
			{
				if (prefix == "xmlns" && (localName == "xsd" || localName == "xsi"))
				// Omits XSD and XSI declarations. 
				{
					skip = true;
					return;
				}

				base.WriteStartAttribute(prefix, localName, ns);
			}

			public override void WriteString(string text)
			{
				if (skip) return;

				base.WriteString(text);
			}

			public override void WriteEndAttribute()
			{
				if (skip)
				{
					// Reset the flag, so we keep writing. 
					skip = false;
					return;
				}

				base.WriteEndAttribute();
			}
		}
	}
}
