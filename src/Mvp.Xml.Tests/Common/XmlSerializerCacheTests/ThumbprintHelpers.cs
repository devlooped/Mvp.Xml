using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class ThumbprintHelpers
{
    internal static void SameThumbprint(XmlAttributeOverrides ov1, XmlAttributeOverrides ov2)
    {
        var print1 = XmlAttributeOverridesThumbprinter.GetThumbprint(ov1);
        var print2 = XmlAttributeOverridesThumbprinter.GetThumbprint(ov2);

        //Console.WriteLine("p1 {0}, p2 {1}", print1, print2);
        Assert.Equal(print1, print2);
    }

    internal static void DifferentThumbprint(XmlAttributeOverrides ov1, XmlAttributeOverrides ov2)
    {
        var print1 = XmlAttributeOverridesThumbprinter.GetThumbprint(ov1);
        var print2 = XmlAttributeOverridesThumbprinter.GetThumbprint(ov2);

        Assert.False(print1 == print2);
    }
}
