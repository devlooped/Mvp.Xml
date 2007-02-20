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
    /// Collection of unit tests for GotDotNet Dynamic module functions.
    /// </summary>
    [TestFixture]
    public class GDNDynamicTests : ExsltUnitTests
    {        
        protected override string TestDir 
        {
            get { return "tests/GotDotNet/Dynamic/"; }
        }
        protected override string ResultsDir 
        {
            get { return "results/GotDotNet/Dynamic/"; }
        } 
        
        /// <summary>
        /// Tests the following function:
        ///     dyn2:evaluate()
        /// </summary>
        [Test]
        public void EvaluateTest() 
        {
            RunAndCompare("source.xml", "evaluate.xslt", "evaluate.xml");
        }        
    }
}
