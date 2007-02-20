using System;
using System.Diagnostics;

using Mvp.Xml.XPointer;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Mvp.Xml.XPointer.Test
{
	/// <summary>
	/// Summary description for XPointerParserTests.
	/// </summary>
	[TestFixture]
	public class XPointerParserTests
	{        

        public XPointerParserTests() 
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
        }

        [Test]
        [ExpectedException(typeof(XPointerSyntaxException))]
        public void SyntaxErrorTest()
        {
            Pointer p = Pointer.Compile("too bad");			
        }

        [Test]
        public void ParenthesisTest() 
        {
            Pointer p = Pointer.Compile("xmlns(p=http://foo.com^))");
            p = Pointer.Compile("xmlns(p=http://foo.com^()");
        }
        
        [Test]
        public void EscapingCircumflexTest() 
        {
            Pointer p = Pointer.Compile("xmlns(p=http://foo.com^^)");            
        }

        [Test]
        [ExpectedException(typeof(XPointerSyntaxException))]
        public void CircumflexErrorTest() 
        {
            Pointer p = Pointer.Compile("xmlns(p=http://fo^o.com)");            
        }     
   
        [Test]
        [ExpectedException(typeof(XPointerSyntaxException))]
        public void BadNCName() 
        {
            Pointer p = Pointer.Compile("foo:bar");            
        }     

        [Test]
        [ExpectedException(typeof(XPointerSyntaxException))]
        public void BadElementPointer() 
        {
            Pointer p = Pointer.Compile("element(1/33/foo)");            
        } 
    
        [Test]       
        public void UnknownSchemePointer() 
        {
            Pointer p = Pointer.Compile("xpath1(/foo) foo(abr)");            
        } 
    }
}
