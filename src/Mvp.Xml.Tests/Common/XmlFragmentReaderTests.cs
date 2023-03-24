using System;
using System.IO;
using System.Xml;
using Xunit;

namespace Mvp.Xml.Tests.Common;

public class XmlFragmentReaderTests
{
    [Fact]
    public void ThrowsIfRootNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new XmlFragmentReader((string)null, XmlReader.Create(new StringReader("<item>foo</item>"))));
    }

    [Fact]
    public void ThrowsIfRootNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new XmlFragmentReader(string.Empty, XmlReader.Create(new StringReader("<item>foo</item>"))));
    }

    [Fact]
    public void ThrowsIfRootNamespaceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new XmlFragmentReader("Foo", null, XmlReader.Create(new StringReader("<item>foo</item>"))));
    }

    [Fact]
    public void RootNamespaceCanBeEmpty()
    {
        new XmlFragmentReader("Foo", string.Empty, XmlReader.Create(new StringReader("<item>foo</item>")));
    }

    [Fact]
    public void ThrowsIfNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new XmlFragmentReader((XmlQualifiedName)null, XmlReader.Create(new StringReader("<item>foo</item>"))));
    }

    [Fact]
    public void CanFakeRoot()
    {
        var qname = new XmlQualifiedName("foo", "mvp-xml");
        XmlReader reader = new XmlFragmentReader(qname, XmlReader.Create(new StringReader("<item id='1'>foo</item>")));

        Assert.Equal(ReadState.Initial, reader.ReadState);
        Assert.True(reader.Read());
        Assert.Equal(qname.Name, reader.LocalName);
        Assert.Equal(qname.Namespace, reader.NamespaceURI);
        Assert.False(reader.HasAttributes);
        Assert.True(reader.Read());
        Assert.Equal("item", reader.LocalName);
        reader.Skip();
        Assert.Equal(qname.Name, reader.LocalName);
        Assert.Equal(qname.Namespace, reader.NamespaceURI);
        Assert.False(reader.Read());
        Assert.Equal(ReadState.EndOfFile, reader.ReadState);
    }

    [Fact]
    public void RootNameMatchesFake()
    {
        XmlReader reader = new XmlFragmentReader("foo", XmlReader.Create(new StringReader("<item/>")));

        Assert.True(reader.Read());
        Assert.Equal("foo", reader.LocalName);
    }

    [Fact]
    public void RootNamespaceMatchesFake()
    {
        XmlReader reader = new XmlFragmentReader("foo", "mvp-xml", XmlReader.Create(new StringReader("<item/>")));

        Assert.True(reader.Read());
        Assert.Equal("mvp-xml", reader.NamespaceURI);
    }

    [Fact]
    public void NamespaceURIRestoredAfterFake()
    {
        XmlReader reader = new XmlFragmentReader("foo", "mvp-xml", XmlReader.Create(new StringReader("<item/>")));

        Assert.True(reader.Read());
        Assert.True(reader.Read());
        Assert.Equal(String.Empty, reader.NamespaceURI);
    }

    [Fact]
    public void CannotReadPastFake()
    {
        XmlReader reader = new XmlFragmentReader("foo", "mvp-xml", XmlReader.Create(new StringReader("<item/>")));

        Assert.True(reader.Read());
        Assert.True(reader.Read());
        Assert.True(reader.Read());
        Assert.Equal("mvp-xml", reader.NamespaceURI);
        Assert.False(reader.Read());
    }
}
