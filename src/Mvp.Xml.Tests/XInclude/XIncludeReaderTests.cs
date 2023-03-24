using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using Xunit;

namespace Mvp.Xml.XInclude.Test;

/// <summary>
/// XIncludeReader general tests.
/// </summary>

public class XIncludeReaderTests
{
    public XIncludeReaderTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result)
    {
        RunAndCompare(source, result, false);
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result, bool textAsCDATA)
    {
        RunAndCompare(source, result, textAsCDATA, null);
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result, bool textAsCDATA, XmlResolver resolver)
    {
        var doc = new XmlDocument();
        doc.PreserveWhitespace = true;
        var xir = new XIncludingReader(source);
        if (resolver != null)
            xir.XmlResolver = resolver;
        xir.ExposeTextInclusionsAsCDATA = textAsCDATA;
        //            while (xir.Read()) 
        //            {
        //                Console.WriteLine("{0} | {1} | {2} | {3}", xir.NodeType, xir.Name, xir.Value, xir.IsEmptyElement);                
        //            }
        //            throw new Exception();
        try
        {
            doc.Load(xir);
        }
        catch (Exception)
        {
            xir.Close();
            throw;
        }
        xir.Close();
        var s = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse,
            IgnoreWhitespace = true
        };
        //s.ProhibitDtd = false;
        var r1 = XmlReader.Create(new StringReader(doc.OuterXml), s, doc.BaseURI);
        var r2 = XmlReader.Create(result, s);
        try
        {
            while (r1.Read())
            {
                Assert.True(r2.Read());
                while (r1.NodeType == XmlNodeType.XmlDeclaration ||
                    r1.NodeType == XmlNodeType.Whitespace)
                    r1.Read();
                while (r2.NodeType == XmlNodeType.XmlDeclaration ||
                    r2.NodeType == XmlNodeType.Whitespace)
                    r2.Read();
                Assert.Equal(r1.XmlLang, r2.XmlLang);
                switch (r1.NodeType)
                {
                    case XmlNodeType.Attribute:
                        Assert.Equal(XmlNodeType.Attribute, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.LocalName, r2.LocalName);
                        Assert.Equal(r1.NamespaceURI, r2.NamespaceURI);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.CDATA:
                        Assert.True(r2.NodeType == XmlNodeType.CDATA || r2.NodeType == XmlNodeType.Text);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.Comment:
                        Assert.Equal(XmlNodeType.Comment, r2.NodeType);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.DocumentType:
                        Assert.Equal(XmlNodeType.DocumentType, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        //Ok, don't compare DTD content
                        //Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.Element:
                        Assert.Equal(XmlNodeType.Element, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.LocalName, r2.LocalName);
                        Assert.Equal(r1.NamespaceURI, r2.NamespaceURI);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.Entity:
                        Assert.Equal(XmlNodeType.Entity, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.EndElement:
                        Assert.Equal(XmlNodeType.EndElement, r2.NodeType);
                        break;
                    case XmlNodeType.EntityReference:
                        Assert.Equal(XmlNodeType.EntityReference, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.Notation:
                        Assert.Equal(XmlNodeType.Notation, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        Assert.Equal(XmlNodeType.ProcessingInstruction, r2.NodeType);
                        Assert.Equal(r1.Name, r2.Name);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.SignificantWhitespace:
                        Assert.Equal(XmlNodeType.SignificantWhitespace, r2.NodeType);
                        Assert.Equal(r1.Value, r2.Value);
                        break;
                    case XmlNodeType.Text:
                        Assert.True(r2.NodeType == XmlNodeType.CDATA || r2.NodeType == XmlNodeType.Text);
                        Assert.Equal(r1.Value.Replace("\r\n", "\n").Replace("\r", "").Trim(), r2.Value.Replace("\r\n", "\n").Replace("\r", "").Trim());
                        break;
                    default:
                        break;
                }
            }
            Assert.False(r2.Read());
            Assert.True(r1.ReadState == ReadState.EndOfFile || r1.ReadState == ReadState.Closed);
            Assert.True(r2.ReadState == ReadState.EndOfFile || r2.ReadState == ReadState.Closed);
        }
        catch (Exception)
        {
            r1.Close();
            r1 = null;
            r2.Close();
            r2 = null;
            ReportResults(result, doc);
            throw;
        }
        finally
        {
            if (r1 != null)
                r1.Close();
            if (r2 != null)
                r2.Close();
        }
        ReportResults(result, doc);
    }

    static void ReportResults(string expected, XmlDocument actual)
    {
        var sr = new StreamReader(expected);
        var expectedResult = sr.ReadToEnd();
        sr.Close();
        var ms = new MemoryStream();
        actual.Save(new StreamWriter(ms, Encoding.UTF8));
        ms.Position = 0;
        var actualResult = new StreamReader(ms).ReadToEnd();
        Console.WriteLine("\n-----------Expected result:-----------\n{0}", expectedResult);
        Console.WriteLine("-----------Actual result:-----------\n{0}", actualResult);
    }

    /// <summary>
    /// General test - it should work actually.
    /// </summary>
    [Fact]
    public void ItWorksAtLeast()
    {
        RunAndCompare("../../XInclude/tests/document.xml", "../../XInclude/results/document.xml");
    }


    /// <summary>
    /// Non XML character in the included document.
    /// </summary>
    [Fact]
    public void NonXMLChar()
    {
        Assert.Throws<NonXmlCharacterException>(() =>
            RunAndCompare("../../XInclude/tests/nonxmlchar.xml", "../../XInclude/results/nonxmlchar.xml"));
    }

    /// <summary>
    /// File not found and no fallback.
    /// </summary>
    [Fact]
    public void FileNotFound()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("../../XInclude/tests/filenotfound.xml", "../../XInclude/results/filenotfound.xml"));
    }

    /// <summary>
    /// Includes itself by url.
    /// </summary>
    [Fact]
    public void IncludesItselfByUrl()
    {
        RunAndCompare("../../XInclude/tests/includesitself.xml", "../../XInclude/results/includesitself.xml");
    }

    /// <summary>
    /// Includes itself by url - no href - as text.
    /// </summary>
    [Fact]
    public void IncludesItselfNoHrefText()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("../../XInclude/tests/includesitself-nohref-text.xml", "../../XInclude/results/includesitself-nohref-text.xml"));
    }

    /// <summary>
    /// Text inclusion. 
    /// </summary>
    [Fact]
    public void TextInclusion()
    {
        RunAndCompare("../../XInclude/tests/working_example.xml", "../../XInclude/results/working_example.xml");
    }

    /// <summary>
    /// Text inclusion. 
    /// </summary>
    [Fact]
    public void TextInclusion2()
    {
        RunAndCompare("../../XInclude/tests/working_example2.xml", "../../XInclude/results/working_example2.xml");
    }

    /// <summary>
    /// Fallback.
    /// </summary>
    [Fact]
    public void Fallback()
    {
        RunAndCompare("../../XInclude/tests/fallback.xml", "../../XInclude/results/fallback.xml");
    }

    /// <summary>
    /// XPointer.
    /// </summary>
    [Fact]
    public void XPointer()
    {
        RunAndCompare("../../XInclude/tests/xpointer.xml", "../../XInclude/results/xpointer.xml");
    }

    /// <summary>
    /// xml:lang fixup
    /// </summary>
    [Fact]
    public void XmlLangTest()
    {
        RunAndCompare("../../XInclude/tests/langtest.xml", "../../XInclude/results/langtest.xml");
    }

    /// <summary>
    /// ReadOuterXml() test.
    /// </summary>
    [Fact]
    public void OuterXmlTest()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document.xml");
        xir.MoveToContent();
        var outerXml = xir.ReadOuterXml();
        xir.Close();
        xir = new XIncludingReader("../../XInclude/tests/document.xml");
        xir.MoveToContent();
        var doc = new XmlDocument();
        doc.PreserveWhitespace = true;
        doc.Load(xir);
        var outerXml2 = doc.DocumentElement.OuterXml;
        Assert.Equal(outerXml, outerXml2);
    }

    /// <summary>
    /// ReadInnerXml() test.
    /// </summary>
    [Fact]
    public void InnerXmlTest()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document.xml");
        xir.MoveToContent();
        var innerXml = xir.ReadInnerXml();
        xir.Close();
        xir = new XIncludingReader("../../XInclude/tests/document.xml");
        xir.MoveToContent();
        var doc = new XmlDocument();
        doc.PreserveWhitespace = true;
        doc.Load(xir);
        var innerXml2 = doc.DocumentElement.InnerXml;
        Assert.Equal(innerXml, innerXml2);
    }

    /// <summary>
    /// Depth test.
    /// </summary>
    [Fact]
    public void DepthTest()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document.xml");
        var sb = new StringBuilder();
        while (xir.Read())
        {
            Console.WriteLine("{0} | {1} | {2} | {3}",
                xir.NodeType, xir.Name, xir.Value, xir.Depth);
            sb.Append(xir.Depth);
        }
        var expected = "00011211111111223221100";
        Assert.Equal(sb.ToString(), expected);
    }

    /// <summary>
    /// Custom resolver test.
    /// </summary>
    [Fact]
    public void CustomResolver()
    {
        RunAndCompare("../../XInclude/tests/resolver.xml", "../../XInclude/results/resolver.xml", false, new TestResolver());
    }

    /// <summary>
    /// Test for a bug discovered by Martin Wickett.
    /// </summary>
    [Fact]
    public void Test_Martin()
    {
        RunAndCompare("../../XInclude/tests/test-Martin.xml", "../../XInclude/results/test-Martin.xml");
    }

    /// <summary>
    /// Test for string as input (no base URI)
    /// </summary>
    [Fact]
    public void NoBaseURITest()
    {
        var sr = new StreamReader("../../XInclude/tests/document.xml");
        var xml = sr.ReadToEnd();
        sr.Close();

        Assert.Throws<FatalResourceException>(() =>
        {
            var xir = new XIncludingReader(new StringReader(xml));
            var w = XmlWriter.Create(Console.Out);
            while (xir.Read()) ;
        });
    }

    /// <summary>
    /// Caching test.
    /// </summary>
    [Fact]
    public void CachingTest()
    {
        RunAndCompare("../../XInclude/tests/caching.xml", "../../XInclude/results/caching.xml");
    }

    /// <summary>
    /// Infinite loop (bug 1187498)
    /// </summary>
    [Fact]
    public void LoopTest()
    {
        RunAndCompare("../../XInclude/tests/loop.xml", "../../XInclude/results/loop.xml");
    }

    [Fact]
    public void GetAttributeTest()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document.xml");
        while (xir.Read())
        {
            if (xir.NodeType == XmlNodeType.Element && xir.Name == "disclaimer")
            {
                Assert.True(xir.AttributeCount == 1);
                Assert.EndsWith("disclaimer.xml", xir.GetAttribute(0));
                Assert.EndsWith("disclaimer.xml", xir[0]);
            }
        }
    }

    [Fact]
    public void GetAttributeTest2()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document2.xml");
        xir.MakeRelativeBaseUri = false;
        while (xir.Read())
        {
            if (xir.NodeType == XmlNodeType.Element && xir.Name == "disclaimer")
            {
                Assert.True(xir.AttributeCount == 1);
                Assert.EndsWith("tests/disclaimerWithXmlBase.xml", xir.GetAttribute(0));
                Assert.EndsWith("tests/disclaimerWithXmlBase.xml", xir[0]);
            }
        }
    }

    [Fact]
    public void GetAttributeTest3()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document.xml");
        while (xir.Read())
        {
            if (xir.NodeType == XmlNodeType.Element && xir.Name == "disclaimer")
            {
                Assert.True(xir.AttributeCount == 1);
                xir.MoveToAttribute(0);
                Assert.True(xir.Name == "xml:base");
                Assert.EndsWith("disclaimer.xml", xir.Value);
            }
        }
    }

    [Fact]
    public void GetAttributeTest4()
    {
        var xir = new XIncludingReader("../../XInclude/tests/document2.xml");
        xir.MakeRelativeBaseUri = false;
        while (xir.Read())
        {
            if (xir.NodeType == XmlNodeType.Element && xir.Name == "disclaimer")
            {
                Assert.True(xir.AttributeCount == 1);
                xir.MoveToAttribute(0);
                Console.WriteLine(xir.Value);
                Assert.True(xir.Name == "xml:base");
                Assert.EndsWith("tests/disclaimerWithXmlBase.xml", xir.Value);
            }
        }
    }

    [Fact]
    public void SerializerTest()
    {
        var ser = new XmlSerializer(typeof(Document));
        using var r = new XIncludingReader("../../XInclude/tests/Document3.xml");
        var doc = (Document)ser.Deserialize(r);
        Assert.NotNull(doc);
        Assert.True(doc.Name == "Foo");
        Assert.NotNull(doc.Items);
        Assert.True(doc.Items.Length == 1);
        Assert.True(doc.Items[0].Value == "Bar");
    }

    [Fact]
    public void EncodingTest()
    {
        var r = new XmlTextReader(new StringReader("<foo/>"));
        var xir = new XIncludingReader(r);
        xir.ExposeTextInclusionsAsCDATA = true;
        xir.MoveToContent();
        Assert.True(xir.Encoding == UnicodeEncoding.Unicode);
    }

    [Fact]
    public void ShouldPreserveCDATA()
    {
        var xml = "<HTML><![CDATA[<img src=\"/_layouts/images/\">]]></HTML>";
        var xir = new XIncludingReader(new StringReader(xml));
        xir.Read();
        Assert.Equal("<HTML><![CDATA[<img src=\"/_layouts/images/\">]]></HTML>", xir.ReadOuterXml());
    }

    [Fact]
    public void TestXPointerIndentationBug()
    {

        var resolver = new XmlUrlResolver();
        resolver.Credentials = CredentialCache.DefaultCredentials;
        var xsltSettings = new XsltSettings();
        xsltSettings.EnableDocumentFunction = true;
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        //settings.ProhibitDtd = false;
        var reader = XmlReader.Create("../../XInclude/tests/Transform.xsl", settings);
        var xInputReader = new XIncludingReader("../../XInclude/tests/FileA.xml");
        try
        {
            var processor = new MvpXslTransform(false);
            processor.Load(reader, xsltSettings, resolver);
            //xInputReader.XmlResolver = new XMLBase();
            var xInputDoc = new XmlDocument();
            xInputDoc.Load(xInputReader);
            var xInput = new XmlInput(xInputDoc);
            var stringW = new StringWriter();
            var xOutput = new XmlOutput(stringW);
            processor.Transform(xInput, null, xOutput);
            //processor.TemporaryFiles.Delete();
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?>NodeA Content", stringW.ToString());
        }
        finally
        {
            reader.Close();
            xInputReader.Close();
        }
    }

    [Fact]
    public void TestLineInfo()
    {
        var r = new XIncludingReader("../../XInclude/tests/document.xml");
        var lineInfo = ((IXmlLineInfo)r);
        Assert.True(lineInfo.HasLineInfo());
        r.Read();
        Assert.Equal(1, lineInfo.LineNumber);
        Assert.Equal(3, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(1, lineInfo.LineNumber);
        Assert.Equal(22, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(2, lineInfo.LineNumber);
        Assert.Equal(2, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(2, lineInfo.LineNumber);
        Assert.Equal(54, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(6, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(8, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(54, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(56, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(1, lineInfo.LineNumber);
        Assert.Equal(22, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(2, lineInfo.LineNumber);
        Assert.Equal(5, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(2, lineInfo.LineNumber);
        Assert.Equal(17, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(3, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(3, lineInfo.LineNumber);
        Assert.Equal(12, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(4, lineInfo.LineNumber);
        Assert.Equal(2, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(4, lineInfo.LineNumber);
        Assert.Equal(13, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(5, lineInfo.LineNumber);
        Assert.Equal(4, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(5, lineInfo.LineNumber);
        Assert.Equal(6, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(7, lineInfo.LineNumber);
        Assert.Equal(18, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(7, lineInfo.LineNumber);
        Assert.Equal(20, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(8, lineInfo.LineNumber);
        Assert.Equal(3, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(4, lineInfo.LineNumber);
        Assert.Equal(75, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(6, lineInfo.LineNumber);
        Assert.Equal(3, lineInfo.LinePosition);
        r.Read();
        Assert.Equal(6, lineInfo.LineNumber);
        Assert.Equal(12, lineInfo.LinePosition);
    }
}

//public class XMLBase : XmlUrlResolver
//{
//    public override Uri ResolveUri(Uri baseUri, string relativeUri)
//    {
//        return base.ResolveUri(baseUri, HttpContext.Current.Server.MapPath("/") + relativeUri);
//    }
//}

public class Document
{
    string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    Item[] items;

    public Item[] Items
    {
        get { return items; }
        set { items = value; }
    }
}

public class Item
{
    string value;

    public string Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}

public class TestResolver : XmlUrlResolver
{
    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
        if (absoluteUri.Scheme == "textreader")
            return new StringReader(@"<text attr=""val"">From custom resolver (as TextReader)</text>");
        else if (absoluteUri.Scheme == "stream")
        {
            return File.OpenRead("../../XInclude/results/document.xml");
        }
        else if (absoluteUri.Scheme == "xmlreader")
        {
            return XmlReader.Create(new StringReader(@"<text attr=""val"">From custom resolver (as XmlReader)</text>"), null, absoluteUri.AbsoluteUri);
        }
        else
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
    }

}
