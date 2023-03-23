using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.Common.XPath;
using Xunit;

namespace Mvp.Xml.Tests.Common;

public class XPathDocumentWriterFixture
{
    [Fact]
    public void ShouldCreateXmlWriterForDocument()
    {
        var writer = new XPathDocumentWriter();
        Assert.NotNull(writer);
    }

    [Fact]
    public void ShouldAllowEmptyBaseUri()
    {
        var writer = new XPathDocumentWriter(String.Empty);
        Assert.NotNull(writer);
    }

    [Fact]
    public void ShouldThrowWithNullBaseUri()
    {
        Assert.Throws<ArgumentNullException>(() => new XPathDocumentWriter((string)null));
    }

    [Fact]
    public void ShouldThrowIfNoRootWritten()
    {
        var writer = new XPathDocumentWriter();
        Assert.Throws<XmlException>(writer.Close);
    }

    [Fact]
    public void ShouldAcceptStartElementRootOnly()
    {
        var writer = new XPathDocumentWriter();
        writer.WriteStartElement("Foo");
        var doc = writer.Close();
    }

    [Fact]
    public void ShouldWriteRootElement()
    {
        var writer = new XPathDocumentWriter();
        writer.WriteElementString("hello", "world");
        var doc = writer.Close();

        Assert.NotNull(doc);

        var xml = GetXml(doc);

        Assert.Equal("<hello>world</hello>", xml);
    }

    [Fact]
    public void ShouldUseBaseUriForDocument()
    {
        var writer = new XPathDocumentWriter("kzu-uri");
        writer.WriteStartElement("Foo");
        var doc = writer.Close();

        Assert.NotNull(doc);
        Assert.Equal("kzu-uri", doc.CreateNavigator().BaseURI);
    }

    [Fact]
    public void WriterIsDisposable()
    {
        XPathDocument doc;
        using (var writer = new XPathDocumentWriter())
        {
            writer.WriteElementString("hello", "world");
            doc = writer.Close();
        }

        Assert.NotNull(doc);
    }

    static string GetXml(XPathDocument doc)
    {
        var nav = doc.CreateNavigator();
        var sw = new StringWriter();
        var set = new XmlWriterSettings();
        set.OmitXmlDeclaration = true;
        using (var w = XmlWriter.Create(sw, set))
        {
            nav.WriteSubtree(w);
        }

        return sw.ToString();
    }
}
