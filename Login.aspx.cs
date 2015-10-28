using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserName.Focus();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string ADserver = System.Configuration.ConfigurationManager.AppSettings["ADserver"].ToString();
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool auth = TTSH.BusinessLogic.Authentication.LdapAuthentication.Authenticate(ADserver, userName, password);
            if (auth)
            {
                string[] Groups = TTSH.BusinessLogic.Authentication.LdapAuthentication.GetGroupNames(userName, password);
                string UserGp = String.Join(",", Groups.ToArray());
                UserGp = "'" + UserGp.Replace(",", "','") + "'";

                Response.Write(UserGp);
            }
            else
            {
                FailureText.Text = "Invalid Login Name/Password.";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            FailureText.Text = ex.Message;
        }
    }
}