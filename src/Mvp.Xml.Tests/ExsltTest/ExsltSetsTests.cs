using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT Sets module functions.
/// </summary>
public class ExsltSetsTests : ExsltUnitTests
{        
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/Sets/"; 
    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/Sets/"; 
                   
    /// <summary>
    /// Tests the following function:
    ///     set:difference()
    /// </summary>
    [Fact]
    public void DifferenceTest() 
    {
        RunAndCompare("source.xml", "difference.xslt", "difference.xml");
    }
    
    /// <summary>
    /// Tests the following function:
    ///     set:intersection()
    /// </summary>
    [Fact]
    public void IntersectionTest() 
    {
        RunAndCompare("source.xml", "intersection.xslt", "intersection.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     set:distinct()
    /// </summary>
    [Fact]
    public void DistinctTest() 
    {
        RunAndCompare("source.xml", "distinct.xslt", "distinct.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     set:has-same-node()
    /// </summary>
    [Fact]
    public void HasSameNodeTest() 
    {
        RunAndCompare("source.xml", "has-same-node.xslt", "has-same-node.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     set:leading()
    /// </summary>
    [Fact]
    public void LeadingTest() 
    {
        RunAndCompare("source.xml", "leading.xslt", "leading.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     set:trailing()
    /// </summary>
    [Fact]
    public void TrailingTest() 
    {
        RunAndCompare("source.xml", "trailing.xslt", "trailing.xml");
    }        
}
