<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="frmGrantSeniorCSCS.aspx.cs" Inherits="TTSHWeb.frmGrantSeniorCSCS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Webform/jsGrantSeniorCSCS.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="GrantSeniorCSCS container" runat="server" id="DivMain">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Grant Senior CSCS View <span>Search, Filter and Edit Grant Senior CSCS</span></h1>

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
                                <th style="width: 100px">Grant No.</th>
                                <th>Grant Name</th>
                                <th>Awarding Organization</th>

                                <th>PI Name</th>
                                <th>Start Date.</th>
                                <th>Grant Expiry Date</th>

                                <th style="width: 95px">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="RptGrantGridSeniorCSCS" OnItemCommand="RptGrantGridSeniorCSCS_ItemCommand" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td data-th=">Grant No.">
                                            <p><%#Eval("s_Grant_No") %></p>
                                        </td>
                                        <td data-th="Grant Name">
                                            <p><%#Eval("GrantName") %></p>
                                        </td>
                                        <td data-th="Awarding Organization">
                                            <p><%#Eval("AwardOrg") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_NAME") %></p>
                                        </td>
                                        <td data-th="Start Date.">

                                            <p><%#Eval("StartDate") %></p>
                                        </td>
                                        <td data-th="Grant Expiry Date">
                                            <p><%#Eval("GrantExpDate") %></p>
                                        </td>

                                        <td data-th="Action">
                                            <p class="grid-action">
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# Convert.ToInt32(Eval("i_ID"))==0 %>'>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
												
													<img title="Add GrantSeniorCSCS Detail" alt="" style="width:20px;" src="Images/Add-New.png"></asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Convert.ToInt32(Eval("i_ID"))!=0 %>'>

                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
												
													<img title="Edit GrantSeniorCSCS Detail" alt="" src="Images/icon-edit.png"></asp:LinkButton></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Convert.ToInt32(Eval("i_ID"))!=0 %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_ID")) %>' CommandName="cmdDelete" runat="server">
                                                        <img title="Delete GrantSeniorCSCS Detail" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton></asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# Convert.ToInt32(Eval("i_ID"))!=0 %>'>
                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick="ResetAll();DoPostBack();return true;" CommandName="cmdView" runat="server">
                                                    
												<img title="View GrantSeniorCSCS Detail" alt="" src="Images/icon-view.png">
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



                    <asp:Button CssClass="action" ID="btnNew" runat="server" Text="Add New Detail" OnClick="btnNew_Click" />

                </p>
            </div>
        </div>
    </div>

    <div class="GrantSeniorCSCS container" id="DivEntry" runat="server">
        <span style="float: right; margin-top: 65px">
            <asp:LinkButton ID="lnkback" Text="Back to View" OnClick="lnkback_Click" runat="server"></asp:LinkButton></span>

        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Grant Senior CSCS<span>Grant Entry Form <b>( Project ID:</b><b id="DispProjectId" runat="server"> </b>)</span></h1>
            </div>

        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmGrantDetails ">Grant Details <span>( - )</span></h3>
            </div>
        </div>
        <div class="frmProject">
            <div class="frm frmGrantDetails" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Awarding Organization <b>*</b></label>
                            <asp:DropDownList ID="ddlAwardOrg" CssClass="ctlselect" runat="server"></asp:DropDownList>
                        </p>

                        <p>
                            <label>Grant Name<b>*</b></label>

                            <%--<asp:TextBox ID="TxtGrantName" CssClass="ctlinput" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlGrantName" CssClass="ctlselect" runat="server"></asp:DropDownList>
                        </p>

                        <p>
                            <label>Duration of Grant<b>*</b></label>
                            <%--<asp:TextBox ID="TxtDurationofGrant" CssClass="ctlinput" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlDurationofGrant" CssClass="ctlselect" runat="server"></asp:DropDownList>
                        </p>
                        <p>
                            <label>Grant Extended</label>

                            <asp:DropDownList ID="ddlGrantExtended" CssClass="ctlselect" runat="server">
                                <asp:ListItem Selected="True" Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>

                        </p>
                        <p>
                            <label>Date of Approval</label>
                            <asp:TextBox ID="TxtApprDate" CssClass="ctlinput ctlinput-sm datepicker extendedField" runat="server"></asp:TextBox>
                        </p>
                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Grant Number<b>*</b></label>
                            <asp:TextBox ID="TxtGrantNo" CssClass="ctlinput" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Research IO<b>*</b></label>
                            <asp:TextBox ID="TxtReaserchIO" CssClass="ctlinput" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Grant Expiry Date<b>*</b></label>
                            <asp:TextBox ID="TxtgrantExpDate" CssClass="ctlinput ctlinput-sm datepicker " runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>New Expiry Date <b id="bNExp" style="display: none">*</b></label>
                            <asp:TextBox ID="TxtNExpDate" CssClass="ctlinput ctlinput-sm datepicker extendedField" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Grant Extended Period</label>
                            <%--<asp:TextBox ID="TxtGrantExtendPeriod" CssClass="ctlinput" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlGrantExtendPeriod" CssClass="ctlselect extendedField" runat="server"></asp:DropDownList>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmAwardLetter ">Award Letter Details <span>( - )</span></h3>
            </div>
        </div>

        <div class="frm frmAwardLetter" style="display: block;">
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>Date of Award Letters<b>*</b></label>
                        <asp:TextBox ID="TxtAwrdLetterDate" CssClass="ctlinput ctlinput-sm datepicker " runat="server"></asp:TextBox>
                    </p>

                    <p>
                        <label>Start Date<b>*</b></label>
                        <asp:TextBox ID="TxtStartDate" CssClass="ctlinput ctlinput-sm datepicker " runat="server"></asp:TextBox>
                    </p>


                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>File of Award Letters</label>
                        <asp:FileUpload ID="fldAwardLetter" CssClass="ctlinput" runat="server" />
                        <asp:HiddenField ID="HdnFldAwardLetter" runat="server" />
                        <span>
                            <asp:LinkButton ID="LnkAwardLetterDwnlod" OnClientClick="DownloadAwardFile(this);return false;" runat="server"></asp:LinkButton></span>
                    </p>
                    <p>
                        <label>Protected Time</label>
                        <asp:TextBox ID="TxtprotectedTime" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>

                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmPI">Principal Investigator (PI) Details <span>( - )</span>
                </h3>
                <p runat="server" style="display: none" id="PMorePi"><span>+</span>  <a class="newPI link" data-frm="frmNewPIDetails">Cancel Adding PI</a></p>
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
                                    <th style="width: 45px; text-align: right">Action</th>
                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="rptrPIDetails" runat="server">
                                    <ItemTemplate>
                                        <tr data-th="Department" piid="<%# Eval("i_ID")%>">
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
                                            <td data-th="Action" style="text-align: right">
                                                <p class="grid-action">

                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick="return delPiRows(this);" CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Pi Detail" alt="" src="../images/icon-delete.png">
                                                    </asp:LinkButton>

                                                </p>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>




                            </tbody>
                        </table>
                    </div>
                    <p runat="server" id="Pmore" class="align-right"><a class="link" onclick="AddMorePI();">+ Add More PI</a></p>
                </div>
            </div>
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


                        <asp:Button CssClass="action" ID="btnMorePiSave" OnClientClick="return SaveMorePi();" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnMorePiCancel" OnClientClick="return ClearCloseMorePiSection();" runat="server" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>
        <div class="row">
            <div class="col-md-12">
                <h3 class="frmHead" data-frm="frmBudget ">Year-wise Budget Distribution(Year-wise Budget Distribution) <span>( - )</span></h3>
            </div>
        </div>
        <div class="frm frmBudget" style="display: block;">

            <%--<div class="frm frmTTSHPiDetail" style="display: block;">--%>
            <div class="row">
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PI Name<b>*</b></label>
                        <asp:DropDownList ID="ddlPIName" choosed="No" CssClass="ctlselect" Enabled="false" runat="server">
                            <asp:ListItem>--Select--</asp:ListItem>
                        </asp:DropDownList>

                    </p>
                </div>
            </div>




            <div class="row">
            <div class="col-md-12 tblSingleBudget" >
              
<p class="ptrY1 pFin">&nbsp;</p>
          
          		<table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY1>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 1</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>
<p class="ptrY2 pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY2>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 2</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrY3 pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY3>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 3</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrY4 pFin" >&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY4>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 4</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrY5 pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY5>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 5</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrY6 pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY6>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 6</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrY7 pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trY7>
                <tr>
                    <td width="120" class="tblFinYearHead">Year 7</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrMonths pFin"> &nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonths>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="ptrMonthsExt pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>

                <p class="pFin">&nbsp;</p>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tabFin" rid=trMonthsExt>
                <tr>
                    <td width="120" class="tblFinYearHead">6 Month (Ext)</td>
                    <td>
		          		<table width="100%" border="0" cellpadding="5" cellspacing="5">
							<tr class="tblFinHead">
                                <td>Factors</td>
                                <td>Est. Budget</td>
                                <td colspan="4" align="center">Actual Spendings <span class="tblFinShowHide">+</span></td>
                            </tr>
							<tr class="tblFinSubHead">
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                           
                            </tr>
                            
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Man Power</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>  
                            <tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Consumables</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                       
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Equipments</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                        
							<tr class="tblFinCtrl tblCtrlHide">
                                <td width="25%">Miscellaneous</td>
                                <td width="15%"><input type="text" placeholder="" ></td>
                                <td  width="15%"><input type="text" placeholder="" ></td>

                            </tr>                                                                                                                
                                 
							<tr class="tblFinTotal">
                                <td width="25%"><b>Total</b></td>
                                <td width="15%"></td>
                                <td  width="15%"></td>

                            </tr>                                                        
                        </table>
                        
                    </td>                  
                </tr>
                </table>
          
            </div>
          </div>




            <%--</div>--%>
        </div>
        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p style="text-align: right">


                    <asp:Button CssClass="action" ID="btnSave" runat="server" OnClientClick="return IsValidate();" Text="Save Details" OnClick="btnSave_Click" />
                    <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" OnClientClick="ResetAll();DoPostBack();return true;" />
                    <asp:Button ID="delete" Style="display: none" runat="server" />
                </p>
            </div>
        </div>
        <div id="HdnSection">
            <asp:HiddenField ID="HdnMode" Value="Insert" runat="server" />
            <asp:HiddenField ID="HdnProjectId" Value="0" runat="server" />
            <asp:HiddenField ID="HdnGranDId" Value="0" runat="server" />
            <asp:HiddenField ID="HdnPi_ID" Value="" runat="server" />
            <asp:HiddenField ID="hdnYearlyBudget" Value="" runat="server" />
            <asp:HiddenField ID="hdnQuarterlyBudget" Value="" runat="server" />
            <asp:HiddenField ID="hdnSelectedPI" Value="" runat="server" />



        </div>
    </div>
</asp:Content>
