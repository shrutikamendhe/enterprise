using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb;
using TTSHWeb.TTSHWCFReference;
using System.Data;
using System.ComponentModel;
using System.Collections;

namespace TTSHWeb.TTSHMasterPage
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
          
		  if (!IsPostBack )
		  {
              try
              {
                  string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                  if (HttpContext.Current.Session["MenuDT"] != null)
                  {
                      PopulateMenu();
                  }
                  else
                  {
                      Response.Redirect("Login.aspx");
                  }
              }
              catch (Exception Ex)
              {
                  //WriteLog(Ex.Message + Environment.NewLine + Ex.Source + Environment.NewLine + Ex.StackTrace);
              }
		  }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        #region Methods
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

        /*****************************************/
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            object[] values = new object[props.Count];
            using (DataTable table = new DataTable())
            {
                long pCt = props.Count;
                for (int i = 0; i < pCt; ++i)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                foreach (T item in data)
                {
                    long vCt = values.Length;
                    for (int i = 0; i < vCt; ++i)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
        }
        #endregion

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }
       
    }
}