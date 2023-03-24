using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlArrayItemThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlArrayItemThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void SameItemType()
    {
        var array1 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        var array2 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentItemTypes()
    {
        var array1 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        var array2 = new XmlArrayItemAttribute("myname", typeof(SerializeMeToo));

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameDataType()
    {
        var array1 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array1.DataType = "myfirstxmltype";
        var array2 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array2.DataType = "myfirstxmltype";

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentDataTypes()
    {
        var array1 = new XmlArrayItemAttribute();
        array1.DataType = "typeone";
        var array2 = new XmlArrayItemAttribute();
        array2.DataType = "typetwo";

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNestingLevel()
    {
        var array1 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array1.NestingLevel = 1;
        var array2 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array2.NestingLevel = 1;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNestingLevels()
    {
        var array1 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array1.NestingLevel = 1;
        var array2 = new XmlArrayItemAttribute("myname", typeof(SerializeMe));
        array2.NestingLevel = 2;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameElementName()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        var array2 = new XmlArrayItemAttribute("myname");

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentTypes()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        var array2 = new XmlArrayItemAttribute("myname");

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMeToo), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentElementNames()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        var array2 = new XmlArrayItemAttribute("myothername");

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNamespace()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.Namespace = "mynamespace";

        var array2 = new XmlArrayItemAttribute("myname");
        array2.Namespace = "mynamespace";

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNamespace()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.Namespace = "mynamespace";
        var array2 = new XmlArrayItemAttribute("myname");
        array2.Namespace = "myothernamespace";

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNullable()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.IsNullable = true;
        var array2 = new XmlArrayItemAttribute("myname");
        array2.IsNullable = true;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNullable()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.IsNullable = true;
        var array2 = new XmlArrayItemAttribute("myname");
        array2.IsNullable = false;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameForm()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var array2 = new XmlArrayItemAttribute("myname");
        array2.Form = System.Xml.Schema.XmlSchemaForm.Qualified;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentForm()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        array1.Form = System.Xml.Schema.XmlSchemaForm.Qualified;
        var array2 = new XmlArrayItemAttribute("myname");
        array2.Form = System.Xml.Schema.XmlSchemaForm.Unqualified;

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameMemberName()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        var array2 = new XmlArrayItemAttribute("myname");

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheMember", atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentMemberName()
    {
        var array1 = new XmlArrayItemAttribute("myname");
        var array2 = new XmlArrayItemAttribute("myname");

        atts1.XmlArrayItems.Add(array1);
        atts2.XmlArrayItems.Add(array2);

        ov1.Add(typeof(SerializeMe), "TheMember", atts1);
        ov2.Add(typeof(SerializeMe), "TheOtherMember", atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }


}
