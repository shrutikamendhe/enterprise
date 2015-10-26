<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestWeb.aspx.cs" Inherits="TTSHWeb.TestWeb" %>

<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="float:left;width:50%;top:0px;background-color:lightgray">
            <%-- <asp:DropDownList ID="ddlSelect" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSelect_SelectedIndexChanged">
                 <asp:ListItem Text="FEASIBALITY" Value="FEASIBALITY"></asp:ListItem>
                 <asp:ListItem Text="ETHICS" Value="ETHICS"></asp:ListItem>
                 <asp:ListItem Text="SELECTED" Value="SELECTED"></asp:ListItem>
                 <asp:ListItem Text="REGULATORY" Value="REGULATORY"></asp:ListItem>
                 <asp:ListItem Text="GRANT" Value="GRANT"></asp:ListItem>
                 <asp:ListItem Text="CONTRACT" Value="CONTRACT"></asp:ListItem>
             </asp:DropDownList>--%>
            </div>
            <div style="float:right;width:50%;top:0px;background-color:cornsilk">
                <uc1:SearchBox runat="server" ID="SearchBox" />
            </div>
        </div>
       
        <br />
        <br />
        <br />
    <div>
        <asp:Label ID="lblErr" runat="server" ForeColor="Red" Width="200px"></asp:Label>
        <br />
        <asp:GridView ID="grdSearch" runat="server"></asp:GridView>

    </div>
    
    </form>
</body>
</html>
