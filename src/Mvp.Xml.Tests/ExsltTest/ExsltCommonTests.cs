using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT Common module functions.
/// </summary>
public class ExsltCommonTests : ExsltUnitTests
{
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/Common/";

    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/Common/";

    /// <summary>
    /// Tests the following function:
    ///     exsl:node-set()
    /// </summary>
    [Fact]
    public void NodeSetTest()
    {
        RunAndCompare("source.xml", "node-set.xslt", "node-set.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     exsl:object-type()
    /// </summary>
    [Fact]
    public void ObjectTypeTest()
    {
        RunAndCompare("source.xml", "object-type.xslt", "object-type.xml");
    }
}
