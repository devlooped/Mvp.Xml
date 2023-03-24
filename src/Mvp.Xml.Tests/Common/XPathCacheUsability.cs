using System;
using System.Xml;
using System.Xml.XPath;

using Mvp.Xml.Common.XPath;

namespace Mvp.Xml.Tests;

public class XPathCacheUsability
{
    /// <summary>
    /// This code is just to show how it's used. 
    /// If run it will throw exceptions.
    /// </summary>
    public void Test()
    {
        var document = new XPathDocument(String.Empty).CreateNavigator();
        var it = XPathCache.Select("/customer/order/item", document);

        it = XPathCache.Select("/customer/order[id=$id]/item", document,
            new XPathVariable("id", "23"));

        string[] ids = null;
        foreach (var id in ids)
        {
            it = XPathCache.Select("/customer/order[id=$id]/item", document,
                new XPathVariable("id", id));
        }

        var mgr = new XmlNamespaceManager(document.NameTable);
        mgr.AddNamespace("po", "example-po");

        it = XPathCache.Select("/po:customer[id=$id]", document, mgr,
            new XPathVariable("id", "0736"));

        var doc = new XmlDocument();
        var list = XPathCache.SelectNodes("/customer", doc);
    }
}
