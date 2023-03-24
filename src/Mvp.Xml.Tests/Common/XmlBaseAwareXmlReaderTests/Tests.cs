using System;
using System.Xml;
using System.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests.XmlBaseAwareXmlReaderTests;

public class Tests
{
    [Fact]
    public void BasicTest()
    {
        XmlReader r = new XmlBaseAwareXmlReader("../../Common/XmlBaseAwareXmlReaderTests/test.xml");
        while (r.Read())
        {
            if (r.NodeType == XmlNodeType.Element)
            {
                switch (r.Name)
                {
                    case "catalog":
                        Assert.EndsWith("XmlBaseAwareXmlReaderTests/test.xml", r.BaseURI);
                        break;
                    case "files":
                        Assert.True(r.BaseURI == "file:///d:/Files/");
                        break;
                    case "file":
                        Assert.True(r.BaseURI == "file:///d:/Files/");
                        break;
                    case "a":
                        Assert.EndsWith("XmlBaseAwareXmlReaderTests/test.xml", r.BaseURI);
                        break;
                    case "b":
                        Assert.True(r.BaseURI == "file:///d:/Files/a/");
                        break;
                    case "c":
                        Assert.True(r.BaseURI == "file:///d:/Files/c/");
                        break;
                    case "e":
                        Assert.True(r.BaseURI == "file:///d:/Files/c/");
                        break;
                    case "d":
                        Assert.True(r.BaseURI == "file:///d:/Files/a/");
                        break;
                }
            }
            else if (r.NodeType == XmlNodeType.Text && r.Value.Trim() != "")
            {
                Assert.True(r.BaseURI == "file:///d:/Files/c/");
            }
            else if (r.NodeType == XmlNodeType.Comment)
            {
                Assert.True(r.BaseURI == "file:///d:/Files/a/");
            }
            else if (r.NodeType == XmlNodeType.ProcessingInstruction)
            {
                Assert.True(r.BaseURI == "file:///d:/Files/");
            }
        }
        r.Close();
    }

    [Fact]
    public void ReaderWithPath()
    {
        var settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        XmlReader r = new XmlBaseAwareXmlReader("../../Common/XmlBaseAwareXmlReaderTests/relativeTest.xml");
        var doc = new XPathDocument(r);
        var nav = doc.CreateNavigator();
        var ni = nav.Select("/catalog");
        ni.MoveNext();
        Assert.EndsWith("/XmlBaseAwareXmlReaderTests/relativeTest.xml", ni.Current.BaseURI);
        ni = nav.Select("/catalog/relative/relativenode");
        ni.MoveNext();
        Console.WriteLine(ni.Current.BaseURI);
        Assert.True(ni.Current.BaseURI.IndexOf("/XmlBaseAwareXmlReaderTests/") != -1);
    }

}
