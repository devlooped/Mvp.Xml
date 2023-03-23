using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Mvp.Xml.Common.XPath;
using Xunit;

namespace Mvp.Xml.Tests.SubtreeeXPathNavigatorTests;


public class SubtreeTests
{
    [Fact(Skip = "Manual")]
    public void SubtreeSpeed()
    {
        var xdoc = new XPathDocument(Globals.GetResource(Globals.LibraryResource));
        var nav = xdoc.CreateNavigator();
        var doc = new XmlDocument();
        doc.Load(Globals.GetResource(Globals.LibraryResource));

        var xslt = new XslCompiledTransform();
        xslt.Load("../../Common/SubtreeeXPathNavigatorTests/print_root.xsl");

        var stopWatch = new Stopwatch();

        // Warmup
        var stmdom = new MemoryStream();
        var wd = new XmlDocument();
        wd.LoadXml(doc.DocumentElement.FirstChild.OuterXml);
        xslt.Transform(wd, null, stmdom);
        var stmxpath = new MemoryStream();
        nav.MoveToRoot();
        nav.MoveToFirstChild();
        nav.MoveToFirstChild();
        xslt.Transform(new SubtreeXPathNavigator(nav), null, stmxpath);
        nav = doc.CreateNavigator();

        var count = 10;
        float dom = 0;
        float xpath = 0;

        for (var i = 0; i < count; i++)
        {
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            stmdom = new MemoryStream();

            stopWatch.Start();

            // Create a new document for each child
            foreach (XmlNode testNode in doc.DocumentElement.ChildNodes)
            {
                var tmpDoc = new XmlDocument();
                tmpDoc.LoadXml(testNode.OuterXml);

                // Transform the subset.
                xslt.Transform(tmpDoc, null, stmdom);
            }
            stopWatch.Stop();
            dom += stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            GC.Collect();
            System.Threading.Thread.Sleep(1000);

            stmxpath = new MemoryStream();

            var expr = nav.Compile("/library/book");

            stopWatch.Start();
            var books = nav.Select(expr);
            while (books.MoveNext())
            {
                xslt.Transform(new SubtreeXPathNavigator(books.Current), null, stmxpath);
            }
            stopWatch.Stop();
            xpath += stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
        }

        Console.WriteLine("XmlDocument transformation: {0}", dom / count);
        Console.WriteLine("SubtreeXPathNavigator transformation: {0}", xpath / count);

        stmdom.Position = 0;
        stmxpath.Position = 0;

        Console.WriteLine(new StreamReader(stmdom).ReadToEnd());
        Console.WriteLine(new string('*', 100));
        Console.WriteLine(new string('*', 100));
        Console.WriteLine(new StreamReader(stmxpath).ReadToEnd());
    }

    [Fact]
    public void ShouldBeOnRootNodeTypeOnCreation()
    {
        var xml = @"
	<root>
		<salutations>
			<salute>Hi there <name>kzu</name>.</salute>
			<salute>Bye there <name>vga</name>.</salute>
		</salutations>
		<other>
			Hi there without salutations.
		</other>
	</root>";

        var set = new XmlReaderSettings();
        set.IgnoreWhitespace = true;
        var doc = new XPathDocument(XmlReader.Create(new StringReader(xml), set));
        var nav = doc.CreateNavigator();

        nav.MoveToFirstChild();
        nav.MoveToFirstChild();

        var subtree = new SubtreeXPathNavigator(nav);

        Assert.Equal(XPathNodeType.Root, subtree.NodeType);
    }

    [Fact]
    public void ShouldNotMoveOutsideRootXPathDocument()
    {
        var xml = @"
	<root>
		<salutations>
			<salute>Hi there <name>kzu</name>.</salute>
			<salute>Bye there <name>vga</name>.</salute>
		</salutations>
		<other>
			Hi there without salutations.
		</other>
	</root>";

        var set = new XmlReaderSettings();
        set.IgnoreWhitespace = true;
        var doc = new XPathDocument(XmlReader.Create(new StringReader(xml), set));
        var nav = doc.CreateNavigator();

        nav.MoveToFirstChild(); //root
        nav.MoveToFirstChild(); //salutations

        var subtree = new SubtreeXPathNavigator(nav);
        subtree.MoveToFirstChild(); //salutations
        Assert.Equal("salutations", subtree.LocalName);
        subtree.MoveToFirstChild(); //salute
        subtree.MoveToRoot();

        Assert.Equal(XPathNodeType.Root, subtree.NodeType);
        subtree.MoveToFirstChild(); //salutations
        Assert.Equal("salutations", subtree.LocalName);
    }

    [Fact]
    public void ShouldNotMoveOutsideRootXmlDocument()
    {
        var xml = @"
	<root>
		<salutations>
			<salute>Hi there <name>kzu</name>.</salute>
			<salute>Bye there <name>vga</name>.</salute>
		</salutations>
		<other>
			Hi there without salutations.
		</other>
	</root>";

        var set = new XmlReaderSettings();
        set.IgnoreWhitespace = true;
        var doc = new XmlDocument();
        doc.Load(XmlReader.Create(new StringReader(xml), set));
        var nav = doc.CreateNavigator();

        nav.MoveToFirstChild(); //root
        nav.MoveToFirstChild(); //salutations

        var subtree = new SubtreeXPathNavigator(nav);
        subtree.MoveToFirstChild(); //salutations
        Assert.Equal("salutations", subtree.LocalName);
        subtree.MoveToFirstChild(); //salute
        subtree.MoveToRoot();

        Assert.Equal(XPathNodeType.Root, subtree.NodeType);
        subtree.MoveToFirstChild(); //salutations
        Assert.Equal("salutations", subtree.LocalName);
    }
}
