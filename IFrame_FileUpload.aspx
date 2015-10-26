<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IFrame_FileUpload.aspx.cs" Inherits="TTSHWeb.IFrame_FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" >
    <link rel="apple-touch-icon" href="apple-touch-icon.png" />
    <!-- Place favicon.ico in the root directory -->
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/normalize.css" />
    <link rel="stylesheet" href="../css/base.css" />
    <link rel="stylesheet" href="../css/custom.css" />
    <link href="../css/tableSorter.css" rel="stylesheet" />
    <link href="../App_Themes/default.css" rel="stylesheet" />
    <link href="../css/ModelPopUp.css" rel="stylesheet" />
    <link href="../css/jQ-UI-Dialog.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.11.3.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=11" />
    <style type="text/css">
        .ctlfilename
        {
            width: 185px!important;
            height: 30px!important;
            font-size: 13px!important;
        }
        .ctlinvisible {
            width:0px;
            visibility:hidden;
        }
        .rowset {
            height:35px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="rowset" >
            <asp:TextBox ID="txtFileName" runat="server" CssClass="ctlfilename" />
            <input type="button" id="btnBrowse" class="action" value="Browse..." />
            <asp:TextBox ID="txtSavePath" runat="server" CssClass="txtSavePath" Style="visibility: hidden;" />
            <asp:FileUpload ID="file1" runat="server"  CssClass="ctlinvisible"/>
            <asp:Button runat="server" ID="btnupload" CssClass="ctlinvisible"  Text="Upload" OnClick="btnupload_Click" />
        </div>
    </form>
</body>
<script type="text/javascript">
    $(document).ready(function () {
        $('#btnBrowse').click(function () {
            $('#file1').click();
            return false;
        });
        $('#file1').change(function () {
            $('#btnupload').click();
        });
    });
</script>
</html>
