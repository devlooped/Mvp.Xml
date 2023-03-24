using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mvp.Xml.Serialization;

/// <summary>
/// Delegete type for events to raise from the
/// serializer cache.
/// </summary>
public delegate void SerializerCacheDelegate(Type type, XmlAttributeOverrides overrides, Type[] types, XmlRootAttribute root, string defaultNamespace);

/// <summary>
/// The XmlSerializerCache allows to work around the 
/// assembly leak problem in the <see cref="XmlSerializer"/> 
/// ( LINK )
/// The cache will inspect if it contains any previously cached 
/// instances that are compatible with the parameters passed to the
/// various overloads to the GetSerializer method before constructing 
/// a new XmlSerializer instance.
/// </summary>
/// <remarks>
/// In contrast to the <see cref="XmlSerializer"/>, the XmlSerializerCache requires
/// a permission set that allows reflecting over private members.
/// </remarks>
public class XmlSerializerCache : IDisposable
{
    /// <summary>
    /// The NewSerializer event fires when the XmlSerializerCache
    /// receives a request for an XmlSerializer instance
    /// that is not in the cache and it needs to create a
    /// new instance.
    /// </summary>
    public event SerializerCacheDelegate NewSerializer;

    /// <summary>
    /// The CacheHit even fires when the XmlSerializerCache
    /// receives a request for a previously cached instance
    /// of an XmlSerializer
    /// </summary>
    public event SerializerCacheDelegate CacheHit;

    /// <summary>
    /// The Dictionary to store cached serializer instances.
    /// </summary>
    readonly Dictionary<string, XmlSerializer> serializers = new();

    /// <summary>
    /// An object to synchonize access to the Dictionary instance.
    /// </summary>
    readonly object syncRoot = new();

    /// <summary>
    /// Get an XmlSerializer instance for the
    /// specified parameters. The method will check if
    /// any any previously cached instances are compatible
    /// with the parameters before constructing a new  
    /// XmlSerializer instance.
    /// </summary>
    public XmlSerializer GetSerializer(Type type, string defaultNamespace) => GetSerializer(type, null, new Type[0], null, defaultNamespace);

    /// <summary>
    /// Get an XmlSerializer instance for the
    /// specified parameters. The method will check if
    /// any any previously cached instances are compatible
    /// with the parameters before constructing a new  
    /// XmlSerializer instance.
    /// </summary>
    public XmlSerializer GetSerializer(Type type, XmlRootAttribute root) => GetSerializer(type, null, new Type[0], root, null);

    /// <summary>
    /// Get an XmlSerializer instance for the
    /// specified parameters. The method will check if
    /// any any previously cached instances are compatible
    /// with the parameters before constructing a new  
    /// XmlSerializer instance.
    /// </summary>
    public XmlSerializer GetSerializer(Type type, XmlAttributeOverrides overrides) => GetSerializer(type, overrides, new Type[0], null, null);

    /// <summary>
    /// Get an XmlSerializer instance for the
    /// specified parameters. The method will check if
    /// any any previously cached instances are compatible
    /// with the parameters before constructing a new  
    /// XmlSerializer instance.
    /// </summary>
    public XmlSerializer GetSerializer(Type type, Type[] types) => GetSerializer(type, null, types, null, null);

    /// <summary>
    /// Get an XmlSerializer instance for the
    /// specified parameters. The method will check if
    /// any any previously cached instances are compatible
    /// with the parameters before constructing a new  
    /// XmlSerializer instance.
    /// </summary>
    public XmlSerializer GetSerializer(Type type, XmlAttributeOverrides overrides, Type[] types, XmlRootAttribute root, string defaultNamespace)
    {
        var key = CacheKeyFactory.MakeKey(type, overrides, types, root, defaultNamespace);

        XmlSerializer serializer = null;
        if (!serializers.ContainsKey(key))
        {
            lock (syncRoot)
            {
                if (!serializers.ContainsKey(key))
                {
                    serializer = new XmlSerializer(type
                        , overrides
                        , types
                        , root
                        , defaultNamespace);
                    serializers.Add(key, serializer);

                    NewSerializer?.Invoke(type
                        , overrides
                        , types
                        , root
                        , defaultNamespace);
                } // if( null == Serializers[key] )
            } // lock
        } // if( null == Serializers[key] )
        else
        {
            serializer = serializers[key];
            // Tell the listeners that we already 
            // had a serializer that matched the attributes
            CacheHit?.Invoke(type, overrides, types, root, defaultNamespace);
        }

        System.Diagnostics.Debug.Assert(null != serializer);
        return serializer;
    }
    /// <summary>
    /// Implementation of IDisposable.Dispose. Call to 
    /// clean up resources held by the XmlSerializerCache.
    /// </summary>
    public void Dispose() { }
}
