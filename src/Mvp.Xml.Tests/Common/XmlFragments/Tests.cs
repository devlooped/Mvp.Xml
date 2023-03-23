using System.IO;
using System.Xml;
using Mvp.Xml.Common;
using Xunit;

namespace Mvp.Xml.Tests.XmlFragments;


public class Tests
{
    [Fact]
    [System.Obsolete]
    public void ReadFragments()
    {
        var doc = new XmlDocument();
        using Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
        doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs)));
    }

    [Fact]
    [System.Obsolete]
    public void ReadFragmentsRoot()
    {
        var doc = new XmlDocument();
        using (Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs, "pubs")));
        }

        Assert.Equal("pubs", doc.DocumentElement.LocalName);
    }

    [Fact]
    [System.Obsolete]
    public void ReadFragmentsRootNs()
    {
        var doc = new XmlDocument();
        using Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
        doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs, "pubs", "mvp-xml")));
    }
}
