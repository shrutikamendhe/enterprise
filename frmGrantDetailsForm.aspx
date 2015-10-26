<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="frmGrantDetailsForm.aspx.cs" Inherits="TTSHWeb.frmGrantDetailsForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Ddl {
            width: 150px !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('[id*=tblSingleBudget] tbody tr input[type=text]').keydown(function (e) {
                var n = $("input:text").length;
                if (e.which == 13) { //Enter key
                    e.preventDefault(); //Skip default behavior of the enter key
                    var nextIndex = $('input:text').index(this) + 1;
                    if (nextIndex < n)
                        $('input:text')[nextIndex].focus();
                    else {
                        $('input:text')[nextIndex - 1].blur();
                    }
                }
            });
        });


    </script>
    <script src="Scripts/Webform/jsGrantDetails.js"></script>

    <!-- hide grid Years
        
        $('[id*=tblSingleBudget] tbody').find('tr[rid*=trY1]').css('display','none')
        -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <div class="GrantDetail container" runat="server" id="DivMain">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Grant Detail View <span>Search, Filter and Edit Grant Details</span></h1>

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

                                <th>Grant Detail Status</th>
                                <th>DSRB/IRB No.</th>
                                <th>PI Name</th>

                                <th style="width: 95px">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="RptGrantGrid" OnItemCommand="RptGrantGrid_ItemCommand" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td data-th="Project ID">
                                            <p><%#Eval("s_Display_Project_ID") %></p>
                                        </td>
                                        <td data-th="Project Title">
                                            <p><%#Eval("s_Project_Title") %></p>
                                        </td>
                                        <td data-th="Project Category">
                                            <p><%#Eval("Project_Category_Name") %></p>
                                        </td>
                                        <td data-th="Grant Detail Status">
                                            <p><%#Eval("GrantDetailStatus") %></p>
                                        </td>
                                        <td data-th="DSRB/IRB No.">

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_Name") %></p>
                                        </td>

                                        <td data-th="Action">
                                            <p class="grid-action">
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# "New".Contains(Eval("GrantDetailStatus").ToString()) %>'>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>'>
												
													<img title="Add Grant Detail" alt="" style="width:20px;" src="Images/Add-New.png"></asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("GrantDetailStatus").ToString()!="New" %>'>

                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.GD_ID")%>'>
												
													<img title="Edit Grant Detail" alt="" src="Images/icon-edit.png"></asp:LinkButton></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("GrantDetailStatus").ToString()!="New" %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.GD_ID")%>' OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("GD_ID")) %>' CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Grant Detail" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton></asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# Eval("GrantDetailStatus").ToString()!="New" %>'>
                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.GD_ID")%>' OnClientClick="ResetAll();DoPostBack();return true;" CommandName="cmdView" runat="server">
                                                    
												<img title="View Grant Detail" alt="" src="Images/icon-view.png">
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
        <div class="row margin-top frmAction" style="margin-top: 5px;">
            <div class="col-md-12">
                <p style="text-align: left">



                    <asp:Button CssClass="action" ID="btnNew" runat="server" Text="Add New Project" OnClientClick="window.open( 'frmProject_Master.aspx?NewPage=true','_blank' );return false;" />

                </p>
            </div>
        </div>
    </div>


    <div class="GrantDetail container" id="DivEntry" runat="server">
        <span style="float: right; margin-top: 65px">
            <asp:LinkButton ID="lnkback" Text="Back to View" OnClick="lnkback_Click" runat="server"></asp:LinkButton></span>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Grant Details <span>Grant Entry Form <b>( Project ID:</b><b id="DispProjectId" runat="server"> </b>)</span></h1>
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
                <h3 class="frmHead" data-frm="frmPI">Principal Investigator (PI) Details <span>( - )</span>
                </h3>
                <%--  <p runat="server" id="PMorePi"><span>+</span>  <a class="newPI link" data-frm="frmNewPIDetails">Record New PI Details</a></p>--%>
            </div>
        </div>

        <div class="frm frmPI" style="display: block;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper">
                        <table id="tblPiDetail" class="tblResposive">
                            <thead>
                                <tr>

                                    <th style="width: 450px; text-align: left">Department</th>
                                    <th style="text-align: left">PI Name</th>
                                    <th style="text-align: left">Email</th>
                                    <th style="text-align: left">Phone</th>
                                    <th style="text-align: left">PI MCR No.</th>

                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="rptrPIDetails" runat="server">
                                    <ItemTemplate>
                                        <tr>
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

                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>




                            </tbody>
                        </table>
                    </div>
                    <%--  <p runat="server" id="Pmore" class="align-right"><a class="link" onclick="AddMorePI();">+ Add More PI</a></p>--%>
                </div>
            </div>
        </div>

        <%--    <div class="frmNewPIDetails" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 runat="server" id="hrMorePi" style="color: rgb(228, 16, 83); margin-bottom: 1em;">Record New Principal Investigator (PI) Details					                  
                    </h3>
                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="UpPi">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6 col-sm-6">
                            <p>
                                <label>Department <b>*</b></label>
                                <asp:HiddenField ID="HdnNewDeptId" runat="server" />

                                <asp:TextBox ID="TxtNewDepartment" onpaste="return false;" placeholder="Type Keyword to search Department" CssClass="ctlinput" runat="server"></asp:TextBox>

                            </p>
                            <p>
                                <label>First / Given Name <b>*</b></label>
                                <asp:TextBox ID="txtPiFirstName" CssClass="ctlinput" onkeypress="return RestrictPower(event);" placeholder="First Name" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>PI Email <b>*</b></label>
                                <asp:TextBox ID="txtPIEmailAddress" onblur="checkValidEmail(this);" placeholder="PI Email" CssClass="ctlinput" runat="server"></asp:TextBox>
                            </p>


                        </div>
                        <div class="col-md-6 col-sm-6">
                            <p>
                                <label>PI MCR No.<b>*</b></label>
                                <asp:TextBox ID="txtPIMCR_NO" CssClass="ctlinput" placeholder="PI MCR No" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>Last Name / Surname <b>*</b></label>
                                <asp:TextBox ID="txtPiLastName" CssClass="ctlinput" onkeypress="return RestrictPower(event);" placeholder="Last Name" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>Phone No.</label>
                                <asp:TextBox ID="txtPiPhNo" CssClass="ctlinput" onKeypress="return SingaporePhformat();" onpaste="return false" placeholder="Phone No" runat="server"></asp:TextBox>
                            </p>

                        </div>
                    </div>
                    <div class="row margin-top frmAction">
                        <div class="col-md-12">
                            <p style="text-align: right">


                                <asp:Button CssClass="action" ID="btnPISave" runat="server" Text="Save" />
                                <asp:Button CssClass="action" ID="btnPICancel" runat="server" OnClientClick="return ClearCloseNewPiSection();" Text="Reset" />

                            </p>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="frmAddMorePIDetails" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 style="color: rgb(228, 16, 83); margin-bottom: 1em;">Add  Principal  Investigator (PI)					                  
                    </h3>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Department <b>*</b></label>
                        <asp:HiddenField ID="HdnDeptId" runat="server" />
                        <asp:HiddenField ID="HdnDeptTxt" runat="server" />
                        <asp:TextBox ID="TxtDepartment" onpaste="return false;" onblur="ClearOnblur(this);" onKeydown="items('');" placeholder="Type Keyword to search Department" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>PI Email </label>
                        <asp:TextBox ID="txtPIEmail" CssClass="ctlinput" placeholder="PI Email" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>Phone No.</label>
                        <asp:TextBox ID="txtPiPhoneNo" CssClass="ctlinput" placeholder="Phone No" runat="server"></asp:TextBox>
                    </p>

                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PI Name<b>*</b></label>
                        <asp:HiddenField ID="HdnpiId" runat="server" />
                        <asp:HiddenField ID="HdnPITxt" runat="server" />
                        <asp:TextBox ID="TxtPIName" onpaste="return false;" onblur="CheckPiOnBlur(this);" onKeydown="items('pi');" onchange="items('pi')" placeholder="Type Keyword to search PI" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>PI MCR No.</label>
                        <asp:TextBox ID="txtPiMCRNo" CssClass="ctlinput" placeholder="PI MCR No" runat="server"></asp:TextBox>
                    </p>
                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnMorePiSave" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnMorePiCancel" OnClientClick="return ClearCloseMorePiSection();" runat="server" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>--%>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmSingleProject">Grant Detail <span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmSingleProject" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Grant Id<b>*</b></label>

                        <asp:TextBox ID="TxtGrantId" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>

                    <p>
                        <label>Grant Type<b>*</b></label>


                        <asp:TextBox ID="TxtGrantType" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>Grant Sub-Sub Category<b>*</b></label>


                        <asp:TextBox ID="TxtGrantSSType" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>Date of Award Letter<b>*</b></label>


                        <asp:TextBox ID="TxtDateofAwardLetter" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox>

                    </p>
                    <p id="AwardP">
                        <label>Award Letter<b>*</b></label>


                        <asp:FileUpload ID="FldAwardLetterfil" CssClass="ctlinput" runat="server" />
                        <asp:HiddenField ID="HdnAwardLetterFile" runat="server" />
                    </p>
                    <span>
                        <asp:LinkButton ID="LnkAwardLetterfile" runat="server"></asp:LinkButton></span>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Grant Details Status<b>*</b></label>
                        <asp:DropDownList ID="ddlGrantDetailStatus" runat="server" CssClass="ctlselect"></asp:DropDownList>

                    </p>
                    <p>
                        <label>Grant Sub Category<b>*</b></label>


                        <asp:TextBox ID="TxtGrantSubType" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>Grant Sub-Sub-Sub Category<b>*</b></label>


                        <asp:TextBox ID="TxtGrantSSSType" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>Grant Expiry Date<b>*</b></label>


                        <asp:TextBox ID="TxtGrantExpDate" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox>

                    </p>

                </div>



            </div>
            <div id="AnyExtDiv" class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Any Extension</label>
                        <asp:DropDownList ID="ddlExtension" onchange="SetEnableOnAnyExt();" runat="server" CssClass="ctlselect">
                            <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>
                        <label>
                            New Grant Expiry Date<b>*</b>
                        </label>
                        <asp:TextBox ID="TxtNewGrantExpDate" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox>
                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Select Duration<b>*</b></label>
                        <asp:DropDownList ID="ddlDuration" runat="server" CssClass="ctlselect">
                        </asp:DropDownList>
                    </p>
                </div>
            </div>
            <div id="OtherStatusDiv" class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>
                            Research IO<b>*</b>
                        </label>
                        <asp:TextBox ID="TxtReaserchIO" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
                <div id="ODiv" class="col-md-6 col-sm-6">
                    <p>
                        <label>FTE</label>
                        <asp:TextBox ID="TxtFTE" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
            </div>
            <div id="DivDonation" style="display: none" class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>
                            Donation Amount<b>*</b>
                        </label>
                        <asp:TextBox ID="TxtDonationAmt" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Donation Instituion/Body</label>
                        <asp:TextBox ID="TxtDonationInst" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmTTSHPiDetail">TTSH PI Details Year Wise Budget Distribution (Single Project) <span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmTTSHPiDetail" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PI Name<b>*</b></label>
                        <asp:DropDownList ID="ddlPIName" CssClass="ctlselect" runat="server"></asp:DropDownList>

                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="tblResposiveWrapper">
                        <table id="tblSingleBudget" class="tblResposive">
                            <thead>
                                <tr>

                                    <th style="width: 100px; text-align: left">Year</th>
                                    <th style="width: 550px; text-align: left">Factors</th>
                                    <th>Estimated Budget</th>
                                    <th>Actual Spending</th>

                                </tr>
                            </thead>

                            <tbody>
                                <tr rid="trY1">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Year1">Year1</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr1" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr1" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY1">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr1" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr1" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY1">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr1" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr1" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY1">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr1" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr1" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY1">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year1 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y1EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y1ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY2">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span1">Year2</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr2" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr2" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY2">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr2" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr2" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY2">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr2" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr2" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY2">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr2" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr2" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY2">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year2 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y2EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y2ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY3">
                                    <td rowspan="5" data-th="Year">
                                        <span id="Span4">Year3</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr3" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr3" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY3">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr3" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr3" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY3">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr3" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr3" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY3">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr3" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr3" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY3">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year3 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y3EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y3ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY4">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span7">Year4</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr4" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr4" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY4">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr4" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr4" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY4">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr4" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr4" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY4">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr4" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr4" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY4">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year4 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y4EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y4ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY5">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span10">Year5</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr5" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr5" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY5">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr5" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr5" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY5">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr5" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr5" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY5">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr5" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr5" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY5">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year5 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y5EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y5ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY6">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span13">Year6</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY6">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY6">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY6">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY6">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year6 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y6EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y6ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trY7">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span16">Year7</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr7" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr7" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trY7">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr7" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr7" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY7">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr7" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr7" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY7">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr7" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr7" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trY7">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">Year7 Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="Y7EstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="Y7ActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trYMonths">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span19">6 Months</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trYMonths">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trYMonths">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trYMonths">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="Estyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="Actyr6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trYMonths">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">6 Months Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="6MonthEstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="6MonthActTotal"></span>
                                    </td>

                                </tr>

                                <tr rid="trExtYMonths">
                                    <td rowspan="4" data-th="Year">
                                        <span id="Span22">6 Months Ext</span>
                                    </td>
                                    <td data-th="Factors">
                                        <span>Man Power</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="EstyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="ActyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>


                                </tr>
                                <tr rid="trExtYMonths">

                                    <td data-th="Factors">
                                        <span>Consumables</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="EstyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="ActyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trExtYMonths">

                                    <td data-th="Factors">
                                        <span>Equipment</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="EstyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="ActyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trExtYMonths">

                                    <td data-th="Factors">
                                        <span>Miscellaneuos</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <input type="text" tid="EstyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>
                                    <td data-th="Actual Spending">
                                        <input type="text" tid="ActyrExt6Months" style="width: 200px" class="ctlinput" />
                                    </td>

                                </tr>
                                <tr rid="trExtYMonths">

                                    <td data-th="Factors">
                                        <span style="font-weight: bold">6 Months Ext Total</span>
                                    </td>
                                    <td data-th="Estimated Budget">
                                        <span style="font-weight: bold" id="6MonthExtEstTotal"></span>
                                    </td>
                                    <td data-th="Actual Spending">
                                        <span style="font-weight: bold" id="6MonthExtActTotal"></span>
                                    </td>

                                </tr>
                            </tbody>
                        </table>
                    </div>

                </div>

            </div>




        </div>


        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmTTSHPiMultiDetail">TTSH PI Details Year Wise Budget Distribution (Multiple Project) <span>( - )</span></h3>
            </div>
        </div>

        <div class="frm frmTTSHPiMultiDetail" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Child Project<b>*</b></label>
                        <asp:DropDownList ID="ddlChildProject" CssClass="ctlselect" runat="server"></asp:DropDownList>

                    </p>

                    <p>
                        <label>Any Extension </label>
                        <asp:DropDownList ID="ddlChildExtesnion" onchange="ShowHideMultiExtSection();" CssClass="ctlselect" runat="server">
                            <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                    </p>
                    <p>
                        <label>Extension Duration<b>*</b></label>
                        <asp:DropDownList ID="ddlExtDuration" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>

                    </p>

                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PI Name<b>*</b></label>
                        <asp:DropDownList ID="ddlChildPI" CssClass="ctlselect" runat="server"></asp:DropDownList>

                    </p>
                    <p>
                        <label>New Grant Expiry Date<b>*</b></label>
                        <asp:TextBox ID="TxtMultiExtNewGrantExpDate" CssClass="ctlinput ctlinput-sm datepicker" runat="server"></asp:TextBox>

                    </p>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 class="frmHead" data-frm="frmChildPI">Child Project Details <span>( - )</span>
                    </h3>

                </div>
            </div>
            <div class="frm frmChildPI" style="display: block;">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <div class="tblResposiveWrapper">
                            <table id="tblChildProject" class="tblResposive">
                                <thead>
                                    <tr>
                                        <th>Project Id</th>
                                        <th>Project Title</th>
                                        <th style="text-align: left; width: 150px;">PI Name</th>

                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="RptrChildProject" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td data-th="Project Id">
                                                    <p><%# Eval("s_Display_Project_ID") %></p>
                                                </td>
                                                <td data-th="Project Title">
                                                    <p><%# Eval("s_Project_Title") %></p>
                                                </td>
                                                <td data-th="PI Name">
                                                    <p><%# Eval("PI_Name") %></p>
                                                </td>


                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>




                                </tbody>
                            </table>
                        </div>
                        <p runat="server" id="Pmore" class="align-right"><a class="link" onclick="window.open('frmProject_Master.aspx?NewPage=true','_blank' );return false;">Add New Project</a></p>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmIsVariation">Variation Needed Budget Distribution <span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmIsVariation" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Is Variation Needed</label>
                        <asp:DropDownList ID="ddlVariation" onchange="SHowHideVariationNeeded();" CssClass="ctlselect" runat="server">
                            <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>

                    </p>
                </div>

            </div>
            <div id="VariationBlock" class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Year<b>*</b></label>
                        <asp:DropDownList ID="ddlVariationYear" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>

                    </p>
                </div>
                <div class="col-md-12 col-sm-12">
                    <table style="width: 100%; border: 10px">

                        <tr>
                            <td>
                                <p>
                                    <label>Select Category<b>*</b> </label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:DropDownList ID="ddlCat1" CssClass="ctlselect" runat="server">
                                        <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Man Power" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Consumables" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Equipments" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Miscellaneous" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <td style="padding-left: 10px;">
                                    <p>
                                        <label>Estimated Amount Allocated</label>
                                    </p>
                                </td>
                                <td>
                                    <p>
                                        <asp:TextBox ID="TxtEst1Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                    </p>
                                </td>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Amount to be Deducted</label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:TextBox ID="TxtDed1Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Total Amount Left</label>
                                </p>
                            </td>



                            <td>
                                <p>
                                    <asp:TextBox ID="TxtLeft1Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    <label>Select Category<b>*</b> </label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:DropDownList ID="ddlCat2" CssClass="ctlselect" runat="server">
                                        <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Man Power" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Consumables" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Equipments" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Miscellaneous" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <td style="padding-left: 10px;">
                                    <p>
                                        <label>Estimated Amount Allocated</label>
                                    </p>
                                </td>
                                <td>
                                    <p>
                                        <asp:TextBox ID="TxtEst2Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                    </p>
                                </td>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Amount to be Deducted</label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:TextBox ID="TxtDed2Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Total Amount Left</label>
                                </p>
                            </td>



                            <td>
                                <p>
                                    <asp:TextBox ID="TxtLeft2Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    <label>Select Category<b>*</b> </label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:DropDownList ID="ddlCat3" CssClass="ctlselect" runat="server">
                                        <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Man Power" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Consumables" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Equipments" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Miscellaneous" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <td style="padding-left: 10px;">
                                    <p>
                                        <label>Estimated Amount Allocated</label>
                                    </p>
                                </td>
                                <td>
                                    <p>
                                        <asp:TextBox ID="TxtEst3Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                    </p>
                                </td>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Amount to be Deducted</label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:TextBox ID="TxtDed3Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Total Amount Left</label>
                                </p>
                            </td>



                            <td>
                                <p>
                                    <asp:TextBox ID="TxtLeft3Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>
                                    <label>Select Category<b>*</b> </label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:DropDownList ID="ddlCat4" CssClass="ctlselect" runat="server">
                                        <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Man Power" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Consumables" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Equipments" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Miscellaneous" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <td style="padding-left: 10px;">
                                    <p>
                                        <label>Estimated Amount Allocated</label>
                                    </p>
                                </td>
                                <td>
                                    <p>
                                        <asp:TextBox ID="TxtEst4Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                    </p>
                                </td>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Amount to be Deducted</label>
                                </p>
                            </td>
                            <td>
                                <p>
                                    <asp:TextBox ID="TxtDed4Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                            <td style="padding-left: 10px;">
                                <p>
                                    <label>Total Amount Left</label>
                                </p>
                            </td>



                            <td>
                                <p>
                                    <asp:TextBox ID="TxtLeft4Amt" CssClass="ctlinput-sm" runat="server"></asp:TextBox>
                                </p>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row margin-top frmAction">
                    <div class="col-md-12">
                        <p style="text-align: right">


                            <asp:Button CssClass="action" ID="btnvariationSave" runat="server" Text="Save" />
                            <asp:Button CssClass="action" ID="btnvariationCancel" runat="server" Text="Reset" />

                        </p>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmOtherDetail">Mentor Detail<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmOtherDetail" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Indirects</label>
                        <asp:TextBox ID="TxtIndirects" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>
                            Mentor
                        </label>
                        <asp:DropDownList ID="ddlMentor" onchange="ShowHideMentor();" runat="server" CssClass="ctlselect">
                            <asp:ListItem Text="--Select--" Value="-1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Indirects</label>
                        <asp:TextBox ID="TxtIndirectAmt" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>

            </div>
            <div id="MentorDiv" style="display: none" class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Mentor Name</label>
                        <asp:TextBox ID="TxtMentorName" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>
                            Mentor Department
                        </label>
                        <asp:TextBox ID="TxtmentorDept" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Mentor Institution</label>
                        <asp:TextBox ID="TxtMentorInstition" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
            </div>
        </div>



        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmTechPIDetail">Technical PI Detail<span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmTechPIDetail" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Name of Technical PI</label>
                        <asp:TextBox ID="TxtTechPIName" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>Technical PI Department</label>
                        <asp:TextBox ID="TxtTechPiDept" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Technical PI Institution</label>
                        <asp:TextBox ID="TxtTechPiInst" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>Point of Submission</label>
                        <asp:TextBox ID="TxtPointofSub" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                </div>
            </div>
        </div>


        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p style="text-align: right">


                    <asp:Button CssClass="action" ID="btnSave" runat="server" Text="Save Details" />
                    <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" />
                    <asp:Button ID="delete" runat="server" Style="display: none" />
                </p>
            </div>
        </div>
        <div id="HdnSection">
            <asp:HiddenField ID="HdnMode" Value="Insert" runat="server" />
            <asp:HiddenField ID="HdnProjectId" Value="0" runat="server" />
            <asp:HiddenField ID="HdnGranDId" Value="0" runat="server" />
            <asp:HiddenField ID="HdnExtension" Value="" runat="server" />
            <asp:HiddenField ID="HdnCheckfld" Value="" runat="server" />
            <asp:HiddenField ID="HdnDuration" Value="" runat="server" />
        </div>
    </div>

    </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="RptGrantGrid" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
