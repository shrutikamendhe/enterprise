using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;
using System.Web;
namespace TTSHWeb
{
    public partial class frmRegulatory : System.Web.UI.Page
    {
        #region " Page Event "

        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.REGULATORY;
            SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
            SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;

            if (!IsPostBack)
            {



                ClearHDN();
                TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                t.Text = "";
                FillMainGrid();
                ShowPanel();
                BindCombo();
            }
        }
        #endregion

        #region " Repeater Event "
        protected void RptsRegulatoryGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "")
                {
                    ClearHDN();
                    HdnProjectId.Value = e.CommandArgument.ToString();
                    if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview" | e.CommandName.ToLower() == "cmdadd")
                    {
                        HdnMode.Value = e.CommandName.ToString().ConverMode();

                        ShowPanel("entry");
                        bool enabled = (e.CommandName.ToString().ConverMode().ToLower() == "delete" || e.CommandName.ToString().ConverMode().ToLower() == "view") ? false : true;

                        ClearAllRepeaterControls();
                        if (HdnMode.Value.ToLower() == "insert")
                        {
                            FillProjectDataForNewEntry();
                        }
                        else
                        {
                            HdnRegId.Value = e.CommandArgument.ToString();
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

        protected void RptAmendmentDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableRow trAmend = (HtmlTableRow)e.Item.FindControl("trAmend");
                    HtmlTableCell tc = new HtmlTableCell();
                    LinkButton lbDownload = new LinkButton();
                    var s_Uploaded_File = ((TTSHWeb.TTSHWCFReference.Regulatory_Ammendments_Details)(e.Item.DataItem)).s_Uploaded_File;

                    lbDownload.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + s_Uploaded_File + "')";
                    lbDownload.Text = s_Uploaded_File.GetFileName();

                    tc.Controls.Add(lbDownload);
                    trAmend.Controls.RemoveAt(1);
                    trAmend.Controls.AddAt(1, tc);
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }

        protected void RptrStatusMontly_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableRow trAmend = (HtmlTableRow)e.Item.FindControl("trStatusM");
                    HtmlTableCell tc = new HtmlTableCell();
                    LinkButton lbDownload = new LinkButton();
                    var s_Uploaded_File = ((TTSHWeb.TTSHWCFReference.Regulatory_Submission_Status)(e.Item.DataItem)).s_Uploaded_File;

                    lbDownload.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + s_Uploaded_File + "')";
                    lbDownload.Text = s_Uploaded_File.GetFileName();

                    tc.Controls.Add(lbDownload);
                    trAmend.Controls.RemoveAt(2);
                    trAmend.Controls.AddAt(2, tc);
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }

        }
        #endregion

        #region " Button Event "
        protected void lnkback_Click(object sender, EventArgs e)
        {
            ShowPanel();
            //  FillMainGrid();
            HdnMode.Value = "Insert";
            HdnProjectId.Value = "0";
        }
        void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            FillMainGrid();
        }

        void SearchBox_ButtonSearchClick(object sender, EventArgs e)
        {
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            if (string.IsNullOrEmpty(SearchBox.ErrorString))
            {
                Search[] lst = SearchBox.SearchOutput;

                try
                {

                    
                    client.Open();
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("REGULATORY", UserID);
                    DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);


                    List<Search> oNewGrid = new List<Search>();
                    List<Search> oOldSearch = new List<Search>();

                    if (IsAdmin == false)
                    {
                        if (lst != null && lst.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                        {
                            oOldSearch = lst.ToList();
                            oNewGrid = oOldSearch.Where(z => z.iRecordExists == 0).Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList();
                            //oNewGrid.ForEach(i => i.Status = "New");
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
                                        if (item.s_DisplayProject_ID.ToUpper().Trim() == element.s_Display_Project_ID.ToUpper().Trim())
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

                            RptsRegulatoryGrid.DataSource = oOldSearch; /*use the object according to your need*/
                            RptsRegulatoryGrid.DataBind();
                        }
                        else
                        {

                            if (oDOList != null && oDOList.Count() > 0)
                            {
                                foreach (var element in lst)
                                {
                                    element.Status = "View";
                                }
                            }
                            else
                            {
                                foreach (var element in lst)
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
                                            if (item.s_DisplayProject_ID.ToUpper().Trim() == element.s_Display_Project_ID.ToUpper().Trim())
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
                            }
                            RptsRegulatoryGrid.DataSource = lst; /*use the object according to your need*/
                            RptsRegulatoryGrid.DataBind();
                        }
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
                        oOldSearch = oOldSearch.OrderByDescending(z => z.i_Project_ID).ToList();

                        RptsRegulatoryGrid.DataSource = oOldSearch; /*use the object according to your need*/
                        RptsRegulatoryGrid.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                client.Close();

                //RptsRegulatoryGrid.DataSource = lst;
                //RptsRegulatoryGrid.DataBind();
            }
            else
            {
                // this.MsgBox(SearchBox.ErrorString);
                RptsRegulatoryGrid.DataSource = null;
                RptsRegulatoryGrid.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowPanel();
            FillMainGrid();
            HdnMode.Value = "Insert";
            HdnProjectId.Value = "0";
        }
        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("Regulatory_Delete", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnRegId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Regulatory Detail Delete  Successfully..!!");
                    ShowPanel();
                    FillMainGrid();
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }
        #endregion

        #region " Methods "
        protected void BindCombo()
        {
            ddlCTCStatus.FillCombo(DropDownName.CTCstatus);
            ddlLeadSponsor.FillCombo(DropDownName.LeadSponsor);
            ddlStatusRptmonth.FillCombo(DropDownName.RegularoryStatusReportFor);
            ddlIp.FillCombo(DropDownName.RegulatoryIPStorage);
        }
        protected void FillMainGrid()
        {
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Regulatory_Master> rm = new List<Regulatory_Master>();
            try
            {
                rm = cl.FillGridRegulatoryMain().ToList();
                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                try
                {
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("REGULATORY", UserID);
                    DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);

                    if (IsAdmin == false)
                    {
                        List<Regulatory_Master> oNewGrid = new List<Regulatory_Master>();
                        if (rm != null && rm.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                        {
                            oNewGrid = rm.Where(z => z.CTCCount == 0).Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList();
                            oNewGrid.ForEach(z => z.Status = "New");
                            rm.RemoveAll(z => z.CTCCount == 0);
                            rm.AddRange(oNewGrid);
                            
                            rm.Where(z => z.CTCCount != 0).Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "Edit");
                            rm.Where(z => z.CTCCount != 0).Where(z => !oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "View");
                            rm = rm.OrderByDescending(z => z.i_Project_ID).ToList();
                        }
                        else if (rm != null && rm.Count() > 0)
                        {
                            rm.ForEach(x => x.Status = "View");
                            rm.OrderByDescending(z => z.i_Project_ID);
                        }

                    }
                    else
                    {
                        rm.Where(z => z.CTCCount == 0).ToList().ForEach(i => i.Status = "New");
                        rm.Where(z => z.CTCCount != 0).ToList().ForEach(i => i.Status = "Edit");
                        rm = rm.OrderByDescending(z => z.i_Project_ID).ToList();
                    }

                }
                catch (Exception ex)
                {

                }

                RptsRegulatoryGrid.DataSource = rm;
                RptsRegulatoryGrid.DataBind();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString());
            }
        }
        protected void ShowPanel(string type = "Main")
        {
            DivMain.Style["display"] = "block";
            DivEntry.Style["display"] = "block";
            btnSave.Visible = true;
            btnSave.Text = "Save";
            if (type.ToLower() == "entry")
            {
                TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
                DivMain.Style["display"] = "none";
                TxtSubject.Text = cl.GetValidate("RegulatorySubRecruitedBy", HdnProjectId.Value, "", "", "");
                switch (HdnMode.Value.ToLower())
                {

                    case "insert":
                        btnSave.Text = "Save";
                        break;
                    case "update":
                        btnSave.Text = "Update";
                        break;
                    case "delete":
                        btnSave.Text = "Delete";
                        break;
                    case "view":
                        btnSave.Visible = false;
                        break;
                }
            }
            else
            {
                DivEntry.Style["display"] = "none";
            }
        }
        protected void FillProjectDataForNewEntry()
        {
            TTSHWCFServiceClient servcl = new TTSHWCFServiceClient();
            List<PI_Master> piLst = new List<PI_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            RegulatoryNewProjectEntry pdc = new RegulatoryNewProjectEntry();
            try
            {
                pdc = servcl.GetNewProjectEntry(Convert.ToInt32(HdnProjectId.Value));
                piLst = pdc.Pilisst.ToList();
                pjmasterlist = pdc.pmlist.ToList();
                TxtProjTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Title"].ToString();
                TxtAlias1.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias1"].ToString();
                TxtAlias2.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias2"].ToString();
                TxtShortTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Short_Title"].ToString();
                TxtIrbNo.Text = pjmasterlist.ListToDatatable().Rows[0]["s_IRB_No"].ToString();
                TxtprojCategory.Text = pjmasterlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                DispProjectId.InnerText = pjmasterlist.ListToDatatable().Rows[0]["s_Display_Project_ID"].ToString();
                rptrPIDetails.DataSource = piLst; rptrPIDetails.DataBind();
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.ToString());

            }

        }
        protected bool Save()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Regulatory_Master _Regulatory_Master = new Regulatory_Master();
            List<Regulatory_StudyTeam> lstRegulatory_StudyTeam = new List<Regulatory_StudyTeam>();
            List<Regulatory_ICF_Details> lstRegulatory_ICF_Details = new List<Regulatory_ICF_Details>();
            List<Regulatory_Submission_Status> lstRegulatory_Submission_Status = new List<Regulatory_Submission_Status>();
            List<Regulatory_Ammendments_Details> lstRegulatory_Ammendments_Details = new List<Regulatory_Ammendments_Details>();
            List<RegulatoryIPManagement> lstRegulatoryIPManagement = new List<RegulatoryIPManagement>();
            string result = "";
            try
            {
                _Regulatory_Master.i_ID = Convert.ToInt32(HdnRegId.Value);
                _Regulatory_Master.i_Project_ID = Convert.ToInt32(HdnProjectId.Value);
                _Regulatory_Master.i_Sponsor_ID = Convert.ToInt32(ddlLeadSponsor.SelectedValue);
                _Regulatory_Master.s_Other_Sponsor = Convert.ToString(TxtOtherLeadSpnsor.Text);
                _Regulatory_Master.b_Prism_AppStatus = (ddlPrismStatus.SelectedValue == "1") ? true : false;
                _Regulatory_Master.s_Prism_AppNo = Convert.ToString(TxtPrimsAppNo.Text);
                _Regulatory_Master.dt_Prism_AppDate = (TxtPrismSubmissionDate.Text.Trim() != "") ? TxtPrismSubmissionDate.Text : null;
                _Regulatory_Master.i_CTC_status_ID = Convert.ToInt32(ddlCTCStatus.SelectedValue);

                // if (ddlCTCStatus.SelectedItem.Text.ToLower() == "active")
                // {
                _Regulatory_Master.s_CTC_Document = (fldCTCdocument.HasFile) ?
                                              String.Join(",", Common.UpLoadNew(fldCTCdocument, Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                             "";

                _Regulatory_Master.s_CTC_EmailDocument = (fldCTCEmailApprDoc.HasFile) ?
                                                        string.Join(",", Common.UpLoadNew(fldCTCEmailApprDoc, Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                                                            "";
                if (HdnMode.Value.ToLower() == "update")
                {
                    if (HdnCTCDocPath.Value.Length > 0)
                    {
                        _Regulatory_Master.s_CTC_Document = (_Regulatory_Master.s_CTC_Document != "") ? _Regulatory_Master.s_CTC_Document : HdnCTCDocPath.Value;
                    }
                    if (HdnCTCEmailApprDoc.Value.Length > 0)
                    {
                        _Regulatory_Master.s_CTC_EmailDocument = (_Regulatory_Master.s_CTC_EmailDocument != "") ? _Regulatory_Master.s_CTC_EmailDocument : HdnCTCEmailApprDoc.Value;
                    }
                }

                _Regulatory_Master.dt_CTC_ApprDate = (TxtCTCAppDate.Text.Trim() != "") ? TxtCTCAppDate.Text : "";
                _Regulatory_Master.s_CTC_No = Convert.ToString(TxtCtcNo.Text);
                _Regulatory_Master.dt_CTC_ExpiryDate = (TxtCTCExpiryDate.Text.Trim() != "") ? TxtCTCExpiryDate.Text : null;
                // }


                //  if (ddlCTCStatus.SelectedItem.Text.ToLower() == "extended")
                //   {
                _Regulatory_Master.dt_NewExt_Appr_Date = (TxtCTCExtAppDate.Text.Trim() != "") ? TxtCTCExtAppDate.Text : null;
                _Regulatory_Master.dt_NewExpiry_Date = (TxtNewCTCExpiryDate.Text.Trim() != "") ? TxtNewCTCExpiryDate.Text : null;
                _Regulatory_Master.s_NewCTCEmailApprDoc = (fldNewCTCEmailApprDoc.HasFile) ?
                                                    String.Join(",", Common.UpLoadNew(fldNewCTCEmailApprDoc, Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                                    "";


                _Regulatory_Master.s_ExtCTCEmailApprDoc = (fldExtCTCEmailApprDoc.HasFile) ?
                                                    String.Join(",", Common.UpLoadNew(fldExtCTCEmailApprDoc, Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                                    "";

                if (HdnMode.Value.ToLower() == "update")
                {
                    _Regulatory_Master.s_NewCTCEmailApprDoc = _Regulatory_Master.s_NewCTCEmailApprDoc != "" ? _Regulatory_Master.s_NewCTCEmailApprDoc : HdnNCTCEmailApprDoc.Value;
                    _Regulatory_Master.s_ExtCTCEmailApprDoc = _Regulatory_Master.s_ExtCTCEmailApprDoc != "" ? _Regulatory_Master.s_ExtCTCEmailApprDoc : HdnExtCTCEmailApprDoc.Value;
                }

                //  }
                _Regulatory_Master.s_Protocol_No = Convert.ToString(TxtProtocolNo.Text);
                _Regulatory_Master.s_Protocol_Ver_No = Convert.ToString(TxtprotocolVersionNo.Text);
                _Regulatory_Master.dt_Protocol_Date = (TxtProtocolDate.Text.Trim() != "") ? TxtProtocolDate.Text : null;
                _Regulatory_Master.s_RecruitedBy_TTSH = Convert.ToString(TxtSubject.Text);
                _Regulatory_Master.s_Remarks = Convert.ToString(TxtRemark.Text);

                //***********************For Six Months Update**************************************************************
                if (ddlCTCStatus.SelectedItem.Text.ToLower() == "active" || ddlCTCStatus.SelectedItem.Text.ToLower() == "extended")
                {
                    if (TxtPendingScreenOutcome.Text.Trim() != "") _Regulatory_Master.i_Pending_Screen_Outcome = Convert.ToInt32(TxtPendingScreenOutcome.Text);
                    if (TxtScrenFailure.Text.Trim() != "") _Regulatory_Master.i_Screen_Failure = Convert.ToInt32(TxtScrenFailure.Text);
                    if (TxtScreened.Text.Trim() != "") _Regulatory_Master.i_Screened = Convert.ToInt32(TxtScreened.Text);
                    if (TxtRandEnrolled.Text.Trim() != "") _Regulatory_Master.i_Randomized = Convert.ToInt32(TxtRandEnrolled.Text);
                    if (Txttermination.Text.Trim() != "") _Regulatory_Master.i_Withdrawn = Convert.ToInt32(Txttermination.Text);
                    _Regulatory_Master.s_Withdrawn_Reason = Convert.ToString(TxtReasonWithdrawn.Text);
                    if (TxtoutPatient.Text.Trim() != "") _Regulatory_Master.i_Ongoing_Patient = Convert.ToInt32(TxtoutPatient.Text);
                    if (TxtCompletedNo.Text.Trim() != "") _Regulatory_Master.i_Completed_No = Convert.ToInt32(TxtCompletedNo.Text);
                    if (TxtSaeNo.Text.Trim() != "") _Regulatory_Master.i_SAE_No = Convert.ToInt32(TxtSaeNo.Text);
                    _Regulatory_Master.s_SAE_Reason = Convert.ToString(TxtReasonForSAE.Text);
                    _Regulatory_Master.b_Internal_Audit = (ddlInternalAudit.SelectedValue == "1") ? true : false;
                    _Regulatory_Master.DT_LASTUPDATED_DATE = (TxtLastUpDate.Text != "") ? TxtLastUpDate.Text : null;
                    _Regulatory_Master.RegSIxMId = Convert.ToInt32(Common.iffBlank(RegSixMId.Value, 0));

                    //****************************** Regulatory Submission Status ****************************************
                    if (HdnStatusReportSubmissionFileDetails.Value.Trim() != "")
                    {
                        string[] SubStatusVal = HdnStatusReportSubmissionFileDetails.Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                        for (int i = 0; i < SubStatusVal.Length; i++)
                        {
                            string filePath = "";
                            if (SubStatusVal[i].Split(',')[0].Contains("~"))
                            {
                                filePath = SubStatusVal[i].Split(',')[0];
                            }
                            else
                            {
                                filePath = (SubStatusVal[i].Split(',')[0].Trim() != "") ?
                                                    String.Join(",", Common.GetFilesFromStringPath(SubStatusVal[i].Split(',')[0], Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                                    "";
                            }

                            lstRegulatory_Submission_Status.Add(new Regulatory_Submission_Status
                            {

                                i_Interval_ID = Convert.ToInt32(Common.iffBlank(SubStatusVal[i].Split(',')[1], 0)),
                                i_Regulatory_ID = Convert.ToInt32(HdnRegId.Value),
                                s_File_Title = Convert.ToString(Common.iffBlank(SubStatusVal[i].Split(',')[2], "")),
                                s_Uploaded_File = filePath
                            });
                        }
                    }

                    // ******************************** END*****************************************************************
                }
                //************************END*******************************************************************************

                //******************************Regulatory Study Team******************************************************
                if (HdnStudyTeamMembersDetails.Value.Trim() != "")
                {
                    string[] StudyVal = HdnStudyTeamMembersDetails.Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                    for (int i = 0; i < StudyVal.Length; i++)
                    {
                        lstRegulatory_StudyTeam.Add(new Regulatory_StudyTeam
                        {
                            s_First_Name = Convert.ToString(Common.iffBlank(StudyVal[i].Split(',')[0], "")),
                            s_Last_Name = Convert.ToString(Common.iffBlank(StudyVal[i].Split(',')[1], "")),
                            s_Email_ID = Convert.ToString(Common.iffBlank(StudyVal[i].Split(',')[2], "")),
                            i_Regulatory_ID = Convert.ToInt32(HdnRegId.Value)
                        });
                    }
                }

                //************************************END******************************************************************


                //*************************Regulatory ICF Details************************************************************
                if (HdnPISDetails.Value.Trim() != "")
                {
                    string[] ICFVal = HdnPISDetails.Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                    for (int i = 0; i < ICFVal.Length; i++)
                    {
                        lstRegulatory_ICF_Details.Add(new Regulatory_ICF_Details
                        {
                            s_Version_No = Convert.ToString(Common.iffBlank(ICFVal[i].Split(',')[0], "")),
                            dt_ICF_Date = Convert.ToString(Common.iffBlank(ICFVal[i].Split(',')[1], null)),
                            i_Regulatory_ID = Convert.ToInt32(HdnRegId.Value)
                        });
                    }
                }

                //****************************** END *************************************************************************



                //******************************Regulatory Amendment Details**********************************************
                if (HdnAmendmentsDetails.Value.Trim() != "")
                {
                    string[] AmendVal = HdnAmendmentsDetails.Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                    for (int i = 0; i < AmendVal.Length; i++)
                    {
                        string Fapth = "";
                        if (AmendVal[i].Split(',')[0].Contains("~"))
                        {
                            Fapth = AmendVal[i].Split(',')[0];
                        }
                        else
                        {
                            Fapth = (AmendVal[i].Split(',')[0].Trim() != "") ?
                                                     String.Join(",", Common.GetFilesFromStringPath(AmendVal[i].Split(',')[0], Common.FolderLocation.Regulatory).Select(r => r.ToString())) :
                                                     "";
                        }
                        lstRegulatory_Ammendments_Details.Add(new Regulatory_Ammendments_Details
                        {
                            i_Regulatory_ID = Convert.ToInt32(HdnRegId.Value),
                            dt_Submission_Date = Convert.ToString(Common.iffBlank(AmendVal[i].Split(',')[1], null)),
                            s_Uploaded_File = Fapth
                        });
                    }
                }

                //******************************** END *******************************************************************

                //************************** Regulatory IP Management*************************************************************
                if (HdnOtherDetails.Value.Trim() != "")
                {
                    string[] IPVal = HdnOtherDetails.Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                    if (IPVal.Length > 0)
                    {
                        if (!IPVal[0].Contains("undefined"))
                        {
                            for (int i = 0; i < IPVal.Length; i++)
                            {
                                lstRegulatoryIPManagement.Add(new RegulatoryIPManagement
                                {
                                    i_Regulatory_ID = Convert.ToInt32(HdnRegId.Value),
                                    s_Investigational_Product = Convert.ToString(Common.iffBlank(IPVal[i].Split(',')[1], "")),
                                    s_IPManagement = Convert.ToInt32(Common.iffBlank(IPVal[i].Split(',')[0], "")),
                                    s_StorageLocation = Convert.ToString(Common.iffBlank(IPVal[i].Split(',')[3], "")),
                                });
                            }
                        }
                    }
                }
                //--------UID and UName----
                _Regulatory_Master.UName = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString();
                _Regulatory_Master.UID = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString();
                //----------- END ------------

                //*************************** END ***********************************************************************************

                result = cl.Regulatory_Master_DML(_Regulatory_Master,
                                                lstRegulatory_StudyTeam.ToArray(),
                                                lstRegulatory_ICF_Details.ToArray(),
                                                lstRegulatory_Submission_Status.ToArray(),
                                                lstRegulatory_Ammendments_Details.ToArray(),
                                                lstRegulatoryIPManagement.ToArray(), HdnMode.Value);
                if (Convert.ToString(result.Split('|')[0]).ToLower() == "success" && result.Split('|')[1].CheckInt() == true)
                {
                    switch (HdnMode.Value.ToLower())
                    {
                        case "update": this.MsgBox("Regulatory Detail Update  Successfully..!!"); break;
                        case "delete": this.MsgBox("Regulatory Detail Delete  Successfully..!!"); break;
                        case "insert": this.MsgBox("Regulatory Detail Save  Successfully..!!"); break;
                    }
                    ShowPanel();
                    FillMainGrid();
                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
                return false;
            }
            return true;
        }
        protected void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Regulatory_Master _Regulatory_Master = new Regulatory_Master();
            List<Regulatory_StudyTeam> lstRegulatory_StudyTeam = new List<Regulatory_StudyTeam>();
            List<Regulatory_ICF_Details> lstRegulatory_ICF_Details = new List<Regulatory_ICF_Details>();
            List<Regulatory_Submission_Status> lstRegulatory_Submission_Status = new List<Regulatory_Submission_Status>();
            List<Regulatory_Ammendments_Details> lstRegulatory_Ammendments_Details = new List<Regulatory_Ammendments_Details>();
            List<RegulatoryIPManagement> lstRegulatoryIPManagement = new List<RegulatoryIPManagement>();
            List<RegulatorySixMonthUpdate> lstRegulatorySixMonthUpdate = new List<RegulatorySixMonthUpdate>();
            List<PI_Master> piList = new List<PI_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            try
            {
                _Regulatory_Master = cl.GetRegulatory_MasterDetailsByID(Convert.ToInt32(HdnRegId.Value));
                if (_Regulatory_Master != null)
                {
                    lstRegulatory_StudyTeam = (_Regulatory_Master.RegStudyTeamList != null) ? _Regulatory_Master.RegStudyTeamList.ToList() : null;
                    lstRegulatory_ICF_Details = (_Regulatory_Master.RegICFDetails != null) ? _Regulatory_Master.RegICFDetails.ToList() : null;
                    lstRegulatory_Submission_Status = (_Regulatory_Master.RegSubStatusList != null) ? _Regulatory_Master.RegSubStatusList.ToList() : null;
                    lstRegulatory_Ammendments_Details = (_Regulatory_Master.RegAmendDetails != null) ? _Regulatory_Master.RegAmendDetails.ToList() : null;
                    lstRegulatoryIPManagement = (_Regulatory_Master.RegIPList != null) ? _Regulatory_Master.RegIPList.ToList() : null;
                    lstRegulatorySixMonthUpdate = (_Regulatory_Master.RegSixMUpdateList != null) ? _Regulatory_Master.RegSixMUpdateList.ToList() : null;
                    pjmasterlist = (_Regulatory_Master.pmlist != null) ? _Regulatory_Master.pmlist.ToList() : null;
                    piList = (_Regulatory_Master.Pilisst != null) ? _Regulatory_Master.Pilisst.ToList() : null;

                    BindCombo();
                    if (pjmasterlist != null)
                    {
                        DataTable dt = new DataTable();
                        dt = pjmasterlist.ListToDatatable();
                        TxtProjTitle.Text = Convert.ToString(dt.Rows[0]["s_Project_Title"]);
                        TxtprojCategory.Text = Convert.ToString(dt.Rows[0]["Project_Category_Name"]);
                        DispProjectId.InnerText = Convert.ToString(dt.Rows[0]["s_Display_Project_ID"]);
                        HdnProjectId.Value = Convert.ToString(dt.Rows[0]["i_ID"]);
                        TxtIrbNo.Text = Convert.ToString(dt.Rows[0]["s_IRB_No"]);
                        TxtAlias1.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias1"]);
                        TxtAlias2.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias2"]);
                        TxtShortTitle.Text = Convert.ToString(dt.Rows[0]["s_Short_Title"]);
                    }
                    if (piList != null)
                    {
                        rptrPIDetails.DataSource = piList;
                        rptrPIDetails.DataBind();
                    }
                    ddlLeadSponsor.SelectedIndex = ddlLeadSponsor.Items.IndexOf(ddlLeadSponsor.Items.FindByValue(Convert.ToString(_Regulatory_Master.i_Sponsor_ID)));
                    TxtOtherLeadSpnsor.Text = _Regulatory_Master.s_Other_Sponsor; HdnOtherSponsor.Value = TxtOtherLeadSpnsor.Text;
                    ddlPrismStatus.SelectedIndex = _Regulatory_Master.b_Prism_AppStatus == true ? 1 : 2;
                    TxtPrismSubmissionDate.Text = _Regulatory_Master.dt_Prism_AppDate;
                    TxtPrimsAppNo.Text = _Regulatory_Master.s_Prism_AppNo;
                    ddlCTCStatus.SelectedIndex = ddlCTCStatus.Items.IndexOf(ddlCTCStatus.Items.FindByValue(Convert.ToString(_Regulatory_Master.i_CTC_status_ID)));
                    if (ddlCTCStatus.SelectedItem.Text.ToLower() == "active") { HdnCTCStatus.Value = "1"; }
                    if (ddlCTCStatus.SelectedItem.Text.ToLower() == "extended" || ddlCTCStatus.SelectedItem.Text.ToLower() == "completed"
                        || ddlCTCStatus.SelectedItem.Text.ToLower() == "terminated"
                        ) { HdnExtendedStatus.Value = "1"; HdnCTCStatus.Value = "1"; }
                    //  if (ddlCTCStatus.SelectedItem.Text.ToLower() == "active")
                    // {
                    //******************************* CTC Document***********************************
                    HdnCTCDocPath.Value = _Regulatory_Master.s_CTC_Document;
                    LnkDwnldCTCDoc.Text = _Regulatory_Master.s_CTC_Document.GetFileName();
                    LnkDwnldCTCDoc.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + _Regulatory_Master.s_CTC_Document + "')";
                    //*******************************  END  ********************************************
                    TxtCTCAppDate.Text = _Regulatory_Master.dt_CTC_ApprDate;
                    //******************************* CTC Email Appr Document***********************************
                    HdnCTCEmailApprDoc.Value = _Regulatory_Master.s_CTC_EmailDocument;
                    LnkDnwldCTCEmailApprDoc.Text = _Regulatory_Master.s_CTC_EmailDocument.GetFileName();
                    LnkDnwldCTCEmailApprDoc.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + _Regulatory_Master.s_CTC_EmailDocument + "')";


                    //*******************************  END  ********************************************
                    TxtCtcNo.Text = _Regulatory_Master.s_CTC_No;
                    TxtCTCExpiryDate.Text = _Regulatory_Master.dt_CTC_ExpiryDate;
                    //*******************************CTC Email Approval Doc When CTC Status is Extended***********************************
                    //   }

                    // if (ddlCTCStatus.SelectedItem.Text.ToLower() == "extended")
                    //  {
                    TxtCTCExtAppDate.Text = _Regulatory_Master.dt_NewExt_Appr_Date;
                    TxtNewCTCExpiryDate.Text = _Regulatory_Master.dt_NewExpiry_Date;

                    HdnNCTCEmailApprDoc.Value = _Regulatory_Master.s_NewCTCEmailApprDoc;
                    LnkDnwldNCTCEmailApprDoc.Text = _Regulatory_Master.s_NewCTCEmailApprDoc.GetFileName();
                    LnkDnwldNCTCEmailApprDoc.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + _Regulatory_Master.s_NewCTCEmailApprDoc + "')";

                    HdnExtCTCEmailApprDoc.Value = _Regulatory_Master.s_ExtCTCEmailApprDoc;
                    LnkDwnldExtCTCEmailApprDoc.Text = _Regulatory_Master.s_ExtCTCEmailApprDoc.GetFileName();
                    LnkDwnldExtCTCEmailApprDoc.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + _Regulatory_Master.s_ExtCTCEmailApprDoc + "')";
                    // }
                    //*******************************  END  ********************************************


                    //************************** Additional Study Team Member ***********************
                    RptrStudyMember.DataSource = lstRegulatory_StudyTeam;
                    RptrStudyMember.DataBind();
                    //******************************   END ******************************************


                    //*************************** Protocol Details *********************************
                    TxtProtocolNo.Text = _Regulatory_Master.s_Protocol_No;
                    TxtprotocolVersionNo.Text = _Regulatory_Master.s_Protocol_Ver_No;
                    TxtProtocolDate.Text = _Regulatory_Master.dt_Protocol_Date;
                    //***************************   END   ******************************************

                    //***************************** PIS ******************************************
                    rptPIS.DataSource = lstRegulatory_ICF_Details;
                    rptPIS.DataBind();
                    //**************************** END *************************************************

                    // ******************************** SIX Months Update **********************************
                    if (ddlCTCStatus.SelectedItem.Text.ToLower() == "active" || ddlCTCStatus.SelectedItem.Text.ToLower() == "extended" || ddlCTCStatus.SelectedItem.Text.ToLower() == "completed" || ddlCTCStatus.SelectedItem.Text.ToLower() == "terminated")
                    {
                        DataTable dt = new DataTable();
                        DataView dv = lstRegulatorySixMonthUpdate.ListToDatatable().DefaultView;

                      //  dv.Sort = "SortDate desc";
                        dt = dv.ToTable();

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                TxtPendingScreenOutcome.Text = Convert.ToString(dt.Rows[0]["i_Pending_Screen_Outcome"]);
                                TxtScreened.Text = Convert.ToString(dt.Rows[0]["i_Screened"]);
                                Txttermination.Text = Convert.ToString(dt.Rows[0]["i_Withdrawn"]);
                                TxtoutPatient.Text = Convert.ToString(dt.Rows[0]["i_Ongoing_Patient"]);
                                TxtSaeNo.Text = Convert.ToString(dt.Rows[0]["i_SAE_No"]);
                                ddlInternalAudit.SelectedIndex = Convert.ToBoolean(dt.Rows[0]["b_Internal_Audit"]) == true ? 1 : 2;
                                TxtScrenFailure.Text = Convert.ToString(dt.Rows[0]["i_Screen_Failure"]);
                                TxtRandEnrolled.Text = Convert.ToString(dt.Rows[0]["i_Randomized"]);
                                TxtReasonWithdrawn.Text = Convert.ToString(dt.Rows[0]["s_Withdrawn_Reason"]);
                                TxtCompletedNo.Text = Convert.ToString(dt.Rows[0]["i_Completed_No"]);
                                TxtReasonForSAE.Text = Convert.ToString(dt.Rows[0]["s_SAE_Reason"]);
                                TxtLastUpDate.Text = Convert.ToString(dt.Rows[0]["dt_LastUpdated_date"]);
                                RegSixMId.Value = Convert.ToString(dt.Rows[0]["RegSIxMId"]);
                                hdnLastUpDate.Value = Convert.ToString(dt.Rows[0]["dt_LastUpdated_date"]);
                                HdnSel.Value = Convert.ToString(dt.Rows[0]["NoOfMonths"]);
                                //********************** Status Submission Report**********************
                                RptrStatusMontly.DataSource = lstRegulatory_Submission_Status;
                                RptrStatusMontly.DataBind();
                                //************************   END  *************************************
                                dv = dt.DefaultView;
                                dv.Sort = "rnum asc";
                                dt = dv.ToTable();
                                dt.Columns.Remove("SortDate"); dt.Columns.Remove("rnum");
                                ClearRows(dt);

                                SHowTabs(dt);



                            }
                        }
                    }
                    //********************************** END ***********************************************

                    //*************** Amendment Details *******************************************************
                    RptAmendmentDetails.DataSource = lstRegulatory_Ammendments_Details;
                    RptAmendmentDetails.DataBind();
                    //*************** END *********************************************************************

                    //********************* IP Management******************************
                    RptrOther.DataSource = lstRegulatoryIPManagement;
                    RptrOther.DataBind();
                    TxtSubject.Text = _Regulatory_Master.s_RecruitedBy_TTSH;
                    TxtRemark.Text = _Regulatory_Master.s_Remarks;
                    //************************ END *********************************
                }


            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }
        protected void SHowTabs(DataTable dt)
        {
            dt.dtReadOnlyAndAllowDbNull();

            try
            {
                if (dt != null)
                {

                    if (dt.Rows.Count > 1)
                    {
                     
                        HiddenField Hdn;
                        HtmlAnchor a;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var MonthName = dt.Rows[i]["s_SixmonthName"].ToString();
                            int Countval = Convert.ToInt32(dt.Rows[i]["NoOfMonths"].ToString());

                            a = new HtmlAnchor();
                            Hdn = new HiddenField();

                            a.ID = "btn" + Countval.ToString();
                            a.InnerText = MonthName;
                            a.Attributes.Add("onclick", "return FillControls(this);");
                            a.Attributes.Add("Key", Countval.ToString());
                            a.Style.Add("width", "124px");
                            a.Style.Add("margin-right", "5px");
                            a.Style.Add("margin-bottom", "5px");
                            Hdn.ID = "Hdn~" + Countval.ToString();
                            string HdnId = Hdn.ID.Split('~')[1];

                            for (int j = dt.Columns.Count - 1; j > 1; j--)
                            {

                                if (Countval == Convert.ToInt32(HdnId))
                                {
                                    Hdn.Value += dt.Rows[i][j].ToString() + ",";
                                }
                            }
                            if (Convert.ToInt32(HdnSel.Value) == Convert.ToInt32(HdnId))
                            {
                                a.Attributes.Add("sel", "y");
                                a.Attributes.Add("class", "active");
                            }

                            a.Controls.Add(Hdn);
                            tblSIxMonth.Controls.Add(a);   //--------- for New Tab UI

                        }



                        // tblSIxMonth.Controls.Add(tr);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ClearAllRepeaterControls()
        {
            rptrPIDetails.DataSource = null;
            rptrPIDetails.DataBind();

            RptrStatusMontly.DataSource = null;
            RptrStatusMontly.DataBind();

            RptrStudyMember.DataSource = null;
            RptrStudyMember.DataBind();

            rptPIS.DataSource = null;
            rptPIS.DataBind();

            RptAmendmentDetails.DataSource = null;
            RptAmendmentDetails.DataBind();

            RptrOther.DataSource = null;
            RptrOther.DataBind();

            LnkDnwldCTCEmailApprDoc.Text = "";
            LnkDnwldNCTCEmailApprDoc.Text = "";
            LnkDwnldCTCDoc.Text = "";
            LnkDwnldExtCTCEmailApprDoc.Text = "";
        }

        protected void ClearRows(DataTable dt)
        {
            int k = 0;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["dt_LastUpdated_date"] != DBNull.Value)
                    {
                        k = i;
                        k = k + 1;

                    }
                }
                for (int i = dt.Rows.Count - 1; i >= k; i--)
                {
                    dt.Rows.RemoveAt(i);
                }
            }
        }

        protected void ClearHDN()
        {

            HdnProjectId.Value = "0";
            HdnRegId.Value = "0";
            HdnStudyTeamMembersDetails.Value = "";
            HdnPISDetails.Value = "";
            HdnStatusReportSubmissionFileDetails.Value = "";
            HdnAmendmentsDetails.Value = "";
            HdnOtherDetails.Value = "";
            RegSixMId.Value = "0";
            HdnSel.Value = "";
            hdnLastUpDate.Value = "";
            HdnAmendmentsDetails.Value = "";
            HdnCTCDocPath.Value = "";
            HdnExtCTCEmailApprDoc.Value = "";
            HdnGlobPath.Value = "";
            HdnNCTCEmailApprDoc.Value = "";
            HdnOtherDetails.Value = "";
            HdnPISDetails.Value = "";
            HdnStatusReportSubmissionFileDetails.Value = "";
            HdnStudyTeamMembersDetails.Value = "";
            HdnOtherSponsor.Value = "";
            HdnCTCStatus.Value = "";
            HdnExtendedStatus.Value = "";
        }
        [WebMethod]
        [ScriptMethod()]
        public static string Fpath(string obj)
        {

            string path = "";
            string FileName = Path.GetFileName(obj);
            path = obj;
            return path + "|" + FileName;
        }
        #endregion


    }
}