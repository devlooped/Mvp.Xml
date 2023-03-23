using System;
using System.Collections.Generic;
using System.Xml.Xsl;

// ReSharper disable once CheckNamespace
namespace Mvp.Xml.Common.Xsl;

/// <summary>
/// Represents compiled collection of XSLT 2.0 Character map, see http://www.w3.org/TR/xslt20/#character-maps.
/// </summary>
/// <remarks>
/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
/// </remarks>
class CharacterMapping
{
    readonly Dictionary<string, CharacterMap> maps = new();

    /// <summary>
    /// Adds mapping for given character.
    /// </summary>        
    public void AddCharacterMap(string name, CharacterMap map)
    {
        if (maps.ContainsKey(name))
            throw new XsltCompileException("Duplicate character map '" + name + "'.");

        maps.Add(name, map);
    }

    /// <summary>
    /// Recursive expansion of character maps, removing unused. 
    /// </summary>        
    public Dictionary<char, string> Compile(List<string> charMapsToBeUsed)
    {
        var usedmaps = new List<string>(charMapsToBeUsed);
        var compiledMap = new Dictionary<char, string>();
        var mapStack = new Stack<string>();
        foreach (var mapName in usedmaps)
        {
            CompileMap(mapName, compiledMap, mapStack);
        }
        return compiledMap;
    }

    void CompileMap(string mapName, Dictionary<char, string> compiledMap, Stack<string> mapStack)
    {
        if (!maps.ContainsKey(mapName))
            throw new XsltCompileException("Unknown character map '" + mapName + "'");

        if (mapStack.Contains(mapName))
            throw new XsltCompileException("Character map " + mapName + " references itself, directly or indirectly.");

        var map = maps[mapName];
        foreach (var c in map.Map.Keys)
        {
            if (compiledMap.ContainsKey(c))
            {
                Console.WriteLine("Replacing " + (int)c + ": " + compiledMap[c] + " with " + map.Map[c]);
                compiledMap[c] = map.Map[c];
            }
            else
            {
                compiledMap.Add(c, map.Map[c]);
            }
        }
        if (map.ReferencedCharacterMaps != null)
        {
            mapStack.Push(mapName);
            foreach (var name in map.ReferencedCharacterMaps)
            {
                CompileMap(name, compiledMap, mapStack);
            }
            mapStack.Pop();
        }
    }
}
