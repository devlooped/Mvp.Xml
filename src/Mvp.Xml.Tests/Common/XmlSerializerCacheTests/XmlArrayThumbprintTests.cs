using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlArrayThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlArrayThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();

    }

    [Fact]
    public void XmlArraySameName()
    {
        var array1 = new XmlArrayAttribute("myname");
        var array2 = new XmlArrayAttribute("myname");

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentTypes()
    {
        var array1 = new XmlArrayAttribute("myname");
        var array2 = new XmlArrayAttribute("myname");

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMeToo), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentNames()
    {
        var array1 = new XmlArrayAttribute("myname");
        var array2 = new XmlArrayAttribute("myothername");

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameNamespace()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.Namespace = "mynamespace";

        var array2 = new XmlArrayAttribute("myname");
        array2.Namespace = "mynamespace";

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentNamespace()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.Namespace = "mynamespace";
        var array2 = new XmlArrayAttribute("myname");
        array2.Namespace = "myothernamespace";

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameNullable()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.IsNullable = true;
        var array2 = new XmlArrayAttribute("myname");
        array2.IsNullable = true;

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentNullable()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.IsNullable = true;
        var array2 = new XmlArrayAttribute("myname");
        array2.IsNullable = false;

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameForm()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var array2 = new XmlArrayAttribute("myname");
        array2.Form = System.Xml.Schema.XmlSchemaForm.Qualified;

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentForm()
    {
        var array1 = new XmlArrayAttribute("myname");
        array1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var array2 = new XmlArrayAttribute("myname");
        array2.Form = System.Xml.Schema.XmlSchemaForm.Unqualified;

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArraySameMemberName()
    {
        var array1 = new XmlArrayAttribute("myname");
        var array2 = new XmlArrayAttribute("myname");

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void XmlArrayDifferentMemberName()
    {
        var array1 = new XmlArrayAttribute("myname");
        var array2 = new XmlArrayAttribute("myname");

        atts1.XmlArray = array1;
        atts2.XmlArray = array2;

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheOtherMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
