using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Common.Xsl
{
    /// <summary>
    /// <see cref="XmlReader"/> implementation able to read and filter out XSLT 2.0-like character map declarations
    /// from XSLT stylesheets.
    /// For character mapping semantics see http://www.w3.org/TR/xslt20/#character-maps.
    /// The only deviation from XSLT 2.0 is that "output", "character-map" and "output-character" elements
    /// must be in the "http://www.xmllab.net/nxslt" namespace.
    /// </summary>
    public class CharacterMappingXmlReader : XmlWrappingReader 
    {
        private CharacterMapping mapping;
        private string nxsltNamespace;
        private string characterMapTag;
        private string nameTag;
        private string outputCharacterTag;
        private string characterTag;
        private string stringTag;
        private string outputTag;
        private string useCharacterMapsTag;
        private List<string> useCharacterMaps;

        /// <summary>
        /// Creates new instance of the <see cref="CharacterMappingXmlReader"/> with given
        /// base <see cref="XmlReader"/>.
        /// </summary>        
        public CharacterMappingXmlReader(XmlReader baseReader)
            : base(baseReader)
        {
            this.nxsltNamespace = base.NameTable.Add("http://www.xmllab.net/nxslt");
            this.characterMapTag = base.NameTable.Add("character-map");
            this.nameTag = base.NameTable.Add("name");
            this.outputCharacterTag = base.NameTable.Add("output-character");
            this.characterTag = base.NameTable.Add("character");
            this.stringTag = base.NameTable.Add("string");
            this.outputTag = base.NameTable.Add("output");
            this.useCharacterMapsTag = base.NameTable.Add("use-character-maps");
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
                CharacterMap map = new CharacterMap();
                string referencedMaps = base[useCharacterMapsTag];
                if (!string.IsNullOrEmpty(referencedMaps))
                {
                    map.ReferencedCharacterMaps = referencedMaps.Split(' ');
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
                        map.AddMapping(character[0], _string);
                    }
                }
                if (this.mapping == null)
                {
                    this.mapping = new CharacterMapping();
                }
                this.mapping.AddCharacterMap(mapName, map);
            }
            else if (base.NodeType == XmlNodeType.Element && base.NamespaceURI == nxsltNamespace &&
               base.LocalName == outputTag)
            {
                //nxslt:output
                string useMaps = base[useCharacterMapsTag];
                if (!string.IsNullOrEmpty(useMaps))
                {
                    if (this.useCharacterMaps == null)
                    {
                        this.useCharacterMaps = new List<string>();
                    }
                    this.useCharacterMaps.AddRange(useMaps.Split(' '));
                }
                XmlReader subr = base.ReadSubtree();
                while (subr.Read());                
            }
            return baseRead;
        }

        /// <summary>
        /// Compiles character map.
        /// </summary>        
        public Dictionary<char, string> CompileCharacterMapping()
        {
            if (this.mapping == null) {
                return new Dictionary<char,string>();
            }
            return this.mapping.Compile(this.useCharacterMaps);
        }
    }
}
