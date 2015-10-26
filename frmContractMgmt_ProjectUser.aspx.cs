using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

using TTSHWeb.TTSHWCFReference;


namespace TTSHWeb
{
    public partial class frmContractMgmt_ProjectUser : System.Web.UI.Page
    {
        string LoginUser = "";
        string LoginUserId = "";
        #region " page Event "
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.CONTRACT;
                SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
                SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;
                if (!IsPostBack)
                {
                    TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                    t.Text = "";
                    FillGridMain();
                    CallJavascript();
                    ShowPanel();
                    FillReviewdBy();
                }
                //Get Login User Details
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    LoginUser = HttpContext.Current.Session["UserName"].ToString();

                }
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    LoginUserId = HttpContext.Current.Session["UserID"].ToString();

                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        #endregion


        #region " Reapeater Event "
        protected void rptrContractDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "")
                {
                    HdnId.Value = e.CommandArgument.ToString();
                    HdnProject_Id.Value = e.CommandArgument.ToString();
                    if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview" | e.CommandName.ToLower() == "cmdadd")
                    {
                        HdnMode.Value = e.CommandName.ToString().ConverMode();
                        ShowPanel("entry");
                        if (HdnMode.Value.ToLower() != "insert")
                            FillControl();
                        else
                            GetProjectDetailsAtNew();

                    }
                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        #endregion

        #region " Button Event "
        protected void btnNewCollaboratorSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCollaborator();
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
                if (SaveContractDetail())
                {
                    ShowPanel();
                    FillGridMain();

                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                FillGridMain();
                ShowPanel();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        protected void lnkback_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPanel();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            try
            {
                FillGridMain();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }

        void SearchBox_ButtonSearchClick(object sender, EventArgs e)
        {
            try
            {
                SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;

                if (string.IsNullOrEmpty(SearchBox.ErrorString))
                {

                    TTSHWCFServiceClient client = new TTSHWCFServiceClient();
                    client.Open();

                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("CONTRACT", UserID);
                    DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);


                    List<Search> oNewGrid = new List<Search>();
                    List<Search> oOldSearch = new List<Search>();

                    List<Search> lst = SearchBox.SearchOutput.ToList();
                    rptrContractDetail.DataSource = null;
                    rptrContractDetail.DataBind();
                    if (!IsAdmin)
                    {
                        if (lst != null && lst.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                        {
                            oOldSearch = lst.ToList();
                            oNewGrid = oOldSearch.Where(z => z.iRecordExists == 0).Where(z => oDOList.Any(x => x.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
                            oOldSearch.RemoveAll(z => z.iRecordExists == 0);
                            oOldSearch.AddRange(oNewGrid);

                            foreach (var element in oOldSearch)
                            {
                                if (element.iRecordExists == 0)
                                {
                                    element.Status = "New";
                                }
                                else
                                {
                                    bool flag = false;
                                    foreach (var item in oDOList)
                                    {
                                        if (item.s_DisplayProject_ID == element.s_Display_Project_ID)
                                        {
                                            flag = true;
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (flag == true)
                                    {
                                        element.Status = "Edit";
                                    }
                                    else
                                    {
                                        element.Status = "View";
                                    }
                                }
                            }

                            oOldSearch = oOldSearch.OrderByDescending(z => z.i_ID).ToList();

                            rptrContractDetail.DataSource = oOldSearch; /*use the object according to your need*/
                            rptrContractDetail.DataSource = oOldSearch;
                            rptrContractDetail.DataBind();
                        }
                        else
                        {

                            oOldSearch = lst.ToList();
                            oNewGrid = oOldSearch.Where(z => z.iRecordExists == 0).ToList();
                            oOldSearch.RemoveAll(z => z.iRecordExists == 0);
                            oOldSearch.AddRange(oNewGrid);

                            foreach (var element in oOldSearch)
                            {
                                if (element.iRecordExists == 0)
                                {
                                    element.Status = "New";
                                }
                                else
                                {
                                    element.Status = "Edit";
                                }
                            }

                            rptrContractDetail.DataSource = oOldSearch; //use the object according to your need
                            rptrContractDetail.DataBind();

                        }

                    }
                    else
                    {
                        lst.Where(x => x.Status.ToUpper() == "NO").ToList().ForEach(x => x.Status = "Edit");
                        lst.Where(x => x.Status.ToUpper() == "YES").ToList().ForEach(x => x.Status = "New");
                        lst.OrderByDescending(z => z.i_Project_ID);

                        rptrContractDetail.DataSource = lst; //use the object according to your need
                        rptrContractDetail.DataBind();
                    }

                }
                else
                {
                    rptrContractDetail.DataSource = null;
                    rptrContractDetail.DataBind();
                }

            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        #endregion

        #region " Method "
        protected void CallJavascript()
        {
            try
            {
                btnMCollaboratrCancel.Attributes.Add("onclick", "return ClearCloseMoreCollabortorControls();");
                btnNewCollaboratorCancel.Attributes.Add("onclick", "return CallNCollaboratorDetails()");
                btnMCollaboratrSave.Attributes.Add("onclick", "return SaveMoreCollabortor('" + TxtMCollaborator.ClientID + "','" + HdnMCollaboratorId.ClientID + "' , '" + TxtMInstitution.ClientID + "','" + TxtMPhNo.ClientID + "','" + TxtContractId.ClientID + "', '" + TxtMEmailAdd1.ClientID + "','" + TxtMEmailAdd2.ClientID + "','" + TxtMCountry.ClientID + "','" + TxtContractReqDate.ClientID + "');");
                btnSave.Attributes.Add("onclick", "return IsValidate('" + HdnMode.ClientID + "','" + HdnProject_Id.ClientID + "', '" + HdnContract_Collaborator_Details.ClientID + "','" + DdlReviewedBy.ClientID + "','" + TxtContAssignDate.ClientID + "','" + HdnContractReqDate.ClientID + "');");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        protected bool SaveCollaborator()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Contract_Collobrator_Master ccm = new Contract_Collobrator_Master(); string result = string.Empty;
            try
            {
                ccm.i_ID = 0;
                ccm.s_Name = TxtNCollabator.Text;
                ccm.s_Email1 = TxtNEmail1.Text;
                ccm.s_Institution = TxtNInstitution.Text;
                ccm.s_Email2 = TxtNEmail2.Text;
                ccm.s_PhoNo = TxtNPhNo.Text;
                if (HdnNCountryId.Value != "")
                {
                    ccm.i_Country_ID = Convert.ToInt32(HdnNCountryId.Value);
                }

                result = cl.Contract_Collobrator_Master_DML(ccm, "Insert");
                if (result.Split('|')[0].ToLower().Trim() == "success" && result.Split('|')[1].ToLower().Trim().CheckInt() == true)
                {
                    this.PopUpMsg("Collaborator Details Saved Successfully", "CallNCollaboratorDetails()");
                    //switch ( HdnMode.Value.ToLower() )
                    //	{
                    //	case "update": this.PopUpMsg("Collaborator Update Successfully..!!", "CallNCollaboratorDetails();"); break;
                    //	case "delete": this.PopUpMsg("Collaborator Delete Successfully..!!", "CallNCollaboratorDetails()"); break;
                    //	case "insert": this.PopUpMsg(" Collaborator Save Successfully..!!", "CallNCollaboratorDetails()"); break;
                    //	}


                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                    return false;
                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
                return false;
            }
            return true;
        }
        protected void FillGridMain()
        {
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Contract_Master> cmlist = new List<Contract_Master>();
            try
            {

                string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                Project_DataOwner[] oDOList = cl.GetProjectsByDO("CONTRACT", UserID);
                DataOwner_Entity[] oDataOwner = cl.GetAllDataOwner("TAdmin");
                var AdminArray = (from s in oDataOwner
                                  select s.GUID).ToList();

                bool IsAdmin = AdminArray.Contains(UserID);

                cmlist = cl.FillGrid_Contract_Master().ToList();
                if (!IsAdmin)
                {
                    List<Contract_Master> oNewGrid = new List<Contract_Master>();
                    if (cmlist != null && cmlist.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                    {
                        //Available For Add To The Current User
                        oNewGrid.AddRange(cmlist.Where(x => x.Status.ToUpper() == "YES" && x.ContAppStatus.ToUpper() == "NO").Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)));
                        oNewGrid.ForEach(i => i.Status = "New");
                        cmlist.RemoveAll(z => z.Status.ToUpper() == "YES" && z.ContAppStatus.ToUpper() == "NO");
                        cmlist.AddRange(oNewGrid);

                        //WHose Status Filled By Curent User
                        cmlist.Where(x => x.Status.ToUpper() == "NO").Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList().ForEach(x => x.Status = "Edit");

                        //Whose Status Available For View Only
                        cmlist.Where(x => x.Status.ToUpper() == "NO").Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID != z.s_Display_Project_ID)).ToList().ForEach(x => x.Status = "View");

                        cmlist.OrderByDescending(z => z.i_Project_ID);

                    }
                    else if (cmlist != null && cmlist.Count() > 0)
                    {
                        cmlist.ForEach(x => x.Status = "View");
                        cmlist.OrderByDescending(z => z.i_Project_ID);
                    }

                }
                else
                {
                    cmlist.Where(x => x.Status.ToUpper() == "NO").ToList().ForEach(x => x.Status = "Edit");
                    cmlist.Where(x => x.Status.ToUpper() == "YES").ToList().ForEach(x => x.Status = "New");
                    cmlist.OrderByDescending(z => z.i_Project_ID);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            rptrContractDetail.DataSource = cmlist;
            rptrContractDetail.DataBind();
        }
        public bool SaveContractDetail()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Contract_Master cm = new Contract_Master();
            List<Contract_Collaborator_Details> lst = new List<Contract_Collaborator_Details>();
            DataTable dt = (DataTable)ViewState["dt"]; string result = string.Empty;
            try
            {
                string[] collaboratorValues = HdnContract_Collaborator_Details.Value.Replace("^", "").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                for (int i = 0; i < collaboratorValues.Length; i++)
                {
                    lst.Add(new Contract_Collaborator_Details
                    {
                        i_Contract_Master_ID = Convert.ToInt32("1"),
                        i_Contract_Collaborator_ID = Convert.ToInt32(collaboratorValues[i].Split(',')[0]),
                        s_InitialContract_ID = Convert.ToString(collaboratorValues[i].Split(',')[1]),
                        dt_Contract_Request_Date = (collaboratorValues[i].Split(',')[2])
                    });
                }
                cm.i_ID = Convert.ToInt32(HdnId.Value);
                cm.i_Project_ID = Convert.ToInt32(HdnProject_Id.Value);//project Id
                cm.dt_Contract_AssignDate = TxtContAssignDate.Text;
                cm.i_ReviewedBy_ID = Convert.ToString(DdlReviewedBy.SelectedValue);
                cm.S_ReviewedByName = DdlReviewedBy.SelectedItem.Text;

                cm.s_Short_Title = TxtShortTitle.Text;
                cm.s_Project_Alias1 = TxtAlias1.Text;
                cm.s_Project_Alias2 = TxtAlias2.Text;
                cm.s_IRB_No = TxtIrbNo.Text;

                //--------UID and UName----
                cm.UName = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString();
                cm.UID = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString();
                //----------- END ------------

                result = cl.Contract_Master_DML(cm, lst.ToArray(), HdnMode.Value.ToLower());
                if (result.Split('|')[0].ToLower().Trim() == "success" && result.Split('|')[1].ToLower().Trim().CheckInt() == true)
                {

                    switch (HdnMode.Value.ToLower())
                    {
                        //case "update": this.PopUpMsg("Contract Application Details Updated  Successfully", "PerformCancel();"); break;
                        //case "delete": this.PopUpMsg("Contract Application Details Deleted  Successfully", "PerformCancel();"); break;
                        //case "insert": this.PopUpMsg("Contract Application Details Saved    Successfully", "PerformCancel();"); break;

                        case "update": this.MsgBox("Contract Application Details Updated  Successfully"); break;
                        case "delete": this.MsgBox("Contract Application Details Deleted  Successfully"); break;
                        case "insert": this.MsgBox("Contract Application Details Saved    Successfully"); break;
                    }

                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
                return false;
            }
            return true;
        }
        protected void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Project_Master> Pjctlist = new List<Project_Master>();
            List<Contract_Collobrator_Master> ccmlist = new List<Contract_Collobrator_Master>();
            List<PI_Master> pi_mlist = new List<PI_Master>();
            Contract_Master cmlist = new Contract_Master();
            try
            {
                cmlist = cl.GetContract_MasterDetailsByID(Convert.ToInt32(HdnId.Value));
                Pjctlist = cmlist.pjctmList.ToList();
                TxtProjTitle.Text = Pjctlist.ListToDatatable().Rows[0]["s_Project_Title"].ToString();
                TxtAlias1.Text = Pjctlist.ListToDatatable().Rows[0]["s_Project_Alias1"].ToString();
                TxtAlias2.Text = Pjctlist.ListToDatatable().Rows[0]["s_Project_Alias2"].ToString();
                TxtShortTitle.Text = Pjctlist.ListToDatatable().Rows[0]["s_Short_Title"].ToString();
                TxtIrbNo.Text = Pjctlist.ListToDatatable().Rows[0]["s_IRB_No"].ToString();
                TxtprojCategory.Text = Pjctlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                HdnProject_Id.Value = Pjctlist.ListToDatatable().Rows[0]["i_ID"].ToString();
                DispProjectId.InnerText = Pjctlist.ListToDatatable().Rows[0]["s_Display_Project_ID"].ToString();
                pi_mlist = cmlist.pmlist.ToList();
                rptrPIDetails.DataSource = pi_mlist; rptrPIDetails.DataBind();
                TxtContAssignDate.Text = cmlist.dt_Contract_AssignDate;
                TxtContractReqDate.Text = cmlist.dt_Contract_ReqDate;
                ccmlist = cmlist.ccdlist.ToList();
                RptrCollaborator.DataSource = ccmlist; RptrCollaborator.DataBind();
                FillReviewdBy();
                DdlReviewedBy.SelectedIndex = DdlReviewedBy.Items.IndexOf(DdlReviewedBy.Items.FindByValue(Convert.ToString(cmlist.i_ReviewedBy_ID)));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        protected void ShowPanel(string Type = "main")
        {
            try
            {
                DivMain.Style["display"] = "block"; DivContractDetailContainer.Style["display"] = "block";
                btnSave.Text = "Save Details";
                btnSave.Visible = true;
                btnMCollaboratrSave.Visible = true; btnMCollaboratrCancel.Visible = true;
                btnNewCollaboratorSave.Visible = true; btnNewCollaboratorCancel.Visible = true;
                PMoreCollaborator.Visible = true; PNewCollaborator.Visible = true;
                switch (Type.ToLower())
                {
                    case "main":
                        DivContractDetailContainer.Style["display"] = "none";
                        break;
                    case "entry":
                        DivMain.Style["display"] = "none";
                        switch (HdnMode.Value.ToLower())
                        {
                            case "new":
                            case "insert":
                                ClearControl();

                                break;
                            case "update":
                                btnSave.Text = "Update Details";
                                break;
                            case "delete":
                                btnSave.Text = "Delete Details";
                                PMoreCollaborator.Visible = false; PNewCollaborator.Visible = false;
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "ClearAll('" + HdnMode.Value + "')", true);
                                break;
                            case "view":
                                btnSave.Visible = false;
                                PMoreCollaborator.Visible = false; PNewCollaborator.Visible = false;
                                btnMCollaboratrSave.Visible = false; btnMCollaboratrCancel.Visible = false;
                                btnNewCollaboratorSave.Visible = false; btnNewCollaboratorCancel.Visible = false;
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "ClearAll('" + HdnMode.Value + "')", true);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        protected void ClearControl()
        {
            try
            {
                TxtProjTitle.Text = ""; TxtAlias1.Text = ""; TxtAlias2.Text = ""; TxtShortTitle.Text = ""; TxtprojCategory.Text = ""; TxtIrbNo.Text = "";
                rptrPIDetails.DataSource = null; rptrPIDetails.DataBind(); RptrCollaborator.DataSource = null; RptrCollaborator.DataBind();
                TxtNCollabator.Text = ""; TxtNEmail1.Text = ""; TxtNInstitution.Text = ""; TxtNCountry.Text = ""; TxtNEmail2.Text = ""; TxtNPhNo.Text = "";
                TxtMCollaborator.Text = ""; TxtMInstitution.Text = ""; TxtMPhNo.Text = ""; TxtContractId.Text = ""; TxtMEmailAdd1.Text = ""; TxtMEmailAdd2.Text = ""; TxtMCountry.Text = "";
                DdlReviewedBy.SelectedIndex = 0;

            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        protected void GetProjectDetailsAtNew()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Project_Master plist = new Project_Master(); List<PI_Master> List_DEPT_PI = new List<PI_Master>();
            try
            {
                plist = cl.GetProject_MasterDetailsByID(Convert.ToInt32(Common.iffBlank(HdnProject_Id.Value, 0)));
                TxtProjTitle.Text = plist.s_Project_Title;
                TxtAlias1.Text = plist.s_Project_Alias1;
                TxtShortTitle.Text = plist.s_Short_Title;
                TxtprojCategory.Text = plist.Project_Category_Name;
                TxtAlias2.Text = plist.s_Project_Alias2;
                TxtIrbNo.Text = plist.s_IRB_No;
                DispProjectId.InnerText = plist.s_Display_Project_ID;
                List_DEPT_PI = plist.DEPT_PI.ToList();
                rptrPIDetails.DataSource = List_DEPT_PI; rptrPIDetails.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void FillReviewdBy()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                DataOwner_Entity[] oDataOwner = cl.GetAllDataOwner("TCONTRACT");
                DataTable dt = new DataTable();
                dt.Columns.Add("value");
                dt.Columns.Add("text");
                dt.Rows.Add("0", "--Select--");
                if (oDataOwner.Length > 0)
                {
                    for (int i = 0; i < oDataOwner.Length; i++)
                    {
                        dt.Rows.Add(oDataOwner[i].GUID, oDataOwner[i].MemberName);
                    }
                }
                //dt.Rows.Add("1", "Chao-yu");
                //dt.Rows.Add("2", "Xiao-nan");
                //dt.Rows.Add("3", "Ming-zhu");
                //dt.Rows.Add("4", "Han-yue");
                //dt.Rows.Add("5", "Langlang Xuezhi");
                //dt.Rows.Add("6", "Chenlang");
                //dt.Rows.Add("7", "Gaomin");
                //DdlReviewedBy.DataSource = dt;
                //DdlReviewedBy.DataTextField = "text";
                //DdlReviewedBy.DataValueField = "value";
                //DdlReviewedBy.DataBind();

                DdlReviewedBy.DataSource = dt;
                DdlReviewedBy.DataTextField = "text";
                DdlReviewedBy.DataValueField = "value";
                DdlReviewedBy.DataBind();
                DdlReviewedBy.SelectedIndex = 0;//reviewd by Id feild Int change to String
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("DeleteContractMaster", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Contract Application Details Deleted Successfully..!!");
                    ShowPanel();
                    FillGridMain();
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }



    }
}