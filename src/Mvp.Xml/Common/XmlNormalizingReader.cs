using System.Xml;

namespace Mvp.Xml;

/// <summary>
/// Reader that only exposes xmlns attribute declarations if they 
/// have not been previously declared by a parent element, 
/// normalizing the output XML so that no duplicate namespace 
/// declarations exist.
/// </summary>
public class XmlNormalizingReader : XmlWrappingReader
{
    readonly XmlNamespaceManager nsManager;
    readonly string xmlNsNamespace;

    /// <summary>
    /// Initializes the normalizing reader with the 
    /// underlying reader to use.
    /// </summary>
    /// <param name="baseReader">Underlying reader to normalize.</param>
    public XmlNormalizingReader(XmlReader baseReader) : base(baseReader)
    {
        nsManager = new XmlNamespaceManager(baseReader.NameTable);
        xmlNsNamespace = nsManager.LookupNamespace("xmlns");
    }

    /// <summary>
    /// See <see cref="XmlReader.Read"/>.
    /// </summary>
    public override bool Read()
    {
        var read = base.Read();

        if (NodeType == XmlNodeType.Element)
        {
            nsManager.PushScope();
            for (var go = BaseReader.MoveToFirstAttribute(); go; go = BaseReader.MoveToNextAttribute())
            {
                if (BaseReader.NamespaceURI != xmlNsNamespace)
                    continue;

                var prefix = GetNamespacePrefix();

                // Only push if it's not already defined.
                if (nsManager.LookupNamespace(prefix) == null)
                    nsManager.AddNamespace(prefix, Value);
            }

            // If it had attributes, we surely moved through all of them searching for namespaces
            if (BaseReader.HasAttributes)
                BaseReader.MoveToElement();
        }
        else if (NodeType == XmlNodeType.EndElement)
        {
            nsManager.PopScope();
        }

        return read;
    }

    /// <summary>
    /// See <see cref="XmlReader.AttributeCount"/>.
    /// </summary>
    public override int AttributeCount
    {
        get
        {
            var count = 0;
            for (var go = MoveToFirstAttribute(); go; go = MoveToNextAttribute())
            {
                count++;
            }

            return count;
        }
    }

    /// <summary>
    /// See <see cref="XmlReader.MoveToFirstAttribute"/>.
    /// </summary>
    public override bool MoveToFirstAttribute()
    {
        var moved = base.MoveToFirstAttribute();
        while (moved && IsXmlNs && !IsLocalXmlNs)
        {
            moved = MoveToNextAttribute();
        }

        if (!moved)
            MoveToElement();

        return moved;
    }

    /// <summary>
    /// See <see cref="XmlReader.MoveToNextAttribute"/>.
    /// </summary>
    public override bool MoveToNextAttribute()
    {
        var moved = base.MoveToNextAttribute();
        while (moved && IsXmlNs && !IsLocalXmlNs)
        {
            moved = MoveToNextAttribute();
        }

        return moved;
    }

    bool IsXmlNs => NamespaceURI == xmlNsNamespace;

    bool IsLocalXmlNs => nsManager.GetNamespacesInScope(XmlNamespaceScope.Local).ContainsKey(GetNamespacePrefix());

    // This is not very intuitive, but it's how it works.
    // In the first case, a non-empty prefix is represented 
    // as an xmlns:foo="bar" declaration, where xmlns is the 
    // actual attribute prefix, and where the real prefix 
    // being declared is the reader localname (foo in this case).
    // If no prefix is being declared for the namespace, 
    // it's an xmlns="foo" declaration, therefore we pass empty string.
    string GetNamespacePrefix() => Prefix.Length > 0 ? LocalName : string.Empty;
}
