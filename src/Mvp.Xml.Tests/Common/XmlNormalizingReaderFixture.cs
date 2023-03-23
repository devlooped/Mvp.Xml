using Mvp.Xml.Common;
using Xunit;

namespace Mvp.Xml.Tests.Common;

public class XmlNormalizingReaderFixture : TestFixtureBase
{
    [Fact]
    public void HidesDuplicateNamespace()
    {
        var source = "<item xmlns:sx='sse'><data xmlns:sx='sse' id='3'/></item>";
        var reader = new XmlNormalizingReader(GetReader(source));

        reader.MoveToContent();
        Assert.Equal(1, reader.AttributeCount);
        Assert.True(reader.MoveToFirstAttribute());

        reader.MoveToElement();
        reader.Read();

        Assert.Equal(1, reader.AttributeCount);
        Assert.True(reader.MoveToFirstAttribute());
        Assert.Equal("id", reader.LocalName);
        Assert.Equal("3", reader.Value);
        Assert.False(reader.MoveToNextAttribute());
    }

    [Fact]
    public void ReaderDoesNotReportDuplicateNamespaces()
    {
        var source = @"
		<item xmlns:sx='http://www.microsoft.com/schemas/rss/sse' xmlns:sa3='http://www.microsoft.com/schemas/sa3/request' xmlns:geo='geo-tagging'>
				<sx:sync id='101' version='2' deleted='false' noconflicts='false' xmlns:sx='http://www.microsoft.com/schemas/rss/sse'/>
				<title>12345fgcomputers, projectors, ptz cameras, and PC speakerphones for video wall</title>
				<sa3:Data xmlns:sa3='http://www.microsoft.com/schemas/sa3/request'>
				  <sa3:ID>Robert Kirkpatrick/Groove_Sun, 23 Jul 2006 04:07:46 GMT_65576216301.309654</sa3:ID>
				  <unknown-element xmlns='kzu-unknown'/>
				</sa3:Data>
				<geo:location xmlns:geo='geo-tagging'>
				  <geo:latitude>120</geo:latitude>
				</geo:location>
				<sa3:Info xmlns:sa3='http://www.microsoft.com/schemas/sa3/request'>kzu</sa3:Info>
		</item>
				";

        var expected = NormalizeFormat(@"
		<item xmlns:sx='http://www.microsoft.com/schemas/rss/sse' xmlns:sa3='http://www.microsoft.com/schemas/sa3/request' xmlns:geo='geo-tagging'>
				<sx:sync id='101' version='2' deleted='false' noconflicts='false'/>
				<title>12345fgcomputers, projectors, ptz cameras, and PC speakerphones for video wall</title>
				<sa3:Data>
				  <sa3:ID>Robert Kirkpatrick/Groove_Sun, 23 Jul 2006 04:07:46 GMT_65576216301.309654</sa3:ID>
				  <unknown-element xmlns='kzu-unknown'/>
				</sa3:Data>
				<geo:location>
				  <geo:latitude>120</geo:latitude>
				</geo:location>
				<sa3:Info>kzu</sa3:Info>
		</item>
				");

        var reader = new XmlNormalizingReader(GetReader(source));
        var actual = ReadToEnd(reader);

        Assert.Equal(expected, actual);
    }
}
