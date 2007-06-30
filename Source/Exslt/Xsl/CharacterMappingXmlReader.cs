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
    /// must be in the "http://www.xmllab.net/nxslt" namespace:
    /// <pre>
    /// &lt;xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    ///     xmlns:nxslt="http://www.xmllab.net/nxslt">
    ///    &lt;nxslt:output use-character-maps="testmap"/>
    ///    &lt;nxslt:character-map name="testmap">
    ///         &lt;nxslt:output-character character="&#160;" string="&amp;nbsp;" />
    ///    &lt;/nxslt:character-map>
    /// </pre>
    /// When reading is done, resulting compiled character map can be compiled calling 
    /// <see cref="CharacterMappingXmlReader.CompileCharacterMapping()"/> method.
    /// </summary>
    /// <remarks>
    /// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>
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
        private string currMapName;
        private CharacterMap currMap;

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
                currMapName = base[nameTag];
                if (string.IsNullOrEmpty(currMapName))
                {
                    throw new System.Xml.Xsl.XsltCompileException("Required 'name' attribute of nxslt:character-map element is missing.");
                }
                currMap = new CharacterMap();
                string referencedMaps = base[useCharacterMapsTag];
                if (!string.IsNullOrEmpty(referencedMaps))
                {
                    currMap.ReferencedCharacterMaps = referencedMaps.Split(' ');
                }                                                                
            }
            else if (base.NodeType == XmlNodeType.EndElement && base.NamespaceURI == nxsltNamespace &&
                base.LocalName == characterMapTag)
            {
                if (this.mapping == null)
                {
                    this.mapping = new CharacterMapping();
                }
                this.mapping.AddCharacterMap(currMapName, currMap);
            }
            else if (base.NodeType == XmlNodeType.Element && base.NamespaceURI == nxsltNamespace
              && base.LocalName == outputCharacterTag)
            {
                //nxslt:output-character                        
                string character = base[characterTag];
                if (string.IsNullOrEmpty(character))
                {
                    throw new System.Xml.Xsl.XsltCompileException("Required 'character' attribute of nxslt:output-character element is missing.");
                }
                if (character.Length > 1)
                {
                    throw new System.Xml.Xsl.XsltCompileException("'character' attribute value of nxslt:output-character element is too long - must be a single character.");
                }
                string _string = base[stringTag];
                if (string.IsNullOrEmpty(character))
                {
                    throw new System.Xml.Xsl.XsltCompileException("Required 'string' attribute of nxslt:output-character element is missing.");
                }
                currMap.AddMapping(character[0], _string);
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
