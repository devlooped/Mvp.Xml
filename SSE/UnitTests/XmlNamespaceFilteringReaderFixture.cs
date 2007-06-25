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

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class XmlNamespaceFilteringReaderFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldFilterFromNamespace()
		{
			string xml = @"
				<payload>
					<sitrep:Subject xmlns:sitrep='http://www.mercycorps.org/afghanistan/sitrep'>Balkh team safe</sitrep:Subject> 
					<sitrep:Report xmlns:sitrep='http://www.mercycorps.org/afghanistan/sitrep'>The MercyCorps team in Balkh is now safe in the Mazaar-e-Sharif compound</sitrep:Report> 
					<georss:point xmlns:georss='http://www.georss.org/georss'>36.7 67.117</georss:point>
				</payload>";

			XmlReader payload = GetReader(xml);
			payload.MoveToContent();
			payload.Read();
			XmlNamespaceFilteringReader reader = new XmlNamespaceFilteringReader(
				payload,
				"http://www.mercycorps.org/afghanistan/sitrep");

			StringWriter sw = new StringWriter();
			XmlWriterSettings set = new XmlWriterSettings();
			set.ConformanceLevel = ConformanceLevel.Fragment;
			using (XmlWriter writer = XmlWriter.Create(sw, set))
			{
				while (!reader.EOF)
				{
					writer.WriteNode(reader, false);
				}
			}

			string expected = @"
					<sitrep:Subject xmlns:sitrep='http://www.mercycorps.org/afghanistan/sitrep'>Balkh team safe</sitrep:Subject> 
					<sitrep:Report xmlns:sitrep='http://www.mercycorps.org/afghanistan/sitrep'>The MercyCorps team in Balkh is now safe in the Mazaar-e-Sharif compound</sitrep:Report>";
			expected = NormalizeFormat(expected);
			string actual = NormalizeFormat(sw.ToString());

			WriteIfDebugging(expected);
			WriteIfDebugging(actual);

			Assert.AreEqual(expected, actual);
		}
	}
}
