using System.IO;
using Xunit;

namespace Mvp.Xml.XInclude.Test;

/// <summary>
/// Edinburgh University test cases from the XInclude Test suite.
/// </summary>

public class Elliotte_Rusty_HaroldTests
{
    public Elliotte_Rusty_HaroldTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result)
    {
        XIncludeReaderTests.RunAndCompare(
            "../../XInclude/XInclude-Test-Suite/Harold/test/" + source,
                "../../XInclude/XInclude-Test-Suite/Harold/test/" + result);
    }

    /// <summary>
    /// xml:base attribute is used to resolve relative URLs in href attributes        
    /// </summary>
    [Fact]
    public void harold_01()
    {
        RunAndCompare("xmlbasetest.xml", "../result/xmlbasetest.xml");
    }

    /// <summary>
    /// Use XPointer to include an include element in another document,
    /// and make sure that's fully resolved too        
    /// </summary>
    [Fact]
    public void harold_02()
    {
        RunAndCompare("resolvethruxpointer.xml", "../result/resolvethruxpointer.xml");
    }

    /// <summary>
    /// xml:base attribute on the xi:include element is used to resolve relative URL in href        
    /// </summary>
    [Fact]
    public void harold_03()
    {
        RunAndCompare("xmlbasetest2.xml", "../result/xmlbasetest2.xml");
    }

    /// <summary>
    /// xml:base attribute from an unincluded element
    /// still applies to its included descendants        
    /// </summary>
    [Fact]
    public void harold_04()
    {
        RunAndCompare("xmlbasetest3.xml", "../result/xmlbasetest3.xml");
    }



    /// <summary>
    /// An include element includes its following sibling element, which has a child
    /// include element including the sibling element after that one.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_05()
    {
        Assert.Throws<FatalResourceException>(() => RunAndCompare("marshtest.xml", "../result/marshtest.xml"));
    }

    /// <summary>
    /// Include a document that uses XPointers to reference various parts of itself        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_06()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("includedocumentwithintradocumentreferences.xml", "../result/includedocumentwithintradocumentreferences.xml"));
    }

    /// <summary>
    /// xml:lang attribute from including document does not override xml:lang attribute in included document        
    /// </summary>
    [Fact]
    public void harold_07()
    {
        RunAndCompare("langtest1.xml", "../result/langtest1.xml");
    }

    /// <summary>
    /// xml:lang attribute is added to retain the included element's language, even
    /// though the language was originaly declared on an unincluded element        
    /// </summary>
    [Fact]
    public void harold_08()
    {
        RunAndCompare("langtest2.xml", "../result/langtest2.xml");
    }

    /// <summary>
    /// xml:lang='' is added when the included document does not declare a language
    /// and the including element does        
    /// </summary>
    [Fact]
    public void harold_09()
    {
        RunAndCompare("langtest3.xml", "../result/langtest3.xml");
    }


    /// <summary>
    /// Test that the
    /// xml:base attribute is not used to resolve a missing href.
    /// According to RFC 2396 empty string URI always refers to the current document irrespective of base URI.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_10()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("marshtestwithxmlbase.xml", "../result/marshtestwithxmlbase.xml"));
    }

    /// <summary>
    /// There's no difference between href="" and no href attribute.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_11()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("marshtestwithxmlbaseandemptyhref.xml", "../result/marshtestwithxmlbase.xml"));
    }

    /// <summary>
    /// Make sure base URIs are preserved when including from the same document.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_12()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("includefromsamedocumentwithbase.xml", "../result/includefromsamedocumentwithbase.xml"));
    }

    /// <summary>
    /// Syntactically incorrect IRI is a fatal error (Eitehr I'm missing something or the spec needs to state this prinicple more clearly.)        
    /// </summary>
    /// <remarks>WE TREAT IT AS RESOURCE ERROR</remarks> 
    [Fact]
    public void harold_13()
    {
        Assert.Throws<DirectoryNotFoundException>(() => RunAndCompare("badiri.xml", ""));
    }

    /// <summary>
    /// Syntactically incorrect IRI with an unrecognized scheme is a fatal error        
    /// </summary>
    /// <remarks>WE TREAT IT AS RESOURCE ERROR</remarks> 
    [Fact]
    public void harold_14()
    {
        Assert.Throws<DirectoryNotFoundException>(() => RunAndCompare("badiri2.xml", ""));
    }

    /// <summary>
    /// Syntactically correct IRI with an unrecognized scheme is a resource error        
    /// </summary>
    [Fact]
    public void harold_15()
    {
        RunAndCompare("goodiri.xml", "../result/goodiri.xml");
    }

    /// <summary>
    /// accept attribute contains carriage-return/linefeed pair        
    /// </summary>
    [Fact]
    public void harold_16()
    {
        Assert.Throws<InvalidAcceptHTTPHeaderValueError>(() => RunAndCompare("badaccept1.xml", ""));
    }

    /// <summary>
    /// accept attribute contains Latin-1 character (non-breaking space)        
    /// </summary>
    [Fact]
    public void harold_17()
    {
        Assert.Throws<InvalidAcceptHTTPHeaderValueError>(() => RunAndCompare("badaccept2.xml", ""));
    }

    /// <summary>
    /// Unprefixed, unrecognized attributes on an include element are ignored        
    /// </summary>
    [Fact]
    public void harold_18()
    {
        RunAndCompare("extraattributes.xml", "../result/c1.xml");
    }

    /// <summary>
    /// Fallback elements can be empty        
    /// </summary>
    [Fact]
    public void harold_19()
    {
        RunAndCompare("emptyfallback.xml", "../result/emptyfallback.xml");
    }

    /// <summary>
    /// Included documents can themselves use fallbacks        
    /// </summary>
    [Fact]
    public void harold_20()
    {
        RunAndCompare("metafallbacktest.xml", "../result/metafallbacktest.xml");
    }

    /// <summary>
    /// An included document can use a fallback that points into the included document        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_21()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("metafallbacktest6.xml", "../result/metafallbacktest6.xml"));
    }

    /// <summary>
    /// An included document can use a fallback that includes another document as text        
    /// </summary>
    [Fact]
    public void harold_22()
    {
        RunAndCompare("metafallbacktest2.xml", "../result/metafallbacktest2.xml");
    }

    /// <summary>
    /// A fallback element in an included document contains an include element with a parse attribute with an illegal value.        
    /// </summary>
    [Fact]
    public void harold_23()
    {
        Assert.Throws<XIncludeSyntaxError>(() => RunAndCompare("metafallbacktest3.xml", ""));
    }

    /// <summary>
    /// A fallback element in an included document contains an include element with neither an xpointer nor an href attribute.        
    /// </summary>
    [Fact]
    public void harold_24()
    {
        Assert.Throws<XIncludeSyntaxError>(() => RunAndCompare("metafallbacktest4.xml", ""));
    }

    /// <summary>
    /// A fallback element in an included document contains an include element whose href attribute has a fragment ID.        
    /// </summary>
    [Fact]
    public void harold_25()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("metafallbacktestwithfragmentid.xml", ""));
    }

    /// <summary>
    /// The XPointer does not select anything in the acquired infoset, but does select something in the source infoset.        
    /// </summary>
    [Fact]
    public void harold_26()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("metafallbacktest5.xml", ""));
    }

    /// <summary>
    /// A fallback in an included document contains some text and a comment, but no elements.        
    /// </summary>
    [Fact]
    public void harold_27()
    {
        RunAndCompare("metafallbacktotexttest.xml", "../result/metafallbacktotexttest.xml");
    }

    /// <summary>
    /// Include element points to another include element, which has a missing resource
    /// and therefore activates a fallback.        
    /// </summary>
    [Fact]
    public void harold_28()
    {
        RunAndCompare("metafallbacktestwithxpointer.xml", "../result/metafallbacktestwithxpointer.xml");
    }

    /// <summary>
    /// An include element can include another include element that
    /// then uses a fallback.        
    /// </summary>
    [Fact]
    public void harold_29()
    {
        RunAndCompare("metafallbacktestwithxpointer2.xml", "../result/metafallbacktestwithxpointer2.xml");
    }

    /// <summary>
    /// An include element can include another include element that
    /// then fails to find a resource, which is a fatal error if there's no fallback.        
    /// </summary>
    [Fact]
    public void harold_30()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("metamissingfallbacktestwithxpointer.xml", ""));
    }

    /// <summary>
    /// An include element can include another include element that
    /// then fails to find a resource, but it has a fallback, which itself has an include child, which then throws a fatal error.        
    /// </summary>
    [Fact]
    public void harold_31()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("metafallbackwithbadxpointertest.xml", ""));
    }

    /// <summary>
    /// Basic test from XInclude spec        
    /// </summary>
    [Fact]
    public void harold_32()
    {
        RunAndCompare("parseequalxml.xml", "../result/c1.xml");
    }

    /// <summary>
    /// An include element points to a document that includes it, using an xpointer to select part of that document.        
    /// </summary>
    [Fact]
    public void harold_33()
    {
        Assert.Throws<CircularInclusionException>(() => RunAndCompare("legalcircle.xml", ""));
    }

    /// <summary>
    /// An include element points to another include element in the same document.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_34()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("recursewithinsamedocument.xml", "../result/recursewithinsamedocument.xml"));
    }

    /// <summary>
    /// Include elements can be siblings        
    /// </summary>
    [Fact]
    public void harold_35()
    {
        RunAndCompare("paralleltest.xml", "../result/paralleltest.xml");
    }

    /// <summary>
    /// Namespaces (and lack thereof) must be preserved in included documents        
    /// </summary>
    [Fact]
    public void harold_36()
    {
        RunAndCompare("namespacetest.xml", "../result/namespacetest.xml");
    }

    /// <summary>
    /// Detect an inclusion loop when an include element refers to itself        
    /// </summary>
    [Fact]
    public void harold_37()
    {
        Assert.Throws<FatalResourceException>(() => RunAndCompare("internalcircular.xml", ""));
    }

    /// <summary>
    /// Detect an inclusion loop when an include element refers to its ancestor element
    /// in the same document        
    /// </summary>
    [Fact]
    public void harold_38()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("internalcircularviaancestor.xml", ""));
    }

    /// <summary>
    /// Processing a document that contains no include elements produces the same document.        
    /// </summary>
    [Fact]
    public void harold_39()
    {
        RunAndCompare("latin1.xml", "../result/latin1.xml");
    }


    /// Basic inclusion        
    /// </summary>
    [Fact]
    public void harold_40()
    {
        RunAndCompare("simple.xml", "../result/simple.xml");
    }

    /// <summary>
    /// The root element of a document can be an include element.        
    /// </summary>
    [Fact]
    public void harold_41()
    {
        RunAndCompare("roottest.xml", "../result/roottest.xml");
    }

    /// <summary>
    /// The root element of a document can be an include element.
    /// In this test the included document has a prolog and an epilog and the root element is replaced        
    /// </summary>
    [Fact]
    public void harold_42()
    {
        RunAndCompare("roottest2.xml", "../result/roottest2.xml");
    }

    /// <summary>
    /// testIncludeElementsCannotHaveIncludeChildren        
    /// </summary>
    [Fact]
    public void harold_43()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("nestedxinclude.xml", ""));
    }

    /// <summary>
    /// Include elements cannot have children from the xinclude namespace except for fallback.        
    /// </summary>
    [Fact]
    public void harold_44()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("nestedxincludenamespace.xml", ""));
    }

    /// <summary>
    /// Fallback can only be a child of xinclude element        
    /// </summary>
    [Fact]
    public void harold_45()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("nakedfallback.xml", ""));
    }

    /// <summary>
    /// A fallback element cannot have a fallback child element.        
    /// </summary>
    [Fact]
    public void harold_46()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("fallbackcontainsfallback.xml", ""));
    }

    /// <summary>
    /// "The appearance of more than one xi:fallback element, an xi:include element,
    /// or any other element from the XInclude namespace is a fatal error."
    /// In this test the fallback is activated.        
    /// </summary>
    [Fact]
    public void harold_47()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("multiplefallbacks.xml", ""));
    }

    /// <summary>
    /// "The appearance of more than one xi:fallback element, an xi:include element,
    /// or any other element from the XInclude namespace is a fatal error."
    /// In this test the fallback is not activated.        
    /// </summary>
    [Fact]
    public void harold_48()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("multiplefallbacks2.xml", ""));
    }

    /// <summary>
    /// A document cannot include itself        
    /// </summary>
    [Fact]
    public void harold_49()
    {
        Assert.Throws<CircularInclusionException>(() =>
            RunAndCompare("circle1.xml", ""));
    }

    /// <summary>
    /// Document A includes document B which includes document A        
    /// </summary>
    [Fact]
    public void harold_50()
    {
        Assert.Throws<CircularInclusionException>(() =>
            RunAndCompare("circle2a.xml", ""));
    }

    /// <summary>
    /// Include element is missing an href and xpointer attribute        
    /// </summary>
    [Fact]
    public void harold_51()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("missinghref.xml", ""));
    }

    /// <summary>
    /// parse attribute must have value xml or text        
    /// </summary>
    [Fact]
    public void harold_52()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("badparseattribute.xml", ""));
    }

    /// <summary>
    /// Missing resource is fatal when there's no fallback        
    /// </summary>
    [Fact]
    public void harold_53()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("missingfile.xml", ""));
    }

    /// <summary>
    /// Missing resource is non-fatal when there's a fallback        
    /// </summary>
    [Fact]
    public void harold_54()
    {
        RunAndCompare("fallbacktest.xml", "../result/fallbacktest.xml");
    }

    /// <summary>
    /// Fallback elements can themselves contain include elements        
    /// </summary>
    [Fact]
    public void harold_55()
    {
        RunAndCompare("fallbacktest2.xml", "../result/fallbacktest2.xml");
    }

    /// <summary>
    /// encoding="UTF-16"        
    /// </summary>
    [Fact]
    public void harold_56()
    {
        RunAndCompare("utf16.xml", "../result/utf16.xml");
    }

    /// <summary>
    /// A shorthand XPointer        
    /// </summary>
    [Fact]
    public void harold_57()
    {
        RunAndCompare("xptridtest.xml", "../result/xptridtest.xml");
    }

    /// <summary>
    /// XPointer that selects nothing is a resource error, and fatal because there's no fallback.        
    /// </summary>
    [Fact]
    public void harold_58()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("xptridtest2.xml", ""));
    }

    /// <summary>
    /// XPointers of the forms described in [XPointer Framework] and [XPointer element() scheme] must be supported.        
    /// </summary>
    [Fact]
    public void harold_59()
    {
        RunAndCompare("xptrtumblertest.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Unrecognized colonized XPointer schemes are skipped, and the following scheme is used.        
    /// </summary>
    [Fact]
    public void harold_60()
    {
        RunAndCompare("colonizedschemename.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Even if the first XPointer part locates a resource, a syntax error in
    /// the second XPointer part is still a fatal error.        
    /// </summary>
    [Fact]
    public void harold_61()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("laterfailure.xml", ""));
    }

    /// <summary>
    /// Even if the first XPointer part locates a resource, a syntax error in
    /// the second XPointer part is still a fatal error.        
    /// </summary>
    [Fact]
    public void harold_62()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("laterfailure2.xml", ""));
    }

    /// <summary>
    /// You can include another element from the same document without an href attribute.        
    /// </summary>
    /// <remarks>INTRA-DOCUMENT REFERENCES ARE NOT SUPPORTED BY THE XIncludingReader.</remarks>
    [Fact]
    public void harold_63()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("onlyxpointer.xml", "../result/onlyxpointer.xml"));
    }

    /// <summary>
    /// Test with 3 element schemes in the XPointer.
    /// The first and second one point to nothing. The third one
    /// selects something. XPointer parts are evaluated from left to right until one finds something.        
    /// </summary>
    [Fact]
    public void harold_64()
    {
        RunAndCompare("xptr2tumblertest.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Test with 2 element schemes in the XPointer.
    /// The first one uses an ID that doesn't exist
    /// and points to nothing. The second one
    /// selects something.        
    /// </summary>
    [Fact]
    public void harold_65()
    {
        RunAndCompare("xptrtumblertest.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Make sure XPointer syntax errors are treated as a resource
    /// error, not a fatal error; and thus fallbacks are applied        
    /// </summary>
    [Fact]
    public void harold_66()
    {
        RunAndCompare("xptrtumblerfailsbutfallback.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Make sure XPointer syntax errors are treated as a resource
    /// error, not a fatal error; and thus fallbacks are applied        
    /// </summary>
    [Fact]
    public void harold_67()
    {
        RunAndCompare("xptrsyntaxerrorbutfallback.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// Test with 3 element schemes in the XPointer, separated by white space.
    /// The first one points to nothing. The third one
    /// selects something.        
    /// </summary>
    [Fact]
    public void harold_68()
    {
        RunAndCompare("xptrtumblertest.xml", "../result/xptrtumblertest.xml");
    }

    /// <summary>
    /// An XPointer that doesn't point to anything is a resource error; and fatal because there's no fallback        
    /// </summary>
    [Fact]
    public void harold_69()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("xptrtumblertest2.xml", ""));
    }

    /// <summary>
    /// Syntax error in an XPointer is a resource error; and fatal because there's no fallback        
    /// </summary>
    [Fact]
    public void harold_70()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("badxptr.xml", ""));
    }

    /// <summary>
    /// Syntax error in an XPointer is a resource error; and fatal because there's no fallback        
    /// </summary>
    [Fact]
    public void harold_71()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("badxptr2.xml", ""));
    }

    /// <summary>
    /// Unrecognized XPointer scheme activates fallback        
    /// </summary>
    [Fact]
    public void harold_72()
    {
        RunAndCompare("xptrfallback.xml", "../result/xptrfallback.xml");
    }

    /// <summary>
    /// XPointer uses an element scheme where the first part is an ID        
    /// </summary>
    [Fact]
    public void harold_73()
    {
        RunAndCompare("xptridandtumblertest.xml", "../result/xptridandtumblertest.xml");
    }

    /// <summary>
    /// Can autodetect UTF16 big endian files with a with a byte order mark when parse="text"        
    /// </summary>
    [Fact]
    public void harold_74()
    {
        RunAndCompare("UTF16BigEndianWithByteOrderMark.xml", "../result/UTF16BigEndianWithByteOrderMark.xml");
    }

    /// <summary>
    /// Can autodetect UTF16 little endian files with a with a byte order mark when parse="text"        
    /// </summary>
    [Fact]
    public void harold_75()
    {
        RunAndCompare("UTF16LittleEndianWithByteOrderMark.xml", "../result/UTF16LittleEndianWithByteOrderMark.xml");
    }

    /// <summary>
    /// Can autodetect UTF-8 files with a with a byte order mark when parse="text"        
    /// </summary>
    [Fact]
    public void harold_76()
    {
        RunAndCompare("UTF8WithByteOrderMark.xml",
            "../result/UTF8WithByteOrderMark.xml");
    }

    /// <summary>
    /// Can autodetect UCS2 big endian files with a without a byte order mark when parse="text"        
    /// </summary>
    [Fact]
    public void harold_77()
    {
        var root = new DirectoryInfo("../../XInclude/XInclude-Test-Suite/Harold/test/");
        XIncludeReaderTests.RunAndCompare(
            "file://" + Path.Combine(root.FullName, "UnicodeBigUnmarked.xml"),
            "../../XInclude/XInclude-Test-Suite/Harold/result/UnicodeBigUnmarked.xml");
    }

    /// <summary>
    /// Can autodetect UCS2 little endian files with a without a byte order mark when parse="text"        
    /// </summary>		
    [Fact]
    public void harold_78()
    {
        var root = new DirectoryInfo("../../XInclude/XInclude-Test-Suite/Harold/test/");
        XIncludeReaderTests.RunAndCompare(
            "file://" + Path.Combine(root.FullName, "UnicodeLittleUnmarked.xml"),
            "../../XInclude/XInclude-Test-Suite/Harold/result/UnicodeLittleUnmarked.xml");
    }



    /// <summary>
    /// Can autodetect EBCDIC files with a without a byte order mark when parse="text"        
    /// </summary>
    /// <remarks>EBCDIC is not supported encoding</remarks>		
    [Fact(Skip = "Couldn't make this work")]
    public void harold_79()
    {
        var root = new DirectoryInfo("../../XInclude/XInclude-Test-Suite/Harold/test/");

        Assert.Throws<FatalResourceException>(() =>
            XIncludeReaderTests.RunAndCompare(
                "file://" + Path.Combine(root.FullName, "EBCDIC.xml"),
                "../../XInclude/XInclude-Test-Suite/Harold/result/EBCDIC.xml"));
    }

    /// <summary>
    /// Syntax error in an XPointer is a resource error; and fatal becaue there's no fallback        
    /// </summary>
    [Fact]
    public void harold_80()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("badxptr3.xml", ""));
    }

    /// <summary>
    /// Syntax error in an XPointer is a resource error; and fatal becaue there's no fallback        
    /// </summary>
    [Fact]
    public void harold_81()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("badxptr4.xml", ""));
    }

    /// <summary>
    /// Circular references via xpointer are fatal        
    /// </summary>
    [Fact]
    public void harold_82()
    {
        Assert.Throws<CircularInclusionException>(() =>
            RunAndCompare("circlepointer1.xml", ""));
    }

    /// <summary>
    /// href attribute with fragment ID is a fatal error even when there's an xpointer attribute        
    /// </summary>
    [Fact]
    public void harold_83()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("xpointeroverridesfragmentid.xml", ""));
    }

    /// <summary>
    /// href attribute with fragment ID is a fatal error        
    /// </summary>
    [Fact]
    public void harold_84()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("ignoresfragmentid.xml", ""));
    }

    /// <summary>
    /// Line breaks must be preserved verbatim when including a document with parse="text"        
    /// </summary>
    [Fact]
    public void harold_85()
    {
        RunAndCompare("lineends.xml", "../result/lineends.xml");
    }

    /// <summary>
    /// A fragment identifier is semantically bad; but still meets the
    /// syntax of fragment IDs from RFC 2396.        
    /// </summary>
    [Fact]
    public void harold_86()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("meaninglessfragmentid.xml", ""));
    }

    /// <summary>
    /// accept-language="fr"
    /// This test connects to IBiblio to load the included
    /// data. This is necessary because file URLs don't support
    /// content negotiation        
    /// </summary>
    [Fact]
    public void harold_87()
    {
        RunAndCompare("acceptfrench.xml", "../result/acceptfrench.xml");
    }

    /// <summary>
    /// accept-language="en"
    /// This test connects to IBiblio to load the included
    /// data. This is necessary because file URLs don't support
    /// content negotiation        
    /// </summary>
    [Fact]
    public void harold_88()
    {
        RunAndCompare("acceptenglish.xml", "../result/acceptenglish.xml");
    }

    /// <summary>
    /// accept="text/plain"
    /// This test connects to IBiblio to load the included
    /// data. This is necessary because file URLs don't support
    /// content negotiation        
    /// </summary>
    [Fact]
    public void harold_89()
    {
        RunAndCompare("acceptplaintext.xml", "../result/acceptplaintext.xml");
    }

    /// <summary>
    /// accept="text/html"
    /// This test connects to IBiblio to load the included
    /// data. This is necessary because file URLs don't support
    /// content negotiation        
    /// </summary>
    [Fact]
    public void harold_90()
    {
        RunAndCompare("accepthtml.xml", "../result/accepthtml.xml");
    }

    /// <summary>
    /// Unrecognized scheme in XPointer is a fatal error if there's no fallback        
    /// </summary>
    [Fact]
    public void harold_91()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("unrecognizedscheme.xml", ""));
    }

    /// <summary>
    /// Unrecognized scheme in XPointer is a resource error so fallbacks apply        
    /// </summary>
    [Fact]
    public void harold_92()
    {
        RunAndCompare("unrecognizedschemewithfallback.xml", "../result/unrecognizedschemewithfallback.xml");
    }

    /// <summary>
    /// Basic inclusions as XML and text        
    /// </summary>
    [Fact]
    public void harold_93()
    {
        RunAndCompare("test.xml", "../result/test.xml");
    }



    /// <summary>
    /// Included document has an include element with neither href nor xpointer attribute        
    /// </summary>
    [Fact]
    public void harold_94()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
            RunAndCompare("toplevel.xml", ""));
    }

    /// <summary>
    /// XPointers are resolved against the acquired infoset, not thge source infoset        
    /// </summary>
    [Fact]
    public void harold_95()
    {
        RunAndCompare("tobintop.xml", "../result/tobintop.xml");
    }

    /// <summary>
    /// Test that a non-child sequence in an xpointer is treated as a resource error.        
    /// </summary>
    [Fact]
    public void harold_96()
    {
        RunAndCompare("badelementschemedata.xml", "../result/badelementschemedata.xml");
    }

    /// <summary>
    /// Since the xpointer attribute is not a URI reference, %-escaping must not appear in the XPointer, nor is there any need for a processor to apply or reverse such escaping.        
    /// </summary>
    [Fact]
    public void harold_97()
    {
        Assert.Throws<FatalResourceException>(() =>
            RunAndCompare("xpointerwithpercentescape.xml", ""));
    }
}
