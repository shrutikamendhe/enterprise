using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace TTSHWeb
{
    public partial class Reports : System.Web.UI.Page
    {
        #region Classes

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

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    string path = "";
                    if (!string.IsNullOrEmpty(Request.QueryString["Report"]))
                    {
                        path = getReportPath(Convert.ToString(Request.QueryString["Report"]));
                        if (path.Trim() != string.Empty)
                        {
                            ReportViewerSSRS.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                            ReportViewerSSRS.ServerReport.ReportServerUrl = new Uri("http://srvpps01:7070/ReportServer");
                            ReportViewerSSRS.ServerReport.ReportServerCredentials = new ReportCredentials("spfarm", "ROOT#123", "RSINNGP");
                            ReportViewerSSRS.ServerReport.ReportPath = path;
                            ReportViewerSSRS.ServerReport.Refresh();
                        }
                        else
                        {
                            throw new Exception("No report source selected.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }
        #endregion

        #region Functions/Subroutines

        public string getReportPath(string queryStringValue)
        {
            string ReportPath;
            try
            {
                switch (queryStringValue.ToLower())
                {
                    case "auditreport":
                        ReportPath = "/AuditReports/AuditTrialNew";
                        break;

                    case "projectbydeptpi":
                        ReportPath = "/TTSHREPORT/ProjectBYDeptPI";
                        break;

                    case "contractactive":
                        ReportPath = "/TTSHREPORT/ContractActiveRpt";
                        break;

                    case "contractexecuted":
                        ReportPath = "/TTSHREPORT/ContractExecutedRpt";
                        break;

                    case "contractexpired":
                        ReportPath = "/TTSHREPORT/ContractExpiredd";
                        break;

                    case "contractinnegotiation":
                        ReportPath = "/TTSHREPORT/ContractInNegotiationRpt";
                        break;

                    case "contractrequested":
                        ReportPath = "/TTSHREPORT/ContractRequested";
                        break;

                    case "contractstatus":
                        ReportPath = "/TTSHREPORT/ContractStatus";
                        break;

                    case "contractturnaroundtime":
                        ReportPath = "/TTSHREPORT/ContractTurnaroundtime";
                        break;

                    case "contractwithdrawn":
                        ReportPath = "/TTSHREPORT/ContractWithdrawn";
                        break;

                    case "cupboard":
                        ReportPath = "/TTSHREPORT/CupboardReport";
                        break;

                    case "feasibilitystatus":
                        ReportPath = "/TTSHREPORT/FeasibilityStatusReport";
                        break;

                    case "projectlistofctregularorystatus":
                        ReportPath = "/TTSHREPORT/ProjectListofCTRegularoryStatus";
                        break;

                    case "coordinatorselectedproject":
                        ReportPath = "/TTSHREPORT/rptCoordinatorSelectedProject";
                        break;

                    case "crocradetails":
                        ReportPath = "/TTSHREPORT/rptCroCraDetails";
                        break;

                    case "saestatus":
                        ReportPath = "/TTSHREPORT/rptSAEStatus";
                        break;

                    //case "selectedprojbarchart":
                    //    ReportPath = "/TTSHREPORT/SelectedProjBarChartRpt";
                    //    break;

                    case "selectedprojnopatients":
                        ReportPath = "/TTSHREPORT/rptSelectedPNoPatients";
                        break;
                    
                    case "projectlistofctregulatorystatus":
                        ReportPath = "/TTSHREPORT/ProjectListofCTRegulatoryStatus";
                        break;

                    case "druglocation":
                        ReportPath = "/TTSHREPORT/DrugLocationReport";
                        break;
                   
                    case "rptsponsorpi":
                        ReportPath = "/TTSHREPORT/rptSponsorPI";
                        break;

                    default:
                        ReportPath = "/AuditReports/AuditTrialNew";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReportPath;
        }

        #endregion
    }
}