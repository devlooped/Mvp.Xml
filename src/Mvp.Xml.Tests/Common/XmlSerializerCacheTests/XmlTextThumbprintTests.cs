using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlTextThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlTextThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void SameType()
    {
        var text1 = new XmlTextAttribute(typeof(SerializeMe));
        var text2 = new XmlTextAttribute(typeof(SerializeMe));

        atts1.XmlText = text1;
        atts2.XmlText = text2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentTypes()
    {
        var text1 = new XmlTextAttribute(typeof(SerializeMe));
        var text2 = new XmlTextAttribute(typeof(SerializeMeToo));

        atts1.XmlText = text1;
        atts2.XmlText = text2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);

    }

    [Fact]
    public void SameDataType()
    {
        var text1 = new XmlTextAttribute();
        text1.DataType = "xmltype1";
        var text2 = new XmlTextAttribute();
        text2.DataType = "xmltype1";

        atts1.XmlText = text1;
        atts2.XmlText = text2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentDataTypes()
    {
        var text1 = new XmlTextAttribute();
        text1.DataType = "xmltype1";
        var text2 = new XmlTextAttribute();
        text2.DataType = "xmltype2";

        atts1.XmlText = text1;
        atts2.XmlText = text2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);

    }
}
