using System;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.Common;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Mvp.Xml.Tests.XmlBaseAwareXmlTextReaderTests
{
	[TestClass]
	public class Tests
	{
        [TestMethod]
        public void BasicTest() 
        {
            XmlTextReader r = new XmlBaseAwareXmlTextReader("../../Common/XmlBaseAwareXmlTextReaderTests/test.xml");
            while (r.Read()) 
            {
                if (r.NodeType == XmlNodeType.Element) 
                {
                    switch (r.Name) 
                    {
                        case "catalog":
                            Assert.IsTrue(r.BaseURI.EndsWith("XmlBaseAwareXmlTextReaderTests/test.xml"));
                            break;
                        case "files":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/");
                            break;
                        case "file":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/");
                            break;
                        case "a":
							Assert.IsTrue(r.BaseURI.EndsWith("XmlBaseAwareXmlTextReaderTests/test.xml"));
                            break;
                        case "b":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/a/");
                            break;
                        case "c":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/c/");
                            break;
                        case "e":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/c/");
                            break;
                        case "d":
                            Assert.IsTrue(r.BaseURI == "file:///d:/Files/a/");
                            break;
                    }
                }
                else if (r.NodeType == XmlNodeType.Text && r.Value.Trim() != "") 
                {
                    Assert.IsTrue(r.BaseURI == "file:///d:/Files/c/");                    
                }
                else if (r.NodeType == XmlNodeType.Comment) 
                {
                    Assert.IsTrue(r.BaseURI == "file:///d:/Files/a/");                    
                }
                else if (r.NodeType == XmlNodeType.ProcessingInstruction) 
                {
                    Assert.IsTrue(r.BaseURI == "file:///d:/Files/");                    
                }
            }
            r.Close();
        }   

		[TestMethod]
		public void ReaderWithPath() 
		{
			XmlTextReader r = new XmlBaseAwareXmlTextReader("../../Common/XmlBaseAwareXmlTextReaderTests/relativeTest.xml");
			r.WhitespaceHandling = WhitespaceHandling.None;
			XPathDocument doc = new XPathDocument(r);
			XPathNavigator nav = doc.CreateNavigator();
			XPathNodeIterator ni = nav.Select("/catalog");
			ni.MoveNext();
			Assert.IsTrue(ni.Current.BaseURI.EndsWith("/XmlBaseAwareXmlTextReaderTests/relativeTest.xml"));
			ni = nav.Select("/catalog/relative/relativenode");
			ni.MoveNext();
			Console.WriteLine(ni.Current.BaseURI);
			Assert.IsTrue(ni.Current.BaseURI.IndexOf("/XmlBaseAwareXmlTextReaderTests/") != -1);
		}   

	}
}
