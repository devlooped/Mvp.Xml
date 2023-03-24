using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Xunit;

namespace Mvp.Xml.Tests.UpperLowerTests;


public class FirstUpperLowerTests
{
    [Fact]
    public void XmlFirstUpperReader()
    {
        var xml = "<customer id='1' pp:id='aba' xmlns='urn-kzu' xmlns:pp='urn-pepenamespace'><pp:order /><order id='1'>Chocolates</order></customer>";

        var fr = new XmlFirstUpperReader(new StringReader(xml));

        fr.MoveToContent();
        Assert.Equal("Customer", fr.LocalName);
        fr.MoveToFirstAttribute();
        Assert.Equal("Id", fr.LocalName);
        fr.MoveToNextAttribute();
        Assert.Equal("pp:Id", fr.Name);

        // Namespace ordering is not guaranteed.
        fr.MoveToNextAttribute();
        Assert.True(fr.Name == "xmlns" || fr.Name == "xmlns:pp");
        fr.MoveToNextAttribute();
        Assert.True(fr.Name == "xmlns" || fr.Name == "xmlns:pp");

        fr.MoveToElement();
        fr.Read();
        Assert.Equal("pp:Order", fr.Name);
    }

    [Fact]
    public void XmlFirstLowerWriter()
    {
        var xml = "<Customer Id=\"1\" pp:Id=\"aba\" xmlns=\"urn-kzu\" xmlns:pp=\"urn-pepenamespace\"><pp:Order /><Order Id=\"1\">chocolates</Order></Customer>";

        var tr = new XmlTextReader(new StringReader(xml));

        var sw = new StringWriter();
        var fw = new XmlFirstLowerWriter(sw);

        fw.WriteNode(tr, true);
        fw.Flush();

        Assert.Equal(xml.ToLower(), sw.ToString());
    }

    [Fact]
    public void Deserialization()
    {
        var fu = new XmlFirstUpperReader("../../Common/UpperLowerTests/Customer.xml");
        var settings = new XmlReaderSettings();
        settings.ValidationType = ValidationType.Schema;
        settings.Schemas.Add(XmlSchema.Read(XmlReader.Create("../../Common/UpperLowerTests/Customer.xsd"), null));
        var vr = XmlReader.Create(fu, settings);
        var ser = new XmlSerializer(typeof(Customer));
        var c = (Customer)ser.Deserialize(vr);

        Assert.Equal("0736", c.Id);
        Assert.Equal("Daniel Cazzulino", c.Name);
        Assert.Equal(25, c.Order.Id);
    }

    [Fact]
    public void Serialization()
    {
        var fu = new XmlFirstUpperReader("../../Common/UpperLowerTests/Customer.xml");
        var ser = new XmlSerializer(typeof(Customer));
        var c = (Customer)ser.Deserialize(fu);

        var sw = new StringWriter();
        var fl = new XmlFirstLowerWriter(sw);

        ser.Serialize(fl, c);

        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?><customer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" id=\"0736\" xmlns=\"mvp-xml-customer\"><name>Daniel Cazzulino</name><order id=\"25\" /></customer>",
                      sw.ToString());
    }
}