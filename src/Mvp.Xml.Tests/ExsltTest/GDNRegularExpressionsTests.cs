using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet RegularExpressions module functions.
/// </summary>
public class GDNRegularExpressionsTests : ExsltUnitTests
{        
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/RegularExpressions/"; 
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/RegularExpressions/"; 
                   
    /// <summary>
    /// Tests the following function:
    ///     regexp2:tokenize()
    /// </summary>
    [Fact]
    public void TokenizeTest() 
    {
        RunAndCompare("source.xml", "tokenize.xslt", "tokenize.xml");
    }                    
}
