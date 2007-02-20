using System;
using Mvp.Xml.Exslt;
using System.Xml;
using System.Xml.XPath;
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


namespace ExsltTest
{
    /// <summary>
    /// Superclass for unit tests.
    /// </summary>
    public class ExsltUnitTests
    {
        protected virtual string TestDir 
        {
            get { return "../../ExsltTest/tests/EXSLT/Common/"; }
        }
        protected virtual string ResultsDir 
        {
			get { return "../../ExsltTest/results/EXSLT/Common/"; }
        }

        protected void RunAndCompare(string source, string stylesheet, 
            string result) 
        {
            XPathDocument doc = new XPathDocument(TestDir + source);            
            ExsltTransform exslt = new ExsltTransform();
            exslt.Load(TestDir + stylesheet);
            StringWriter res = new StringWriter();
            exslt.Transform(doc, null, res);
            StreamReader sr = new StreamReader(ResultsDir + result);
            string expectedResult = sr.ReadToEnd();
            sr.Close();
			if (res.ToString() != expectedResult)
			{
				Console.WriteLine("Actual Result was {0}", res.ToString());
				Console.WriteLine("Expected Result was {0}", expectedResult);
			}
            Assert.IsTrue(res.ToString() == expectedResult);
        }        
    }
}
