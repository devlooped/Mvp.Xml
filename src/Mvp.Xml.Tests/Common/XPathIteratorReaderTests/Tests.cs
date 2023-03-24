using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests.XPathIteratorReaderTests;

public class Tests
{
    [Fact]
    public void TestRss()
    {
        var theWord = "XML";
        var doc = new XPathDocument("../../Common/XPathIteratorReaderTests/rss.xml");
        var it = doc.CreateNavigator().Select(
            "/rss/channel/item[contains(title,'" + theWord + "')]");

        var reader = new XPathIteratorReader(it);
        reader.MoveToContent();
        var xml = reader.ReadOuterXml();

        using (var sw = new StreamWriter(@"subset.xml", false))
        {
            var tw = new XmlTextWriter(sw);
            tw.WriteNode(new XPathIteratorReader(it), false);
            tw.Close();
        }

        Assert.True(xml != String.Empty);
    }

    [Fact]
    public void Test1()
    {
        var doc = new XPathDocument(Globals.GetResource(Globals.PubsResource));
        var it = doc.CreateNavigator().Select("//price[text() < 5]");

        var reader = new XPathIteratorReader(it, "prices");
        reader.MoveToContent();
        var xml = reader.ReadOuterXml();

        Assert.True(xml != String.Empty);
    }

    [Fact]
    public void FunctionalTest()
    {
        var doc = new XPathDocument(new StringReader(
            "<customer xmlns='mvp-xml'><order id='1'/><order id='2'/><order id='5'/></customer>"));
        var nav = doc.CreateNavigator();

        var mgr = new XmlNamespaceManager(nav.NameTable);
        mgr.AddNamespace("mvp", "mvp-xml");
        // On purpose, the query is wrong because it doesn't use the prefix.
        var expr = nav.Compile("//order[@id < 3]");
        expr.SetContext(mgr);
        var it = nav.Select(expr);

        var reader = new XPathIteratorReader(it, "orders");
        reader.MoveToContent();
        var xml = reader.ReadOuterXml();

        Assert.Equal("<orders></orders>", xml);

        // With the right query now.
        expr = nav.Compile("//mvp:order[@id < 3]");
        expr.SetContext(mgr);
        it = nav.Select(expr);

        reader = new XPathIteratorReader(it, "orders");
        reader.MoveToContent();
        xml = reader.ReadOuterXml();

        Assert.Equal("<orders><order id=\"1\" xmlns=\"mvp-xml\" /><order id=\"2\" xmlns=\"mvp-xml\" /></orders>", xml);
    }
}
