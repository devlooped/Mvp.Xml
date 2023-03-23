using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using Xunit;

namespace Mvp.Xml.Tests;

public class MvpXslTransformTests
{
    byte[] standardResult;
    byte[] resolverTestStandardResult;
    MvpXslTransform xslt, xslt2;
    XsltArgumentList args;

    public MvpXslTransformTests()
    {
        var xslt = new XslCompiledTransform();
        xslt.Load("../../Common/MvpXslTransformTests/test1.xslt");
        var ms = new MemoryStream();
        using (var r = GetReader(Globals.NorthwindResource))
        {
            xslt.Transform(r, Arguments, ms);
        }
        standardResult = ms.ToArray();
        var xslt2 = new XslCompiledTransform();
        xslt2.Load("../../Common/MvpXslTransformTests/resolver-test.xslt", XsltSettings.TrustedXslt, null);
        var ms2 = new MemoryStream();
        var w = XmlWriter.Create(ms2);
        using (var r2 = XmlReader.Create("../../Common/MvpXslTransformTests/test.xml"))
        {
            xslt2.Transform(r2, Arguments, w, new MyXmlResolver());
        }
        w.Close();
        resolverTestStandardResult = ms2.ToArray();
    }

    XsltArgumentList Arguments
    {
        get
        {
            if (args == null)
            {
                args = new XsltArgumentList();
                args.AddParam("prm1", "", "value1");
            }
            return args;
        }
    }

    MvpXslTransform GetMvpXslTransform()
    {
        if (xslt == null)
        {
            xslt = new MvpXslTransform();
            xslt.Load("../../Common/MvpXslTransformTests/test1.xslt");
        }
        return xslt;
    }

    MvpXslTransform GetMvpXslTransform2()
    {
        if (xslt2 == null)
        {
            xslt2 = new MvpXslTransform();
            xslt2.Load("../../Common/MvpXslTransformTests/resolver-test.xslt", XsltSettings.TrustedXslt, null);
        }
        return xslt2;
    }

    static void CompareResults(byte[] standard, byte[] test)
    {
        Assert.Equal(standard.Length, test.Length);
        for (var i = 0; i < standard.Length; i++)
        {
            Assert.True(standard[i] == test[i], string.Format("Values aren't equal: {0}, {1}, positoin {2}", standard[i], test[i], i));
        }
    }

    static XmlReader GetReader(string xml)
    {
        var s = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        //s.ProhibitDtd = false;
        return XmlReader.Create(Globals.GetResource(xml), s);
    }

    [Fact]
    public void TestStringInput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestStreamInput()
    {
        var xslt = GetMvpXslTransform();
        using var fs = File.OpenRead("../../Common/northwind.xml");
        var input = new XmlInput(fs);
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestTextReaderInput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(new StreamReader("../../Common/northwind.xml", Encoding.GetEncoding("windows-1252")));
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestXmlReaderInput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(XmlReader.Create("../../Common/northwind.xml"));
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(standardResult, ms.ToArray());
    }


    [Fact]
    public void TestIXPathNavigableInput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(new XPathDocument("../../Common/northwind.xml", XmlSpace.Preserve));
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestStringInput2()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        var ms = new MemoryStream();
        var r = xslt.Transform(input, Arguments);
        var w = XmlWriter.Create(ms);
        w.WriteNode(r, false);
        w.Close();
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestStreamInput2()
    {
        var xslt = GetMvpXslTransform();
        using var fs = File.OpenRead("../../Common/northwind.xml");
        var input = new XmlInput(fs);
        var ms = new MemoryStream();
        var r = xslt.Transform(input, Arguments);
        var w = XmlWriter.Create(ms);
        w.WriteNode(r, false);
        w.Close();
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestTextReaderInput2()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(new StreamReader("../../Common/northwind.xml", Encoding.GetEncoding("windows-1252")));
        var ms = new MemoryStream();
        var r = xslt.Transform(input, Arguments);
        var w = XmlWriter.Create(ms);
        w.WriteNode(r, false);
        w.Close();
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestXmlReaderInput2()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(XmlReader.Create("../../Common/northwind.xml"));
        var ms = new MemoryStream();
        var r = xslt.Transform(input, Arguments);
        var w = XmlWriter.Create(ms);
        w.WriteNode(r, false);
        w.Close();
        CompareResults(standardResult, ms.ToArray());
    }


    [Fact]
    public void TestIXPathNavigableInput2()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput(new XPathDocument("../../Common/northwind.xml", XmlSpace.Preserve));
        var ms = new MemoryStream();
        var r = xslt.Transform(input, Arguments);
        var w = XmlWriter.Create(ms);
        w.WriteNode(r, false);
        w.Close();
        CompareResults(standardResult, ms.ToArray());
    }

    [Fact]
    public void TestStringOutput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        xslt.Transform(input, Arguments, new XmlOutput("../../Common/MvpXslTransformTests/out.xml"));
        using var fs = File.OpenRead("../../Common/MvpXslTransformTests/out.xml");
        var bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        CompareResults(standardResult, bytes);
    }

    [Fact]
    public void TestStreamOutput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        using (var outStrm = File.OpenWrite("../../Common/MvpXslTransformTests/out.xml"))
        {
            xslt.Transform(input, Arguments, new XmlOutput(outStrm));
        }
        using var fs = File.OpenRead("../../Common/MvpXslTransformTests/out.xml");
        var bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        CompareResults(standardResult, bytes);
    }

    [Fact]
    public void TestTextWriterOutput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        TextWriter w = new StreamWriter("../../Common/MvpXslTransformTests/out.xml", false, Encoding.UTF8);
        xslt.Transform(input, Arguments, new XmlOutput(w));
        w.Close();
        using var fs = File.OpenRead("../../Common/MvpXslTransformTests/out.xml");
        var bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        CompareResults(standardResult, bytes);
    }

    [Fact]
    public void TestXmlWriterOutput()
    {
        var xslt = GetMvpXslTransform();
        var input = new XmlInput("../../Common/northwind.xml");
        var w = XmlWriter.Create("../../Common/MvpXslTransformTests/out.xml");
        xslt.Transform(input, Arguments, new XmlOutput(w));
        w.Close();
        using var fs = File.OpenRead("../../Common/MvpXslTransformTests/out.xml");
        var bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        CompareResults(standardResult, bytes);
    }

    [Fact]
    public void ResolverTestStringInput()
    {
        var xslt = GetMvpXslTransform2();
        var input = new XmlInput("../../Common/MvpXslTransformTests/test.xml", new MyXmlResolver());
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(resolverTestStandardResult, ms.ToArray());
    }

    [Fact]
    public void ResolverTestStreamInput()
    {
        var xslt = GetMvpXslTransform2();
        using var fs = File.OpenRead("../../Common/MvpXslTransformTests/test.xml");
        var input = new XmlInput(fs, new MyXmlResolver());
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(resolverTestStandardResult, ms.ToArray());
    }

    [Fact]
    public void ResolverTestTextReaderInput()
    {
        var xslt = GetMvpXslTransform2();
        var input = new XmlInput(new StreamReader("../../Common/MvpXslTransformTests/test.xml"), new MyXmlResolver());
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(resolverTestStandardResult, ms.ToArray());
    }

    [Fact]
    public void ResolverTestXmlReaderInput()
    {
        var xslt = GetMvpXslTransform2();
        var input = new XmlInput(XmlReader.Create("../../Common/MvpXslTransformTests/test.xml"), new MyXmlResolver());
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(resolverTestStandardResult, ms.ToArray());
    }

    [Fact]
    public void ResolverTestIXPathNavigableInput()
    {
        var xslt = GetMvpXslTransform2();
        var input = new XmlInput(new XPathDocument("../../Common/MvpXslTransformTests/test.xml"), new MyXmlResolver());
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        CompareResults(resolverTestStandardResult, ms.ToArray());
    }

    [Fact]
    public void ExsltTest()
    {
        var xslt = new MvpXslTransform();
        xslt.Load("../../Common/MvpXslTransformTests/exslt-test.xslt");
        var input = new XmlInput("../../Common/MvpXslTransformTests/test.xml");
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        var expected = "<out>3</out>";
        CompareResults(Encoding.ASCII.GetBytes(expected), ms.ToArray());
    }

    [Fact]
    public void NoExsltTest()
    {
        var xslt = new MvpXslTransform();
        xslt.Load("../../Common/MvpXslTransformTests/exslt-test.xslt");
        var input = new XmlInput("../../Common/MvpXslTransformTests/test.xml");
        var ms = new MemoryStream();
        xslt.SupportedFunctions = Mvp.Xml.Exslt.ExsltFunctionNamespace.None;
        try
        {
            xslt.Transform(input, Arguments, new XmlOutput(ms));
        }
        catch (XsltException) { return; }
        Assert.Fail("Shoudn't be here.");
    }

    [Fact]
    public void CharMapTest()
    {
        const string expected = "<out attr=\"a&nbsp;b\"><text>Some&nbsp;text, now ASP.NET <%# Eval(\"foo\") %> and more&nbsp;text.</text><foo attr=\"<data>\">text <%= fff() %> and more&nbsp;text.</foo></out>";

        var xslt = new MvpXslTransform { SupportCharacterMaps = true };
        xslt.Load("../../Common/MvpXslTransformTests/char-map.xslt");
        var input = new XmlInput(new StringReader("<foo attr=\"{data}\">text {%= fff() %} and more&#xA0;text.</foo>"));
        var sw = new StringWriter();
        xslt.Transform(input, Arguments, new XmlOutput(sw));

        Assert.Equal(expected, sw.ToString());
    }

    [Fact]
    public void CharMapTest2()
    {
        const string expected = "<out attr=\"a&nbsp;b\"><text>Some&nbsp;text, now ASP.NET <%# Eval(\"foo\") %> and more&nbsp;text.</text><foo attr=\"<data>\">text <%= fff() %> and more&nbsp;text.</foo></out>";

        var xslt = new MvpXslTransform { SupportCharacterMaps = true };
        xslt.Load("../../Common/MvpXslTransformTests/char-map.xslt");
        var input = new XmlInput(new StringReader("<foo attr=\"{data}\">text {%= fff() %} and more&#xA0;text.</foo>"));
        var ms = new MemoryStream();
        xslt.Transform(input, Arguments, new XmlOutput(ms));
        ms.Position = 0;
        var sr = new StreamReader(ms);

        Assert.Equal(expected, sr.ReadToEnd());
    }

    [Fact]
    public void CharMapTest3()
    {
        const string expected = "<out attr=\"a&nbsp;b\"><text>Some&nbsp;text, now ASP.NET <%# Eval(\"foo\") %> and more&nbsp;text.</text><foo attr=\"<data>\">text <%= fff() %> and more&nbsp;text.</foo></out>";

        var xslt = new MvpXslTransform { SupportCharacterMaps = true };
        xslt.Load("../../Common/MvpXslTransformTests/char-map.xslt");
        var input = new XmlInput(new StringReader("<foo attr=\"{data}\">text {%= fff() %} and more&#xA0;text.</foo>"));
        var sw = new StringWriter();
        var w = XmlWriter.Create(sw, xslt.OutputSettings);
        xslt.Transform(input, Arguments, new XmlOutput(w));
        w.Close();

        Assert.Equal(expected, sw.ToString());
    }


    [Fact]
    public void CharMapTest4()
    {
        const string expected = "<out attr=\"a&nbsp;b\"><text>Some&nbsp;text, now ASP.NET <%# Eval(\"foo\") %> and more&nbsp;text.</text><foo attr=\"<data>\">text <%= fff() %> and more&nbsp;text.</foo></out>";

        var xslt = new MvpXslTransform { SupportCharacterMaps = true };
        xslt.Load(XmlReader.Create("../../Common/MvpXslTransformTests/char-map.xslt"));
        var input = new XmlInput(new StringReader("<foo attr=\"{data}\">text {%= fff() %} and more&#xA0;text.</foo>"));
        var sw = new StringWriter();
        xslt.Transform(input, Arguments, new XmlOutput(sw));

        Assert.Equal(expected, sw.ToString());
    }

    [Fact]
    public void CharMapTest6()
    {
        const string expected = "<out attr=\"a&nbsp;b\"><text>Some&nbsp;text, now ASP.NET <%# Eval(\"foo\") %> and more&nbsp;text.</text><foo attr=\"<data>\">text <%= fff() %> and more&nbsp;text.</foo></out>";

        var xslt = new MvpXslTransform { SupportCharacterMaps = true };
        var d = new XPathDocument("../../Common/MvpXslTransformTests/char-map.xslt");
        xslt.Load(d);
        var input = new XmlInput(new StringReader("<foo attr=\"{data}\">text {%= fff() %} and more&#xA0;text.</foo>"));
        var sw = new StringWriter();
        xslt.Transform(input, Arguments, new XmlOutput(sw));

        Assert.Equal(expected, sw.ToString());
    }

    [Fact]
    public void XhtmlTest()
    {
        var xslt = new MvpXslTransform();
        xslt.Load("../../Common/MvpXslTransformTests/xhtml.xslt");
        xslt.EnforceXHTMLOutput = true;
        var input = new XmlInput(new StringReader("<foo/>"));
        var sw = new StringWriter();
        var w = XmlWriter.Create(sw, xslt.OutputSettings);
        xslt.Transform(input, Arguments, new XmlOutput(w));
        w.Close();
        Console.WriteLine(sw.ToString());
        Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?><!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Page title</title></head><body><p>Para</p><p></p><br /><p><img src=\"ddd\" /></p></body></html>", sw.ToString());
    }
}

public class MyXmlResolver : XmlUrlResolver
{
    public MyXmlResolver() { }

    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
        if (absoluteUri.Scheme == "my")
        {
            var xml = "<resolver>data</resolver>";
            return XmlReader.Create(new StringReader(xml));
        }
        else
        {
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}
