using Xunit;
using Xunit.Abstractions;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet Sets module functions.
/// </summary>
public class GDNSetsTests(ITestOutputHelper output) : ExsltUnitTests(output)
{
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/Sets/";
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/Sets/";

    /// <summary>
    /// Tests the following function:
    ///     set2:subset()
    /// </summary>
    [Fact]
    public void SubsetTest()
    {
        RunAndCompare("source.xml", "subset.xslt", "subset.xml");
    }
}
