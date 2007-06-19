#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Data;

using Mvp.Xml.Common.Xsl;
using Mvp.Xml.Tests;

namespace Mvp.Xml.Tests.CharacterMappingXmlWriterTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestShouldReplaceInText()
        {
            Dictionary<char, string> mapping = new Dictionary<char, string>();
            mapping.Add('f', "FOO");
            StringWriter sw = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = false;
            CharacterMappingXmlWriter writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
            writer.WriteElementString("foo", "fgh");
            writer.Close();
            Assert.IsTrue(sw.ToString() == "<foo>FOOgh</foo>");
        }
    }
}
