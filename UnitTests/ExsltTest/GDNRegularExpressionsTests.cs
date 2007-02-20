using System;
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
    /// Collection of unit tests for GotDotNet RegularExpressions module functions.
    /// </summary>
    [TestFixture]
    public class GDNRegularExpressionsTests : ExsltUnitTests
    {        

        protected override string TestDir 
        {
            get { return "tests/GotDotNet/RegularExpressions/"; }
        }
        protected override string ResultsDir 
        {
            get { return "results/GotDotNet/RegularExpressions/"; }
        }   
                       
        /// <summary>
        /// Tests the following function:
        ///     regexp2:tokenize()
        /// </summary>
        [Test]
        public void TokenizeTest() 
        {
            RunAndCompare("source.xml", "tokenize.xslt", "tokenize.xml");
        }                    
    }
}
