<%@ Page Title="" Language="C#" MasterPageFile="~/TTSHMasterPage/TTSH.Master" AutoEventWireup="true" CodeBehind="Frm_DMS.aspx.cs"
    Inherits="TTSHWeb.Frm_DMS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/DMS.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updatepaneldoc" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--  Main Seach Div--%>
            <div>
                <div class="roleManager container" runat="server" id="documentsearch">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <h1>Document Management System<span>Search By Document Title,Document Description</span>
                            </h1>
                            <%--     </div>--%>
                            <div id="SearchDocument" runat="server">
                                <div class="row" id="SearchContainer">
                                    <%--  <div class="col-md-6">--%>
                                    <div class="col-md-12 col-sm-12">

                                        <asp:HiddenField ID="HdnNeProjectId" runat="server" />
                                        <asp:HiddenField ID="Hidden" runat="server" />
                                        <p>

                                            <asp:TextBox ID="txtDoucumentSearch" runat="server" onkeypress="searchKeyPress(event);" placeholder="Search by Document Title,Document Description" CssClass="ctlinput" />
                                            <asp:Button ID="txtSearchDoc" OnClick="txtSearchDoc_Click" OnClientClick="return ValidateSearch();" runat="server" Text="Search" CssClass="action dmsaction" />
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="action dmsaction" OnClick="btnClear_Click"  OnClientClick="return ValidateClear();" />
                                        </p>
                                    </div>
                                    <%--       </div>--%>
                                </div>






                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="tblResposiveWrapper">
                                            <table id="tblResposive">
                                                <thead>
                                                    <tr>

                                                        <th width="300">Project Title</th>
                                                        <th width="100">Document Title</th>
                                                        <th width="150">Document Description</th>
                                                        <th width="150">Document Cateogry</th>

                                                        <th style="text-align: center" width="95">Action</th>
                                                    </tr>
                                                </thead>

                                                <tbody>

                                                    <asp:Repeater ID="rptrDocument" runat="server" OnItemCommand="rptrDocument_ItemCommand" OnItemDataBound="rptrDocument_ItemDataBound1">
                                                        <ItemTemplate>


                                                            <tr runat="server" id="DocumentUploaded">
                                                                <td data-th="Project Title">
                                                                    <p style="text-wrap: normal"><%#Eval("ProjectTile") %></p>
                                                                </td>
                                                                <td data-th="Document Title">
                                                                    <p><%#Eval("DocTitle") %></p>
                                                                </td>
                                                                <td data-th="Document Description">
                                                                    <p><%#Eval("DocDescription") %></p>
                                                                </td>
                                                                <td data-th="Document Cateogry">
                                                                    <p><%#Eval("DocCategory") %></p>
                                                                </td>


                                                                <td style="text-align: center;" data-th="Action">
                                                                    <p class="grid-action">
                                                                        <asp:ImageButton AlternateText="Download file" ImageUrl="~/Images/download_icon_small.png" runat="server" ID="imgDownload" />




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
                            </div>
                        </div>
                    </div>
                    <%-- <div class="roleManager container" id="canceldiv" runat="server">
                <div class="row">
                    <p><span>-</span>  <a class="link" data-frm="frmNewPIDetails">Cancel Uploading Docs </a></p>
                </div>--%>
                </div>
                <div class="container UploadContainer" id="DocsContainer" runat="server">
                    <div class="row DocsUpload">
                        <%--<div class="col-md-12 col-sm-12">--%>
                        <a class="otherDocs link" data-frm="OtherDocs"><span>+</span>  Upload Other Document</a>
                        <a class="ProjectDocs link" data-frm="ProjectDocs"><span>+</span>Upload Project related Document</a>
                        <%--</div>--%>
                    </div>

                </div>


                <%-- Project Related Doc--%>
                <div class="roleManager container" id="DocumentUpload" runat="server">


                    <div class="frmProjectDocs">
                        <%--   <div class="row">
                           
                        </div>--%>
                        <div class="row removeslide">
                            <div class="col-md-12 ProjectDocDetails">
                                <h3 class="frmHead" data-frm="frmDetails1">Project Details <span>( - )</span></h3>
                            </div>
                        </div>
                        <div class="frmDetails1">
                            <div class="row removeslide">
                                <div class="col-md-12 col-sm-12" style="padding-left: 0px;margin-left:0px;">
                                    <div class="col-md-6 col-sm-6" style="padding-left: 0px;margin-left:0px;">
                                        <asp:HiddenField ID="Hiddocmanlib" runat="server" />

                                        <p runat="server" id="P1">
                                            <label>Search Project<b>*</b></label>
                                       
                                            <asp:TextBox ID="txtProjectSearch" runat="server" CssClass="ctlinput" Style="width: 450px" placeholder="Search Project by Project Title,Project Id,IRB No" />

                                        </p>
                                    </div>
                                </div>

                                <div class="col-md-6 col-sm-6 projectDetails">
                                    <p>
                                        <label>Project Title </label>
                                        <asp:TextBox ID="TxtProjectTitle" CssClass="ctltext" TextMode="MultiLine" onpaste="return false;" Enabled="false" ReadOnly="true"
                                            runat="server"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label>Alias 1</label>
                                        <asp:TextBox ID="TxtAlias1" CssClass="ctlinput" runat="server" Enabled="false"></asp:TextBox>
                                    </p>

                                    <p>
                                        <label>Alias 2</label>
                                        <asp:TextBox ID="TxtAlias2" CssClass="ctlinput" runat="server" Enabled="false"></asp:TextBox>
                                    </p>




                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <p>
                                        <label>Project ID </label>
                                        <asp:TextBox ID="TxtProjectId" CssClass="ctlinput" runat="server" Enabled="false" ReadOnly="true"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label>Short Title </label>


                                        <asp:TextBox ID="TxtShortTitle" CssClass="ctlinput" runat="server" Enabled="false" TabIndex="1"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label>DSRB/IRB No</label>
                                        <asp:TextBox ID="TxtDSRVIRB" CssClass="ctlinput" runat="server" Enabled="false" ReadOnly="true"></asp:TextBox>
                                    </p>


                                </div>
                            </div>
                        </div>
                        <div class="row removeslide">
                            <div class="col-md-12 ProjectDocDetails">
                                <h3 class="frmHead" data-frm="frmDetails2">Documents Section <span>( - )</span></h3>
                            </div>
                        </div>
                        <div class="frmDetails2">
                            <div class="row removeslide">



                                <div class="tblResposiveWrapper">
                                    <table id="tblProjectDocDetail" class="tblResposive removeHover">
                                        <thead>
                                            <tr>

                                                <th style="text-align: left">Document Title&nbsp;<b class="requireddoc">*</b></th>
                                                <th style="text-align: left">Document Description&nbsp;<b class="requireddoc">*</b></th>
                                                <th style="text-align: left">Document Category&nbsp;<b class="requireddoc">*</b></th>
                                                <th style="text-align: left">Browse File&nbsp;<b class="requireddoc">*</b></th>
                                                <th style="text-align: center">Action</th>
                                            </tr>
                                        </thead>

                                        <tbody>

                                            <asp:Repeater ID="rptrProjectDocDetails" runat="server" OnItemDataBound="rptrProjectDocDetails_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr class="removeHover">
                                                        <td data-th="Document Title">
                                                            <p>
                                                                <asp:TextBox ID="txtDocTitle" runat="server" CssClass="ctltext doctitle" TextMode="MultiLine" Text="" MaxLength="100"></asp:TextBox>
                                                            </p>
                                                        </td>
                                                        <td data-th="Document Description">
                                                            <asp:TextBox ID="txtDocDescription" runat="server" CssClass="ctltext docdesc" Text="" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                                        </td>
                                                        <td data-th="Document Category">
                                                            <asp:DropDownList ID="ddlDocCategory" runat="server" AutoPostBack="false" CssClass="ctlselect"></asp:DropDownList>
                                                        </td>
                                                        <td data-th="Browse File">

                                                            <input type="text" class="txtUpload" id="txtIRBFile" readonly="true" />

                                                            <span class="btn btn-default btn-file action uploadaction">Browse...
                                                                 
                                                   <asp:FileUpload ID="fuIRBFile" onchange="return FloadChange(this);" title="Document Upload" CssClass="action" runat="server" />
                                                                <asp:TextBox ID="txtSavePathn" runat="server" CssClass="txtSavePath" Style="visibility: hidden; width: 0px;"></asp:TextBox>
                                                            </span>
                                                        </td>

                                                        <td data-th="Action" style="text-align: center">
                                                            <p class="grid-action">
                                                                <a>
                                                                    <img title="delete" alt="Delete" onclick="return DeleteRow(this);" src="../images/icon-delete.png"></a>
                                                            </p>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>




                                        </tbody>
                                    </table>
                                </div>
                                <p id="Pmore" class="align-right"><a class="ProjectDocs1 link" onclick="addRow();">+ Add More Docs</a></p>
                            </div>
                        </div>
                    </div>
                    <%--  Project Common Div--%>

                    <div class="frmOtherDocs row">
                         <div class="row">
                            <div class="col-md-12">
                                <h3 class="frmHead" data-frm="frmDetails3">Documents Section <span>( - )</span></h3>
                            </div>
                        </div>
                        <div class="frmDetails3">
                        <div class="tblResposiveWrapper">
                            <table id="tableOtherDoc" class="tblResposive removeHover">
                                <thead>
                                    <tr>

                                        <th style="text-align: left;">Document Title&nbsp;<b class="requireddoc">*</b></th>
                                        <th style="text-align: left;">Document Description&nbsp;<b class="requireddoc">*</b></th>
                                        <th style="text-align: left;">Browse File&nbsp;<b class="requireddoc">*</b></th>
                                        <th style="text-align: center;">Action</th>
                                    </tr>
                                </thead>

                                <tbody>

                                    <asp:Repeater ID="rptrOtherDetailsDetails" runat="server">
                                        <ItemTemplate>
                                            <tr class="removeHover">
                                                <td data-th="Document Title">
                                                    <p>
                                                        <asp:TextBox ID="txtDocTitle" runat="server" CssClass="ctltext doctitle docOthertitle" Text="" TextMode="MultiLine"></asp:TextBox>
                                                    </p>
                                                </td>
                                                <td data-th="Document Description">
                                                    <asp:TextBox ID="txtDocDescription" runat="server" CssClass="ctltext docdesc docOtherdesc" TextMode="MultiLine" Text=""></asp:TextBox>
                                                </td>

                                                <td data-th="Browse File">

                                                    <input type="text" class="txtUpload txtOtherUpload" id="txtIRBFile" readonly="true" />
                                                    <asp:TextBox ID="txtSavePathn" runat="server" CssClass="txtSavePath" Style="visibility: hidden; width: 0px;"></asp:TextBox>
                                                    <span class="btn btn-default btn-file action uploadaction">Browse...
                                                     
                                                    <asp:FileUpload ID="fuIRBFile" onchange="return FloadChange(this);" title="Document Upload" CssClass="action" runat="server" />

                                                    </span>
                                                </td>


                                                <td data-th="Action" style="text-align: center;">
                                                    <p class="grid-action">
                                                        <a>
                                                            <img alt="Delete" onclick="return DeleteRow(this);" src="../images/icon-delete.png" title="delete"></img>

                                                        </a>
                                                    </p>
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>

                        <p runat="server" id="P2" class="align-right"><a class="OtherDocs link" onclick="addRow();">+ Add More Docs</a></p>
                    </div>
                        </div>
                    <%-- </div>--%>

                    <asp:HiddenField ID="HdnPath" runat="server" />
                    <div class="row margin-top frmAction">
                        <div class="col-md-12" id="actionbuttons" runat="server">
                            <p style="text-align: right; margin-right: -10px;">
                                <asp:Button CssClass="action" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return ValidateSave();" />
                                <asp:Button CssClass="action" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />


                            </p>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="HdnGlobPath" runat="server" Value="" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtSearchDoc" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="rptrDocument" EventName="ItemCommand" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="rptrDocument" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function CLearSearch() {
            $('#<%=txtDoucumentSearch.ClientID%>').bind("mouseup", function (e) {
                var $input = $(this),
     oldValue = $input.val();

                if (oldValue == "") return;


                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue == "") {
                         $('#<%=btnClear.ClientID%>').click();
                        //$input.trigger("cleared");
                    }
                }, 1);
            });
            $('#<%=txtProjectSearch.ClientID%>').bind("mouseup", function (e) {
                var $input = $(this),
     oldValue = $input.val();

                if (oldValue == "") return;


                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue == "") {
                        $(divName).find("input:text,textarea").val('');
                        $(tablename).find('tbody').find('input:text,textarea').val('');
                        $(tablename).find('tbody').find('input[type="text"]').val('');
                        //$input.trigger("cleared");
                    }
                }, 1);
              });
            return false;
        }
    </script>

    <script type="text/javascript" src="Scripts/Frm_DMS.js"></script>


    <script type="text/javascript">




        $(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(CallAutocomplete);
        });
        //$(document).ready(function () {
        //    CallAutocomplete();
        //});
        function CallAutocomplete() {
            ApplyPaging('tblResposive', 'Paging', 10);
            $("#tblResposive").addClass("removeHover");
            CLearSearch(); 
            $('.otherDocs').unbind('click').click(function () {
                ShowUploadSection(2);
            });
            $('.ProjectDocs').unbind('click').click(function () {
                ShowUploadSection(1);
            });
            SearchText('<%=txtProjectSearch.ClientID%>', '<%=HdnNeProjectId.ClientID%>', 10, "Project~spAutoComplete", Setvalues, '<%=Hidden.ClientID%>');
            var pagingInterval = setInterval(function () {

                if ($(".header").length > 0) {
                    ReApplyPaging('tblResposive');
                    clearInterval(pagingInterval);
                }
            }, 100);

            $('#' + '<%=txtDoucumentSearch.ClientID%>').keyup(function (e) {
                searchKeyPress(e);
            });

        }



        function FloadChange(obj) {

            if (obj.value != "") {
                var uploadfiles = $(obj).get(0);

                var uploadedfiles = uploadfiles.files;

                var fromdata = new FormData();

                for (var i = 0; i < uploadedfiles.length; i++) {

                    fromdata.append(uploadedfiles[i].name, uploadedfiles[i]);

                }


                var choice = {};

                choice.url = "DMSUpload.ashx";

                choice.type = "POST";

                choice.data = fromdata;

                choice.contentType = false;

                choice.processData = false;

                choice.success = function (result) {
                    if (result != null) {
                        //JSON.stringify(eval("(" + result + ")")).split('|')[1].replace('"', '');
                        $('[id*=HdnGlobPath]').val('');
                        $('[id*=HdnGlobPath]').val(result);
                        $(obj).parent().parent().find('[id*=txtIRBFile]').val(result.split('|')[1]);
                        $(obj).parent().parent().find('[id*=txtSavePath]').val(result.split('|')[0]);
                        return false;
                    }
                };

                choice.error = function (err) { alert(err.statusText); };

                $.ajax(choice);

            }
            return true;


        }

    </script>


</asp:Content>

