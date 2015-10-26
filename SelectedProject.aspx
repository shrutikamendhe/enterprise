<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="SelectedProject.aspx.cs" Inherits="TTSHWeb.SelectedProject" %>

<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Script references -->

    <script src="Scripts/jquery.tablesorter.min.js"></script>
    <script src="Scripts/Selected.js"></script>
    <script src="Scripts/validation.js"></script>
    <!--End of Script references -->

    <!--Style references -->
    <link href="../css/tableSorter.css" rel="stylesheet" />
    <link href="css/Selcted.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {

            //  Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(temp);
        });


    </script>

    <!--End of Style references -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdnCurrMonth" runat="server" />
    <asp:HiddenField ID="hdnIRBFileEnabled" runat="server" />
    <asp:HiddenField ID="hdnIRBFile" runat="server" />
    <asp:HiddenField ID="hdnUpdateBtnText" runat="server" />
    <asp:HiddenField ID="hdnIsSelectedUser" runat="server" />
    <asp:HiddenField ID="HdnId" Value="0" runat="server" />
    <div class="projectGrid container" runat="server" id="projectGrid">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Selected Project Details View <span>Search, Filter and Edit Selected Project Details</span></h1>
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
                                <th>Study Status</th>
                                <th width="110">DSRB/IRB No.</th>
                                <th width="150">PI Name</th>
                                <th width="95">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptrProjectDetail" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <p><%#Eval("s_Display_Project_ID") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("s_Project_Title") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("s_Project_Category") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("Study_Status") %></p>
                                        </td>
                                        <td>

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td>
                                            <p><%#Eval("PI_Names") %></p>
                                        </td>
                                        <td>
                                            <p class="grid-action">

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("Status").ToString()=="New" %>'>

                                                    <asp:LinkButton ID="NewLink" OnCommand="NewLink_Command" CommandArgument='<%#Eval("i_Project_ID") %>' runat="server"><img title="Add Selected Project Details" alt="" src="images/Add-New.png"></asp:LinkButton>


                                                </asp:PlaceHolder>

                                                <%--<asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Status").ToString() !="New" %>'>--%>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Status").ToString() =="Edit" %>'>
                                                    <asp:LinkButton ID="EditLink" OnCommand="EditLink_Command" CommandArgument='<%#Eval("i_Project_ID")+","+Eval("s_Display_Project_ID") %>' runat="server">
                                                        <img title="Edit Selected Project Details" alt="" src='<%# Eval("cordinatorstatus").ToString() !="New" ? "images/icon-edit.png" :"images/Add-New.png" %>'>
                                                    </asp:LinkButton>
                                                </asp:PlaceHolder>

                                                 <%--<asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# Eval("Status").ToString() !="New" %>'>--%>
                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# Eval("Status").ToString() =="Edit" %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%#Eval("i_Project_ID")+","+Eval("s_Display_Project_ID") %>'
                                                        OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_Project_ID")) %>'
                                                        CommandName="cmdDelete" runat="server">
                                                            <img title="Delete Ethics Detail" alt="" src="../images/icon-delete.png">
                                                    </asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <%--<asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# Eval("Status").ToString() !="New" && Eval("cordinatorstatus").ToString() !="New" %>'>--%>
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server"  Visible='<%# "View,Edit".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ViewLink" OnCommand="ViewLink_Command" CommandArgument='<%#Eval("i_Project_ID")+","+Eval("s_Display_Project_ID") %>' runat="server">
                                                        <img title="View Selected Project Details" alt="" src="images/icon-view.png">
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
        <asp:Button ID="btnAddNewProject" OnClientClick="AddNewProject();return false;" CssClass="action" Style="margin-top: 20px; margin-bottom: 30px;" runat="server" Text="Add New Project" />
         <asp:Button ID="delete" Style="display: none" runat="server" OnClick="delete_Click" />
    </div>

    <div class="container SelectedContainer" id="SelectedContainer" runat="server" visible="false">
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Selected Project Details <span>Selected Project Details Form ( <b runat="server" id="bProjectID">Project ID: ####</b> )</span></h1>
            </div>
            <div class="col-md-6 col-sm-6">
                <p class="align-right" style="margin-top: 65px;">
                    <asp:LinkButton CssClass="link" OnClick="backToGrid_Click" ID="backToGrid" runat="server">Back To View</asp:LinkButton>
                </p>
            </div>

        </div>

        <div class="frmProject">
            <!-- Project Details -->
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



                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Project Category <b>*</b></label>
                            <asp:DropDownList CssClass="ctlselect" ID="ddlProjectCategory" disabled="disabled" title="Project Category" runat="server" TabIndex="2"></asp:DropDownList>
                        </p>

                        <p>
                            <label>Short Title</label>
                            <asp:TextBox ID="txtShortTitle" CssClass="ctlinput len100" title="Short Title" runat="server" TabIndex="7"></asp:TextBox>
                        </p>
                        <p>
                            <label>Alias 2</label>
                            <asp:TextBox ID="txtAlias2" CssClass="ctlinput len150" title="Alias 2" runat="server" TabIndex="5"></asp:TextBox>
                        </p>

                    </div>
                </div>
            </div>
            <!--End of  Project Details -->

            <!-- PI details-->
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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                            <label>PI Email <b>*</b></label>
                            <asp:TextBox ID="txtPIEmail" CssClass="ctlinput Email Req" disabled="disabled" title="PI Email" runat="server"></asp:TextBox>
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
                            <label>PI MCR No.<b>*</b></label>
                            <asp:TextBox ID="txtPiMCRNo" CssClass="ctlinput Req" disabled="disabled" title="PI MCR No." runat="server"></asp:TextBox>
                        </p>
                        <p style="text-align: right; padding-right: 112px;">
                            <label>&nbsp;</label>
                            <asp:Button ID="Button3" CssClass="action" OnClientClick="SaveMorePiClick(); return false;" runat="server" Text="Save" />
                            <asp:Button ID="Button4" CssClass="action" OnClientClick="ClearMorePi(); return false;" runat="server" Text="Reset" />

                        </p>
                    </div>
                </div>
            </div>
            <!-- End of PI Details -->
            <!--Co ordinator details -->
            <div id="cordinatorContainer" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <h3 class="frmHead" data-frm="frmcoordinator">Co-Ordinator Details <span>( - )</span></h3>
                    </div>
                </div>
                <div class="frm frmcoordinator" style="display: block;">
                    <div class="row">
                        <div class="col-md-6 col-sm-6">

                            <p>
                                <label>Team Needed <b>*</b></label>
                                <asp:DropDownList ID="ddlTeamNeeded" CssClass="ctlselect Req" title="Team Needed" TabIndex="1" runat="server">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </p>
                            <p>
                                <label>Blinded Co-Ordinator <b>*</b></label>
                                <asp:DropDownList ID="ddlBlindedCordinator" CssClass="ctlselect Req" title="Blinded Co-Ordinator" TabIndex="1" runat="server"></asp:DropDownList>

                            </p>
                            <p>
                                <label>Back Up Co-Ordinators (Blinded) <b>*</b></label>
                                <%--<asp:CheckBoxList ID="cblBackupBlindedCordinator" CssClass="ctlselect Req" title="Back Up Co-Ordinators (Blinded)" runat="server"></asp:CheckBoxList>--%>
                                <asp:HiddenField runat="server" ID="HdnCoordinatorIdBlinded" />
                                <asp:HiddenField ID="HdnCoordinatorTextBlinded" runat="server" />

                                <asp:TextBox ID="SearchBlinded" CssClass="ctlinput" placeholder="Back Up Co-Ordinators (Blinded)" ReadOnly="true" runat="server"></asp:TextBox>
                                <div runat="server" id="CheckBoxListDivBlinded" class="CheckboxList">
                                    <asp:CheckBoxList ID="chkboxlistBlinded" CssClass="ctlselect" CellPadding="0" CellSpacing="0" runat="server"></asp:CheckBoxList>
                                </div>
                                <asp:PopupControlExtender ID="Pexd" PopupControlID="CheckBoxListDivBlinded" Position="Bottom" TargetControlID="SearchBlinded" runat="server"></asp:PopupControlExtender>


                            </p>
                            <p>
                                <label>Updated By </label>
                                <asp:TextBox ID="txtUpdatedBy" CssClass="ctlinput" title="Updated By" TabIndex="13" runat="server"></asp:TextBox>
                            </p>


                        </div>
                        <div class="col-md-6 col-sm-6">
                            <p>
                                <label>Selected Project Start Date <b>*</b></label>
                                <asp:TextBox ID="txtSelectedStartDate" CssClass="ctlinput ctlinput-sm datepicker Req" title="Selected Project Start Date" TabIndex="10" runat="server"></asp:TextBox>

                            </p>
                            <p>
                                <label>UnBlinded Co-Ordinator <b>*</b></label>
                                <asp:DropDownList ID="ddlUnBlindedCordinator" CssClass="ctlselect Req" title="Blinded Co-Ordinator" TabIndex="1" runat="server"></asp:DropDownList>


                            </p>
                            <p>
                                <label>Back Up Co-Ordinators (UnBlinded) <b>*</b></label>
                                <%--<asp:CheckBoxList ID="cblBackupUnBlindedCordinator" CssClass="ctlselect Req" title="Back Up Co-Ordinators (UnBlinded)" runat="server"></asp:CheckBoxList>--%>
                                <asp:HiddenField runat="server" ID="HdnCoordinatorIdUnBlinded" />
                                <asp:HiddenField ID="HdnCoordinatorTextUnBlinded" runat="server" />

                                <asp:TextBox ID="SearchUnBlinded" CssClass="ctlinput" placeholder="Back Up Co-Ordinators (UnBlinded)" ReadOnly="true" runat="server"></asp:TextBox>
                                <div runat="server" id="CheckBoxListDivUnBlinded" class="CheckboxList">
                                    <asp:CheckBoxList ID="chkboxlistUnBlinded" CssClass="ctlselect" CellPadding="0" CellSpacing="0" runat="server"></asp:CheckBoxList>
                                </div>
                                <asp:PopupControlExtender ID="PopupControlExtender1" PopupControlID="CheckBoxListDivUnBlinded" Position="Bottom" TargetControlID="SearchUnBlinded" runat="server"></asp:PopupControlExtender>

                            </p>
                            <p>
                                <label>Date Last Updated/Reviewed</label>
                                <asp:TextBox ID="txtDateUpdated" CssClass="ctlinput ctlinput-sm datepicker" TabIndex="1" title="Date Last Updated/Reviewed" runat="server"></asp:TextBox>

                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <!--Project Requirements -->
            <div id="ProjectRequirementContainer" class="frmprojrequirement" runat="server" visible="false" >
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmProjReq">Project Requirements <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmProjReq" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>Clinic 1</label>
                            <asp:TextBox ID="txtClinic1" CssClass="ctlinput" title="Clinic 1" TabIndex="1" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Clinic Days For Research</label>
                            <asp:TextBox ID="txtClinicDaysResearch" CssClass="ctlinput" title="Clinic Days For Research" TabIndex="1" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Recruitment Start Date </label>
                            <asp:TextBox ID="txtRecruitStartDate" CssClass="ctlinput ctlinput-sm datepicker" title="Recruitment Start Date" TabIndex="24" runat="server"></asp:TextBox>
                        </p>

                        <!--Target -->

                        <p>
                            <label>Target for TTSH <b>*</b></label>
                            <asp:TextBox ID="txtTargetforTTSH" Text="1" CssClass="ctlinput Req Range1-9999" title="Target for TTSH" TabIndex="1" runat="server"></asp:TextBox>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>Clinic 2 </label>
                            <asp:TextBox ID="txtClinic2" CssClass="ctlinput" title="Clinic 2" TabIndex="1" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Duration of  follow Ups </label>
                            <asp:TextBox ID="txtDurationofFollowups" CssClass="ctlinput" TabIndex="1" title="Duration of  follow Ups" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Recruitment End Date </label>
                            <asp:TextBox ID="txtRecruitEndDate" CssClass="ctlinput ctlinput-sm datepicker" title="Recruitment End Date" TabIndex="24" runat="server"></asp:TextBox>
                        </p>
                    </div>
                </div>
            </div>
                </div>

            <!--End Project Requirements -->

            <!--End of Co ordinator details -->
            <div id="MonthlyDeatailContainer" runat="server" visible="false">
                <div class="row" style="margin-bottom: 30px;">
                    <div class="col-md-12">
                        <h3 class="frmHead" data-frm="frmmonthly">Monthly Details <span>( - )</span></h3>
                    </div>
                </div>
                <div class="frm frmmonthly tabs" style="display: block;">

                    <div class="row">
                        <div class="col-md-12 tab-02">
                            <label>Select Year and Month</label>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-md-1 tab-02">

                            <asp:DropDownList ID="ddlYear" CssClass="ctlselectCustom tabDate" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" title="Year" TabIndex="1" runat="server">
                            </asp:DropDownList>
                        </div>

                        <!-- -->
                        <div class="col-md-11 tab-02">
                            <asp:UpdatePanel ID="updtPnlMonthContainer" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div runat="server" id="monthButton_Container">
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlYear" EventName="SelectedIndexChanged" />

                                </Triggers>
                            </asp:UpdatePanel>
                            <!-- -->
                        </div>
                        <asp:Button ID="btnMonthTrigger" runat="server" OnCommand="month_Click_Command" Style="display: none" CommandArgument="abc" month="Jan" Text="Button" />

                        <div class="col-md-12">
                            <!-- -->
                        </div>
                    </div>
                    <asp:UpdatePanel ID="updtPnlMonthlyDetails" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div id="BlindedContainer" runat="server" visible="false">

                                <!--SAE details -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmsae">SAE Details <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmsae" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">

                                            <p>
                                                <label>SAE Status (Internal) <b>*</b></label>
                                                <asp:DropDownList ID="ddlSAEStatus" CssClass="ctlselect Req" title="SAE Status (Internal)" TabIndex="1" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </p>
                                            <p>
                                                <label>Mode of Notification <b>*</b></label>
                                                <asp:DropDownList ID="ddlModeofNotification" CssClass="ctlselect Req" title="Mode of Notification" TabIndex="1" runat="server"></asp:DropDownList>


                                            </p>
                                            <p>
                                                <label>Readmission Date <b>*</b></label>
                                                <asp:TextBox ID="txtReadmissionDate" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="1" title="Readmission Date" runat="server"></asp:TextBox>

                                            </p>
                                            <p>
                                                <label>Date of Coordinators Knowledge <b>*</b></label>
                                                <asp:TextBox ID="txtdtCordinatorsKnowledge" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="1" title="Date of Coordinators Knowledge" runat="server"></asp:TextBox>
                                            </p>


                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Patient Study No </label>
                                                <asp:TextBox ID="txtPatientStudyNo" CssClass="ctlinput" title="Patient Study No" TabIndex="1" runat="server"></asp:TextBox>

                                            </p>
                                            <p>
                                                <label>Readmission Samediscipline <b>*</b></label>
                                                <asp:DropDownList ID="ddlReadmission" CssClass="ctlselect Req" title="Readmission Samediscipline" TabIndex="1" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>


                                            </p>
                                            <p>
                                                <label>Discharged Date <b>*</b></label>
                                                <asp:TextBox ID="txtDischargeDate" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="14" title="Discharged Date" runat="server"></asp:TextBox>

                                            </p>

                                        </div>
                                    </div>
                                </div>
                                <!--End of SAE details -->

                                <!--CRO and CRA Details -->
                                <div class="row">
                                    <div class="col-md-12 col-sm-12">
                                        <h3 class="frmHead" data-frm="frmCRO">CRO and CRA Details <span>( - )</span>
                                        </h3>
                                        <p><span>+</span> <a class="newCRO link" data-frm="frmNewCRODetails">Record New CRO</a></p>

                                    </div>
                                </div>

                                <div class="frm frmCRO" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12">
                                            <div class="tblResposiveWrapper">
                                                <table class="tblResposive" id="tblCRODetail">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 450px; text-align: left">CRO</th>
                                                            <th style="text-align: left">CRA Name</th>
                                                            <th style="text-align: left">CRA Email Address</th>
                                                            <th style="text-align: left">CRA Phone No.</th>

                                                            <th style="width: 45px; text-align: right">Action</th>
                                                        </tr>
                                                    </thead>

                                                    <tbody>
                                                        <asp:Repeater ID="rptrCRODetails" runat="server">
                                                            <ItemTemplate>
                                                                <tr croid='<%# Eval("CROID") %>' craid='<%# Eval("CRAID") %>'>
                                                                    <td data-th="Department">
                                                                        <p><%# Eval("CRO") %></p>
                                                                    </td>
                                                                    <td data-th="PI Name">
                                                                        <p><%# Eval("CRA") %></p>
                                                                    </td>
                                                                    <td data-th="Email">
                                                                        <p><%# Eval("Email") %></p>
                                                                    </td>
                                                                    <td data-th="Phone">
                                                                        <p><%# Eval("Phone") %></p>
                                                                    </td>

                                                                    <td data-th="Action">
                                                                        <p class="grid-action">
                                                                            <a>
                                                                                <img title="Delete" alt="" onclick="delCRORows(this)" src="images/icon-delete.png"></a>

                                                                        </p>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </tbody>
                                                </table>
                                            </div>
                                            <p class="align-right"><a class="link" onclick="AddMoreCRO();">+ Add More CRA</a></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="frmNewCRODetails" style="display: none;">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12">
                                            <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Record New CRO				                  
                                            </h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12 col-sm-12">
                                                    <p>
                                                        <label>CRO Name <b>*</b></label>
                                                        <asp:TextBox ID="txtCROName" CssClass="ctlinput-sm Req" Style="width: 444px;" title="CRO Name" runat="server"></asp:TextBox>


                                                        <asp:Button ID="btnSaveNewCRO" CssClass="action" OnClick="btnSaveNewCRO_Click" OnClientClick="if(!SaveNewCRO()){return false;};" runat="server" Text="Save" />
                                                        <asp:Button ID="btnCancelNewCRO" CssClass="action" OnClientClick="ClearNewCRO(); return false;" runat="server" Text="Cancel" />
                                                    </p>

                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSaveNewCRO" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="frmAddMoreCRODetails" style="display: none;">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12">
                                            <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Add CRA					                  
                                            </h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <asp:UpdatePanel ID="updPnlCRO" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-6 col-sm-6">
                                                    <p>
                                                        <label>CRO <b>*</b></label>
                                                        <asp:TextBox ID="txtCROSearch" CssClass="ctlinput Req autoComp" hdnparam="HdnCROId" title="CRO" placeholder="Type Keyword to search CRO" runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="HdnCROId" runat="server" />
                                                    </p>
                                                    <p>
                                                        <label>CRA Email </label>
                                                        <asp:TextBox ID="txtCRAEmail" CssClass="ctlinput Email" title="CRA Email" runat="server"></asp:TextBox>
                                                    </p>


                                                </div>
                                                <div class="col-md-6 col-sm-6">
                                                    <p>
                                                        <label>CRA Name <b>*</b></label>
                                                        <asp:TextBox ID="TxtCRAName" CssClass="ctlinput Req" title="CRA Name" runat="server"></asp:TextBox>

                                                    </p>
                                                    <p>
                                                        <label>CRA Phone No.</label>
                                                        <asp:TextBox ID="txtCRAPhoneNo" onkeypress="return SingaporePhformat();" onpaste="return false;" CssClass="ctlinput" title="CRA Phone No." runat="server"></asp:TextBox>
                                                    </p>
                                                    <p style="text-align: right; padding-right: 112px;">
                                                        <label>&nbsp;</label>
                                                        <asp:Button ID="SaveMoreCRO" CssClass="action" OnClientClick="if( !SaveMoreCROClick()) { return false;}" OnClick="SaveMoreCRO_Click" runat="server" Text="Add" />
                                                        <asp:Button ID="Button2" CssClass="action" OnClientClick="ClearMoreCRO(); return false;" runat="server" Text="Reset" />

                                                    </p>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="SaveMoreCRO" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <!--End of CRO and CRA Details -->

                                <!--Study details -->
                                <asp:HiddenField ID="hdnBudgetFiles" runat="server" />
                                <asp:HiddenField ID="hdnBudgetFileNotSaved" runat="server" />

                                <asp:HiddenField ID="hdnSavedComments" runat="server" />
                                <asp:HiddenField ID="hdnUnSavedComments" runat="server" />
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmstudy">Study Section <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmstudy" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Study Status </label>
                                                <asp:DropDownList ID="ddlStudyStatus" CssClass="ctlselect" title="Study Status" TabIndex="20" runat="server">
                                                </asp:DropDownList>
                                            </p>




                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Type of Study </label>
                                                <asp:DropDownList ID="ddlTypeofStudy" CssClass="ctlselect" title="Type of Study" TabIndex="20" runat="server">
                                                </asp:DropDownList>
                                            </p>


                                        </div>




                                    </div>
                                    <%--   <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Study Budget Document  <b>*</b></label>
                                                <input type="text" class="txtStudyBudgetDocument" readonly="true" id="txtInsuranceFile" />
                                                <span class="btn btn-default btn-file action">Browse...
                            <asp:FileUpload ID="fuStudyBudgetDocument" title="Study Budget Document" CssClass="action" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:LinkButton ID="btnDownStudyBudgetDocument" Visible="false" runat="server"></asp:LinkButton>
                                                </label>

                                            </p>
                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Comments for Budget</label>
                                                <asp:TextBox ID="txtBudgetComments" CssClass="ctlinput" title="Comments for Budget" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                    </div>--%>
                                </div>
                                <!--End of Study  details -->

                                <!-- Archiving - Section details -->
                                <div class="row archivingSection">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmarchiving">Archiving - Section <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmarchiving archivingSection" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Awaiting Archiving <b>*</b></label>
                                                <asp:DropDownList ID="ddlAwaitingArchiving" CssClass="ctlselect Req" title="Awaiting Archiving" TabIndex="1" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </p>

                                            <p>
                                                <label>Date Sent for Archiving <b>*</b></label>
                                                <asp:TextBox ID="txtDateSentForArchiving" CssClass="ctlinput ctlinput-sm datepicker" Enabled="false" TabIndex="1" title="Date Sent for Archiving" runat="server"></asp:TextBox>
                                            </p>

                                            <p>
                                                <label>Off site Company <b>*</b></label>
                                                <asp:TextBox ID="txtOffSiteCompany" CssClass="ctlinput" Enabled="false" title="Off site Company" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>



                                            <p>
                                                <label>Agreement Number</label>
                                                <asp:TextBox ID="txtAgreementNumber" CssClass="ctlinput" title="Agreement Number" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>

                                            <p>
                                                <label>Any Approved Study Budget </label>
                                                <asp:DropDownList ID="ddlApprovedStudyBugdet" CssClass="ctlselect" TabIndex="1" title="Any Approved Study Budget" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>

                                            </p>



                                        </div>
                                        <div class="col-md-6 col-sm-6">

                                            <p>
                                                <label>Reason <b>*</b></label>
                                                <asp:TextBox ID="txtReason" CssClass="ctlinput" Enabled="false" title="Reason" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>


                                            <p>
                                                <label>End Date of Archiving <b>*</b></label>
                                                <asp:TextBox ID="txtEndDateArchiving" CssClass="ctlinput ctlinput-sm datepicker" Enabled="false" TabIndex="1" title="End Date of Archiving" runat="server"></asp:TextBox>
                                            </p>

                                            <p>
                                                <label>Number of Boxes</label>
                                                <asp:TextBox ID="txtNumberOfBoxes" CssClass="ctlinput" title="Number of Boxes" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>



                                            <p>
                                                <label>Agreement File </label>

                                                <input type="text" class="txtUpload" readonly="true" id="txtIRBFile" />

                                                <span class="btn btn-default btn-file action">Browse...
                                                    <asp:FileUpload ID="fuIRBFile" title="DSRB/IRB File" CssClass="action" Enabled="true" runat="server" />
                                                </span>
                                                <label>
                                                    <asp:LinkButton ID="btnDownIRBFile" Visible="false" OnClientClick="DownloadAgreementFile(this);return false;" OnClick="btnDownIRBFile_Click" runat="server"></asp:LinkButton>
                                                </label>



                                            </p>

                                            <p>
                                                <label>Amount</label>
                                                <asp:TextBox ID="txtAmount" CssClass="ctlinput" title="Amount" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                        </div>

                                        <!--- +++++++++++++++++++-->

                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <h3 class="frmHead" data-frm="frmStatusReportFile">Study Budget Document File    <span>( - )</span></h3>
                                            <p runat="server" id="P3"><span>+</span>  <a class="MonlyStatusRpt link" data-frm="frmMonthlyStatusReport">Add Study Budget Document </a></p>
                                        </div>
                                    </div>
                                    <div class="frm frmStatusReportFile" style="display: block;">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12">
                                                <div class="tblResposiveWrapper">
                                                    <table id="tblStatusMontly" class="tblResposive removeHover">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 500px">Study Budget Document</th>
                                                                <th>Comments for Budget</th>
                                                                <th style="width: 45px; text-align: right">Action</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptrBudgetFile" runat="server">
                                                                <ItemTemplate>
                                                                    <tr id="Tr1" runat="server" class="''" filepath='<%# Eval("s_Budget_Document_File") %>'>
                                                                        <td class="fileName">
                                                                            <p>
                                                                                <a onclick="DownloadBudgetFile(this)"><%# Eval("Name") %> </a>
                                                                            </p>
                                                                        </td>
                                                                        <td class="fileComment">
                                                                            <p><%# Eval("s_Budget_Comments")%></p>
                                                                        </td>
                                                                        <td data-th="Action">
                                                                            <p class="grid-action">

                                                                                <a>
                                                                                    <img title="delete" alt="Delete" onclick="return delBudgetFile(this);" src="../images/icon-delete.png"></a>

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

                                    <div class="frmMonthlyStatusReport" style="display: none;">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12">
                                                <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New Study Budget Document				                  
                </h3>
                                            </div>



                                            <div class="col-md-6 col-sm-6 BudgetFile" runat="server" id="BudgetFileContainer">

                                                <p class="BudgetFile1">
                                                    <label>Study Budget Document <b>*</b></label>


                                                    <asp:TextBox ID="txtBudgetFile1" class="txtUpload" runat="server"></asp:TextBox>
                                                    <span class="btn btn-default btn-file action">Browse...
                                                        <asp:FileUpload ID="fuBudgetFile1" onchange="FloadChange(this)" class="action" runat="server" />
                                                    </span>

                                                </p>



                                            </div>
                                            <div class="col-md-6 col-sm-6 BudgetComment" runat="server" id="BudgetCommentContainer">
                                                <p>
                                                    <label>Comments for Budget</label>
                                                    <asp:TextBox ID="txtBugdetComments1" title="Comments for Budget" class="ctlinput" runat="server"></asp:TextBox>
                                                </p>

                                            </div>



                                            <%--<div class="col-md-12 col-sm-12">--%>

                                                <p class="align-right">
                                                    <%--<asp:LinkButton ID="btnAddMoreBudgetFile"  OnClick="btnAddMoreBudgetFile_Click" style="margin-right: 112px" runat="server">+ Add More</asp:LinkButton>--%>
                                                    <a class="link" onclick="AddMoreBudgetFile();">+ Add More</a>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="row margin-top frmAction">
                                            <div class="col-md-12">
                                                <p style="text-align: right">


                                                    <asp:Button ID="Button1" CssClass="action" OnClientClick="return SaveBudgetFile();" runat="server" Text="Add" />
                                                    <asp:Button ID="Button5" CssClass="action" runat="server" OnClientClick="return ResetBudgetFile();" Text="Cancel" />
                                                   
                                                </p>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- ++++++++++++++++++++++++++++++++-->
                                </div>
                                <!--End of  Archiving - Section  details -->

                                <!--Other details -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmother">Other Details <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmother" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">

                                            <p>
                                                <label>Screen Failure <b>*</b></label>
                                                <asp:TextBox ID="txtScreenFailure" Text="0" CssClass="ctlinput Req Range0-9999" title="Screen Failure" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Completed <b>*</b></label>
                                                <asp:TextBox ID="txtCompleted" Text="0" CssClass="ctlinput Req Range0-9999" title="Completed" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                             <p>
                                                <label>CTC Expiry Date </label>
                                                <asp:TextBox ID="txtCTCExpiryDate" CssClass="ctlinput ctlinput-sm" Enabled="false" title="CTC Expiry Date " TabIndex="1" runat="server"></asp:TextBox>
                                            </p>

                                            <!--End Target -->

                                            <!-- DRB/IRB-->
                                            <p>
                                                <label>DSRB/IRB #  </label>
                                                <asp:TextBox ID="txtIRB" CssClass="ctlinput" Enabled="false" title="DSRB/IRB #" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>

                                            <p>
                                                <label>CTM Status</label>
                                                <asp:DropDownList ID="ddlCTMStatus" CssClass="ctlselect" TabIndex="1" title="CTM Status" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Applicable" Selected="True" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Not-Applicable" Value="0"></asp:ListItem>
                                                </asp:DropDownList>


                                            </p>
                                           
                                            <!--End of DRB/IRB-->



                                        </div>
                                        <div class="col-md-6 col-sm-6">

                                            <!--Target -->
                                            <p>
                                                <label>Screened # (Signed Informed Consent Form)  <b>*</b></label>
                                                <asp:TextBox ID="txtScreened" Text="0" CssClass="ctlinput Req Range0-9999" title="Screened # (Signed Informed Consent Form)" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Randomized<b> *</b></label>
                                                <asp:TextBox ID="txtRandomized" Text="1" CssClass="ctlinput Req Range0-9999" title="Randomized" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Withdrawal  <b>*</b></label>
                                                <asp:TextBox ID="txtWithdrawal" Text="0" CssClass="ctlinput Req Range0-9999" title="Withdrawal" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                            <!--End Target -->

                                            <!-- DRB/IRB-->
                                            <p>
                                                <label>DSRB/IRB Expiry Date <b>*</b></label>
                                                <asp:TextBox ID="txtIRBExpiryDate" CssClass="ctlinput ctlinput-sm datepicker Req" title="DSRB/IRB Expiry Date" TabIndex="1" runat="server"></asp:TextBox>

                                            </p>
                                            <p>

                                                <label>CTM Expiry Date  <b>*</b></label>
                                                <asp:TextBox ID="txtCTMExpiryDate" CssClass="ctlinput ctlinput-sm Req" title="CTM Expiry Date" TabIndex="1" runat="server"></asp:TextBox>

                                            </p>
                                            <!--End of DRB/IRB-->





                                        </div>
                                    </div>
                                </div>
                                <!--End of Other  details -->

                                <!--Cupboard-Section details(blinded) -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmcupblinded">Cupboard - Section (Blinded) <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmcupblinded" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Expected Month when study will end <b>*</b></label>
                                                <asp:TextBox ID="txtExpectedMonth" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="1" title="" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Updated By </label>
                                                <asp:TextBox ID="txtUpdatedByBlinded" CssClass="ctlinput" Enabled="false" title="Updated By" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Cup Board number <b>*</b></label>

                                                <asp:DropDownList ID="ddlCupboardNoBlinded" CssClass="ctlselect Req" title="Cup Board number" TabIndex="1" runat="server"></asp:DropDownList>
                                            </p>
                                            <p>
                                                <label>Date Last Updated/Reviewed </label>
                                                <asp:TextBox ID="txtLastUpdatedBlinded" CssClass="ctlinput ctlinput-sm" Enabled="false" TabIndex="14" title="Date Last Updated/Reviewed" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <!--End of Cupboard-Section  details -->
                            </div>

                            <div id="UnBlindedContainer" runat="server" visible="false">
                                <!--Drug Location-Section  -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmdrugloc">Drug Location - Section  <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmdrugloc" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Name <b>*</b></label>
                                                <asp:TextBox ID="txtDrugName" CssClass="ctlinput Req" TabIndex="1" title="Name" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Dose <b>*</b></label>
                                                <asp:TextBox ID="txtDose" CssClass="ctlinput Req" TabIndex="14" title="Dose" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                        <div class="col-md-6 col-sm-6">

                                            <p>
                                                <label>Date of expiry <b>*</b></label>
                                                <asp:TextBox ID="txtDateofExpiry" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="14" title="Date of expiry" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Location <b>*</b></label>
                                                <asp:DropDownList ID="ddlLocation" CssClass="ctlselect Req" title="Location" TabIndex="9" runat="server"></asp:DropDownList>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <!--End of Drug Location-Section -->

                                <!--Cupboard-Section details(Unblinded) -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <h3 class="frmHead" data-frm="frmcupunblinded">Cupboard - Section (Un Blinded) <span>( - )</span></h3>
                                    </div>
                                </div>
                                <div class="frm frmcupunblinded" style="display: block;">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Expected Month when study will end <b>*</b></label>
                                                <asp:TextBox ID="txtExpectedMonthUnBlinded" CssClass="ctlinput ctlinput-sm datepicker Req" TabIndex="1" title="Expected Month when study will end" runat="server"></asp:TextBox>
                                            </p>
                                            <p>
                                                <label>Updated By <b>*</b></label>

                                                <asp:TextBox ID="txtUpdatedByUnblinded"  CssClass="ctlinput Req" title="Updated By" Enabled="false" TabIndex="1" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                        <div class="col-md-6 col-sm-6">
                                            <p>
                                                <label>Cup Board number <b>*</b></label>
                                                <asp:DropDownList ID="ddlCupboardNoUnblinded" CssClass="ctlselect Req" title="Cup Board number" TabIndex="9" runat="server"></asp:DropDownList>
                                            </p>
                                            <p>
                                                <label>Date Last Updated/ Reviewed </label>
                                                <asp:TextBox ID="txtLastUpdatedUnBlinded" CssClass="ctlinput ctlinput-sm" Enabled="false" TabIndex="14" title="Date Last Updated/ Reviewed" runat="server"></asp:TextBox>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <!--End of Cupboard-Section  details(Unblinded)  -->

                            </div>

                            <!--Entry for month datepicker -->
                            <div class="row">
                                <div class="col-md-6 col-sm-6">

                                    <p>
                                        <label>Entry for Month <b>*</b></label>
                                        <asp:TextBox ID="txtEntryMonth" CssClass="ctlinput ctlinput-sm datepickerMonth Req" TabIndex="14" title="Entry for Month" runat="server"></asp:TextBox>
                                    </p>
                                </div>
                            </div>
                            <!--End Entry for month datepicker -->



                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlYear" />
                            <asp:AsyncPostBackTrigger ControlID="btnMonthTrigger" />

                        </Triggers>
                    </asp:UpdatePanel>





                </div>
            </div>
        </div>
        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p align="right">

                    <asp:Button CssClass="action" ID="btnUpdateSelected" Visible="false" runat="server" Text="Update Details" TabIndex="29" OnClientClick="if( !UpdateClick()){return false;}" OnClick="btnUpdateSelected_Click" />
                    <asp:Button CssClass="action" ID="btnSaveSelected" runat="server" Text="Save Details" TabIndex="30" OnClientClick="if( !SaveClick()){return false;}" OnClick="btnSaveSelected_Click" />
                    <asp:Button CssClass="action" ID="btnCancelSelected" runat="server" OnClick="btnCancelSelected_Click" Text="Cancel" TabIndex="31" />

                </p>
            </div>
        </div>

    </div>
    <div class="hiddenFields">
        <asp:HiddenField ID="HdnPi_ID" runat="server" />
        <asp:HiddenField ID="HdnpiId" runat="server" />
        <asp:HiddenField ID="HdnProjectID" runat="server" />
        <asp:HiddenField ID="hdnCROCRAIDs" runat="server" />
        <asp:HiddenField ID="hdnSelectedID" runat="server" />

        <asp:HiddenField ID="hdnDisplayMode" runat="server" />
    </div>
</asp:Content>
