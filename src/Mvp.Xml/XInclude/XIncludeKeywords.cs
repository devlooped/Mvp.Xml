using System.Xml;

namespace Mvp.Xml.XInclude;

/// <summary>
/// XInclude syntax keyword collection.	
/// </summary>
/// <author>Oleg Tkachenko, http://www.xmllab.net</author>
class XIncludeKeywords
{
    public XIncludeKeywords(XmlNameTable nt)
    {
        nameTable = nt;
        //Preload some keywords
        XIncludeNamespace = nameTable.Add(sXIncludeNamespace);
        OldXIncludeNamespace = nameTable.Add(sOldXIncludeNamespace);
        Include = nameTable.Add(sInclude);
        Href = nameTable.Add(sHref);
        Parse = nameTable.Add(sParse);
    }

    //
    // Keyword strings
    const string sXIncludeNamespace = "http://www.w3.org/2001/XInclude";
    const string sOldXIncludeNamespace = "http://www.w3.org/2003/XInclude";
    const string sInclude = "include";
    const string sHref = "href";
    const string sParse = "parse";
    const string sXml = "xml";
    const string sText = "text";
    const string sXpointer = "xpointer";
    const string sAccept = "accept";
    const string sAcceptLanguage = "accept-language";
    const string sEncoding = "encoding";
    const string sFallback = "fallback";
    const string sXmlNamespace = "http://www.w3.org/XML/1998/namespace";
    const string sBase = "base";
    const string sXmlBase = "xml:base";
    const string sLang = "lang";
    const string sXmlLang = "xml:lang";
    readonly XmlNameTable nameTable;

    //
    // Properties
    string xml;
    string text;
    string xpointer;
    string accept;
    string acceptLanguage;
    string encoding;
    string fallback;
    string xmlNamespace;
    string _Base;
    string xmlBase;
    string lang;
    string xmlLang;

    // http://www.w3.org/2003/XInclude
    public string XIncludeNamespace { get; }

    // http://www.w3.org/2001/XInclude
    public string OldXIncludeNamespace { get; }

    // include
    public string Include { get; }

    // href
    public string Href { get; }

    // parse
    public string Parse { get; }

    // xml
    public string Xml => xml ??= nameTable.Add(sXml);

    // text
    public string Text => text ??= nameTable.Add(sText);

    // xpointer
    public string Xpointer => xpointer ??= nameTable.Add(sXpointer);

    // accept
    public string Accept => accept ??= nameTable.Add(sAccept);

    // accept-language
    public string AcceptLanguage => acceptLanguage ??= nameTable.Add(sAcceptLanguage);

    // encoding
    public string Encoding => encoding ??= nameTable.Add(sEncoding);

    // fallback
    public string Fallback => fallback ??= nameTable.Add(sFallback);

    // Xml namespace
    public string XmlNamespace => xmlNamespace ??= nameTable.Add(sXmlNamespace);

    // Base
    public string Base => _Base ??= nameTable.Add(sBase);

    // xml:base
    public string XmlBase => xmlBase ??= nameTable.Add(sXmlBase);

    // Lang
    public string Lang => lang ??= nameTable.Add(sLang);

    // xml:lang
    public string XmlLang => xmlLang ??= nameTable.Add(sXmlLang);

    // Comparison
    public static bool Equals(string keyword1, string keyword2) => ReferenceEquals(keyword1, keyword2);//return (object)keyword1 == (object)keyword2;
}
