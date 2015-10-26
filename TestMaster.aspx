<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="TestMaster.aspx.cs" Inherits="TTSHWeb.TestMaster" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    test
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <asp:TextBox ID="txtCountry" runat ="server" onpaste="return false;" placeholder="Type Keyword to search Country" CssClass="ctlinput" />
        <asp:HiddenField ID="HdnCountryId" runat="server" />
     <div class="roleManager container" runat="server" id="roleManager">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Create Roles<span>Search, Filter and View Audit Trial</span></h1>
            </div>
            <div class="col-md-6 col-sm-6 paging">
                <div class="grid-search">
                    <uc1:SearchBox runat="server" ID="SearchBox" />
                </div>
            </div>
        </div>
    </div>

    
    <div class="container rolesContainer" id="rolesContainer" runat="server" >
     <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmDetails">Create Role<span>( - )</span></h3>
                </div>
            </div>  
         <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                         <p>
                            <label>Group Name<b>*</b></label>
                          
                               <asp:DropDownList ID="ddlGroupName"  TabIndex="1" runat="server"  CssClass="ctlselect" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged" >
                                 
                               </asp:DropDownList>
                             
                         </p>
      </div>
                    </div>
             </div>
        <asp:UpdatePanel runat="server" ID="UpPi" UpdateMode="Conditional">
                <ContentTemplate>
         <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                         <p>
                            <label>Access Rights<b>*</b></label>
                            <asp:TreeView ID="tvAccess" runat="server" ShowCheckBoxes="All" CssClass="tvAccess">
                            </asp:TreeView>
                         </p>
      </div>
                    </div>
             </div>
                         </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlGroupName" EventName="SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>
    <script type="text/javascript" >
        $(document).ready(function () {
            //Nitin V:To Get The List Of Groups
           // GetGroups("", "", "", "", "<%=ddlGroupName.ClientID%>");
            //Nitin V:BindTreeView
            

        });
        
     
    </script>
    </div>
</asp:Content>
