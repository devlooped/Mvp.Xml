using System.Collections.Generic;
using System.IO;
using System.Xml;
using Mvp.Xml.Common.Xsl;
using Xunit;

namespace Mvp.Xml.Tests.CharacterMappingXmlWriterTests;

public class Tests
{
    [Fact]
    public void TestShouldReplaceInText()
    {
        var mapping = new Dictionary<char, string>();
        mapping.Add('f', "FOO");
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;
        var writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
        writer.WriteElementString("foo", "fgh");
        writer.Close();
        Assert.True(sw.ToString() == "<foo>FOOgh</foo>");
    }

    [Fact]
    public void TestShouldReplaceInText2()
    {
        var mapping = new Dictionary<char, string>();
        mapping.Add('f', "FOO");
        mapping.Add('z', "ZzZ");
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;
        var writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
        writer.WriteElementString("foo", "abcd z efgh f zzz.");
        writer.Close();
        Assert.True(sw.ToString() == "<foo>abcd ZzZ eFOOgh FOO ZzZZzZZzZ.</foo>");
    }

    [Fact]
    public void TestShouldReplaceInAttribute()
    {
        var mapping = new Dictionary<char, string>();
        mapping.Add('f', "FOO");
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;
        var writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
        writer.WriteStartElement("foo");
        writer.WriteAttributeString("bar", "fghj");
        writer.WriteEndElement();
        writer.Close();
        Assert.True(sw.ToString() == "<foo bar=\"FOOghj\" />");
    }

    [Fact]
    public void TestShouldNotEscape()
    {
        var mapping = new Dictionary<char, string>();
        mapping.Add('(', "<");
        mapping.Add(')', ">");
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;
        var writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
        writer.WriteElementString("foo", "(%= bar%)");
        writer.Close();
        Assert.True(sw.ToString() == "<foo><%= bar%></foo>");
    }

    [Fact]
    public void TestShouldNotEscapeInAttribute()
    {
        var mapping = new Dictionary<char, string>();
        mapping.Add('(', "<");
        mapping.Add(')', ">");
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = false;
        var writer = new CharacterMappingXmlWriter(XmlWriter.Create(sw, settings), mapping);
        writer.WriteStartElement("foo");
        writer.WriteAttributeString("bar", "(%= bar%)");
        writer.WriteEndElement();
        writer.Close();
        Assert.True(sw.ToString() == "<foo bar=\"<%= bar%>\" />");
    }
}
