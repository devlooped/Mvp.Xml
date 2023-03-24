using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.XPath;

namespace Mvp.Xml.XPointer;

/// <summary>
/// xpath1() scheme based XPointer pointer part.
/// </summary>
class XPath1SchemaPointerPart : PointerPart
{
    string xpath;

    /// <summary>
    /// Evaluates <see cref="XPointer"/> pointer part and returns pointed nodes.
    /// </summary>
    /// <param name="doc">Document to evaluate pointer part on</param>
    /// <param name="nm">Namespace manager</param>
    /// <returns>Pointed nodes</returns>		    
    public override XPathNodeIterator Evaluate(XPathNavigator doc, XmlNamespaceManager nm)
    {
        try
        {
            return XPathCache.Select(xpath, doc, nm);
        }
        catch
        {
            return null;
        }
    }

    public static XPath1SchemaPointerPart ParseSchemaData(XPointerLexer lexer)
    {
        var part = new XPath1SchemaPointerPart();
        try
        {
            part.xpath = lexer.ParseEscapedData();
        }
        catch (Exception e)
        {
            throw new XPointerSyntaxException(string.Format(
                CultureInfo.CurrentCulture,
                Properties.Resources.SyntaxErrorInXPath1SchemeData,
                e.Message));
        }
        return part;
    }
}
