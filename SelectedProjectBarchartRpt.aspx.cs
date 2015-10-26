using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using TTSHWeb.TTSHWCFReference;


namespace TTSHWeb
{
    public partial class SelectedProjectBarchartRpt : System.Web.UI.Page
    {

        #region " Page Event "
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
                string result = string.Empty;
                List<RptSelectedProject> rptobj = new List<RptSelectedProject>();



                rptobj = cl.GetProjectDetails(TxtStartDate.Text, TxtEndDate.Text).ToList();

                RptProjectGrid.DataSource = rptobj;
                RptProjectGrid.DataBind();
            }
        }
        #endregion



        #region " Reapeter Event "
        protected void RptProjectGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                //  int projectID = Convert.ToInt32(e.CommandArgument);
                //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + Server.MapPath("SelectedChartRpt.aspx") + "?ProjectID=" + e.CommandArgument.ToString() + "');", true);

                //   ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('"+ Server.MapPath("SelectedChartRpt.aspx")+"?ProjectID="+ e.CommandArgument.ToString()+ ",null,height=251px, width=600px,status=no, resizable=no, scrollbars=no, toolbar=no,location=no,menubar=no ');" , true);

                //Session["RProjectID"] = e.CommandArgument.ToString();

                //string Path = Server.MapPath("SelectedChartRpt.aspx?Startdate=" + TxtStartDate.Text + "&Enddate="+TxtStartDate.Text);

                //Response.Redirect(Path,false);

                HiddenField1.Value = e.CommandArgument.ToString();

                GetYear(Convert.ToInt32(e.CommandArgument.ToString()));
                ShowCode();

                ddlFromMonth.SelectedValue = "1";
                ddlToMonth.SelectedValue = "12";


                DivMain.Visible = false;
                ProjectDetailContainer.Visible = true;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
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

                ReportViewerSSRSDemo.ServerReport.ReportPath = "/TTSHREPORT/SelectedBarchartRpt2";

                List<ReportParameter> parameters = new List<ReportParameter>();

                parameters.Add(new ReportParameter("ProjectID", HiddenField1.Value));

                parameters.Add(new ReportParameter("Year", ddlYear.SelectedItem.Text));

                parameters.Add(new ReportParameter("FromMonth", ddlFromMonth.SelectedItem.Value));

                parameters.Add(new ReportParameter("ToMonth", ddlToMonth.SelectedItem.Value));

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
            try
            {
                DivMain.Visible = true;
                ProjectDetailContainer.Visible = false;
            }
            catch (Exception)
            {

                throw;
            }

            //  Response.Redirect("SelectedProjectReport.aspx?Startdate=" + HdfldStartdate.Value + "&Enddate=" + HdfldEnddate.Value);
        }

        protected void btnShow1_Click(object sender, EventArgs e)
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtStartDate.Text.Trim() != "" && TxtEndDate.Text.Trim() != "")
                { 
                    
              
                TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
                string result = string.Empty;
                List<RptSelectedProject> rptobj = new List<RptSelectedProject>();

                rptobj = cl.GetProjectDetails(TxtStartDate.Text, TxtEndDate.Text).ToList();

                RptProjectGrid.DataSource = rptobj;
                RptProjectGrid.DataBind();
              
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }

}