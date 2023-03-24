using Xunit;

namespace Mvp.Xml.Serialization.Tests;

public class StringSorterHelperTests
{
    public StringSorterHelperTests()
    {

    }

    const string s1 = "Alex";
    const string s2 = "Bert";
    const string s3 = "Christoph";

    [Fact]
    public void SortStrings()
    {
        var sorter = new StringSorter();
        sorter.AddString(s3);
        sorter.AddString(s2);
        sorter.AddString(s1);

        AssertStringOrder(sorter.GetOrderedArray());
    }

    [Fact]
    public void SortMoreStrings()
    {
        var sorter = new StringSorter();
        sorter.AddString(s2);
        sorter.AddString(s1);
        sorter.AddString(s3);

        AssertStringOrder(sorter.GetOrderedArray());
    }

    [Fact]
    public void SortStrings2()
    {
        var sorter = new StringSorter();
        sorter.AddString(s3);
        sorter.AddString(s1);
        sorter.AddString(s2);

        AssertStringOrder(sorter.GetOrderedArray());
    }

    [Fact]
    public void SortStrings3()
    {
        var sorter = new StringSorter();
        sorter.AddString(s1);
        sorter.AddString(s2);
        sorter.AddString(s3);

        AssertStringOrder(sorter.GetOrderedArray());
    }
    void AssertStringOrder(string[] sortedArray)
    {
        Assert.Equal(s1, sortedArray[0]);
        Assert.Equal(s2, sortedArray[1]);
        Assert.Equal(s3, sortedArray[2]);

    }
}
