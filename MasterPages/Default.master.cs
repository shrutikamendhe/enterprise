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
            Response.Write(ex.Message);
        }
    }
}
