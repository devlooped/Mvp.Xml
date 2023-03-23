using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using Mvp.Xml.Common.XPath;

namespace Mvp.Xml.XPointer;

/// <summary>
/// Shorthand XPointer pointer.
/// </summary>
class ShorthandPointer : Pointer
{
    readonly string ncName;

    /// <summary>
    /// Creates shorthand XPointer given bare name.
    /// </summary>
    /// <param name="n">Shorthand (bare name)</param>
    public ShorthandPointer(string n) => ncName = n;

    /// <summary>
    /// Evaluates <see cref="XPointer"/> pointer and returns 
    /// iterator over pointed nodes.
    /// </summary>
    /// <remarks>Note, that returned XPathNodeIterator is already moved once.</remarks>
    /// <param name="nav">XPathNavigator to evaluate the 
    /// <see cref="XPointer"/> on.</param>
    /// <returns><see cref="XPathNodeIterator"/> over pointed nodes</returns>	    					
    public override XPathNodeIterator Evaluate(XPathNavigator nav)
    {
        var result = XPathCache.Select("id('" + ncName + "')", nav, (XmlNamespaceManager)null);
        if (result != null && result.MoveNext())
            return result;

        throw new NoSubresourcesIdentifiedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.NoSubresourcesIdentifiedException, ncName));
    }
}
