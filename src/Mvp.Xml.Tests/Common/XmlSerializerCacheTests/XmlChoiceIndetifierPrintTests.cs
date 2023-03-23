using System;
using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

class ChoiceIdentifierAttributeProvider : System.Reflection.ICustomAttributeProvider
{

    string name;
    public ChoiceIdentifierAttributeProvider(string name)
    {
        this.name = name;
    }

    public object[] GetCustomAttributes(bool inherit)
    {
        return new object[] { new XmlChoiceIdentifierAttribute(name) };
    }

    public object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        object[] o = null;
        if (attributeType == typeof(XmlChoiceIdentifierAttribute))
        {
            o = new object[1];
            o[0] = new XmlChoiceIdentifierAttribute(name);
        }
        else
        {
            o = new object[0];
        }

        return o;
    }

    public bool IsDefined(Type attributeType, bool inherit)
    {
        var retVal = false;
        if (typeof(System.Xml.Serialization.XmlChoiceIdentifierAttribute) == attributeType)
        {
            retVal = true;
        }
        return retVal;
    }
}


public class XmlChoiceIndetifierPrintTests
{
    XmlAttributeOverrides ov1;
    XmlAttributeOverrides ov2;

    XmlAttributes atts1;
    XmlAttributes atts2;

    public XmlChoiceIndetifierPrintTests()
    {
        ov1 = new XmlAttributeOverrides();
        ov2 = new XmlAttributeOverrides();
    }

    [Fact]
    public void SameMemberName()
    {
        atts1 = new XmlAttributes(new ChoiceIdentifierAttributeProvider("myname"));
        atts2 = new XmlAttributes(new ChoiceIdentifierAttributeProvider("myname"));

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.SameThumbprint(ov1, ov2);
    }

    [Fact]
    public void DifferentMemberName()
    {
        atts1 = new XmlAttributes(new ChoiceIdentifierAttributeProvider("myname"));
        atts2 = new XmlAttributes(new ChoiceIdentifierAttributeProvider("myothername"));

        ov1.Add(typeof(SerializeMe), atts1);
        ov2.Add(typeof(SerializeMe), atts2);

        ThumbprintHelpers.DifferentThumbprint(ov1, ov2);

    }
}
