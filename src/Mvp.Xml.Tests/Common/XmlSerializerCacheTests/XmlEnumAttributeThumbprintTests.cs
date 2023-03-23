using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlEnumAttributeThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlEnumAttributeThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void SameName()
    {
        var enum1 = new XmlEnumAttribute("enum1");
        var enum2 = new XmlEnumAttribute("enum1");

        atts1.XmlEnum = enum1;
        atts2.XmlEnum = enum2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentName()
    {
        var enum1 = new XmlEnumAttribute("enum1");
        var enum2 = new XmlEnumAttribute("enum2");

        atts1.XmlEnum = enum1;
        atts2.XmlEnum = enum2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
