<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="CustomerSite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<h1>
				Customer Repository</h1>
			<p>
				<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="customerSource">
					<Columns>
						<asp:CommandField ShowDeleteButton="false" ShowEditButton="True" />
						<asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
						<asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
						<asp:BoundField DataField="Birthday" HeaderText="Birthday" SortExpression="Birthday" />
						<asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
						<asp:BoundField DataField="Timestamp" HeaderText="Timestamp" ReadOnly="True" SortExpression="Timestamp" />
					</Columns>
				</asp:GridView>
				<asp:ObjectDataSource ID="customerSource" runat="server" DataObjectTypeName="CustomerLibrary.Customer"
					DeleteMethod="Delete" InsertMethod="Add" SelectMethod="GetAll" TypeName="CustomerLibrary.CustomerDataAccess"
					UpdateMethod="Update">
					<DeleteParameters>
						<asp:Parameter Name="id" Type="int32" />
					</DeleteParameters>
					<UpdateParameters>
						<asp:Parameter Name="Birthday" Type="DateTime" />
						<asp:Parameter Name="Timestamp" Type="DateTime" />
					</UpdateParameters>
				</asp:ObjectDataSource>
			</p>
		</div>
	</form>
</body>
</html>
