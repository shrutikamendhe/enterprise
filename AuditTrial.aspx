<%@ Page Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="AuditTrial.aspx.cs" Inherits="TTSHWeb.AuditTrial" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Audit.css" rel="stylesheet" />
    <script src="Scripts/Audit.js"></script>
    <script src="Scripts/validation.js"></script>
    <link href="../css/tableSorter.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="AuditGrid container" runat="server" id="auditGrid">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Audit Trial View <span>Search, Filter and View Audit Trial</span></h1>
            </div>
            <div class="col-md-6 col-sm-6 paging">
                <div class="grid-search">
                    <uc1:SearchBox runat="server" ID="SearchBox" />
                </div>
            </div>
        </div>
    </div>

    <div class="container EthicsContainer" id="EthicsContainer" runat="server" >

          <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmDetails">Audit Trial <span>( - )</span></h3>
                </div>
            </div>

            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                         <p>
                            <label>Modules<b>*</b></label>
                            <asp:DropDownList CssClass="ctlselect" ID="ddlAuditModules" title="Audit Modules" runat="server" TabIndex="1" OnSelectedIndexChanged="ddlAuditModules_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                         </p>
                        <p>
                            <label>Fields</label>
                            <asp:ListBox CssClass="ctllistSelect" ID="listSelectFrom" title="Audit Actions" runat="server" TabIndex="5" SelectionMode="Multiple"></asp:ListBox>
                            <a href="JavaScript:void(0);" id="btnAdd">Add &raquo;</a>
                           
                        </p> 
                    </div>
                     <div class="col-md-6 col-sm-6">
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                 &nbsp;
                            </p>
                            <p>
                                 &nbsp;
                            </p>
                            <p>
                            <a href="JavaScript:void(0);" id="btnRemove">&laquo; Remove</a>
                             <asp:ListBox CssClass="ctllistSelect" ID="listSelectTo" title="Audit Actions" runat="server" TabIndex="5" SelectionMode="Multiple" Height="100"></asp:ListBox>
                            </p>
                        </div>
                </div>
            </div>
             <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                         <p>
                            <label>Operations<b>*</b></label>
                             <asp:DropDownList CssClass="ctlselect" ID="ddlAuditActions" title="Audit Actions" runat="server" TabIndex="5">
                                 <asp:ListItem Value="All">All</asp:ListItem>
                                 <asp:ListItem Value="Update">Update</asp:ListItem>
                                 <asp:ListItem Value ="Delete">Delete</asp:ListItem>
                             </asp:DropDownList>
                         </p>
                        <p>
                            <label>Date Range From</label>
                            <asp:TextBox ID="txtAuditFromDate" CssClass="ctlinput ctlinput-sm datepicker" title="Audit From Date" TabIndex="8" runat="server"></asp:TextBox>
                            
                       </p>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            &nbsp;
                        </p>
                        <p>
                             &nbsp;
                        </p>
                        <p>
                            <label>Date Range To</label>
                            <asp:TextBox ID="txtAuditToDate" CssClass="ctlinput ctlinput-sm datepicker" title="Audit To Date" TabIndex="9" runat="server"></asp:TextBox>
                        </p>
                    </div>
                </div>
            </div>
             <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <%--<p style="text-align:right;">--%>
                    <p align="right">
                        <asp:Button CssClass="action" ID="btnAuditShow" runat="server" Text="Show Details" TabIndex="29"/>
                        <asp:Button CssClass="action" ID="btnAuditCancel" runat="server" Text="Cancel" TabIndex="30" />
                    </p>
                </div>
            </div>
            <div class="row">
            <div class="col-md-12">
                <div class="tblResposiveWrapper">
                    <table id="tblResposive">
                        <thead>
                            <tr>
                                <th width="100">Project ID</th>
                                <th width="300">Table Name</th>
                                <th width="150">Field Name</th>
                                <th width="100">Old Value</th>
                                <th width="110">New Value</th>
                                <th width="50">Action</th>
                                <th width="50">User</th>
                                <th width="145">Date</th>
                               
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptrAuditTrial" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <p><%#Eval("Project ID") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Table Name") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Field Name") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Old Value") %></p>
                                        </td>
                                        <td>

                                            <p><%#Eval("New Value") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Action") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("User") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Date") %></p>
                                        </td>
                                    </tr>
                                </ItemTemplate>

                            </asp:Repeater>

                        </tbody>
                    </table>

                    <!-- Grid View -->



                    <!-- Grid View -->
                </div>
            </div>

        </div>
  </div>
   <%-- <div class="frmAudit" runat="server" >
        <div class="row" >
            <div class="col-md-6 col-sm-6">
                <h3 class="frmHead" data-frm="frmDetails">Audit Trial</h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <p>
                    <label>Modules<b>*</b></label>
                    <asp:DropDownList CssClass="ctlselect" ID="ddlAuditModules" title="Audit Modules" runat="server" TabIndex="1"></asp:DropDownList>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <p>
                    <label>Fields<b>*</b></label>
                    <select name="selectfrom" id="select_from" multiple="multiple">
                        <option value="1">Item 1</option>
                        <option value="2">Item 2</option>
                        <option value="3">Item 3</option>
                        <option value="4">Item 4</option>
                    </select>
                    <a href="JavaScript:void(0);" id="btn_add">Add &raquo;</a>
                    <a href="JavaScript:void(0);" id="btn_remove">&laquo; Remove</a>
                    <select name="selectto" id="select_to" multiple="multiple">
                        <option value="5">Item 5</option>
                        <option value="6">Item 6</option>
                        <option value="7">Item 7</option>
                    </select>
                </p> 
            </div>
        </div>
         <div class="row">
             <div class="col-md-6 col-sm-6">
                <p>
                  <label>Actions<b>*</b></label>
                  <asp:DropDownList CssClass="ctlselect" ID="ddlAuditActions" title="Audit Actions" runat="server" TabIndex="5">
                    <asp:ListItem Value="Update">Update</asp:ListItem>
                    <asp:ListItem Value ="Delete">Delete</asp:ListItem>
                  </asp:DropDownList>
                </p>
                <p>
                    <label>Date Range From</label>
                    <asp:TextBox ID="txtAuditFromDate" CssClass="ctlinput ctlinput-sm datepicker" title="Audit From Date" TabIndex="8" runat="server"></asp:TextBox>
                    <label>Date Range To</label>
                    <asp:TextBox ID="txtAuditToDate" CssClass="ctlinput ctlinput-sm datepicker" title="Audit To Date" TabIndex="9" runat="server"></asp:TextBox>
                </p>
             </div>


         </div>

    </div>--%>
</asp:Content>
