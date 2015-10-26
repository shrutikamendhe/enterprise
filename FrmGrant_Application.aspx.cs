using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.UI.HtmlControls;
namespace TTSHWeb
{
    public partial class FrmGrant_Application : System.Web.UI.Page
    {

        string LoginUser = string.Empty;
        string LoginUserId = string.Empty;
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.Grant_Master;
            SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
            SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;
            try
            {


                if (!IsPostBack)
                {
                    ShowPanel();
                    FillGridMain();
                    BindCombo();

                }
                CallJavascript();
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

                this.MsgBox(ex.Message.ToString());
            }
        }
        #endregion

        #region Search Box Events
        private void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            try
            {
                FillGridMain();
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }

        private void SearchBox_ButtonSearchClick(object sender, EventArgs e)
        {
            try
            {
                SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;

                if (string.IsNullOrEmpty(SearchBox.ErrorString))
                {
                    TTSHWCFServiceClient client = new TTSHWCFServiceClient();
                    client.Open();
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("GRANT", UserID);
                    DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");
                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();
                    bool IsAdmin = AdminArray.Contains(UserID);

                    List<Search> lst = SearchBox.SearchOutput.ToList();

                    if (!IsAdmin)
                    {
                        List<Search> oNewGrid = new List<Search>();
                        //var v = cmlist.Select(z => z.Status).Distinct().ToList();
                        ////WHose Status Filled By Curent User
                        oNewGrid = lst.Where(x => x.i_ID > 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
                        oNewGrid.ForEach(x => x.Status = "Edit");
                        ////Whose Status Available For View Only
                        oNewGrid.AddRange(lst.Where(x => x.i_ID > 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID != z.s_Display_Project_ID)).ToList());
                        oNewGrid.Where(x => (string.IsNullOrEmpty(x.Status))).ToList().ForEach(x => x.Status = "View");
                        //Available For Add To The Current User
                        oNewGrid.AddRange(lst.Where(x => x.i_ID == 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList());
                        oNewGrid.Where(x => (string.IsNullOrEmpty(x.Status))).ToList().ForEach(x => x.Status = "New");
                        oNewGrid.Where(x => x.isGrantDetailsApplied == true).ToList().ForEach(x => x.Status = "View");

                        lst = oNewGrid;
                    }
                    else
                    {
                        lst.Where(x => x.i_ID > 0).ToList().ForEach(x => x.Status = "Edit");
                        lst.Where(x => x.i_ID == 0).ToList().ForEach(x => x.Status = "New");
                        lst.Where(x => x.isGrantDetailsApplied == true).ToList().ForEach(x => x.Status = "View");

                        rptrGrantMaster.DataSource = lst;
                        rptrGrantMaster.DataBind();
                    }
                    rptrGrantMaster.DataSource = lst;
                    rptrGrantMaster.DataBind();
                }
                else
                {
                    //  this.MsgBox(SearchBox.ErrorString);
                    rptrGrantMaster.DataSource = null;
                    rptrGrantMaster.DataBind();
                }
                CallJS("ShowNoRecords();");

            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }
        #endregion

        #region Repeater Events
        protected void rptrGrantMaster_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "")
                {
                    ClearHDN();

                    if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview" | e.CommandName.ToLower() == "cmdadd")
                    {
                        HdnMode.Value = e.CommandName.ToString().ConverMode();

                        ShowPanel("entry");
                        bool enabled = (e.CommandName.ToString().ConverMode().ToLower() == "delete" || e.CommandName.ToString().ConverMode().ToLower() == "view") ? false : true;

                        ClearAllRepeaterControls();
                        if (HdnMode.Value.ToLower() == "insert")
                        {
                            string Id = e.CommandArgument.ToString().Split('|')[0];
                            HdnProjectId.Value = Id;
                            HdnParentProjCount.Value = e.CommandArgument.ToString().Split('|')[1];

                            HdnForChild.Value = (e.CommandArgument.ToString().Split('|')[2].ToLower() == "m" && e.CommandArgument.ToString().Split('|')[3].ToLower() == "m") ? "1" : "0";

                            HdnChildParent.Value = (e.CommandArgument.ToString().Split('|')[4].ToLower() == "child") ? "1" : "0";

                            FillProjectDataForNewEntry();
                        }
                        else
                        {
                            HdnGrantId.Value = e.CommandArgument.ToString();
                            FillControl();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }
        #endregion

        #region DropDown Change events

        protected void ddlGrantType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CallJs("ApplyReDatePicker();Changeevents();");
                ddlGrantSubType1.FillCombo(DropDownName.GrantSubType1, ddlGrantType.SelectedValue);
                ddlGrantSubType1.Enabled = ddlGrantType.SelectedValue != "0";
                ddlGrantSubType1.Enabled = ddlGrantSubType1.Items.Count > 1;
                ddlGrantSubType1.Enabled = ddlGrantType.SelectedValue != "0";
                ddlGrantSubType2.Enabled = false;
                ddlGrantSubType3.Enabled = false;
                ddlGrantSubType1.SelectedIndex = -1;
                ddlGrantSubType2.SelectedIndex = -1;
                ddlGrantSubType3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }

        protected void ddlGrantSubType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CallJs("ApplyReDatePicker();Changeevents();");
                ddlGrantSubType2.FillCombo(DropDownName.GrantSubType2, ddlGrantSubType1.SelectedValue);
                //if (ddlGrantSubType2.Items.Count > 0)
                ddlGrantSubType2.Enabled = ddlGrantSubType1.SelectedValue != "0";
                ddlGrantSubType2.Enabled = ddlGrantSubType2.Items.Count > 1;
                ddlGrantSubType3.Enabled = false;
                ddlGrantSubType2.SelectedIndex = -1;
                ddlGrantSubType3.SelectedIndex = -1;


            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }

        protected void ddlGrantSubType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.CallJs("ApplyReDatePicker();Changeevents();");
                ddlGrantSubType3.FillCombo(DropDownName.GrantSubType3, ddlGrantSubType2.SelectedValue);
                ddlGrantSubType3.Enabled = ddlGrantSubType2.SelectedValue != "0";
                ddlGrantSubType3.Enabled = ddlGrantSubType3.Items.Count > 1;
                ddlGrantSubType3.SelectedIndex = -1;

            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }
        #endregion

        #region Button Click Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            List<Project_Dept_PI> pdlist = new List<Project_Dept_PI>();

            string mode = "";

            try
            {
                mode = HdnMode.Value.ToString().ToLower().Trim();
                Grant_Master grant_master = new Grant_Master();
                if (mode != "delete")
                {
                    string[] PiIds = HdnPi_ID.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                    for (int i = 0; i < PiIds.Length; i++)
                    {
                        pdlist.Add(new Project_Dept_PI { i_PI_ID = Convert.ToInt32(PiIds[i]) });
                    }
                    grant_master = Newtonsoft.Json.JsonConvert.DeserializeObject<Grant_Master>(HidenGrantMaster.Value.ToString());
                }


                if (mode == "update" || mode == "delete")
                {
                    grant_master.i_ID = Convert.ToInt32(HdnGrantId.Value.ToString());
                }
                grant_master.s_Created_By = LoginUserId;
                grant_master.s_CreatedBy_Name = LoginUser;

                TTSHWCFServiceClient servcl = new TTSHWCFServiceClient();
                result = servcl.Grant_Application(grant_master, pdlist.ToArray(), HdnMode.Value);
                if (Convert.ToString(result.Split('|')[0]).ToLower().Trim() == "success".ToLower() && result.Split('|')[1].CheckInt() == true)
                {
                    if (mode == "update")
                        this.MsgBox("Grant Applications Details Updated  Successfully..!!");
                    if (mode == "insert")
                        this.MsgBox("Grant Applications Details Added  Successfully..!!");
                    if (mode == "delete")
                        this.MsgBox("Grant Applications Details Deleted  Successfully..!!");
                    ShowPanel();
                    FillGridMain();
                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Back To View Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkback_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPanel();
                FillGridMain();
                HdnMode.Value = "Insert";
                HdnProjectId.Value = "0";
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }
        /// <summary>
        /// New POi Saved Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPISave_Click(object sender, EventArgs e)
        {
            try
            {
                SavePI();
                this.CallJs("ApplyReDatePicker(); ApplyToggle();Changeevents();");
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }
        //Main Cancel Buttons
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPanel();

                HdnMode.Value = "Insert";
                HdnProjectId.Value = "0";
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }

        #endregion

        #region User Defined Functions
        /// <summary>
        /// USed For Saving The Pi
        /// </summary>
        /// <returns></returns>

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


        protected void CallJavascript()
        {
            btnMorePiCancel.Attributes.Add("onclick", "javascript:return ClearCloseMorePiSection();");
            btnPICancel.Attributes.Add("onclick", "javascript:return CallNewPi();");
            // TxtDispProjId.Attributes.Add("onblur", "javascript:return GetValidatefrmDB('" + HdnError.ClientID + "','ValidateDispID' ,'" + TxtDispProjId.ClientID + "','" + HdnId.Value + "');");
            btnMorePiSave.Attributes.Add("onclick", "javascript:return SaveMorePi('" + TxtDepartment.ClientID + "', '" + TxtPIName.ClientID + "','" + txtPIEmail.ClientID + "', '" + txtPiPhoneNo.ClientID + "', '" + txtPiMCRNo.ClientID + "', '" + HdnpiId.ClientID + "','" + rptrPIDetails.ClientID + "');");
            //btnSave.Attributes.Add("onclick", "javascript:return IsValidate('" + HdnPi_ID.ClientID + "','" + chkboxlist.ClientID + "','" + TxtDispProjId.ClientID + "', '" + TxtprojTitle.ClientID + "', '" + TxtstartDate.ClientID + "', '" + ddlProjCategory.ClientID + "','" + ddlProjSubType.ClientID + "', '" + ddlProjType.ClientID + "', '" + ddlFeasibilityStatus.ClientID + "', '" + ddlCollbrationInv.ClientID + "', '" + ddlfundingReq.ClientID + "', '" + ddlParentProjName.ClientID + "','" + ddlstartbyTTSH.ClientID + "', '" + ddlChildParent.ClientID + "', '" + txtParentProjId.ClientID + "','" + HdnCoordinatorId.ClientID + "','" + HdnCoordinatorText.ClientID + "', '" + HdnMode.ClientID + "');");
            //ddlChildParent.Attributes.Add("onchange", "javascript:EnableParentControls(this, '" + txtParentProjId.ClientID + "', '" + ddlParentProjName.ClientID + "');");
            btnPISave.Attributes.Add("onclick", "javascript:return ValidateNewPi('" + TxtNewDepartment.ClientID + "', '" + txtPiFirstName.ClientID + "', '" + txtPIEmailAddress.ClientID + "','" + txtPiLastName.ClientID + "', '" + txtPIMCR_NO.ClientID + "');");

            //chkboxlist.Attributes.Add("onclick", "javascript:return SetChkFilterforALLkWithCount(this,'" + TextSearch.ClientID + "');");

        }

        protected void FillGridMain()
        {
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<GrantApplication> se = new List<GrantApplication>();
            try
            {
                string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                Project_DataOwner[] oDOList = cl.GetProjectsByDO("GRANT", UserID);
                DataOwner_Entity[] oDataOwner = cl.GetAllDataOwner("TAdmin");

                var AdminArray = (from s in oDataOwner
                                  select s.GUID).ToList();

                bool IsAdmin = AdminArray.Contains(UserID);
                se = cl.FillGrid_GrantApplication().ToList();
                if (!IsAdmin)
                {

                    List<GrantApplication> oNewGrid = new List<GrantApplication>();
                    //var v = cmlist.Select(z => z.Status).Distinct().ToList();
                    ////WHose Status Filled By Curent User
                    oNewGrid = se.Where(x => x.i_ID > 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
                    oNewGrid.ForEach(x => x.Status = "Edit");
                    ////Whose Status Available For View Only
                    oNewGrid.AddRange(se.Where(x => x.i_ID > 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID != z.s_Display_Project_ID)).ToList());
                    oNewGrid.Where(x => (string.IsNullOrEmpty(x.Status))).ToList().ForEach(x => x.Status = "View");
                    //Available For Add To The Current User
                    oNewGrid.AddRange(se.Where(x => x.i_ID == 0).Where(z => oDOList.Any(x1 => x1.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList());
                    oNewGrid.Where(x => (string.IsNullOrEmpty(x.Status))).ToList().ForEach(x => x.Status = "New");
                    oNewGrid.Where(x => x.GrantDetails_Applied == true).ToList().ForEach(x => x.Status = "View");
                    se = oNewGrid;

                }
                else
                {
                    se.Where(x => x.i_ID > 0).ToList().ForEach(x => x.Status = "Edit");
                    se.Where(x => x.i_ID == 0).ToList().ForEach(x => x.Status = "New");
                    se.Where(x => x.GrantDetails_Applied == true).ToList().ForEach(x => x.Status = "View");

                }
                rptrGrantMaster.DataSource = se;
                rptrGrantMaster.DataBind();
                CallJS("ShowNoRecords();");

            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }



        private void ClearAllRepeaterControls()
        {
            //  throw new NotImplementedException();
        }
        public void CallJS(string script, string Name = "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Name, script, true);
        }

        private void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Grant_Master grant_Master = new Grant_Master();
            List<PI_Master> piLst = new List<PI_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            GrantMasterDetails grantmasterdetails = new GrantMasterDetails();
            try
            {
                grantmasterdetails = cl.GetGrantApplicationDetails(Convert.ToInt32(HdnGrantId.Value));
                grant_Master = grantmasterdetails.grant_Master;
                txtparentprojectamount.Text = "0";
                txtRemainingAmount.Text = "0";
                HdnParentProjectDurId.Value = "0";
                HdnParentProjectAmount.Value = "0";
                HdnMaxChildDurationId.Value = "0";
                HdnMaxChildduration.Value = "";
                if (grant_Master != null)
                {
                    HdnProjectId.Value = grant_Master.i_Project_ID.ToString();
                    TxtApplicationId.Text = grant_Master.s_Application_ID.ToString();
                    ddlGrantType.SelectedValue = grant_Master.i_GrantType_ID.ToString();
                    ddlGrantType_SelectedIndexChanged(null, EventArgs.Empty);
                    ddlGrantSubType1.SelectedValue = grant_Master.i_Grant_SubType_ID.ToString();
                    ddlGrantSubType1_SelectedIndexChanged(null, EventArgs.Empty);
                    ddlGrantSubType2.SelectedValue = grant_Master.i_Grant_Sub_SubType_ID.ToString();
                    ddlGrantSubType2_SelectedIndexChanged(null, EventArgs.Empty);
                    ddlGrantSubType3.SelectedValue = grant_Master.i_Grant_Sub_Sub_SubType_ID.ToString();
                    TxtAmountRequested.Text = grant_Master.i_Amount_Requested.ToString();
                    TxtClolsingDate.Text = grant_Master.dt_Closing_Date.HasValue ? grant_Master.dt_Closing_Date.Value.ToString("dd-MMM-yyyy") : "";
                    TxtMentor.Text = grant_Master.s_Mentor.ToString();
                    ddlGrantAwardOrgan.SelectedValue = grant_Master.i_AwardOrganization.ToString();
                    ddlOutCome.SelectedValue = grant_Master.i_Outcome.ToString();
                    if (grant_Master.dt_AwardDate1.HasValue)
                    {
                        TxtAwardLetter.Text = grant_Master.dt_AwardDate1.Value.ToString("dd-MMM-yyyy");
                        hidAwardLetter.Value = TxtAwardLetter.Text;
                    }
                    if (grant_Master.dt_ApplicationDate.HasValue)
                    {
                        TxtGrantAppDate.Text = grant_Master.dt_ApplicationDate.Value.ToString("dd-MMM-yyyy");
                    }
                    TxtOldApplicationId.Text = grant_Master.s_Old_Application_ID.ToString();
                    ddlSubmissionStatus.SelectedValue = grant_Master.i_SubmissionStatus.ToString();
                    if (grant_Master.s_Duration.CheckInt())
                        ddlDuration.SelectedValue = grant_Master.s_Duration.ToString();
                    txtFTe.Text = grant_Master.i_FTE.ToString();
                    txtReviewersComments.Text = grant_Master.s_Reviewers_Comments.ToString();
                    piLst = grantmasterdetails.Pilisst.ToList();
                    pjmasterlist.Add(grantmasterdetails.project);
                    txtGrantrName.Text = grant_Master.s_Grant_Name.ToString();
                    if (grant_Master.i_AwardCountryID > 0)
                    {
                        txtCountryName.Text = grant_Master.CountryName.ToString();
                        HdnCountryId.Value = grant_Master.i_AwardCountryID.ToString();
                    }
                    if (Convert.ToDecimal(grant_Master.Total_ChildAmount.ToString()) > 0)
                        HdnParentProjectAmount.Value = grant_Master.Total_ChildAmount.ToString();

                    TxtProjTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Title"].ToString();
                    TxtAlias1.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias1"].ToString();
                    TxtAlias2.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias2"].ToString();
                    TxtShortTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Short_Title"].ToString();
                    TxtIrbNo.Text = pjmasterlist.ListToDatatable().Rows[0]["s_IRB_No"].ToString();
                    //ddlProjCategory.SelectedItem.Text = pjmasterlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                    txtProjCategory.Text = pjmasterlist[0].Project_Category_Name.ToString();
                    DispProjectId.InnerText = pjmasterlist.ListToDatatable().Rows[0]["s_Display_Project_ID"].ToString();
                    rptrPIDetails.DataSource = piLst;
                    rptrPIDetails.DataBind();
                    HdnUpdateMode.Value = "1";
                    if (pjmasterlist[0].b_Ischild == true && pjmasterlist[0].i_Parent_ProjectID > 0)
                    {
                        if (grantmasterdetails.parentProject.GrantApplied)
                        {
                            TxtParentProjectId.Text = grantmasterdetails.parentProject.s_Display_Project_ID.ToString();
                            TxtParentProjectName.Text = grantmasterdetails.parentProject.s_Project_Title.ToString();
                            txtparentprojectamount.Text = grantmasterdetails.parentProject.Total_Amount.ToString();
                            txtRemainingAmount.Text = grantmasterdetails.parentProject.Remaining_Amount.ToString();
                            txtparentprojectduration.Text = grantmasterdetails.parentProject.s_Duration.ToString();
                            HdnParentProjectDurId.Value = grantmasterdetails.parentProject.i_DurationID.ToString();
                            txtRemainingAmount.Text = Convert.ToString(Convert.ToDouble(grantmasterdetails.parentProject.Remaining_Amount) + Convert.ToDouble(grant_Master.i_Amount_Requested));

                        }
                        else
                        {
                            CallJS("MessageBox('Parent project Not Applied For The Grant.');");
                            ShowPanel();
                            FillGridMain();
                        }
                    }
                    HdnMaxChildDurationId.Value = grant_Master.i_Child_DurationID.ToString();
                    HdnMaxChildduration.Value = grant_Master.s_Child_Duration.ToString();
                    SHowParentInformation(pjmasterlist[0].b_Ischild);
                    //this.CallJs("$('.s_Old_Application_ID').attr('disabled', !($('.i_SubmissionStatus').find('option:selected').text().toLowerCase() == 'Re-Submission'.toLowerCase()));$('#' + getclientId('txtReviewersComments')).attr('disabled', !($('.i_Outcome').find('option:selected').text().toLowerCase() == 'Not-Successful'.trim().toLowerCase()));SetAwardDate();SetAwardDatePicker();");
                    this.CallJs("$('.s_Old_Application_ID').attr('disabled', !($('.i_SubmissionStatus').find('option:selected').text().toLowerCase() == 'Re-Submission'.toLowerCase()));SetAwardDate();SetAwardDatePicker();");
                    if (grantmasterdetails.grant_Master != null)
                        HdnParentProjectAmount.Value = grantmasterdetails.grant_Master.Total_ChildAmount.ToString();
                }

                if (HdnMode.Value.ToLower() == "view")
                {
                    txtReviewersComments.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }
        /// <summary>
        /// For Showing Or Hiding The Parent Project
        /// </summary>
        /// <param name="isVisible"></param>
        public void SHowParentInformation(bool isVisible)
        {
            try
            {
                parentprojectId.Visible = isVisible;
                parentprojectname.Visible = isVisible;
                parentprojectdur.Visible = isVisible;
                parentprojectAmt.Visible = isVisible;
                RemainingProjectAmt.Visible = isVisible;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void FillProjectDataForNewEntry()
        {
            TTSHWCFServiceClient servcl = new TTSHWCFServiceClient();
            List<PI_Master> piLst = new List<PI_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            GrantMasterDetails grantmasterdetails = new GrantMasterDetails();
            try
            {
                txtparentprojectamount.Text = "0";
                txtRemainingAmount.Text = "0";
                HdnParentProjectDurId.Value = "0";
                HdnParentProjectAmount.Value = "0";
                HdnMaxChildDurationId.Value = "0";
                HdnMaxChildduration.Value = "";
                grantmasterdetails = servcl.GetNewProjectDetailsForGrant(Convert.ToInt32(HdnProjectId.Value));
                piLst = grantmasterdetails.Pilisst.ToList();
                pjmasterlist.Add(grantmasterdetails.project);

                TxtProjTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Title"].ToString();
                TxtAlias1.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias1"].ToString();
                TxtAlias2.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias2"].ToString();
                TxtShortTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Short_Title"].ToString();
                TxtIrbNo.Text = pjmasterlist.ListToDatatable().Rows[0]["s_IRB_No"].ToString();
                //ddlProjCategory.Text = pjmasterlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                // ddlProjCategory.SelectedItem.Text = pjmasterlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                DispProjectId.InnerText = pjmasterlist.ListToDatatable().Rows[0]["s_Display_Project_ID"].ToString();
                rptrPIDetails.DataSource = piLst;
                rptrPIDetails.DataBind();
                HdnUpdateMode.Value = "0";
                if (pjmasterlist[0].b_Ischild == true && pjmasterlist[0].i_Parent_ProjectID > 0)
                {
                    if (grantmasterdetails.parentProject.GrantApplied)
                    {
                        TxtParentProjectId.Text = grantmasterdetails.parentProject.s_Display_Project_ID.ToString();
                        TxtParentProjectName.Text = grantmasterdetails.parentProject.s_Project_Title.ToString();
                        txtparentprojectamount.Text = grantmasterdetails.parentProject.Total_Amount.ToString();
                        txtRemainingAmount.Text = grantmasterdetails.parentProject.Remaining_Amount.ToString();
                        txtparentprojectduration.Text = grantmasterdetails.parentProject.s_Duration.ToString();
                        HdnParentProjectDurId.Value = grantmasterdetails.parentProject.i_DurationID.ToString();
                    }
                    else
                    {
                        this.MsgBox("Please Apply grant application for Parent project first");
                        ShowPanel();
                        FillGridMain();
                        this.MsgBox("Please Apply grant application for Parent project first");
                    }
                }

                SHowParentInformation(pjmasterlist[0].b_Ischild);
                txtProjCategory.Text = pjmasterlist[0].Project_Category_Name.ToString();
                // this.CallJs("$('.s_Old_Application_ID').attr('disabled', !($('.i_SubmissionStatus').find('option:selected').text().toLowerCase() == 'Re-Submission'.toLowerCase()));$('#' + getclientId('txtReviewersComments')).attr('disabled', !($('.i_Outcome').find('option:selected').text().toLowerCase() == 'Not-Successful'.trim().toLowerCase()));SetAwardDate();SetAwardDatePicker();");
                this.CallJs("$('.s_Old_Application_ID').attr('disabled', !($('.i_SubmissionStatus').find('option:selected').text().toLowerCase() == 'Re-Submission'.toLowerCase()));SetAwardDate();SetAwardDatePicker();");
                //if (grantmasterdetails.grant_Master != null )
                //    HdnParentProjectAmount.Value = grantmasterdetails.grant_Master.Total_ChildAmount.ToString();

                if (Convert.ToInt32(Common.iffBlank(HdnParentProjCount.Value, 0)) > 0 || (Convert.ToInt32(HdnChildParent.Value) > 0))
                {
                    ddlGrantType.FillCombo(DropDownName.GrantType);
                    ddlGrantType.SelectedIndex = 3;
                    ddlGrantType_SelectedIndexChanged(null, null);

                    ddlGrantSubType1.SelectedIndex = 2;
                    ddlGrantType.Enabled = false;
                    ddlGrantSubType1.Enabled = false;
                }
                
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.ToString());

            }
        }

        protected void ShowPanel(string type = "Main")
        {
            try
            {
                DivMain.Style["display"] = "block";
                DivEntry.Style["display"] = "block";
                btnSave.Visible = true;
                btnSave.Text = "Save";
                if (type.ToLower() == "entry")
                {
                    //TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
                    DivMain.Style["display"] = "none";
                    DivEntry.Style["display"] = "block";
                    hrMorePi.Visible = true;
                    PMorePi.Visible = true;
                    Pmore.Visible = true;
                    btnSave.Visible = true;
                    switch (HdnMode.Value.ToLower())
                    {

                        case "insert":
                            EnableDisableControls(divGrantDetails, true, "TxtParentProjectId", "TxtParentProjectName", "txtRemainingAmount", "txtparentprojectduration", "txtparentprojectamount");
                            btnSave.Text = "Save";
                            //  ddlOutCome.SelectedValue = "3";
                            ddlSubmissionStatus.SelectedValue = "1";
                            ddlGrantSubType1.Enabled = false;
                            ddlGrantSubType2.Enabled = false;
                            ddlGrantSubType3.Enabled = false;

                            break;
                        case "update":
                            EnableDisableControls(divGrantDetails, true, "TxtParentProjectId", "TxtParentProjectName", "txtRemainingAmount", "txtparentprojectduration", "txtparentprojectamount");
                            btnSave.Text = "Update";
                            break;
                        case "delete":
                            btnSave.Text = "Delete";
                            break;
                        case "view":
                            //Common.EnableAllandClearControl(Master, false, false);
                            EnableDisableControls(divGrantDetails, false, "TxtParentProjectId", "TxtParentProjectName", "txtRemainingAmount", "txtparentprojectduration", "txtparentprojectamount");
                            hrMorePi.Visible = false; PMorePi.Visible = false;
                            Pmore.Visible = false;
                            btnSave.Visible = false;
                            break;
                    }
                }
                else
                {
                    HdnMode.Value = "Insert";
                    HdnProjectId.Value = "0";
                    DivEntry.Style["display"] = "none";
                    // divAction.Style["display"] = "none";
                    // divGrantDetails.Style["display"] = "none";
                    DivMain.Style["display"] = "block";

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ClearHDN()
        {
            HdnProjectId.Value = "";
            HdnMode.Value = "";
            HdnGrantId.Value = "";
            HdnPi_ID.Value = "";
            HdnParentProjCount.Value = "0";
        }

        protected void BindCombo()
        {
            try
            {
                ddlGrantType.FillCombo(DropDownName.GrantType);
                ddlGrantAwardOrgan.FillCombo(DropDownName.GrantAwardingOrganization);
                ddlOutCome.FillCombo(DropDownName.GrantOutCome);
                ddlSubmissionStatus.FillCombo(DropDownName.GrantSubmissionStatus);
                ddlDuration.FillCombo(DropDownName.GrantDuration);
                // ddlCountryName.FillCombo(DropDownName.Country);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Enable Disabled All The Controls Onto The Page
        /// </summary>
        /// <param name="ctl">COntrol Name</param>
        /// <param name="enabled">To Make Enabled or Diabled</param>
        protected void EnableDisableControls(Control ctl, bool enabled, params string[] excludedcontrold)
        {
            try
            {
                foreach (Control child in ctl.Controls)
                {
                    if (child.HasControls())
                    {
                        EnableDisableControls(child, enabled, excludedcontrold);
                    }
                    if (!excludedcontrold.Contains(child.ID))
                    {
                        var textbox = child as TextBox;
                        if (textbox != null)
                            textbox.Enabled = enabled;

                        var dropDownList = child as DropDownList;
                        if (dropDownList != null)
                            dropDownList.Enabled = enabled;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected void rptrGrantMaster_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {


                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("td");
                    HtmlGenericControl parentProjectCount = (HtmlGenericControl)e.Item.FindControl("parentProjectCount");
                    HtmlGenericControl ChildParentProject = (HtmlGenericControl)e.Item.FindControl("ChildParentProject");
                    HtmlGenericControl ParentProject = (HtmlGenericControl)e.Item.FindControl("ParentProject");
                    // var s_Uploaded_File = ((TTSHWeb.TTSHWCFReference.GrantApplication)(e.Item.DataItem)).IsChildorParent;
                    object item = DataBinder.Eval(e.Item.DataItem, "IsChildorParent");
                    if (item.ToString().ToLower() == "parent")
                    {
                        string s = "Total Child Project : " + parentProjectCount.InnerText + " " + ParentProject.InnerText;
                        td.Attributes.Add("title", s);
                    }
                    else
                    {
                        if (ChildParentProject.InnerText.ToString() != " ")
                        {
                            td.Attributes.Add("title", ChildParentProject.InnerText.ToString());
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}