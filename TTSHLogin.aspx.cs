using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TTSHWeb.TTSHWCFService;


namespace FormsAuthAd
{
    public partial class TTSHLogin : System.Web.UI.Page
    {
        #region Declaration

        TTSHWCFServiceClient proxy = null;
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            proxy = new TTSHWCFServiceClient();

            string ADserver = System.Configuration.ConfigurationManager.AppSettings["ADserver"].ToString();
            string AdminGroup = System.Configuration.ConfigurationManager.AppSettings["AdminGroup"].ToString();
            string UserGroup = System.Configuration.ConfigurationManager.AppSettings["UserGroup"].ToString();

            //if (proxy.AuthenticateADUsers(ADserver, txtUserName.Text.Trim(), txtPassword.Text.Trim()))
            if (proxy.AuthenticateADUsersByName(txtUserName.Text.Trim()))
            {
                HttpContext.Current.Session["UserName"] = txtUserName.Text.Trim();
               
                proxy.Close();
                proxy = new TTSHWCFServiceClient();
              
                string[] Groups = proxy.GetGroupNames(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                string UserGp = Groups[1].ToString();
               
                switch (UserGp)
                {
                    case "Administrator" :
                         _UserManagement.GetMenusForRole(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MenuIdAdmin"]));
                         Response.Redirect("Home.aspx", false);
                    break;

                    case "ProjectFeasibility":
                    break;

                    case "ProjectEthics":
                    break;

                    case "ProjectSelected":
                    break;

                    case "RegulatoryUser":
                    break;

                    case "GrantUser":
                    break;

                    case "ContractUser":
                    break;

                    default:
                    break;
                }

                //if (Groups.Contains(AdminGroup))
                //{
                //    _UserManagement.GetMenusForRole(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MenuIdAdmin"]));
                //    Response.Redirect("Home.aspx", false);
                //}
                //else if (Groups.Contains(UserGroup))
                //{
                //    _UserManagement.GetMenusForRole(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MenuIdUser"]));
                //    Response.Redirect("Home.aspx", false);
                //}
                //else { FailureText.Text = string.Join("|", Groups); }
            }
            else
            {
                FailureText.Text = "Invalid UserName/Password.";
            }

            /**************************** Old Code ****************************/
            //String adPath = "LDAP://192.168.0.109"; //Fully-qualified Domain Name
            //LdapAuthentication adAuth = new LdapAuthentication(adPath);

            //try
            //{
            //    if (true == adAuth.IsAuthenticated(txtDomain.Text, txtUserName.Text, txtPassword.Text))
            //    {
            //        String groups = adAuth.GetGroups();
                    
            //        adAuth.GetUserPicture1();

            //        //Create the ticket, and add the groups.
            //        bool isCookiePersistent = chkPersist.Checked;
            //        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, txtUserName.Text,
            //      DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);

            //        //Encrypt the ticket.
            //        String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //        //Create a cookie, and then add the encrypted ticket to the cookie as data.
            //        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            //        if (true == isCookiePersistent)
            //            authCookie.Expires = authTicket.Expiration;

            //        //Add the cookie to the outgoing cookies collection.
            //        Response.Cookies.Add(authCookie);

            //        //You can redirect now.
            //        Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUserName.Text, false));
            //    }
            //    else
            //    {
            //        FailureText.Text = "Authentication did not succeed. Check user name and password.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    FailureText.Text = "Error authenticating. " + ex.Message;
            //}
            /****************************End Old Code ****************************/

        }
    }
}