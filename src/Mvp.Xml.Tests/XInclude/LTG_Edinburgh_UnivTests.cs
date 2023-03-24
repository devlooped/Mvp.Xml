using Xunit;

namespace Mvp.Xml.XInclude.Test;

/// <summary>
/// Edinburgh University test cases from the XInclude Test suite.
/// </summary>

public class LTG_Edinburgh_UnivTests
{
    public LTG_Edinburgh_UnivTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result)
    {
        XIncludeReaderTests.RunAndCompare(
                "../../XInclude/XInclude-Test-Suite/EdUni/test/" + source,
                "../../XInclude/XInclude-Test-Suite/EdUni/test/" + result);
    }

    /// <summary>
    /// Simple whole-file inclusion.        
    /// </summary>
    [Fact]
    public void eduni_1()
    {
        RunAndCompare("book.xml", "../result/book.xml");
    }

    /// <summary>
    /// Verify that xi:include elements in the target have been processed in the acquired infoset, ie before the xpointer is applied.        
    /// </summary>
    [Fact]
    public void eduni_2()
    {
        RunAndCompare("extract.xml", "../result/extract.xml");
    }

    /// <summary>
    /// Check xml:lang fixup        
    /// </summary>
    [Fact]
    public void eduni_3()
    {
        RunAndCompare("lang.xml", "../result/lang.xml");
    }
}
