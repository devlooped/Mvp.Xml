using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Common.Xsl
{
    /// <summary>
    /// <see cref="XmlWriter"/> implementation able to substitute characters appearing in text or attribute nodes.
    /// For character mapping semantics see http://www.w3.org/TR/xslt20/#character-maps.
    /// </summary>
    public class CharacterMappingXmlWriter : XmlWrappingWriter
    {
        private Dictionary<char, string> mapping;

        /// <summary>
        /// Creates new instance of the <see cref="CharacterMappingXmlWriter"/>
        /// with given base <see cref="XmlReader"/> and charcter mapping.
        /// </summary>        
        public CharacterMappingXmlWriter(XmlWriter baseWriter, Dictionary<char, string> mapping)
            : base(baseWriter)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// See <see cref="XmlWriter.WriteString"/>.
        /// </summary>        
        public override void WriteString(string text)
        {
            if (mapping != null && mapping.Count > 0) {
                StringBuilder buf = new StringBuilder();
                foreach (char c in text) {
                    if (mapping.ContainsKey(c)) {
                        FlushBuffer(buf);
                        base.WriteRaw(mapping[c]);
                    } else {
                        buf.Append(c);
                    }
                }
                FlushBuffer(buf);
            } else {
                base.WriteString(text);
            }            
        }

        private void FlushBuffer(StringBuilder buf) {
            if (buf.Length > 0) {
                base.WriteString(buf.ToString());
                buf.Length = 0;
            }
        }        
    }
}
