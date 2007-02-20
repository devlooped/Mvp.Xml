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
    /// Collection of unit tests for EXSLT Common module functions.
    /// </summary>
    [TestFixture]
    public class ExsltCommonTests : ExsltUnitTests
    {        

        protected override string TestDir 
        {
            get { return "tests/EXSLT/Common/"; }
        }
        protected override string ResultsDir 
        {
            get { return "results/EXSLT/Common/"; }
        }   
                       
        /// <summary>
        /// Tests the following function:
        ///     exsl:node-set()
        /// </summary>
        [Test]
        public void NodeSetTest() 
        {
            RunAndCompare("source.xml", "node-set.xslt", "node-set.xml");
        }        

        /// <summary>
        /// Tests the following function:
        ///     exsl:object-type()
        /// </summary>
        [Test]
        public void ObjectTypeTest() 
        {
            RunAndCompare("source.xml", "object-type.xslt", "object-type.xml");
        }        
    }
}