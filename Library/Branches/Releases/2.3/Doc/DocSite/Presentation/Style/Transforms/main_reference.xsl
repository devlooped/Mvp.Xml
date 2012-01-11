<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1"
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >

	<!-- stuff specific to comments authored in DDUEXML -->

	<xsl:include href="utilities_reference.xsl" />
	<xsl:include href="utilities_dduexml.xsl" />
  <xsl:include href="htmlBody.xsl"/>
  
  <xsl:variable name="summary" select="normalize-space(/document/comments/ddue:dduexml/ddue:summary)" />
  <xsl:variable name="hasSeeAlsoSection" 
                select="boolean( 
                           (count(/document/comments/ddue:dduexml/ddue:relatedTopics/*) > 0)  or 
                           ($group='type' or $group='member' or $group='list')
                        )"/>
  <xsl:variable name="examplesSection" select="boolean(string-length(/document/comments/ddue:dduexml/ddue:codeExamples[normalize-space(.)]) > 0)"/>
  <xsl:variable name="languageFilterSection" select="boolean(string-length(/document/comments/ddue:dduexml/ddue:codeExamples[normalize-space(.)]) > 0)" />
	<xsl:template name="body">

    <!-- generic topics boilerplate -->
    <xsl:if test="/document/reference/templates">
      <p><span class="introStyle"><include item="genericIntro" /></span></p>
    </xsl:if>

    <!--internalOnly boilerplate -->
    <xsl:call-template name="internalOnly"/>

    <!-- obsolete boilerplate -->
    <xsl:if test="/document/reference/attributes/attribute/type[@api='T:System.ObsoleteAttribute']">
      <xsl:call-template name="obsoleteSection" />
    </xsl:if>

    <!-- HostProtectionAttribute boilerplate -->
    <xsl:if test="/document/reference/attributes/attribute/type[@api='T:System.Security.Permissions.HostProtectionAttribute']">
      <p>
        <include item="hostProtectionAttributeLong">
          <parameter>
            <xsl:value-of select="concat($subgroup, 'Lower')"/>
          </parameter>
          <parameter>
            <b>
            <xsl:for-each select="/document/reference/attributes/attribute[type[@api='T:System.Security.Permissions.HostProtectionAttribute']]/assignment">
              <xsl:value-of select="@name"/>
              <xsl:if test="position() != last()">
                <xsl:text> | </xsl:text>
              </xsl:if>
            </xsl:for-each>
            </b>
          </parameter>
         </include>
      </p>
    </xsl:if>
    
    <!-- summary -->
		<xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:summary" />
    <xsl:if test="$group='namespace'">
      <xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:remarks" />
    </xsl:if>
       
    <!-- assembly information -->
    <xsl:if test="not($group='list' or $group='root' or $group='namespace')">
      <xsl:call-template name="requirementsInfo"/>
    </xsl:if>
    
    <!-- syntax -->
    <xsl:if test="not($group='list' or $group='namespace')">
      <xsl:apply-templates select="/document/syntax" />
    </xsl:if>

    <!-- show authored Dependency Property Information section for properties -->
    <xsl:if test="$subgroup='property'">
      <xsl:apply-templates select="//ddue:section[starts-with(@address,'dependencyPropertyInfo')]" mode="section"/>
    </xsl:if>

    <!-- show authored Routed Event Information section for events -->
    <xsl:if test="$subgroup='event'">
      <xsl:apply-templates select="//ddue:section[starts-with(@address,'routedEventInfo')]" mode="section"/>
    </xsl:if>

    <!-- members -->
		<xsl:choose>
			<xsl:when test="$group='root'">
				<xsl:apply-templates select="/document/reference/elements" mode="root" />
			</xsl:when>
			<xsl:when test="$group='namespace'">
				<xsl:apply-templates select="/document/reference/elements" mode="namespace" />
			</xsl:when>
			<xsl:when test="$subgroup='enumeration'">
        			<xsl:apply-templates select="/document/reference/elements" mode="enumeration" />
			</xsl:when>
			<xsl:when test="$group='type'">
				<xsl:apply-templates select="/document/reference/elements" mode="type" />
			</xsl:when>
      <xsl:when test="$group='list'">
        <xsl:choose>
          <xsl:when test="$subgroup='overload'">
            <xsl:apply-templates select="/document/reference/elements" mode="overload" />
          </xsl:when>
          <xsl:when test="$subgroup='DerivedTypeList'">
            <xsl:apply-templates select="/document/reference/elements" mode="derivedType" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="/document/reference/elements" mode="member" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
		</xsl:choose>
    <!-- exceptions -->
    <xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:exceptions" />
		<!-- remarks -->
    <xsl:if test="not($group='namespace')">
      <xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:remarks" />
    </xsl:if>
		<!-- example -->
		<xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:codeExamples" />
    <!-- permissions -->
    <xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:permissions" />
    <!-- inheritance -->
    <xsl:apply-templates select="/document/reference/family" />
		<!-- other comment sections -->
    <xsl:if test="$subgroup='class' or $subgroup='structure'">
      <xsl:call-template name="threadSafety" />
    </xsl:if>
    <xsl:if test="not($group='list' or $group='namespace' or $group='root')">
      <!--platforms-->
      <xsl:apply-templates select="/document/reference/platforms" />
      <!--versions-->
      <xsl:apply-templates select="/document/reference/versions" />
    </xsl:if>
    <!-- see also -->
    <xsl:call-template name="seeAlsoSection"/>

  </xsl:template> 

	<xsl:template name="obsoleteSection">
    <p>
      <include item="ObsoleteBoilerPlate">
        <parameter>
          <xsl:value-of select="$subgroup"/>
        </parameter>
      </include>
      <xsl:for-each select="/document/comments/ddue:dduexml/ddue:obsoleteCodeEntity">
				<xsl:text> </xsl:text>
				<include item="nonobsoleteAlternative">
					<parameter><xsl:apply-templates select="ddue:codeEntityReference" /></parameter>
				</include>
			</xsl:for-each>
		</p>
  </xsl:template>

  <xsl:template name="internalOnly">
    <xsl:if test="/document/comments/ddue:dduexml/ddue:internalOnly or /document/reference/containers/ddue:internalOnly">
      <div id="internalonly" class="seeAlsoNoToggleSection">
        <p/>
        <include item="internalOnly" />
      </div>
    </xsl:if>
  </xsl:template>
	
	<xsl:template name="getParameterDescription">
		<xsl:param name="name" />
		<xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:parameters/ddue:parameter[string(ddue:parameterReference)=$name]/ddue:content" />
	</xsl:template>

	<xsl:template name="getReturnsDescription">
		<xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:returnValue" />
	</xsl:template>

	<xsl:template match="templates">
		<xsl:call-template name="subSection">
      <xsl:with-param name="title"><include item="templatesTitle" /></xsl:with-param>
			<xsl:with-param name="content">
				<dl>
					<xsl:for-each select="template">
						<xsl:variable name="parameterName" select="@name" />
						<dt>
							<span class="parameter"><xsl:value-of select="$parameterName"/></span>
						</dt>
						<dd>
              		<xsl:apply-templates select="/document/comments/ddue:dduexml/ddue:genericParameters/ddue:genericParameter[string(ddue:parameterReference)=$parameterName]/ddue:content" />
            </dd>
					</xsl:for-each>
				</dl>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="getElementDescription">
		<xsl:apply-templates select="ddue:summary[1]/ddue:para/node()" />
	</xsl:template>

  <xsl:template name="getInternalOnlyDescription">
    <xsl:choose>
      <xsl:when test="ddue:internalOnly">
        <include item="infraStructure" />
      </xsl:when>
      <xsl:when test="count(element) &gt; 0">
        <xsl:variable name="internal">
          <xsl:for-each select="element">
            <xsl:if test="not(ddue:internalOnly)">
              <xsl:text>no</xsl:text>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>
        <xsl:if test="not(normalize-space($internal))">
          <include item="infraStructure" />
        </xsl:if>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="getOverloadSummary">
   
  </xsl:template>

  <xsl:template name="getOverloadSections">
    
  </xsl:template>
  
  <xsl:template match="syntax">
    <xsl:if test="count(*) > 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'syntax'" />
        <xsl:with-param name="title">
          <include item="syntaxTitle"/>
        </xsl:with-param>
        <xsl:with-param name="content">
          <div id="syntaxCodeBlocks" class="code">
            <xsl:call-template name="syntaxBlocks" />
          </div>
          <!-- parameters & return value -->
          <xsl:apply-templates select="/document/reference/templates" />
          <xsl:apply-templates select="/document/reference/parameters" />
          <xsl:apply-templates select="/document/reference/returns" />
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  
  
	<!-- DDUEXML elements that behave differently in conceptual and reference -->

	<xsl:template match="ddue:exceptions">
		<xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'ddueExceptions'"/>
			<xsl:with-param name="title"><include item="exceptionsTitle" /></xsl:with-param>
			<xsl:with-param name="content">
				<xsl:choose>
					<xsl:when test="ddue:exception">
            <div class="tableSection">
            <table width="100%" cellspacing="2" cellpadding="5" frame="lhs">
							<tr>
								<th class="exceptionNameColumn"><include item="exceptionNameHeader" /></th>
								<th class="exceptionConditionColumn"><include item="exceptionConditionHeader" /></th>
							</tr>
							<xsl:for-each select="ddue:exception">
								<tr>
									<td>
                    <xsl:apply-templates select="ddue:codeEntityReference" />
                  </td>
									<td>
                    <xsl:apply-templates select="ddue:content" />
                  </td>
								</tr>
							</xsl:for-each>
						</table>
            </div>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

  <xsl:template match="ddue:permissions">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'permissions'" />
      <xsl:with-param name="title">
        <include item="permissionsTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <ul>
          <xsl:for-each select="ddue:permission">
            <li>
              <xsl:apply-templates select="ddue:codeEntityReference" />
              <xsl:text> </xsl:text>
              <xsl:apply-templates select="ddue:content" />
            </li>
          </xsl:for-each>
        </ul>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="ddue:codeExample">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template name="runningHeader">
   <include item="runningHeaderText" />
  </xsl:template>

  <xsl:template name="memberIntro">
    <xsl:if test="$subgroup='members'">
      <p>
        <xsl:apply-templates select="/document/reference/containers/ddue:summary"/>
      </p>
    </xsl:if>

    <xsl:if test="/document/reference/elements/element/memberdata[@visibility='public' or @visibility='family' or @visibility='family or assembly' or @visibility='assembly']">
      <!-- if there are exposed members, show a boilerplate intro p -->
      <xsl:variable name="introTextItemId">
        <xsl:choose>
          <xsl:when test="/document/reference/containers/type/templates">genericExposedMembersTableText</xsl:when>
          <xsl:otherwise>exposedMembersTableText</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <p>
        <include item="{$introTextItemId}">
          <parameter>
            <referenceLink target="{/document/reference/containers/type/@api}" />
          </parameter>
          <parameter>
            <xsl:value-of select="$subgroup"/><xsl:text>Subgroup</xsl:text>
          </parameter>
        </include>
      </p>
    </xsl:if>
  </xsl:template>

  <xsl:template name="mshelpCodelangAttributes">
   
    <xsl:for-each select="/document/comments/ddue:dduexml/ddue:codeExamples/ddue:codeExample/ddue:legacy/ddue:content/ddue:snippets/ddue:snippet">

      <xsl:if test="not(@language=preceding::*/@language)">
        <xsl:variable name="codeLang">
          <xsl:choose>
            <xsl:when test="@language = 'VBScript' or @language = 'vbs'">
              <xsl:text>VBScript</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'VisualBasic' or @language = 'vb' or @language = 'vb#' or @language = 'VB' or @language = 'kbLangVB'" >
              <xsl:text>kbLangVB</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'CSharp' or @language = 'c#' or @language = 'cs' or @language = 'C#'" >
              <xsl:text>CSharp</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'ManagedCPlusPlus' or @language = 'cpp' or @language = 'cpp#' or @language = 'c' or @language = 'c++' or @language = 'C++' or @language = 'kbLangCPP'" >
              <xsl:text>kbLangCPP</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'JSharp' or @language = 'j#' or @language = 'jsharp' or @language = 'VJ#'">
              <xsl:text>VJ#</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'JScript' or @language = 'js' or @language = 'jscript#' or @language = 'jscript' or @language = 'JScript' or @language = 'kbJScript'">
              <xsl:text>kbJScript</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'xml'">
              <xsl:text>xml</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'html'">
              <xsl:text>html</xsl:text>
            </xsl:when>
            <xsl:when test="@language = 'vb-c#'">
              <xsl:text>visualbasicANDcsharp</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>other</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$codeLang='other'" />
          <xsl:otherwise>
            <xsl:call-template name="codeLang">
              <xsl:with-param name="codeLang" select="$codeLang" />
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>

    </xsl:for-each>
  </xsl:template>
  

</xsl:stylesheet>
