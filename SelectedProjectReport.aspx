<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="SelectedProjectReport.aspx.cs" Inherits="TTSHWeb.SelectedProjectReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="projectGrid container" runat="server" id="DivMain">
                     
        <div class="frmProject">
            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-12">

                        <table>
                            <tr>
                                <td style="width:150px;">
                                    From<label> Date :&nbsp; </label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtStartDate" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox></td>
                                <td style="width:50px;">&nbsp;</td>
                                <td style="width:150px;">
                                    To<label> Date : </label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEndDate" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox></td>
                                <td style="width:50px;">&nbsp;</td>
                                <td>
                                    <asp:Button CssClass="action" ID="btnShow" OnClick="btnShow_Click" runat="server" Text="Show" /></td>
                            </tr>
                        </table>

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

                                <asp:Repeater ID="RptProjectGrid" OnItemCommand="RptProjectGrid_ItemCommand" runat="server" >
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <p>
                                                    <%# Container.ItemIndex+1 %>
                                                </p>

                                            </td>
                                            <td>
                                                <p><%#Eval("DisplayProjectID") %></p>
                                            </td>
                                            <td>
                                                <p><%#Eval("ProjectTitle") %></p>
                                            </td>


                                            <td>
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
                    
                              <%-- <td>
                                    &nbsp;From Month :&nbsp;</td>
                    
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
                    
                                <td>
                                    &nbsp;</td>
                    
                                <td>
                                    &nbsp; To Month :&nbsp;&nbsp; </td>
                    
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
                    
                                <td>
                                    &nbsp;</td>--%>
                    
                                <td>
                                    &nbsp;</td>
                    
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button CssClass="action" ID="btnShow1" OnClick="btnShow1_Click" runat="server" Text="Show" /></td>
                                <td>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
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


        <%--<div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPIDetails" >
                </h3>


            </div>
        </div>--%>
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
