<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptPIRpt.aspx.cs" Inherits="TTSHWeb.DeptPIRpt" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      
     <script type="text/javascript">
      ResizeReport();

           function ResizeReport() {
               var viewer = document.getElementById("<%= MainReportViewer.ClientID %>");
        var htmlheight = document.documentElement.clientHeight;
        viewer.style.height = (htmlheight) + "px";
       
    }

    window.onresize = function resize() { ResizeReport(); }
</script>

</head>
<body>
     
    <form id="form1" runat="server">


      <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="56000"></asp:ScriptManager>
        <asp:Panel ID="Panel1" runat="server">
          <rsweb:ReportViewer ID="MainReportViewer" runat="server" ProcessingMode="Remote" Width="100%" Height="700px">
          </rsweb:ReportViewer> 
        </asp:Panel>
        </form>
           
</body>
</html>
