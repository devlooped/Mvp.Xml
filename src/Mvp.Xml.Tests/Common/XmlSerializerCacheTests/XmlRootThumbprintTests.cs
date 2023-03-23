using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlRootThumbprintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlRootThumbprintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();

        atts1 = new XmlAttributes();
        atts2 = new XmlAttributes();
    }

    [Fact]
    public void SameDataType()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.DataType = "myfirstxmltype";
        var root2 = new XmlRootAttribute("myname");
        root2.DataType = "myfirstxmltype";

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentDataType()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.DataType = "myfirstxmltype";
        var root2 = new XmlRootAttribute("myname");
        root2.DataType = "mysecondxmltype";

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameElementName()
    {
        var root1 = new XmlRootAttribute("myname");
        var root2 = new XmlRootAttribute("myname");

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentElementName()
    {
        var root1 = new XmlRootAttribute("myname");
        var root2 = new XmlRootAttribute("myothername");

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNullable()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.IsNullable = true;
        var root2 = new XmlRootAttribute("myname");
        root2.IsNullable = true;

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNullable()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.IsNullable = true;
        var root2 = new XmlRootAttribute("myname");
        root2.IsNullable = false;

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }

    [Fact]
    public void SameNamespace()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.Namespace = "mynamespace";

        var root2 = new XmlRootAttribute("myname");
        root2.Namespace = "mynamespace";

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentNamespace()
    {
        var root1 = new XmlRootAttribute("myname");
        root1.Namespace = "mynamespace";
        var root2 = new XmlRootAttribute("myname");
        root2.Namespace = "myothernamespace";

        atts1.XmlRoot = root1;
        atts2.XmlRoot = root2;

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);
    }
}
