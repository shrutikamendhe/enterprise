<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="SelectedProjectBarchartRpt.aspx.cs" Inherits="TTSHWeb.SelectedProjectBarchartRpt" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {
            if ($("#tblResposive tbody tr").length == 0) {
                $("#tblResposive tbody").html("<tr><td colspan='3' > No Records Available <td></tr>");
                $("#projectPaging").hide();
                $("#tblResposive thead th").css("background-image", "none");
                $("#tblResposive thead th").unbind("click");
            }
        });


        function CheckToDateonBlur() {

            var TxtStartDate = $('[id*=TxtStartDate]');
            var TxtEndDate = $('[id*=TxtEndDate]');

            if (TxtStartDate.val().trim() != "" && TxtEndDate.val().trim() != "") {
                var dates = new Array();
                dates.push(TxtStartDate.val());
                var LDate = GetMaxDate(dates);
                var CTCExp = ConvertDatetoMDY(TxtEndDate.val());
                var reslt = CompareDate(LDate, CTCExp);
                if (reslt == false) {
                    MessageBox("To Date should be Greater than or Equal to From Date ");
                    TxtEndDate.val('');
                    return false;
                }
            }

            return true;
        }

        function isValidate() {
            var TxtStartDate = $('[id*=TxtStartDate]');
            var TxtEndDate = $('[id*=TxtEndDate]');

            if (TxtStartDate.val().trim() == "" || TxtEndDate.val().trim() == "") {
                MessageBox("Please Select Both Date ");
                return false;
            }

            if (TxtStartDate.val().trim() != "" && TxtEndDate.val().trim() != "") {
                var dates = new Array();
                dates.push(TxtStartDate.val());
                var LDate = GetMaxDate(dates);
                var CTCExp = ConvertDatetoMDY(TxtEndDate.val());
                var reslt = CompareDate(LDate, CTCExp);
                if (reslt == false) {
                    MessageBox("To Date should be Greater than or Equal to From Date ");
                    TxtEndDate.val('');
                    return false;
                }
            }

            return true;
        }

        function CompareDate(dat1, dat2) {
            var cfd = Date.parse(dat1);
            var ctd = Date.parse(dat2);

            var date1 = new Date(cfd);
            var date2 = new Date(ctd);

            if (Date.parse(date1) > Date.parse(date2)) {
                return false;
            }

            return true;
        }

        function callLoader() {
            hideLoading();
            var span = $('.ReportViewerControl div[id$=_AsyncWait_Wait] span');
            span.text('Please Wait..');
            var img = $('.ReportViewerControl div[id$=_AsyncWait_Wait] img');
            img.attr('src', 'Images/loader-32.GIF')
        }

        $(document).ready(function () {

            callLoader();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callLoader);
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="projectGrid container" runat="server" id="DivMain">

        <div class="frmProject">
            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-12">
                        <div class="tblResposiveWrapper">

                            <table>
                                <tr>
                                    <td style="width: 150px;">From<label> Date :&nbsp; </label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtStartDate" autocomplete="off" CssClass="ctlinput ctlinput-sm datepicker" onchange="return CheckToDateonBlur();" runat="server"></asp:TextBox></td>
                                    <td style="width: 50px;">&nbsp;</td>
                                    <td style="width: 150px;">To<label> Date : </label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtEndDate" autocomplete="off" CssClass="ctlinput ctlinput-sm datepicker" runat="server" onchange="return CheckToDateonBlur();"></asp:TextBox></td>
                                    <td style="width: 50px;">&nbsp;</td>
                                    <td>
                                        <asp:Button CssClass="action" ID="btnShow" OnClick="btnShow_Click" runat="server" Text="Show" OnClientClick="return isValidate();" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPIDetails">Project Details
                </h3>


            </div>
        </div>
        <div class="frm frmPIDetails" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper">
                        <table id="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 80px">Sr No.</th>
                                    <th style="width: 100px">Project ID</th>
                                    <th>Project Title</th>
                                    <th style="width: 135px">Action</th>
                                </tr>
                            </thead>

                            <tbody>

                                <asp:Repeater ID="RptProjectGrid" OnItemCommand="RptProjectGrid_ItemCommand" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td data-th="Sr No.">
                                                <p>
                                                    <%# Container.ItemIndex+1 %>
                                                </p>

                                            </td>
                                            <td data-th="Project ID">
                                                <p><%#Eval("DisplayProjectID") %></p>
                                            </td>
                                            <td data-th="Project Title">
                                                <p><%#Eval("ProjectTitle") %></p>
                                            </td>


                                            <td data-th="Action">
                                                <p>


                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                                        <asp:LinkButton ID="lnkShowChart" runat="server" Text="Show Bar Chart"
                                                            CommandArgument='<%#Eval("ID") %>'>                                                           
												
                                                        </asp:LinkButton>

                                                    </asp:PlaceHolder>


                                                </p>
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                </asp:Repeater>

                            </tbody>
                        </table>
                    </div>

                </div>
            </div>
        </div>

    </div>



    <div class="container EthicsContainer" id="ProjectDetailContainer" runat="server" visible="false">

        <div class="frmProject">

            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-12">

                        <table style="width: 98%">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <label>Select Year :&nbsp; </label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlYear" CssClass="ctlselect" runat="server">
                                                </asp:DropDownList></td>

                                            <td>&nbsp;</td>

                                            <td>&nbsp;From Month :&nbsp;</td>

                                            <td>
                                                <asp:DropDownList ID="ddlFromMonth" CssClass="ctlselect" runat="server">
                                                    <asp:ListItem Selected="True" Value="1">Jan</asp:ListItem>
                                                    <asp:ListItem Value="2">Feb</asp:ListItem>
                                                    <asp:ListItem Value="3">Mar</asp:ListItem>
                                                    <asp:ListItem Value="4">Apr</asp:ListItem>
                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                    <asp:ListItem Value="6">Jun</asp:ListItem>
                                                    <asp:ListItem Value="7">Jul</asp:ListItem>
                                                    <asp:ListItem Value="8">Aug</asp:ListItem>
                                                    <asp:ListItem Value="9">Sep</asp:ListItem>
                                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                                </asp:DropDownList></td>

                                            <td>&nbsp;</td>

                                            <td>&nbsp; To Month :&nbsp;&nbsp; </td>

                                            <td>
                                                <asp:DropDownList ID="ddlToMonth" runat="server" CssClass="ctlselect">
                                                    <asp:ListItem Value="1">Jan</asp:ListItem>
                                                    <asp:ListItem Value="2">Feb</asp:ListItem>
                                                    <asp:ListItem Value="3">Mar</asp:ListItem>
                                                    <asp:ListItem Value="4">Apr</asp:ListItem>
                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                    <asp:ListItem Value="6">Jun</asp:ListItem>
                                                    <asp:ListItem Value="7">Jul</asp:ListItem>
                                                    <asp:ListItem Value="8">Aug</asp:ListItem>
                                                    <asp:ListItem Value="9">Sep</asp:ListItem>
                                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                                    <asp:ListItem Value="12" Selected="True">Dec</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td>&nbsp;</td>

                                            <td>&nbsp;</td>

                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button CssClass="action" ID="btnShow1" OnClick="btnShow1_Click" runat="server" Text="Show" /></td>
                                            <td>
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align: right">
                                    <asp:LinkButton ID="lnkback" Text="Back to View" runat="server" OnClick="lnkback_Click"></asp:LinkButton></td>

                            </tr>

                        </table>




                    </div>
                </div>
            </div>
        </div>


        <%--<div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPIDetails" >
                </h3>


            </div>
        </div>--%>
        <div class="frm frmPIDetails" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <rsweb:reportviewer id="ReportViewerSSRSDemo" class="ReportViewerControl" runat="server" showtoolbar="False" keepsessionalive="false" width="100%"></rsweb:reportviewer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
