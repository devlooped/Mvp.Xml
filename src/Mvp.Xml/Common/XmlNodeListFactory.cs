using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.Common;

/// <summary>
/// Constructs <see cref="XmlNodeList"/> instances from 
/// <see cref="XPathNodeIterator"/> objects.
/// </summary>
/// <remarks>See http://weblogs.asp.net/cazzu/archive/2004/04/14/113479.aspx. 
/// <para>Author: Daniel Cazzulino, <a href="https://cazzulino.com">blog</a></para>
/// <para>Contributors: Oleg Tkachenko, http://www.xmllab.net</para>
/// </remarks>
public static class XmlNodeListFactory
{
    /// <summary>
    /// Creates an instance of a <see cref="XmlNodeList"/> that allows 
    /// enumerating <see cref="XmlNode"/> elements in the iterator.
    /// </summary>
    /// <param name="iterator">The result of a previous node selection 
    /// through an <see cref="XPathNavigator"/> query.</param>
    /// <returns>An initialized list ready to be enumerated.</returns>
    /// <remarks>The underlying XML store used to issue the query must be 
    /// an object inheriting <see cref="XmlNode"/>, such as 
    /// <see cref="XmlDocument"/>.</remarks>
    public static XmlNodeList CreateNodeList(XPathNodeIterator iterator) => new XmlNodeListIterator(iterator);

    class XmlNodeListIterator : XmlNodeList
    {
        readonly XPathNodeIterator iterator;
        readonly IList<XmlNode> nodes = new List<XmlNode>();

        public XmlNodeListIterator(XPathNodeIterator iterator) => this.iterator = iterator.Clone();

        public override IEnumerator GetEnumerator() => new XmlNodeListEnumerator(this);

        public override XmlNode Item(int index)
        {
            if (index >= nodes.Count)
                ReadTo(index);

            // Compatible behavior with .NET
            if (index >= nodes.Count || index < 0)
                return null;

            return nodes[index];
        }

        public override int Count
        {
            get
            {
                if (!Done)
                    ReadToEnd();

                return nodes.Count;
            }
        }


        /// <summary>
        /// Reads the entire iterator.
        /// </summary>
        void ReadToEnd()
        {
            while (iterator.MoveNext())
            {
                // Check IHasXmlNode interface.
                if (!(iterator.Current is IHasXmlNode node))
                    throw new ArgumentException(Properties.Resources.XmlNodeListFactory_IHasXmlNodeMissing);

                nodes.Add(node.GetNode());
            }

            Done = true;
        }

        /// <summary>
        /// Reads up to the specified index, or until the 
        /// iterator is consumed.
        /// </summary>
        void ReadTo(int to)
        {
            while (nodes.Count <= to)
            {
                if (iterator.MoveNext())
                {
                    // Check IHasXmlNode interface.
                    if (!(iterator.Current is IHasXmlNode node))
                        throw new ArgumentException(Properties.Resources.XmlNodeListFactory_IHasXmlNodeMissing);

                    nodes.Add(node.GetNode());
                }
                else
                {
                    Done = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Flags that the iterator has been consumed.
        /// </summary>
        bool Done { get; set; }

        /// <summary>
        /// Current count of nodes in the iterator (read so far).
        /// </summary>
        int CurrentPosition => nodes.Count;

        class XmlNodeListEnumerator : IEnumerator
        {
            readonly XmlNodeListIterator iterator;
            int position = -1;

            public XmlNodeListEnumerator(XmlNodeListIterator iterator) => this.iterator = iterator;

            void IEnumerator.Reset() => position = -1;

            bool IEnumerator.MoveNext()
            {
                position++;
                iterator.ReadTo(position);

                // If we reached the end and our index is still 
                // bigger, there're no more items.
                return !iterator.Done || position < iterator.CurrentPosition;
            }

            object IEnumerator.Current => iterator[position];
        }
    }
}
