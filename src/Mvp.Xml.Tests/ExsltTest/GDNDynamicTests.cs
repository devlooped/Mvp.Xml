using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet Dynamic module functions.
/// </summary>
public class GDNDynamicTests : ExsltUnitTests
{        
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/Dynamic/"; 
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/Dynamic/"; 
    
    /// <summary>
    /// Tests the following function:
    ///     dyn2:evaluate()
    /// </summary>
    [Fact]
    public void EvaluateTest() 
    {
        RunAndCompare("source.xml", "evaluate.xslt", "evaluate.xml");
    }        
}
