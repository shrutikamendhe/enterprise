<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="ReportProjectDeptPI.aspx.cs" Inherits="TTSHWeb.ReportProjectDeptPI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/RPrjDeptPI.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Report
                <span>Project PI According To Department
                </span>
                </h1>
                
            </div>
        </div>
        <div class="row">
            <asp:UpdatePanel ID="updRpt" runat="server">
                <ContentTemplate>
                    <div class="col-md-12">
                        
                        <h3 class="frmHead" data-frm="frmDetails">Report Inputs <span>( - )</span></h3>
                        <div class="frmDetails">
                            <div class="col-md-6 col-sm-6">
                                <p>
                                    <label>Select Start Date <b>*</b> </label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                </p>
                                <p>
                                    <label>Category <b>*</b> </label>
                                    <asp:TextBox ID="txtCategory" CssClass="ctlinput" placeholder="Category" ReadOnly="true" runat="server"></asp:TextBox>
                                    <input id="hidCategory" runat="server" type="hidden" />
                                    <div runat="server" id="divCategory" class="CheckboxList">
                                        <asp:CheckBoxList ID="chkCategory" runat="server" CssClass="ctlselect" CellPadding="0" CellSpacing="0">
                                        </asp:CheckBoxList>
                                    </div>
                                    <asp:PopupControlExtender ID="Pexd" PopupControlID="divCategory" Position="Bottom" TargetControlID="txtCategory" runat="server"></asp:PopupControlExtender>
                                </p>
                                <p>
                                    <label>Department <b>*</b> </label>
                                    <asp:TextBox ID="txtDept" CssClass="ctlinput" placeholder="Department" ReadOnly="true" runat="server"></asp:TextBox>
                                    <input id="hidDept" runat="server" type="hidden" />
                                    <div runat="server" id="divDept" class="CheckboxList">
                                        <asp:CheckBoxList ID="chkDept" runat="server" CssClass="ctlselect" OnSelectedIndexChanged="chkDept_SelectedIndexChanged" AutoPostBack="true" CellPadding="0" CellSpacing="0">
                                        </asp:CheckBoxList>
                                    </div>
                                    <asp:PopupControlExtender ID="PopUpDept" PopupControlID="divDept" Position="Bottom" TargetControlID="txtDept" runat="server"></asp:PopupControlExtender>
                                </p>
                                <p></p>
                            </div>
                            <div class="col-md-6 col-sm-6">
                                <p>
                                    <label>Select End Date <b>*</b> </label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;"></asp:TextBox>
                                </p>
                                <p>
                                    <label>Type <b>*</b> </label>
                                    <asp:TextBox ID="txtType" CssClass="ctlinput" placeholder="Type" ReadOnly="true" runat="server"></asp:TextBox>
                                    <input id="hidType" runat="server" type="hidden" />
                                    <div runat="server" id="divType" class="CheckboxList">
                                        <asp:CheckBoxList ID="chkType" runat="server" CssClass="ctlselect" CellPadding="0" CellSpacing="0">
                                        </asp:CheckBoxList>
                                    </div>
                                    <asp:PopupControlExtender ID="PopUpType" PopupControlID="divType" Position="Bottom" TargetControlID="txtType" runat="server"></asp:PopupControlExtender>
                                </p>
                                <p>
                                    <label>PI Name <b>*</b> </label>
                                    <asp:TextBox ID="txtPI" CssClass="ctlinput" placeholder="PI Name" ReadOnly="true" runat="server"></asp:TextBox>
                                    <input id="hidPI" runat="server" type="hidden" />
                                    <div runat="server" id="divPI" class="CheckboxList">
                                        <asp:CheckBoxList ID="chkPI" runat="server" CssClass="ctlselect" CellPadding="0" CellSpacing="0">
                                        </asp:CheckBoxList>
                                    </div>
                                    <asp:PopupControlExtender ID="popUpPI" PopupControlID="divPI" Position="Bottom" TargetControlID="txtPI" runat="server"></asp:PopupControlExtender>
                                </p>
                                <p style="text-align: right">
                                    <asp:Button ID="btnViewRpt" runat="server" Text="View Report" CssClass="action" OnClick="btnViewRpt_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="action" OnClick="btnClear_Click" />
                                </p>
                            </div>
                        </div>
                    </div>
                  <%--   </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="chkDept" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnViewRpt" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
                    <div class="col-md-12">
                        <h3 class="frmHead" data-frm="frmReport">Report <span>( - )</span></h3>
                        <div class="frmReport" style="height: 500px; overflow: auto">
                            <rsweb:ReportViewer ID="RViewer" runat="server" ShowToolBar="True" Width="100%"></rsweb:ReportViewer>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                 </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="chkDept" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnViewRpt" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
