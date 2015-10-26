<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="MenuMasterMappping.aspx.cs" Inherits="TTSHWeb.MenuMasterMappping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(FinalValidation);
            $('#' + getclientId('chkall')).change(function () {
                $('input[type=checkbox]').attr('checked', $('#' + getclientId('chkall')).is(':checked'));
            });
            // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        });

    </script>
    <link href="css/MenuAccessRights.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="roleManager container" runat="server" id="roleManager">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Menu Access Rights<span>Grant / Revoke Menu Access Rights</span></h1>
            </div>
            <div class="col-md-6 col-sm-6 paging">
                <div class="grid-search">
                    <%--         <uc1:SearchBox runat="server" ID="SearchBox" />--%>
                </div>
            </div>
        </div>
    </div>





    <div class="container rolesContainer" id="rolesContainer" runat="server">

        <asp:UpdatePanel runat="server" ID="UpPi">

            <ContentTemplate>
                <div class="frm frmDetails" style="display: block;">
                    <div class="row">
                        <div class="col-md-12">

                            <p>
                                <label>Select Group <b>*</b></label>

                                <asp:DropDownList ID="ddlGroupName" TabIndex="1" runat="server" CssClass="ctlselect ctlinput-md" AutoPostBack="true" OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged"></asp:DropDownList>
                            </p>
                        </div>
                    </div>
                </div>



                <div class="frm frmDetails" style="display: block; min-height: 200px;margin-top: -20px;">
                    <div class="row">
                        <div class="col-md-12">
                           
                            <h2>Access Rights</h2>
                            <p>
                                <input type="checkbox" id="chkall" runat="server" onchange="return CheckUncheckAll();">Select All</p>
                        </div>
                    </div>

                    <div class="row" runat="server" id="row" >
                       <%-- <div class="col-md-4 col-sm-6">
                            <div class="user-access">
                                <asp:TreeView ID="tvAccess" runat="server" ShowCheckBoxes="All" CssClass="tvAccess">
                                </asp:TreeView>
                            </div>
                        </div>--%>
                    </div>
                    <div class="row margin-top frmAction">
                        <div class="col-md-12">
                            <p style="text-align: right">
                                <asp:HiddenField ID="HidMenuAccess" runat ="server" Value="" />

                                <asp:Button CssClass="action" ID="btnSave" runat="server" Text="Save Access" OnClick="btnSave_Click" OnClientClick="javascript:return FinalValidation();" />
                                <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />

                            </p>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlGroupName" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </div>


    <script src="Scripts/MenuMasterMapping.js" type="text/javascript"></script>
</asp:Content>
