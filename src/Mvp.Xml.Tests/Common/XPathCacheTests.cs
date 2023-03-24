using System;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

public class XPathCacheTests : IDisposable
{
    XPathDocument Document;
    XPathDocument DocumentNoNs;

    public XPathCacheTests()
    {
        Document = new XPathDocument(Globals.GetResource(Globals.PubsNsResource));
        DocumentNoNs = new XPathDocument(Globals.GetResource(Globals.PubsResource));
    }

    public void Dispose()
    {
        Document = null;
    }

    [Fact]
    public void DefaultNamespace()
    {
        var pubs = new dsPubs();
        pubs.id = "0736";

        var ser =
            new System.Xml.Serialization.XmlSerializer(typeof(dsPubs), String.Empty);

        var sw = new System.IO.StringWriter();
        ser.Serialize(sw, pubs);

        var xml = sw.ToString();
        Assert.NotNull(xml);
    }

    [Fact]
    public void DynamicVariable()
    {
        var expr = "//mvp:titles[mvp:price > 10]";
        var dyn = "//mvp:titles[mvp:price > $price]";
        var price = 10;

        var docnav = Document.CreateNavigator();
        var xpath = docnav.Compile(expr);
        var mgr = new XmlNamespaceManager(docnav.NameTable);
        mgr.AddNamespace(Globals.MvpPrefix, Globals.MvpNamespace);
        xpath.SetContext(mgr);

        var count1 = Document.CreateNavigator().Select(xpath).Count;
        // Test with compiled expression.
        var count2 = XPathCache.Select(expr, Document.CreateNavigator(), mgr).Count;

        Assert.Equal(count1, count2);

        // Test with dynamic expression.
        count2 = XPathCache.Select(dyn, Document.CreateNavigator(),
            mgr, new XPathVariable("price", price)).Count;

        Assert.Equal(count1, count2);
    }

    [Fact]
    public void PrefixMapping()
    {
        var expr = "//mvp:titles[mvp:price > 10]";

        var docnav = Document.CreateNavigator();
        var xpath = docnav.Compile(expr);
        var mgr = new XmlNamespaceManager(docnav.NameTable);
        mgr.AddNamespace(Globals.MvpPrefix, Globals.MvpNamespace);
        xpath.SetContext(mgr);

        var count1 = Document.CreateNavigator().Select(xpath).Count;
        var count2 = XPathCache.Select(expr, Document.CreateNavigator(),
            new XmlPrefix(Globals.MvpPrefix, Globals.MvpNamespace, Document.CreateNavigator().NameTable)).Count;

        Assert.Equal(count1, count2);
    }

    [Fact]
    public void Sorted1()
    {
        var expr = "//mvp:titles";

        var docnav = Document.CreateNavigator();
        var xpath = docnav.Compile(expr);
        var mgr = new XmlNamespaceManager(docnav.NameTable);
        mgr.AddNamespace(Globals.MvpPrefix, Globals.MvpNamespace);
        xpath.SetContext(mgr);
        var sort = docnav.Compile("mvp:price");
        sort.SetContext(mgr);
        xpath.AddSort(sort, XmlSortOrder.Ascending, XmlCaseOrder.LowerFirst, String.Empty, XmlDataType.Number);

        var it = Document.CreateNavigator().Select(xpath);

        DebugUtils.XPathNodeIteratorToConsole(it);

        it = Document.CreateNavigator().Select(xpath);

        it.MoveNext();
        it.Current.MoveToFirstChild();
        var id1 = it.Current.Value;

        var cached = XPathCache.SelectSorted(
            expr, Document.CreateNavigator(),
            "mvp:price", XmlSortOrder.Ascending, XmlCaseOrder.LowerFirst, String.Empty, XmlDataType.Number,
            new XmlPrefix(Globals.MvpPrefix, Globals.MvpNamespace, Document.CreateNavigator().NameTable));

        DebugUtils.XPathNodeIteratorToConsole(cached);

        cached = XPathCache.SelectSorted(
            expr, Document.CreateNavigator(),
            "mvp:price", XmlSortOrder.Ascending, XmlCaseOrder.LowerFirst, String.Empty, XmlDataType.Number,
            new XmlPrefix(Globals.MvpPrefix, Globals.MvpNamespace, Document.CreateNavigator().NameTable));

        cached.MoveNext();
        cached.Current.MoveToFirstChild();
        var id2 = cached.Current.Value;

        Assert.Equal(id1, id2);
    }
}
