<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="SelectedChartRpt.aspx.cs" Inherits="TTSHWeb.SelectedChartRpt" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <div class="container EthicsContainer" id="ProjectDetailContainer" runat="server">
                     
        <div class="frmProject">

            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-12">

                        <table style="width:98%"  >
                            <tr>
                                <td><table>
                            <tr>
                                <td >
                                    <label>Select Year :&nbsp; </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" CssClass="ctlselect" runat="server">                                      
                                </asp:DropDownList></td>
                    
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button CssClass="action" ID="btnShow" OnClick="btnShow_Click" runat="server" Text="Show" /></td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
</td>
                                <td style="text-align:right"><asp:LinkButton ID="lnkback" Text="Back to View" runat="server" OnClick="lnkback_Click"></asp:LinkButton></td>

                            </tr>

                        </table>

                        


                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPIDetails" >
                </h3>


            </div>
        </div>
        <div class="frm frmPIDetails" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper">
                        <rsweb:ReportViewer ID="ReportViewerSSRSDemo" runat="server" ShowToolBar="False" Width="100%"></rsweb:ReportViewer>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>
