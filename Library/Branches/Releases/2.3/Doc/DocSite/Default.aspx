<%@ Page Language="C#" MasterPageFile="~/DocSite.Master" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="DocSite.Default" %>

<asp:Content ID="HelpFile" ContentPlaceHolderID="Content" runat="server">
	<iframe runat="server" class="docsite_content_frame" id="ContentFrame" width="100%" frameborder="0" scrolling="no"></iframe>
	<asp:HiddenField runat="server" ID="ContentUrl" />
	
	<script type="text/javascript">
	var contentFrame_FirstLoad = true;
	function ContentFrame_onload()
	{
		if (!contentFrame_FirstLoad)
			<%= GetSyncTocClientCallback() %>;

		contentFrame_FirstLoad = false;
	}
	</script>
</asp:Content>
