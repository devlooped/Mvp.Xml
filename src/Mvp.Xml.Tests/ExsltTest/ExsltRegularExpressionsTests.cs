using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT RegularExpressions module functions.
/// </summary>
public class ExsltRegularExpressionsTests : ExsltUnitTests
{
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/RegularExpressions/";
    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/RegularExpressions/";

    /// <summary>
    /// Tests the following function:
    ///     regexp:test()
    /// </summary>
    [Fact]
    public void TestTest()
    {
        RunAndCompare("source.xml", "test.xslt", "test.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     regexp:match()
    /// </summary>
    [Fact]
    public void MatchTest()
    {
        RunAndCompare("source.xml", "match.xslt", "match.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     regexp:replace()
    /// </summary>
    [Fact]
    public void ReplaceTest()
    {
        RunAndCompare("source.xml", "replace.xslt", "replace.xml");
    }
}
