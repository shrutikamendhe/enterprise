using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPages_Default : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //------Added by Atul
            if (!Request.Url.AbsoluteUri.Contains("Reports"))
            {
                if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
                {
                }
                else
                {
                    //CancelUnexpectedRePost();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<h1> Error: </h1><p>" + ex.Message + "</p><br/><h1>" + ex.StackTrace + "</h1>");
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        if (Session != null)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
        }

        Response.Redirect("Login.aspx");
    }
}
