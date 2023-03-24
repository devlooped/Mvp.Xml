using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.XPointer;

/// <summary>
/// SchemaBased XPointer pointer.
/// </summary>
class SchemaBasedPointer : Pointer
{
    readonly IList<PointerPart> parts;
    readonly string xpointer;

    /// <summary>
    /// Creates scheme based XPointer given list of pointer parts.
    /// </summary>
    /// <param name="parts">List of pointer parts</param>
    /// <param name="xpointer">String representation of the XPointer 
    /// (for error diagnostics)</param>
    public SchemaBasedPointer(IList<PointerPart> parts, string xpointer)
    {
        this.parts = parts;
        this.xpointer = xpointer;
    }

    /// <summary>
    /// Evaluates <see cref="XPointer"/> pointer and returns 
    /// iterator over pointed nodes.
    /// </summary>
    /// <param name="nav">XPathNavigator to evaluate the 
    /// <see cref="XPointer"/> on.</param>
    /// <returns><see cref="XPathNodeIterator"/> over pointed nodes</returns>	    					
    public override XPathNodeIterator Evaluate(XPathNavigator nav)
    {
        var nm = new XmlNamespaceManager(nav.NameTable);
        foreach (var part in parts)
        {
            var result = part.Evaluate(nav, nm);
            if (result != null && result.MoveNext())
                return result;
        }
        throw new NoSubresourcesIdentifiedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.NoSubresourcesIdentifiedException, xpointer));
    }
}
