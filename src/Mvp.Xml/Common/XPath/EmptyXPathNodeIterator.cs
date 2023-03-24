using System.Xml.XPath;

namespace Mvp.Xml.XPath;

/// <summary>
/// Empty <see cref="XPathNodeIterator"/>, used to represent empty
/// node sequence. Can be used to return empty nodeset out of an XSLT or XPath extension function.
/// Implemented as a singleton.
/// </summary>
/// <remarks>
/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
/// </remarks>
public class EmptyXPathNodeIterator : XPathNodeIterator
{
    /// <summary>
    /// EmptyXPathNodeIterator instance.
    /// </summary>
    public static EmptyXPathNodeIterator Instance { get; } = new();

    /// <summary>
    /// This is a singleton, get an instance instead.
    /// </summary>
    EmptyXPathNodeIterator() { }

    /// <summary>
    /// See <see cref="XPathNodeIterator.Clone()"/>
    /// </summary>        
    public override XPathNodeIterator Clone() => this;

    /// <summary>
    /// Always 0. See <see cref="XPathNodeIterator.Count"/>
    /// </summary>
    public override int Count { get; } = 0;

    /// <summary>
    /// Always null. See <see cref="XPathNodeIterator.Current"/>
    /// </summary>
    public override XPathNavigator Current { get; } = null;

    /// <summary>
    /// Always 0. See <see cref="XPathNodeIterator.CurrentPosition"/>
    /// </summary>
    public override int CurrentPosition { get; } = 0;

    /// <summary>
    /// Always false. See <see cref="XPathNodeIterator.MoveNext()"/>
    /// </summary>
    public override bool MoveNext() => false;
}
