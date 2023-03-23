using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet Strings module functions.
/// </summary>
public class GDNStringsTests : ExsltUnitTests
{        
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/Strings/"; 
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/Strings/"; 

    /// <summary>
    /// Tests the following function:
    ///     str2:lowercase()
    /// </summary>
    [Fact]
    public void LowercaseTest() 
    {
        RunAndCompare("source.xml", "lowercase.xslt", "lowercase.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str2:uppercase()
    /// </summary>
    [Fact]
    public void UppercaseTest() 
    {
        RunAndCompare("source.xml", "uppercase.xslt", "uppercase.xml");
    }
}
