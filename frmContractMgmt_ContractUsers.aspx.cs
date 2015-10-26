using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using TTSHWeb.TTSHWCFReference;
using System.Web;
namespace TTSHWeb
{
    public partial class frmContractMgmt_ContractUsers : System.Web.UI.Page
    {
        #region " Load Event "
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.CONTRACT_MGMT;
            SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
            SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;

            if (!IsPostBack)
            {
                ClearHdnFld();
                TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                t.Text = "";
                FillMainGrid();
                FillClausesCheckList();
                FillCombo();
                ShowPanel();
                TxtContStartDate.Text = Common.SetCurrentDate();

            }
            CallJavascript();

        }
        #endregion

        #region " Button Event "
        protected void btnContractSave_Click(object sender, EventArgs e)
        {


            SaveFinalContract();

        }
        protected void lnkback_Click(object sender, EventArgs e)
        {
            ShowPanel();
            // FillMainGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {



            ShowPanel();
            FillMainGrid();
            switch (HdnMode.Value.ToLower())
            {
                case "insert":
                    this.MsgBox("Contract Management Details for the Project saved successfully...!!!");
                    break;
                case "update":
                    this.MsgBox("Contract Management Details for the Project Update successfully...!!!");
                    break;
                case "delete":
                    this.MsgBox("Contract Management Details for the Project delete successfully...!!!");
                    break;
            }
        }
        protected void lnkDwnldCorespondace_Click(object sender, EventArgs e)
        {
            Common.DownloadFileNew(hdnCoresPath.Value, Response);
        }

        protected void lnkDwnldAmendment_Click(object sender, EventArgs e)
        {
            Common.DownloadFileNew(HdnAmendPath.Value, Response);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowPanel();
            FillMainGrid();
        }
        protected void delcont_Click(object sender, EventArgs e)
        {
            Delete();
            this.PopUpMsg("Contract Detail Delete Successfully..!!", "AfterDelete()");
        }
        protected void btnContractCancel_Click(object sender, EventArgs e)
        {
            ClearContractControlAfterSave();
            //chkCollaboratorList.FillCheckList(DropDownName.FillCollaborators, HdnProjectId.Value);
            FillControl();
            btnContractSave.Text = "Save";
            HdnContractMode.Value = "Insert";
            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "collapse", "HideCollapseAtEdit()", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "collapse", "OnCancel()", true);
        }
        protected void btnResets_Click(object sender, EventArgs e)
        {
            btnContractSave.Style["display"] = "block";
            btnContractCancel.Style["display"] = "block";
            ClearContractControlAfterSave();
            HdnContractId.Value = "0";
            chkCollaboratorList.FillCheckList(DropDownName.FillCollaborators, HdnProjectId.Value);
            btnContractSave.Text = "Save";
            HdnContractMode.Value = "Insert";
            //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "collapse", "Retfalse();", true);
            h3.Style["display"] = "block";
            PNewCollaborator.Style["display"] = "block";
            LinkButton2.Style["display"] = "none";
        }
        void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            FillMainGrid();
        }

        void SearchBox_ButtonSearchClick(object sender, EventArgs e)
        {
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;
            if (string.IsNullOrEmpty(SearchBox.ErrorString))
            {
                Search[] lst = SearchBox.SearchOutput;
                rptrContractDetail.DataSource = lst;
                rptrContractDetail.DataBind();
            }
            else
            {
                //  this.MsgBox(SearchBox.ErrorString);
                rptrContractDetail.DataSource = null;
                rptrContractDetail.DataBind();
            }
        }
        #endregion

        #region " Repeater Event "
        protected void rptrContractDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                ClearHdnFld();

                HdnProjectId.Value = e.CommandArgument.ToString();
                if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview" | e.CommandName.ToLower() == "cmdadd")
                {
                    HdnMode.Value = e.CommandName.ToString().ConverMode();
                    PNewCollaborator.Style["display"] = "block";
                    //btnContractCancel.Style["display"] = "block";
                    //btnContractSave.Style["display"] = "block";
                    h3.Style["display"] = "block";
                    //btnCReset.Style["display"] = "none";
                    LinkButton2.Style["display"] = "none";
                    HdnContractMode.Value = "Insert";
                    btnContractSave.Text = "Save";
                    ShowPanel("entry");
                    bool enabled = (e.CommandName.ToString().ConverMode().ToLower() == "delete" || e.CommandName.ToString().ConverMode().ToLower() == "view") ? false : true;
                    EnableControls(false);
                    ClearContractControlAfterSave(btnText: e.CommandName.ToString().ConverMode());
                    if (HdnMode.Value.ToLower() != "insert")
                    {
                        FillProjectDataForNewEntry();
                        FillControl();
                    }
                    else
                    {

                        FillProjectDataForNewEntry();
                    }

                }
            }
        }
        protected void RptClauseDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlstatus = (DropDownList)e.Item.FindControl("ddlstatus");
                HiddenField HdnClauseId = (HiddenField)e.Item.FindControl("HdnClauseId");
                TextBox TxtComments = (TextBox)e.Item.FindControl("TxtComments");
                TextBox TxtProposedChanges = (TextBox)e.Item.FindControl("TxtProposedChanges");
                ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindByText(HdnClauseId.Value));

            }
        }

        protected void RptContract_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                PNewCollaborator.Style["display"] = "block";
                btnContractCancel.Style["display"] = "block";
                btnContractSave.Style["display"] = "block";
                //btnCReset.Style["display"] = "none";
                //h3.Style["display"] = "block";
                LinkButton2.Style["display"] = "none";
                HdnContractId.Value = e.CommandArgument.ToString();
                if (e.CommandName.ToLower() == "cmddelete" | e.CommandName.ToLower() == "cmdedit" | e.CommandName.ToLower() == "cmdview")
                {
                    HdnContractMode.Value = e.CommandName.ToString().ConverMode();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "collapse", "HideCollapseAtEdit()", true);
                    //if (HdnContractMode.Value.ToLower() == "delete")
                    //{
                    //    this.ConfirmMsg("Are You Sure to Delete this Contract..!!", "FuncDelete();");
                    //}
                    //else
                    //{
                    bool enabled = (e.CommandName.ToString().ConverMode().ToLower() == "delete" || e.CommandName.ToString().ConverMode().ToLower() == "view") ? false : true;

                    ClearContractControlAfterSave(enabled);
                    FillProjectDataForNewEntry();
                    FillControl();
                    FillContractDetail(e.CommandName.ToString().ConverMode());

                    // }

                }
            }
        }
        #endregion

        #region " Methods "
        protected void FillMainGrid()
        {
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Contract_Details> cdlist = new List<Contract_Details>();
            cdlist = cl.FillGrid_Contract_Details().ToList();
            rptrContractDetail.DataSource = cdlist; rptrContractDetail.DataBind();

        }
        protected void FillCombo()
        {
            ddlContractCategory.FillCombo(DropDownName.Contract_Category);
            ddlContractStatus.FillCombo(DropDownName.Contract_Status);
        }
        protected void FillClausesCheckList()
        {
            try
            {
                chkClause.FillCheckList(DropDownName.FillClauses);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void FillProjectDataForNewEntry()
        {
            TTSHWCFServiceClient servcl = new TTSHWCFServiceClient();
            List<PI_Master> piLst = new List<PI_Master>();
            List<Contract_Collobrator_Master> ccmlst = new List<Contract_Collobrator_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            ProjectDataforContractUsers pdc = new ProjectDataforContractUsers();
            try
            {
                pdc = servcl.FillProjectDataforContractUsers(Convert.ToInt32(HdnProjectId.Value));
                piLst = pdc.Pilisst.ToList();
                ccmlst = pdc.ccmlist.ToList();
                pjmasterlist = pdc.pmlist.ToList();
                TxtProjTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Title"].ToString();
                TxtAlias1.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias1"].ToString();
                TxtAlias2.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Project_Alias2"].ToString();
                TxtShortTitle.Text = pjmasterlist.ListToDatatable().Rows[0]["s_Short_Title"].ToString();
                TxtIrbNo.Text = pjmasterlist.ListToDatatable().Rows[0]["s_IRB_No"].ToString();
                TxtprojCategory.Text = pjmasterlist.ListToDatatable().Rows[0]["Project_Category_Name"].ToString();
                DispProjectId.InnerText = pjmasterlist.ListToDatatable().Rows[0]["s_Display_Project_ID"].ToString();
                rptrPIDetails.DataSource = piLst; rptrPIDetails.DataBind();
                RptrCollaborator.DataSource = ccmlst; RptrCollaborator.DataBind();
                var q = (from i in ccmlst select new { i.i_ID, i.s_Name }).ToList().ListToDatatable();
                FillCollaboratoList(q);

            }
            catch (Exception)
            {

                throw;
            }

        }
        protected void ShowPanel(string Type = "main")
        {
            DivMain.Style["display"] = "block"; DivContractDetailContainer.Style["display"] = "block";
            btnSave.Text = "Save Details";
            btnSave.Visible = true;

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

                            break;
                        case "view":
                            btnSave.Visible = false;

                            break;
                    }
                    break;
            }

        }
        protected void ClearControl()
        {
            TxtProjTitle.Text = ""; TxtAlias1.Text = ""; TxtAlias2.Text = ""; TxtShortTitle.Text = ""; TxtprojCategory.Text = ""; TxtIrbNo.Text = "";
            rptrPIDetails.DataSource = null; rptrPIDetails.DataBind(); RptrCollaborator.DataSource = null; RptrCollaborator.DataBind(); RptContract.DataSource = null; RptContract.DataBind();

        }
        protected void EnableControls(bool IsEnable = true)
        {
            TxtProjTitle.Enabled = IsEnable; ; TxtAlias1.Enabled = IsEnable; ; TxtAlias2.Enabled = IsEnable; ; TxtShortTitle.Enabled = IsEnable; ; TxtprojCategory.Enabled = IsEnable; ; TxtIrbNo.Enabled = IsEnable; ;

        }
        protected void FillCollaboratoList(DataTable dt)
        {
            chkCollaboratorList.DataSource = dt;
            chkCollaboratorList.DataTextField = "s_Name";
            chkCollaboratorList.DataValueField = "i_ID";
            chkCollaboratorList.DataBind();
        }
        protected void CallJavascript()
        {
            btnContractSave.Attributes.Add("onclick", "return ValidateContract('" + chkCollaboratorList.ClientID + "', '" + TxtContractName.ClientID + "', '" + ddlContractCategory.ClientID + "', '" + TxtContractId.ClientID + "', '" + HdnMode.ClientID + "', '" + HdnClauseValues.ClientID + "','" + TxtGovCountry.ClientID + "','" + ddlContractStatus.ClientID + "','" + chkClause.ClientID + "','" + HdnContractFiles.ClientID + "');");
        }
        protected void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<SelectedCollborators_Details> lstSelCollab = new List<SelectedCollborators_Details>();
            List<Selected_Clause_Details> lstSelClause = new List<Selected_Clause_Details>();
            List<ContractDetails_MultipleContractFile> lstmultiple = new List<ContractDetails_MultipleContractFile>();
            List<PI_Master> piList = new List<PI_Master>();
            List<Contract_Collobrator_Master> ccdlist = new List<Contract_Collobrator_Master>();
            List<Project_Master> pjmasterlist = new List<Project_Master>();
            Contract_Details _Contract_Details = new Contract_Details();
            List<ContractList> ctlist = new List<ContractList>();
            try
            {
                _Contract_Details = cl.GetContract_DetailsDetailsByID(Convert.ToInt32(HdnProjectId.Value));


                //****************Contact*******************
                RptContract.DataSource = null; RptContract.DataBind();
                if (_Contract_Details.contlist != null)
                {
                    ctlist = _Contract_Details.contlist.ToList();
                    RptContract.DataSource = ctlist;
                    RptContract.DataBind();
                }

                //***************END**********************

                //************Selected Collaborator*************
                chkCollaboratorList.FillCheckList(DropDownName.FillCollaborators, HdnProjectId.Value);
                //**************END*******************************
            }
            catch (Exception)
            {


            }
        }


        protected void ClearContractControlAfterSave(bool IsEnable = true, string btnText = "Save")
        {

            btnContractSave.Visible = true;
            TxtContractName.Text = "";
            TxtContractId.Text = "";
            ddlContractCategory.SelectedIndex = 0;
            ddlContractStatus.SelectedIndex = 0;
            TxtGovCountry.Text = "";
            HdnGovCountry.Value = "";

            TxtContractExpDate.Text = "";
            foreach (ListItem item in chkCollaboratorList.Items)
            {
                item.Selected = false;
            }
            foreach (ListItem item in chkClause.Items)
            {
                item.Selected = false;
            }
            TxtLastDate.Text = ""; TxtDateofLast.Text = "";
            TxtEffectiveDate.Text = "";
            TxtContractFinalizeDate.Text = "";
            ddlAmendments.SelectedIndex = 0;
            TxtNContractExpiryDate.Text = "";
            TxtprocedureCost.Text = "";
            TxtCoOrdinatorFess.Text = "";
            TxtInvestigatorFees.Text = "";
            TxtProjectBudgetCash.Text = "";

            RptClauseDetail.DataSource = null; RptClauseDetail.DataBind();
            lnkDwnldAmendment.Text = ""; HdnAmendPath.Value = "";
            lnkDwnldCorespondace.Text = ""; hdnCoresPath.Value = "";


            TxtContractName.Enabled = IsEnable;
            TxtContractId.Enabled = IsEnable;
            ddlContractCategory.Enabled = IsEnable;
            ddlContractStatus.Enabled = IsEnable;
            TxtGovCountry.Enabled = IsEnable;
            HdnGovCountry.Value = "";
            fldAmendmentFile.Enabled = IsEnable;
            fldContractFile.Enabled = IsEnable;
            fldCorespondace.Enabled = IsEnable;
            TxtContractExpDate.Enabled = IsEnable;
            chkClause.Enabled = IsEnable;
            chkCollaboratorList.Enabled = IsEnable;
            TxtLastDate.Enabled = IsEnable;
            TxtEffectiveDate.Enabled = IsEnable;
            TxtContractFinalizeDate.Enabled = IsEnable;
            ddlAmendments.Enabled = IsEnable;

            TxtNContractExpiryDate.Enabled = IsEnable;
            TxtprocedureCost.Enabled = IsEnable;
            TxtCoOrdinatorFess.Enabled = IsEnable;
            TxtInvestigatorFees.Enabled = IsEnable;
            TxtProjectBudgetCash.Enabled = IsEnable;
            lnkDwnldAmendment.Enabled = IsEnable; HdnAmendPath.Value = "";
            lnkDwnldCorespondace.Enabled = IsEnable; hdnCoresPath.Value = "";


        }
        protected bool SaveFinalContract()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<SelectedCollborators_Details> lstSelCollab = new List<SelectedCollborators_Details>();
            List<Selected_Clause_Details> lstSelClause = new List<Selected_Clause_Details>();
            List<ContractDetails_MultipleContractFile> lstmultiple = new List<ContractDetails_MultipleContractFile>();
            Contract_Details cd = new Contract_Details();
            SelectedCollborators_Details scd = new SelectedCollborators_Details();
            Selected_Clause_Details selcd = new Selected_Clause_Details();
            ContractDetails_MultipleContractFile cdm = new ContractDetails_MultipleContractFile();
            string result = "";

            try
            {
                cd.i_ID = Convert.ToInt32(Common.iffBlank(HdnContractId.Value, 0));
                cd.s_Contract_Display_Id = Convert.ToString(Common.iffBlank(TxtContractId.Text, ""));
                cd.s_Contract_Name = Convert.ToString(Common.iffBlank(TxtContractName.Text, ""));
                cd.i_Project_ID = Convert.ToInt32(HdnProjectId.Value);
                cd.i_Contract_Category_ID = Convert.ToInt32(Common.iffBlank(ddlContractCategory.SelectedValue, 0));
                cd.i_Contract_Status_ID = Convert.ToInt32(Common.iffBlank(ddlContractStatus.SelectedValue, 0));
                cd.dt_Contract_StatusDate = TxtContStartDate.Text == "" ? null : TxtContStartDate.Text;
                cd.i_Govt_Lawcountry = Convert.ToInt32(Common.iffBlank(HdnGovCountry.Value, 0));
                cd.dt_LastUpdated_Date = TxtLastDate.Text == "" ? null : TxtLastDate.Text;
                cd.s_Clauses_File = Convert.ToString(Common.iffBlank((fldCorespondace.HasFile) ? String.Join(",", Common.UpLoadNew(fldCorespondace, Common.FolderLocation.ContractMgmtUser).Select(r => r.ToString())) : hdnCoresPath.Value, ""));
                if (hdnCoresPath.Value.Length > 0)
                {
                    cd.s_Clauses_File = (cd.s_Clauses_File != "") ? cd.s_Clauses_File : hdnCoresPath.Value;
                }
                cd.i_Hospital_Cost = Convert.ToDouble(Common.iffBlank(TxtprocedureCost.Text, 0));
                cd.i_Investigator_fees = Convert.ToDouble(Common.iffBlank(TxtInvestigatorFees.Text, 0));
                cd.i_Coordinator_fess = Convert.ToDouble(Common.iffBlank(TxtCoOrdinatorFess.Text, 0));

                string[] arr = String.Join(",", chkCollaboratorList.Items.OfType<ListItem>().Where(r => r.Selected).Select(r => r.Value)).Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    lstSelCollab.Add(new SelectedCollborators_Details
                    {
                        i_Contract_Details_Id = 0,
                        i_Collobrator_ID = Convert.ToInt32(arr[i])
                    });
                }
                string[] ClausesVal = HdnClauseValues.Value.ToString().Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.ToString()).ToArray();
                for (int j = 0; j < ClausesVal.Length; j++)
                {
                    lstSelClause.Add(new Selected_Clause_Details
                    {
                        i_Contract_ID = 0,
                        i_Contract_Clause_ID = Convert.ToInt32(Common.iffBlank(ClausesVal[j].Split('|')[0], 0)),
                        s_Status = Convert.ToString(Common.iffBlank(ClausesVal[j].Split('|')[1], "")),
                        s_Comments = Convert.ToString(Common.iffBlank(ClausesVal[j].Split('|')[3], "")),
                        s_Proposed_Changes = Convert.ToString(Common.iffBlank(ClausesVal[j].Split('|')[4], "")),
                    });
                }
                //if (ddlContractStatus.SelectedItem.Text.ToLower() == "completed")
                //{
                    cd.dt_Effective_Date = TxtEffectiveDate.Text == "" ? null : TxtEffectiveDate.Text;
                    cd.dt_Expiry_Date = TxtContractExpDate.Text == "" ? null : TxtContractExpDate.Text;
                    cd.dt_Finalization_Date = TxtContractFinalizeDate.Text == "" ? null : TxtContractFinalizeDate.Text;
                    cd.dt_LastSigned_Date = TxtDateofLast.Text == "" ? null : TxtDateofLast.Text;


                    if (fldContractFile.HasFiles)
                    {
                        string[] multipleFiles = String.Join(",", Common.UpLoadNew(fldContractFile, Common.FolderLocation.ContractMgmtUser).Select(r => r.ToString())).Split(',');
                        for (int k = 0; k < multipleFiles.Length; k++)
                        {
                            lstmultiple.Add(new ContractDetails_MultipleContractFile
                            {
                                i_ContractDetailsID = 0,
                                s_ContractFile = Convert.ToString(Common.iffBlank(multipleFiles[k], ""))
                            });
                        }
                    }
                    if (HdnContractFiles.Value.Length > 0)
                    {
                        string[] files = HdnContractFiles.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < files.Length; i++)
                        {
                            lstmultiple.Add(new ContractDetails_MultipleContractFile
                            {
                                i_ContractDetailsID = 0,
                                s_ContractFile = Convert.ToString(Common.iffBlank(files[i], ""))
                            });

                        }
                    }

                    if (ddlAmendments.SelectedItem.Text.ToLower() == "yes")
                    {
                        cd.b_Amendments = Convert.ToInt32(Common.iffBlank(ddlAmendments.SelectedItem.Value, 0)) == 1 ? true : false;

                        cd.dt_NewExpiry_Date = Convert.ToString(Common.iffBlank(TxtNContractExpiryDate.Text, ""));
                        cd.s_AmendmenstContract_File = Convert.ToString(Common.iffBlank((fldAmendmentFile.HasFile) ? String.Join(",", Common.UpLoadNew(fldAmendmentFile, Common.FolderLocation.ContractMgmtUser).Select(r => r.ToString())) : HdnAmendPath.Value, ""));
                        if (HdnAmendPath.Value.Length > 0)
                        {
                            cd.s_AmendmenstContract_File = (cd.s_AmendmenstContract_File != "") ? cd.s_AmendmenstContract_File : HdnAmendPath.Value;
                        }

                    }

               // }

                //--------UID and UName----
                cd.UName = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString();
                cd.UID = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString();
                //----------- END ------------

                result = cl.Contract_Details_DML(cd, lstSelCollab.ToArray(), lstSelClause.ToArray(), lstmultiple.ToArray(), HdnContractMode.Value);

                //}
                if (result.Split('|')[0].ToLower().Trim() == "success" && result.Split('|')[1].ToLower().CheckInt() == true)
                {

                    switch (HdnContractMode.Value.ToLower())
                    {
                        case "update": this.PopUpMsg("Contract Detail Update  Successfully..!!", "AfterSave();"); break;
                        case "delete": this.PopUpMsg("Contract Detail Delete  Successfully..!!", "AfterSave();"); break;
                        case "insert": this.PopUpMsg("Contract Detail Save  Successfully..!!", "AfterSave();"); break;

                    }
                    btnResets_Click(null, null);
                    btnContractSave.Text = "Save";
                    HdnContractId.Value = result.Split('|')[1];
                    //ClearContractControlAfterSave();
                    FillControl();
                }
                else
                {
                    this.MsgBox(result.Split('|')[1]);
                    return false;
                }
            }
            //}

                //}
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
                return false;
            }
            return true;
        }
        protected void FillContractDetail(string mode = "Save", int ProjectId = 0)
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<SelectedCollborators_Details> lstSelCollab = new List<SelectedCollborators_Details>();
            List<Selected_Clause_Details> lstSelClause = new List<Selected_Clause_Details>();
            List<ContractDetails_MultipleContractFile> lstmultiple = new List<ContractDetails_MultipleContractFile>();
            Contract_Details _Contract_Details = new Contract_Details();
            List<ContractList> ctlist = new List<ContractList>();
            List<Contract_Status_Date> lstcsd = new List<Contract_Status_Date>();
            try
            {
                _Contract_Details = cl.GetContractDeta(Convert.ToInt32(Common.iffBlank(HdnContractId.Value, 0)), ProjectId);

                ////****************Contact*******************
                //ctlist = _Contract_Details.contlist.ToList();
                //RptContract.DataSource = ctlist;
                //RptContract.DataBind();
                ////***************END**********************



                //if (Convert.ToInt32(Common.iffBlank(HdnContractId.Value, 0)) > 0)
                //{


                //************Selected Collaborator*************
                chkCollaboratorList.FillCheckList(DropDownName.FillCollaborators, HdnProjectId.Value);
                lstSelCollab = _Contract_Details.lstSelCollab.ToList();
                if (chkCollaboratorList.Items.Count > 0)
                {
                    for (int i = 0; i < lstSelCollab.ListToDatatable().Rows.Count; i++)
                    {
                        chkCollaboratorList.Items.Add(new ListItem() { Value = Convert.ToString(lstSelCollab.ListToDatatable().Rows[i]["i_Collobrator_ID"]), Text = Convert.ToString(lstSelCollab.ListToDatatable().Rows[i]["s_Name"]), Selected = true });
                    }

                }
                else
                {
                    chkCollaboratorList.DataSource = lstSelCollab;
                    chkCollaboratorList.DataTextField = "s_Name";
                    chkCollaboratorList.DataValueField = "i_Collobrator_ID";
                    chkCollaboratorList.DataBind();
                    foreach (ListItem item in chkCollaboratorList.Items)
                    {
                        item.Selected = true;
                    }
                }

                //var id = (from i in lstSelCollab select new { i.i_Collobrator_ID }).ToList().ListToDatatable();

                //for (int i = 0; i < id.Rows.Count; i++)
                //{
                //    for (int j = 0; j < chkCollaboratorList.Items.Count; j++)
                //    {
                //        if (chkCollaboratorList.Items[j].Value == Convert.ToString(id.Rows[i]["i_Collobrator_ID"]))
                //        {
                //            chkCollaboratorList.Items[j].Selected = true;
                //        }
                //    }
                //}
                //**************END*******************************

                //***********************Selected Clauese***************
                lstSelClause = _Contract_Details.lstSelClause.ToList();
                RptClauseDetail.DataSource = lstSelClause; RptClauseDetail.DataBind();

                //**************************END************************



                TxtContractName.Text = _Contract_Details.s_Contract_Name;
                TxtContractId.Text = _Contract_Details.s_Contract_Display_Id;
                FillCombo();
                ddlContractCategory.SelectedIndex = ddlContractCategory.Items.IndexOf(ddlContractCategory.Items.FindByValue(_Contract_Details.i_Contract_Category_ID.ToString()));
                ddlContractStatus.SelectedIndex = ddlContractStatus.Items.IndexOf(ddlContractStatus.Items.FindByValue(_Contract_Details.i_Contract_Status_ID.ToString()));
                if (ddlContractStatus.SelectedItem.Text.ToLower() == "terminated/withdrawn" || ddlContractStatus.SelectedItem.Text.ToLower() == "completed")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showMsg", "javascript:DisableOnCompletedStatus();", true);

                }
                lstcsd = _Contract_Details.listcsd.ToList();
                DataRow[] dr = lstcsd.ListToDatatable().Select("i_Contract_Status_ID=" + ddlContractStatus.SelectedValue);
                TxtContStartDate.Text = (dr.Length > 0) ? Convert.ToString(dr[0].ItemArray[0]) : "";
                FillClausesCheckList();
                var q = (from i in lstSelClause select new { i.Clause_Name, i.i_Contract_Clause_ID }).ToList().ListToDatatable();
                string arrvalue = "";
                foreach (DataRow item in q.Rows)
                {
                    arrvalue +=item["i_Contract_Clause_ID"] + ",";
                }
                arrvalue = (arrvalue!="") ? arrvalue.TrimEnd(','):"";
                for (int j = 0; j < q.Rows.Count; j++)
                {


                    for (int i = 0; i < chkClause.Items.Count; i++)
                    {
                        if (chkClause.Items[i].Value == Convert.ToString(q.Rows[j]["i_Contract_Clause_ID"]))
                        {
                            chkClause.Items[i].Selected = true;

                        }
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showMsg", "javascript:PushValues('" + arrvalue + "')", true);
                TxtGovCountry.Text = _Contract_Details.s_Country;
                HdnGovCountry.Value = Convert.ToString(_Contract_Details.i_Govt_Lawcountry);

                //***************Corrrespondance File*******************
                hdnCoresPath.Value = _Contract_Details.s_Clauses_File;
                lnkDwnldCorespondace.Text = _Contract_Details.s_Clauses_File.GetFileName();
                //*************END*********************
                TxtLastDate.Text = _Contract_Details.dt_LastUpdated_Date;
                //if (ddlContractStatus.SelectedItem.Text.ToLower() == "completed")
                //{


                    //************Multiple Contract File************
                    lstmultiple = _Contract_Details.lstmultiple.ToList();
                    var h = (from k in lstmultiple select new { k.s_ContractFile }).ToList().ListToDatatable();

                    if (h != null)
                    {

                        for (int i = 0; i < h.Rows.Count; i++)
                        {


                            HiddenField hdn = new HiddenField();
                            LinkButton lnk = new LinkButton();
                            HtmlGenericControl div = new HtmlGenericControl("div"); HtmlGenericControl a = new HtmlGenericControl("a");
                            lnk.ID = "lnk" + i.ToString();
                            hdn.Value = Convert.ToString(h.Rows[i][0]);
                            hdn.ID = "hdn" + i.ToString();
                            a.ID = "a~" + i.ToString();
                            lnk.Text = h.Rows[i][0].ToString().GetFileName();
                            a.InnerText = "X";

                            lnk.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + hdn.Value + "')";

                            div.Attributes.Add("class", "MultiFile-label");
                            a.Attributes.Add("class", "MultiFile-remove");
                            a.Style["margin-right"] = "5px";
                            a.Attributes.Add("onclick", "return RemoveDownloadFile(this)");
                            lnk.Attributes.Add("class", "MultiFile-title");
                            div.Controls.Add(a);
                            div.Controls.Add(lnk);
                            div.Controls.Add(hdn);
                            multilistdiv.Controls.Add(div);

                        }

                    }



                    TxtContractExpDate.Text = _Contract_Details.dt_Expiry_Date;
                    HdnContExpDate.Value = _Contract_Details.dt_Expiry_Date;
                    TxtEffectiveDate.Text = _Contract_Details.dt_Effective_Date;
                    TxtDateofLast.Text = _Contract_Details.dt_LastSigned_Date;
                    TxtContractFinalizeDate.Text = _Contract_Details.dt_Finalization_Date;



                    ddlAmendments.SelectedIndex = (_Contract_Details.b_Amendments == true) ? 1 : 0;
                    if (ddlAmendments.SelectedIndex == 1)
                    {



                        TxtNContractExpiryDate.Text = _Contract_Details.dt_NewExpiry_Date;
                        //**************Amendments File******************
                        HdnAmendPath.Value = _Contract_Details.s_AmendmenstContract_File;
                        lnkDwnldAmendment.Text = _Contract_Details.s_AmendmenstContract_File.GetFileName();
                        //***************END*****************************
                    }
               // }
                //*****************END************************
                TxtprocedureCost.Text = (_Contract_Details.i_Hospital_Cost == 0) ? "" : Convert.ToString(_Contract_Details.i_Hospital_Cost);
                TxtInvestigatorFees.Text = (_Contract_Details.i_Investigator_fees == 0) ? "" : Convert.ToString(_Contract_Details.i_Investigator_fees);
                TxtCoOrdinatorFess.Text = (_Contract_Details.i_Coordinator_fess == 0) ? "" : Convert.ToString(_Contract_Details.i_Coordinator_fess);
                TxtProjectBudgetCash.Text = Convert.ToString(Convert.ToInt32(Common.iffBlank(_Contract_Details.i_Hospital_Cost, 0)) +
                                                                Convert.ToInt32(Common.iffBlank(_Contract_Details.i_Investigator_fees, 0)) +
                                                                Convert.ToInt32(Common.iffBlank(_Contract_Details.i_Coordinator_fess, 0)));
                if (TxtProjectBudgetCash.Text == "0")
                {
                    TxtProjectBudgetCash.Text = "";
                }

                btnContractSave.Visible = true;

                if (mode.ToLower() == "view")
                {
                    PNewCollaborator.Style["display"] = "none";
                    //  btnCReset.Style["display"] = "block";
                    btnContractCancel.Style["display"] = "none";
                    btnContractSave.Style["display"] = "none";
                    h3.Style["display"] = "none";
                    LinkButton2.Style["display"] = "block";

                }
                if (mode.ToLower() == "delete")
                {
                    btnCReset.Style["display"] = "none";
                    btnContractSave.Style["display"] = "block";
                    btnContractCancel.Style["display"] = "block";
                    btnContractSave.Text = mode;
                }
                else
                {
                    btnContractSave.Text = mode;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void Delete()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            string result = cl.GetValidate("DeleteContract", "1", "a", HdnContractId.Value, "");
            if (result != "")
            {

                ClearContractControlAfterSave();
                RptContract.DataSource = null; RptContract.DataBind();
                FillControl();


            }

        }
        protected void ClearHdnFld()
        {
            HdnContractFiles.Value = "";
            HdnProjectId.Value = "";
            HdnClauseValues.Value = "";
            HdnselUser.Value = "";
            HdnContExpDate.Value = "";
        }

        #endregion

    }
}