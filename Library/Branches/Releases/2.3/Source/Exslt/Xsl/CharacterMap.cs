using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Common.Xsl {
    
    /// <summary>
    /// Represents XSLT 2.0 Character map, see http://www.w3.org/TR/xslt20/#character-maps.
    /// </summary>
    /// <remarks>
    /// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
    /// </remarks>
    internal class CharacterMap {        
        private Dictionary<char, string> map;
        private string[] usedCharMaps;

        /// <summary>
        /// Creates empty character map.
        /// </summary>
        public CharacterMap() {                        
            this.map = new Dictionary<char, string>();
        }

        /// <summary>
        /// Adds mapping for given character.
        /// </summary>        
        public void AddMapping(char character, string replace)
        {
            if (map.ContainsKey(character))
            {
                map[character] = replace;
            }
            else
            {
                map.Add(character, replace);
            }
        }                

        /// <summary>
        /// Gets mapping collection.
        /// </summary>
        public Dictionary<char, string> Map
        {
            get
            {
                return map;
            }
        }

        /// <summary>
        /// Referenced character maps.
        /// </summary>
        public string[] ReferencedCharacterMaps
        {
            get
            {
                return usedCharMaps;
            }
            set
            {
            	usedCharMaps = value;
            }
        }        
    }
}
