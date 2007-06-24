<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1"
 				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >

  <xsl:import href="../../shared/transforms/utilities_reference.xsl"/>

  <xsl:output method="xml" omit-xml-declaration="yes" encoding="utf-8" />
  <!-- <xsl:output method="xml" omit-xml-declaration="yes" encoding="utf-8" doctype-public="-//W3C//DTD HTML 4.0 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd" /> -->

	<!-- key parameter is the api identifier string -->
	<xsl:param name="key" />
	<xsl:param name="metadata" value="false" />
  <xsl:param name="languages">false</xsl:param>
    
	<xsl:include href="utilities_metadata.xsl" />
  <xsl:include href="xamlSyntax.xsl"/>

  <xsl:template match="/">
		<html>
			<head>
        <META NAME="save" CONTENT="history"/>
        <title><xsl:call-template name="topicTitlePlain"/></title>
				<xsl:call-template name="insertStylesheets" />
				<xsl:call-template name="insertScripts" />
				<xsl:call-template name="insertFilename" />
				<xsl:call-template name="insertMetadata" />
			</head>
			<body>
        
       <xsl:call-template name="upperBodyStuff"/>
				<!--<xsl:call-template name="control"/>-->
				<xsl:call-template name="main"/>
			</body>
		</html>
	</xsl:template>

	<!-- useful global variables -->

  <xsl:variable name="group">
    <xsl:choose>
      <xsl:when test="/document/reference/topicdata/@group = 'api'">
        <xsl:value-of select="/document/reference/apidata/@group" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="/document/reference/topicdata/@group" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="subgroup">
    <xsl:choose>
      <xsl:when test="/document/reference/topicdata/@group = 'api'">
        <xsl:value-of select="/document/reference/apidata/@subgroup" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="/document/reference/topicdata/@subgroup" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  
  <xsl:variable name="subsubgroup">
    <xsl:choose>
      <xsl:when test="/document/reference/topicdata/@group = 'api'">
        <xsl:value-of select="/document/reference/apidata/@subsubgroup" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="/document/reference/topicdata/@subsubgroup" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  	<xsl:variable name="pseudo" select="boolean(/document/reference/topicdata[@pseudo='true'])"/>
  
   	<xsl:variable name="namespaceName">
    		<xsl:value-of select="substring-after(/document/reference/containers/namespace/@api,':')"/>
  	</xsl:variable>
  
 
	<!-- document head -->

	<xsl:template name="insertStylesheets">
		<link rel="stylesheet" type="text/css" href="../styles/presentation.css" />
		<!-- make mshelp links work -->
		<link rel="stylesheet" type="text/css" href="ms-help://Hx/HxRuntime/HxLink.css" />
    <link rel="stylesheet" type="text/css" href="ms-help://Dx/DxRuntime/DxLink.css" />
	</xsl:template>

	<xsl:template name="insertScripts">
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath"><parameter>EventUtilities.js</parameter></includeAttribute>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath"><parameter>SplitScreen.js</parameter></includeAttribute>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath"><parameter>Dropdown.js</parameter></includeAttribute>
    </script>
    <script type="text/javascript">
      <includeAttribute name="src" item="scriptPath"><parameter>script_manifold.js</parameter></includeAttribute>
    </script>
  </xsl:template>

  <xsl:template match="parameters">
    
    <xsl:call-template name="subSection">
      <xsl:with-param name="title">
        <include item="parametersTitle"/>
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:for-each select="parameter">
          <xsl:variable name="paramName" select="@name"/>
          <dl paramName="{$paramName}">
            <dt>
              <span class="parameter">
                <xsl:value-of select="$paramName"/>
              </span>
            </dt>
            <dd>
              <xsl:apply-templates select="*[1]" mode="link" />
              <br />
              <xsl:call-template name="getParameterDescription">
                <xsl:with-param name="name" select="@name" />
              </xsl:call-template>
            </dd>
          </dl>
        </xsl:for-each>
      </xsl:with-param>
    </xsl:call-template>
    
  </xsl:template>

  <xsl:template match="returns">
    <xsl:call-template name="subSection">
      <xsl:with-param name="title">
        <include>
          <xsl:attribute name="item">
            <xsl:value-of select="$subgroup" />
            <xsl:text>ValueTitle</xsl:text>
          </xsl:attribute>
        </include>
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:apply-templates select="*[1]" mode="link">
          <xsl:with-param name="qualified" select="true()" />
        </xsl:apply-templates>
        <br />
        <xsl:call-template name="getReturnsDescription" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
	<xsl:template match="element" mode="root">
		<tr>
			<td>
        <xsl:choose>
          <xsl:when test="apidata/@name = ''">
            <referenceLink target="{@api}" qualified="false">
              <include item="defaultNamespace" />
            </referenceLink>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="createReferenceLink">
              <xsl:with-param name="id" select="@api" />
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </td>
			<td>
        <xsl:call-template name="getElementDescription" />
      </td>
		</tr>
	</xsl:template>

  <xsl:template match="element" mode="namespace">
    <tr>
      <xsl:attribute name="data">
        <xsl:value-of select="apidata/@subgroup" />
        <xsl:text>; public</xsl:text>
      </xsl:attribute>
      <td>
        <xsl:call-template name="typeIcon" />
      </td>
      <td>
        <xsl:call-template name="createReferenceLink">
          <xsl:with-param name="id" select="@api" />
        </xsl:call-template>
      </td>
      <td>
        <xsl:call-template name="getInternalOnlyDescription" />
        <xsl:if test="attributes/attribute/type[@api='T:System.ObsoleteAttribute']">
          <xsl:text> </xsl:text>
          <include item="obsoleteRed" />
        </xsl:if>
        <xsl:if test="attributes/attribute/type[@api='T:System.Security.Permissions.HostProtectionAttribute']">
          <xsl:text> </xsl:text>
          <include item="hostProtectionAttributeShort" />
        </xsl:if>
        <xsl:call-template name="getElementDescription" />
      </td>
    </tr>
  </xsl:template>

	<xsl:template match="element" mode="enumeration">
    <xsl:variable name="supportedOnXna">
      <xsl:call-template name="IsMemberSupportedOnXna"/>
    </xsl:variable>
    <xsl:variable name="supportedOnCf">
      <xsl:call-template name="IsMemberSupportedOnCf"/>
    </xsl:variable>
    <tr>
      <td>
        <!-- platform icons -->
        <xsl:if test="normalize-space($supportedOnCf)!=''">
          <img>
            <includeAttribute name="src" item="iconPath">
              <parameter>CFW.gif</parameter>
            </includeAttribute>
            <includeAttribute name="alt" item="CompactFrameworkAltText" />
          </img>
        </xsl:if>

        <xsl:if test="normalize-space($supportedOnXna)!=''">
          <img>
            <includeAttribute name="src" item="iconPath">
              <parameter>xna.gif</parameter>
            </includeAttribute>
            <includeAttribute name="alt" item="XNAFrameworkAltText" />
          </img>
        </xsl:if>
      </td>
			<td>
        <span class="referenceNoLink"><xsl:value-of select="apidata/@name"/></span>
			</td>
			<td>
          <xsl:call-template name="getElementDescription" />
      </td>
		</tr>
	</xsl:template>

  <xsl:template match="element" mode="derivedType">
    <tr>
      <td>
        <xsl:choose>
          <xsl:when test="@display-api">
            <referenceLink target="{@api}" display-target="{@display-api}" />
          </xsl:when>
          <xsl:otherwise>
            <referenceLink target="{@api}" />
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
       
        <xsl:call-template name="getInternalOnlyDescription" />
        <xsl:if test="attributes/attribute/type[@api='T:System.ObsoleteAttribute']">
          <xsl:text> </xsl:text>
          <include item="obsoleteRed" />
        </xsl:if>
        <xsl:if test="attributes/attribute/type[@api='T:System.Security.Permissions.HostProtectionAttribute']">
          <xsl:text> </xsl:text>
          <include item="hostProtectionAttributeShort" />
        </xsl:if>
        <xsl:call-template name="getElementDescription" />
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="element" mode="overload">
    <xsl:variable name="supportedOnXna">
      <xsl:call-template name="IsMemberSupportedOnXna"/>
    </xsl:variable>
    <xsl:variable name="supportedOnCf">
      <xsl:call-template name="IsMemberSupportedOnCf"/>
    </xsl:variable>
    <xsl:variable name="staticMember">
      <xsl:call-template name="IsMemberStatic"/>
    </xsl:variable>
    <xsl:variable name="inheritedMember">
      <xsl:call-template name="IsMemberInherited"/>
    </xsl:variable>
    <xsl:variable name="protectedMember">
      <xsl:call-template name="IsMemberProtected"/>
    </xsl:variable>

    <tr>
      <xsl:if test="normalize-space($inheritedMember)!=''">
        <xsl:attribute name="name">inheritedMember</xsl:attribute>
      </xsl:if>
      <xsl:if test="normalize-space($protectedMember)!=''">
        <xsl:attribute name="protected">true</xsl:attribute>
      </xsl:if>
      <xsl:if test="normalize-space($supportedOnXna)=''">
        <xsl:attribute name="notSupportedOnXna">true</xsl:attribute>
      </xsl:if>
      <xsl:if test="normalize-space($supportedOnCf)=''">
        <xsl:attribute name="notSupportedOn">netcf</xsl:attribute>
      </xsl:if>

      <xsl:attribute name="data">
        <xsl:value-of select="apidata/@subgroup" />
        <xsl:choose>
          <xsl:when test="memberdata/@visibility='public'">
            <xsl:text>; public</xsl:text>
          </xsl:when>
          <xsl:when test="memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly']">
            <xsl:text>; protected</xsl:text>
          </xsl:when>
          <!-- NOTE: EII members (private-virtual) fall through to this xsl:otherwise block -->
          <xsl:otherwise>
            <xsl:text>; public</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="memberdata/@static = 'true'">
            <xsl:text>; static</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>; instance</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="normalize-space($inheritedMember)=''">
            <xsl:text>; declared</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>; inherited</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <td>
        <!-- item icons -->
        <xsl:call-template name="memberIcons">
          <xsl:with-param name="memberVisibility">
            <xsl:choose>
              <xsl:when test="memberdata/@visibility='family' or memberdata/@visibility='family or assembly' or memberdata/@visibility='assembly'">prot</xsl:when>
              <xsl:when test="memberdata/@visibility='private'">priv</xsl:when>
              <xsl:otherwise>pub</xsl:otherwise>
            </xsl:choose>
          </xsl:with-param>
          <xsl:with-param name="staticMember" select="normalize-space($staticMember)" />
          <xsl:with-param name="supportedOnXna" select="normalize-space($supportedOnXna)"/>
          <xsl:with-param name="supportedOnCf" select="normalize-space($supportedOnCf)"/>
        </xsl:call-template>
      </td>
      <td>
        <!-- item name -->
        <xsl:choose>
          <xsl:when test="@display-api">
            <referenceLink target="{@api}" display-target="{@display-api}" />
          </xsl:when>
          <xsl:otherwise>
            <referenceLink target="{@api}" />
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <!-- item description -->
        <xsl:call-template name="getInternalOnlyDescription" />
        <xsl:if test="attributes/attribute/type[@api='T:System.ObsoleteAttribute']">
          <xsl:text> </xsl:text>
          <include item="obsoleteRed" />
        </xsl:if>
        <xsl:if test="attributes/attribute/type[@api='T:System.Security.Permissions.HostProtectionAttribute']">
          <xsl:text> </xsl:text>
          <include item="hostProtectionAttributeShort" />
        </xsl:if>
        <xsl:call-template name="getElementDescription" />
        <xsl:choose>
					<xsl:when test="normalize-space($inheritedMember)!=''">
            <xsl:text> </xsl:text>
            <include item="inheritedFrom">
              <parameter>
                <xsl:apply-templates select="containers/type" mode="link" />
              </parameter>
            </include>
          </xsl:when>
          <xsl:when test="overrides">
            <xsl:text> </xsl:text>
            <include item="overridesMember">
              <parameter>
                <xsl:apply-templates select="overrides/member" mode="link" />
              </parameter>
            </include>
          </xsl:when>
        </xsl:choose>

        <xsl:if test="normalize-space($supportedOnCf)!=''">
          <p/><include item="supportedByNetCF" />
        </xsl:if>
        <xsl:if test="normalize-space($supportedOnXna)!=''">
          <p/><include item="supportedByXNA" />
        </xsl:if>
             
      </td>
    </tr>
  </xsl:template>

	<xsl:template name="insertFilename">
		<meta name="guid">
			<xsl:attribute name="content">
				<xsl:value-of select="/document/reference/file/@name" />
			</xsl:attribute>
		</meta>
	</xsl:template>

	<!-- writing templates -->

	<xsl:template name="csTemplates">
		<xsl:param name="seperator" select="string(',')" />
		<xsl:text>&lt;</xsl:text>
		<xsl:for-each select="template">
			<xsl:value-of select="@name" />
			<xsl:if test="not(position()=last())">
				<xsl:value-of select="$seperator" />
			</xsl:if>
		</xsl:for-each>
		<xsl:text>&gt;</xsl:text>
	</xsl:template>

	<xsl:template name="csTemplatesInIndex" >
		<xsl:text>%3C</xsl:text>
		<xsl:for-each select="template">
			<xsl:value-of select="@name" />
			<xsl:if test="not(position()=last())">
				<xsl:text>%2C </xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:text>%3E</xsl:text>
	</xsl:template>

	<xsl:template name="vbTemplates">
		<xsl:param name="seperator" select="string(',')" />
		<xsl:text>(Of </xsl:text>
		<xsl:for-each select="template">
			<xsl:value-of select="@name" />
			<xsl:if test="not(position()=last())">
				<xsl:value-of select="$seperator" />
			</xsl:if>
		</xsl:for-each>
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template name="typeTitle">
		<xsl:if test="containers/container[@type]">
			<xsl:for-each select="containers/container[@type]">
				<xsl:call-template name="typeTitle" />
			</xsl:for-each>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="apidata/@name" />
		<xsl:if test="count(templates/template) > 0">
			<xsl:for-each select="templates"><xsl:call-template name="csTemplates" /></xsl:for-each>
		</xsl:if> 
	</xsl:template>

	<!-- document body -->

	<!-- control window -->

	<xsl:template name="control">
		<div id="control">
			<span class="topicTitle"><xsl:call-template name="topicTitleDecorated" /></span><br/>
		</div>
	</xsl:template>

	<!-- Title in topic -->

  <!-- Title in topic -->

  <xsl:template name="topicTitlePlain">
    <xsl:param name="qualifyMembers" select="false()" />
    <include>
      <xsl:attribute name="item">
        <xsl:if test="boolean(/document/reference/templates) and not($group='list')">
          <xsl:text>generic_</xsl:text>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="string($subsubgroup)">
            <xsl:value-of select="$subsubgroup" />
          </xsl:when>
          <xsl:when test="string($subgroup)">
            <xsl:choose>
              <xsl:when test="$subgroup='overload'">
                <xsl:value-of select="/document/reference/apidata/@subgroup"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$subgroup" />
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$group" />
          </xsl:otherwise>
        </xsl:choose>
        <xsl:text>TopicTitle</xsl:text>
      </xsl:attribute>
      <parameter>
        <xsl:call-template name="shortNamePlain">
          <xsl:with-param name="qualifyMembers" select="$qualifyMembers" />
        </xsl:call-template>
      </parameter>
      <parameter>
        <xsl:if test="document/reference/memberdata/@overload" >
          <xsl:for-each select="/document/reference">
            <xsl:call-template name="parameterTypesPlain" />
          </xsl:for-each>
        </xsl:if>
      </parameter>
    </include>
  </xsl:template>

  <xsl:template name="topicTitleDecorated">
    <xsl:param name="titleType" />
    <include>
      <xsl:attribute name="item">
        <xsl:choose>
          <xsl:when test="$titleType = 'tocTitle' and $group='namespace'">
            <xsl:text>tocTitle</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="string($subsubgroup)">
                <xsl:value-of select="$subsubgroup" />
              </xsl:when>
              <xsl:when test="string($subgroup)">
                <xsl:choose>
                  <xsl:when test="$subgroup='overload'">
                    <xsl:value-of select="/document/reference/apidata/@subgroup" />
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$subgroup" />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$group" />
              </xsl:otherwise>
            </xsl:choose>
            <xsl:text>TopicTitle</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <parameter>
        <xsl:call-template name="shortNameDecorated">
          <xsl:with-param name="titleType" select="$titleType" />
        </xsl:call-template>
      </parameter>
      <parameter>
        <xsl:if test="document/reference/memberdata/@overload" >
          <xsl:for-each select="/document/reference">
            <xsl:call-template name="parameterTypesDecorated" />
          </xsl:for-each>
        </xsl:if>
      </parameter>
    </include>
  </xsl:template>

	
	<!-- Title in TOC -->

	<!-- Index entry -->
	
	<!-- main window -->

  <xsl:template name="main">
    <div id="mainSection">

      <div id="mainBody">
        <div id="allHistory" class="saveHistory" onsave="saveAll()" onload="loadAll()"/>
        <xsl:call-template name="head" />
        <xsl:call-template name="body" />
        <xsl:call-template name="foot" />
      </div>
    </div>
    
  </xsl:template>

	<xsl:template name="head">
		<include item="header" />
	</xsl:template>

  <xsl:template name="syntaxBlocks">

    <xsl:for-each select="/document/syntax/div[@codeLanguage]">
      <xsl:choose>
        <xsl:when test="@codeLanguage='VisualBasic'">
          <xsl:call-template name="languageSyntaxBlock">
            <xsl:with-param name="language">VisualBasicDeclaration</xsl:with-param>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="@codeLanguage='JSharp'">
          <xsl:if test="not(/document/reference/versions) or boolean(/document/reference/versions/versions[@name='netfw']/version[not(@name='netfw35')])">
            <xsl:call-template name="languageSyntaxBlock" />
          </xsl:if>
        </xsl:when>
        <xsl:when test="@codeLanguage='XAML'">
          <xsl:call-template name="XamlSyntaxBlock"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="languageSyntaxBlock" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="languageSyntaxBlock">
    <xsl:param name="language" select="@codeLanguage"/>
    <span codeLanguage="{$language}">
      <table>
        <tr>
          <th>
            <include item="{$language}" />
          </th>
        </tr>
        <tr>
          <td>
            <pre xml:space="preserve"><xsl:text/><xsl:copy-of select="node()"/><xsl:text/></pre>
          </td>
        </tr>
      </table>
    </span>
  </xsl:template>

	<xsl:template match="elements" mode="root">
		<xsl:if test="count(element) > 0">
           
			<xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'namespaces'"/>
				<xsl:with-param name="title"><include item="namespacesTitle" /></xsl:with-param>
				<xsl:with-param name="content">
				<table class="members" id="memberList" frame="lhs">
					<tr>
						<th class="nameColumn"><include item="namespaceNameHeader"/></th>
						<th class="descriptionColumn"><include item="namespaceDescriptionHeader" /></th>
					</tr>
					<xsl:apply-templates select="element" mode="root">
						<xsl:sort select="apidata/@name" />
					</xsl:apply-templates>
				</table>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

  <xsl:template name="namespaceSection">
    <xsl:param name="listSubgroup" />
    <xsl:variable name="header" select="concat($listSubgroup, 'TypesFilterLabel')"/>
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="$listSubgroup"/>
      <xsl:with-param name="title">
        <include item="{$header}" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:call-template name="namespaceList">
          <xsl:with-param name="listSubgroup" select="$listSubgroup" />
        </xsl:call-template>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="elements" mode="namespace">
   
    <xsl:if test="element/apidata/@subgroup = 'class'">
      <xsl:call-template name="namespaceSection">
        <xsl:with-param name="listSubgroup" select="'class'" />
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="element/apidata/@subgroup = 'structure'">
      <xsl:call-template name="namespaceSection">
        <xsl:with-param name="listSubgroup" select="'structure'" />
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="element/apidata/@subgroup = 'interface'">
      <xsl:call-template name="namespaceSection">
        <xsl:with-param name="listSubgroup" select="'interface'" />
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="element/apidata/@subgroup = 'delegate'">
      <xsl:call-template name="namespaceSection">
        <xsl:with-param name="listSubgroup" select="'delegate'" />
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="element/apidata/@subgroup = 'enumeration'">
      <xsl:call-template name="namespaceSection">
        <xsl:with-param name="listSubgroup" select="'enumeration'" />
      </xsl:call-template>
    </xsl:if>

  </xsl:template>

  <xsl:template name="namespaceList">
    <xsl:param name="listSubgroup" />

    <table id="typeList" class="members" frame="lhs">
      <tr>
        <th class="iconColumn">
          &#160;
       </th>
        <th class="nameColumn">
          <include item="{$listSubgroup}NameHeader"/>
        </th>
        <th class="descriptionColumn">
          <include item="typeDescriptionHeader" />
        </th>
      </tr>
      <xsl:apply-templates select="element[apidata/@subgroup=$listSubgroup]" mode="namespace">
        <xsl:sort select="@api" />
      </xsl:apply-templates>
    </table>
    
  </xsl:template>

  <xsl:template match="elements" mode="enumeration">
    <xsl:if test="count(element) > 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'members'"/>
        <xsl:with-param name="title">
          <include item="enumMembersTitle" />
        </xsl:with-param>
        <xsl:with-param name="content">
          <table class="members" id="memberList" frame="lhs">
            <tr>
              <th class="iconColumn"></th>
              <th class="nameColumn">
                <include item="memberNameHeader"/>
              </th>
              <th class="descriptionColumn">
                <include item="memberDescriptionHeader" />
              </th>
            </tr>
            <xsl:apply-templates select="element" mode="enumeration"/>
          </table>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="element" mode="members">
    <xsl:param name="subgroup"/>
      <xsl:if test="memberdata[@visibility='public'] and apidata[@subgroup=$subgroup]">
          public;
      </xsl:if>
      <xsl:if test="memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly'] and apidata[@subgroup=$subgroup]">
        protected;
      </xsl:if>
      <xsl:if test="memberdata[@visibility='private'] and apidata[@subgroup=$subgroup] and not(proceduredata[@virtual = 'true'])">
        private;
      </xsl:if>
      <xsl:if test="memberdata[@visibility='private'] and proceduredata[@virtual = 'true']">
        explicit;
      </xsl:if>
  </xsl:template>

  <xsl:template match="elements" mode="member">

    <xsl:call-template name="memberIntro" />

    <xsl:if test="element/apidata[@subgroup='constructor']">
      <xsl:call-template name="memberlistSectionGroup">
        <xsl:with-param name="listSubgroup" select="'constructor'" />
        <xsl:with-param name="members" select="element[apidata[@subgroup='constructor']]" />
      </xsl:call-template>
    </xsl:if>
   
    <!-- method subgroup includes operators -->
    <xsl:if test="element/apidata[@subgroup='method']">
      <xsl:call-template name="memberlistSectionGroup">
        <xsl:with-param name="listSubgroup">method</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subgroup='method']]" />
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="element/apidata[@subgroup='field']">
      <xsl:call-template name="memberlistSectionGroup">
        <xsl:with-param name="listSubgroup">field</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subgroup='field']]" />
      </xsl:call-template>
    </xsl:if>
       
    <xsl:if test="element/apidata[@subgroup='property' and not(@subsubgroup)]">
      <xsl:call-template name="memberlistSectionGroup">
        <xsl:with-param name="listSubgroup">property</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subgroup='property'] and not(@subsubgroup)]" />
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="element/apidata[@subsubgroup='attachedProperty']">
      <xsl:call-template name="memberlistSection">
        <xsl:with-param name="listSubsubgroup">attachedProperty</xsl:with-param>
        <xsl:with-param name="listVisibility">public</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subsubgroup='attachedProperty']]" />
      </xsl:call-template>
    </xsl:if>
       
    <xsl:if test="element/apidata[@subgroup='event' and not(@subsubgroup)]">
      <xsl:call-template name="memberlistSectionGroup">
        <xsl:with-param name="listSubgroup">event</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subgroup='event'] and not(@subsubgroup)]" />
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="element/apidata[@subsubgroup='attachedEvent']">
      <xsl:call-template name="memberlistSection">
        <xsl:with-param name="listSubsubgroup">attachedEvent</xsl:with-param>
        <xsl:with-param name="listVisibility">public</xsl:with-param>
        <xsl:with-param name="members" select="element[apidata[@subsubgroup='attachedEvent']]" />
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="element[memberdata[@visibility='private'] and proceduredata[@virtual = 'true']]">
      <xsl:call-template name="memberlistSection">
        <xsl:with-param name="explicit">true</xsl:with-param>
        <xsl:with-param name="listVisibility">private</xsl:with-param>
        <xsl:with-param name="members" select="element[memberdata[@visibility='private'] and proceduredata[@virtual = 'true']]" />
      </xsl:call-template>
    </xsl:if>

  </xsl:template>

  <xsl:template name="memberlistSectionGroup">
    <xsl:param name="members"/>
    <xsl:param name="listSubgroup" />

    <xsl:variable name="linkToProtectedSection">
      <xsl:if test="$members//memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly']">true</xsl:if>
    </xsl:variable>
    <xsl:if test="$members//memberdata[@visibility='public']">
      <xsl:call-template name="memberlistSection">
        <xsl:with-param name="listSubgroup" select="$listSubgroup" />
        <xsl:with-param name="listVisibility">public</xsl:with-param>
        <xsl:with-param name="members" select="$members[.//memberdata[@visibility='public']]" />
        <xsl:with-param name="linkToProtectedSection" select="normalize-space($linkToProtectedSection)"/>
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="$members//memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly']">
      <xsl:call-template name="memberlistSection">
        <xsl:with-param name="listSubgroup" select="$listSubgroup" />
        <xsl:with-param name="listVisibility">protected</xsl:with-param>
        <xsl:with-param name="members" select="$members[.//memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly']]" />
      </xsl:call-template>
    </xsl:if>

  </xsl:template>

  <xsl:template name="memberlistSection">
    <xsl:param name="members"/>
    <xsl:param name="listSubgroup" />
    <xsl:param name="listSubsubgroup" />
    <xsl:param name="listVisibility" />
    <xsl:param name="explicit" />
    <xsl:param name="linkToProtectedSection"/>

    <xsl:variable name="header">
      <xsl:choose>
        <xsl:when test="$explicit='true'">ExplicitInterfaceImplementation</xsl:when>
        <xsl:when test="boolean($listSubsubgroup)">
          <xsl:value-of select="concat($listVisibility, $listSubsubgroup)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat($listVisibility, $listSubgroup)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="$header" />
      <xsl:with-param name="title">
        <include item="{$header}" />
      </xsl:with-param>
      <xsl:with-param name="toplink" select="true()"/>
      <xsl:with-param name="nonToggletitle">
        <!-- add a link to the protected members section, if there are any -->
        <xsl:if test="boolean($linkToProtectedSection)">
          <include item="seeMemberGroup">
            <parameter>
              <a href="#protected{$listSubgroup}Toggle" onclick="OpenSection(protected{$listSubgroup}Toggle)">
                <include item="protected{$listSubgroup}" />
              </a>
            </parameter>
          </include>
        </xsl:if>
      </xsl:with-param>
      <xsl:with-param name="content">
        <table id="typeList" class="members" frame="lhs">
          <tr>
            <th class="iconColumn">
              &#160;
            </th>
            <th class="nameColumn">
              <include item="typeNameHeader"/>
            </th>
            <th class="descriptionColumn">
              <include item="typeDescriptionHeader" />
            </th>
          </tr>

          <!-- add a row for each member of the current subgroup-visibility -->
          <xsl:apply-templates select="$members" mode="memberlistRow">
            <xsl:sort select="apidata/@name" />
            <xsl:with-param name="listVisibility" select="$listVisibility"/>
          </xsl:apply-templates>
        </table>
      </xsl:with-param>
    </xsl:call-template>

  </xsl:template>

  <xsl:template match="elements" mode="type">
            
	</xsl:template>

  <xsl:template name="IsMemberSupportedOnXna">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberSupportedOnXna"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="platformFilterExcludesXna" select="boolean(platforms and not(platforms/platform[.='Xbox360']))" />
        <xsl:if test="boolean(not($platformFilterExcludesXna) and (@xnafw or element/@xnafw))">
          <xsl:text>supported</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="IsMemberSupportedOnCf">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberSupportedOnCf"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="platformFilterExcludesCF" select="boolean( platforms and not(platforms[platform[.='PocketPC'] or platform[.='SmartPhone'] or platform[.='WindowsCE']]) )" />
        <xsl:if test="boolean(not($platformFilterExcludesCF) and (@netcfw or element/@netcfw))">
          <xsl:text>yes</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template name="IsMemberStatic">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberStatic"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="apidata[@subsubgroup='attachedProperty' or @subsubgroup='attachedEvent']"/>
      <xsl:otherwise>
        <xsl:if test="memberdata/@static='true'">
          <xsl:text>yes</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <!-- returns a non-empty string if the element is inherited, or for overloads if any of the overloads is inherited -->
  <xsl:template name="IsMemberInherited">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberInherited"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="containers/type[@api!=$typeId]">
          <xsl:text>yes</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <!-- returns a non-empty string if the element is declared, or for overloads if any of the overloads is declared -->
  <xsl:template name="IsMemberDeclared">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberDeclared"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="containers/type[@api=$typeId]">
          <xsl:text>yes</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template name="IsMemberProtected">
    <xsl:choose>
      <xsl:when test="element and not(@signatureset)">
        <xsl:for-each select="element">
          <xsl:call-template name="IsMemberProtected"/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="memberdata[@visibility='family' or @visibility='family or assembly' or @visibility='assembly']">
          <xsl:text>yes</xsl:text>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="element" mode="memberlistRow">
    <xsl:param name="listVisibility"/>
    <xsl:variable name="supportedOnXna">
      <xsl:call-template name="IsMemberSupportedOnXna"/>
    </xsl:variable>
    <xsl:variable name="supportedOnCf">
      <xsl:call-template name="IsMemberSupportedOnCf"/>
    </xsl:variable>
    <xsl:variable name="staticMember">
      <xsl:call-template name="IsMemberStatic"/>
    </xsl:variable>
    <xsl:variable name="inheritedMember">
      <xsl:call-template name="IsMemberInherited"/>
    </xsl:variable>
    <xsl:variable name="declaredMember">
      <xsl:call-template name="IsMemberDeclared"/>
    </xsl:variable>
    <xsl:variable name="protectedMember">
      <xsl:call-template name="IsMemberProtected"/>
    </xsl:variable>
    <!-- do not show non-static members of static types -->
    <xsl:if test=".//memberdata/@static='true' or not(/document/reference/typedata[@abstract='true' and @sealed='true'])">
      <tr>
        <xsl:if test="normalize-space($declaredMember)=''">
          <xsl:attribute name="name">inheritedMember</xsl:attribute>
        </xsl:if>
        <xsl:if test="normalize-space($protectedMember)!=''">
          <xsl:attribute name="protected">true</xsl:attribute>
        </xsl:if>
        <xsl:if test="normalize-space($supportedOnXna)=''">
          <xsl:attribute name="notSupportedOnXna">true</xsl:attribute>
        </xsl:if>
        <xsl:if test="normalize-space($supportedOnCf)=''">
          <xsl:attribute name="notSupportedOn">netcf</xsl:attribute>
        </xsl:if>

        <td>
          <xsl:call-template name="memberIcons">
            <xsl:with-param name="memberVisibility">
              <xsl:choose>
                <xsl:when test="$listVisibility='public'">pub</xsl:when>
                <xsl:when test="$listVisibility='private'">priv</xsl:when>
                <xsl:when test="$listVisibility='protected'">prot</xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="memberdata/@visibility='family' or memberdata/@visibility='family or assembly' or memberdata/@visibility='assembly'">prot</xsl:when>
                    <xsl:when test="memberdata/@visibility='private'">priv</xsl:when>
                    <xsl:otherwise>pub</xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
            <xsl:with-param name="staticMember" select="normalize-space($staticMember)" />
            <xsl:with-param name="supportedOnXna" select="normalize-space($supportedOnXna)"/>
            <xsl:with-param name="supportedOnCf" select="normalize-space($supportedOnCf)"/>
          </xsl:call-template>
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="@display-api">
              <referenceLink target="{@api}" display-target="{@display-api}" show-parameters="false" />
            </xsl:when>
            <xsl:otherwise>
              <referenceLink target="{@api}" show-parameters="false" />
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td>
            <xsl:call-template name="getInternalOnlyDescription" />
          <xsl:if test="attributes/attribute/type[@api='T:System.ObsoleteAttribute']">
            <xsl:text> </xsl:text>
            <include item="obsoleteRed" />
          </xsl:if>
          <xsl:if test="attributes/attribute/type[@api='T:System.Security.Permissions.HostProtectionAttribute']">
            <xsl:text> </xsl:text>
            <include item="hostProtectionAttributeShort" />
          </xsl:if>
          <xsl:if test="topicdata[@subgroup='overload']">
            <include item="Overloaded"/>
            <xsl:text> </xsl:text>
          </xsl:if>
          <xsl:apply-templates select="element" mode="overloadSummary" />
          <xsl:call-template name="getElementDescription" />
          <xsl:choose>
            <xsl:when test="@signatureset">
              <!-- TODO add boilerplate for other members in the sig set -->
            </xsl:when>
            <xsl:when test="not(topicdata[@subgroup='overload'])">
              <xsl:choose>
                <xsl:when test="normalize-space($inheritedMember)!=''">
                  <xsl:text> </xsl:text>
                  <include item="inheritedFrom">
                    <parameter>
                      <xsl:apply-templates select="containers/type" mode="link" />
                    </parameter>
                  </include>
                </xsl:when>
                <xsl:when test="overrides/member">
                  <xsl:text> </xsl:text>
                  <include item="overridesMember">
                    <parameter>
                      <xsl:apply-templates select="overrides/member" mode="link" />
                    </parameter>
                  </include>
                </xsl:when>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>

  <xsl:template match="elements" mode="derivedType">
    <xsl:if test="count(element) > 0">
      <xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'DerivedClasses'"/>
        <xsl:with-param name="title">
          <include item="derivedClasses" />
        </xsl:with-param>
        <xsl:with-param name="content">
          <table class="members" id="memberList" frame="lhs">
            <tr>
              <th class="nameColumn">
                <include item="memberNameHeader"/>
              </th>
              <th class="descriptionColumn">
                <include item="memberDescriptionHeader" />
              </th>
            </tr>
            <xsl:apply-templates select="element" mode="derivedType">
              <xsl:sort select="apidata/@name" />
            </xsl:apply-templates>
          </table>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

	<xsl:template match="elements" mode="overload">
   <xsl:if test="count(element) > 0">
			<xsl:call-template name="section">
        <xsl:with-param name="toggleSwitch" select="'overloadMembers'"/>
				<xsl:with-param name="title"><include item="membersTitle" /></xsl:with-param>
				<xsl:with-param name="content">
				<table class="members" id="memberList" frame="lhs">
					<tr>
            <th class="iconColumn"></th>
						<th class="nameColumn"><include item="typeNameHeader"/></th>
						<th class="descriptionColumn"><include item="typeDescriptionHeader" /></th>
					</tr>
					<xsl:apply-templates select="element" mode="overload">
						<xsl:sort select="apidata/@name" />
					</xsl:apply-templates>
				</table>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
    <xsl:apply-templates select="element" mode="overloadSections">
      <xsl:sort select="apidata/@name" />
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="elements" mode="overloadSummary">
    <xsl:apply-templates select="element" mode="overloadSummary" >
      <xsl:sort select="apidata/@name"/>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="element" mode="overloadSummary">
    <xsl:call-template name="getOverloadSummary" />
  </xsl:template>

  <xsl:template match="element" mode="overloadSections">
    <xsl:call-template name="getOverloadSections" />
  </xsl:template>

	<xsl:template name="typeIcon">
    <xsl:choose>
      <xsl:when test="apidata/@subgroup='class'">
        <img>
          <includeAttribute name="src" item="iconPath">
            <parameter>pubclass.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="publicClassAltText" />
        </img>
      </xsl:when>
      <xsl:when test="apidata/@subgroup='structure'">
        <img>
          <includeAttribute name="src" item="iconPath">
            <parameter>pubstructure.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="publicStructureAltText" />
        </img>
      </xsl:when>
      <xsl:when test="apidata/@subgroup='interface'">
        <img>
          <includeAttribute name="src" item="iconPath">
            <parameter>pubinterface.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="publicInterfaceAltText" />
        </img>
      </xsl:when>
      <xsl:when test="apidata/@subgroup='delegate'">
        <img>
          <includeAttribute name="src" item="iconPath">
            <parameter>pubdelegate.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="publicDelegateAltText" />
        </img>
      </xsl:when>
      <xsl:when test="apidata/@subgroup='enumeration'">
        <img>
          <includeAttribute name="src" item="iconPath">
            <parameter>pubenum.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="publicEnumerationAltText" />
        </img>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="memberIcons">
    <xsl:param name="memberVisibility" />
    <xsl:param name="staticMember" />
    <xsl:param name="supportedOnXna"/>
    <xsl:param name="supportedOnCf"/>
    
    <xsl:variable name="memberSubgroup">
      <xsl:choose>
        <xsl:when test="apidata/@subgroup='constructor'">
          <xsl:text>method</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="apidata/@subgroup" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- test for explicit interface implementations, which get the interface icon -->
    <xsl:if test="memberdata/@visibility='private' and proceduredata/@virtual='true'">
      <img>
        <includeAttribute name="src" item="iconPath">
          <parameter>pubinterface.gif</parameter>
        </includeAttribute>
        <includeAttribute name="alt" item="ExplicitInterfaceAltText" />
      </img>
    </xsl:if>
    
    <img>
      <includeAttribute name="src" item="iconPath">
        <parameter>
          <xsl:value-of select="concat($memberVisibility,$memberSubgroup,'.gif')" />
        </parameter>
      </includeAttribute>
      <xsl:choose>
        <xsl:when test="apidata/@subsubgroup">
          <includeAttribute name="alt" item="{concat($memberVisibility,apidata/@subsubgroup,'AltText')}" />
        </xsl:when>
        <xsl:otherwise>
          <includeAttribute name="alt" item="{concat($memberVisibility,$memberSubgroup,'AltText')}" />
        </xsl:otherwise>
      </xsl:choose>
    </img>

    <xsl:if test="$staticMember!=''">
      <img>
        <includeAttribute name="src" item="iconPath">
          <parameter>static.gif</parameter>
        </includeAttribute>
        <includeAttribute name="alt" item="staticAltText" />
      </img>
    </xsl:if>
    
    <xsl:if test="$supportedOnCf!=''">
      <img>
        <includeAttribute name="src" item="iconPath">
          <parameter>CFW.gif</parameter>
        </includeAttribute>
        <includeAttribute name="alt" item="CompactFrameworkAltText" />
      </img>
    </xsl:if>
    
    <xsl:if test="$supportedOnXna!=''">
      <img>
        <includeAttribute name="src" item="iconPath">
          <parameter>xna.gif</parameter>
        </includeAttribute>
        <includeAttribute name="alt" item="XNAFrameworkAltText" />
      </img>
    </xsl:if>
  </xsl:template>

	<!-- Footer stuff -->
	
	<xsl:template name="foot">
    <div id="footer">
      <div class="footerLine">
        <img width="100%" height="3px">
          <includeAttribute name="src" item="iconPath">
            <parameter>footer.gif</parameter>
          </includeAttribute>
          <includeAttribute name="alt" item="footerImage" />
        </img>
      </div>

      <include item="footer">
        <parameter>
          <xsl:value-of select="$key"/>
        </parameter>
        <parameter>
          <xsl:call-template name="topicTitlePlain"/>
        </parameter>
      </include>
    </div>
	</xsl:template>

	<!-- Assembly information -->

	<xsl:template name="requirementsInfo">
    <p/>
    <include item="requirementsNamespaceLayout">
      <parameter>
        <xsl:value-of select="$namespaceName"/>
      </parameter>
    </include>
    <br/>
    <xsl:call-template name="assembliesInfo"/>

    <!-- some apis display a XAML xmlns uri -->
    <xsl:call-template name="xamlXmlnsInfo"/>
  </xsl:template>

  <xsl:template name="assemblyNameAndModule">
    <xsl:param name="library" select="/document/reference/containers/library"/>
    <include item="assemblyNameAndModule">
      <parameter>
        <xsl:value-of select="$library/@assembly"/>
      </parameter>
      <parameter>
        <xsl:value-of select="$library/@module"/>
      </parameter>
    </include>
  </xsl:template>

  <xsl:template name="assembliesInfo">
    <xsl:choose>
      <xsl:when test="count(/document/reference/containers/library)&gt;1">
        <include item="requirementsAssembliesLabel"/>
        <xsl:for-each select="/document/reference/containers/library">
          <xsl:text>&#xa0;&#xa0;</xsl:text>
          <xsl:call-template name="assemblyNameAndModule">
            <xsl:with-param name="library" select="."/>
          </xsl:call-template>
          <br/>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <include item="requirementsAssemblyLabel"/>
        <xsl:text>&#xa0;</xsl:text>
        <xsl:call-template name="assemblyNameAndModule"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Platform information -->

  <xsl:template match="platforms">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'platformsTitle'"/>
      <xsl:with-param name="title">
        <include item="platformsTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <p>
          <xsl:for-each select="platform">
            <include item="{.}" /><xsl:if test="position()!=last()"><xsl:text>, </xsl:text></xsl:if>
          </xsl:for-each>
        </p>
        <p>
          <include item="developmentPlatformsLayout"/>
        </p>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <!-- Version information -->

  <xsl:template match="versions">
    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'versionsTitle'"/>
      <xsl:with-param name="title">
        <include item="versionsTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:call-template name="processVersions" />
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="processVersions">
    <xsl:choose>
      <xsl:when test="versions">
        <xsl:for-each select="versions">
          <!-- $platformFilterExcluded is based on platform filtering information -->
          <xsl:variable name="platformFilterExcluded" select="boolean(/document/reference/platforms and ( (@name='netcfw' and not(/document/reference/platforms/platform[.='PocketPC']) and not(/document/reference/platforms/platform[.='SmartPhone']) and not(/document/reference/platforms/platform[.='WindowsCE']) ) or (@name='xnafw' and not(/document/reference/platforms/platform[.='Xbox360']) ) ) )" />
          <xsl:if test="not($platformFilterExcluded) and count(version) &gt; 0">
            <h4 class ="subHeading">
              <include item="{@name}" />
            </h4>
            <xsl:call-template name="processVersions" />
          </xsl:if>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <!-- show the versions in which the api is supported, if any -->
        <xsl:variable name="supportedCount" select="count(version[not(@obsolete)])"/>
        <xsl:if test="$supportedCount &gt; 0">
          <include item="supportedIn_{$supportedCount}">
            <xsl:for-each select="version[not(@obsolete)]">
              <parameter>
                <include item="{@name}" />
              </parameter>
            </xsl:for-each>
          </include>
          <br/>
        </xsl:if>
        <!-- show the versions in which the api is obsolete with a compiler warning, if any -->
        <xsl:for-each select="version[@obsolete='warning']">
          <include item="obsoleteWarning">
            <parameter>
              <include item="{@name}" />
            </parameter>
          </include>
          <br/>
        </xsl:for-each>
        <!-- show the versions in which the api is obsolete and does not compile, if any -->
        <xsl:for-each select="version[@obsolete='error']">
          <xsl:if test="position()=last()">
            <include item="obsoleteError">
              <parameter>
                <include item="{@name}" />
              </parameter>
            </include>
            <br/>
          </xsl:if>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Inheritance hierarchy -->

  <xsl:template match="family">

    <xsl:call-template name="section">
      <xsl:with-param name="toggleSwitch" select="'family'"/>
      <xsl:with-param name="title">
        <include item="familyTitle" />
      </xsl:with-param>
      <xsl:with-param name="content">
        <xsl:variable name="ancestorCount" select="count(ancestors/*)" />
        <xsl:variable name="childCount" select="count(descendents/*)" />
        
        <xsl:for-each select="ancestors/type">
          <xsl:sort select="position()" data-type="number" order="descending" />

          <xsl:call-template name="indent">
            <xsl:with-param name="count" select="position()" />
          </xsl:call-template>

          <xsl:apply-templates select="self::type" mode="link">
            <xsl:with-param name="qualified" select="true()" />
          </xsl:apply-templates>
 
          <br/>
        </xsl:for-each>

        <xsl:call-template name="indent">
          <xsl:with-param name="count" select="$ancestorCount + 1" />
        </xsl:call-template>
       
        <referenceLink target="{$key}" qualified="true"/>
        <br/>
        
        <xsl:choose>

          <xsl:when test="descendents/@derivedTypes">
            <xsl:call-template name="indent">
              <xsl:with-param name="count" select="$ancestorCount + 2" />
            </xsl:call-template>
            <referenceLink target="{descendents/@derivedTypes}" qualified="true">
              <include item="derivedClasses"/>
            </referenceLink>
          </xsl:when>
          <xsl:otherwise>
            
            <xsl:for-each select="descendents/type">
              <xsl:call-template name="indent">
                <xsl:with-param name="count" select="$ancestorCount + 2" />
              </xsl:call-template>

              <xsl:apply-templates select="self::type" mode="link">
                <xsl:with-param name="qualified" select="true()" />
              </xsl:apply-templates>

              <br/>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template name="createTableEntries">
    <xsl:param name="count" />
    <xsl:if test="number($count) > 0">
      <td>&#x20;</td>
      <xsl:call-template name="createTableEntries">
        <xsl:with-param name="count" select="number($count)-1" />
      </xsl:call-template>
		</xsl:if>
	</xsl:template>


	<!-- Link to create type -->
  <!-- Don't use this! Use the mode="link" templates. -->
<!--
  <xsl:template name="typeReferenceLink">
    <xsl:param name="api" />
    <xsl:param name="qualified" />
    <xsl:param name="specialization" />
    
   <referenceLink target="{$api}" qualified="{$qualified}">
      <xsl:choose>
        <xsl:when test="$specialization = 'true'">
          <xsl:attribute name="show-templates">false</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="show-templates">true</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </referenceLink>  
  </xsl:template>
-->
	<!-- Naming -->

	<xsl:template name="shortName">
		<xsl:choose>
			<xsl:when test="$subgroup='constructor'">
				<xsl:value-of select="/document/reference/containers/type/apidata/@name" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="/document/reference/apidata/@name" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


  <!-- decorated names -->

  <xsl:template name="shortNameDecorated">
    <!--<xsl:param name="titleType" /> -->
    <xsl:choose>
      <!-- type overview pages and member list pages get the type name -->
      <xsl:when test="$group='type' or ($group='list' and not($subgroup='overload'))">
        <xsl:for-each select="/document/reference[1]">
          <xsl:call-template name="typeNameDecorated" />
        </xsl:for-each>
      </xsl:when>
      <!-- constructors and member list pages also use the type name -->
      <xsl:when test="$subgroup='constructor' or ($subgroup='overload' and /document/reference/apidata/@subgroup='constructor')">
        <xsl:for-each select="/document/reference/containers/type[1]">
          <xsl:call-template name="typeNameDecorated" />
        </xsl:for-each>
      </xsl:when>
      <!-- member pages use the qualified member name -->
      <xsl:when test="$group='member' or ($subgroup='overload' and /document/reference/apidata/@group='member')">
        <xsl:for-each select="/document/reference/containers/type[1]">
          <xsl:call-template name="typeNameDecorated" />
        </xsl:for-each>
        <span class="cs">.</span>
        <span class="vb">.</span>
        <span class="cpp">::</span>
        <xsl:for-each select="/document/reference[1]">
          <xsl:value-of select="apidata/@name" />
          <xsl:apply-templates select="templates" mode="decorated" />
        </xsl:for-each>
      </xsl:when>
      <!-- namespace (and any other) topics just use the name -->
      <xsl:when test="/document/reference/apidata/@name = ''">
        <include item="defaultNamespace" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="/document/reference/apidata/@name" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- plain names -->
  <xsl:template name="shortNamePlain">
    <xsl:param name="qualifyMembers" select="false()" />
    <xsl:choose>
      <!-- type overview pages and member list pages get the type name -->
      <xsl:when test="$group='type' or ($group='list' and not($subgroup='overload'))">
        <xsl:for-each select="/document/reference[1]">
          <xsl:call-template name="typeNamePlain" />
        </xsl:for-each>
      </xsl:when>
      <!-- constructors and member list pages also use the type name -->
      <xsl:when test="$subgroup='constructor' or ($subgroup='overload' and /document/reference/apidata/@subgroup='constructor')">
        <xsl:for-each select="/document/reference/containers/type[1]">
          <xsl:call-template name="typeNamePlain" />
        </xsl:for-each>
      </xsl:when>
      <!-- namespace, member (and any other) topics just use the name -->
      <xsl:when test="/document/reference/apidata/@name = ''">
        <include item="defaultNamespace" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="$qualifyMembers and /document/reference/apidata/@group='member'">
          <xsl:for-each select="/document/reference/containers/type[1]">
            <xsl:call-template name="typeNamePlain" />
          </xsl:for-each>
          <xsl:text>.</xsl:text>
        </xsl:if>
        <xsl:value-of select="/document/reference/apidata/@name" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="typeNamePlain">
    <xsl:param name="annotate" select="false()" />
    <xsl:if test="(containers/type)|type">
      <xsl:for-each select="(containers/type)|type">
        <xsl:call-template name="typeNamePlain">
          <xsl:with-param name="annotate" select="$annotate" />
        </xsl:call-template>
      </xsl:for-each>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <xsl:value-of select="apidata/@name" />
    <xsl:if test="$annotate and templates/template">
      <xsl:value-of select="concat('`',count(templates/template))"/>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
