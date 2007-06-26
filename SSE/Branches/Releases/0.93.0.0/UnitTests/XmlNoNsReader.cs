using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mvp.Xml.Synchronization.Tests
{
	internal class XmlNoNsReader : XmlWrappingReader
	{
		public XmlNoNsReader(XmlReader baseReader)
			: base(baseReader)
		{
		}

		public override string NamespaceURI
		{
			get
			{
				return String.Empty;
			}
		}
	}

	internal class XmlWrappingReader : XmlReader, IXmlLineInfo
	{
		// Fields
		protected XmlReader reader;
		protected IXmlLineInfo readerAsIXmlLineInfo;

		// Methods
		internal XmlWrappingReader(XmlReader baseReader)
		{
			this.Reader = baseReader;
		}

		public override void Close()
		{
			this.reader.Close();
		}

		protected override void Dispose(bool disposing)
		{
			((IDisposable)this.reader).Dispose();
		}

		public override string GetAttribute(int i)
		{
			return this.reader.GetAttribute(i);
		}

		public override string GetAttribute(string name)
		{
			return this.reader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			return this.reader.GetAttribute(name, namespaceURI);
		}

		public virtual bool HasLineInfo()
		{
			if (this.readerAsIXmlLineInfo != null)
			{
				return this.readerAsIXmlLineInfo.HasLineInfo();
			}
			return false;
		}

		public override string LookupNamespace(string prefix)
		{
			return this.reader.LookupNamespace(prefix);
		}

		public override void MoveToAttribute(int i)
		{
			this.reader.MoveToAttribute(i);
		}

		public override bool MoveToAttribute(string name)
		{
			return this.reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return this.reader.MoveToAttribute(name, ns);
		}

		public override bool MoveToElement()
		{
			return this.reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return this.reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return this.reader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return this.reader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return this.reader.ReadAttributeValue();
		}

		public override void ResolveEntity()
		{
			this.reader.ResolveEntity();
		}

		public override void Skip()
		{
			this.reader.Skip();
		}

		// Properties
		public override int AttributeCount
		{
			get
			{
				return this.reader.AttributeCount;
			}
		}

		public override string BaseURI
		{
			get
			{
				return this.reader.BaseURI;
			}
		}

		public override bool CanResolveEntity
		{
			get
			{
				return this.reader.CanResolveEntity;
			}
		}

		public override int Depth
		{
			get
			{
				return this.reader.Depth;
			}
		}

		public override bool EOF
		{
			get
			{
				return this.reader.EOF;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return this.reader.HasAttributes;
			}
		}

		public override bool HasValue
		{
			get
			{
				return this.reader.HasValue;
			}
		}

		public override bool IsDefault
		{
			get
			{
				return this.reader.IsDefault;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				return this.reader.IsEmptyElement;
			}
		}

		public virtual int LineNumber
		{
			get
			{
				if (this.readerAsIXmlLineInfo != null)
				{
					return this.readerAsIXmlLineInfo.LineNumber;
				}
				return 0;
			}
		}

		public virtual int LinePosition
		{
			get
			{
				if (this.readerAsIXmlLineInfo != null)
				{
					return this.readerAsIXmlLineInfo.LinePosition;
				}
				return 0;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.reader.LocalName;
			}
		}

		public override string Name
		{
			get
			{
				return this.reader.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				return this.reader.NamespaceURI;
			}
		}

		public override XmlNameTable NameTable
		{
			get
			{
				return this.reader.NameTable;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return this.reader.NodeType;
			}
		}

		public override string Prefix
		{
			get
			{
				return this.reader.Prefix;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return this.reader.QuoteChar;
			}
		}

		protected XmlReader Reader
		{
			get
			{
				return this.reader;
			}
			set
			{
				this.reader = value;
				this.readerAsIXmlLineInfo = value as IXmlLineInfo;
			}
		}

		public override ReadState ReadState
		{
			get
			{
				return this.reader.ReadState;
			}
		}

		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.reader.SchemaInfo;
			}
		}

		public override XmlReaderSettings Settings
		{
			get
			{
				return this.reader.Settings;
			}
		}

		public override string Value
		{
			get
			{
				return this.reader.Value;
			}
		}

		public override Type ValueType
		{
			get
			{
				return this.reader.ValueType;
			}
		}

		public override string XmlLang
		{
			get
			{
				return this.reader.XmlLang;
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				return this.reader.XmlSpace;
			}
		}
	}
}
