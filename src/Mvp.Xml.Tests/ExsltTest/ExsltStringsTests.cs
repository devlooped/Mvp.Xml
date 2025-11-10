using Xunit;
using Xunit.Abstractions;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT Strings module functions.
/// </summary>
public class ExsltStringsTests(ITestOutputHelper output) : ExsltUnitTests(output)
{
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/Strings/";
    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/Strings/";

    /// <summary>
    /// Tests the following function:
    ///     str:tokenize()
    /// </summary>
    [Fact]
    public void TokenizeTest()
    {
        RunAndCompare("source.xml", "tokenize.xslt", "tokenize.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:replace()
    /// </summary>
    [Fact]
    public void ReplaceTest()
    {
        RunAndCompare("source.xml", "replace.xslt", "replace.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:padding()
    /// </summary>
    [Fact]
    public void PaddingTest()
    {
        RunAndCompare("source.xml", "padding.xslt", "padding.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:align()
    /// </summary>
    [Fact]
    public void AlignTest()
    {
        RunAndCompare("source.xml", "align.xslt", "align.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:encode-uri()
    /// </summary>
    [Fact]
    public void EncodeUriTest()
    {
        RunAndCompare("source.xml", "encode-uri.xslt", "encode-uri.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:decode-uri()
    /// </summary>
    [Fact]
    public void DecodeUriTest()
    {
        RunAndCompare("source.xml", "decode-uri.xslt", "decode-uri.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:concat()
    /// </summary>
    [Fact]
    public void ConcatTest()
    {
        RunAndCompare("source.xml", "concat.xslt", "concat.xml");
    }

    /// <summary>
    /// Tests the following function:
    ///     str:split()
    /// </summary>
    [Fact]
    public void SplitTest()
    {
        RunAndCompare("source.xml", "split.xslt", "split.xml");
    }
}
