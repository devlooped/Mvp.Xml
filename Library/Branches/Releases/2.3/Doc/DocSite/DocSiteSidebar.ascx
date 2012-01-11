<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DocSiteSidebar.ascx.cs" Inherits="DocSite.DocSiteSidebar" %>
<%@ Register Src="DocSiteIndex.ascx" TagName="DocSiteIndex" TagPrefix="DocSite" %>
<%@ Register Src="DocSiteContents.ascx" TagName="DocSiteContents" TagPrefix="DocSite" %>

<div id="sidebar_button_container">
	<div class="sidebar_button" id="sidebar_contents_button" title="Table of Contents"
	onclick="selectDocsiteSidebarButton(document.getElementById('sidebar_contents_button'), document.getElementById('docsite_toc'));
	<%= "document.getElementById('" + selectedButtonHiddenField.ClientID + "').value = 'sidebar_contents_button'" %>;">
		Contents
	</div>
	<div class="sidebar_button" id="sidebar_index_button" title="Help Index"
	onclick="selectDocsiteSidebarButton(document.getElementById('sidebar_index_button'), document.getElementById('docsite_index'));
	<%= "document.getElementById('" + selectedButtonHiddenField.ClientID + "').value = 'sidebar_index_button'" %>;">
		Index
	</div>
</div>
<div id="docsite_sidebar">
	<div class="sidebar_content_hidden" id="docsite_toc">
		<docsite:docsitecontents id="contents" runat="server" OnSelectedTopicChanged="contents_SelectedTopicChanged" />
	</div>
	<div class="sidebar_content_hidden" id="docsite_index">
		<docsite:docsiteindex id="index" runat="server" OnSelectedHelpFileChanged="index_SelectedHelpFileChanged" />
	</div>
</div>
<div id="docsite_sidebar_handle"></div>
	
<asp:HiddenField runat="server" ID="selectedButtonHiddenField" />
