using System.Xml.XPath;

namespace Mvp.Xml.XPath;

/// <summary>
/// Enables a class to return an <see cref="XPathNavigator"/> from the current context or position.
/// </summary>
/// <remarks>
/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
/// </remarks>
public interface IHasXPathNavigator
{
    /// <summary>
    /// Returns the <see cref="XPathNavigator"/> for the current context or position.
    /// </summary>        
    XPathNavigator GetNavigator();
}
