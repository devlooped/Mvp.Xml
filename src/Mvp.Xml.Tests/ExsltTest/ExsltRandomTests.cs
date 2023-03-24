using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT Random module functions.
/// </summary>
public class ExsltRandomTests : ExsltUnitTests
{
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/Random/";
    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/Random/";

    /// <summary>
    /// Tests the following function:
    ///     random:random-sequence()
    /// </summary>
    [Fact]
    public void RandomSequenceTest()
    {
        RunAndCompare("source.xml", "random-sequence.xslt", "random-sequence.xml");
    }
}
