using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlElementThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlElementThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void SameItemType()
    {
        var element1 = new XmlElementAttribute("myname", typeof(SerializeMe));
        var element2 = new XmlElementAttribute("myname", typeof(SerializeMe));

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentItemTypes()
    {
        var element1 = new XmlElementAttribute("myname", typeof(SerializeMe));
        var element2 = new XmlElementAttribute("myname", typeof(SerializeMeToo));

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameDataType()
    {
        var element1 = new XmlElementAttribute("myname", typeof(SerializeMe));
        element1.DataType = "myfirstxmltype";
        var element2 = new XmlElementAttribute("myname", typeof(SerializeMe));
        element2.DataType = "myfirstxmltype";

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentDataTypes()
    {
        var element1 = new XmlElementAttribute();
        element1.DataType = "typeone";
        var element2 = new XmlElementAttribute();
        element2.DataType = "typetwo";

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameElementName()
    {
        var element1 = new XmlElementAttribute("myname");
        var element2 = new XmlElementAttribute("myname");

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentElementNames()
    {
        var element1 = new XmlElementAttribute("myname");
        var element2 = new XmlElementAttribute("myothername");

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentTypes()
    {
        var element1 = new XmlElementAttribute("myname");
        var element2 = new XmlElementAttribute("myname");

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMeToo), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNamespace()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.Namespace = "mynamespace";

        var element2 = new XmlElementAttribute("myname");
        element2.Namespace = "mynamespace";

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNamespace()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.Namespace = "mynamespace";
        var element2 = new XmlElementAttribute("myname");
        element2.Namespace = "myothernamespace";

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNullable()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.IsNullable = true;
        var element2 = new XmlElementAttribute("myname");
        element2.IsNullable = true;

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNullable()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.IsNullable = true;
        var element2 = new XmlElementAttribute("myname");
        element2.IsNullable = false;

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameForm()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var element2 = new XmlElementAttribute("myname");
        element2.Form = System.Xml.Schema.XmlSchemaForm.Qualified;

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentForm()
    {
        var element1 = new XmlElementAttribute("myname");
        element1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var element2 = new XmlElementAttribute("myname");
        element2.Form = System.Xml.Schema.XmlSchemaForm.Unqualified;

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameMemberName()
    {
        var element1 = new XmlElementAttribute("myname");
        var element2 = new XmlElementAttribute("myname");

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentMemberName()
    {
        var element1 = new XmlElementAttribute("myname");
        var element2 = new XmlElementAttribute("myname");

        atts1.XmlElements.Add(element1);
        atts2.XmlElements.Add(element2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheOtherMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
