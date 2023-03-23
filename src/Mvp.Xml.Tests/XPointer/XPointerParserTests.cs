using Xunit;

namespace Mvp.Xml.XPointer.Test;

/// <summary>
/// Summary description for XPointerParserTests.
/// </summary>

public class XPointerParserTests
{
    public XPointerParserTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    [Fact]
    public void SyntaxErrorTest()
    {
        Assert.Throws<XPointerSyntaxException>(() => 
            Pointer.Compile("too bad"));
    }

    [Fact]
    public void ParenthesisTest()
    {
        var p = Pointer.Compile("xmlns(p=http://foo.com^))");
        p = Pointer.Compile("xmlns(p=http://foo.com^()");
    }

    [Fact]
    public void EscapingCircumflexTest()
    {
        var p = Pointer.Compile("xmlns(p=http://foo.com^^)");
    }

    [Fact]
    public void CircumflexErrorTest()
    {
        Assert.Throws<XPointerSyntaxException>(() =>
            Pointer.Compile("xmlns(p=http://fo^o.com)"));
    }

    [Fact]
    public void BadNCName()
    {
        Assert.Throws<XPointerSyntaxException>(() =>
            Pointer.Compile("foo:bar"));
    }

    [Fact]
    public void BadElementPointer()
    {
        Assert.Throws<XPointerSyntaxException>(() =>
            Pointer.Compile("element(1/33/foo)"));
    }

    [Fact]
    public void UnknownSchemePointer()
    {
        var p = Pointer.Compile("xpath1(/foo) foo(abr)");
    }
}
