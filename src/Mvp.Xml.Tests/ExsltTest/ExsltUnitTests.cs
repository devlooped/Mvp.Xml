using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Mvp.Xml.Common.Xsl;
using Xunit;

namespace ExsltTest;

/// <summary>
/// Superclass for unit tests.
/// </summary>
public class ExsltUnitTests
{
    protected virtual string TestDir => "../../ExsltTest/tests/EXSLT/Common/";
    protected virtual string ResultsDir => "../../ExsltTest/results/EXSLT/Common/";

    protected void RunAndCompare(string source, string stylesheet,
        string result)
    {
        var doc = new XPathDocument(TestDir + source);
        var res = new StringWriter();

        // deprecated
        //ExsltTransform exslt = new ExsltTransform();
        //exslt.Load(TestDir + stylesheet);
        //exslt.Transform(doc, null, res);

        var transform = new MvpXslTransform();
        transform.Load(TestDir + stylesheet);
        transform.Transform(new XmlInput(doc), null, new XmlOutput(res));

        var sr = new StreamReader(ResultsDir + result);
        var expectedResult = sr.ReadToEnd();
        var expected = XDocument.Load(new StringReader(expectedResult));

        var actualResult = res.ToString();
        var actual = XDocument.Load(new StringReader(actualResult));

        sr.Close();

        var areEqual = XNode.DeepEquals(expected, actual);
        if (!areEqual)
        {
            Console.WriteLine(@"Actual Result was {0}", actualResult);
            Console.WriteLine(@"Expected Result was {0}", expectedResult);
        }
        Assert.True(areEqual);
    }
}
