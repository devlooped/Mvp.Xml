namespace Mvp.Xml.Exslt;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///   This class implements additional functions in the http://gotdotnet.com/exslt/strings namespace.
/// </summary>		
public class GdnStrings
{
    /// <summary>
    /// Implements the following function 
    ///		string uppercase(string)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <remarks>THIS FUNCTION IS NOT IN EXSLT!!!</remarks>
    public string Uppercase(string str) => str.ToUpper();

    public string uppercase(string str) => Uppercase(str);

    /// <summary>
    /// Implements the following function 
    ///		string lowercase(string)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <remarks>THIS FUNCTION IS NOT IN EXSLT!!!</remarks>
    public string Lowercase(string str) => str.ToLower();

    public string lowercase(string str) => Lowercase(str);
}
