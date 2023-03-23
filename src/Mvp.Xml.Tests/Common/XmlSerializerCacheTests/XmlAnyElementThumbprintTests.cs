using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlAnyElementThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlAnyElementThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void AnyElementAttributesSameMember()
    {
        var any1 = new XmlAnyElementAttribute();
        var any2 = new XmlAnyElementAttribute();

        atts1.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void AnyElementAttributesSameNamespace()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");
        var any2 = new XmlAnyElementAttribute("myname", "myns");

        atts1.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNamespacesAnyElementAttributes()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");
        var any2 = new XmlAnyElementAttribute("myname", "myotherns");

        atts1.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNamesAnyElementAttributes()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");
        var any2 = new XmlAnyElementAttribute("myothername", "myns");

        atts1.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void AnyElementAttributeDifferentMember()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");

        atts1.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any1);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "AnotherMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void TwoSameAnyElement()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");
        var any2 = new XmlAnyElementAttribute("myothername", "myns");

        atts1.XmlAnyElements.Add(any1);
        atts1.XmlAnyElements.Add(any2);

        atts2.XmlAnyElements.Add(any1);
        atts2.XmlAnyElements.Add(any2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void TwoDifferentAnyElement()
    {
        var any1 = new XmlAnyElementAttribute("myname", "myns");
        var any2 = new XmlAnyElementAttribute("myothername", "myns");
        var any3 = new XmlAnyElementAttribute("mythirdname", "my3ns");

        atts1.XmlAnyElements.Add(any1);
        atts1.XmlAnyElements.Add(any2);

        atts2.XmlAnyElements.Add(any3);
        atts2.XmlAnyElements.Add(any2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
