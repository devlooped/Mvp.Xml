using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using Xunit;

namespace Mvp.Xml.Tests.XslReaderTests;

public class XslReaderTests
{
    static string copyTransform =
@"<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
          <xsl:template match='/'>
            <xsl:copy-of select='/' />
          </xsl:template>
        </xsl:stylesheet>";


    public static XmlReader GetReader(string xml)
    {
        var s = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        //s.ProhibitDtd = false;
        return XmlReader.Create(Globals.GetResource(xml), s);
    }

    /// <summary>
    /// Compare with standard XmlReader test
    /// </summary>
    [Fact]
    public void Test1()
    {
        CompareWithStandardReader(true, 16);
    }

    void CompareWithStandardReader(bool multiThread, int bufSize)
    {
        var r = GetReader(Globals.NorthwindResource);
        var xslt = new XslCompiledTransform();
        xslt.Load("../../Common/XslReaderTests/test1.xslt");
        var ms = new MemoryStream();
        var s = new XmlWriterSettings();
        s.OmitXmlDeclaration = true;
        var w = XmlWriter.Create(ms, s);
        xslt.Transform(r, w);
        r.Close();
        w.Close();
        var buf = ms.ToArray();
        var standard = XmlReader.Create(new MemoryStream(buf));
        var xslReader = new XslReader(xslt, multiThread, bufSize);
        xslReader.StartTransform(new XmlInput(GetReader(Globals.NorthwindResource)), null);
        CompareReaders(standard, xslReader);
    }

    void CompareReaders(XmlReader standard, XmlReader custom)
    {
        while (standard.Read())
        {
            Assert.True(custom.Read());
            CompareReaderProperties(standard, custom);

            if (standard.HasAttributes)
            {
                while (standard.MoveToNextAttribute())
                {
                    Assert.True(custom.MoveToNextAttribute());
                    CompareReaderProperties(standard, custom);
                }
                standard.MoveToElement();
                Assert.True(custom.MoveToElement());
            }
        }
    }

    static void CompareReaderProperties(XmlReader standard, XmlReader custom)
    {
        Assert.Equal(standard.AttributeCount, custom.AttributeCount);
        Assert.Equal(standard.BaseURI, custom.BaseURI);
        Assert.Equal(standard.Depth, custom.Depth);
        Assert.Equal(standard.EOF, custom.EOF);
        Assert.Equal(standard.HasAttributes, custom.HasAttributes);
        Assert.Equal(standard.HasValue, custom.HasValue);
        Assert.Equal(standard.IsDefault, custom.IsDefault);
        Assert.Equal(standard.IsEmptyElement, custom.IsEmptyElement);
        Assert.Equal(standard.LocalName, custom.LocalName);
        Assert.Equal(standard.Name, custom.Name);
        Assert.Equal(standard.NamespaceURI, custom.NamespaceURI);
        Assert.Equal(standard.NodeType, custom.NodeType);
        Assert.Equal(standard.Prefix, custom.Prefix);
        Assert.Equal(standard.QuoteChar, custom.QuoteChar);
        Assert.Equal(standard.ReadState, custom.ReadState);
        Assert.Equal(standard.Value, custom.Value);
        Assert.Equal(standard.ValueType, custom.ValueType);
        Assert.Equal(standard.XmlLang, custom.XmlLang);
        Assert.Equal(standard.XmlSpace, custom.XmlSpace);
        Assert.Equal(standard.LookupNamespace("foo"), custom.LookupNamespace("foo"));
    }

    /// <summary>
    /// Test LookupNamespace()
    /// </summary>
    [Fact]
    public void Test2()
    {
        var xml = @"<foo xmlns:f=""bar""/>";
        var standard = XmlReader.Create(new StringReader(xml));

        var xslt = new XslCompiledTransform();
        xslt.Load(XmlReader.Create(new StringReader(copyTransform)));
        var xslReader = new XslReader(xslt);
        xslReader.StartTransform(new XmlInput(new StringReader(xml)), null);

        standard.MoveToContent();
        xslReader.MoveToContent();

        Assert.True(standard.NodeType == xslReader.NodeType);
        Assert.True(standard.Name == xslReader.Name);
        var nsUri1 = standard.LookupNamespace("f");
        var nsUri2 = xslReader.LookupNamespace("f");
        Assert.True(nsUri1 == nsUri2,
            string.Format("'{0}' != '{1}'", nsUri1, nsUri2));
    }

    /// <summary>
    /// Test Read() after EOF
    /// </summary>
    [Fact]
    public void Test3()
    {
        var xml = @"<foo xmlns:f=""bar""/>";
        var xslt = new XslCompiledTransform();
        xslt.Load(XmlReader.Create(new StringReader(copyTransform)));
        var xslReader = new XslReader(xslt);
        xslReader.StartTransform(new XmlInput(new StringReader(xml)), null);
        while (!xslReader.EOF)
        {
            xslReader.Read();
        }
        Assert.False(xslReader.Read());
    }

    /// <summary>
    /// Test singlethread with small buffer
    /// </summary>
    [Fact]
    public void Test4()
    {
        CompareWithStandardReader(false, 2);
    }

    /// <summary>
    /// Test different bufer sizes
    /// </summary>
    [Fact]
    public void Test5()
    {
        for (var b = -1024; b < 1024; b += 100)
        {
            CompareWithStandardReader(false, b);
        }
        for (var b = -1024; b < 1024; b += 100)
        {
            CompareWithStandardReader(true, b);
        }
        CompareWithStandardReader(true, 0);
        CompareWithStandardReader(true, 1);
        CompareWithStandardReader(true, int.MinValue);
    }

    /// <summary>
    /// Test reader restart
    /// </summary>
    [Fact]
    public void Test6()
    {
        var r = GetReader(Globals.NorthwindResource);
        var xslt = new XslCompiledTransform();
        xslt.Load("../../Common/XslReaderTests/test1.xslt");
        var ms = new MemoryStream();
        var s = new XmlWriterSettings();
        s.OmitXmlDeclaration = true;
        var w = XmlWriter.Create(ms, s);
        xslt.Transform(r, w);
        r.Close();
        w.Close();
        var buf = ms.ToArray();
        var standard = XmlReader.Create(new MemoryStream(buf));
        var xslReader = new XslReader(xslt, true, 16);
        xslReader.StartTransform(new XmlInput(GetReader(Globals.NorthwindResource)), null);
        xslReader.MoveToContent();
        xslReader.Read();
        //Now restart it
        xslReader.StartTransform(new XmlInput(GetReader(Globals.NorthwindResource)), null);
        CompareReaders(standard, xslReader);
    }

    [Fact]
    public void Test7()
    {
        Assert.Throws<OverflowException>(() =>
            CompareWithStandardReader(true, int.MaxValue / 2 + 2));
    }

    [Fact]
    public void TestWithDataSet()
    {
        var stylesheet = @"
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
          <xsl:template match='/'>                      
<BTRChart xmlns:w=""http://schemas.microsoft.com/office/word/2003/wordml""><Data><DundasChart xmlns=""urn:www.benefittech.com:Chart"">[Data]</DundasChart></Data></BTRChart></xsl:template>
        </xsl:stylesheet>
";
        var doc = new XmlDocument();
        var xslt = new XslCompiledTransform();
        xslt.Load(XmlReader.Create(new StringReader(stylesheet)));
        var reader = new XslReader(xslt);
        var ds = new DataSet();
        ds.ReadXml(reader.StartTransform(new XmlInput(doc), null));
        var writer = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.Indent = false;
        settings.OmitXmlDeclaration = true;
        var w = XmlWriter.Create(writer, settings);
        ds.WriteXml(w);
        w.Close();
        Assert.Equal(@"<BTRChart><Data><DundasChart xmlns=""urn:www.benefittech.com:Chart"">[Data]</DundasChart></Data></BTRChart>", writer.ToString());
    }

    [Fact]
    public void TestEmptyElement()
    {
        var xslt = new XslCompiledTransform();
        xslt.Load("../../Common/XslReaderTests/test2.xslt");
        var xslReader = new XslReader(xslt);
        xslReader.StartTransform(new XmlInput(GetReader(Globals.NorthwindResource)), null);
        xslReader.MoveToContent();
        Assert.True(xslReader.NodeType == XmlNodeType.Element);
        Assert.True(xslReader.Name == "empty");
        Assert.False(xslReader.IsEmptyElement);
    }
}
