using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Common.Xsl
{
    /// <summary>
    /// XmlReader implementation able to read and filter out character map declarations
    /// from XSLT stylesheets.
    /// </summary>
    public class CharacterMappingXmlReader : XmlWrappingReader 
    {
        private CharacterMap map;
        private string nxsltNamespace;
        private string characterMapTag;
        private string nameTag;
        private string outputCharacterTag;
        private string characterTag;
        private string stringTag;

        /// <summary>
        /// Creates new instance of the <see cref="CharacterMappingXmlReader"/> with given
        /// base <see cref="XmlReader"/>.
        /// </summary>        
        public CharacterMappingXmlReader(XmlReader baseReader)
            : this(baseReader, new CharacterMap())
        {            
        }

        /// <summary>
        /// Compiled character map.
        /// </summary>
        public CharacterMap CharacterMap
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
            }
        }

        /// <summary>
        /// Creates new instance of the <see cref="CharacterMappingXmlReader"/> with given
        /// base <see cref="XmlReader"/> and <see cref="CharacterMap"/>
        /// </summary>        
        public CharacterMappingXmlReader(XmlReader baseReader, CharacterMap map)
            : base(baseReader)
        {
            this.map = map;
            this.nxsltNamespace = base.NameTable.Add("http://www.xmllab.net/nxslt");
            this.characterMapTag = base.NameTable.Add("character-map");
            this.nameTag = base.NameTable.Add("name");
            this.outputCharacterTag = base.NameTable.Add("output-character");
            this.characterTag = base.NameTable.Add("character");
            this.stringTag = base.NameTable.Add("string");

        }

        /// <summary>
        /// See <see cref="XmlReader.Read"/>.
        /// </summary>        
        public override bool Read()
        {
            bool baseRead = base.Read();
            if (base.NodeType == XmlNodeType.Element && base.NamespaceURI == nxsltNamespace &&
                base.LocalName == characterMapTag)
            {
                //nxslt:character-map
                string mapName = base[nameTag];
                if (string.IsNullOrEmpty(mapName))
                {
                    throw new System.Xml.Xsl.XsltCompileException("Required 'name' attribute of nxslt:character-map element is missing.");
                }
                XmlReader subr = base.ReadSubtree();
                while (subr.Read())
                {
                    if (subr.NodeType == XmlNodeType.Element && subr.NamespaceURI == nxsltNamespace
                        && subr.LocalName == outputCharacterTag)
                    {
                        //nxslt:output-character                        
                        string character = subr[characterTag];
                        if (string.IsNullOrEmpty(character)) 
                        {
                            throw new System.Xml.Xsl.XsltCompileException("Required 'character' attribute of nxslt:output-character element is missing.");
                        }
                        if (character.Length > 1)
                        {
                            throw new System.Xml.Xsl.XsltCompileException("'character' attribute value of nxslt:output-character element is too long - must be a single character.");
                        }
                        string _string = subr[stringTag];
                        if (string.IsNullOrEmpty(character))
                        {
                            throw new System.Xml.Xsl.XsltCompileException("Required 'string' attribute of nxslt:output-character element is missing.");
                        }
                        this.map.AddMapping(mapName, character[0], _string);
                    }
                }
            }
            return baseRead;
        }
    }
}
