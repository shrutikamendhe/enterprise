using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;
namespace TTSHWeb
{
    public partial class frmProject_Master : System.Web.UI.Page
    {
        #region " Page Event "
        protected void Page_Load(object sender, EventArgs e)
        {

            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.ALLPROJECTS;
            SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
            SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;
            if (!IsPostBack)
            {
                ClearHDN();
                ddlChildParent.Attributes.Add("title", "");
                TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                t.Text = "";
                if (Convert.ToString(Common.iffBlank(Request.QueryString["Newpage"], "")) != "")
                {
                    ShowPanel("entry");
                }
                else
                {
                    FillGridMain();

                    ShowPanel();

                }

                CallJavascript();
                BindCombo();
                BindCoOrdinator();

                BindDataOwners();
                AddNewBtnDisplay();
            }


        }

        private void AddNewBtnDisplay()
        {
            try
            {
                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                string UserID = Convert.ToString(Session["UserID"]).ToUpper();

                DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");
                DataOwner_Entity[] oDataOwnerMonitor = client.GetAllDataOwner("TProjectMonitoring");

                var AdminArray = (from s in oDataOwner
                                  select s.GUID).ToList();
                bool IsAdmin = AdminArray.Contains(UserID);


                bool IsMonitor = false;
                if (IsAdmin == false)
                {
                    var MonitorArray = (from s in oDataOwnerMonitor
                                        select s.GUID).ToList();
                    IsMonitor = MonitorArray.Contains(UserID);
                }

                if (IsAdmin == true || IsMonitor == true)
                {
                    btnNew.Attributes.Add("style", "display:block");
                }
                else
                {
                    btnNew.Attributes.Add("style", "display:none");
                }

            }
            catch (Exception ex)
            {

            }
        }

        void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            FillGridMain();
        }

        void SearchBox_ButtonSearchClick(object sender, EventArgs e)
        {
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();


            if (string.IsNullOrEmpty(SearchBox.ErrorString))
            {
                Search[] lst = SearchBox.SearchOutput;

                try
                {
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    DataOwner_Entity[] oDataOwner = cl.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);

                    if (IsAdmin == false)
                    {
                        lst.Where(z => z.Created_By.ToUpper() != UserID).ToList().ForEach(i => i.Status = "View");
                        lst.Where(z => z.Created_By.ToUpper() == UserID).ToList().ForEach(i => i.Status = "Edit");
                        lst.OrderByDescending(z => z.i_ID);
                    }
                    else
                    {
                        lst.ToList().ForEach(i => i.Status = "Edit");
                        lst.OrderByDescending(z => z.i_ID);
                    }
                }
                catch (Exception ex1)
                {

                }

                rptrProjectDetail.DataSource = lst;
                rptrProjectDetail.DataBind();
            }
            else
            {
                //this.MsgBox(SearchBox.ErrorString);
                rptrProjectDetail.DataSource = null;
                rptrProjectDetail.DataBind();
            }
        }
        #endregion

        #region " Page Method "
        public void BindCoOrdinator()
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("value"); dt.Columns.Add("text");
            //dt.Rows.Add("1", "Chua Phung Kim");
            //dt.Rows.Add("2", "Goh Chok Swee");
            //dt.Rows.Add("3", "Rachel Nicholas");
            //dt.Rows.Add("4", "Elaine Shane");
            //dt.Rows.Add("5", "Cherilyn shalihin");
            //dt.Rows.Add("6", "Gemmie Raymond");
            //dt.Rows.Add("7", "Khaw Boon wan");
            //chkboxlist.FillCheckList(dt, "text", "value", true);
            chkboxlist.FillCheckList(DropDownName.Coordinators);

        }

        #endregion

        #region " Reapeter Event "
        protected void rptrProjectDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                HdnId.Value = e.CommandArgument.ToString();
                if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview")
                {
                    HdnMode.Value = e.CommandName.ToString().ConverMode();
                    FillControl();
                }
            }
        }

        #endregion

        #region " Button Event "
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();

        }
        protected void btnPISave_Click(object sender, EventArgs e)
        {
            SavePI();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {

            ClearHDN();
            HdnMode.Value = "Insert";
            ShowPanel("entry");
            TxtstartDate.Text = Common.SetCurrentDate();
            BindCombo();
            ddlParentProjName.Enabled = false;
            ddlChildParent.Attributes.Add("title", "");
            txtParentProjId.Enabled = false; ddlProjSubType.Enabled = false;

            if (ddlProjSubType.Enabled == false)
            {
                ddlProjSubType.Items.Insert(0, new ListItem("--Select--", System.Convert.ToString(0)));
            }
            BindCoOrdinator();
            ChangeButtonText();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FillGridMain();
            ShowPanel();
            HdnMode.Value = "Insert";
            ChangeButtonText();
        }
        protected void ddlProjType_SelectedIndexChanged(object sender, EventArgs e)
        {

            //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "Auto", "CallAutocomplete();", true);
            //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "DataOwner", "ApplyDOEvents();", true);
            this.CallJs("EnableParentControls('" + ddlChildParent.ClientID + "', '" + txtParentProjId.ClientID + "', '" + ddlParentProjName.ClientID + "')");

            if (ddlProjCategory.SelectedItem.Text.ToLower() == "pharma")
            {
                ddlCollbrationInv.Enabled = false;
            }
            else
            {
                ddlCollbrationInv.Enabled = true;
            }
            if (ddlProjType.SelectedIndex > 0)
            {
                ddlProjSubType.Enabled = true;
                ddlProjSubType.FillCombo(DropDownName.FillSubType, ddlProjType.SelectedValue);

            }
            else
            {
                ddlProjSubType.Enabled = false;
                ddlProjSubType.Items.Clear(); ddlProjSubType.Items.Add(new ListItem("--Select--", System.Convert.ToString(0)));
            }

        }
        protected void ddlParentProjName_SelectedIndexChanged(object sender, EventArgs e)
        {

            TTSHWCFServiceClient cls = new TTSHWCFServiceClient();
            if (ddlParentProjName.SelectedIndex > 0)
            {
                string values = cls.GetValidate("GetDisplayId", ddlParentProjName.SelectedValue, "", "", ""); txtParentProjId.Text = "";
                txtParentProjId.Text = values;
                ddlParentProjName.Enabled = true; txtParentProjId.Enabled = false;
            }
            else
            {
                txtParentProjId.Text = "";
            }
            this.CallJs("TrimParentProject();");

        }
        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("Delete_Project", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Project Details Deleted Successfully..!!");
                    ShowPanel();
                    FillGridMain();
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }
        protected void lnkback_Click(object sender, EventArgs e)
        {
            ShowPanel();
            //FillGridMain();
        }
        #endregion

        #region " Other Methods "

        protected void BindCombo()
        {
            ddlProjCategory.FillCombo(DropDownName.Project_Category);
            ddlProjType.FillCombo(DropDownName.Project_Type);
            ddlParentProjName.FillCombo(DropDownName.Project_Name, HdnId.Value);
            ddlFeasibilityStatus.FillCombo(DropDownName.ProjectFeasibility);
            ddlProjectStatus.FillCombo(DropDownName.Project_Status);
            ddlProjSubType.Enabled = false;
        }
        protected void FillGridMain()
        {
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Project_Master> pm = new List<Project_Master>();
            try
            {
                pm = cl.FillGrid_Project_Master().ToList();

                try
                {
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    DataOwner_Entity[] oDataOwner = cl.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);

                    if (IsAdmin == false)
                    {
                        pm.Where(z => z.s_CreatedBy_ID.ToUpper() != UserID).ToList().ForEach(i => i.Status = "View");
                        pm.Where(z => z.s_CreatedBy_ID.ToUpper() == UserID).ToList().ForEach(i => i.Status = "Edit");
                        pm.OrderByDescending(z => z.i_ID);
                    }
                    else
                    {
                        pm.ForEach(i => i.Status = "Edit");
                        pm.OrderByDescending(z => z.i_ID);
                    }
                }
                catch (Exception ex1)
                {

                }

                rptrProjectDetail.DataSource = pm;
                rptrProjectDetail.DataBind();
            }
            catch
            {
                throw;
            }
        }
        public bool Save()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            string result = string.Empty;
            Project_Master pm = new Project_Master();
            Project_Dept_PI pdi = new Project_Dept_PI();
            List<Project_Dept_PI> pdlist = new List<Project_Dept_PI>();
            Project_Coordinator_Details pcd = new Project_Coordinator_Details();
            List<Project_Coordinator_Details> pcdList = new List<Project_Coordinator_Details>();
            try
            {

                pm.i_ID = Convert.ToInt32(Common.iffBlank(HdnId.Value, 0));
                TxtstartDate.Enabled = true;
                TxtDispProjId.Enabled = true;
                pm.Project_StartDate = TxtstartDate.Text;
                pm.s_Display_Project_ID = TxtDispProjId.Text;
                pm.s_Project_Title = Common.SetReplace(TxtprojTitle.Text);
                pm.s_Short_Title = Common.SetReplace(TxtShortTitle.Text);
                pm.s_Project_Alias1 = Common.SetReplace(TxtProjTitleAlias1.Text);
                pm.s_Project_Alias2 = Common.SetReplace(TxtProjTitleAlias2.Text);
                pm.s_Project_Desc = Common.SetReplace(TxtProjDescription.Text);
                pm.s_IRB_No = Common.SetReplace(TxtIRBno.Text);
                pm.i_Project_Category_ID = Convert.ToInt32(Common.iffBlank(ddlProjCategory.SelectedValue, 0));
                pm.i_Project_Type_ID = Convert.ToInt32(Common.iffBlank(ddlProjType.SelectedValue, 0));
                pm.i_Project_Subtype_ID = Convert.ToInt32(Common.iffBlank(ddlProjSubType.SelectedValue, 0));
                pm.i_Parent_ProjectID = Convert.ToInt32(ddlParentProjName.SelectedValue);
                pm.b_IsFeasible = Convert.ToInt32(ddlFeasibilityStatus.SelectedValue);
                pm.b_Isselected_project = ddlselectedproject.SelectedValue == "0" ? false : true;
                pm.b_Collaboration_Involved = ddlCollbrationInv.SelectedValue == "0" ? false : true;
                pm.b_StartBy_TTSH = ddlstartbyTTSH.SelectedValue == "0" ? false : true;
                pm.b_Funding_req = ddlfundingReq.SelectedValue == "0" ? false : true;
                pm.b_Ischild = ddlChildParent.SelectedValue == "1" ? false : true;
                pm.s_Research_IO = txtResearchOrder.Text;
                pm.s_Research_IP = txtReserchInsurance.Text;
                //-------------pass tables to Sp-------------------------
                string[] PiIds = HdnPi_ID.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                for (int i = 0; i < PiIds.Length; i++)
                {
                    pdlist.Add(new Project_Dept_PI { i_PI_ID = Convert.ToInt32(PiIds[i]) });
                }

                string[] coOrIds = HdnCoordinatorId.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                string[] CoText = HdnCoordinatorText.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                for (int i = 0; i < coOrIds.Length; i++)
                {
                    pcdList.Add(new Project_Coordinator_Details { i_Coordinator_ID = Convert.ToString(coOrIds[i]), s_Coordinator_name = CoText[i] });
                }

                //--------UID and UName----
                pm.UName = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString();
                pm.UID = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString();
                pm.i_ProjectStatus = Convert.ToInt32(ddlProjectStatus.SelectedValue);
                pm.Dt_ProjectEndDate = Convert.ToString(TxtProjectEndDate.Text);
                pm.b_EthicsNeeded = (ddlEthicsNeeded.SelectedItem.Text.ToLower() == "yes") ? true : false;

                //----------- END ------------

                /*data owner*/
                pm.s_Ethics_DataOwner = (ddlDO_Ethics.SelectedValue != "0") ? ddlDO_Ethics.SelectedValue : null;
                pm.s_Selected_DataOwner = (ddlDO_Selected.SelectedValue != "0") ? ddlDO_Selected.SelectedValue : null;
                pm.s_Regulatory_DataOwner = (ddlDO_Regulatory.SelectedValue != "0") ? ddlDO_Regulatory.SelectedValue : null;
                pm.s_Feasibility_DataOwner = (ddlDO_Feasibility.SelectedValue != "0") ? ddlDO_Feasibility.SelectedValue : null;
                pm.s_Contract_DataOwner = (ddlDO_Contract.SelectedValue != "0") ? ddlDO_Contract.SelectedValue : null;
                pm.s_Grant_DataOwner = (ddlDO_Grant.SelectedValue != "0") ? ddlDO_Grant.SelectedValue : null;
                /*data owner*/

                //---------------------------------------------------------
                result = cl.Project_Master(pm, pdlist.ToArray(), pcdList.ToArray(), HdnMode.Value);
                if (result.Split('|')[0].ToLower().Trim() == "success" && result.Split('|')[1].ToLower().Trim().CheckInt() == true)
                {
                    switch (HdnMode.Value.ToLower())
                    {
                        //case "update": this.PopUpMsg("Project Details Updated Successfully", "PerformCancel()"); break;
                        //case "delete": this.PopUpMsg("Project Details Deleted Successfully", "PerformCancel()"); break;
                        //case "insert": this.PopUpMsg(" Project Details Saved Successfully", "PerformCancel()"); break;

                        case "update": this.MsgBox("Project Details Updated Successfully"); break;
                        case "delete": this.MsgBox("Project Details Deleted Successfully"); break;
                        case "insert": this.MsgBox(" Project Details Saved Successfully"); break;
                    }
                    ShowPanel();
                    FillGridMain();
                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.ToString());
                return false;
            }

            return true;
        }
        protected bool SavePI()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            string result = string.Empty;
            PI_Master pi = new PI_Master();
            try
            {
                pi.i_ID = Convert.ToInt32("1");
                pi.i_Dept_ID = Convert.ToInt32(HdnNewDeptId.Value);
                pi.s_Firstname = txtPiFirstName.Text;
                pi.s_Lastname = txtPiLastName.Text;
                pi.s_Email = txtPIEmailAddress.Text;
                pi.s_Phone_no = txtPiPhNo.Text;
                pi.s_MCR_No = txtPIMCR_NO.Text;
                result = cl.PI_Master(pi, "Insert");
                if (result.Split('|')[0].ToLower().Trim() == "success" && result.Split('|')[1].ToLower().Trim().CheckInt() == true)
                {
                    this.PopUpMsg(" PI Details Save Successfully..!!", "CallNewPi()");
                    //switch ( HdnMode.Value.ToLower() )
                    //	{
                    //	case "update": this.PopUpMsg("Update Record Successfully..!!", "CallNewPi()"); break;
                    //	case "delete": this.PopUpMsg("Delete Record Successfully..!!", "CallNewPi()"); break;
                    //	case "insert": this.PopUpMsg(" Insert Record Successfully..!!", "CallNewPi()"); break;
                    //	}
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "func", "CallAutocomplete()", true);
                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.ToString());
                return false;

            }
            return true;
        }
        protected void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Project_Master plist = new Project_Master();
            List<PI_Master> List_DEPT_PI = new List<PI_Master>();
            List<Project_Coordinator_Details> List_Co_Ord = new List<Project_Coordinator_Details>();
            try
            {
                ShowPanel("entry");
                plist = cl.GetProject_MasterDetailsByID(Convert.ToInt32(Common.iffBlank(HdnId.Value, 0)));
                BindCombo();
                BindCoOrdinator();
                TxtDispProjId.Text = Common.GetReplace(plist.s_Display_Project_ID);
                DispProjectId.InnerText = TxtDispProjId.Text;
                TxtstartDate.Text = plist.Project_StartDate;
                TxtprojTitle.Text = Common.GetReplace(plist.s_Project_Title);
                TxtShortTitle.Text = Common.GetReplace(plist.s_Short_Title);
                TxtProjTitleAlias1.Text = Common.GetReplace(plist.s_Project_Alias1);
                TxtProjTitleAlias2.Text = Common.GetReplace(plist.s_Project_Alias2);
                TxtProjDescription.Text = Common.GetReplace(plist.s_Project_Desc);
                TxtIRBno.Text = Common.GetReplace(plist.s_IRB_No);

                ddlProjCategory.SelectedIndex = ddlProjCategory.Items.IndexOf(ddlProjCategory.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.i_Project_Category_ID, ""))));
                ddlProjType.SelectedIndex = ddlProjType.Items.IndexOf(ddlProjType.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.i_Project_Type_ID, ""))));
                ddlProjType_SelectedIndexChanged(null, null);
                ddlProjSubType.SelectedIndex = ddlProjSubType.Items.IndexOf(ddlProjSubType.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.i_Project_Subtype_ID, ""))));
                ddlProjSubType.Enabled = (ddlProjSubType.SelectedIndex > 0) ? true : false;
                ddlFeasibilityStatus.SelectedIndex = ddlFeasibilityStatus.Items.IndexOf(ddlFeasibilityStatus.Items.FindByValue(Convert.ToString(plist.b_IsFeasible)));
                HdnFeasibilityStatus.Value = Convert.ToString(plist.b_IsFeasible);
                ddlselectedproject.SelectedIndex = ddlselectedproject.Items.IndexOf(ddlselectedproject.Items.FindByValue(Convert.ToString(plist.b_Isselected_project == true ? "1" : "0")));
                ddlCollbrationInv.SelectedIndex = ddlCollbrationInv.Items.IndexOf(ddlCollbrationInv.Items.FindByValue(Convert.ToString(plist.b_Collaboration_Involved == true ? "1" : "0")));
                if (ddlProjCategory.SelectedItem.Text.ToLower() == "pharma")
                {
                    ddlCollbrationInv.Enabled = false;
                }
                ddlstartbyTTSH.SelectedIndex = ddlstartbyTTSH.Items.IndexOf(ddlstartbyTTSH.Items.FindByValue(Convert.ToString(plist.b_StartBy_TTSH == true ? "1" : "0")));
                ddlfundingReq.SelectedIndex = ddlfundingReq.Items.IndexOf(ddlfundingReq.Items.FindByValue(Convert.ToString(plist.b_Funding_req == true ? "1" : "0")));
                ddlChildParent.SelectedIndex = ddlChildParent.Items.IndexOf(ddlChildParent.Items.FindByValue(Convert.ToString(plist.b_Ischild == true ? "0" : "1")));
                if (ddlChildParent.SelectedValue == "0")
                {
                    ddlParentProjName.Enabled = true; txtParentProjId.Enabled = true;
                    ddlParentProjName.SelectedIndex = ddlParentProjName.Items.IndexOf(ddlParentProjName.Items.FindByValue(Convert.ToString(plist.i_Parent_ProjectID)));
                    ddlParentProjName_SelectedIndexChanged(null, null);
                }
                else
                {
                    ddlParentProjName.Enabled = false; txtParentProjId.Enabled = false;
                }

                //-------Newly Added 31-08-2015------------
                ddlProjectStatus.SelectedIndex = ddlProjectStatus.Items.IndexOf(ddlProjectStatus.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.i_ProjectStatus, ""))));
                TxtProjectEndDate.Text = Convert.ToString(plist.Dt_ProjectEndDate);
                ddlEthicsNeeded.SelectedValue = (plist.b_EthicsNeeded == true) ? "1" : "0";
                //---- END---------------------------------


                ScriptManager.RegisterStartupScript(Page, typeof(Page), "Enable", "BindDoObjects();", true);
                /*dataowner fill*/
                ddlDO_Ethics.SelectedIndex = ddlDO_Ethics.Items.IndexOf(ddlDO_Ethics.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Ethics_DataOwner, ""))));
                ddlDO_Feasibility.SelectedIndex = ddlDO_Feasibility.Items.IndexOf(ddlDO_Feasibility.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Feasibility_DataOwner, ""))));
                ddlDO_Contract.SelectedIndex = ddlDO_Contract.Items.IndexOf(ddlDO_Contract.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Contract_DataOwner, ""))));
                ddlDO_Selected.SelectedIndex = ddlDO_Selected.Items.IndexOf(ddlDO_Ethics.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Selected_DataOwner, ""))));
                ddlDO_Regulatory.SelectedIndex = ddlDO_Regulatory.Items.IndexOf(ddlDO_Regulatory.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Regulatory_DataOwner, ""))));
                ddlDO_Grant.SelectedIndex = ddlDO_Grant.Items.IndexOf(ddlDO_Grant.Items.FindByValue(Convert.ToString(Common.iffBlank(plist.s_Grant_DataOwner, ""))));

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "enable", "alert('Hello!')", true);

                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "enable", "BindDoObjects();", true);
                /**/

                List_DEPT_PI = plist.DEPT_PI.ToList();
                var q = (from i in List_DEPT_PI select new { i.i_Dept_ID, i.i_ID }).ToList().ListToDatatable();
                rptrPIDetails.DataSource = List_DEPT_PI;
                rptrPIDetails.DataBind();

                txtResearchOrder.Text = Common.GetReplace(plist.s_Research_IO);
                txtReserchInsurance.Text = Common.GetReplace(plist.s_Research_IP);
                List_Co_Ord = plist.COORDINATOR.ToList();
                for (int j = 0; j < chkboxlist.Items.Count; j++)
                {
                    for (int i = 0; i < List_Co_Ord.Count; i++)
                    {
                        if (chkboxlist.Items[j].Value == Convert.ToString(List_Co_Ord[i].i_Coordinator_ID))
                        {
                            chkboxlist.Items[j].Selected = true;
                            TextSearch.Text += chkboxlist.Items[j].Text + ",";
                        }
                    }
                }
                if (TextSearch.Text != "")
                {
                    TextSearch.Text = TextSearch.Text.TrimEnd(',');

                }

                //	TxtDispProjId.Attributes.Add("onblur", "javascript:return GetValidatefrmDB('" + HdnError.ClientID + "','ValidateDispID' ,'" + TxtDispProjId.ClientID + "','" + HdnId.Value + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "ClearAll('" + HdnMode.Value + "');", true);

                if (HdnMode.Value.ToLower() == "update")
                {

                    String s = cl.GetValidate("RestrictChild", HdnId.Value, "", "", "");
                    if (s != "")
                    {
                        ddlChildParent.Enabled = false;
                        ddlChildParent.Attributes.Add("title", "Child Project is Created for this Project..!!");
                    }
                }
                ChangeButtonText();

                MakeControlValidate();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        protected void CallJavascript()
        {
            btnMorePiCancel.Attributes.Add("onclick", "javascript:return ClearCloseMorePiSection();");
            btnPICancel.Attributes.Add("onclick", "javascript:return CallNewPi();");
            TxtDispProjId.Attributes.Add("onblur", "javascript:return GetValidatefrmDB('" + HdnError.ClientID + "','ValidateDispID' ,'" + TxtDispProjId.ClientID + "','" + HdnId.Value + "');");
            btnMorePiSave.Attributes.Add("onclick", "javascript:return SaveMorePi('" + TxtDepartment.ClientID + "', '" + TxtPIName.ClientID + "','" + txtPIEmail.ClientID + "', '" + txtPiPhoneNo.ClientID + "', '" + txtPiMCRNo.ClientID + "', '" + HdnpiId.ClientID + "','" + rptrPIDetails.ClientID + "');");
            btnSave.Attributes.Add("onclick", "javascript:return IsValidate('" + HdnPi_ID.ClientID + "','" + chkboxlist.ClientID + "','" + TxtDispProjId.ClientID + "', '" + TxtprojTitle.ClientID + "', '" + TxtstartDate.ClientID + "', '" + ddlProjCategory.ClientID + "','" + ddlProjSubType.ClientID + "', '" + ddlProjType.ClientID + "', '" + ddlFeasibilityStatus.ClientID + "', '" + ddlCollbrationInv.ClientID + "', '" + ddlfundingReq.ClientID + "', '" + ddlParentProjName.ClientID + "','" + ddlstartbyTTSH.ClientID + "', '" + ddlChildParent.ClientID + "', '" + txtParentProjId.ClientID + "','" + HdnCoordinatorId.ClientID + "','" + HdnCoordinatorText.ClientID + "', '" + HdnMode.ClientID + "');");
            ddlChildParent.Attributes.Add("onchange", "javascript:EnableParentControls(this, '" + txtParentProjId.ClientID + "', '" + ddlParentProjName.ClientID + "');");
            btnPISave.Attributes.Add("onclick", "javascript:return ValidateNewPi('" + TxtNewDepartment.ClientID + "', '" + txtPiFirstName.ClientID + "', '" + txtPIEmailAddress.ClientID + "','" + txtPiLastName.ClientID + "', '" + txtPIMCR_NO.ClientID + "');");

            ddlProjCategory.Attributes.Add("onchange", "javascript:SetCollaboratorOnProjectCategory();");
            chkboxlist.Attributes.Add("onclick", "javascript:return SetChkFilterforALLkWithCount(this,'" + TextSearch.ClientID + "');");

        }
        protected void ShowPanel(string type = "main")
        {
            DivMain.Style["display"] = "block";
            ProjectDetailContainer.Style["display"] = "block";
            hrMorePi.Visible = true; PMorePi.Visible = true; Pmore.Visible = true;

            if (type.ToLower() == "main")
            {
                // btnNew.Focus();

                ProjectDetailContainer.Style["display"] = "none";
            }
            else
            {
                switch (HdnMode.Value.ToLower())
                {
                    case "new":
                    case "insert":
                        Common.EnableAllandClearControl(Master, true, true);
                        break;
                    case "update":
                        Common.EnableAllandClearControl(Master, true, true);
                        break;
                    case "delete":
                    case "view":
                        Common.EnableAllandClearControl(Master, false, true);
                        hrMorePi.Visible = false; PMorePi.Visible = false;

                        Pmore.Visible = false;
                        break;

                }

                DivMain.Style["display"] = "none";
                DataTable dt = new DataTable();
                rptrPIDetails.DataSource = dt; rptrPIDetails.DataBind();
            }

        }
        protected void ChangeButtonText()
        {
            btnSave.Text = "Save Details";
            btnMorePiSave.Text = "Save";
            btnPISave.Text = "Save";

            btnSave.Visible = true;
            btnMorePiSave.Visible = true;
            btnPISave.Visible = true;

            btnCancel.Visible = true;
            btnMorePiCancel.Visible = true;
            btnPICancel.Visible = true;

            btnCancel.Enabled = true;
            btnMorePiSave.Enabled = true;
            btnPISave.Enabled = true;
            btnMorePiCancel.Enabled = true;
            btnPICancel.Enabled = true;
            switch (HdnMode.Value.ToLower())
            {
                case "delete":
                    btnSave.Text = "Delete Details";
                    btnMorePiSave.Enabled = false;
                    btnMorePiCancel.Enabled = false;
                    btnPISave.Enabled = false;
                    btnPICancel.Enabled = false;
                    break;
                case "view":
                    btnSave.Visible = false;
                    btnMorePiSave.Visible = false;
                    btnPISave.Visible = false;
                    btnMorePiCancel.Visible = false;
                    btnPICancel.Visible = false;
                    break;
                case "update":
                    btnSave.Text = "Update Details";
                    break;
            }

        }
        protected void MakeControlValidate()
        {
            if (HdnMode.Value.ToLower() == "update")
            {
                TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
                string s = cl.GetValidate("ValidateProjectStatusFeild", HdnId.Value, "", "", "");
                if (s != "")
                {
                    string[] arr = s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string procatID = arr[0];
                    string ProTypeID = arr[1];
                    string FeasibleStatus = arr[2];
                    string SelProject = arr[3];
                    string ColabInv = arr[4];
                    string funcReq = arr[5];
                    string ENeed = arr[6];
                    string Reg = arr[7];
                    string Grant = arr[8];
                    string S_TTSH = arr[9];
                    
                    if (ENeed != "")
                    {
                        if (ENeed == "1")
                        {
                            ddlEthicsNeeded.Enabled = false;
                            ddlEthicsNeeded.Attributes.Add("title", "Ethics Needed Can't ne Changed");
                        }
                    }
                    if (FeasibleStatus != "")
                    {
                        if (FeasibleStatus == "1" || FeasibleStatus == "2")
                        {
                            ddlProjCategory.Enabled = false;
                            ddlProjCategory.Attributes.Add("title", "Project Category Can't be Changed ");

                            ddlFeasibilityStatus.Enabled = false;
                            ddlFeasibilityStatus.Attributes.Add("title", "Feasibility Status Can't be Changed "); FeasibleExist.Value = "1";
                        }

                    }
                    if (SelProject != "")
                    {
                        if (SelProject == "1")
                        {
                            ddlselectedproject.Enabled = false;
                            ddlselectedproject.Attributes.Add("title", "Selected Project Can't be Changed ");
                        }

                    }
                    if (ColabInv != "")
                    {
                        if (ColabInv == "1")
                        {
                            ddlCollbrationInv.Enabled = false; CollabExist.Value = "1";
                            ddlCollbrationInv.Attributes.Add("title", "Collaboration Involved Can't be Changed ");
                        }

                    }
                    if (Reg == "1")
                    {
                        ddlProjCategory.Enabled = false;
                        ddlProjCategory.Attributes.Add("title", "Project Category Can't be Changed ");
                        ddlProjType.Enabled = false;
                        ddlProjType.Attributes.Add("title", "Project Type Can't be Changed ");
                    }
                    if (Grant == "1")
                    {
                        ddlProjCategory.Enabled = false;
                        ddlProjCategory.Attributes.Add("title", "Project Category Can't be Changed ");
                        ddlProjType.Enabled = false;
                        ddlProjType.Attributes.Add("title", "Project Type Can't be Changed ");

                        ddlstartbyTTSH.Enabled = false;
                        ddlstartbyTTSH.Attributes.Add("title", "Start by TTSH  Can't be Changed ");

                        ddlfundingReq.Enabled = false;
                        ddlfundingReq.Attributes.Add("title", "Funding Required  Can't be Changed ");
                    }

                }

            }
        }
        protected void ClearHDN()
        {
            HdnCoordinatorId.Value = "0";
            HdnCoordinatorText.Value = "";
            HdnDeptId.Value = "0";
            HdnDeptTxt.Value = "";
            HdnError.Value = "";
            HdnId.Value = "0";
            HdnMode.Value = "Insert";
            HdnNewDeptId.Value = "0";
            HdnPi_ID.Value = "0";
            HdnpiId.Value = "0";
            HdnPITxt.Value = "";
            HdnFeasibilityStatus.Value = "0";
            FeasibleExist.Value = "0"; CollabExist.Value = "0";
        }



        protected void BindDataOwners()
        {
            try
            {
                TTSHWCFServiceClient oSClient = new TTSHWCFServiceClient();
                oSClient.Open();
                DataOwner_Entity[] oDataOwner = oSClient.GetAllDataOwner(null);

                if (oDataOwner != null && oDataOwner.Count() > 0)
                {

                    ddlDO_Contract.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TProjectMonitoring".ToUpper());
                    ddlDO_Contract.DataValueField = "GUID";
                    ddlDO_Contract.DataTextField = "MemberName";
                    ddlDO_Contract.DataBind();
                    ddlDO_Contract.Items.Insert(0, new ListItem("-- Select--", "0"));


                    ddlDO_Grant.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TGRANT");
                    ddlDO_Grant.DataValueField = "GUID";
                    ddlDO_Grant.DataTextField = "MemberName";
                    ddlDO_Grant.DataBind();
                    ddlDO_Grant.Items.Insert(0, new ListItem("-- Select--", "0"));

                    ddlDO_Ethics.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TProjectMonitoring".ToUpper());
                    ddlDO_Ethics.DataValueField = "GUID";
                    ddlDO_Ethics.DataTextField = "MemberName";
                    ddlDO_Ethics.DataBind();
                    ddlDO_Ethics.Items.Insert(0, new ListItem("-- Select--", "0"));

                    ddlDO_Feasibility.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TProjectMonitoring".ToUpper());
                    ddlDO_Feasibility.DataValueField = "GUID";
                    ddlDO_Feasibility.DataTextField = "MemberName";
                    ddlDO_Feasibility.DataBind();
                    ddlDO_Feasibility.Items.Insert(0, new ListItem("-- Select--", "0"));

                    ddlDO_Regulatory.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TREGULATORY");
                    ddlDO_Regulatory.DataValueField = "GUID";
                    ddlDO_Regulatory.DataTextField = "MemberName";
                    ddlDO_Regulatory.DataBind();
                    ddlDO_Regulatory.Items.Insert(0, new ListItem("-- Select--", "0"));

                    ddlDO_Selected.DataSource = oDataOwner.Where(z => z.GroupName.ToUpper() == "TProjectMonitoring".ToUpper());
                    ddlDO_Selected.DataValueField = "GUID";
                    ddlDO_Selected.DataTextField = "MemberName";
                    ddlDO_Selected.DataBind();
                    ddlDO_Selected.Items.Insert(0, new ListItem("-- Select--", "0"));



                }


                oSClient.Close();

            }
            catch (Exception ex)
            {

            }

        #endregion



        }


    }
}