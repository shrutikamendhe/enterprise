<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="FrmGrant_Application.aspx.cs" Inherits="TTSHWeb.FrmGrant_Application" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/GrantApplication.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/Frm_GrantMaster.js"></script>
    <script type="text/javascript">
        function CheckProjectCreation(obj) {
           // var f = $(obj).parent().find('input[type=hidden]').val();
           // if (f != "") {
           //     if (f.toString().toLowerCase() == "false") {
           //         MessageBox("Please Apply grant application for Parent project first");
           //         return false;
           //     }
           // }
           // return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="HdnProjectId" runat="server" Value="" />
    <asp:HiddenField ID="HdnMode" runat="server" Value="" />
    <asp:HiddenField ID="HdnGrantId" runat="server" Value="" />
    <asp:HiddenField ID="HidenGrantMaster" runat="server" Value="" />
    <asp:HiddenField ID="HdnPi_ID" runat="server" />
    <asp:HiddenField ID="HdnUpdateMode" runat="server" Value="0" />
    <asp:HiddenField ID="HdnParentProjectAmount" runat="server" Value="0" />
    <asp:HiddenField ID="HdnMaxChildDurationId" runat="server" Value="0" />
    <asp:HiddenField ID="HdnMaxChildduration" runat="server" Value="0" />

    <asp:HiddenField ID="HdnParentProjCount" Value="0" runat="server" />

    <asp:HiddenField ID="HdnForChild" Value="0" runat="server" />
    <asp:HiddenField ID="HdnChildParent" Value="0" runat="server" />
    <div class="GrantApplication container" runat="server" id="DivMain">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Grant Application <span>Search, Filter and Edit Grant Application Details</span></h1>

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
                                <th style="width: 400px">Project Title</th>
                                <th style="width: 150px;">Application Outcome Status</th>
                                <th>DSRB/IRB No</th>
                                <th style="width: 150px;">PI Name</th>
                                <th>IsChild Or Parent</th>
                                <th style="width: 95px">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptrGrantMaster" runat="server" OnItemDataBound="rptrGrantMaster_ItemDataBound" OnItemCommand="rptrGrantMaster_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td data-th="Project ID">
                                            <p><%#Eval("s_Display_Project_ID") %></p>
                                        </td>
                                        <td data-th="Project Title">
                                            <p><%#Eval("s_Project_Title") %></p>
                                        </td>
                                        <td data-th="Application Outcome Status">
                                            <p><%#Eval("s_OutcomeStatus") %></p>
                                        </td>


                                        <td data-th="DSRB/IRB No">
                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%# Eval("PI_NAME") %></p>
                                        </td>
                                        <td id="td" runat="server" data-th="IsChild Or Parent">
                                            <p><%# Eval("IsChildorParent") %></p>
                                            <span id="parentProjectCount" runat="server" style="display: none"><%# Eval("parentProjectCount") %></span>
                                            <span id="ChildParentProject" runat="server" style="display: none"><%# Eval("ChildParentProject") %></span>
                                            <span id="ParentProject" runat="server" style="display: none"><%# Eval("ParentProject") %></span>
                                        </td>
                                        <td data-th="Action">
                                            <p class="grid-action">
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# Eval("Status").ToString() == "New" %>'>
                                                    <asp:HiddenField ID="HdnGrantFill" Value='<%# Eval("GrantDetails_Applied") %>' runat="server" />
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" OnClientClick="ResetAll();DoPostBack();" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID") + "|" +  Eval("parentProjectCount") + "|" +  Eval("Prog") +"|"+  Eval("Multi") + "|" + Eval("IsChildorParent") %>'>
													        <img title="Add Grant Application" alt="" style="width:20px;" src="Images/Add-New.png">
                                                    </asp:LinkButton>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Eval("Status")=="Edit" %>'>
                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" OnClientClick="ResetAll();DoPostBack();return true;" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
													<img title="Edit Grant Application" alt="" src="Images/icon-edit.png">
                                                    </asp:LinkButton>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Status")=="Edit" %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick='<%# String.Format("return ConfirmDelete(\"{0}\");",  Eval("i_ID")) %>' CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Grant Application" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# "Edit,View".Contains(Eval("Status").ToString()) %>'>
                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' OnClientClick="ResetAll();DoPostBack();return true;" CommandName="cmdView" runat="server">
												<img title="View Grant Application" alt="" src="Images/icon-view.png">
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
        <div class="row" id="Paging">
            <div class="col-md-6 paging">
                <div class="page-info">
                    <%--<h3>Results Found : 18  </h3>--%>
                    <p>Showing Page 2 of Total 4 Pages | <a href="#">First Page</a> | <a href="#">Last Page</a></p>
                </div>
            </div>
            <div class="col-md-6 paging">
                <div class="pages">
                    <%--	<a class="current-page" href="#">1</a> <a href="#">2</a> <a href="#">3</a> <a href="#">4</a>--%>
                </div>
            </div>
        </div>
        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p style="text-align: left">
                    <asp:Button CssClass="action" ID="btnNew" runat="server" Text="Add New Project" OnClientClick="window.open( 'frmProject_Master.aspx?NewPage=true','_blank' );return false;" />

                </p>
            </div>
        </div>

    </div>
    <!--Project Start View-->
    <div class="container GrantContainer" id="DivEntry" runat="server">
        <span style="float: right; margin-top: 65px">
            <asp:LinkButton ID="lnkback" Text="Back to View" runat="server" OnClick="lnkback_Click"></asp:LinkButton></span>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Grant Application Form</h1>
                <span>Grant Form <b>( Project ID:</b><b id="DispProjectId" runat="server"> </b>)</span>


            </div>

        </div>
        <div class="frmProject">
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 class="frmHead" data-frm="frmDetails">Project Details<span>( - )</span>
                    </h3>
                </div>
            </div>
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

                            <asp:TextBox ID="txtProjCategory" CssClass="ctlinput" runat="server" Enabled="false" />
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
                <p runat="server" id="PMorePi"><span>+</span>  <a class="newPI link" data-frm="frmNewPIDetails">Record New PI Details</a></p>

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

        <div class="frmNewPIDetails" style="display: none;">
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

                                <asp:TextBox ID="TxtNewDepartment" onpaste="return false;" CssClass="ctlinput" runat="server"></asp:TextBox>

                            </p>
                            <p>
                                <label>First / Given Name <b>*</b></label>
                                <asp:TextBox ID="txtPiFirstName" CssClass="ctlinput" onkeypress="return RestrictPower(event);" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>PI Email <b>*</b></label>
                                <asp:TextBox ID="txtPIEmailAddress" onblur="checkValidEmail(this);" CssClass="ctlinput" runat="server"></asp:TextBox>
                            </p>


                        </div>
                        <div class="col-md-6 col-sm-6">
                            <p>
                                <label>PI MCR No.<b>*</b></label>
                                <asp:TextBox ID="txtPIMCR_NO" CssClass="ctlinput" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>Last Name / Surname <b>*</b></label>
                                <asp:TextBox ID="txtPiLastName" CssClass="ctlinput" onkeypress="return RestrictPower(event);" runat="server"></asp:TextBox>
                            </p>
                            <p>
                                <label>Phone No.</label>
                                <asp:TextBox ID="txtPiPhNo" CssClass="ctlinput" onKeypress="return SingaporePhformat();" onpaste="return false" runat="server"></asp:TextBox>
                            </p>

                        </div>
                    </div>
                    <div class="row margin-top frmAction">
                        <div class="col-md-12">
                            <p style="text-align: right">


                                <asp:Button CssClass="action" ID="btnPISave" runat="server" Text="Save" OnClick="btnPISave_Click" />
                                <asp:Button CssClass="action" ID="btnPICancel" runat="server" Text="Reset" />

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
                        <asp:TextBox ID="TxtDepartment" onpaste="return false;" onblur="ClearOnblur(this);" onKeydown="items('');" CssClass="ctlinput" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <label>PI Email </label>
                        <asp:TextBox ID="txtPIEmail" CssClass="ctlinput" runat="server" Enabled="false"></asp:TextBox>
                    </p>
                    <p>
                        <label>Phone No.</label>
                        <asp:TextBox ID="txtPiPhoneNo" CssClass="ctlinput" runat="server" Enabled="false"></asp:TextBox>
                    </p>

                </div>
                <div class="col-md-6 col-sm-6">
                    <p>
                        <label>PI Name<b>*</b></label>
                        <asp:HiddenField ID="HdnpiId" runat="server" />
                        <asp:HiddenField ID="HdnPITxt" runat="server" />
                        <asp:TextBox ID="TxtPIName" onpaste="return false;" onblur="CheckPiOnBlur(this);" CssClass="ctlinput" runat="server"></asp:TextBox>

                        <%--                        <asp:TextBox ID="TxtPIName" onpaste="return false;" onblur="CheckPiOnBlur(this);" onKeydown="items('pi');" onchange="items('pi')"  CssClass="ctlinput" runat="server"></asp:TextBox>--%>
                    </p>
                    <p>
                        <label>PI MCR No.</label>
                        <asp:TextBox ID="txtPiMCRNo" CssClass="ctlinput" runat="server" Enabled="false"></asp:TextBox>
                    </p>
                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="text-align: right">


                        <asp:Button CssClass="action" ID="btnMorePiSave" runat="server" Text="Save" />
                        <asp:Button CssClass="action" ID="btnMorePiCancel" runat="server" Text="Reset" />

                    </p>
                </div>
            </div>

        </div>





        <asp:UpdatePanel ID="updatepaneldoc" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h3 class="frmHead" data-frm="frmGrant">Grant Application Details<span>( - )</span>
                        </h3>
                    </div>
                </div>

                <div class="row frmGrant" id="divGrantDetails" runat="server">
                    <div class="col-md-6 col-sm-6">
                        <p id="parentprojectname" runat="server">
                            <label>Parent Project Name</label>

                            <asp:TextBox ID="TxtParentProjectName" runat="server" CssClass="ctltext" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                        </p>

                        <p id="parentprojectAmt" runat="server">
                            <label>Parent Project Amount Requested</label>
                            <asp:TextBox ID="txtparentprojectamount" runat="server" CssClass="ctlinput" Enabled="false"></asp:TextBox>
                        </p>

                        <p>
                            <label>Application Id<b>*</b></label>

                            <asp:TextBox ID="TxtApplicationId" runat="server" CssClass="ctlinput s_Application_ID"></asp:TextBox>
                        </p>
                        <p>
                            <label>Grant Type <b>*</b></label>
                            <asp:DropDownList ID="ddlGrantType" CssClass="ctlselect i_GrantType_ID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGrantType_SelectedIndexChanged"></asp:DropDownList>

                        </p>

                        <p>
                            <label>Grant Sub-Sub Category<b>*</b></label>
                            <asp:DropDownList ID="ddlGrantSubType2" CssClass="ctlselect i_Grant_Sub_SubType_ID" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlGrantSubType2_SelectedIndexChanged"></asp:DropDownList>

                        </p>

                        <p>
                            <label>Grant Awarding Organization<b>*</b></label>

                            <asp:DropDownList ID="ddlGrantAwardOrgan" CssClass="ctlselect i_AwardOrganization" runat="server"></asp:DropDownList>
                        </p>
                        <p>
                            <label>Name Of Grant</label>
                            <asp:TextBox ID="txtGrantrName" runat="server" CssClass="ctlinput s_Grant_Name" />

                        </p>
                        <p>
                            <label>Amount Requested<b>*</b></label>

                            <asp:TextBox ID="TxtAmountRequested" runat="server" CssClass="ctlinput i_Amount_Requested" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                        </p>
                        <p>
                            <label>Submission Status<b>*</b></label>
                            <asp:DropDownList ID="ddlSubmissionStatus" CssClass="ctlselect i_SubmissionStatus" runat="server"></asp:DropDownList>

                        </p>



                        <p>
                            <label>Mentor</label>

                            <asp:TextBox ID="TxtMentor" runat="server" CssClass="ctlinput s_Mentor"></asp:TextBox>
                        </p>

                        <p>
                            <label>Outcome</label>

                            <asp:DropDownList ID="ddlOutCome" CssClass="ctlselect i_Outcome" runat="server"></asp:DropDownList>
                        </p>


                        <p>
                            <label>Reviewers Comment(Comment If Not Successful)<b id="commentsasterik" runat="server">*</b></label>
                            <asp:TextBox ID="txtReviewersComments" runat="server" CssClass="ctltext s_Reviewers_Comments" TextMode="MultiLine" />
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">


                        <p id="parentprojectId" runat="server" class="parentprojectId">
                            <label>Parent Project Id</label>
                            <asp:TextBox ID="TxtParentProjectId" runat="server" CssClass="ctlinput" Enabled="false"></asp:TextBox>
                        </p>
                        <p id="parentprojectdur" runat="server">
                            <label>Parent Project Duration(In Years)</label>
                            <asp:TextBox ID="txtparentprojectduration" runat="server" CssClass="ctlinput " Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="HdnParentProjectDurId" runat="server" Value="" />
                        </p>
                        <p id="RemainingProjectAmt" runat="server">
                            <label>Remaining Project Amount</label>
                            <asp:TextBox ID="txtRemainingAmount" runat="server" CssClass="ctlinput" Enabled="false"></asp:TextBox>
                        </p>
                        <p>
                            <label>Grant Application Date<b>*</b></label>
                            <asp:TextBox ID="TxtGrantAppDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker dt_ApplicationDate"></asp:TextBox>
                        </p>
                        <p>
                            <label>Grant Sub Category<b>*</b></label>
                            <asp:DropDownList ID="ddlGrantSubType1" AutoPostBack="true" CssClass="ctlselect i_Grant_SubType_ID" runat="server" OnSelectedIndexChanged="ddlGrantSubType1_SelectedIndexChanged"></asp:DropDownList>

                        </p>

                        <p>
                            <label>Grant Sub-Sub-Sub Category<b>*</b></label>
                            <asp:DropDownList ID="ddlGrantSubType3" CssClass="ctlselect i_Grant_Sub_Sub_SubType_ID" runat="server"></asp:DropDownList>

                        </p>
                        <p>
                            <label>Country Name<b>*</b></label>
                            <%--                            <asp:DropDownList ID="ddlCountryName" CssClass="ctlselect i_AwardCountryID" runat="server"></asp:DropDownList>--%>
                            <asp:TextBox ID="txtCountryName" onpaste="return false;" CssClass="ctlinput" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="HdnCountryId" runat="server" Value="" />

                        </p>
                        <p class="duration">
                        </p>
                        <p>
                            <label>Duration(In Years)<b>*</b></label>
                            <asp:DropDownList ID="ddlDuration" CssClass="ctlselect s_Duration" runat="server"></asp:DropDownList>
                        </p>


                        <p>
                            <label>Old Application Id(If it's a resubmission)</label>
                            <asp:TextBox ID="TxtOldApplicationId" runat="server" CssClass="ctlinput s_Old_Application_ID"></asp:TextBox>
                        </p>

                        <p>
                            <label>FTE(Projected TimeLine)</label>
                            <asp:TextBox ID="txtFTe" runat="server" CssClass="ctlinput i_FTE" onkeypress="return isNumberKeyWithMax(event,this,1)"></asp:TextBox>

                        </p>
                        <p class="AwardLetter">
                            <label>Date Of Award Letter<b>*</b></label>

                            <asp:TextBox ID="TxtAwardLetter" runat="server" CssClass="ctlinput ctlinput-sm datepicker dt_AwardDate"></asp:TextBox>
                            <asp:HiddenField ID="hidAwardLetter" runat="server" Value="" />
                        </p>
                        <p>
                            <label>Closing Date</label>

                            <asp:TextBox ID="TxtClolsingDate" runat="server" CssClass="ctlinput ctlinput-sm datepicker dt_Closing_Date"></asp:TextBox>

                        </p>

                    </div>
                </div>



            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlGrantType" EventName="SelectedIndexChanged" />

                <asp:AsyncPostBackTrigger ControlID="ddlGrantSubType1" EventName="SelectedIndexChanged" />

                <asp:AsyncPostBackTrigger ControlID="ddlGrantSubType2" EventName="SelectedIndexChanged" />

                <%-- <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>

        <%--<div id="divAction"  runat="server" >--%>
        <div class="row margin-top frmAction">
            <div class="col-md-12 col-sm-12">
                <p style="text-align: right">
                    <asp:Button CssClass="action" ID="btnSave" runat="server" Text="Save Details" OnClick="btnSave_Click" OnClientClick="return SetModelParameters();" />
                    <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button ID="delete" Style="display: none" runat="server" />
                </p>
                <%--  </div>--%>
            </div>
        </div>
    </div>



    <script type="text/javascript">
        $(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(CallAutocomplete);
            // Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CallAutocomplete);
        });
        function CallNewPi() {
            ClearCloseNewPiSection();
            SearchText('<%=TxtNewDepartment.ClientID%>', '<%=HdnNewDeptId.ClientID%>', 10, "Department~spAutoComplete");
            SearchText('<%=TxtDepartment.ClientID%>', '<%=HdnDeptId.ClientID%>', 10, "Department~spAutoComplete", FillPi, '<%=HdnDeptTxt.ClientID%>');
            SearchText('<%=txtCountryName.ClientID%>', '<%=HdnCountryId.ClientID%>', 10, "Country~spAutoComplete");
            $('[id*=TxtNewDepartment]').focus();
            return false;
        }

        function ClearPiDetails() {
            $('#<%=TxtDepartment.ClientID%>').bind("mouseup", function (e) {
                var $input = $(this),
					oldValue = $input.val();

                if (oldValue == "") return;

                // When this event is fired after clicking on the clear button
                // the value is not cleared yet. We have to wait for it.
                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue == "") {
                        $("[id*=HdnDeptId]").val('');
                        if ($("[id*=HdnDeptId]").val().trim() == '') {
                            $("[id*=TxtPIName]").autocomplete({ disabled: true });
                        }
                        else {
                            $("[id*=TxtPIName]").autocomplete({ disabled: false });
                        }
                        items('');
                        $input.trigger("cleared");
                    }
                }, 1);
            });

            $('#<%=TxtPIName.ClientID%>').bind("mouseup", function (e) {
                var $input = $(this),
					oldValue = $input.val();

                if (oldValue == "") return;

                // When this event is fired after clicking on the clear button
                // the value is not cleared yet. We have to wait for it.
                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue == "") {
                        items('');
                        $input.trigger("cleared");
                    }
                }, 1);
            });

            $('#<%=txtCountryName.ClientID%>').bind("mouseup", function (e) {
                var $input = $(this),
                       oldValue = $input.val();

                if (oldValue == "") return;

                // When this event is fired after clicking on the clear button
                //  the value is not cleared yet. We have to wait for it.
                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue == "") {
                        $('#<%=HdnCountryId.ClientID%>').val(0);
                        $input.trigger("cleared");
                    }
                }, 1);
            });
        }
    </script>
</asp:Content>
