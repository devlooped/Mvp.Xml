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
    /// Collection of unit tests for GotDotNet Math module functions.
    /// </summary>
    [TestFixture]
    public class GDNMathTests : ExsltUnitTests
    {        
        protected override string TestDir 
        {
            get { return "tests/GotDotNet/Math/"; }
        }
        protected override string ResultsDir 
        {
            get { return "results/GotDotNet/Math/"; }
        }   
        
        /// <summary>
        /// Tests the following function:
        ///     math2:avg()
        /// </summary>
        [Test]
        public void AvgTest() 
        {
            RunAndCompare("source.xml", "avg.xslt", "avg.xml");
        }                 
    }
}