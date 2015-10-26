using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TTSHWeb.TTSHWCFReference;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web;


namespace TTSHWeb
{
    public partial class FeasibilityDetails : System.Web.UI.Page
    {

        string LoginUser = "";
        string LoginUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.FEASIBALITY; /*setting filtercriteria*/
            SearchBox.ButtonSearchClick += new EventHandler(SearchBox_ButtonClick); /*adding events*/
            SearchBox.ButtonClearClick += new EventHandler(SearchBox_ClearClick); /*adding events*/

            if (!IsPostBack)
            {
                TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                t.Text = "";
                FillGrid();
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
            //End of Get Login User Details
        }

        public void FillGrid()
        {
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            List<Feasibility_Grid> gridData = new List<Feasibility_Grid>();

            gridData = client.Feasibility_FillGrid().ToList();

            try
            {
                string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                Project_DataOwner[] oDOList = client.GetProjectsByDO("FEASIBILITY", UserID);
                DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                var AdminArray = (from s in oDataOwner
                                  select s.GUID).ToList();

                bool IsAdmin = AdminArray.Contains(UserID);

                if (IsAdmin == false)
                {
                    List<Feasibility_Grid> oNewGrid = new List<Feasibility_Grid>();
                    if (gridData != null && gridData.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                    {
                        var v = gridData.Select(z => z.Feasibility_Status_Name).Distinct().ToList();
                        oNewGrid = gridData.Where(z => z.Feasibility_Status_Name.ToUpper() == "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.Equals(z.s_Display_Project_ID))).ToList();
                        oNewGrid.ForEach(i => i.Status = "New");
                        gridData.RemoveAll(z => z.Feasibility_Status_Name.ToUpper() == "NEW");
                        gridData.AddRange(oNewGrid);
                        gridData.Where(z => z.Feasibility_Status_Name.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "View");
                        gridData.Where(z => z.Feasibility_Status_Name.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "Edit");
                        gridData = gridData.OrderByDescending(z => z.i_Project_ID).ToList();
                    }
                    else if (gridData != null && gridData.Count() > 0)
                    {
                        gridData.ForEach(x => x.Status = "View");
                        gridData.OrderByDescending(z => z.i_Project_ID);
                    }
                }
                else
                {
                    gridData.Where(z => z.Feasibility_Status_Name.ToUpper() == "NEW").ToList().ForEach(i => i.Status = "New");
                    //gridData.Where(z => z.Feasibility_Status_Name.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "View");
                    gridData.Where(z => z.Feasibility_Status_Name.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "Edit");
                    gridData = gridData.OrderByDescending(z => z.i_Project_ID).ToList();
                }
            }
            catch (Exception ex1)
            {

            }


            rptrProjectDetail.DataSource = gridData;

            rptrProjectDetail.DataBind();

        }

        protected void NewLink_Command(object sender, CommandEventArgs e)
        {
            projectGrid.Visible = false;

            //To reset all the controls
            ResetControls();

            FeasibilityContainer.Visible = true;

            PopulateDropDown();

            btnUpdateFeasibility.Visible = false;

            btnSaveFeasibility.Visible = true;

            TTSHWCFReference.Project_Master project = new Project_Master();

            int project_id = Convert.ToInt32(e.CommandArgument);

            Session["ProjectID"] = project_id;

            bProjectID.InnerText = "Project ID: " + project_id;

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            project = client.GetProject_MasterDetailsByID(project_id);

            txtProjectTitle.Text = project.s_Project_Title;

            txtFeasibilityTitle.Text = project.s_Project_Title;

            txtShortTitle.Text = project.s_Short_Title;

            ddlProjectCategory.SelectedValue = project.i_Project_Category_ID.ToString();

            txtAlias1.Text = project.s_Project_Alias1;

            txtAlias2.Text = project.s_Project_Alias2;
            bProjectID.InnerText = "Project ID: " + project.s_Display_Project_ID;
            //Set Update By and Updated date
            txtLastUpdatedBy.Text = LoginUser;

            txtDateUpdated.Text = DateTime.Now.ToString("dd-MMM-yy");

            FillPIGrid(project.DEPT_PI.ToList());

            ddlConfidentialAgreement.SelectedValue = "1";

            txtFeasibilityStartDate.Text = DateTime.Now.ToString("dd-MMM-yy");
            if (ddlConfidentialAgreement.SelectedValue == "1")
            {
                hdnAgreementFileEnabled.Value = "Yes";
            }
            else
            {
                hdnAgreementFileEnabled.Value = "No";
            }

        }

        protected void EditLink_Command(object sender, CommandEventArgs e)
        {
            projectGrid.Visible = false;

            FeasibilityContainer.Visible = true;

            txtFeasibilityStartDate.Enabled = false;

            ResetControls();

            PopulateDropDown();

            btnUpdateFeasibility.Visible = true;
            btnSaveFeasibility.Visible = false;

            int FeasibilityID = Convert.ToInt32(e.CommandArgument);

            Session["FeasibilityID"] = FeasibilityID;

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();
            TTSHWCFReference.Feasibility_Details feasibility = client.GetFeasibility_DetailsByID(FeasibilityID);

            bProjectID.InnerText = "Project ID: " + feasibility.i_Project_ID;

            Session["ProjectID"] = feasibility.i_Project_ID;

            //Set project details
            string projectXML = feasibility.PROJECT_DATA;

            //Parse xml and bind project details
            if (projectXML != string.Empty && projectXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(projectXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/PROJECT/PROJECT_DATA");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        txtAlias1.Text = node["s_Project_Alias1"] == null ? "" : node["s_Project_Alias1"].InnerText;

                        txtAlias2.Text = node["s_Project_Alias2"] == null ? "" : node["s_Project_Alias2"].InnerText;

                        txtProjectTitle.Text = node["s_Project_Title"] == null ? "" : node["s_Project_Title"].InnerText;

                        txtShortTitle.Text = node["s_Short_Title"] == null ? "" : node["s_Short_Title"].InnerText;

                        ddlProjectCategory.SelectedValue = node["i_Project_Category_ID"] == null ? "" : node["i_Project_Category_ID"].InnerText;

                        bProjectID.InnerText = "Project ID:" + node["s_Display_Project_ID"].InnerText;
                    }
                }
            }
            //End of Set project details

            string CROXML = "";

            CROXML = feasibility.CRA;
            //Parse xml and bind CRO details
            if (CROXML != string.Empty && CROXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(CROXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/CRA/CRA_DETAILS");

                    foreach (XmlNode node in xmlNodeList)
                    {
                        txtCRO.Text = node["CRO_NAME"] == null ? "" : node["CRO_NAME"].InnerText;
                        HdnCROTxt.Value = txtCRO.Text;
                        hdnCROID.Value = node["i_CRO_ID"] == null ? "" : node["i_CRO_ID"].InnerText;
                    }
                }
            }
            //End of Set CRO details

            string Sponsorxml = feasibility.SPONSOR;

            //Parse xml and bind Sponsor details
            if (Sponsorxml != string.Empty && Sponsorxml != null)
            {

                using (XmlReader reader = XmlReader.Create(new StringReader(Sponsorxml)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/SPONSOR/SPONSOR_D");

                    foreach (XmlNode node in xmlNodeList)
                    {
                        txtSponsorName.Text = node["s_Name"] == null ? "" : node["s_Name"].InnerText;
                        HdnSponsorTxt.Value = txtSponsorName.Text;
                        hdnSponsorID.Value = node["i_ID"] == null ? "" : node["i_ID"].InnerText;
                    }
                }

            }
            //End of Set Sponsor details

            //Fill Pi Grid
            if (feasibility.DEPT_PI != null)
                FillPIGrid(feasibility.DEPT_PI.ToList());

            //Set all text box
            txtCo_Investigator.Text = feasibility.s_Coinvestigator;
            txtDateInitialEmailSent.Text = (feasibility.s_Email_Send_Date == null || feasibility.s_Email_Send_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.s_Email_Send_Date).ToString("dd-MMM-yyyy"));
            txtDateUpdated.Text = (feasibility.dt_Modify_Date == null ? "" : Convert.ToDateTime(feasibility.dt_Modify_Date).ToString("dd-MMM-yyyy"));
            txtFeasibilityStartDate.Text = (feasibility.dt_Feasibility_Start_Date == null || feasibility.dt_Feasibility_Start_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Feasibility_Start_Date).ToString("dd-MMM-yyyy"));
            txtFeasibilityTitle.Text = feasibility.s_Feasibility_Title;
            txtIMInvitation.Text = feasibility.s_IM_Invitation;
            txtInterestComment.Text = feasibility.s_Interest_Comments;
            txtLastUpdatedBy.Text = feasibility.s_ModifyBy_Name;
            txtProtocolComments.Text = feasibility.s_Protocol_Comments;
            txtProtocolDate.Text = (feasibility.dt_Protocol_Date == null || feasibility.dt_Protocol_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Protocol_Date).ToString("dd-MMM-yyyy"));
            txtProtocolDocNo.Text = feasibility.s_Prototcol_Doc_No;
            txtProtocolNumber.Text = feasibility.s_Protocol_No;
            txtSiteVisitDate.Text = (feasibility.dt_Site_Visit_Date == null || feasibility.dt_Site_Visit_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Site_Visit_Date).ToString("dd-MMM-yyyy"));
            txtSurveyComments.Text = feasibility.s_Survey_Comments;
            txtSurveyDate.Text = (feasibility.dt_Survey_Date == null || feasibility.dt_Survey_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Survey_Date).ToString("dd-MMM-yyyy"));

            //Set All Drop Down 
            if (feasibility.b_Confidential_Agreement == null) ddlConfidentialAgreement.SelectedValue = "-1";
            else ddlConfidentialAgreement.SelectedValue = (feasibility.b_Confidential_Agreement == true ? "1" : "0");
            if (feasibility.b_Confidential_Agreement == true)
            {
                hdnAgreementFileEnabled.Value = "Yes";
            }
            else
            {
                hdnAgreementFileEnabled.Value = "No";
            }

            if (feasibility.i_Feasibility_Status_ID == 0) ddlCurrentStatus.SelectedValue = "-1";
            else ddlCurrentStatus.SelectedValue = feasibility.i_Feasibility_Status_ID.ToString();

            if (feasibility.b_Feasibility_Outcome == null) ddlFeasibilityOutcome.SelectedValue = "-1";
            else ddlFeasibilityOutcome.SelectedValue = (feasibility.b_Feasibility_Outcome == true ? "1" : "0");

            if (feasibility.s_In_File == null) ddlInFile.SelectedValue = "-1";
            else ddlInFile.SelectedValue = (feasibility.s_In_File == true ? "1" : "0");

            ddlInterest.SelectedValue = feasibility.b_Interest == true ? "1" : "0";
            /*Download file links*/

            //Checklist link
            if (feasibility.s_Checklist_File != null && feasibility.s_Checklist_File != "")
            {
                DownChecklist.Visible = true;

                hdnChecklist.Value = feasibility.s_Checklist_File;

                string[] arr = feasibility.s_Checklist_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownChecklist.Text = fName;

            }

            // Agreement file
            if (feasibility.s_Confidential_Agreement_File != null && feasibility.s_Confidential_Agreement_File != "")
            {
                DownAgreementFile.Visible = true;

                hdnAgreementFile.Value = feasibility.s_Confidential_Agreement_File;

                string[] arr = feasibility.s_Confidential_Agreement_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownAgreementFile.Text = fName;

            }

            // Protocol file
            if (feasibility.s_Prototcol_File != null && feasibility.s_Prototcol_File != "")
            {
                DownProtocolFile.Visible = true;

                hdnProtocolFile.Value = feasibility.s_Prototcol_File;

                string[] arr = feasibility.s_Prototcol_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownProtocolFile.Text = fName;

            }

            // Questionaire file
            if (feasibility.s_Questionnaire_File != null && feasibility.s_Questionnaire_File != "")
            {
                DownQuestFile.Visible = true;

                hdnQuestFile.Value = feasibility.s_Questionnaire_File;

                string[] arr = feasibility.s_Questionnaire_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownQuestFile.Text = fName;

            }


        }

        private void FillPIGrid(List<PI_Master> listPI)
        {
            rptrPIDetails.DataSource = listPI;
            rptrPIDetails.DataBind();

        }

        protected void ViewLink_Command(object sender, CommandEventArgs e)
        {
            projectGrid.Visible = false;

            FeasibilityContainer.Visible = true;

            ResetControls();

            PopulateDropDown();

            btnUpdateFeasibility.Visible = true;
            btnSaveFeasibility.Visible = false;

            int FeasibilityID = Convert.ToInt32(e.CommandArgument);

            Session["FeasibilityID"] = FeasibilityID;

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();
            TTSHWCFReference.Feasibility_Details feasibility = client.GetFeasibility_DetailsByID(FeasibilityID);

            bProjectID.InnerText = "Project ID: " + feasibility.i_Project_ID;

            Session["ProjectID"] = feasibility.i_Project_ID;

            //Set project details
            string projectXML = feasibility.PROJECT_DATA;

            //Parse xml and bind project details
            if (projectXML != string.Empty && projectXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(projectXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/PROJECT/PROJECT_DATA");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        txtAlias1.Text = node["s_Project_Alias1"] == null ? "" : node["s_Project_Alias1"].InnerText;

                        txtAlias2.Text = node["s_Project_Alias2"] == null ? "" : node["s_Project_Alias2"].InnerText;

                        txtProjectTitle.Text = node["s_Project_Title"] == null ? "" : node["s_Project_Title"].InnerText;

                        txtShortTitle.Text = node["s_Short_Title"] == null ? "" : node["s_Short_Title"].InnerText;

                        ddlProjectCategory.SelectedValue = node["i_Project_Category_ID"] == null ? "" : node["i_Project_Category_ID"].InnerText;
                        bProjectID.InnerText = "Project ID:" + node["s_Display_Project_ID"].InnerText;

                    }
                }
            }
            //End of Set project details

            string CROXML = "";

            CROXML = feasibility.CRA;
            //Parse xml and bind CRO details
            if (CROXML != string.Empty && CROXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(CROXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/CRA/CRA_DETAILS");

                    foreach (XmlNode node in xmlNodeList)
                    {
                        txtCRO.Text = node["CRO_NAME"] == null ? "" : node["CRO_NAME"].InnerText;

                        hdnCROID.Value = node["i_CRO_ID"] == null ? "" : node["i_CRO_ID"].InnerText;
                    }
                }
            }
            //End of Set CRO details

            string Sponsorxml = feasibility.SPONSOR;

            //Parse xml and bind Sponsor details
            if (Sponsorxml != string.Empty && Sponsorxml != null)
            {

                using (XmlReader reader = XmlReader.Create(new StringReader(Sponsorxml)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/SPONSOR/SPONSOR_D");

                    foreach (XmlNode node in xmlNodeList)
                    {
                        txtSponsorName.Text = node["s_Name"] == null ? "" : node["s_Name"].InnerText;

                        hdnSponsorID.Value = node["i_ID"] == null ? "" : node["i_ID"].InnerText;
                    }
                }

            }
            //End of Set Sponsor details

            //Fill Pi Grid
            if (feasibility.DEPT_PI != null)
                FillPIGrid(feasibility.DEPT_PI.ToList());

            //Set all text box
            txtCo_Investigator.Text = feasibility.s_Coinvestigator;
            txtDateInitialEmailSent.Text = (feasibility.s_Email_Send_Date == null || feasibility.s_Email_Send_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.s_Email_Send_Date).ToString("dd-MMM-yyyy"));
            txtDateUpdated.Text = (feasibility.dt_Modify_Date == null ? "" : Convert.ToDateTime(feasibility.dt_Modify_Date).ToString("dd-MMM-yyyy"));
            txtFeasibilityStartDate.Text = (feasibility.dt_Feasibility_Start_Date == null || feasibility.dt_Feasibility_Start_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Feasibility_Start_Date).ToString("dd-MMM-yyyy"));
            txtFeasibilityTitle.Text = feasibility.s_Feasibility_Title;
            txtIMInvitation.Text = feasibility.s_IM_Invitation;
            txtInterestComment.Text = feasibility.s_Interest_Comments;
            txtLastUpdatedBy.Text = feasibility.s_ModifyBy_Name;
            txtProtocolComments.Text = feasibility.s_Protocol_Comments;
            txtProtocolDate.Text = (feasibility.dt_Protocol_Date == null || feasibility.dt_Protocol_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Protocol_Date).ToString("dd-MMM-yyyy"));
            txtProtocolDocNo.Text = feasibility.s_Prototcol_Doc_No;
            txtProtocolNumber.Text = feasibility.s_Protocol_No;
            txtSiteVisitDate.Text = (feasibility.dt_Site_Visit_Date == null || feasibility.dt_Site_Visit_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Site_Visit_Date).ToString("dd-MMM-yyyy"));
            txtSurveyComments.Text = feasibility.s_Survey_Comments;
            txtSurveyDate.Text = (feasibility.dt_Survey_Date == null || feasibility.dt_Survey_Date == DateTime.MinValue ? "" : Convert.ToDateTime(feasibility.dt_Survey_Date).ToString("dd-MMM-yyyy"));

            //Set All Drop Down 
            if (feasibility.b_Confidential_Agreement == null) ddlConfidentialAgreement.SelectedValue = "1";
            else ddlConfidentialAgreement.SelectedValue = (feasibility.b_Confidential_Agreement == true ? "1" : "0");
            if (feasibility.b_Confidential_Agreement == true)
            {
                hdnAgreementFileEnabled.Value = "Yes";
            }
            else
            {
                hdnAgreementFileEnabled.Value = "No";
            }

            if (feasibility.i_Feasibility_Status_ID == 0) ddlCurrentStatus.SelectedValue = "-1";
            else ddlCurrentStatus.SelectedValue = feasibility.i_Feasibility_Status_ID.ToString();

            if (feasibility.b_Feasibility_Outcome == null) ddlFeasibilityOutcome.SelectedValue = "-1";
            else ddlFeasibilityOutcome.SelectedValue = (feasibility.b_Feasibility_Outcome == true ? "1" : "0");

            if (feasibility.s_In_File == null) ddlInFile.SelectedValue = "-1";
            else ddlInFile.SelectedValue = (feasibility.s_In_File == true ? "1" : "0");

            ddlInterest.SelectedValue = feasibility.b_Interest == true ? "1" : "0";
            /*Download file links*/

            //Checklist link
            if (feasibility.s_Checklist_File != null && feasibility.s_Checklist_File != "")
            {
                DownChecklist.Visible = true;

                hdnChecklist.Value = feasibility.s_Checklist_File;

                string[] arr = feasibility.s_Checklist_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownChecklist.Text = fName;

            }

            // Agreement file
            if (feasibility.s_Confidential_Agreement_File != null && feasibility.s_Confidential_Agreement_File != "")
            {
                DownAgreementFile.Visible = true;

                hdnAgreementFile.Value = feasibility.s_Confidential_Agreement_File;

                string[] arr = feasibility.s_Confidential_Agreement_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownAgreementFile.Text = fName;

            }

            // Protocol file
            if (feasibility.s_Prototcol_File != null && feasibility.s_Prototcol_File != "")
            {
                DownProtocolFile.Visible = true;

                hdnProtocolFile.Value = feasibility.s_Prototcol_File;

                string[] arr = feasibility.s_Prototcol_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownProtocolFile.Text = fName;

            }

            // Questionaire file
            if (feasibility.s_Questionnaire_File != null && feasibility.s_Questionnaire_File != "")
            {
                DownQuestFile.Visible = true;

                hdnQuestFile.Value = feasibility.s_Questionnaire_File;

                string[] arr = feasibility.s_Questionnaire_File.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                DownQuestFile.Text = fName;

            }

            CallJS("DisableAllControl();");
        }

        protected void backToGrid_Click(object sender, EventArgs e)
        {

        }

        protected void btnSaveNewPI_Click(object sender, EventArgs e)
        {
            try
            {

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                TTSHWCFReference.PI_Master pi = new PI_Master();

                pi.i_Dept_ID = Convert.ToInt32(HdnNewDeptId.Value);

                pi.s_Email = txtNewPIEmail.Text;

                pi.s_Firstname = txtNewPIFName.Text;

                pi.s_Lastname = txtNewPILName.Text;

                pi.s_MCR_No = txtNewPIMCR.Text;

                pi.s_Phone_no = txtNewPIPhone.Text;

                string res = client.PI_Master(pi, Mode.Insert.ToString());

                var retMsg = res.Split('|')[1];
                if (retMsg != "")
                {
                    int val;

                    if (!int.TryParse(retMsg, out val))
                        CallJS("MessageBox('" + retMsg + "');ApplyAutoComplete();");
                    else
                        CallJS("$('.newPI').click();ApplyAutoComplete();MessageBox('PI Details saved successfully')");
                }
                else
                {
                    CallJS("$('.newPI').click();ApplyAutoComplete();MessageBox('PI Details saved successfully')");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnUpdateFeasibility_Click1(object sender, EventArgs e)
        {
            /*File UpLoadNew */

            //For Agreement file
            if (fuAgreementFile.HasFile && hdnAgreementFileEnabled.Value == "Yes")
            {
                string[] arr = Common.UpLoadNew(fuAgreementFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnAgreementFile.Value = arr[0];
                }

            }

            //For Checklist file
            if (fuChecklist.HasFile)
            {
                string[] arr = Common.UpLoadNew(fuChecklist, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnChecklist.Value = arr[0];
                }

            }

            //For Protocol file
            if (fuProtocolFile.HasFile && hdnProtocolFileEnabled.Value == "Yes")
            {
                string[] arr = Common.UpLoadNew(fuProtocolFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnProtocolFile.Value = arr[0];
                }

            }

            //For Protocol file
            if (fuQuestFile.HasFile)
            {
                string[] arr = Common.UpLoadNew(fuQuestFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnQuestFile.Value = arr[0];
                }

            }
            /*File UpLoadNew */

            TTSHWCFReference.Feasibility_Details feasibility = new Feasibility_Details();

            feasibility.b_Confidential_Agreement = (ddlConfidentialAgreement.SelectedValue == "-1" || ddlConfidentialAgreement.SelectedValue == "0" ? false : true);

            if (ddlFeasibilityOutcome.SelectedValue == "-1") feasibility.b_Feasibility_Outcome = null;
            else feasibility.b_Feasibility_Outcome = (ddlFeasibilityOutcome.SelectedValue == "0" ? false : true);

            feasibility.b_Interest = (ddlInterest.SelectedValue == "-1" || ddlInterest.SelectedValue == "0" ? false : true);

            if (txtFeasibilityStartDate.Text.Trim() == "")
                feasibility.dt_Feasibility_Start_Date = null;
            else
                feasibility.dt_Feasibility_Start_Date = Convert.ToDateTime(txtFeasibilityStartDate.Text);

            if (txtDateUpdated.Text.Trim() == "")
                feasibility.dt_Modify_Date = null;
            else
                feasibility.dt_Modify_Date = Convert.ToDateTime(txtDateUpdated.Text);

            if (txtProtocolDate.Text.Trim() == "")
                feasibility.dt_Protocol_Date = null;
            else
                feasibility.dt_Protocol_Date = Convert.ToDateTime(txtProtocolDate.Text);

            if (txtSiteVisitDate.Text.Trim() == "")
                feasibility.dt_Site_Visit_Date = null;
            else
                feasibility.dt_Site_Visit_Date = Convert.ToDateTime(txtSiteVisitDate.Text);

            if (txtSurveyDate.Text.Trim() == "")
                feasibility.dt_Survey_Date = null;
            else
                feasibility.dt_Survey_Date = Convert.ToDateTime(txtSurveyDate.Text);

            if (txtDateInitialEmailSent.Text.Trim() == "")
                feasibility.s_Email_Send_Date = null;
            else
                feasibility.s_Email_Send_Date = Convert.ToDateTime(txtDateInitialEmailSent.Text);

            feasibility.s_Feasibility_Title = txtFeasibilityTitle.Text;

            feasibility.s_IM_Invitation = txtIMInvitation.Text;

            if (ddlInFile.SelectedValue == "-1") feasibility.s_In_File = null;
            else feasibility.s_In_File = (ddlInFile.SelectedValue == "0" ? false : true);

            feasibility.s_Interest_Comments = txtInterestComment.Text;

            feasibility.s_ModifyBy_ID = txtLastUpdatedBy.Text;

            feasibility.s_Protocol_No = txtProtocolNumber.Text;

            feasibility.s_Prototcol_Doc_No = txtProtocolDocNo.Text;

            feasibility.s_Protocol_Comments = txtProtocolComments.Text;

            feasibility.s_Survey_Comments = txtSurveyComments.Text;

            feasibility.CRA = hdnCROID.Value;

            feasibility.SPONSOR = hdnSponsorID.Value;

            feasibility.s_Short_Title = txtShortTitle.Text;

            feasibility.s_Project_Alias1 = txtAlias1.Text;

            feasibility.s_Project_Alias2 = txtAlias2.Text;


            feasibility.i_Feasibility_Status_ID = (ddlCurrentStatus.SelectedValue == "-1") ? -1 : Convert.ToInt32(ddlCurrentStatus.SelectedValue);

            feasibility.i_ID = Convert.ToInt32(Session["FeasibilityID"]);

            feasibility.i_Project_ID = Convert.ToInt32(Session["ProjectID"]);

            feasibility.s_Coinvestigator = txtCo_Investigator.Text;

            if (hdnAgreementFile.Value != null && hdnAgreementFile.Value != "")
                feasibility.s_Confidential_Agreement_File = hdnAgreementFile.Value;

            if (hdnChecklist.Value != null && hdnChecklist.Value != "")
                feasibility.s_Checklist_File = hdnChecklist.Value;

            if (hdnProtocolFile.Value != null && hdnProtocolFile.Value != "")
                feasibility.s_Prototcol_File = hdnProtocolFile.Value;

            if (hdnQuestFile.Value != null && hdnQuestFile.Value != "")
                feasibility.s_Questionnaire_File = hdnQuestFile.Value;

            List<Project_PI> PIs = new List<Project_PI>();
            string[] splitPiId = HdnPi_ID.Value.Split(',');
            foreach (string pi in splitPiId)
            {
                PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = Convert.ToInt32(Session["ProjectID"]) });
            }

            feasibility.Project_PIs = PIs.ToArray();

            feasibility.UserCId = LoginUserId;

            feasibility.Username = LoginUser;

            feasibility.s_ModifyBy_Name = LoginUser;


            TTSHWCFServiceClient client = new TTSHWCFServiceClient();
            client.Feasibility_Details(feasibility, Mode.Update);

            projectGrid.Visible = true;
            FeasibilityContainer.Visible = false;

            //CallJS("MessageBoxEvent('Feasibility updated successsfully','PerformCancel();');");
            CallJS("MessageBox('Feasibility updated successsfully');");
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            FillGrid();
        }

        protected void btnSaveFeasibility_Click(object sender, EventArgs e)
        {
            //For Agreement file
            if (fuAgreementFile.HasFile && hdnAgreementFileEnabled.Value == "Yes")
            {
                string[] arr = Common.UpLoadNew(fuAgreementFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnAgreementFile.Value = arr[0];
                }

            }

            //For Checklist file
            if (fuChecklist.HasFile)
            {
                string[] arr = Common.UpLoadNew(fuChecklist, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnChecklist.Value = arr[0];
                }

            }

            //For Protocol file
            if (fuProtocolFile.HasFile && hdnProtocolFileEnabled.Value == "Yes")
            {
                string[] arr = Common.UpLoadNew(fuProtocolFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnProtocolFile.Value = arr[0];
                }

            }

            //For Protocol file
            if (fuQuestFile.HasFile)
            {
                string[] arr = Common.UpLoadNew(fuQuestFile, Common.FolderLocation.FeasibilityFiles);

                if (arr.Count() != 0)
                {
                    hdnQuestFile.Value = arr[0];
                }

            }
            /*File UpLoadNew */

            TTSHWCFReference.Feasibility_Details feasibility = new Feasibility_Details();

            feasibility.b_Confidential_Agreement = (ddlConfidentialAgreement.SelectedValue == "-1" || ddlConfidentialAgreement.SelectedValue == "0" ? false : true);

            if (ddlFeasibilityOutcome.SelectedValue == "-1") feasibility.b_Feasibility_Outcome = null;
            else feasibility.b_Feasibility_Outcome = (ddlFeasibilityOutcome.SelectedValue == "0" ? false : true);

            feasibility.b_Interest = (ddlInterest.SelectedValue == "-1" || ddlInterest.SelectedValue == "0" ? false : true);

            if (txtFeasibilityStartDate.Text.Trim() == "")
                feasibility.dt_Feasibility_Start_Date = null;
            else
                feasibility.dt_Feasibility_Start_Date = Convert.ToDateTime(txtFeasibilityStartDate.Text);

            if (txtDateUpdated.Text.Trim() == "")
                feasibility.dt_Modify_Date = DateTime.Now;
            else
                feasibility.dt_Modify_Date = Convert.ToDateTime(txtDateUpdated.Text);

            if (txtProtocolDate.Text.Trim() == "")
                feasibility.dt_Protocol_Date = null;
            else
                feasibility.dt_Protocol_Date = Convert.ToDateTime(txtProtocolDate.Text);

            if (txtSiteVisitDate.Text.Trim() == "")
                feasibility.dt_Site_Visit_Date = null;
            else
                feasibility.dt_Site_Visit_Date = Convert.ToDateTime(txtSiteVisitDate.Text);

            if (txtSurveyDate.Text.Trim() == "")
                feasibility.dt_Survey_Date = null;
            else
                feasibility.dt_Survey_Date = Convert.ToDateTime(txtSurveyDate.Text);

            if (txtDateInitialEmailSent.Text.Trim() == "")
                feasibility.s_Email_Send_Date = null;
            else
                feasibility.s_Email_Send_Date = Convert.ToDateTime(txtDateInitialEmailSent.Text);

            feasibility.s_Feasibility_Title = txtFeasibilityTitle.Text;

            feasibility.s_IM_Invitation = txtIMInvitation.Text;

            if (ddlInFile.SelectedValue == "-1") feasibility.s_In_File = null;
            else feasibility.s_In_File = (ddlInFile.SelectedValue == "0" ? false : true);

            feasibility.s_Interest_Comments = txtInterestComment.Text;

            feasibility.s_ModifyBy_ID = txtLastUpdatedBy.Text;

            feasibility.s_Protocol_No = txtProtocolNumber.Text;

            feasibility.s_Prototcol_Doc_No = txtProtocolDocNo.Text;

            feasibility.s_Protocol_Comments = txtProtocolComments.Text;

            feasibility.s_Survey_Comments = txtSurveyComments.Text;

            feasibility.CRA = hdnCROID.Value;

            feasibility.SPONSOR = hdnSponsorID.Value;

            feasibility.s_Short_Title = txtShortTitle.Text;

            feasibility.s_Project_Alias1 = txtAlias1.Text;

            feasibility.s_Project_Alias2 = txtAlias2.Text;


            feasibility.i_Feasibility_Status_ID = (ddlCurrentStatus.SelectedValue == "-1") ? -1 : Convert.ToInt32(ddlCurrentStatus.SelectedValue);

            feasibility.i_ID = Convert.ToInt32(Session["FeasibilityID"]);

            feasibility.i_Project_ID = Convert.ToInt32(Session["ProjectID"]);

            feasibility.s_Coinvestigator = txtCo_Investigator.Text;

            if (hdnAgreementFile.Value != null && hdnAgreementFile.Value != "")
                feasibility.s_Confidential_Agreement_File = hdnAgreementFile.Value;

            if (hdnChecklist.Value != null && hdnChecklist.Value != "")
                feasibility.s_Checklist_File = hdnChecklist.Value;

            if (hdnProtocolFile.Value != null && hdnProtocolFile.Value != "")
                feasibility.s_Prototcol_File = hdnProtocolFile.Value;

            if (hdnQuestFile.Value != null && hdnQuestFile.Value != "")
                feasibility.s_Questionnaire_File = hdnQuestFile.Value;

            List<Project_PI> PIs = new List<Project_PI>();
            string[] splitPiId = HdnPi_ID.Value.Split(',');
            foreach (string pi in splitPiId)
            {
                PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = Convert.ToInt32(Session["ProjectID"]) });
            }

            feasibility.Project_PIs = PIs.ToArray();

            feasibility.UserCId = LoginUserId;

            feasibility.Username = LoginUser;

            feasibility.s_ModifyBy_Name = LoginUser;


            TTSHWCFServiceClient client = new TTSHWCFServiceClient();
            client.Feasibility_Details(feasibility, Mode.Insert);

            projectGrid.Visible = true;

            FeasibilityContainer.Visible = false;



            CallJS("MessageBox('Feasibility saved successfully');");
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            FillGrid();
        }

        protected void btnCancelFeasibility_Click(object sender, EventArgs e)
        {
            projectGrid.Visible = true;
            FeasibilityContainer.Visible = false;
        }

        public void ResetControls()
        {
            //CallJS("ClearAll('new')");

            foreach (Control ctrl in txtAlias1.Parent.Controls)
            {

                if (ctrl.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)ctrl;
                    txt.Text = "";
                }

                if (ctrl.GetType() == typeof(DropDownList))
                {
                    DropDownList ddl = (DropDownList)ctrl;
                    ddl.ClearSelection();
                }


            }


            //To reset the hidden fields
            hdnAgreementFile.Value = "";
            hdnChecklist.Value = "";
            hdnCROID.Value = "";
            HdnDeptId.Value = "";
            HdnNewDeptId.Value = "";
            HdnPi_ID.Value = "";
            HdnpiId.Value = "";
            hdnProtocolFile.Value = "";
            hdnQuestFile.Value = "";
            hdnSponsorID.Value = "";

            //To reset the downloasd buttons
            DownAgreementFile.Visible = false;
            DownChecklist.Visible = false;
            DownProtocolFile.Visible = false;
            DownQuestFile.Visible = false;



        }

        public void PopulateDropDown()
        {

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlCurrentStatus, DropDownName.Fesibility_Status, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlProjectCategory, DropDownName.Project_Category, "");

        }

        public void CallJS(string script, string Name = "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Name, script, true);
        }

        protected void backToGrid_Click1(object sender, EventArgs e)
        {
            //TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            //t.Text = "";
            FeasibilityContainer.Visible = false;
            projectGrid.Visible = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }

        protected void btnSaveSponsor_Click(object sender, EventArgs e)
        {
            TTSHWCFReference.Sponsor_Master sponsor = new Sponsor_Master();

            sponsor.s_Name = txtNewSponsorName.Text;
            sponsor.s_CreatedBy_ID = "";

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            string res = client.Sponsor(sponsor);

            if (res.Split('|')[1] == "Exists")
            {
                CallJS("MessageBox('Sponsor with same name already exists');");
            }
            else
                CallJS("MessageBox('Sponsor saved successfully'); $('[id*=btnCancelSponsor]').click();");
        }

        #region Web Methods
        [WebMethod]
        [ScriptMethod]
        public static string[] GetText(string Prefix, int count, string ContextKey)
        {
            TTSHWCFServiceClient sc = new TTSHWCFServiceClient();
            List<string> lst = new List<string>();
            lst.AddRange(sc.GetText(Prefix, count, ContextKey));
            return lst.ToArray();
        }
        #endregion

        protected void btnSaveCRO_Click(object sender, EventArgs e)
        {
            TTSHWCFReference.CRO_Master cro = new CRO_Master();

            cro.s_Name = txtNewCRO.Text;
            cro.s_CreatedBy_ID = "";

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            string res = client.CRO(cro);

            if (res.Split('|')[1] == "Exists")
            {
                CallJS("MessageBox('CRO with same name already exists');");
            }
            else
                CallJS("MessageBox('CRO saved successfully'); $('[id*=btnCancelCRO]').click();");
        }

        protected void btnUpdateFeasibility_Click2(object sender, EventArgs e)
        {

        }

        protected void DownAgreementFile_Click2(object sender, EventArgs e)
        {
            Common.DownloadFileNew(hdnAgreementFile.Value, Response);
            CallJS("hideLoading();");
        }

        protected void DownProtocolFile_Click(object sender, EventArgs e)
        {
            Common.DownloadFileNew(hdnProtocolFile.Value, Response);
            CallJS("hideLoading();");
        }

        protected void DownQuestFile_Click(object sender, EventArgs e)
        {
            Common.DownloadFileNew(hdnQuestFile.Value, Response);
            CallJS("hideLoading();");
        }

        protected void DownChecklist_Click(object sender, EventArgs e)
        {
            Common.DownloadFileNew(hdnChecklist.Value, Response);
            CallJS("hideLoading();");
        }

        protected void btnAddNewProject_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmProject_Master.aspx?NewPage=1");
            //string url = ResolveUrl("frmProject_Master.aspx");
            //CallJS("window.open('" + url + "?NewPage=1','_blank')");
        }

        protected void SearchBox_ButtonClick(object sender, EventArgs e)
        {
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text; /*alternate way of getting search value */

            if (string.IsNullOrEmpty(SearchBox.ErrorString)) /* errors will be available in ErrorString property*/
            {
                TTSHWCFReference.Search[] lst = SearchBox.SearchOutput; /*to get list of output*/

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();
                client.Open();
                try
                {


                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("FEASIBILITY", UserID);
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
                            oNewGrid = oOldSearch.Where(z => z.iRecordExists == 0).Where(z => oDOList.Any(x => x.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
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

                            rptrProjectDetail.DataSource = oOldSearch; /*use the object according to your need*/
                            rptrProjectDetail.DataBind();
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
                            }
                            rptrProjectDetail.DataSource = lst; /*use the object according to your need*/
                            rptrProjectDetail.DataBind();
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

                        rptrProjectDetail.DataSource = oOldSearch; /*use the object according to your need*/
                        rptrProjectDetail.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                client.Close();
                //rptrProjectDetail.DataSource = lst; /*use the object according to your need*/
                //rptrProjectDetail.DataBind();

            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("s_Display_Project_ID");
                dt.Columns.Add("s_Project_Title");
                dt.Columns.Add("s_Project_Category");
                dt.Columns.Add("Feasibility_Status_Name");
                dt.Columns.Add("s_IRB_No");
                dt.Columns.Add("PI_Names");
                dt.Columns.Add("i_Project_ID");
                dt.Columns.Add("i_Feasibility_ID");
                dt.Columns.Add("Status");

                rptrProjectDetail.DataSource = dt; /*use the object according to your need*/
                rptrProjectDetail.DataBind();

                //FillGrid();
            }

        }

        protected void SearchBox_ClearClick(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("DeleteFeasibility", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Feasibility Details Deleted Successfully..!!");
                    //ShowPanel();
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message.ToString());
            }
        }

    }
}