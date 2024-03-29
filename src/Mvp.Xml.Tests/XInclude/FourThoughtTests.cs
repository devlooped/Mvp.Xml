using Xunit;

namespace Mvp.Xml.XInclude.Test;

/// <summary>
/// Edinburgh University test cases from the XInclude Test suite.
/// </summary>

public class FourThoughtTests
{
    public FourThoughtTests()
    {
        //Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
    }

    /// <summary>
    /// Utility method for running tests.
    /// </summary>        
    public static void RunAndCompare(string source, string result)
    {
        XIncludeReaderTests.RunAndCompare(
            "../../XInclude/XInclude-Test-Suite/FourThought/test/XInclude/docs/" + source,
                "../../XInclude/XInclude-Test-Suite/FourThought/test/XInclude/docs/" + result);
    }

    /// <summary>
    /// Simple test of including another XML document.        
    /// </summary>
    [Fact]
    public void FourThought_include_01()
    {
        RunAndCompare("ft-include1.xml", "../../../result/XInclude/include1.xml");
    }

    /// <summary>
    /// Test recursive inclusion.        
    /// </summary>
    [Fact]
    public void FourThought_include_02()
    {
        RunAndCompare("ft-include2.xml", "../../../result/XInclude/include2.xml");
    }


    /// <summary>
    /// Simple test of including another text document.        
    /// </summary>
    [Fact]
    public void FourThought_include_03()
    {
        RunAndCompare("ft-include3.xml", "../../../result/XInclude/include3.xml");
    }

    /// <summary>
    /// Simple test of including a set of nodes from an XML document.        
    /// </summary>
    [Fact]
    public void FourThought_include_04()
    {
        RunAndCompare("ft-include4.xml", "../../../result/XInclude/include4.xml");
    }

    /// <summary>
    /// Simple test of including a set of nodes from an XML document.        
    /// </summary>
    [Fact]
    public void FourThought_include_05()
    {
        RunAndCompare("ft-include5.xml", "../../../result/XInclude/include5.xml");
    }

    /// <summary>
    /// Simple test of including a set of nodes from an XML document.        
    /// </summary>
    [Fact]
    public void FourThought_include_06()
    {
        RunAndCompare("ft-include6.xml", "../../../result/XInclude/include6.xml");
    }

    /// <summary>
    /// Simple test of including a set of nodes from an XML document.        
    /// </summary>
    [Fact]
    public void FourThought_include_07()
    {
        RunAndCompare("ft-include7.xml", "../../../result/XInclude/include7.xml");
    }
}