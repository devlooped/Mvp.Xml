using System;
using System.Xml;

namespace Mvp.Xml;

/// <summary>
/// Base <see cref="XmlWriter"/> that can be use to create new writers
/// by wrapping existing ones.
/// </summary>
/// <remarks>
/// <para>Author: Daniel Cazzulino, <a href="https://cazzulino.com">blog</a>.</para>
/// </remarks>
public abstract class XmlWrappingWriter : XmlWriter
{
    XmlWriter baseWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlWrappingWriter"/>.
    /// </summary>
    /// <param name="baseWriter">The underlying writer this instance will wrap.</param>
    protected XmlWrappingWriter(XmlWriter baseWriter)
    {
        Guard.ArgumentNotNull(baseWriter, "baseWriter");

        this.baseWriter = baseWriter;
    }

    /// <summary>
    /// Gets or sets the underlying writer this instance is wrapping.
    /// </summary>
    protected XmlWriter BaseWriter
    {
        get => baseWriter;
        set
        {
            Guard.ArgumentNotNull(value, "value");
            baseWriter = value;
        }
    }

    /// <summary>
    /// See <see cref="XmlWriter.Close"/>.
    /// </summary>
    public override void Close() => baseWriter.Close();

    /// <summary>
    /// See <see cref="XmlWriter.Dispose(bool)"/>.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (WriteState != WriteState.Closed)
            Close();

        ((IDisposable)baseWriter).Dispose();
    }

    /// <summary>
    /// See <see cref="XmlWriter.Flush"/>.
    /// </summary>
    public override void Flush() => baseWriter.Flush();

    /// <summary>
    /// See <see cref="XmlWriter.LookupPrefix"/>.
    /// </summary>
    public override string LookupPrefix(string ns) => baseWriter.LookupPrefix(ns);

    /// <summary>
    /// See <see cref="XmlWriter.WriteBase64"/>.
    /// </summary>
    public override void WriteBase64(byte[] buffer, int index, int count) => baseWriter.WriteBase64(buffer, index, count);

    /// <summary>
    /// See <see cref="XmlWriter.WriteCData"/>.
    /// </summary>
    public override void WriteCData(string text) => baseWriter.WriteCData(text);

    /// <summary>
    /// See <see cref="XmlWriter.WriteCharEntity"/>.
    /// </summary>
    public override void WriteCharEntity(char ch) => baseWriter.WriteCharEntity(ch);

    /// <summary>
    /// See <see cref="XmlWriter.WriteChars"/>.
    /// </summary>
    public override void WriteChars(char[] buffer, int index, int count) => baseWriter.WriteChars(buffer, index, count);

    /// <summary>
    /// See <see cref="XmlWriter.WriteComment"/>.
    /// </summary>
    public override void WriteComment(string text) => baseWriter.WriteComment(text);

    /// <summary>
    /// See <see cref="XmlWriter.WriteDocType"/>.
    /// </summary>
    public override void WriteDocType(string name, string pubid, string sysid, string subset) => baseWriter.WriteDocType(name, pubid, sysid, subset);

    /// <summary>
    /// See <see cref="XmlWriter.WriteEndAttribute"/>.
    /// </summary>
    public override void WriteEndAttribute() => baseWriter.WriteEndAttribute();

    /// <summary>
    /// See <see cref="XmlWriter.WriteEndDocument"/>.
    /// </summary>
    public override void WriteEndDocument() => baseWriter.WriteEndDocument();

    /// <summary>
    /// See <see cref="XmlWriter.WriteEndElement"/>.
    /// </summary>
    public override void WriteEndElement() => baseWriter.WriteEndElement();

    /// <summary>
    /// See <see cref="XmlWriter.WriteEntityRef"/>.
    /// </summary>
    public override void WriteEntityRef(string name) => baseWriter.WriteEntityRef(name);

    /// <summary>
    /// See <see cref="XmlWriter.WriteFullEndElement"/>.
    /// </summary>
    public override void WriteFullEndElement() => baseWriter.WriteFullEndElement();

    /// <summary>
    /// See <see cref="XmlWriter.WriteProcessingInstruction"/>.
    /// </summary>
    public override void WriteProcessingInstruction(string name, string text) => baseWriter.WriteProcessingInstruction(name, text);

    /// <summary>
    /// See <see cref="XmlWriter.WriteRaw(string)"/>.
    /// </summary>
    public override void WriteRaw(string data) => baseWriter.WriteRaw(data);

    /// <summary>
    /// See <see cref="XmlWriter.WriteRaw(char[], int, int)"/>.
    /// </summary>
    public override void WriteRaw(char[] buffer, int index, int count) => baseWriter.WriteRaw(buffer, index, count);

    /// <summary>
    /// See <see cref="XmlWriter.WriteStartAttribute(string, string, string)"/>.
    /// </summary>
    public override void WriteStartAttribute(string prefix, string localName, string ns) => baseWriter.WriteStartAttribute(prefix, localName, ns);

    /// <summary>
    /// See <see cref="XmlWriter.WriteStartDocument()"/>.
    /// </summary>
    public override void WriteStartDocument() => baseWriter.WriteStartDocument();

    /// <summary>
    /// See <see cref="XmlWriter.WriteStartDocument(bool)"/>.
    /// </summary>
    public override void WriteStartDocument(bool standalone) => baseWriter.WriteStartDocument(standalone);

    /// <summary>
    /// See <see cref="XmlWriter.WriteStartElement(string, string, string)"/>.
    /// </summary>
    public override void WriteStartElement(string prefix, string localName, string ns) => baseWriter.WriteStartElement(prefix, localName, ns);

    /// <summary>
    /// See <see cref="XmlWriter.WriteString"/>.
    /// </summary>
    public override void WriteString(string text) => baseWriter.WriteString(text);

    /// <summary>
    /// See <see cref="XmlWriter.WriteSurrogateCharEntity"/>.
    /// </summary>
    public override void WriteSurrogateCharEntity(char lowChar, char highChar) => baseWriter.WriteSurrogateCharEntity(lowChar, highChar);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(bool)"/>.
    /// </summary>
    public override void WriteValue(bool value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(DateTime)"/>.
    /// </summary>
    public override void WriteValue(DateTime value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(decimal)"/>.
    /// </summary>
    public override void WriteValue(decimal value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(double)"/>.
    /// </summary>
    public override void WriteValue(double value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(int)"/>.
    /// </summary>
    public override void WriteValue(int value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(long)"/>.
    /// </summary>
    public override void WriteValue(long value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(object)"/>.
    /// </summary>
    public override void WriteValue(object value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(float)"/>.
    /// </summary>
    public override void WriteValue(float value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteValue(string)"/>.
    /// </summary>
    public override void WriteValue(string value) => baseWriter.WriteValue(value);

    /// <summary>
    /// See <see cref="XmlWriter.WriteWhitespace"/>.
    /// </summary>
    public override void WriteWhitespace(string ws) => baseWriter.WriteWhitespace(ws);

    /// <summary>
    /// See <see cref="XmlWriter.Settings"/>.
    /// </summary>
    public override XmlWriterSettings Settings => baseWriter.Settings;

    /// <summary>
    /// See <see cref="XmlWriter.WriteState"/>.
    /// </summary>
    public override WriteState WriteState => baseWriter.WriteState;

    /// <summary>
    /// See <see cref="XmlWriter.XmlLang"/>.
    /// </summary>
    public override string XmlLang => baseWriter.XmlLang;

    /// <summary>
    /// See <see cref="XmlWriter.XmlSpace"/>.
    /// </summary>
    public override XmlSpace XmlSpace => baseWriter.XmlSpace;
}
