using System;
using System.Xml.Serialization;
using Mvp.Xml.Common.Serialization;
using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class XmlSerializerCacheTests : IDisposable
{
    bool NewInstaceCreated;
    bool CacheHit;
    XmlSerializerCache cache;

    public XmlSerializerCacheTests()
    {
        cache = new XmlSerializerCache();
        ClearFlags();
        ConnectListeners();
    }

    public void Dispose()
    {
        DisonnectListeners();
        cache.Dispose();
        cache = null;
    }

    public void ClearFlags()
    {
        NewInstaceCreated = false;
        CacheHit = false;
    }

    void ConnectListeners()
    {
        cache.NewSerializer += new SerializerCacheDelegate(cache_NewSerializer);
        cache.CacheHit += new SerializerCacheDelegate(cache_CacheHit);
    }

    void DisonnectListeners()
    {
        cache.NewSerializer -= new SerializerCacheDelegate(cache_NewSerializer);
        cache.CacheHit -= new SerializerCacheDelegate(cache_CacheHit);
    }

    [Fact]
    public void AllParams()
    {
        var types1 = new Type[] { typeof(SerializeMe), typeof(SerializeMeToo) };
        var types2 = new Type[] { typeof(SerializeMe), typeof(SerializeMeToo) };
        var over1 = new XmlAttributeOverrides();
        var over2 = new XmlAttributeOverrides();
        var atts1 = new XmlAttributes();
        var atts2 = new XmlAttributes();

        atts1.XmlType = new XmlTypeAttribute("mytype");
        atts2.XmlType = new XmlTypeAttribute("mytype");

        over1.Add(typeof(SerializeMe), atts1);
        over2.Add(typeof(SerializeMe), atts2);

        var root1 = new XmlRootAttribute("someelement");
        var root2 = new XmlRootAttribute("someelement");

        var namespace1 = "mynamespace";
        var namespace2 = "mynamespace";

        var ser1 = cache.GetSerializer(typeof(SerializeMe)
            , over1
            , types1
            , root1
            , namespace1);


        Assert.False(CacheHit);
        Assert.True(NewInstaceCreated);
        ClearFlags();

        var ser2 = cache.GetSerializer(typeof(SerializeMe)
            , over2
            , types2
            , root2
            , namespace2);


        Assert.True(CacheHit);
        Assert.False(NewInstaceCreated);

        Assert.Same(ser1, ser2);
    }

    [Fact]
    public void TypesParam()
    {
        var types = new Type[] { typeof(SerializeMe), typeof(SerializeMeToo) };

        var ser1 = cache.GetSerializer(typeof(SerializeMe)
            , types);

        Assert.False(CacheHit);
        Assert.True(NewInstaceCreated);
        ClearFlags();

        var ser2 = cache.GetSerializer(typeof(SerializeMe)
            , types);

        Assert.True(CacheHit);
        Assert.False(NewInstaceCreated);

        Assert.Same(ser1, ser2);
    }

    [Fact]
    public void OverridesParam()
    {
        var over1 = new XmlAttributeOverrides();
        var over2 = new XmlAttributeOverrides();
        var atts1 = new XmlAttributes();
        var atts2 = new XmlAttributes();

        atts1.XmlType = new XmlTypeAttribute("mytype");
        atts2.XmlType = new XmlTypeAttribute("mytype");

        over1.Add(typeof(SerializeMe), atts1);
        over2.Add(typeof(SerializeMe), atts2);

        var ser1 = cache.GetSerializer(typeof(SerializeMe)
                , over1);
        Assert.False(CacheHit);
        Assert.True(NewInstaceCreated);
        ClearFlags();

        var ser2 = cache.GetSerializer(typeof(SerializeMe)
            , over2);
        Assert.True(CacheHit);
        Assert.False(NewInstaceCreated);

        Assert.Same(ser1, ser2);
    }

    [Fact]
    public void RootParam()
    {
        var root1 = new XmlRootAttribute("someelement");
        var root2 = new XmlRootAttribute("someelement");
        var ser1 = cache.GetSerializer(typeof(SerializeMe)
                , root1);
        Assert.False(CacheHit);
        Assert.True(NewInstaceCreated);
        ClearFlags();

        var ser2 = cache.GetSerializer(typeof(SerializeMe)
            , root2);
        Assert.True(CacheHit);
        Assert.False(NewInstaceCreated);

        Assert.Same(ser1, ser2);
    }

    [Fact]
    public void NamespaceParam()
    {
        var namespace1 = "mynamespace";
        var namespace2 = "mynamespace";

        System.Xml.Serialization.XmlSerializer ser1 = ser1 = cache.GetSerializer(typeof(SerializeMe)
                , namespace1);
        Assert.False(CacheHit);
        Assert.True(NewInstaceCreated);
        ClearFlags();

        var ser2 = cache.GetSerializer(typeof(SerializeMe)
            , namespace2);
        Assert.True(CacheHit);
        Assert.False(NewInstaceCreated);

        Assert.Same(ser1, ser2);
    }


    void cache_NewSerializer(Type type, XmlAttributeOverrides overrides, Type[] types, XmlRootAttribute root, String defaultNamespace)
    {
        NewInstaceCreated = true;
    }

    void cache_CacheHit(Type type, XmlAttributeOverrides overrides, Type[] types, XmlRootAttribute root, String defaultNamespace)
    {
        CacheHit = true;
    }
}
