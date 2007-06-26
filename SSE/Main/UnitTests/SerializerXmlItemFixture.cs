#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class SerializerXmlItemFixture : TestFixtureBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowIfNullData()
		{
			new SerializerXmlItem<SerializableData>(Guid.NewGuid().ToString(), "title",
				"description", DateTime.Now, null);
		}

		[TestMethod]
		public void ShouldSerializePayloadWithoutDefaultXsiAttributes()
		{
			IXmlItem item = new SerializerXmlItem<SerializableData>(
				Guid.NewGuid().ToString(), "title", "description", DateTime.Now, new SerializableData());

			StringWriter sw = new StringWriter();
			XmlWriterSettings set = new XmlWriterSettings();
			set.OmitXmlDeclaration = true;
			set.CheckCharacters = true;
			using (XmlWriter w = XmlWriter.Create(sw, set))
			{
				w.WriteNode(new XmlNodeReader(item.Payload), false);
			}

			Assert.AreEqual("<payload><SerializableData /></payload>", sw.ToString());
		}

		[TestMethod]
		public void ShouldSerializePayloadWithNamespace()
		{
			IXmlItem item = new SerializerXmlItem<SerializableDataNs>(
				Guid.NewGuid().ToString(), "title", "description", DateTime.Now, new SerializableDataNs());

			StringWriter sw = new StringWriter();
			XmlWriterSettings set = new XmlWriterSettings();
			set.OmitXmlDeclaration = true;
			set.CheckCharacters = true;
			using (XmlWriter w = XmlWriter.Create(sw, set))
			{
				w.WriteNode(new XmlNodeReader(item.Payload), false);
			}

			string xml = sw.ToString();

			Assert.AreEqual("<payload><SerializableDataNs xmlns=\"mvp-xml\" /></payload>", xml);
		}

		public class SerializableData
		{
		}

		[XmlRoot(Namespace="mvp-xml")]
		public class SerializableDataNs
		{
		}
	}
}
