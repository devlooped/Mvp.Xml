using System.IO;
using System.Xml;
using Mvp.Xml.Common;
using Xunit;

namespace Mvp.Xml.Tests.Common;

public class XhtmlWriterTests
{
    const string XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml";

    [Fact]
    public void testShouldWriteEmptEndTags()
    {
        var sw = new StringWriter();
        var xw = XmlWriter.Create(sw);
        var w = new XhtmlWriter(xw);
        w.WriteStartDocument();
        w.WriteStartElement("html", XHTML_NAMESPACE);
        writeElement(w, "area");
        writeElement(w, "base");
        writeElement(w, "basefont");
        writeElement(w, "br");
        writeElement(w, "col");
        writeElement(w, "frame");
        writeElement(w, "hr");
        writeElement(w, "img");
        writeElement(w, "input");
        writeElement(w, "isindex");
        writeElement(w, "link");
        writeElement(w, "meta");
        writeElement(w, "param");
        w.WriteEndElement();
        w.WriteEndDocument();
        w.Close();
        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?><html xmlns=\"http://www.w3.org/1999/xhtml\"><area /><base /><basefont /><br /><col /><frame /><hr /><img /><input /><isindex /><link /><meta /><param /></html>",
            sw.ToString());
    }

    [Fact]
    public void testShouldWriteFullEndTags()
    {
        var sw = new StringWriter();
        var xw = XmlWriter.Create(sw);
        var w = new XhtmlWriter(xw);
        w.WriteStartDocument();
        w.WriteStartElement("html", XHTML_NAMESPACE);
        writeElement(w, "script");
        writeElement(w, "p");
        w.WriteEndElement();
        w.WriteEndDocument();
        w.Close();
        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?><html xmlns=\"http://www.w3.org/1999/xhtml\"><script></script><p></p></html>",
            sw.ToString());
    }

    [Fact]
    public void testShouldWriteEmptEndTagsEvenWithAttrs()
    {
        var sw = new StringWriter();
        var xw = XmlWriter.Create(sw);
        var w = new XhtmlWriter(xw);
        w.WriteStartDocument();
        w.WriteStartElement("html", XHTML_NAMESPACE);
        writeElementWithAttrs(w, "area");
        writeElementWithAttrs(w, "base");
        writeElementWithAttrs(w, "basefont");
        writeElementWithAttrs(w, "br");
        w.WriteEndElement();
        w.WriteEndDocument();
        w.Close();
        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?><html xmlns=\"http://www.w3.org/1999/xhtml\"><area foo=\"bar\" /><base foo=\"bar\" /><basefont foo=\"bar\" /><br foo=\"bar\" /></html>",
            sw.ToString());
    }

    static void writeElementWithAttrs(XhtmlWriter w, string name)
    {
        w.WriteStartElement(name, XHTML_NAMESPACE);
        w.WriteAttributeString("foo", "bar");
        w.WriteEndElement();
    }

    static void writeElement(XhtmlWriter w, string name)
    {
        w.WriteStartElement(name, XHTML_NAMESPACE);
        w.WriteEndElement();
    }
}
