using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Xunit;

namespace Mvp.Xml.Tests;

public class XmlNodeFactoryTests
{
    static readonly XmlSerializer ser = new(typeof(XmlNode));

    [Fact]
    public void NodeFromReader()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><element>1</element><element></element><element>2</element></root>";
        var tr = new XmlTextReader(new StringReader(xml));

        var node = XmlNodeFactory.Create(tr);
        var mem = new MemoryStream();
        var tw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
        tw.Formatting = Formatting.None;

        ser.Serialize(tw, node);
        mem.Position = 0;
        var res = new StreamReader(mem).ReadToEnd();

        Assert.Equal(xml, res);
    }


    [Fact]
    public void NodeFromNavigator()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><element>1</element><element /><element>2</element></root>";
        var doc = new XPathDocument(new StringReader(xml));

        var nav = doc.CreateNavigator();
        var node = XmlNodeFactory.Create(nav);
        var mem = new MemoryStream();
        var tw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
        tw.Formatting = Formatting.None;

        ser.Serialize(tw, node);
        mem.Position = 0;
        var res = new StreamReader(mem).ReadToEnd();

        Assert.Equal(xml, res);

        nav.MoveToRoot();
        nav.MoveToFirstChild();
        nav.MoveToFirstChild();

        node = XmlNodeFactory.Create(nav);
        mem = new MemoryStream();
        tw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
        tw.Formatting = Formatting.None;

        ser.Serialize(tw, node);
        mem.Position = 0;
        res = new StreamReader(mem).ReadToEnd();

        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?><element>1</element>", res);
    }

    [Fact]
    public void NodeFromObject()
    {
        var cust = new Customer();
        cust.FirstName = "Daniel";
        cust.LastName = "Cazzulino";

        var node = XmlNodeFactory.Create(cust);
        var mem = new MemoryStream();
        var tw = new XmlTextWriter(mem, System.Text.Encoding.UTF8);
        tw.Formatting = Formatting.None;

        ser.Serialize(tw, node);
        mem.Position = 0;

        var customerSerializer = new XmlSerializer(typeof(Customer));
        var result = (Customer)customerSerializer.Deserialize(mem);

        Assert.Equal(cust.FirstName, result.FirstName);
        Assert.Equal(cust.LastName, result.LastName);
        Assert.Equal(cust.BirthDate, result.BirthDate);
    }

    public class Customer
    {
        public string FirstName;
        public string LastName;
        public DateTime BirthDate;
    }
}
