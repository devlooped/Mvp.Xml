using System.Collections.Generic;

namespace Mvp.Xml.XPointer;

/// <summary>
/// XPointer scheme.
/// </summary>
class XPointerSchema
{
    public enum SchemaType
    {
        Element,
        Xmlns,
        XPath1,
        XPointer,
        Unknown
    }

    public static IDictionary<string, SchemaType> Schemas { get; } = CreateSchemasTable();

    static IDictionary<string, SchemaType> CreateSchemasTable() => new Dictionary<string, SchemaType>(4)
    {
        //<namespace uri>:<ncname>
        { ":element", SchemaType.Element },
        { ":xmlns", SchemaType.Xmlns },
        { ":xpath1", SchemaType.XPath1 },
        { ":xpointer", SchemaType.XPointer }
    };
}
