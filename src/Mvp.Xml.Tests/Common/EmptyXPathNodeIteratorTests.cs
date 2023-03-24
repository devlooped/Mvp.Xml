using Mvp.Xml.Common.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

public class EmptyXPathNodeIteratorTests
{
    [Fact]
    public void Test1()
    {
        var ni = EmptyXPathNodeIterator.Instance;
        while (ni.MoveNext())
        {
            Assert.Fail("EmptyXPathNodeIterator must be empty");
        }
    }

    [Fact]
    public void Test2()
    {
        var ni = EmptyXPathNodeIterator.Instance;
        Assert.True(ni.MoveNext() == false);
        Assert.True(ni.Count == 0);
        Assert.True(ni.Current == null);
        Assert.True(ni.CurrentPosition == 0);
    }
}
