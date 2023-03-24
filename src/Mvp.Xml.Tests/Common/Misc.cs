using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

/// <summary>
/// Miscelaneous tests.
/// </summary>

public class Misc
{
    [Fact]
    public void SerializeXmlDocument()
    {
        var ser = new XmlSerializer(typeof(XmlDocument));
        var j = 3;
        var k = (double)j;
        short s = 1;
        var ds = (double)s;

        Assert.NotNull(ser);
    }

    [Fact]
    public void CursorMovement()
    {
        var doc = new XPathDocument(Globals.GetResource(Globals.NorthwindResource));
        var nav = doc.CreateNavigator();

        nav.MoveToFirstChild();
        nav.MoveToFirstChild();

        var prev = nav.Clone();

        var it = nav.Select("//CustomerID");

        Assert.True(nav.IsSamePosition(prev));
    }
}
