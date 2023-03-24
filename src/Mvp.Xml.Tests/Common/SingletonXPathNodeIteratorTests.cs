using System.IO;
using System.Xml.XPath;
using Mvp.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

public class SingletonXPathNodeIteratorTests
{
    [Fact]
    public void Test1()
    {
        var doc = new XPathDocument(new StringReader("<foo/>"));
        var node = doc.CreateNavigator().SelectSingleNode("/*");
        var ni = new SingletonXPathNodeIterator(node);
        Assert.True(ni.MoveNext());
        Assert.True(ni.Current == node);
        Assert.False(ni.MoveNext());
    }
}
