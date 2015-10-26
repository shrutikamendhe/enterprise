<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="frmContractMgmt_ContractUsers.aspx.cs"
    Inherits="TTSHWeb.frmContractMgmt_ContractUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/SearchBox.ascx" TagPrefix="uc1" TagName="SearchBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery.MultiFile.js"></script>
    <script src="../Scripts/WebForm/jsforContractUser.js"></script>

    <script type="text/javascript">
        $(function () {





            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CallFunc);

            CallFunc();
        });


        function CallFunc() {
            Autocomplete();
            var mode = $('[id*=HdnMode]').val();
            // HideActionButton(mode);
            //if (mode.toLowerCase() != 'new' && mode.toLowerCase() != 'insert') {
            //    ClearAll(mode);
            //}

            //   pageLoadFunc();
        }
        function callReset() {
            $('[id*=btnResets]').click();
            return false;
        }
        function passMultiple(id) {
            return ConfirmBox('Are you sure to delete this Contract..!!', 'DeleteContractClick(' + id + ')')

        }

        function DeleteContractClick(Id) {
            var HdnContractId = $('[id*=HdnContractId]');
            HdnContractId.val(Id);
            $('[id*=delcont]').click();
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HdnContractFiles" runat="server" />
    <asp:HiddenField ID="HdnMode" Value="New" runat="server" />
    <asp:HiddenField ID="HdnContractMode" Value="Insert" runat="server" />
    <asp:HiddenField ID="HdnProjectId" runat="server" />
    <asp:HiddenField ID="HdnContractId" runat="server" />
    <asp:HiddenField ID="HdnClauseValues" runat="server" />
    <asp:HiddenField ID="HdnselUser" runat="server" />
    <asp:HiddenField ID="HdnContExpDate" runat="server" />
    <div class="projectGrid container" runat="server" id="DivMain">
        <div class="row">
            <div class="col-md-6 col-sm-6 paging">
                <h1>Contract Detail View <span>Search, Filter and Edit Contract Details</span></h1>

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
                                <th style="width: 100px">Project ID</th>
                                <th style="width: 400px">Project Title</th>
                                <th style="width: 150px;">Project Category</th>
                                                             <th>DSRB/IRB No.</th>
                                <th>PI Name</th>
                                <th style="width: 150px;">Available Contract</th>
                                <th style="width: 95px">Action</th>
                            </tr>
                        </thead>

                        <tbody>

                            <asp:Repeater ID="rptrContractDetail" runat="server" OnItemCommand="rptrContractDetail_ItemCommand">
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
                                      
                                        <td data-th="DSRB/IRB No.">

                                            <p><%#Eval("s_IRB_No") %></p>
                                        </td>
                                        <td data-th="PI Name">
                                            <p><%#Eval("PI_Name") %></p>
                                        </td>
                                        <td data-th="Available Contract" title='<%# Eval("Contracts") %>'>
                                            <p><%# Eval("Contract_Status") %></p>

                                        </td>
                                        <td data-th="Action">
                                            <p class="grid-action">
                                                <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# Convert.ToString(Eval("Contract_Status"))=="New" %>'>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cmdAdd" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>'>
												
													<img title="Add Contract Detail" alt="" style="width:20px;" src="Images/Add-New.png"></asp:LinkButton>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# Convert.ToString(Eval("Contract_Status"))!="New" %>'>

                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>'>
												
													<img title="Edit Contract Detail" alt="" src="Images/icon-edit.png"></asp:LinkButton></asp:PlaceHolder>
                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Convert.ToString(Eval("Contract_Status"))!="New" %>'>
                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>' CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Contract Detail" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton></asp:PlaceHolder>

                                                <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# Convert.ToString(Eval("Contract_Status"))!="New" %>'>
                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_Project_ID")%>' CommandName="cmdView" runat="server">
                                                    
												<img title="View Contract Detail" alt="" src="Images/icon-view.png">
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


    <div class="container ContractContainer" id="DivContractDetailContainer" runat="server">
        <span style="float: right; margin-top: 65px">
            <asp:LinkButton ID="lnkback" Text="Back to View" runat="server" OnClick="lnkback_Click"></asp:LinkButton></span>
        <div class="row">
            <div class="col-md-6 col-sm-6">
                <h1>Contract Details <span>Contract Entry Form <b>( Project ID:</b><b id="DispProjectId" runat="server"> </b>)</span></h1>


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
                            <asp:TextBox ID="TxtAlias1" CssClass="ctlinput" runat="server" onmousedown="return false" Enabled="false" onkeydown="return false"></asp:TextBox>
                        </p>

                        <p>
                            <label>Short Title</label>
                            <asp:TextBox ID="TxtShortTitle" CssClass="ctlinput" runat="server" onmousedown="return false" Enabled="false" onkeydown="return false"></asp:TextBox>
                        </p>

                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Project Category <b>*</b></label>


                            <asp:TextBox ID="TxtprojCategory" CssClass="ctlinput" runat="server" onmousedown="return false" Enabled="false" onkeydown="return false"></asp:TextBox>
                        </p>
                        <p>
                            <label>Alias 2</label>
                            <asp:TextBox ID="TxtAlias2" CssClass="ctlinput" runat="server" onmousedown="return false" Enabled="false" onkeydown="return false"></asp:TextBox>
                        </p>
                        <p>
                            <label>DSRB/IRB No</label>
                            <asp:TextBox ID="TxtIrbNo" CssClass="ctlinput" runat="server" onmousedown="return false" Enabled="false" onkeydown="return false"></asp:TextBox>

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



                    <div class="tblResposiveWrapper">

                        <table id="tblPiDetail" class="tblResposive">
                            <thead>
                                <tr>
                                    <th style="width: 500px">Department</th>
                                    <th style="width: 150px">PI Name</th>
                                    <th style="width: 100px">Email</th>
                                    <th style="width: 80px">Phone</th>
                                    <th style="width: 120px; text-align: right">PI MCR No.</th>

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
            <div class="col-md-12 col-sm-12">
                <h3 class="frmHead" data-frm="frmCollabDetails">Associated Collaborator Details<span>( - )</span>
                </h3>
            </div>
        </div>
        <div class="frm frmCollabDetails" style="display: block;">
            <div class="row">

                <div class="col-md-12 col-sm-12">



                    <div class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 200px; min-height: 0px;">

                        <table id="Table1" class="tblResposive">
                            <thead>
                                <tr>

                                    <th style="width: 200px">Collaborator</th>
                                    <th>Email Address1</th>
                                    <th>Phone No</th>
                                    <th style="width: 150px">Institution</th>
                                    <th>Country</th>
                                    <th style="width: 180px; text-align: center">Contract Request Date</th>
                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="RptrCollaborator" runat="server">
                                    <ItemTemplate>
                                        <tr>

                                            <td data-th="Collaborator">
                                                <p><%# Eval("s_Name") %></p>
                                            </td>
                                            <td data-th="Email Address1">
                                                <p><%# Eval("s_Email1") %></p>
                                            </td>
                                            <td data-th="Phone No">
                                                <p><%# Eval("s_PhoNo") %></p>
                                            </td>
                                            <td data-th="Institution">
                                                <p><%# Eval("s_Institution") %></p>
                                            </td>

                                            <td data-th="Country">
                                                <p><%# Eval("Country_Name") %></p>
                                            </td>
                                            <td style="text-align: center" data-th="Contract Request Date">
                                                <p><%# Eval("s_date") %></p>

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
                <h3 class="frmHead" data-frm="frmProCat">Create Contract <span>( - )</span></h3>
                <p runat="server" id="PNewCollaborator"><span runat="server" id="spanMsg">+</span> <a class="newPI link" data-frm="frmNewPIDetails">Create New Contract </a></p>
            </div>

        </div>
        <div class="frm frmProCat" style="display: block;">

            <div class="row">
                <div class="col-md-12">

                    <div id="Div1" class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: auto; max-height: 250px; min-height: 20px;">

                        <table id="tblContract" class="tblResposive">
                            <thead>
                                <tr>
                                    <th>Contract Id</th>
                                    <th style="width: 150px">Contract Name</th>
                                    <th>Contract Category</th>
                                    <th>Contract Status</th>
                                    <th>Contract Expiry Date </th>
                                    <th>Amended Contract Expiry Date</th>
                                    <th style="width: 95px">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RptContract" OnItemCommand="RptContract_ItemCommand" runat="server">
                                    <ItemTemplate>
                                        <tr contractid='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
                                            <td data-th="Contract Id">
                                                <p><%# Eval("s_ContractId") %></p>
                                            </td>
                                            <td data-th="Contract Name">
                                                <p><%# Eval("s_Contract_Name")%></p>
                                            </td>
                                            <td data-th="Contract Category">
                                                <p><%# Eval("s_ContractCategory") %></p>
                                            </td>
                                            <td data-th="Contract Status">
                                                <p><%# Eval("s_ContractStatus") %></p>
                                            </td>
                                            <td data-th="Contract Expiry Date">
                                                <p><%# Eval("dt_Expiry_Date") %></p>
                                            </td>
                                            <td data-th="Amended Contract Expiry Date">
                                                <p><%# Eval("dt_NewExpiry_Date") %></p>
                                            </td>
                                            <td data-th="Action">
                                                <p class="grid-action">

                                                    <asp:LinkButton ID="ImgEdit" runat="server" CommandName="cmdEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'>
												
													<img title="Edit Contract Detail" alt="" src="Images/icon-edit.png"></asp:LinkButton>


                                                    <asp:LinkButton ID="ImgDelete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>'
                                                        OnClientClick='<%# String.Format("return passMultiple(\"{0}\");",  Eval("i_ID")) %>'
                                                        CommandName="cmdDelete" runat="server">
                                                        <img title="Delete Contract Detail" alt="" src="Images/icon-delete.png">
                                                    </asp:LinkButton>


                                                    <asp:LinkButton ID="ImgView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.i_ID")%>' CommandName="cmdView" runat="server">
                                                    
												<img title="View Contract Detail" alt="" src="Images/icon-view.png">
                                                    </asp:LinkButton>
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

         <%-- <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>

        <div class="frmNewPIDetails" style="display: none;">
            <span style="float: right;">
                <asp:LinkButton ID="LinkButton2" Text="Back to Contract View" runat="server" Style="display: none" OnClientClick="return OnCancel();"></asp:LinkButton></span>
            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <h3 id="h3" runat="server" style="color: rgb(228, 16, 83); margin-bottom: 1em;">Record New Contract Details					                  
                </h3>
                </div>
            </div>
            <div class="row">

                <div class="col-md-6 col-sm-6">

                    <p>
                        <label>List of All Associated Collaborator<b>*</b></label>
                    </p>
                    <div id="divCollaborator" style="width: auto; max-height: 150px; overflow-y: auto; overflow-x: hidden; margin-top: -15px">
                        <asp:CheckBoxList ID="chkCollaboratorList" CssClass="CheckboxList" runat="server"></asp:CheckBoxList>
                    </div>

                </div>
                <div class="col-md-6 col-sm-6">

                    <p>
                        <label>Contract Name <b>*</b></label>
                        <asp:TextBox ID="TxtContractName" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>
                    <p>
                        <label>Contract Category<b>*</b></label>
                        <asp:DropDownList ID="ddlContractCategory" CssClass="ctlselect" runat="server">
                        </asp:DropDownList>
                    </p>

                    <p>
                        <label>Contract Id<b>*</b></label>
                        <asp:TextBox ID="TxtContractId" CssClass="ctlinput" runat="server"></asp:TextBox>

                    </p>

                </div>

            </div>



            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmProkeyInfo">Contract Detail <span>( - )</span></h3>
                </div>
            </div>

            <div class="frm frmProkeyInfo" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>List of Clause (Applicable/Not Applicable)<b>*</b></label>
                        </p>
                        <div id="divClause" style="width: auto; max-height: 150px; overflow-y: auto; overflow-x: hidden; margin-top: -12px;">
                            <asp:CheckBoxList ID="chkClause" CssClass="CheckboxList" CellPadding="0" onclick="return onClauseChange(this);" CellSpacing="0" runat="server"></asp:CheckBoxList>
                        </div>

                    </div>
                    <div class="col-md-6 col-sm-6">

                        <p>
                            <label>Governing Law Country<b>*</b></label>

                            <asp:HiddenField ID="HdnGovCountry" runat="server" />
                            <asp:TextBox ID="TxtGovCountry" placeholder="Type Keyword to search Country" CssClass="ctlinput" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>File for Correspondance</label>

                            <asp:FileUpload ID="fldCorespondace" title="File for Correspondance" CssClass="ctlinput" runat="server" />



                            <asp:HiddenField ID="hdnCoresPath" runat="server" />
                        </p>
                        <p>
                            <span>
                                <asp:LinkButton ID="lnkDwnldCorespondace" runat="server" OnClick="lnkDwnldCorespondace_Click"></asp:LinkButton></span>
                        </p>
                        <p>
                            <label>Last Updated Date</label>
                            <asp:TextBox ID="TxtLastDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>

                        </p>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px">
                    <div class="col-md-12">
                        <p>Display Selected Clauses</p>

                        <div id="Div2" class="tblResposiveWrapper" style="overflow-x: hidden; overflow-y: no-display; max-height: 250px; min-height: 0px;">

                            <table id="tblClauseDetail" class="tblResposive">
                                <thead>
                                    <tr>

                                        <th style="width: 350px">Selected Clauses</th>
                                        <th>Status</th>
                                        <th>Comments</th>
                                        <th>Proposed Changes</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RptClauseDetail" OnItemDataBound="RptClauseDetail_ItemDataBound" runat="server">
                                        <ItemTemplate>
                                            <tr clauseid='<%# Eval("i_Contract_Clause_ID") %>'>
                                                <td data-th="Selected Clauses">
                                                    <p><%# Eval("Clause_Name") %></p>
                                                </td>
                                                <td data-th="Status">
                                                    <p>

                                                        <asp:HiddenField ID="HdnClauseId" Value='<%# Eval("s_Status") %>' runat="server" />
                                                        <asp:DropDownList runat="server" onchange="EnableDisableClauseControls(this)" ID="ddlstatus">
                                                            <asp:ListItem Value="1" Text="Compliant"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Non Compliant"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="Acceptable"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </p>
                                                </td>
                                                <td data-th="Comments">
                                                    <p>
                                                        <asp:TextBox ID="TxtComments" TextMode="MultiLine" onkeypress="return BlockComma();" onpaste="return BlockComma();" Width="300px" Text='<%# Eval("s_Comments") %>' runat="server"></asp:TextBox>
                                                    </p>
                                                </td>
                                                <td data-th="Proposed Changes">
                                                    <p>
                                                        <asp:TextBox ID="TxtProposedChanges" TextMode="MultiLine" onkeypress="return BlockComma();" onpaste="return BlockComma();" Width="300px" Text='<%# Eval("s_Proposed_Changes") %>' runat="server"></asp:TextBox>
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

            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmStatusFile">Contract Status &  File Section<span>( - )</span></h3>
                </div>
            </div>
            <div class="frm frmStatusFile" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Contract Status<b>*</b></label>
                            <asp:DropDownList ID="ddlContractStatus" onchange="EnableControlOnContractStatus();" CssClass="ctlselect" runat="server">
                            </asp:DropDownList>
                        </p>
                        <p>
                            <label>Upload Contract File</label>
                            <%--<input type="text" class="txtUpload" style="width: 350px;" readonly="true" id="TxtContractFile" />

									<span class="btn btn-default btn-file action" style="width: 210px; height: 10px; padding-left: 15px; padding-right: 0px;">Browse...
									</span>--%>
                            <asp:FileUpload ID="fldContractFile" AllowMultiple="false" title="Contract File" CssClass="multi ctlinput" runat="server" />


                            <div class="MultiFile-list" id="multilistdiv" runat="server">
                            </div>


                        </p>
                        <p>
                            <label>
                                Amendments</label>
                            <asp:DropDownList ID="ddlAmendments" runat="server" CssClass="ctlselect" onchange="EnableOnAmendments()">
                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </p>
                        <p>
                            <label>
                                Upload Amendment Contract File</label>
                            <%--<input type="text" class="txtUpload" readonly="true" id="TxtAmendmentFile" />

									<span class="btn btn-default btn-file action">Browse...</span><%--onchange="ApplyFileUpload();" --%>
                            <asp:FileUpload ID="fldAmendmentFile" runat="server" title="Amendment Contract File" CssClass="ctlinput" />



                            <asp:HiddenField ID="HdnAmendPath" runat="server" />
                        </p>
                        <p>
                            <span>
                                <asp:LinkButton ID="lnkDwnldAmendment" runat="server" Style="text-wrap: normal;" OnClick="lnkDwnldAmendment_Click"></asp:LinkButton>
                            </span>
                        </p>
                        <p>
                            <label>New Contract Expiry Date</label>
                            <asp:TextBox ID="TxtNContractExpiryDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>
                        </p>


                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>
                                <asp:Label runat="server" ID="spanContStatusDate" Text="Contract Status Date"></asp:Label>
                                <b>*</b>
                            </label>


                            <asp:TextBox ID="TxtContStartDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server" TabIndex="1"></asp:TextBox>
                        </p>

                        <p>
                            <label>Effective Date (Study Start Date)<b>*</b></label>


                            <asp:TextBox ID="TxtEffectiveDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server" TabIndex="1"></asp:TextBox>
                        </p>
                        <p>
                            <label>Date of Last Signed<b>*</b></label>
                            <asp:TextBox ID="TxtDateofLast" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            <label>Contract Expiry Date<b>*</b></label>


                            <asp:TextBox ID="TxtContractExpDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server" TabIndex="1"></asp:TextBox>
                        </p>



                        <p>
                            <label>Contract Finalization Date</label>
                            <asp:TextBox ID="TxtContractFinalizeDate" CssClass="ctlinput ctlinput-sm datepicker" onpaste="return false;" onkeypress="return false;" runat="server"></asp:TextBox>
                        </p>

                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-md-12">
                    <h3 class="frmHead" data-frm="frmbudgetExp">Budget and Expense<span>( - )</span></h3>
                </div>
            </div>

            <div class="frm frmbudgetExp" style="display: block;">
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Hospital/Procedure Cost </label>

                            <asp:TextBox ID="TxtprocedureCost" onkeypress="return IsInteger(event,this);" onkeyup="CalculateProjectBudgetCash();" onpaste="return false;" CssClass="ctlinput" runat="server"></asp:TextBox>

                        </p>
                        <p>
                            <label>Investigator Fees </label>
                            <asp:TextBox ID="TxtInvestigatorFees" onkeypress="return IsInteger(event,this);" onkeyup="CalculateProjectBudgetCash();" onpaste="return false;" CssClass="ctlinput" runat="server"></asp:TextBox>

                        </p>



                    </div>
                    <div class="col-md-6 col-sm-6">
                        <p>
                            <label>Co-Ordinator Fees</label>
                            <asp:TextBox ID="TxtCoOrdinatorFess" onkeypress="return IsInteger(event,this);" onkeyup="CalculateProjectBudgetCash();" onpaste="return false;" CssClass="ctlinput" runat="server"></asp:TextBox>
                        </p>

                        <p>
                            <label>Project Budget Cash</label>
                            <asp:TextBox ID="TxtProjectBudgetCash" onkeydown="return false;" onfocus="return false;"
                                onmouseenter="return false;"
                                onmousedown="return false;" onpaste="return false;" onkeypress="return IsInteger(event,this);" CssClass="ctlinput" runat="server"></asp:TextBox>

                        </p>

                    </div>

                </div>
            </div>
            <div class="row margin-top frmAction">
                <div class="col-md-12">
                    <p style="float: right">
                        <table style="float: right">
                            <tr>
                                <td style="width: 70px;">
                                    <asp:Button CssClass="action" ID="btnContractSave" runat="server" Text="Save" Width="60px" OnClick="btnContractSave_Click" /></td>
                                <td>
                                    <asp:Button CssClass="action" ID="btnContractCancel" runat="server" Text="Cancel" OnClientClick="return OnCancel();" OnClick="btnContractCancel_Click" /></td>
                            </tr>
                        </table>






                    </p>
                    <p style="float: right">
                        <asp:Button ID="btnCReset" CssClass="action" runat="server" Text="Cancel Contract" Style="display: none;" OnClientClick="AfterSave();" />
                    </p>
                </div>
            </div>
        </div>
       <%--  </ContentTemplate>
           <%-- <Triggers>--%>
             <%--   <asp:AsyncPostBackTrigger ControlID="btnResets" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="RptContract" EventName="ItemCommand" />
              <%--  <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
               <asp:AsyncPostBackTrigger ControlID="delcont" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>--%>
        <div class="row margin-top frmAction">
            <div class="col-md-12">
                <p style="text-align: right">


                    <asp:Button CssClass="action" ID="btnSave" Style="display: none" runat="server" Text="Save Details" OnClientClick="return ValidateFinalSave();" OnClick="btnSave_Click" />
                    <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnResets" Style="display: none" runat="server" OnClick="btnResets_Click" />
                    <asp:Button ID="delcont" Style="display: none" runat="server" OnClick="delcont_Click" />
                </p>
            </div>
        </div>



    </div>
</asp:Content>

