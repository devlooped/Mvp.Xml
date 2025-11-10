using Xunit;
using Xunit.Abstractions;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet Math module functions.
/// </summary>
public class GDNMathTests(ITestOutputHelper output) : ExsltUnitTests(output)
{
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/Math/";
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/Math/";

    /// <summary>
    /// Tests the following function:
    ///     math2:avg()
    /// </summary>
    [Fact]
    public void AvgTest()
    {
        RunAndCompare("source.xml", "avg.xslt", "avg.xml");
    }
}
