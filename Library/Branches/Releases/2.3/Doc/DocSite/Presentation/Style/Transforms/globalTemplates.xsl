<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
        >

  <xsl:template name="autogenSeeAlsoLinks">
      
    <!-- a link to the containing type on all list and member topics -->
    <xsl:if test="($group='member' or $group='list')">
      <xsl:variable name="typeTopicId">
        <xsl:choose>
          <xsl:when test="/document/reference/topicdata/@typeTopicId">
            <xsl:value-of select="/document/reference/topicdata/@typeTopicId"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="/document/reference/containers/type/@api"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <div class="seeAlsoStyle">
        <include item="SeeAlsoTypeLinkText">
          <parameter>
            <referenceLink target="{$typeTopicId}" />
          </parameter>
          <parameter>
            <xsl:value-of select="/document/reference/containers/type/apidata/@subgroup"/>
          </parameter>
        </include>
      </div>
    </xsl:if>

    <!-- a link to the type's All Members list -->
    <xsl:variable name="allMembersTopicId">
      <xsl:choose>
        <xsl:when test="/document/reference/topicdata/@allMembersTopicId">
          <xsl:value-of select="/document/reference/topicdata/@allMembersTopicId"/>
        </xsl:when>
        <xsl:when test="$group='member' or ($group='list' and $subgroup='overload')">
          <xsl:value-of select="/document/reference/containers/type/topicdata/@allMembersTopicId"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="normalize-space($allMembersTopicId) and not($allMembersTopicId=$key)">
      <div class="seeAlsoStyle">
        <include item="SeeAlsoMembersLinkText">
          <parameter>
            <referenceLink target="{$allMembersTopicId}" />
          </parameter>
        </include>
      </div>
    </xsl:if>

    <!-- a link to the namespace topic -->
    <xsl:variable name="namespaceId">
      <xsl:value-of select="/document/reference/containers/namespace/@api"/>
    </xsl:variable>
    <xsl:if test="normalize-space($namespaceId)">
      <div class="seeAlsoStyle">
        <include item="SeeAlsoNamespaceLinkText">
          <parameter>
            <referenceLink target="{$namespaceId}" />
          </parameter>
        </include>
      </div>
    </xsl:if>
   
  </xsl:template>

  <xsl:variable name="typeId">
    <xsl:choose>
      <xsl:when test="/document/reference/topicdata[@group='api'] and /document/reference/apidata[@group='type']">
        <xsl:value-of select="$key"/>
      </xsl:when>
      <xsl:when test="/document/reference/topicdata/@typeTopicId">
        <xsl:value-of select="/document/reference/topicdata/@typeTopicId"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="/document/reference/containers/type/@api"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <!-- indent by 2*n spaces -->
  <xsl:template name="indent">
    <xsl:param name="count" />
    <xsl:if test="$count &gt; 1">
      <xsl:text>&#160;&#160;</xsl:text>
      <xsl:call-template name="indent">
        <xsl:with-param name="count" select="$count - 1" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="codeSection">
    <xsl:param name="codeLang" />
    <div class="code">
      <span codeLanguage="{$codeLang}">
        <table width="100%" cellspacing="0" cellpadding="0">
          <tr>
            <th>
              <include item="{$codeLang}"/>
              <xsl:text>&#xa0;</xsl:text>
            </th>
            <th>
              <span class="copyCode" onclick="CopyCode(this)" onkeypress="CopyCode_CheckKey(this, event)" onmouseover="ChangeCopyCodeIcon(this)" onmouseout="ChangeCopyCodeIcon(this)" tabindex="0">
                <img class="copyCodeImage" name="ccImage" align="absmiddle">
                  <includeAttribute name="alt" item="copyImage" />
                  <includeAttribute name="src" item="iconPath">
                    <parameter>copycode.gif</parameter>
                  </includeAttribute>
                </img>
                <include item="copyCode"/>
              </span>
            </th>
          </tr>
          <tr>
            <td colspan="2">
              <pre>
                <xsl:apply-templates/>
              </pre>
            </td>
          </tr>
        </table>
      </span>
    </div>

  </xsl:template>

</xsl:stylesheet>