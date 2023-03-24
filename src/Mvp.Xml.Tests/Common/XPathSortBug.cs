using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

/// <summary/>

public class XPathSortBug
{
    [Fact]
    public void Repro()
    {
        var xml = @"
				<root xmlns='mvp-xml'>
					<item>
						<value>25</value>
					</item>
					<item>
						<value>40</value>
					</item>
					<item>
						<value>10</value>
					</item>
				</root>";

        var doc = new XPathDocument(new StringReader(xml));
        var nav = doc.CreateNavigator();

        // Setup namespace resolution.
        var ctx = new XmlNamespaceManager(nav.NameTable);
        ctx.AddNamespace("mvp", "mvp-xml");

        // Create sort expression.
        var sort = nav.Compile("mvp:value");
        sort.SetContext(ctx);

        // Setting the sort *after* the context yields no results at all in .NET 1.X
        // but works in .NET 2.0
        var items = nav.Compile("//mvp:item");
        items.AddSort(sort, XmlSortOrder.Ascending, XmlCaseOrder.None, String.Empty, XmlDataType.Number);
        items.SetContext(ctx);

        var it = doc.CreateNavigator().Select(items);
        Assert.True(it.MoveNext());

        // The same code but setting the sort *after* the context works fine.
        items = nav.Compile("//mvp:item");
        items.SetContext(ctx);
        items.AddSort(sort, XmlSortOrder.Ascending, XmlCaseOrder.None, String.Empty, XmlDataType.Number);

        it = doc.CreateNavigator().Select(items);
        Assert.True(it.MoveNext());
    }
}
