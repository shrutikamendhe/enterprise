using System;
using System.Collections.Generic;
using TTSHWeb.TTSHWCFReference;
using Microsoft.Reporting.WebForms;

namespace TTSHWeb
{
    public partial class SelectedChartRpt : System.Web.UI.Page
    {
       int ProjectID;     
  
        protected void Page_Load(object sender, EventArgs e)
        {         
            if (!IsPostBack)
            {

                //HdfldStartdate.Value = Request.QueryString["Startdate"].ToString();
                //HdfldEnddate.Value = Request.QueryString["Enddate"].ToString();
               

                ProjectID = Convert.ToInt32(Session["RProjectID"].ToString());
                GetYear(ProjectID);
                ShowCode();
            }

        }

        public void GetYear(int ProjectID)
        {
            try
            {
                //TTSHWCFServiceClient serviceClientObj = new TTSHWCFServiceClient();
                ddlYear.FillCombo(DropDownName.GetYear, ProjectID.ToString());
                ddlYear.Items.RemoveAt(0);

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public sealed class ReportCredentials : IReportServerCredentials
        {
            private string _username;
            private string _password;
            private string _domain;

            public ReportCredentials(string username, string password, string domain)
            {
                _username = username;
                _password = password;
                _domain = domain;
            }
            public System.Net.ICredentials NetworkCredentials
            {
                get { return new System.Net.NetworkCredential(_username, _password, _domain); }
            }
            public bool GetFormsCredentials(out System.Net.Cookie authCookie,
            out string userName,
            out string password,
            out string authority)
            {
                authCookie = null;
                userName = null;
                password = null;
                authority = null;

                return false;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get { return null; }
            }
        }

        public void ShowCode()
        {
            try
            {
            
                ReportViewerSSRSDemo.ShowCredentialPrompts = false;

                ReportViewerSSRSDemo.ServerReport.ReportServerCredentials = new ReportCredentials("spfarm", "ROOT#123", "RSINNGP");

                ReportViewerSSRSDemo.ServerReport.ReportServerUrl = new Uri("http://srvpps01:7070/ReportServer");

                ReportViewerSSRSDemo.ServerReport.ReportPath = "/TTSHREPORT/SelectedProjBarChartRpt";

                List<ReportParameter> parameters = new List<ReportParameter>();

                parameters.Add(new ReportParameter("ProjectID", Session["RProjectID"].ToString()));

                parameters.Add(new ReportParameter("Year", ddlYear.SelectedItem.Text));

                ReportViewerSSRSDemo.ServerReport.SetParameters(parameters);

                ReportViewerSSRSDemo.ProcessingMode = ProcessingMode.Remote;

                ReportViewerSSRSDemo.ShowParameterPrompts = false;

                ReportViewerSSRSDemo.ShowPromptAreaButton = false;

                ReportViewerSSRSDemo.ServerReport.Refresh();

                  
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        #region " Button Event "
        protected void lnkback_Click(object sender, EventArgs e)
        {
       //     Response.Redirect("SelectedProjectReport.aspx?Startdate="+HdfldStartdate.Value+"&Enddate="+HdfldEnddate.Value);
        }
        #endregion

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ShowCode();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}