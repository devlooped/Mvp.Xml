using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Mvp.Xml.Tests;
using Xunit;

namespace Mvp.Xml.XPointer.Test;

/// <summary>
/// Unit tests for XPointerReader class.
/// </summary>

public class XPointerReaderTests
{
    XmlReaderSettings readerSettings;

    /// <summary>
    /// Test init
    /// </summary>
    public XPointerReaderTests()
    {
        readerSettings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Parse,
            IgnoreWhitespace = true,
            XmlResolver = new XmlUrlResolver
            {
                Credentials = CredentialCache.DefaultCredentials
            }
        };
        //readerSettings.ProhibitDtd = false;
    }

    /// <summary>
    /// xmlns() + xpath1() + namespaces works
    /// </summary>
    [Fact]
    public void XmlNsXPath1SchemeTest()
    {
        var xptr = "xmlns(m=mvp-xml)xpath1(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)";
        using var reader = XmlReader.Create(
            Globals.GetResource(Globals.PubsNsResource),
            readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var sb = new StringBuilder();
        while (xpr.Read())
        {
            sb.Append(xpr.ReadOuterXml());
        }
        var expected = @"<pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name>";
        Assert.Equal(sb.ToString(), expected);
    }

    /// <summary>
    /// xpath1() + namespaces doesn't work w/o xmlns()
    /// </summary>
    [Fact]
    public void XPath1SchemeWithoutXmlnsTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpath1(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            var sb = new StringBuilder();
            while (xpr.Read())
            {
                sb.Append(xpr.ReadOuterXml());
            }
            var expected = @"<pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name>";
            Assert.Equal(sb.ToString(), expected);
        });
    }

    /// <summary>
    /// xpath1() that doesn't select a node w/o namespaces
    /// </summary>
    [Fact]
    public void XPath1SchemeNoSelectedNodeTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpath1(no-such-node/foo)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            while (xpr.Read()) { }
        });
    }

    /// <summary>
    /// xpath1() that returns scalar value, not a node
    /// </summary>
    [Fact]
    public void XPath1SchemeScalarResultTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpath1(2+2)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            while (xpr.Read()) { }
        });
    }

    /// <summary>
    /// xmlns() + xpointer() + namespaces works
    /// </summary>
    [Fact]
    public void XmlNsXPointerSchemeTest()
    {
        var xptr = "xmlns(m=mvp-xml)xpointer(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)";
        using var reader = XmlReader.Create(
            Globals.GetResource(Globals.PubsNsResource),
            readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var sb = new StringBuilder();
        while (xpr.Read())
        {
            sb.Append(xpr.ReadOuterXml());
        }
        var expected = @"<pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name>";
        Assert.Equal(sb.ToString(), expected);
    }

    /// <summary>
    /// xpointer() + namespaces doesn't work w/o xmlns()
    /// </summary>
    [Fact]
    public void XPointerSchemeWithoutXmlnsTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpointer(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            var sb = new StringBuilder();
            while (xpr.Read())
            {
                sb.Append(xpr.ReadOuterXml());
            }
            var expected = @"<pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name>";
            Assert.Equal(sb.ToString(), expected);
        });
    }

    /// <summary>
    /// xpointer() that doesn't select a node w/o namespaces
    /// </summary>
    [Fact]
    public void XPointerSchemeNoSelectedNodeTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpointer(no-such-node/foo)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            while (xpr.Read()) { }
        });
    }

    /// <summary>
    /// xpointer() that returns scalar value, not a node
    /// </summary>
    [Fact]
    public void XPointerSchemeScalarResultTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpointer(2+2)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            while (xpr.Read()) { }
        });
    }

    /// <summary>
    /// superfluous xmlns() doesn't hurt
    /// </summary>
    [Fact]
    public void SuperfluousXmlNsSchemeTest()
    {
        var xptr = "xmlns(m=mvp-xml)xpointer(dsPubs/publishers[pub_id='1389']/pub_name)";
        using var reader = XmlReader.Create(
            Globals.GetResource(Globals.PubsResource),
            readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var sb = new StringBuilder();
        while (xpr.Read())
        {
            sb.Append(xpr.ReadOuterXml());
        }
        var expected = @"<pub_name>Algodata Infosystems</pub_name>";
        Assert.Equal(sb.ToString(), expected);
    }

    /// <summary>
    /// xpointer() + xmlns() + namespaces doesn't work
    /// </summary>
    [Fact]
    public void XmlnsAfterTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "xpointer(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)xmlns(m=mvp-xml)";
            using var reader = XmlReader.Create(
                Globals.GetResource(Globals.PubsNsResource),
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
            while (xpr.Read()) { }
        });
    }

    /// <summary>
    /// namespace re3efinition doesn't hurt
    /// </summary>
    [Fact]
    public void NamespaceRedefinitionTest()
    {
        var xptr = "xmlns(m=mvp-xml)xmlns(m=http://foo.com)xmlns(m=mvp-xml)xpointer(m:dsPubs/m:publishers[m:pub_id='1389']/m:pub_name)";
        using var reader = XmlReader.Create(
            Globals.GetResource(Globals.PubsNsResource),
            readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var sb = new StringBuilder();
        while (xpr.Read())
        {
            sb.Append(xpr.ReadOuterXml());
        }
        var expected = @"<pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name><pub_name xmlns=""mvp-xml"">Algodata Infosystems</pub_name>";
        Assert.Equal(sb.ToString(), expected);
    }

    /// <summary>
    /// Shorthand pointer works
    /// </summary>
    [Fact]
    public void ShorthandTest()
    {
        var xptr = "o10535";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml",
            readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<Item orderID=""o10535""><OrderDate> 6/13/95</OrderDate><ShipAddress> Mataderos  2312</ShipAddress></Item>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means shorthand XPointer didn't work as expected.");
    }

    /// <summary>
    /// Shorthand pointer works via stream
    /// </summary>
    [Fact]
    public void ShorthandViaStreamTest()
    {
        var xptr = "o10535";
        var file = new FileInfo("../../XPointer/northwind.xml");
        using var fs = file.OpenRead();
        var xpr = new XPointerReader(
            XmlReader.Create(fs, readerSettings, file.FullName), xptr);
        var expected = @"<Item orderID=""o10535""><OrderDate> 6/13/95</OrderDate><ShipAddress> Mataderos  2312</ShipAddress></Item>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means shorthand XPointer didn't work as expected.");
    }

    /// <summary>
    /// Shorthand pointer points to nothing
    /// </summary>
    [Fact]
    public void ShorthandNotFoundTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "no-such-id";
            using var reader = XmlReader.Create(
                "../../XPointer/northwind.xml",
                readerSettings);
            var xpr = new XPointerReader(reader, xptr);
        });
    }

    /// <summary>
    /// element() scheme pointer works
    /// </summary>
    [Fact]
    public void ElementSchemeTest()
    {
        var xptr = "element(o10535)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<Item orderID=""o10535""><OrderDate> 6/13/95</OrderDate><ShipAddress> Mataderos  2312</ShipAddress></Item>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// element() scheme pointer works
    /// </summary>
    [Fact]
    public void ElementSchemeTest2()
    {
        var xptr = "element(o10535/1)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<OrderDate> 6/13/95</OrderDate>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// element() scheme pointer works
    /// </summary>
    [Fact]
    public void ElementSchemeTest3()
    {
        var xptr = "element(/1/1/2)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<CompanyName> Alfreds Futterkiste</CompanyName>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// element() scheme pointer points to nothing
    /// </summary>
    [Fact]
    public void ElementSchemeNotFoundTest()
    {
        Assert.Throws<NoSubresourcesIdentifiedException>(() =>
        {
            var xptr = "element(no-such-id)";
            using var reader = XmlReader.Create(
                "../../XPointer/northwind.xml", readerSettings);
            var xpr = new XPointerReader(reader, xptr);
        });
    }

    /// <summary>
    /// compound pointer
    /// </summary>
    [Fact]
    public void CompoundPointerTest()
    {
        var xptr = "xmlns(p=12345)xpath1(/no/such/node) xpointer(/and/such) element(/1/1/2) element(o10535/1)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<CompanyName> Alfreds Futterkiste</CompanyName>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// Unknown scheme pointer
    /// </summary>
    [Fact]
    public void UnknownSchemeTest()
    {
        var xptr = "dummy(foo) element(/1/1/2)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<CompanyName> Alfreds Futterkiste</CompanyName>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// Unknown scheme pointer
    /// </summary>
    [Fact]
    public void UnknownSchemeTest2()
    {
        var xptr = "foo:dummy(bar) element(/1/1/2)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<CompanyName> Alfreds Futterkiste</CompanyName>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// Unknown scheme pointer
    /// </summary>
    [Fact]
    public void UnknownSchemeTest3()
    {
        var xptr = "xmlns(foo=http://foo.com/schemas)foo:dummy(bar) element(/1/1/2)";
        using var reader = XmlReader.Create(
            "../../XPointer/northwind.xml", readerSettings);
        var xpr = new XPointerReader(reader, xptr);
        var expected = @"<CompanyName> Alfreds Futterkiste</CompanyName>";
        while (xpr.Read())
        {
            Assert.Equal(xpr.ReadOuterXml(), expected);
            return;
        }
        throw new InvalidOperationException("This means XPointer didn't work as expected.");
    }

    /// <summary>
    /// XSD-defined ID
    /// </summary>
    //[Fact]        
    //public void XSDDefnedIDTest() 
    //{           
    //    string xptr = "element(id1389/1)";                        
    //    XmlReader reader = XmlReader.Create("../../pubsNS.xml");
    //    XPointerReader xpr = new XPointerReader(reader, xptr, true);
    //    string expected = @"<pub_name>Algodata Infosystems</pub_name>";
    //    while (xpr.Read()) 
    //    {
    //        Assert.Equal(xpr.ReadOuterXml(), expected);
    //        return;
    //    }            
    //    throw new InvalidOperationException("This means XPointer didn't work as expected.");
    //}      
}
