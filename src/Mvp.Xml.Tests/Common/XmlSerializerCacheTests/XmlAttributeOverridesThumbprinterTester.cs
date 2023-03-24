using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class SerializeMe
{
    public string MyString;
}

public class SerializeMeToo
{
    public string MyString;
}


public class XmlAttributeOverridesThumbprinterTester
{
    [Fact]
    public void SameEmptyObjectTwice()
    {
        // the same object should most certainly
        // result in the same signature, even when it's empty
        var ov = new XmlAttributeOverrides();
        ThumbprintHelpers.SameThumbprint(ov, ov);
    }

    [Fact]
    public void DifferentEmptyObjects()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void TwoDifferentEmptyObjects()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameObjectWithRootAttribute()
    {
        var ov = new XmlAttributeOverrides();
        var atts = new XmlAttributes();
        atts.XmlRoot = new XmlRootAttribute("myRoot");
        ov.Add(typeof(SerializeMe), atts);

        ThumbprintHelpers.SameThumbprint(ov, ov);
    }
    [Fact]
    public void TwoObjectsWithSameRootAttributeDifferentTypes()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();
        var atts1 = new XmlAttributes();
        atts1.XmlRoot = new XmlRootAttribute("myRoot");
        ov1.Add(typeof(SerializeMe), atts1);

        var atts2 = new XmlAttributes();
        atts2.XmlRoot = new XmlRootAttribute("myRoot");
        ov2.Add(typeof(SerializeMeToo), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void TwoObjectsWithDifferentRootAttribute()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var atts1 = new XmlAttributes();
        atts1.XmlRoot = new XmlRootAttribute("myRoot");
        ov1.Add(typeof(SerializeMe), atts1);

        var atts2 = new XmlAttributes();
        atts2.XmlRoot = new XmlRootAttribute("myOtherRoot");
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void TwoObjectsWithSameRootAttribute()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var atts1 = new XmlAttributes();
        atts1.XmlRoot = new XmlRootAttribute("myRoot");
        ov1.Add(typeof(SerializeMe), atts1);

        var atts2 = new XmlAttributes();
        atts2.XmlRoot = new XmlRootAttribute("myRoot");
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameXmlTypeTwice()
    {
        var ov = new XmlAttributeOverrides();
        var att = new XmlTypeAttribute("myType");

        var atts = new XmlAttributes();
        atts.XmlType = att;

        ov.Add(typeof(SerializeMe), atts);

        ThumbprintHelpers.SameThumbprint(ov, ov);
    }

    [Fact]
    public void SameXmlTypeDifferentObjects()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var att = new XmlTypeAttribute("myType");

        var atts1 = new XmlAttributes();
        atts1.XmlType = att;
        var atts2 = new XmlAttributes();
        atts2.XmlType = att;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);
        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }
    [Fact]
    public void DifferentXmlTypes()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var att1 = new XmlTypeAttribute("myType");
        var att2 = new XmlTypeAttribute("myOtherType");

        var atts1 = new XmlAttributes();
        atts1.XmlType = att1;
        var atts2 = new XmlAttributes();
        atts2.XmlType = att2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);
        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentTypesSameXmlTypes()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var att1 = new XmlTypeAttribute("myType");
        var att2 = new XmlTypeAttribute("myType");

        var atts1 = new XmlAttributes();
        atts1.XmlType = att1;
        var atts2 = new XmlAttributes();
        atts2.XmlType = att2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMeToo), atts2);
        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameMemberSameEmptyAttributes()
    {
        var ov1 = new XmlAttributeOverrides();
        var ov2 = new XmlAttributeOverrides();

        var att = new XmlAttributes();
        ov1.Add(typeof(SerializeMe), "TheMember", att);
        ov2.Add(typeof(SerializeMe), "TheMember", att);
        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameMemberSameEmptyAttibuteOverrides()
    {
        var ov1 = new XmlAttributeOverrides();

        var att = new XmlAttributes();
        ov1.Add(typeof(SerializeMe), "TheMember", att);
        ThumbprintHelpers.SameThumbprint(ov1, ov1);
    }


}
