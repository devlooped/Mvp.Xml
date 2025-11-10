using Xunit;
using Xunit.Abstractions;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for GotDotNet Dates and Times module functions.
/// </summary>
public class GDNDatesAndTimesTests(ITestOutputHelper output) : ExsltUnitTests(output)
{
    protected override string TestDir => "../../ExsltTest/tests/GotDotNet/DatesAndTimes/";
    protected override string ResultsDir => "../../ExsltTest/results/GotDotNet/DatesAndTimes/";

    /// <summary>
    /// Tests the following function:
    ///     date2:avg()
    /// </summary>
    [Fact]
    public void AvgTest()
    {
        RunAndCompare("source.xml", "avg.xslt", "avg.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:min()
    /// </summary>
    [Fact]
    public void MinTest()
    {
        RunAndCompare("source.xml", "min.xslt", "min.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:max()
    /// </summary>
    [Fact]
    public void MaxTest()
    {
        RunAndCompare("source.xml", "max.xslt", "max.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:day-name()
    /// </summary>
    [Fact]
    public void DayNameTest()
    {
        RunAndCompare("source.xml", "day-name.xslt", "day-name.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:day-abbreviation()
    /// </summary>
    [Fact]
    public void DayAbbreviationTest()
    {
        RunAndCompare("source.xml", "day-abbreviation.xslt", "day-abbreviation.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:month-name()
    /// </summary>
    [Fact]
    public void MonthNameTest()
    {
        RunAndCompare("source.xml", "month-name.xslt", "month-name.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     date2:month-abbreviation()
    /// </summary>
    [Fact]
    public void MonthAbbreviationTest()
    {
        RunAndCompare("source.xml", "month-abbreviation.xslt", "month-abbreviation.xml");
    }
}
