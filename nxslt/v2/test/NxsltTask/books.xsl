<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
	<html>
		<xsl:apply-templates/>
	</html>
	</xsl:template>

	<xsl:template match="bookstore">
	<!-- Prices and books -->
		<table>
			<xsl:apply-templates select="book"/>
		</table>
	</xsl:template>

	<xsl:template match="book">
		<tr>
                        <td>
				<xsl:value-of select="@ISBN"/>
			</td>
			<td><xsl:value-of select="price"/></td>
		</tr>
	</xsl:template>

</xsl:stylesheet>
