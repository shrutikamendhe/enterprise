using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
                    CancelUnexpectedRePost();
                }
            }

            if (!IsPostBack)
            {

                string sPath = string.Empty;//System.Web.HttpContext.Current.Request.Url.PathAndQuery.ToString();
                if (Request.Url.AbsoluteUri.Contains("Reports"))
                    sPath = System.Web.HttpContext.Current.Request.Url.PathAndQuery.ToString();
                else
                    sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToString();
                if (HttpContext.Current.Session["MenuDT"] != null)
                {
                    PopulateMenu();

                    //if (!DetectAccessRights(ref sPath))
                    //{
                    //    Response.Redirect("Login.aspx");
                    //}
                }
                else
                {
                    //Response.Redirect("Login.aspx");
                }

                hdnUserName.Value = HttpContext.Current.Session["UserName"].ToString();

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

    private void CancelUnexpectedRePost()
    {
        string clientCode = _repostcheckcode.Value;

        //Get Server Code from session (Or Empty if null)
        string serverCode = Session["_repostcheckcode"] as string ?? "";

        if (!IsPostBack || clientCode.Equals(serverCode))
        {
            //Codes are equals - The action was initiated by the user
            //Save new code (Can use simple counter instead Guid)
            string code = Guid.NewGuid().ToString();
            _repostcheckcode.Value = code;
            Session["_repostcheckcode"] = code;
        }
        else
        {
            //Unexpected action - caused by F5 (Refresh) button
            Response.Redirect(Request.Url.ToString());
        }
    }

    // Sapna K: Method to get allowed menu names in session
    private void PopulateMenu()
    {

        try
        {

            // Sapna K: Call Method to get Parent and Child menu names
            DataSet ds = GetDataSetForMenu();
            // Sapna K: Create new arraylist object to store menu item
            System.Collections.ArrayList arrMenus = new System.Collections.ArrayList();
            UserMenu.Items.Clear();
            // Sapna K: Add 'Home' as default menu item
            //MenuItem HomeItem = new MenuItem("Dashboard", "", "", "~/Dashboard.aspx", "_parent");
            //Menu1.Items.Add(HomeItem);
            //arrMenus.Add("Dashboard.aspx");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow parentItem in ds.Tables[0].Rows)
                    {
                        // Sapna K: Add Parent Menu Item in allowed menu item list
                        MenuItem categoryItem = new MenuItem((string)parentItem["MenuName"]);

                        UserMenu.Items.Add(categoryItem);
                        arrMenus.Add((string)parentItem["MenuName"]);

                        foreach (DataRow childItem in parentItem.GetChildRows("Children"))
                        {
                            // Sapna K: Add Child Menu Item in allowed menu item list
                            MenuItem childrenItem = new MenuItem((string)childItem["MenuName"], "", "", "~/" + Convert.ToString(childItem["Url"]), "_parent");
                            categoryItem.ChildItems.Add(childrenItem);
                            arrMenus.Add(Convert.ToString(childItem["Url"]));

                        }
                    }
                }
            }
            HttpContext.Current.Session["AllowedMenus"] = arrMenus;

        }
        catch (Exception Ex) { }
    }

    // Sapna K: Method to get Parent and Child menu names in Data Table and establish a relationship between Parent & Child table
    private DataSet GetDataSetForMenu()
    {
        DataSet ds = new DataSet();
        try
        {
            if (((DataTable)HttpContext.Current.Session["MenuDT"]).Select("[Parent] = '0'").Count() > 0 && ((DataTable)HttpContext.Current.Session["MenuDT"]).Select("[Parent] <> '0'").Count() > 0)
            {
                DataTable DTParent = ((DataTable)HttpContext.Current.Session["MenuDT"]).Select("[Parent] = '0'").CopyToDataTable();
                DataTable DTChild = ((DataTable)HttpContext.Current.Session["MenuDT"]).Select("[Parent] <> '0'").CopyToDataTable();
                ds.Tables.Add(DTParent);
                ds.Tables.Add(DTChild);
                DataSet dd = new DataSet();
                dd = ds;
                ds.Relations.Add("Children", ds.Tables["Table1"].Columns["Child"], ds.Tables["Table2"].Columns["Parent"]);

            }
        }
        catch (Exception Ex) { }
        return ds;
    }


    /// <summary>
    /// Detect User Has Access Rights Or Not
    /// </summary>
    /// <param name="spath">Current Page Url</param>
    /// <returns>True/False</returns>
    public bool DetectAccessRights(ref string spath)
    {
        try
        {
            ArrayList arrMenus = (ArrayList)Session["AllowedMenus"];

            return arrMenus.Contains(spath.Replace("/", ""));
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
