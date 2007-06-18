using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Common.Xsl {
    
    /// <summary>
    /// Represents compiled collection of XSLT 2.0 Character map, see http://www.w3.org/TR/xslt20/#character-maps.
    /// </summary>
    public class CharacterMap {
        private Dictionary<string, Dictionary<char, string>> maps;

        /// <summary>
        /// Creates empty character map.
        /// </summary>
        public CharacterMap() {
            maps = new Dictionary<string, Dictionary<char, string>>();
        }

        /// <summary>
        /// Adds mapping for given character.
        /// </summary>        
        public void AddMapping(string mapName, char character, string replace)
        {
            if (!maps.ContainsKey(mapName))
            {
                maps.Add(mapName, new Dictionary<char,string>());
            }
            maps[mapName].Add(character, replace);
        }

        /// <summary>
        /// Gets mapping for given character.
        /// </summary>        
        public string GetMapping(string mapName, char character)
        {
            return maps[mapName][character];
        }

        /// <summary>
        /// Returns true if there is a mapping for given character and false otherwise.
        /// </summary>        
        public bool ContainsMapping(string mapName, char character)
        {
            return maps.ContainsKey(mapName) && maps[mapName].ContainsKey(character);
        }
    }
}
