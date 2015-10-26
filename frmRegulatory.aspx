<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="frmRegulatory.aspx.cs" Inherits="TTSHWeb.frmRegulatory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Tablstyle.css" rel="stylesheet" />


    <script src="Scripts/Webform/xdate.js"></script>
    <script src="Scripts/WebForm/jsRegulatory.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HdnMode" Value="Insert" runat="server" />
    <asp:HiddenField ID="HdnProjectId" Value="0" runat="server" />
    <asp:HiddenField ID="HdnRegId" Value="0" runat="server" />
    <asp:HiddenField ID="HdnStudyTeamMembersDetails" Value="" runat="server" />
    <asp:HiddenField ID="HdnPISDetails" runat="server" Value="" />
    <asp:HiddenField ID="HdnStatusReportSubmissionFileDetails" Value="" runat="server" />
    <asp:HiddenField ID="HdnAmendmentsDetails" Value="" runat="server" />
    <asp:HiddenField ID="HdnOtherDetails" Value="" runat="server" />
    <asp:HiddenField ID="RegSixMId" runat="server" />
    <asp:HiddenField ID="HdnSel" Value="0" runat="server" />
    <asp:HiddenField ID="HdnGlobPath" Value="" runat="server" />
    <asp:HiddenField ID="HdnOtherSponsor" Value="" runat="server" />
    <asp:HiddenField ID="HdnCTCStatus" Value="" runat="server" />
    <asp:HiddenField ID="HdnExtendedStatus" Value="" runat="server" />
    <div class="RegulatoryGrid container" runat="server" id="DivMain">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Regulatory Detail View <span>Search, Filter and Edit Regulatory Details</span></h1>

            </div>
            <div class="col-md-6 col-sm-6 paging">
                <div class="grid-search">
                    <uc1:searchbox runat="server" id="SearchBox" />
                </div>
            </div>
        </div>

        <div class="row">



            <div class="col-md-12">
                <div class="tblResposiveWrapper">
                    <table id="tblResposive">
                        <thead>
                            <tr>
                                <th style="width: 100px">Project ID</th>
                                <th>Project Title</th>
                                <th>Project Category</th>

                                <th>CTC Status</th>
                                <th>DSRB/IRB No.</th>
                                <th>PI Name</th>

                                <th style="width: 95px">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="RptsRegulatoryGrid" OnItemCommand="RptsRegulatoryGrid_ItemCommand" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td data-th="Project ID">
                                            <p><%#Eval("s_Display_Project_ID") %></p>
                                        </td>
                                        <td data-th="Project Title">
                                            <p><%#Eval("s_Project_Title") %></p>
                                        </td>
                                        <td data-th="Project Category">
                                            <p><%#Eval("Project_Category") %></p>
                                        </td>
                                        <td data-th="CTC Status">
                                            <p><%#Eval("CTC_status") %></p>
                                        </td>
                                        <td data-th="DSRB/IRB No.">

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_Name") %></p>
                                        </td>

                                        <td data-th="Action">
                                            <p class="grid-action">
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# "New".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>'>
												
													<img title="Add Regulatory Detail" alt="" style="width:20px;" src="Images/Add-New.png"></asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# "Edit".Contains(Eval("Status").ToString()) %>'>

                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
												
													<img title="Edit Regulatory Detail" alt="" src="Images/icon-edit.png"></asp:LinkButton></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# "Edit".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_ID")) %>' CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Regulatory Detail" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton></asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# "Edit,View".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick="ResetAll();DoPostBack();return true;" CommandName="cmdView" runat="server">
                                                    
												<img title="View Regulatory Detail" alt="" src="Images/icon-view.png">
                                                    </asp:LinkButton></asp:PlaceHolder>
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


        <div class="row" id="Paging">
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

    </div>
    <div class="container RegulatoryContainer" id="DivEntry" runat="server">
        <span style="float: right; margin-top: 65px">
            <asp:LinkButton ID="lnkback" Text="Back to View" OnClick="lnkback_Click" runat="server"></asp:LinkButton></span>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Regulatory Details <span>Regulatory Entry Form <b>( Project ID:</b><b id="DispProjectId" runat="server"> </b>)</span></h1>


            </div>

        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmDetails">Project Details <span>( - )</span></h3>
            </div>
        </div>
        <div class="frmProject">

            <div class="frm frmDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>Project Title <b>*</b></label>
                            <asp:TextBox ID="TxtProjTitle" CssClass="ctltext" TextMode="MultiLine" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>
                        </p>

                        <p>
                            <label>Alias 1</label>
                            <asp:TextBox ID="TxtAlias1" CssClass="ctlinput" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>
                        </p>

                        <p>
                            <label>Short Title</label>
                            <asp:TextBox ID="TxtShortTitle" CssClass="ctlinput" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Project Category <b>*</b></label>


                            <asp:TextBox ID="TxtprojCategory" CssClass="ctlinput" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>
                        </p>
                        <p>
                            <label>Alias 2</label>
                            <asp:TextBox ID="TxtAlias2" CssClass="ctlinput" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>
                        </p>
                        <p>
                            <label>DSRB/IRB No</label>
                            <asp:TextBox ID="TxtIrbNo" CssClass="ctlinput" runat="server" Enabled="false" onmousedown="return false" onkeydown="return false"></asp:TextBox>

                        </p>
                    </div>
                </div>


            </div>

        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPIDetails">Principal Investigator (PI) Details<span>( - )</span>
                </h3>
            </div>
        </div>
        <div class="frm frmPIDetails" style="display: block;">
            <div class="row">

                <div class="col-md-12 col-sm-12">



                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">

                        <table id="tblPiDetail" class="tblResposive">
                            <thead>
                                <tr>
                                    <th>Department</th>
                                    <th>PI Name</th>
                                    <th style="width: 250px">Email</th>
                                    <th style="width: 150px">Phone</th>
                                    <th style="width: 150px; text-align: center">PI MCR No.</th>

                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="rptrPIDetails" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td data-th="Department">
                                                <p><%# Eval("s_DeptName")==DBNull.Value?Eval("DeptName"):Eval("s_DeptName") %></p>
                                            </td>
                                            <td data-th="PI Name">
                                                <p><%# Eval("s_PIName")==DBNull.Value?Eval("s_PI_Name"):Eval("s_PIName") %></p>
                                            </td>
                                            <td data-th="Email">
                                                <p><%# Eval("s_Email") %></p>
                                            </td>
                                            <td data-th="Phone">
                                                <p><%# Eval("s_Phone_no") %></p>
                                            </td>
                                            <td style="text-align: center" data-th="PI MCR No.">
                                                <p><%# Eval("s_MCR_No") %></p>
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
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmSponser">Sponsors Details<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmSponser" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Lead  Sponsor<b>*</b></label>
                        <asp:DropDownList ID="ddlLeadSponsor" onchange="SetOtherSponsorDesable();" CssClass="ctlselect" runat="server"></asp:DropDownList>

                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Other Lead Sponsors <b>*</b></label>

                        <asp:TextBox ID="TxtOtherLeadSpnsor" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmPrism">PRISM Application status<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmPrism" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PRISM  Application status<b>*</b></label>
                        <asp:DropDownList ID="ddlPrismStatus" CssClass="ctlselect" runat="server">
                            <asp:ListItem Selected="True" Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>

                    </p>

                    <p>
                        <label>PRISM  Application Submission Date <b>*</b></label>

                        <asp:TextBox ID="TxtPrismSubmissionDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker "></asp:TextBox>

                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PRISM  Application Number<b>*</b></label>
                        <asp:TextBox ID="TxtPrimsAppNo" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmCTCDetails">CTC Details<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmCTCDetails" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>CTC status<b>*</b></label>
                        <asp:DropDownList ID="ddlCTCStatus" onchange="DisableCTCControls();" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>

                    </p>
                    <p>
                        <label>CTC Document<b>*</b></label>
                        <asp:FileUpload runat="server" CssClass="ctlinput" ID="fldCTCdocument" />
                        <asp:HiddenField ID="HdnCTCDocPath" Value="" runat="server" />
                    </p>
                    <span>

                        <asp:LinkButton ID="LnkDwnldCTCDoc" runat="server"></asp:LinkButton></span>
                    <p>
                        <label>CTC Approval Date<b>*</b></label>

                        <asp:TextBox ID="TxtCTCAppDate" runat="server" onchange="SetDates();" CssClass="ctlinput ctlinput-sm datepicker "></asp:TextBox>

                    </p>
                    <p>
                        <label>New CTC Extension Approval Date<b>*</b></label>

                        <asp:TextBox ID="TxtCTCExtAppDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker "></asp:TextBox>

                    </p>
                    <p>
                        <label>New CTC Email Approval Doc</label>
                        <asp:FileUpload runat="server" CssClass="ctlinput" ID="fldNewCTCEmailApprDoc" />
                        <asp:HiddenField ID="HdnNCTCEmailApprDoc" Value="" runat="server" />
                    </p>
                    <span>

                        <asp:LinkButton ID="LnkDnwldNCTCEmailApprDoc" runat="server"></asp:LinkButton></span>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>CTC No<b>*</b></label>
                        <asp:TextBox ID="TxtCtcNo" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>CTC Email Approval Doc</label>
                        <asp:FileUpload runat="server" CssClass="ctlinput" ID="fldCTCEmailApprDoc" />
                        <asp:HiddenField ID="HdnCTCEmailApprDoc" Value="" runat="server" />
                    </p>
                    <span>

                        <asp:LinkButton ID="LnkDnwldCTCEmailApprDoc" runat="server"></asp:LinkButton></span>
                    <p>
                        <label>CTC Expiry Date<b>*</b></label>

                        <asp:TextBox ID="TxtCTCExpiryDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker "></asp:TextBox>

                    </p>
                    <p>
                        <label>New CTC Expiry Date <b>*</b></label>

                        <asp:TextBox ID="TxtNewCTCExpiryDate" runat="server" onchange="return CheckNewCTCExpDateonBlur();" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>
                    <p>
                        <label>Extended CTC Email Approval Doc</label>
                        <asp:FileUpload runat="server" CssClass="ctlinput" ID="fldExtCTCEmailApprDoc" />
                        <asp:HiddenField ID="HdnExtCTCEmailApprDoc" Value="" runat="server" />
                    </p>
                    <span>

                        <asp:LinkButton ID="LnkDwnldExtCTCEmailApprDoc" runat="server"></asp:LinkButton></span>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmAddtionalMember">Additional Study Team Members <span>( - )</span></h3>
                <p runat="server" id="P2"><span>+</span>  <a class="MoreTeamMember link" data-frm="frmAddMoreTeamMember">Add More Team Member</a></p>
            </div>
        </div>
        <div class="frm frmAddtionalMember" style="display: block;">
            <div class="row">

                <div class="col-md-12 col-sm-12">



                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">

                        <table id="tlbStudyMember" class="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 500px">First Name</th>
                                    <th>Last Name</th>
                                    <th>Email Id</th>
                                    <th style="width: 45px; text-align: right">Action</th>

                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="RptrStudyMember" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td data-th="First Name">
                                                <p><%# Eval("s_First_Name") %></p>
                                            </td>
                                            <td data-th="Last Name">
                                                <p><%# Eval("s_Last_Name")%></p>
                                            </td>
                                            <td data-th="Email Address">
                                                <p><%# Eval("s_Email_ID") %></p>
                                            </td>
                                            <td style="text-align: right" data-th="Action">
                                                <p class="grid-action">

                                                    <a>
                                                        <img title="Delete" alt="Delete" onclick="return delStudyTeamRows(this);" src="../images/icon-delete.png"></a>
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
        <div class="frmAddMoreTeamMember" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New Study Member Details		                  
                </h3>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>First Name <b>*</b></label>

                        <asp:TextBox ID="TxtFirstname" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>Email Id<b>*</b></label>
                        <asp:TextBox ID="TxtEmailId" onblur="checkValidEmail(this);" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Last Name<b>*</b></label>

                        <asp:TextBox ID="TxtLastName" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>

                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnMTeamMemberSave" runat="server" OnClientClick="return SaveStudyTeamMember();" Text="Save" />
                        <asp:Button CssClass="action" ID="btnMTeamMemberCancel" runat="server" OnClientClick="return ResetfrmAddMoreTeamMember();" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmProtocolDetails">Protocol Details<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmProtocolDetails" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Protocol Number</label>
                        <asp:TextBox ID="TxtProtocolNo" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Protocol Date </label>

                        <asp:TextBox ID="TxtProtocolDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>

                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Protocol Version No</label>

                        <asp:TextBox ID="TxtprotocolVersionNo" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmPISDetails">PIS/ICF Details<span>( - )</span></h3>
                <p runat="server" id="P1"><span>+</span>  <a class="MorePIS link" data-frm="frmAddMorePIS">Add More PIS/ICF </a></p>
            </div>
        </div>
        <div class="frm frmPISDetails" style="display: block;">
            <div class="row">

                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">

                        <table id="tlbPIS" class="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 500px">PIS/ICF Version No</th>
                                    <th>PIS/ICF Date</th>

                                    <th style="width: 45px; text-align: right">Action</th>

                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="rptPIS" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td data-th="PIS/ICF Version No">
                                                <p><%# Eval("s_Version_No") %></p>
                                            </td>
                                            <td data-th="PIS/ICF Date">
                                                <p><%# Eval("dt_ICF_Date")%></p>
                                            </td>

                                            <td style="text-align: right" data-th="Action">
                                                <p class="grid-action">

                                                    <a>
                                                        <img title="Delete" alt="Delete" onclick="return delPISRows(this);" src="../images/icon-delete.png"></a>

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
        <div class="frmAddMorePIS" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New PIS/ICF Details		                  
                </h3>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PIS/ICF Version No <b>*</b></label>

                        <asp:TextBox ID="TxtPISversionNo" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>



                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PIS/ICF Date<b>*</b></label>

                        <asp:TextBox ID="TxtPISDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>

                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnPISsave" runat="server" OnClientClick="return FunctionPISDetail();" Text="Save" />
                        <asp:Button CssClass="action" ID="btnPISReset" runat="server" OnClientClick="return ResetfrmAddMorePIS();" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmSixMonthUpdate">6 Months Update<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmSixMonthUpdate" style="display: block;">
            <div class="row">
                <div class="col-md-11 tab-02" id="DivTab" runat="server">
                    <div class="tabItems" runat="server" id="tblSIxMonth">
                    </div>

                </div>
            </div>
            <div class="row">

                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Pending Screen Outcome<b>*</b></label>

                        <asp:TextBox ID="TxtPendingScreenOutcome" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Screened<b>*</b></label>

                        <asp:TextBox ID="TxtScreened" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Withdrawn/Early Termination<b>*</b></label>

                        <asp:TextBox ID="Txttermination" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Outgoing Patiend<b>*</b></label>

                        <asp:TextBox ID="TxtoutPatient" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>SAE No.<b>*</b></label>

                        <asp:TextBox ID="TxtSaeNo" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Internal Audit<b>*</b></label>

                        <asp:DropDownList ID="ddlInternalAudit" runat="server" CssClass="ctlselect">
                            <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Screen Failure<b>*</b></label>

                        <asp:TextBox ID="TxtScrenFailure" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Randomized/Enrolled<b>*</b></label>

                        <asp:TextBox ID="TxtRandEnrolled" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Reason for Withdrawn<b>*</b></label>

                        <asp:TextBox ID="TxtReasonWithdrawn" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Completed No<b>*</b></label>

                        <asp:TextBox ID="TxtCompletedNo" runat="server" onpaste="return false;" onkeypress="return IsInteger(event,this);" MaxLength="3" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Reason for SAE<b>*</b></label>

                        <asp:TextBox ID="TxtReasonForSAE" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                    <p>
                        <label>Date Last Updated / Revised<b>*</b></label>

                        <asp:HiddenField runat="server" ID="hdnLastUpDate" Value="" />
                        <asp:TextBox ID="TxtLastUpDate" runat="server" onchange="return CheckDateofLastOnBlur();" CssClass="ctlinput  ctlinput-sm datepicker "></asp:TextBox>

                    </p>
                    <p id="PTag">(Last Updated / Revised Date Should be between CTC Approval and CTC Expiry)</p>
                </div>

            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnSixMSave" Style="display: none" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnSixMCancel" runat="server" Style="display: none" OnClientClick="return ResetfrmSixMonthUpdate();" Text="Reset" />

                    </p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmStatusSubmissionDate">Status Report Submission Date<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmStatusSubmissionDate" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>6 Months Status Report Submission Date </label>
                        <asp:TextBox ID="TxtSixMStateDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>
                    <p>
                        <label>12 Months Status Report Submission Date</label>

                        <asp:TextBox ID="TxtTwelvMStateDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>
                    <p>
                        <label>18 Months Status Report Submission Date</label>

                        <asp:TextBox ID="TxtEightMStateDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>21 Months Status Report Submission Date</label>

                        <asp:TextBox ID="TxtTwentyOMStateDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>
                    <p>
                        <label>24 Months Status Report Submission Date</label>

                        <asp:TextBox ID="TxtTwentyForMStateDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>

                    </p>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmStatusReportFile">Status Report Submission File    <span>( - )</span></h3>
                <p runat="server" id="P3"><span>+</span>  <a class="MonlyStatusRpt link" data-frm="frmMonthlyStatusReport">Add Monthly Status Report </a></p>
            </div>
        </div>
        <div class="frm frmStatusReportFile" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">
                        <table id="tblStatusMontly" class="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 250px">Status Report Monthly</th>
                                    <th style="width: 500px">Report Title</th>
                                    <th style="width: 230px">Upload File Name</th>

                                    <th style="width: 45px; text-align: right">Action</th>

                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RptrStatusMontly" runat="server" OnItemDataBound="RptrStatusMontly_ItemDataBound">
                                    <ItemTemplate>
                                        <tr runat="server" id="trStatusM" filepath='<%# Eval("s_Uploaded_File") %>' intervalid='<%# Eval("i_Interval_ID") %>'>
                                            <td data-th="Status Report Monthly">
                                                <p><%# Eval("ReportName") %></p>
                                            </td>
                                            <td data-th="Report Title">
                                                <p><%# Eval("s_File_Title")%></p>
                                            </td>
                                            <td data-th="Upload File Name">
                                                <p><%# Eval("UpFileName")%></p>
                                            </td>
                                            <td style="text-align: right" data-th="Action">
                                                <p class="grid-action">

                                                    <a>
                                                        <img title="Delete" alt="Delete" onclick="return delStatusSubFileRows(this);" src="../images/icon-delete.png"></a>

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
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New Submission Report File				                  
                </h3>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Select Month Interval<b>*</b></label>

                        <asp:DropDownList ID="ddlStatusRptmonth" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label>Upload File<b>*</b></label>

                        <p>
                            <asp:FileUpload runat="server" ID="fldStatusRptFile" onchange="return FloadChange(this);" CssClass="ctlinput" />

                        </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>File Title<b>*</b></label>

                        <asp:TextBox ID="TxtStatusRptTitle" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>

                </div>
            </div>

            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnStatusRptSave" OnClientClick="return SaveStatusReportSubmissionFile();" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnStatusRptCancel" runat="server" OnClientClick="return ResetfrmMonthlyStatusReport();" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmAmendment">Amendments Details<span>( - )</span></h3>
                <p runat="server" id="P4"><span>+</span>  <a class="AmentDetails link" data-frm="frmAmendmentDetails">Add More Amendment File</a></p>
            </div>
        </div>
        <div class="frm frmAmendment" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">
                        <table id="tblAmendmentDetails" class="tblResposive">
                            <thead>
                                <tr runat="server" id="Thtr">
                                    <th style="width: 200px">Amendment Submission Date</th>
                                    <th style="width: 170px">File Name</th>

                                    <th style="width: 45px; text-align: right">Action</th>

                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RptAmendmentDetails" runat="server" OnItemDataBound="RptAmendmentDetails_ItemDataBound">
                                    <ItemTemplate>
                                        <tr runat="server" id="trAmend" filepath='<%# Eval("s_Uploaded_File") %>'>
                                            <td data-th="Amendment Submission Date">
                                                <p><%# Eval("dt_Submission_Date") %></p>
                                            </td>
                                            <td data-th="File Name">
                                                <p><%# Eval("Uploaded_File")%></p>
                                            </td>

                                            <td style="text-align: right" data-th="Action">
                                                <p class="grid-action">
                                                    <a>
                                                        <img title="Delete" alt="Delete" onclick="return delAmendmentsDetailRows(this);" src="../images/icon-delete.png"></a>

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
        <div class="frmAmendmentDetails" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New Amendments File				                  
                </h3>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Amendment Submission Date<b>*</b></label>


                        <asp:TextBox ID="TxtAmendmentSubDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker"></asp:TextBox>
                    </p>



                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Amendment Approval Letter<b>*</b></label>
                        <input id="TxtAmendmentLetter" class="ctlinput" type="text" style="display: none" /><input id="btnAmendmentLetter" style="display: none" class="action" style="padding-top: 5px; padding-bottom: 5px; padding-left: 5px; margin-top: 15px;" type="button" value="BROWSE.." />
                        <asp:FileUpload runat="server" ID="fldAmendmentLetter" onchange="return FloadChange(this);" CssClass="ctlinput" />

                    </p>

                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnAmendmentSave" runat="server" OnClientClick="return SaveAmendmentsDetails();" Text="Save" />
                        <asp:Button CssClass="action" ID="btnAmendmentCancel" runat="server" OnClientClick="return ResetfrmAmendmentDetails();" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmLast">Others<span>( - )</span></h3>
                <p runat="server" id="P5"><span>+</span>  <a class="Others link" data-frm="frmOtherDetails">Add More Investigational Product</a></p>
            </div>
        </div>
        <div class="frm frmLast" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">
                        <table id="tblOther" class="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 280px">Investigational Product</th>
                                    <th style="width: 540px">Management of IP Actions (storage and dispensing)</th>
                                    <th style="width: 250px">IP Storage Location</th>
                                    <th style="width: 45px; text-align: right">Action</th>

                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RptrOther" runat="server">
                                    <ItemTemplate>
                                        <tr ipid='<%# Eval("s_IPManagement")  %>'>
                                            <td data-th="Investigational Product">
                                                <p><%# Eval("s_Investigational_Product") %></p>
                                            </td>
                                            <td data-th="Management of IP Actions (storage and dispensing)">
                                                <p><%# Eval("s_IPName")%></p>
                                            </td>
                                            <td data-th="IP Storage Location">
                                                <p><%# Eval("s_StorageLocation")  %></p>
                                            </td>
                                            <td data-th="Action">
                                                <p class="grid-action">

                                                    <a>
                                                        <img title="Delete" alt="Delete" onclick="return delInvestProductRows(this);" src="../images/icon-delete.png"></a>
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
            <div class="row">

                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Subject Recruited by TTSH</label>
                        <asp:TextBox ID="TxtSubject" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Remark</label>

                        <asp:TextBox ID="TxtRemark" runat="server" CssClass="ctlinput"></asp:TextBox>

                    </p>
                </div>
            </div>

        </div>
        <div class="frmOtherDetails" style="display: none;">
            <div class="row">

                <div class="col-md-12 col-sm-12">
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">New Investigational Product				                  
                </h3>

                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Investigational Product<b>*</b></label>


                        <asp:TextBox ID="TxtInvestigationalProduct" runat="server" CssClass="ctlinput"></asp:TextBox>
                    </p>

                    <p>
                        <label>IP Storage Location<b id="bIpStorage" style="display: none">*</b></label>


                        <asp:TextBox ID="TxtIPLocation" runat="server" CssClass="ctlinput"></asp:TextBox>
                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Management of IP (storage and dispensing)<b>*</b></label>

                        <asp:DropDownList ID="ddlIp" onchange="SetIPStorageDisable();" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>

                    </p>

                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnOtherSave" OnClientClick="return SaveOtherDetails();" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnOtherCancel" runat="server" OnClientClick="return ResetfrmOtherDetails();" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>

        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p style="text-align: right">


                    <asp:Button CssClass="action" ID="btnSave" OnClick="btnSave_Click" runat="server" OnClientClick="return IsValidte();" Text="Save Details" />
                    <asp:Button CssClass="action" ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" OnClientClick="ResetAll();DoPostBack();return true;" />
                    <asp:Button ID="delete" Style="display: none" runat="server" OnClick="delete_Click" />
                </p>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function FloadChange(obj) {

            if (obj.value != "") {
             

                var uploadfiles = $(obj).get(0);

                var uploadedfiles = uploadfiles.files;
                if (window.FormData !== undefined) {

                    var fromdata = new FormData();

                    for (var i = 0; i < uploadedfiles.length; i++) {

                        fromdata.append(uploadedfiles[i].name.replace(/\'+/gmi, " "), uploadedfiles[i]);

                    }



                    var choice = {};

                    choice.url = "Upload.ashx";

                    choice.type = "POST";

                    choice.data = fromdata;

                    choice.contentType = false;

                    choice.processData = false;

                    choice.success = function (result) {
                        if (result != null) {
                            $('[id*=HdnGlobPath]').val(result); return false;
                        }
                    };

                    choice.error = function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }


                    $.ajax(choice);

                }
                else {

                }

                return true;


            }

        }

    

    </script>
</asp:Content>
