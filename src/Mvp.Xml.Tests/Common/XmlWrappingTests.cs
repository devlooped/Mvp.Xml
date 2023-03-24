using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Mvp.Xml.Common;
using Xunit;

namespace Mvp.Xml.Tests;

public class XmlWrappingTests
{
    [Fact]
    public void ShouldCloseFullElement()
    {
        var xslt = @"
<xslt:stylesheet version=""1.0"" xmlns:xslt=""http://www.w3.org/1999/XSL/Transform"">
  <xslt:output indent=""yes""/>
  <xslt:variable name=""g_jsserver"">http://clariusconsulting.net/</xslt:variable>
  <xslt:template match=""/"">
	<page>
		<xslt:call-template name=""WriteScriptTag"">
		  <xslt:with-param name=""js_url"">foo.js</xslt:with-param>
		</xslt:call-template>
	</page>
  </xslt:template>
  <xslt:template name=""WriteScriptTag"">
    <xslt:param name=""js_url"" />
    <xslt:param name=""js_base"" select=""$g_jsserver"" />
    <xslt:if test=""$js_url!=''"">
      <script type=""text/javascript"">
        <xslt:attribute name=""src"">
          <xslt:choose>
            <xslt:when test=""substring($js_url,1,5) = 'http:'"">
              <xslt:value-of select=""$js_url"" />
            </xslt:when>
            <xslt:when test=""substring($js_url,1,6) = 'https:'"">
              <xslt:value-of select=""$js_url"" />
            </xslt:when>
            <xslt:otherwise>
              <xslt:value-of select=""$js_base"" />
              <xslt:value-of select=""$js_url"" />
            </xslt:otherwise>
          </xslt:choose>
        </xslt:attribute>
		<xslt:text> </xslt:text>
      </script>
    </xslt:if>
  </xslt:template>
</xslt:stylesheet>";

        var input = "<root />";

        var sw = new StringWriter();
        var xw = new ScriptCloseWriter(XmlWriter.Create(sw));

        //Old v.1.x transform would also work fine.
        //XslTransform tx = new XslTransform();
        var tx = new XslCompiledTransform();
        tx.Load(XmlReader.Create(new StringReader(xslt)));

        tx.Transform(XmlReader.Create(new StringReader(input)), xw);

        xw.Close();

        Console.WriteLine(sw.ToString());
    }

    class ScriptCloseWriter : XmlWrappingWriter
    {
        string lastElement = String.Empty;

        public ScriptCloseWriter(XmlWriter baseWriter) : base(baseWriter) { }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(prefix, localName, ns);
            lastElement = localName;
        }

        public override void WriteEndElement()
        {
            if (lastElement == "script")
            {
                base.WriteFullEndElement();
            }
            else
            {
                base.WriteEndElement();
            }
        }
    }
}
