using System.Xml;
using Mvp.Xml.Common;
using Xunit;

namespace Mvp.Xml.Tests;

public class XmlNodeListFactoryTests
{
    XmlDocument pubsDocument;

    public XmlNodeListFactoryTests()
    {
        pubsDocument = new XmlDocument();
        pubsDocument.Load(Globals.GetResource(Globals.PubsResource));
    }

    [Fact]
    public void MultipleCompleteEnumerations()
    {
        var nodeIterator = pubsDocument.CreateNavigator().Select("/dsPubs/publishers");
        var nodeList = XmlNodeListFactory.CreateNodeList(nodeIterator);

        // Get the first node list enumerator.
        var enumerator = nodeList.GetEnumerator();

        while (enumerator.MoveNext())
        {
            // Enumerate all publishers.
        }

        // Get the second node list enumerator.
        enumerator = nodeList.GetEnumerator();

        // Ensure that the second node list enumerator is in a usable state.
        Assert.True(enumerator.MoveNext());
    }

    [Fact]
    public void List1()
    {
        var xml = @"<?xml version='1.0'?>
<root>
	<element>1</element>
	<element></element>
	<element/>
	<element>2</element>
</root>";

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var iterator = doc.CreateNavigator().Select("//element");
        var list = XmlNodeListFactory.CreateNodeList(iterator);

        var count = 0;
        foreach (XmlNode n in list)
        {
            count++;
        }
        Assert.Equal(4, count);

        iterator = doc.CreateNavigator().Select("//element");
        list = XmlNodeListFactory.CreateNodeList(iterator);
        Assert.Equal(4, list.Count);
    }
}
