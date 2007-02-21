using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.Common.XPath;
using System.IO;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Mvp.Xml.Tests.Common
{
	[TestClass]
	public class XPathDocumentHelperTests
	{
		[TestMethod]
		public void ShouldWriteRootElement()
		{
			XPathDocument doc = XPathDocumentHelper.CreateDocument();

			Assert.IsNotNull(doc);

			XmlWriter writer = XPathDocumentHelper.GetWriter(doc);
			writer.WriteElementString("hello", "world");
			writer.Close();

			XPathNavigator nav = doc.CreateNavigator();
			StringWriter sw = new StringWriter();
			XmlWriterSettings set = new XmlWriterSettings();
			set.OmitXmlDeclaration = true;
			using (XmlWriter w = XmlWriter.Create(sw, set))
			{
				nav.WriteSubtree(w);
			}

			Assert.AreEqual("<hello>world</hello>", sw.ToString());
		}
	}
}
