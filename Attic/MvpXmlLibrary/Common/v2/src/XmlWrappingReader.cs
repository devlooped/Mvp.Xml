using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Common
{
	/// <summary>
	/// Base <see cref="XmlReader"/> that can be use to create new readers by 
	/// wrapping existing ones.
	/// </summary>
	/// <remarks>
	/// Supports <see cref="IXmlLineInfo"/> if the underlying writer supports it.
	/// <para>Author: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a>.</para>
	/// </remarks>
	public abstract class XmlWrappingReader : XmlReader, IXmlLineInfo
	{
		XmlReader innerReader;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlWrappingReader"/>.
		/// </summary>
		/// <param name="baseReader">The underlying reader this instance will wrap.</param>
		public XmlWrappingReader(XmlReader baseReader)
		{
			Guard.ArgumentNotNull(baseReader, "baseReader");

			innerReader = baseReader;
		}

		/// <summary>
		/// Gets or sets the underlying reader this instance is wrapping.
		/// </summary>
		protected XmlReader InnerReader
		{
			get { return innerReader; }
			set
			{
				Guard.ArgumentNotNull(value, "value");
				innerReader = value;
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.CanReadBinaryContent"/>.
		/// </summary>
		public override bool CanReadBinaryContent { get { return innerReader.CanReadBinaryContent; } }

		/// <summary>
		/// See <see cref="XmlReader.CanReadValueChunk"/>.
		/// </summary>
		public override bool CanReadValueChunk { get { return innerReader.CanReadValueChunk; } }

		/// <summary>
		/// See <see cref="XmlReader.CanResolveEntity"/>.
		/// </summary>
		public override bool CanResolveEntity { get { return innerReader.CanResolveEntity; } }

		/// <summary>
		/// See <see cref="XmlReader.Dispose"/>.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((IDisposable)innerReader).Dispose();
			}
		}

		/// <summary>
		/// See <see cref="XmlReader.Read"/>.
		/// </summary>
		public override bool Read() { return innerReader.Read(); }

		/// <summary>
		/// See <see cref="XmlReader.Close"/>.
		/// </summary>
		public override void Close() { innerReader.Close(); }

		/// <summary>
		/// See <see cref="XmlReader.GetAttribute(int)"/>.
		/// </summary>
		public override string GetAttribute(int i) { return innerReader.GetAttribute(i); }

		/// <summary>
		/// See <see cref="XmlReader.GetAttribute(string)"/>.
		/// </summary>
		public override string GetAttribute(string name) { return innerReader.GetAttribute(name); }

		/// <summary>
		/// See <see cref="XmlReader.GetAttribute(string, string)"/>.
		/// </summary>
		public override string GetAttribute(string localName, string namespaceURI) { { return innerReader.GetAttribute(localName, namespaceURI); } }

		/// <summary>
		/// See <see cref="XmlReader.LookupNamespace"/>.
		/// </summary>
		public override string LookupNamespace(string prefix) { return innerReader.LookupNamespace(prefix); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToAttribute(int)"/>.
		/// </summary>
		public override void MoveToAttribute(int i) { innerReader.MoveToAttribute(i); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToAttribute(string)"/>.
		/// </summary>
		public override bool MoveToAttribute(string name) { return innerReader.MoveToAttribute(name); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToAttribute(string, string)"/>.
		/// </summary>
		public override bool MoveToAttribute(string localName, string namespaceURI) { return innerReader.MoveToAttribute(localName, namespaceURI); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToElement"/>.
		/// </summary>
		public override bool MoveToElement() { return innerReader.MoveToElement(); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToFirstAttribute"/>.
		/// </summary>
		public override bool MoveToFirstAttribute() { return innerReader.MoveToFirstAttribute(); }

		/// <summary>
		/// See <see cref="XmlReader.MoveToNextAttribute"/>.
		/// </summary>
		public override bool MoveToNextAttribute() { return innerReader.MoveToNextAttribute(); }

		/// <summary>
		/// See <see cref="XmlReader.ReadAttributeValue"/>.
		/// </summary>
		public override bool ReadAttributeValue() { return innerReader.ReadAttributeValue(); }

		/// <summary>
		/// See <see cref="XmlReader.ResolveEntity"/>.
		/// </summary>
		public override void ResolveEntity() { innerReader.ResolveEntity(); }

		/// <summary>
		/// See <see cref="XmlReader.AttributeCount"/>.
		/// </summary>
		public override int AttributeCount { get { return innerReader.AttributeCount; } }

		/// <summary>
		/// See <see cref="XmlReader.BaseURI"/>.
		/// </summary>
		public override string BaseURI { get { return innerReader.BaseURI; } }

		/// <summary>
		/// See <see cref="XmlReader.Depth"/>.
		/// </summary>
		public override int Depth { get { return innerReader.Depth; } }

		/// <summary>
		/// See <see cref="XmlReader.EOF"/>.
		/// </summary>
		public override bool EOF { get { return innerReader.EOF; } }

		/// <summary>
		/// See <see cref="XmlReader.HasValue"/>.
		/// </summary>
		public override bool HasValue { get { return innerReader.HasValue; } }

		/// <summary>
		/// See <see cref="XmlReader.IsDefault"/>.
		/// </summary>
		public override bool IsDefault { get { return innerReader.IsDefault; } }

		/// <summary>
		/// See <see cref="XmlReader.IsEmptyElement"/>.
		/// </summary>
		public override bool IsEmptyElement { get { return innerReader.IsEmptyElement; } }

		/// <summary>
		/// See <see cref="XmlReader.this[int]"/>.
		/// </summary>
		public override string this[int i] { get { return innerReader[i]; } }

		/// <summary>
		/// See <see cref="XmlReader.this[string]"/>.
		/// </summary>
		public override string this[string name] { get { return innerReader[name]; } }

		/// <summary>
		/// See <see cref="XmlReader.this[string, string]"/>.
		/// </summary>
		public override string this[string name, string namespaceURI] { get { return innerReader[name, namespaceURI]; } }

		/// <summary>
		/// See <see cref="XmlReader.LocalName"/>.
		/// </summary>
		public override string LocalName { get { return innerReader.LocalName; } }

		/// <summary>
		/// See <see cref="XmlReader.Name"/>.
		/// </summary>
		public override string Name { get { return innerReader.Name; } }

		/// <summary>
		/// See <see cref="XmlReader.NamespaceURI"/>.
		/// </summary>
		public override string NamespaceURI { get { return innerReader.NamespaceURI; } }

		/// <summary>
		/// See <see cref="XmlReader.NameTable"/>.
		/// </summary>
		public override XmlNameTable NameTable { get { return innerReader.NameTable; } }

		/// <summary>
		/// See <see cref="XmlReader.NodeType"/>.
		/// </summary>
		public override XmlNodeType NodeType { get { return innerReader.NodeType; } }

		/// <summary>
		/// See <see cref="XmlReader.Prefix"/>.
		/// </summary>
		public override string Prefix { get { return innerReader.Prefix; } }

		/// <summary>
		/// See <see cref="XmlReader.QuoteChar"/>.
		/// </summary>
		public override char QuoteChar { get { return innerReader.QuoteChar; } }

		/// <summary>
		/// See <see cref="XmlReader.ReadState"/>.
		/// </summary>
		public override ReadState ReadState { get { return innerReader.ReadState; } }

		/// <summary>
		/// See <see cref="XmlReader.Value"/>.
		/// </summary>
		public override string Value { get { return innerReader.Value; } }

		/// <summary>
		/// See <see cref="XmlReader.XmlLang"/>.
		/// </summary>
		public override string XmlLang { get { return innerReader.XmlLang; } }

		/// <summary>
		/// See <see cref="XmlReader.XmlSpace"/>.
		/// </summary>
		public override XmlSpace XmlSpace { get { return innerReader.XmlSpace; } }

		/// <summary>
		/// See <see cref="XmlReader.ReadValueChunk"/>.
		/// </summary>
		public override int ReadValueChunk(char[] buffer, int index, int count) { return innerReader.ReadValueChunk(buffer, index, count); }

		#region IXmlLineInfo Members

		/// <summary>
		/// See <see cref="IXmlLineInfo.HasLineInfo"/>.
		/// </summary>
		public bool HasLineInfo()
		{
			IXmlLineInfo info = innerReader as IXmlLineInfo;
			if (info != null)
			{
				return info.HasLineInfo();
			}

			return false;
		}

		/// <summary>
		/// See <see cref="IXmlLineInfo.LineNumber"/>.
		/// </summary>
		public int LineNumber
		{
			get
			{
				IXmlLineInfo info = innerReader as IXmlLineInfo;
				if (info != null)
				{
					return info.LineNumber;
				}

				return 0;
			}
		}

		/// <summary>
		/// See <see cref="IXmlLineInfo.LinePosition"/>.
		/// </summary>
		public int LinePosition
		{
			get
			{
				IXmlLineInfo info = innerReader as IXmlLineInfo;
				if (info != null)
				{
					return info.LinePosition;
				}

				return 0;
			}
		}

		#endregion
	}
}
