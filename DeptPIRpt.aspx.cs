using System;
using Microsoft.Reporting.WebForms;

namespace TTSHWeb
{
    public partial class DeptPIRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                MainReportViewer.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials("spfarm", "ROOT#123", "RSINNGP");
                MainReportViewer.ServerReport.ReportServerCredentials = irsc;
                MainReportViewer.ServerReport.ReportServerUrl = new Uri("http://192.168.0.16/ReportServer");
                MainReportViewer.ServerReport.ReportPath = "/TTSHReport/DepartmentFilterRpt";
                MainReportViewer.ServerReport.Refresh();

            }

        }

    }

   
}