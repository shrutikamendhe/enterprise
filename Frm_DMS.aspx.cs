using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;

namespace TTSHWeb
{

    public partial class Frm_DMS : System.Web.UI.Page
    {
        static TTSHWCFServiceClient client;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!Page.IsPostBack)
                {
                    BindEmptyRepetear();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [WebMethod()]
        [ScriptMethod()]
        public static TTSHWCFReference.DocumentManagementSystem GetProjectDetails(int projectid)
        {
            string Result = "";
            client = new TTSHWCFServiceClient();
            try
            {

                TTSHWCFReference.DocumentManagementSystem docManSys = new DocumentManagementSystem();
                docManSys = client.GetDocumentWithProject(projectid);
                return docManSys;

            }
            catch (Exception)
            {

                return null;
            }

        }

        #region "Button Click Events"

        protected void txtSearchDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtDoucumentSearch.Text))
                {
                    List<DocumentManagementSystemFile> docList = new List<DocumentManagementSystemFile>();

                    client = new TTSHWCFServiceClient();

                    docList = client.GetDocuments(txtDoucumentSearch.Text).ToList();
                    docList.ForEach(delegate(DocumentManagementSystemFile docmansys)
                    {
                        if (docmansys.DocTitle.Contains("~"))
                        {
                            docmansys.DocTitle = docmansys.DocTitle.Remove(docmansys.DocTitle.LastIndexOf("~"), docmansys.DocTitle.LastIndexOf(".") - docmansys.DocTitle.LastIndexOf("~"));
                        }
                    }
                    );

                    rptrDocument.DataSource = docList;
                    rptrDocument.DataBind();
                    CallJS("canceldiv();ShowNoRecords();");
                }

            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }

        protected void rptrDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {

                if (e.CommandName != "")
                {

                    //if (e.CommandName.ToLower() == "DownLaod".ToLower())
                    //{
                    //    Common.DownloadFile(e.CommandArgument.ToString(), Response);
                    //}
                    //if (e.CommandName.ToLower() == "Delete".ToLower())
                    //{
                    //}
                }

            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IList<DMS_DocumentManagementSystem> docmansys = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DMS_DocumentManagementSystem>>(Hiddocmanlib.Value.ToString());
                client = new TTSHWCFServiceClient();

                bool b = client.SaveDocumentData(docmansys.ToArray());
                ClearAll(true);

            }
            catch (Exception ex)
            {
                //throw ex;
                this.MsgBox(ex.Message);
            }
        }

        ///
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll(false);

            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll(false);
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }

        protected void rptrProjectDocDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item ||
                     e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var dms = e.Item.DataItem as TTSHWeb.TTSHWCFReference.DocumentManagementSystem;

                    var control = e.Item.FindControl("ddlDocCategory") as DropDownList;
                    control.FillCombo(DropDownName.DocumentCategory);


                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void rptrDocument_ItemDataBound1(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ImageButton imgDownload = (ImageButton)e.Item.FindControl("imgDownload");
                    var s_Uploaded_File = ((TTSHWeb.TTSHWCFReference.DocumentManagementSystemFile)(e.Item.DataItem)).FileName;
                    imgDownload.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + s_Uploaded_File + "')";

                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }

        }
        
        #endregion

        #region "User Defined Function"
        public void ClearAll(bool isMessage)
        {
            string script = string.Empty;
            try
            {
                script = isMessage ? "ClearAll();MessageBox('Documents Uploaded Successfully.');" : string.Empty;
                rptrDocument.DataSource = null;
                rptrDocument.DataBind();
                txtDoucumentSearch.Text = "";
                HdnNeProjectId.Value = "";
                CallJS(string.Format("HideDiv();ShowNoRecords();{0}", script));
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// Used for Showing The Empty Repeater
        /// </summary>
        public void BindEmptyRepetear()
        {
            try
            {
                rptrOtherDetailsDetails.DataSource = Enumerable.Range(0, 1);
                rptrOtherDetailsDetails.DataBind();
                rptrProjectDocDetails.DataSource = Enumerable.Range(0, 1);
                rptrProjectDocDetails.DataBind();
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }
        public void CallJS(string script, string Name = "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Name, script, true);
        } 
        #endregion

     

        
        

    }
}