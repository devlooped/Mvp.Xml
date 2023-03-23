using System;
using System.Collections.Generic;
using System.Xml;

using Mvp.Xml.Common.Xsl;
using Xunit;

namespace Mvp.Xml.Tests.CharacterMappingXmlReaderTests;

public class Tests
{
    public static CharacterMappingXmlReader GetReader()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    public static CharacterMappingXmlReader GetReader2()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style2.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    public static CharacterMappingXmlReader GetReader3()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style3.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    public static CharacterMappingXmlReader GetReader4()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style4.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    public static CharacterMappingXmlReader GetReader5()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style5.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    public static CharacterMappingXmlReader GetReader6()
    {
        var baseReader = XmlReader.Create("../../Common/CharacterMappingXmlReaderTests/style6.xslt");
        return new CharacterMappingXmlReader(baseReader);
    }

    [Fact]
    public void TestReaderShoulReadCharMap()
    {
        var r = GetReader();
        while (r.Read()) ;
        var map = r.CompileCharacterMapping();
        Assert.NotNull(map);
        Assert.True(map.ContainsKey('\u00A0'));
        Assert.True(map['\u00A0'] == "&nbsp;");
    }

    [Fact]
    public void TestReaderShoulReadAllMaps()
    {
        var r = GetReader2();
        while (r.Read()) ;
        var map = r.CompileCharacterMapping();
        Assert.NotNull(map);
        Assert.True(map.ContainsKey('\u00A0'));
        Assert.True(map['\u00A0'] == "&nbsp;");
        Assert.True(map.ContainsKey('\u00A1'));
        Assert.True(map['\u00A1'] == "161");
        Assert.True(map.ContainsKey('\u00A2'));
        Assert.True(map['\u00A2'] == "162");
    }

    [Fact]
    public void TestReaderShouldCompileSingleMap()
    {
        var r = GetReader3();
        while (r.Read()) ;
        var map = r.CompileCharacterMapping();
        Assert.NotNull(map);
        Assert.True(map.ContainsKey('\u00A0'));
        Assert.True(map['\u00A0'] == "&nbsp;");
        Assert.True(map.ContainsKey('\u00A1'));
        Assert.True(map['\u00A1'] == "161");
        Assert.True(map.ContainsKey('\u00A2'));
        Assert.True(map['\u00A2'] == "162");
    }

    [Fact]
    public void TestDuplicate()
    {
        var r = GetReader5();
        try
        {
            while (r.Read()) ;
            Assert.Fail("Must be exception here.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [Fact]
    public void TestLoop()
    {
        var r = GetReader4();
        while (r.Read()) ;
        try
        {
            var map = r.CompileCharacterMapping();
            Assert.Fail("Should be exception");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [Fact]
    public void TestOverride()
    {
        var r = GetReader6();
        while (r.Read()) ;
        var map = r.CompileCharacterMapping();
        Assert.NotNull(map);
        Assert.True(map.ContainsKey('\u00A0'));
        Assert.True(map['\u00A0'] == "&nbsp2;");
        Assert.True(map.ContainsKey('\u00A1'));
        Assert.True(map['\u00A1'] == "161");
    }
}
