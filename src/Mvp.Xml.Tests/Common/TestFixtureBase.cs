using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Mvp.Xml.Tests.Common;

public abstract class TestFixtureBase
{
    [Conditional("DEBUG")]
    protected void WriteIfDebugging(string message)
    {
        if (Debugger.IsAttached)
        {
            Debug.WriteLine(message);
        }
    }

    protected static string NormalizeFormat(string xml)
    {
        return ReadToEnd(GetReader(xml));
    }

    protected static XmlReader GetReader(string xml)
    {
        var settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        settings.CheckCharacters = true;
        settings.ConformanceLevel = ConformanceLevel.Auto;

        return XmlReader.Create(new StringReader(xml), settings);
    }

    protected static string ReadToEnd(XmlReader reader)
    {
        var sw = new StringWriter();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = true;
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        var writer = XmlWriter.Create(sw, settings);
        writer.WriteNode(reader, false);
        writer.Close();
        return sw.ToString();
    }
}
