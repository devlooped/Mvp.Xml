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
        Dictionary<char, string> mapping;

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
            if (mapping != null && mapping.Count > 0)
            {
                StringBuilder buf = new StringBuilder();
                foreach (char c in text)
                {
                    if (mapping.ContainsKey(c))
                    {
                        buf.Append(mapping[c]);
                    }
                    else
                    {
                        buf.Append(c);
                    }
                }
                base.WriteString(buf.ToString());
            }
            else
            {
                base.WriteString(text);
            }
        }
    }
}
