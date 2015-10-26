<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="FeasibilityDetails.aspx.cs" Inherits="TTSHWeb.FeasibilityDetails" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!--Script references -->
    <script src="Scripts/Feasibility.js"></script>
 <%--   <link href="css/tableSorter.css" rel="stylesheet" />--%>
    <script src="Scripts/validation.js"></script>
    <!--End of Script references -->


    <!--Style references -->
    <link href="css/Feasibility.css" rel="stylesheet" />
    <!--End of Style references -->


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HdnCROTxt" Value="" runat="server" />
    <asp:HiddenField ID="HdnSponsorTxt" Value="" runat="server" />
    <asp:HiddenField ID="HdnId" Value="0" runat="server" />
    <div class="projectGrid container" runat="server" id="projectGrid">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Feasibility Details View <span>Search, Filter and Edit Feasibility Details</span></h1>
            </div>
            <div class="col-md-6 col-sm-6 paging">
                <div class="grid-search">
                   
                        <uc1:SearchBox runat="server" ID="SearchBox" />
                   
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="tblResposiveWrapper">
                    <table id="tblResposive">
                        <thead>
                            <tr>
                                <th width="100">Project ID</th>
                                <th width="300">Project Title</th>
                                <th>Project Category</th>
                                <th>Feasibility Status</th>
                                <th width="110">DSRB/IRB No.</th>
                                <th width="150">PI Name</th>
                                <th width="95">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptrProjectDetail" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td data-th="Project ID">
                                            <p><%#Eval("s_Display_Project_ID") %></p>
                                        </td>
                                        <td data-th="Project Title">
                                            <p><%#Eval("s_Project_Title") %></p>
                                        </td>
                                        <td data-th="Project Category">
                                            <p><%#Eval("s_Project_Category") %></p>
                                        </td>
                                        <td data-th="Feasibility Status">
                                            <p><%#Eval("Feasibility_Status_Name") %></p>
                                        </td>
                                        <td data-th="DSRB/IRB No.">

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_Names") %></p>
                                        </td>
                                        <td data-th="Action">
                                            <p class="grid-action">

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("Status").ToString() == "New" %>'>

                                                    <asp:LinkButton ID="NewLink" OnCommand="NewLink_Command" CommandArgument='<%#  Convert.ToInt32(Eval("i_Project_ID"))==0?Eval("i_ID"):Eval("i_Project_ID")    %>' runat="server"><img title="Add Feasibility Details" alt="" src="images/Add-New.png"></asp:LinkButton>


                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Status").ToString() == "Edit"  %>'>
                                                    <asp:LinkButton ID="EditLink" OnCommand="EditLink_Command" CommandArgument='<%#Eval("i_Feasibility_ID") %>' runat="server">
                                                        <img title="Edit Feasibility Details" alt="" src="images/icon-edit.png">
                                                    </asp:LinkButton>

                                                </asp:PlaceHolder>
                                                 <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Feasibility_ID")%>' Visible='<%# "Edit".Contains(Eval("Status").ToString()) %>'
                                                    OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_Feasibility_ID")) %>'
                                                    CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Ethics Detail" alt="" src="../images/icon-delete.png">
                                                </asp:LinkButton>
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# "Edit,View".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ViewLink" OnCommand="ViewLink_Command" CommandArgument='<%#Eval("i_Feasibility_ID") %>' runat="server">
                                                        <img title="View Feasibility Details" alt="" src="images/icon-view.png">
                                                    </asp:LinkButton>

                                                </asp:PlaceHolder>


                                            </p>
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


        <div class="row" id="projectPaging">
            <div class="col-md-6 paging">
                <div class="page-info">
                    <h3>18  Results Found</h3>
                    <p>Showing Page 2 of Total 4 Pages | <a href="#">First Page</a> | <a href="#">Last Page</a></p>
                </div>
            </div>
            <div class="col-md-6 paging">
                <div class="pages">
                </div>
            </div>
        </div>

        <asp:Button ID="btnAddNewProject" OnClientClick="AddNewProject();return false;" OnClick="btnAddNewProject_Click" CssClass="action" Style="margin-top: 20px; margin-bottom: 30px;" runat="server" Text="Add New Project" />
        <asp:Button ID="delete" Style="display: none" runat="server" OnClick="delete_Click" />

    </div>

    <div class="container FeasibilityContainer" id="FeasibilityContainer" runat="server" visible="false">

        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Feasibility Details <span>Feasibility Details Form ( <b runat="server" id="bProjectID">Project ID: ####</b> )</span></h1>
            </div>
            <div class="col-md-6 col-sm-6">
                <p class="align-right" style="margin-top: 65px;">
                    <asp:LinkButton CssClass="link" OnClick="backToGrid_Click1" ID="backToGrid" runat="server">Back To View</asp:LinkButton>
                </p>
            </div>

        </div>

        <div class="frmProject">

            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmDetails">Project Details <span>( - )</span></h3>
                </div>
            </div>

            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>Project Title <b>*</b></label>
                            <asp:TextBox ID="txtProjectTitle" TextMode="MultiLine" disabled="disabled" CssClass="ctltext" title="Project Title" runat="server" TabIndex="1"></asp:TextBox>
                        </p>
                        <p>
                            <label>Alias 1</label>
                            <asp:TextBox ID="txtAlias1" CssClass="ctlinput len150" title="Alias 1" runat="server" TabIndex="3"></asp:TextBox>
                        </p>

                        <p>
                            <label>Short Title</label>
                            <asp:TextBox ID="txtShortTitle" CssClass="ctlinput len100" title="Short Title" runat="server" TabIndex="7"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Project Category <b>*</b></label>
                            <asp:DropDownList CssClass="ctlselect" ID="ddlProjectCategory" disabled="disabled" title="Project Category" runat="server" TabIndex="2"></asp:DropDownList>
                        </p>
                        <p>
                            <label>Alias 2</label>
                            <asp:TextBox ID="txtAlias2" CssClass="ctlinput len150" title="Alias 2" runat="server" TabIndex="5"></asp:TextBox>
                        </p>

                    </div>
                </div>
            </div>

            <!--Feasibility Current Status -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmCurrStatus">Feasibility Current Status <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmCurrStatus" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Feasibility Title <b>*</b></label>
                            <asp:TextBox ID="txtFeasibilityTitle" CssClass="ctltext Req" title="Feasibility Title" TextMode="MultiLine" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Feasibility Status <b>*</b></label>
                            <asp:DropDownList ID="ddlCurrentStatus" CssClass="ctlselect Req" title="Feasibility Current Status" runat="server"></asp:DropDownList>

                        </p>
                        <p>
                            <label>Date Updated</label>
                            <asp:TextBox ID="txtDateUpdated" CssClass="ctlinput ctlinput-sm" Enabled="false" title="Date Updated" runat="server"></asp:TextBox>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Feasibility Start Date <b>*</b></label>
                            <asp:TextBox ID="txtFeasibilityStartDate" CssClass="ctlinput ctlinput-sm datepicker Req" title="Feasibility Start Date" TabIndex="6" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Date of Initial Email Sent to TTSH </label>

                            <asp:TextBox ID="txtDateInitialEmailSent" CssClass="ctlinput ctlinput-sm datepicker" title="Date of Initial Email Sent to TTSH" runat="server"></asp:TextBox>



                        </p>
                        <p>
                            <label>Last Updated By</label>
                            <asp:TextBox ID="txtLastUpdatedBy" CssClass="ctlinput ctlinput-sm" Enabled="false" title="Last Updated By" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>
            <!--End of Feasibility Current Status -->

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 class="frmHead" data-frm="frmPI">Principal Investigator (PI) Details <span>( - )</span>
                    </h3>
                    <p><span>+</span> <a class="newPI link" data-frm="frmNewPIDetails">Record New PI Details</a></p>

                </div>
            </div>

            <div class="frm frmPI" style="display: block;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <div class="tblResposiveWrapper">
                            <table class="tblResposive" id="tblPiDetail">
                                <thead>
                                    <tr>

                                        <%--<th width="100">Department</th>
                                        <th>PI Name</th>
                                        <th>Email</th>
                                        <th>Phone</th>
                                        <th width="120">PI MCR No.</th>
                                        <th width="95">Action</th>--%>
                                        <th style="width: 450px; text-align: left">Department</th>
                                        <th style="text-align: left">PI Name</th>
                                        <th style="text-align: left">Email</th>
                                        <th style="text-align: left">Phone</th>
                                        <th style="text-align: left">PI MCR No.</th>
                                        <th style="width: 45px; text-align: right">Action</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="rptrPIDetails" runat="server">
                                        <ItemTemplate>
                                            <tr piid="<%# Eval("i_ID") %>">
                                                <td data-th="Department">
                                                    <p><%# Eval("s_DeptName") %></p>
                                                </td>
                                                <td data-th="PI Name">
                                                    <p><%# Eval("s_PIName") %></p>
                                                </td>
                                                <td data-th="Email">
                                                    <p><%# Eval("s_Email") %></p>
                                                </td>
                                                <td data-th="Phone">
                                                    <p><%# Eval("s_Phone_no") %></p>
                                                </td>
                                                <td data-th="PI MCR No.">
                                                    <p><%# Eval("s_MCR_No") %></p>
                                                </td>
                                                <td data-th="Action">
                                                    <p class="grid-action">
                                                        <a>
                                                            <img title="Delete" alt="" onclick="delPiRows(this)" src="images/icon-delete.png"></a>

                                                    </p>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>




                                </tbody>
                            </table>
                        </div>
                        <p class="align-right"><a class="link" onclick="AddMorePI();">+ Add More PI</a></p>
                    </div>
                </div>
            </div>



            <div class="frmNewPIDetails" style="display: none;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Record New Principal Investigator (PI) Details					                  
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="col-md-6 col-sm-6">
                                <p>
                                    <label>Department <b>*</b></label>
                                    <asp:TextBox ID="TxtNewDepartment" CssClass="ctlinput Req autoComp" hdnparam="HdnNewDeptId" title="Department" placeholder="Type Keyword to search Department" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="HdnNewDeptId" runat="server" />
                                </p>

                                <p>
                                    <label>First / Given Name <b>*</b></label>
                                    <asp:TextBox ID="txtNewPIFName" CssClass="ctlinput Req Alpha len100" title="First / Given Name" runat="server"></asp:TextBox>

                                </p>
                                <p>
                                    <label>PI Email <b>*</b></label>
                                    <asp:TextBox ID="txtNewPIEmail" onblur="checkValidEmail(this);" CssClass="ctlinput Req" title="PI Email" runat="server"></asp:TextBox>

                                </p>


                            </div>
                            <div class="col-md-6 col-sm-6">
                                <p>
                                    <label>PI MCR No.<b>*</b></label>
                                    <span style="line-height: normal;">
                                        <asp:TextBox ID="txtNewPIMCR" CssClass="ctlinput Req" title="PI MCR No." runat="server"></asp:TextBox>

                                    </span>
                                </p>

                                <p>
                                    <label>Last Name / Surname <b>*</b></label>
                                    <asp:TextBox ID="txtNewPILName" CssClass="ctlinput Req Alpha len100" title="Last Name / Surname" runat="server"></asp:TextBox>

                                </p>
                                <p>
                                    <label>Phone No.</label>
                                    <asp:TextBox ID="txtNewPIPhone" CssClass="ctlinput" onkeypress="return SingaporePhformat();" onpaste="return false;" title="Phone No." runat="server"></asp:TextBox>

                                </p>

                                <%--<p>
                            <label>&nbsp;</label>
                            
                         
                        </p>--%>
                            </div>
                            <div class="col-md-12 col-sm-12" style="text-align: right; padding-right: 127px;">
                                <asp:Button ID="btnSaveNewPI" CssClass="action" OnClientClick="if(!SaveNewPI()){return false;};" OnClick="btnSaveNewPI_Click" runat="server" Text="Save" />
                                <asp:Button ID="btnSaveCancelNewPI" CssClass="action" OnClientClick="ClearNewPi(); return false;" runat="server" Text="Reset" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSaveNewPI" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="frmAddMorePIDetails" style="display: none;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Add Principal Investigator (PI)					                  
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Department <b>*</b></label>
                            <asp:TextBox ID="TxtDepartment" CssClass="ctlinput Req autoComp" hdnparam="HdnDeptId" title="Department" placeholder="Type Keyword to search Department" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="HdnDeptId" runat="server" />
                        </p>
                        <p>
                            <label>PI Email </label>
                            <asp:TextBox ID="txtPIEmail" CssClass="ctlinput Email" disabled="disabled" title="PI Email" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Phone No.</label>
                            <asp:TextBox ID="txtPiPhoneNo" CssClass="ctlinput" disabled="disabled" title="Phone No." runat="server"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>PI Name<b>*</b></label>
                            <asp:TextBox ID="TxtPIName" CssClass="ctlinput Req autoComp" hdnparam="HdnpiId" title="PI Name" placeholder="Type Keyword to search PI" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>PI MCR No.</label>
                            <asp:TextBox ID="txtPiMCRNo" CssClass="ctlinput" disabled="disabled" title="PI MCR No." runat="server"></asp:TextBox>
                        </p>
                        <p style="text-align: right; padding-right: 112px;">
                            <label>&nbsp;</label>
                            <asp:Button ID="Button1" CssClass="action" OnClientClick="SaveMorePiClick(); return false;" runat="server" Text="Save" />
                            <asp:Button ID="Button2" CssClass="action" OnClientClick="ClearMorePi(); return false;" runat="server" Text="Reset" />

                        </p>
                    </div>
                </div>
            </div>


            <!-- CRO Information -->

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 class="frmHead" data-frm="frmCRO">CRO Information <span>( - )</span>
                    </h3>
                </div>
            </div>

            <div class="frm frmCRO" style="display: block;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">

                        <p>
                            <label>CRO </label>
                            <asp:TextBox ID="txtCRO" hdnparam="hdnCROID" title="CRO" CssClass="ctlinput ctlinput-sm  autoComp SponsorCRO" placeholder="Search CRO" runat="server"></asp:TextBox>
                            <label>
                                <a onclick="$('.frmaddCRO').show()" style="margin-left: 367px;">+Add CRO</a></label>

                        </p>
                    </div>
                </div>
            </div>
            <div class="frmaddCRO" style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>CRO Name<b>*</b></label>
                                    <asp:TextBox ID="txtNewCRO" title="CRO Name" CssClass="ctlinput ctlinput-sm SponsorCRO" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSaveCRO" CssClass="action" runat="server" OnClick="btnSaveCRO_Click" OnClientClick="if(!SaveCRO()){return false;};" Text="Save" />
                                    <asp:Button ID="btnCancelCRO" OnClientClick="CancelCRO();return false;" CssClass="action" runat="server" Text="Cancel" />
                                </p>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


            <%--<div class="frmAddMoreCRODetails" style="display: block;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Add New Princilpe Investigator (PI)					                  
                        </h3>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Department <b>*</b></label>
                            <asp:TextBox ID="TextBox7" CssClass="ctlinput Req" title="Department" placeholder="Search Department" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                        </p>
                        <p>
                            <label>PI Email <b>*</b></label>
                            <asp:TextBox ID="TextBox8" CssClass="ctlinput Email Req" disabled="disabled" title="PI Email" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Phone No.</label>
                            <asp:TextBox ID="TextBox9" CssClass="ctlinput" disabled="disabled" title="Phone No." runat="server"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>PI Name<b>*</b></label>
                            <asp:TextBox ID="TextBox10" CssClass="ctlinput Req" title="PI Name" placeholder="Search PI" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>PI MCR No.<b>*</b></label>
                            <asp:TextBox ID="TextBox11" CssClass="ctlinput ctlinput-sm Req" disabled="disabled" title="PI MCR No." runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>&nbsp;</label>
                            <asp:Button ID="Button5" CssClass="action" OnClientClick="SaveMorePiClick(); return false;" runat="server" Text="Save" />
                            <asp:Button ID="Button6" CssClass="action" OnClientClick="ClearMorePi(); return false;" runat="server" Text="Reset" />

                        </p>
                    </div>
                </div>
            </div>--%>
            <!-- End CRO & Sponsor Information -->

            <!-- Sponsor Information Details -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmsponsor">Sponsor Information <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmsponsor" style="display: block;">
                <div class="row">
                    <div class="col-md-12">
                        <p>
                            <label>Sponsor </label>
                            <asp:TextBox ID="txtSponsorName" hdnparam="hdnSponsorID" title="Sponsor Name" placeholder="Search Sponsor" CssClass="ctlinput ctlinput-sm autoComp SponsorCRO" runat="server"></asp:TextBox>
                            <label>
                                <a onclick="$('.frmaddsponsor').show()" style="margin-left: 341px;">+Add Sponsor</a></label>

                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                    </div>
                </div>
            </div>
            <div class="frmaddsponsor" style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    <label>Sponsor Name<b>*</b></label>
                                    <asp:TextBox ID="txtNewSponsorName" title="Sponsor Name" CssClass="ctlinput ctlinput-sm SponsorCRO" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSaveSponsor" CssClass="action" runat="server" OnClick="btnSaveSponsor_Click" OnClientClick="if(!SaveSponsor()){return false;};" Text="Save" />
                                    <asp:Button ID="btnCancelSponsor" OnClientClick="CancelSponsor();return false;" CssClass="action" runat="server" Text="Cancel" />
                                </p>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- -->


            <!-- Feasibility Details  -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmfeasibilitydetail">Feasibility Details  <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmfeasibilitydetail" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Confidential Agreement <b>*</b></label>
                            <asp:DropDownList ID="ddlConfidentialAgreement" CssClass="ctlselect Req" title="Confidential Agreement" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>

                            </asp:DropDownList>

                        </p>
                        <p>
                            <label>Feasibility Survey Date </label>
                            <asp:TextBox ID="txtSurveyDate" CssClass="ctlinput ctlinput-sm datepicker" title="Feasibility Survey Date" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Feasibility Questionaries File</label>

                            <input type="text" class="txtUpload" readonly="true" id="txtQuestFile" />
                            <span class="btn btn-default btn-file action">Browse...
                                <asp:FileUpload ID="fuQuestFile" title="Feasibility Questionaries File" CssClass="action" Enabled="true" runat="server" />
                            </span>
                            <%--<input type="button" class="btnUpload action" value="Browse..." hdnparam="hdnQuestFile" />--%>
                            <label>
                                <asp:LinkButton ID="DownQuestFile" OnClick="DownQuestFile_Click" Visible="false" runat="server"></asp:LinkButton>
                            </label>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Confidential Agreement File <b>*</b></label>

                            <input type="text" class="txtUpload" readonly="true" id="txtAgreementFile" />
                            <span class="btn btn-default btn-file action">Browse...
                                <asp:FileUpload ID="fuAgreementFile" title="Feasibility Questionaries File" CssClass="action" Enabled="true" runat="server" />
                            </span>
                            <%--<input type="button" class="btnUpload action" value="Browse..." hdnparam="hdnAgreementFile" />--%>
                            <label>
                                <asp:LinkButton ID="DownAgreementFile" OnClick="DownAgreementFile_Click2" Visible="false" runat="server"></asp:LinkButton>
                            </label>

                        </p>
                        <p>
                            <label>Feasibility Survey Comments</label>

                            <asp:TextBox ID="txtSurveyComments" CssClass="ctltext" title="Feasibility Survey Comments" TextMode="MultiLine" runat="server"></asp:TextBox>

                        </p>

                    </div>
                </div>
            </div>
            <!-- End Feasibility Details  -->


            <!-- Protocol Details  -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmprotocoldetail">Protocol Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmprotocoldetail" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Protocol Number </label>
                            <asp:TextBox ID="txtProtocolNumber" CssClass="ctlinput ctlinput-sm" title="Protocol Number" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Protocol Doc No.</label>

                            <asp:TextBox ID="txtProtocolDocNo" CssClass="ctlinput ctlinput-sm" title="Protocol Doc No." runat="server"></asp:TextBox>

                        </p>

                        <p>
                            <label>Protocol File <b>*</b></label>

                            <input type="text" class="txtUpload" readonly="true" id="txtProtocolFile" />

                            <span class="btn btn-default btn-file action">Browse...
                                <asp:FileUpload ID="fuProtocolFile" title="Protocol File" CssClass="action" Enabled="true" runat="server" />
                            </span>
                            <%--<input type="button" class="btnUpload action" value="Browse..." hdnparam="hdnProtocolFile" />--%>
                            <label>
                                <asp:LinkButton ID="DownProtocolFile" OnClick="DownProtocolFile_Click" Visible="false" runat="server"></asp:LinkButton>
                            </label>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Protocol Date</label>
                            <asp:TextBox ID="txtProtocolDate" CssClass="ctlinput ctlinput-sm datepicker" title="Protocol Date" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Protocol Comments </label>
                            <asp:TextBox ID="txtProtocolComments" CssClass="ctltext" TextMode="MultiLine" title="Protocol Comments" runat="server"></asp:TextBox>

                        </p>

                    </div>
                </div>
            </div>
            <!-- End Protocol Details  -->


            <!-- Feasibility Outcome -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmoutcome">Feasibility Outcome <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmoutcome" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Interest <b>*</b> </label>
                            <asp:DropDownList ID="ddlInterest" CssClass="ctlselect Req" title="Interest" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>
                        <p>
                            <label>Feasibility Outcome </label>
                            <asp:DropDownList ID="ddlFeasibilityOutcome" CssClass="ctlselect" title="Feasibility Outcome" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Selected" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Selected" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Interest Comment <b>*</b></label>
                            <asp:TextBox ID="txtInterestComment" title="Interest Comment" CssClass="ctltext Req" TextMode="MultiLine" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>
            <!-- End of Feasibility Outcome -->

            <!-- Other Details  -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmother">Other Details  <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmother" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Site Visit Date</label>
                            <asp:TextBox ID="txtSiteVisitDate" CssClass="ctlinput ctlinput-sm datepicker" title="Site Visit Date" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Co Investigator</label>
                            <asp:TextBox ID="txtCo_Investigator" CssClass="ctlinput ctlinput-sm" title="Co Investigator" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>IM Invitation</label>
                            <asp:TextBox ID="txtIMInvitation" CssClass="ctlinput ctlinput-sm" title="IM Invitation" runat="server"></asp:TextBox>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Upload Feasibility Checklist</label>

                            <input type="text" class="txtUpload" readonly="true" id="txtChecklist" />
                            <span class="btn btn-default btn-file action">Browse...
                                <asp:FileUpload ID="fuChecklist" title="Upload Feasibility Checklist" CssClass="action" Enabled="true" runat="server" />
                            </span>
                            <%--<input type="button" class="btnUpload action" value="Browse..." hdnparam="hdnChecklist" />--%>
                            <label>
                                <asp:LinkButton ID="DownChecklist" OnClick="DownChecklist_Click" Visible="false" runat="server"></asp:LinkButton>
                            </label>

                        </p>
                        <p>
                            <label>In File</label>

                            <asp:DropDownList ID="ddlInFile" CssClass="ctlselect" title="Does it require the renewal?" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>

                    </div>
                </div>
            </div>
            <!-- End Feasibility Details  -->


            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p align="right">

                        <asp:Button CssClass="action" ID="btnUpdateFeasibility" Visible="false" runat="server" Text="Update Details" OnClientClick="if( !UpdateClick()){return false;}" OnClick="btnUpdateFeasibility_Click1" />
                        <asp:Button CssClass="action" ID="btnSaveFeasibility" runat="server" Text="Save Details" OnClientClick="if( !SaveClick()){return false;}" OnClick="btnSaveFeasibility_Click" />
                        <asp:Button CssClass="action" ID="btnCancelFeasibility" runat="server" Text="Cancel" OnClick="btnCancelFeasibility_Click" />

                    </p>
                </div>
            </div>
        </div>





    </div>
    <div class="hiddenFields">

        <asp:HiddenField ID="HdnPi_ID" runat="server" />
        <asp:HiddenField ID="HdnpiId" runat="server" />

        <asp:HiddenField ID="hdnQuestFile" runat="server" />
        <asp:HiddenField ID="hdnAgreementFile" runat="server" />
        <asp:HiddenField ID="hdnProtocolFile" runat="server" />
        <asp:HiddenField ID="hdnChecklist" runat="server" />
        <asp:HiddenField ID="hdnSponsorID" runat="server" />
        <asp:HiddenField ID="hdnCROID" runat="server" />

        <asp:HiddenField ID="OverWriteQuestFile" runat="server" />
        <asp:HiddenField ID="OverWriteAgreementFile" runat="server" />
        <asp:HiddenField ID="OverWriteProtocolFile" runat="server" />
        <asp:HiddenField ID="OverWriteChecklist" runat="server" />

        <asp:HiddenField ID="hdnAgreementFileEnabled" runat="server" />
        <asp:HiddenField ID="hdnProtocolFileEnabled" runat="server" />


    </div>
</asp:Content>