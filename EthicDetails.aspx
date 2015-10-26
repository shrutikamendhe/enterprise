<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="EthicDetails.aspx.cs" Inherits="TTSHWeb.EthicDetails" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Ethics.css" rel="stylesheet" />

    <script src="Scripts/Ethics.js"></script>
    <script src="Scripts/validation.js"></script>
    <link href="../css/tableSorter.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HdnId" Value="0" runat="server" />
    <div class="projectGrid container" runat="server" id="projectGrid">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Ethic Details View <span>Search, Filter and Edit Ethic Details</span></h1>
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
                                <th width="150">Project Category</th>
                                <th width="100">Ethic Status</th>
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
                                        <td data-th="Ethic Status">
                                            <p><%#Eval("EthicsStatus") %></p>
                                        </td>
                                        <td data-th="DSRB?IRB No.">

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_Names") %></p>
                                        </td>
                                        <td data-th="Action">
                                            <p class="grid-action">

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# "New".Contains(Eval("Status").ToString()) %>'>

                                                    <asp:LinkButton ID="NewLink" OnCommand="NewLink_Command" CommandArgument='<%#Eval("i_ID") %>' runat="server"><img title="Add Ethics Details" alt="" src="images/Add-New.png"></asp:LinkButton>


                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# "Edit".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="EditLink" OnCommand="EditLink_Command" CommandArgument='<%#Eval("Ethics_ID") %>' runat="server">
                                                        <img title="Edit Ethics Details" alt="" src="images/icon-edit.png"> 
                                                    </asp:LinkButton>

                                                </asp:PlaceHolder>
                                                 <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Ethics_ID")%>' Visible='<%# "Edit".Contains(Eval("Status").ToString()) %>'
                                                    OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("Ethics_ID")) %>'
                                                    CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Ethics Detail" alt="" src="../images/icon-delete.png">
                                                </asp:LinkButton>
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# "Edit,View".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ViewLink" OnCommand="ViewLink_Command" CommandArgument='<%#Eval("Ethics_ID") %>' runat="server">
                                                        <img title="View Ethics Details" alt="" src="images/icon-view.png">
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

    <div class="container EthicsContainer" id="EthicsContainer" runat="server" visible="false">

        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Ethic Details <span>Ethic Details Form ( <b runat="server" id="bProjectID">Project ID: ####</b> )</span></h1>
            </div>
            <div class="col-md-6 col-sm-6">
                <p class="align-right" style="margin-top: 65px;">
                    <asp:LinkButton CssClass="link" OnClick="backToGrid_Click" ID="backToGrid" runat="server">Back To View</asp:LinkButton>
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
                            <label>Alias 2</label>
                            <asp:TextBox ID="txtAlias2" CssClass="ctlinput len150" title="Alias 2" runat="server" TabIndex="5"></asp:TextBox>
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
                        <%--<p>
                            <label>Project Status <b>*</b></label>
                            <asp:DropDownList ID="ddlProjectStatus" CssClass="ctlselect Req" title="Project Status" TabIndex="4" runat="server"></asp:DropDownList>

                        </p>--%>
                         <p>
                            <label>Ethics Start Date <b>*</b></label>
                            <asp:TextBox ID="txtEthicsStartDate" CssClass="ctlinput ctlinput-sm datepicker Req" title="Ethics Start Date" TabIndex="8" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Ethics End Date<b>*</b></label> 
                            <asp:TextBox ID="dtCompleted_Withdrawn" CssClass="ctlinput ctlinput-sm datepicker Req" title="Date Terminated/Withdrawn/Completed" TabIndex="6" runat="server"></asp:TextBox>

                        </p>
                       
                        <p>
                            <label>Ethics Review <b>*</b></label>
                            <asp:DropDownList ID="ddlEthicsReview" CssClass="ctlselect Req" title="Ethics Review" TabIndex="1" runat="server"></asp:DropDownList>
                        </p>
                    </div>
                </div>
            </div>



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
                            <div class="col-md-12 col-sm-12" style="text-align:right;padding-right:127px;">
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
                        <p  style="text-align:right;padding-right:112px;">
                            <label>&nbsp;</label>
                            <asp:Button ID="Button1" CssClass="action" OnClientClick="SaveMorePiClick(); return false;" runat="server" Text="Save" />
                            <asp:Button ID="Button2" CssClass="action" OnClientClick="ClearMorePi(); return false;" runat="server" Text="Reset" />

                        </p>
                    </div>
                </div>
            </div>




            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmIRB">DSRB/IRB Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmIRB" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Type of IRB <b class="bIRB">*</b></label>
                            <asp:DropDownList ID="ddlIRBType" CssClass="ctlselect Req" title="Type of IRB" TabIndex="9" runat="server"></asp:DropDownList>

                        </p>
                        <p>
                            <label>DSRB/IRB Status <b class="bIRBStatus">*</b></label>
                            <asp:DropDownList ID="ddlIRBStatus" CssClass="ctlselect Req" title="DSRB/IRB Status" TabIndex="11" runat="server"></asp:DropDownList>

                        </p>
                        <p>
                            <label>DSRB/IRB # <b>*</b></label>
                            <asp:TextBox ID="txtIRBNumber" CssClass="ctlinput" title="DSRB/IRB #" TabIndex="13" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>DSRB/IRB Approved Date <b>*</b></label>
                            <asp:TextBox ID="txtIRBApproveDate" CssClass="ctlinput ctlinput-sm datepicker" title="DSRB/IRB Approved Date" TabIndex="15" runat="server"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Comments</label>
                            <asp:TextBox ID="txtComments" CssClass="ctltext" title="Comments" TabIndex="10" TextMode="MultiLine" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>DSRB/IRB File <b>*</b></label>

                            <input type="text" class="txtUpload" readonly="true" id="txtIRBFile" />

                            <span class="btn btn-default btn-file action">Browse...
                                <asp:FileUpload ID="fuIRBFile" title="DSRB/IRB File" CssClass="action" Enabled="true" runat="server" />
                            </span>
                            <label>
                                <asp:LinkButton ID="btnDownIRBFile" Visible="false" OnClick="btnDownIRBFile1_Click" runat="server"></asp:LinkButton>
                            </label>


                        </p>
                        <p>
                            <label>DSRB/IRB# Approved End Date <b>*</b></label>
                            <asp:TextBox ID="txtIRBApproveEndDate" CssClass="ctlinput ctlinput-sm datepicker" TabIndex="14" title="DSRB/IRB# Approved End Date" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>

            <!-- Targeted and Recruited Subject Details -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmSub">Targeted and Recruited Subject Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmSub" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Subjects Targeted at TTSH </label>
                            <asp:TextBox ID="txtSubTargeted" title="Subjects Targeted at TTSH" TabIndex="16" CssClass="ctlinput ctlinput-sm Num" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Subjects Recruited by TTSH </label>
                            <asp:TextBox ID="txtSubRecruited" title="Subjects Recruited by TTSH" CssClass="ctlinput ctlinput-sm Num" TabIndex="18" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Does it require the renewal?</label>
                            <asp:DropDownList ID="ddlRenewal" CssClass="ctlselect" title="Does it require the renewal?" TabIndex="20" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Global Subject Targeted</label>
                            <asp:TextBox ID="txtGlobalSubTargeted" runat="server" CssClass="ctlinput ctlinput-sm Num" TabIndex="17" title="Global Subject Targeted"></asp:TextBox>

                        </p>
                        <p>
                            <label>Global Subject Recruited </label>
                            <asp:TextBox ID="txtGlobalSubRecruited" CssClass="ctlinput ctlinput-sm Num" title="Global Subject recruited" TabIndex="19" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>New Study End Date <b>*</b></label>
                            <asp:TextBox ID="txtNewStudyEndDate" CssClass="datepicker ctlinput ctlinput-sm" title="New Study End Date" TabIndex="21" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>
            <!-- -->

            <!-- Clinical Trial Insurance Details -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmtrial">Clinical Trial Insurance Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmtrial" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Clinical Trial Insurance <b>*</b></label>
                            <asp:DropDownList ID="ddlClinicalTrialInsurance" CssClass="ctlselect Req" TabIndex="22" title="Clinical Trial Insurance" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>
                        <p>
                            <label>Period of Insurance </label>
                           <%-- <asp:TextBox ID="txtInsurancePeriod" CssClass="ctlinput ctlinput-sm datepicker" title="Period of Insurance" TabIndex="24" runat="server"></asp:TextBox>--%>

                            <asp:DropDownList ID="ddlPeriodOfInsurance" CssClass="ctlselect" title="Period of Insurance" TabIndex="1" runat="server"></asp:DropDownList>

                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Clinical Trial Insurance File <b>*</b></label>
                            <input type="text" class="txtUpload" readonly="true" id="txtInsuranceFile" />
                            <span class="btn btn-default btn-file action">Browse...
                            <asp:FileUpload ID="fuInsuranceFile" title="Clinical Trial Insurance File" CssClass="action" runat="server" />
                            </span>
                            <label>
                                <asp:LinkButton ID="btnDownInsuranceFile" Visible="false" OnClick="btnDownInsuranceFile_Click" runat="server"></asp:LinkButton>
                            </label>

                        </p>
                    </div>
                </div>
            </div>
            <!-- -->

            <!-- CRIO Details -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmcrio">CRIO Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmcrio" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>CRIO Record Culled <b>*</b></label>
                            <asp:DropDownList ID="ddlCRIOCulled" CssClass="ctlselect Req" TabIndex="24" title="CRIO Record Culled" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>CRIO Record Culled Date<b>*</b></label>
                            <asp:TextBox ID="txtCRIOCulledDate" title="CRIO Record Culled Date" TabIndex="25" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>
            <!-- -->

            <!-- Other Details -->
            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmother">Other Details <span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmother" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Female/Childbearing </label>
                            <asp:DropDownList ID="ddlChildBearing" CssClass="ctlselect" TabIndex="26" title="Female/Childbearing" runat="server">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>

                        </p>
                        <p>
                            <label>Co-Investigator</label>
                            <asp:TextBox ID="txtCoInvestigator" CssClass="ctlinput" title="Co-Investigator" TabIndex="28" runat="server"></asp:TextBox>

                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Ethical Notes/Remarks</label>
                            <asp:TextBox ID="txtEthicalNotes" CssClass="ctltext" TextMode="MultiLine" TabIndex="27" title="Ethical Notes/Remarks" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
            </div>
            <!-- -->


            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p align="right">

                        <asp:Button CssClass="action" ID="btnUpdateEthics" Visible="false" runat="server" Text="Update Details" TabIndex="29" OnClientClick="if( !UpdateClick()){return false;}" OnClick="btnUpdateEthics_Click" />
                        <asp:Button CssClass="action" ID="btnSaveEthics" runat="server" Text="Save Details" TabIndex="30" OnClientClick="if( !SaveClick()){return false;}" OnClick="btnSaveEthics_Click" />
                        <asp:Button CssClass="action" ID="btnCancelEthics" runat="server" Text="Cancel" OnClick="btnCancelEthics_Click" TabIndex="31" />

                    </p>
                </div>
            </div>
        </div>





    </div>
    <div class="hiddenFields">
        <asp:HiddenField ID="hdnIRBFile" runat="server" />
        <asp:HiddenField ID="hdnInsuranceFile" runat="server" />
        <asp:HiddenField ID="HdnPi_ID" runat="server" />
        <asp:HiddenField ID="HdnpiId" runat="server" />
        <asp:HiddenField ID="hdnIRBFileEnabled" runat="server" />
        <asp:HiddenField ID="hdnInsuranceFileEnabled" runat="server" />
        <asp:HiddenField ID="hdnDtTerminatedEnabled" runat="server" />
        <asp:HiddenField ID="hdnNewStudyEndDtEnabled" runat="server" />
        <asp:HiddenField ID="hdnRecordCulledEnabled" runat="server" />


    </div>

</asp:Content>