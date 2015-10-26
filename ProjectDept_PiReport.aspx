<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/TTSHMasterPage/TTSH.Master"  CodeBehind="ProjectDept_PiReport.aspx.cs" Inherits="TTSHWeb.ProjectDept_PiReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            ResizeReport();

            function ResizeReport() {
                try{
                    var viewer = document.getElementById("<%= MainReportViewer.ClientID %>");
                    var htmlheight = document.documentElement.clientHeight;
                    viewer.style.height = (htmlheight) + "px";
                }
                catch(exception){}
         }

         window.onresize = function resize() { ResizeReport(); }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="56000"></asp:ScriptManager>--%>
        <asp:Panel ID="Panel1" runat="server">
          <rsweb:ReportViewer ID="MainReportViewer" runat="server" ProcessingMode="Remote" Width="100%" Height="700px" KeepSessionAlive="false" AsyncRendering="false">
          </rsweb:ReportViewer> 

            <iframe id="frmPrint" name="frmPrint" runat="server" style = "display:none"></iframe>
            <div id="spinner" class="spinner" style="display:none;">
            <table style="height:100%;width:100%">
            <tr>
            <td><img id="img-spinner" src="../Images/ajax-loader.gif" alt="Printing"/></td>
            <td><span style="font-family:Verdana; font-weight:bold;font-size:10pt;width:86px;">Printing...</span></td>
            </tr>
            </table>
            </div>


        </asp:Panel>
</asp:Content>




