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
    /// Collection of unit tests for EXSLT Random module functions.
    /// </summary>
    [TestFixture]
    public class ExsltRandomTests : ExsltUnitTests
    {        

        protected override string TestDir 
        {
            get { return "tests/EXSLT/Random/"; }
        }
        protected override string ResultsDir 
        {
            get { return "results/EXSLT/Random/"; }
        }   
                       
        /// <summary>
        /// Tests the following function:
        ///     random:random-sequence()
        /// </summary>
        [Test]
        public void RandomSequenceTest() 
        {
            RunAndCompare("source.xml", "random-sequence.xslt", "random-sequence.xml");
        }          
    }
}