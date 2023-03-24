using Xunit;

namespace Mvp.Xml.XInclude.Test;

/// <summary>
/// XInclude syntax tests.
/// </summary>

public class XIncludeSyntaxTests
{
    public XIncludeSyntaxTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    /// <summary>
    /// No href and no xpointer attribute.
    /// </summary>
    [Fact]
    public void NoHrefAndNoXPointerAttributes()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
        {
            var xir = new XIncludingReader("../../XInclude/tests/nohref.xml");
            while (xir.Read()) ;
            xir.Close();
        });
    }

    /// <summary>
    /// xi:include child of xi:include.
    /// </summary>
    [Fact]
    public void IncludeChildOfInclude()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
        {
            var xir = new XIncludingReader("../../XInclude/tests/includechildofinclude.xml");
            while (xir.Read()) ;
            xir.Close();
        });
    }

    /// <summary>
    /// xi:fallback not child of xi:include.
    /// </summary>
    [Fact]
    public void FallbackNotChildOfInclude()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
        {
            var xir = new XIncludingReader("../../XInclude/tests/fallbacknotchildinclude.xml");
            while (xir.Read()) ;
            xir.Close();
        });
    }

    /// <summary>
    /// Unknown value of parse attribute.
    /// </summary>
    [Fact]
    public void UnknownParseAttribute()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
        {
            var xir = new XIncludingReader("../../XInclude/tests/unknownparseattr.xml");
            while (xir.Read()) ;
            xir.Close();
        });
    }

    /// <summary>
    /// Two xi:fallback.
    /// </summary>
    [Fact]
    public void TwoFallbacks()
    {
        Assert.Throws<XIncludeSyntaxError>(() =>
        {
            var xir = new XIncludingReader("../../XInclude/tests/twofallbacks.xml");
            while (xir.Read()) ;
            xir.Close();
        });
    }
}
