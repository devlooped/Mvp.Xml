using System;
using System.Text;
using System.Xml.Serialization;

namespace Mvp.Xml.Common.Serialization;

/// <summary>
/// The CacheKeyFactory extracts a unique signature
/// to identify each instance of an XmlSerializer
/// in the cache.
/// </summary>
public static class CacheKeyFactory
{

    /// <summary>
    /// Creates a unique signature for the passed
    /// in parameter. MakeKey normalizes array content
    /// and the content of the XmlAttributeOverrides before
    /// creating the key.
    /// </summary>
    public static string MakeKey(Type type, XmlAttributeOverrides overrides, Type[] types, XmlRootAttribute root, string defaultNamespace)
        => new StringBuilder()
            .Append(type.FullName)
            .Append("??")
            .Append(SignatureExtractor.GetOverridesSignature(overrides))
            .Append("??")
            .Append(SignatureExtractor.GetTypeArraySignature(types))
            .Append("??")
            .Append(SignatureExtractor.GetXmlRootSignature(root))
            .Append("??")
            .Append(SignatureExtractor.GetDefaultNamespaceSignature(defaultNamespace))
            .ToString();
}
