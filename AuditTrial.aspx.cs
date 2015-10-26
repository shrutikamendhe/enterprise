using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;


namespace TTSHWeb
{
    public partial class AuditTrial : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //To popuplate Modules audit Dropdown
                   PopulateDropDown();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ddlAuditModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               FillListBox((System.Web.UI.WebControls.ListBox)listSelectFrom, DropDownName.AuditModulesFields, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAuditShow_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid(Convert.ToDateTime(txtAuditFromDate.Text),Convert.ToDateTime(txtAuditToDate.Text));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Methods

        public void FillListBox(System.Web.UI.WebControls.ListBox lb, TTSHWCFReference.DropDownName listname, string Condition = "")
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                List<TTSHWCFReference.clsDropDown> listbox = cl.GetDropDownData(listname, ddlAuditModules.SelectedValue, "", "", "", "").ToList();
                lb.DataSource = listbox;
                lb.DataTextField = "DisplayField";
                lb.DataValueField = "ValueField";
                lb.DataBind();
                //lb.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            catch
            { }
        }
        public void PopulateDropDown()
        {
            try
            {
                Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlAuditModules, TTSHWCFReference.DropDownName.AuditModules, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FillGrid(DateTime FromDateRange ,DateTime ToDateRange)
        {
            try
            {
                TTSHWCFServiceClient Client = new TTSHWCFServiceClient();
                List<Audit> gridData = new List<Audit>();
                //List<ServiceReference1.Audit> = new List<ServiceReference1.Audit>();
                gridData = Client.FillGrid_Audit(FromDateRange, ToDateRange).ToList();
                rptrAuditTrial.DataSource = gridData;
                rptrAuditTrial.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        #endregion
    }

}