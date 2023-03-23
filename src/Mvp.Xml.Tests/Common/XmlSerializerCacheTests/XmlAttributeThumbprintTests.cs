using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlAttributeThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlAttributeThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void XmlArraySameName()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        var attribute2 = new XmlAttributeAttribute("myname");

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentTypes()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        var attribute2 = new XmlAttributeAttribute("myname");

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMeToo), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentNames()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        var attribute2 = new XmlAttributeAttribute("myothername");

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameNamespace()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        attribute1.Namespace = "mynamespace";

        var attribute2 = new XmlAttributeAttribute("myname");
        attribute2.Namespace = "mynamespace";

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentNamespace()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        attribute1.Namespace = "mynamespace";
        var attribute2 = new XmlAttributeAttribute("myname");
        attribute2.Namespace = "myothernamespace";

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameForm()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        attribute1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var attribute2 = new XmlAttributeAttribute("myname");
        attribute2.Form = System.Xml.Schema.XmlSchemaForm.Qualified;

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentForm()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        attribute1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var attribute2 = new XmlAttributeAttribute("myname");
        attribute2.Form = System.Xml.Schema.XmlSchemaForm.Unqualified;

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameMemberName()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        var attribute2 = new XmlAttributeAttribute("myname");

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentMemberName()
    {
        var attribute1 = new XmlAttributeAttribute("myname");
        var attribute2 = new XmlAttributeAttribute("myname");

        atts1.XmlAttribute = attribute1;
        atts2.XmlAttribute = attribute2;

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheOtherMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
