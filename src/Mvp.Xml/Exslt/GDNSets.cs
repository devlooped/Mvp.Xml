using System.Collections.Generic;
using System.Xml.XPath;
//using System.Web.UI;

using Mvp.Xml.Common.XPath;

namespace Mvp.Xml.Exslt;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///   This class implements additional functions in the http://gotdotnet.com/exslt/sets namespace.
/// </summary>		
public class GdnSets
{
    /// <summary>
    /// Implements the following function 
    ///    boolean subset(node-set, node-set) 
    /// </summary>
    /// <param name="nodeset1">An input nodeset</param>
    /// <param name="nodeset2">Another input nodeset</param>
    /// <returns>True if all the nodes in the first nodeset are contained 
    /// in the second nodeset</returns>
    /// <remarks>THIS FUNCTION IS NOT PART OF EXSLT!!!</remarks>
    public bool Subset(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2)
    {
        if (nodeset1.Count > 125 || nodeset2.Count > 125)
            return Subset2(nodeset1, nodeset2);

        //else
        var nodelist1 = new XPathNavigatorIterator(nodeset1, true);
        var nodelist2 = new XPathNavigatorIterator(nodeset2, true);

        foreach (XPathNavigator nav in nodelist1)
        {
            if (!nodelist2.Contains(nav))
                return false;
        }

        return true;
    }

    public bool subset(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2) => Subset(nodeset1, nodeset2);

    /// <summary>
    /// Implements the following function 
    ///    boolean subset(node-set, node-set) 
    /// This is an optimized version, using document identification
    /// and binary search techniques. 
    /// </summary>
    /// <param name="nodeset1">An input nodeset</param>
    /// <param name="nodeset2">Another input nodeset</param>
    /// <returns>True if all the nodes in the first nodeset are contained 
    /// in the second nodeset</returns>
    /// <author>Dimitre Novatchev</author>
    /// <remarks>THIS FUNCTION IS NOT PART OF EXSLT!!!</remarks>
    public bool Subset2(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2)
    {
        var arDocs = new List<DocPair>();
        var arNodes2 = new List<XPathNavigator>(nodeset2.Count);

        while (nodeset2.MoveNext())
        {
            arNodes2.Add(nodeset2.Current.Clone());
        }

        AuxExslt.FindDocs(arNodes2, arDocs);

        while (nodeset1.MoveNext())
        {
            var currNode = nodeset1.Current;
            if (!AuxExslt.FindNode(arNodes2, arDocs, currNode))
                return false;
        }

        return true;
    }
}
