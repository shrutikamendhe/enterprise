using System;
using System.Net;
using System.Security.Principal;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Reporting.WebForms;
using System.ComponentModel;

namespace TTSHWeb
{
    [Serializable]
    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        //ReportViewer.KeepSessionAlive Property
        [WebBrowsableAttribute(true)]
        [DefaultValueAttribute(true)]
        public bool KeepSessionAlive
        { get; set; }

        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }

        
    }
    
    [Serializable]
    public partial class ProjectDept_PiReport : System.Web.UI.Page
    {
      //  [Serializable()]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                MainReportViewer.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials("spfarm", "ROOT#123", "RSINNGP");
                MainReportViewer.ServerReport.ReportServerCredentials = irsc;
                MainReportViewer.ServerReport.ReportServerUrl = new Uri("http://192.168.0.16/ReportServer");
                MainReportViewer.ServerReport.ReportPath = "/TTSHReport/DepartmentFilterRpt";
                Response.CacheControl = "no-cache";
                Response.Expires = -1;
                MainReportViewer.ServerReport.Refresh();
               
            }
          
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);

        //    #region check for lost session
        //    if (Context.Session != null)
        //    {
        //        if (Session.IsNewSession)
        //        {
        //            string cookieHeader = Request.Headers["Cookie"];
        //            if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
        //            {
        //                Response.Redirect(Request.Url.ToString());
        //            }
        //        }
        //    }

        //    #endregion check for lost session

        //    #region generate keepsessionalive script


        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("$(function () {setInterval(KeepSessionAlive, " + GetSessionTimeoutInMs() + ");");
        //    sb.Append("});");
        //    sb.Append("function KeepSessionAlive() {");
        //    sb.Append(string.Format("$.post('{0}', null);", ResolveUrl("~/KeepSessionAlive.ashx")));
        //    sb.Append("};");

        //    // register on page
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SessionKeepAlive", sb.ToString(), true);

        //    #endregion generate keepsessionalive script
        //}

        //private int GetSessionTimeoutInMs()
        //{
        //    return (this.Session.Timeout * 60000) - 10000;
        //}

    }
    /// 
    /// Summary description for CustomReportCredentials
    /// 

   
}