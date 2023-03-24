﻿using System;
using System.Text;
using System.Xml.Serialization;

namespace Mvp.Xml.Serialization;

// TODO:
// I could test if the creating the hashvalue for the
// individual components of the key is faster than just building one
// long text key and then letting the hashtable do the hashing once.

/// <summary>
/// Helper methods to create the signature for 
/// the XmlSerializer parameters.
/// </summary>
public static class SignatureExtractor
{
    /// <summary>
    /// Returns a signature for the passed in namespace
    /// </summary>
    /// <param name="defaultNamespace"></param>
    /// <returns>signature for the passed in namespace</returns>
    public static string GetDefaultNamespaceSignature(string defaultNamespace) => defaultNamespace;

    /// <summary>
    /// Creates a signature for the passed in XmlRootAttribute
    /// </summary>
    /// <param name="root"></param>
    /// <returns>An instance indpendent signature of the XmlRootAttribute</returns>
    public static string GetXmlRootSignature(XmlRootAttribute root)
        => XmlAttributeOverridesThumbprinter.AddXmlRootPrint(root, new StringBuilder()).ToString();

    /// <summary>
    /// Creates a signature for the passed in XmlAttributeOverrides
    /// </summary>
    /// <param name="overrides"></param>
    /// <returns>An instance indpendent signature of the XmlAttributeOverrides</returns>
    /// <devdoc>
    /// GetHashCode looks at something other than the content
    /// of XmlOverrideAttributes and comes up with different
    /// hash values for two instances that would produce
    /// the same XML is the were applied to the XmlSerializer.
    /// Therefore I need to do something more intelligent
    /// to normalize the content of XmlAttributeOverrides
    /// or extract a thumbpront that only accounts for the
    /// attributes in XmlAttributeOverrides.
    /// 
    /// The main problems is that 
    /// I can only access the hashtables that store the 
    /// overriding attributes through reflection, i.e.
    /// I can't run the XmlSerializerCache in a partially
    /// trusted security context.
    ///
    /// Also, the extra computation to create a purely
    /// content-based thumbprint not offset the savings.
    /// 
    /// If none if these were an issue I'd simply say:
    /// return overrides.GetHashCode().ToString();
    /// </devdoc>
    public static string GetOverridesSignature(XmlAttributeOverrides overrides) =>
        null == overrides ? null : XmlAttributeOverridesThumbprinter.GetThumbprint(overrides);

    /// <summary>
    /// Creates a signature for the passed in Type array. The order
    /// of the elements in the array does not influence the signature.
    /// </summary>
    /// <param name="types"></param>
    /// <returns>An instance indpendent signature of the Type array</returns>
    public static string GetTypeArraySignature(Type[] types)
    {
        if (null == types || types.Length <= 0)
            return null;

        // to make sure we don't account for the order
        // of the types in the array, we create one SortedList 
        // with the type names, concatenate them and hash that.
        var sorter = new StringSorter();
        foreach (var t in types)
        {
            sorter.AddString(t.AssemblyQualifiedName);
        }

        return string.Join(":", sorter.GetOrderedArray());
    }
}
