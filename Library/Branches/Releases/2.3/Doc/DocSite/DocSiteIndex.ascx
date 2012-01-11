<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DocSiteIndex.ascx.cs" Inherits="DocSite.DocSiteIndex" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:UpdatePanel ID="indexUpdatePanel" runat="server" ChildrenAsTriggers="true">
	<ContentTemplate>
		<div class="index_filter">
			<asp:CheckBox runat="server" ID="indexFilterCheckbox" Checked="true" AutoPostBack="true" AccessKey="F" Text="Filter Index" 
				ToolTip="When this box is checked the index is filtered by the specified keywords.  Unchecked, the entire index is displayed unfiltered." />
			<asp:UpdateProgress runat="server" ID="indexUpdateProgress" AssociatedUpdatePanelID="indexUpdatePanel">
				<ProgressTemplate>
					<div class="index_filter_progress">Please wait...</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		</div>
		<div id="indexSearchDiv" class="index_search" runat="server">
			<div class="index_search_input_container">
				<asp:TextBox runat="server" ID="indexSearchTextBox" CssClass="index_search_input" onfocus="this.select();" />
			</div>
			<div class="index_button_container">
				<asp:ImageButton runat="server" ID="indexSearchButton" CssClass="index_search_button" onclick="indexSearchButton_Click" ImageUrl="~/DocSiteSearch.gif"
				onmouseover="this.className='index_search_button index_search_button_hot';" onmouseout="this.className='index_search_button';"
				ToolTip="Filters the index by the specified keywords." />
			</div>
		</div>
		<div runat="server" id="index_list_container">
			<asp:Repeater runat="server" ID="indexRepeater" EnableViewState="false">
				<ItemTemplate>
					<div class='<%# ((currentItemSelected = SelectedHelpFile.Equals(Eval("File") as string, StringComparison.Ordinal)) ? "index_item_selected" : "index_item") %>'>
						<a class="index_item_link" href='<%# (currentItemSelected) ? "#" : GetPostBackClientHyperlink(Eval("File") as string) %>' title='<%# Eval("Name") %>'>
							<%# Eval("Name")%>
						</a>
					</div>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</ContentTemplate>
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="indexFilterCheckbox" EventName="CheckedChanged" />
		<asp:AsyncPostBackTrigger ControlID="indexSearchButton" EventName="Click" />
	</Triggers>
</asp:UpdatePanel>