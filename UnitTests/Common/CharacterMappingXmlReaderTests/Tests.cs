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


namespace Mvp.Xml.Tests.CharacterMappingXmlReaderTests
{
    [TestClass]
    public class Tests
    {
        public static CharacterMappingXmlReader GetReader()
        {
            XmlReader baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style.xslt");
            return new CharacterMappingXmlReader(baseReader);
        }

        [TestMethod]
        public void TestReaderShoulReadCharMap()
        {
            CharacterMappingXmlReader r = GetReader();
            while (r.Read()) ;
            Assert.IsNotNull(r.CharacterMap);
            Assert.IsTrue(r.CharacterMap.ContainsMapping("testmap", '\u00A0'));
            Assert.IsTrue(r.CharacterMap.GetMapping("testmap", '\u00A0') == "&nbsp;");
        }
    }
}
