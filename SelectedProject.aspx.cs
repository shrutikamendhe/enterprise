using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TTSHWeb.TTSHWCFReference;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;
using System.Web.UI.HtmlControls;

namespace TTSHWeb
{
    public partial class SelectedProject : System.Web.UI.Page
    {
        string LoginUser = "";
        string LoginUserId = "";
        bool isSelectedTeamUser = false;
        bool isBlindedUser = false;
        bool isUnblinded = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            Contract_Details cd = new Contract_Details();

            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.SELECTED; /*setting filtercriteria*/
            SearchBox.ButtonSearchClick += new EventHandler(SearchBox_ButtonClick); /*adding events*/
            SearchBox.ButtonClearClick += new EventHandler(SearchBox_ClearClick); /*adding events*/
            //Code to assign user id ,user name and user groups

            if (HttpContext.Current.Session["UserGroups"] != null)
            {
                if (HttpContext.Current.Session["UserGroups"].ToString().Contains("TProjectMonitoring"))
                {
                    isSelectedTeamUser = true;
                }
                else if (HttpContext.Current.Session["UserGroups"].ToString().Contains("TAdmin"))
                {
                    isSelectedTeamUser = true;
                    //isBlindedUser = true;
                    //isUnblinded = true;

                }
                else
                {

                }
            }

            //==Temp Code
            //isSelectedTeamUser = true;
            //LoginUserId = "5911B068-5137-4DC1-BEAE-BF6C74E4FD00";

            if (HttpContext.Current.Session["UserName"] != null)
            {
                LoginUser = HttpContext.Current.Session["UserName"].ToString();

            }
            if (HttpContext.Current.Session["UserID"] != null)
            {
                LoginUserId = HttpContext.Current.Session["UserID"].ToString();

            }

            if (!IsPostBack)
            {
                TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                t.Text = "";
                FillGrid();

            }
        }

        protected void ViewLink_Command(object sender, CommandEventArgs e)
        {

            string[] ids = e.CommandArgument.ToString().Split(',');

            int projectID = Convert.ToInt32(ids[0]);

            hdnDisplayMode.Value = "View";

            bProjectID.InnerText = "Project ID: " + ids[1];

            HdnProjectID.Value = Convert.ToString(projectID);

            monthButton_Container.InnerHtml = "";

            //Hide all the panels/Container first
            HideAllContainer();

            projectGrid.Visible = false;

            //To reset all the controls
            ResetControls();

            SelectedContainer.Visible = true;

            PopulateDropDown();

            btnUpdateSelected.Visible = true;

            btnSaveSelected.Visible = false;

            //To disable Selected Start date and Last Updated by fields
            txtDateUpdated.Enabled = false;
            txtUpdatedBy.Enabled = false;
            txtSelectedStartDate.Enabled = false;


            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            selected = client.GetSelected_Project_DetailsByID(projectID, "", "");

            hdnSelectedID.Value = Convert.ToString(selected.i_ID);



            //====================================================
            List<string> backup_Blinded = new List<string>();
            List<string> backup_UnBlinded = new List<string>();

            string blinded_Unblined = selected.BLINDED_UNBLINDED_XML;

            //Parse xml and bind project details
            if (blinded_Unblined != string.Empty && blinded_Unblined != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(blinded_Unblined)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/BU/BLINDED_UNBLINDED");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        if (node["Blinded_Unblinded"].InnerText == "U")
                        {
                            backup_UnBlinded.Add(node["i_Cordinator_Id"].InnerText);
                        }
                        else
                        {
                            backup_Blinded.Add(node["i_Cordinator_Id"].InnerText);
                        }
                    }
                }
            }

            string Co_Ordinator_Type = "";

            string Blinded = "";

            string UnBlinded = "";

            if (isSelectedTeamUser)
            {
                if (selected.s_Blinded_Coordinator != "0" || backup_Blinded.Count != 0)
                {
                    Blinded = "B";
                }
                if (selected.s_Unblinded_Coordinator != "0" || backup_UnBlinded.Count != 0)
                {
                    UnBlinded = "U";
                }
                Co_Ordinator_Type = Blinded + UnBlinded;
            }
            else
            {

                if (selected.s_Blinded_Coordinator == LoginUserId || backup_Blinded.Contains(LoginUserId))
                {
                    Blinded = "B";
                }
                if (selected.s_Unblinded_Coordinator == LoginUserId || backup_UnBlinded.Contains(LoginUserId))
                {
                    UnBlinded = "U";
                }
                Co_Ordinator_Type = Blinded + UnBlinded;

            }

            //string Co_Ordinator_Type = selected.Co_Ordinator_Type;

            //===================================================================================================

            //Show/ Hide the Content as per login user 
            if (isSelectedTeamUser)
            {
                cordinatorContainer.Visible = true;


                if (selected.i_Notification_Mode != 0 && (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "B"))
                {
                    //BlindedContainer.Visible = true;
                    //MonthlyDeatailContainer.Visible = true;
                    isBlindedUser = true;
                }

                if (selected.s_Drug_Name != "" && selected.s_Drug_Name != null && (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "U"))
                {
                    //UnBlindedContainer.Visible = true;
                    //MonthlyDeatailContainer.Visible = true;
                    isUnblinded = true;
                }
            }
            else
            {
                if (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "B")
                {
                    isBlindedUser = true;
                }
                if (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "U")
                {
                    isUnblinded = true;
                }

            }

            if (isBlindedUser)
            {
                BlindedSetting();
            }
            if (isUnblinded)
            {
                UnblindedSetting();
            }
            else
            {
                FillYearDDL();
            }

            if (selected.monthNames == "")
            {
                string currYear = DateTime.Now.Year.ToString();
                ddlYear.SelectedValue = currYear;
            }
            else
            {
                string year = Convert.ToDateTime(selected.dt_EntryForMonthBlinded).Year.ToString();

                ddlYear.SelectedValue = year;

                if (selected.monthNames != "" && selected.monthNames != null)
                {
                    AppendMonth(selected.monthNames);
                }

            }

            //Bind project details
            string projectXML = selected.Project_Data;

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


                    }
                }
            }

            //Bind Project PIs
            if (selected.DEPT_PI != null)
                FillPIGrid(selected.DEPT_PI.ToList());

            //Fill Co-ordinator details
            ddlTeamNeeded.SelectedValue = (selected.b_IsTeam_Needed ? "1" : "0");

            txtSelectedStartDate.Text = selected.dt_Selected_Start_Date == DateTime.MinValue || selected.dt_Selected_Start_Date == null ? "" : Convert.ToDateTime(selected.dt_Selected_Start_Date).ToString("dd-MMM-yy");

            ddlBlindedCordinator.SelectedValue = selected.s_Blinded_Coordinator == "0" ? "-1" : selected.s_Blinded_Coordinator;

            ddlUnBlindedCordinator.SelectedValue = selected.s_Unblinded_Coordinator == "0" ? "-1" : selected.s_Unblinded_Coordinator;

            //List<string> backup_Blinded = new List<string>();
            //List<string> backup_UnBlinded = new List<string>();

            //string blinded_Unblined = selected.BLINDED_UNBLINDED_XML;

            ////Parse xml and bind project details
            //if (blinded_Unblined != string.Empty && blinded_Unblined != null)
            //{
            //    using (XmlReader reader = XmlReader.Create(new StringReader(blinded_Unblined)))
            //    {
            //        XmlDocument xml = new XmlDocument();

            //        xml.Load(reader);

            //        XmlNodeList xmlNodeList = xml.SelectNodes("/BU/BLINDED_UNBLINDED");

            //        foreach (XmlNode node in xmlNodeList)
            //        {

            //            if (node["Blinded_Unblinded"].InnerText == "U")
            //            {
            //                backup_UnBlinded.Add(node["i_Cordinator_Id"].InnerText);
            //            }
            //            else
            //            {
            //                backup_Blinded.Add(node["i_Cordinator_Id"].InnerText);
            //            }
            //        }
            //    }
            //}

            string displayNameBlinded = "";
            string displayNameUnBlinded = "";

            int count = 0;
            foreach (ListItem item in chkboxlistBlinded.Items)
            {

                if (backup_Blinded.Contains(item.Value))
                {
                    item.Selected = true;
                    if (count == 0)
                        displayNameBlinded += item.Text;
                    else
                        displayNameBlinded += ", " + item.Text;
                    count++;
                }
            }

            count = 0;
            foreach (ListItem item in chkboxlistUnBlinded.Items)
            {

                if (backup_UnBlinded.Contains(item.Value))
                {
                    item.Selected = true;
                    if (count == 0)
                        displayNameUnBlinded += item.Text;
                    else
                        displayNameUnBlinded += ", " + item.Text;
                    count++;
                }
            }
            if (displayNameBlinded != "")
            {
                SearchBlinded.Text = displayNameBlinded;
            }

            if (displayNameUnBlinded != "")
            {
                SearchUnBlinded.Text = displayNameUnBlinded;
            }

            txtUpdatedBy.Text = selected.s_ModifyBy_Name;

            txtDateUpdated.Text = (selected.dt_Modify_Date == DateTime.MinValue || selected.dt_Modify_Date == null ? "" : Convert.ToDateTime(selected.dt_Modify_Date).ToString("dd-MMM-yy"));

            //CallJS("DisableAllControl(1);");

            FillMonthlyDetails(selected);



            return;




        }

        protected void EditLink_Command(object sender, CommandEventArgs e)
        {
            string[] ids = e.CommandArgument.ToString().Split(',');
            int projectID = Convert.ToInt32(ids[0]);

            hdnDisplayMode.Value = "Edit";

            bProjectID.InnerText = "Project ID: " + ids[1];

            HdnProjectID.Value = Convert.ToString(projectID);

            monthButton_Container.InnerHtml = "";

            //Hide all the panels/Container first
            HideAllContainer();

            projectGrid.Visible = false;

            //To reset all the controls
            ResetControls();

            SelectedContainer.Visible = true;

            PopulateDropDown();

            btnUpdateSelected.Visible = true;

            btnSaveSelected.Visible = false;

            //To disable Selected Start date and Last Updated by fields
            txtDateUpdated.Enabled = false;
            txtUpdatedBy.Enabled = false;
            txtSelectedStartDate.Enabled = false;
            txtUpdatedByUnblinded.Enabled = false;
            txtLastUpdatedUnBlinded.Enabled = false;


            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            selected = client.GetSelected_Project_DetailsByID(projectID, "", "");

            hdnSelectedID.Value = Convert.ToString(selected.i_ID);



            //====================================================
            List<string> backup_Blinded = new List<string>();
            List<string> backup_UnBlinded = new List<string>();

            string blinded_Unblined = selected.BLINDED_UNBLINDED_XML;

            //Parse xml and bind project details
            if (blinded_Unblined != string.Empty && blinded_Unblined != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(blinded_Unblined)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/BU/BLINDED_UNBLINDED");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        if (node["Blinded_Unblinded"].InnerText == "U")
                        {
                            backup_UnBlinded.Add(node["i_Cordinator_Id"].InnerText);
                        }
                        else
                        {
                            backup_Blinded.Add(node["i_Cordinator_Id"].InnerText);
                        }
                    }
                }
            }

            string Co_Ordinator_Type = "";

            string Blinded = "";

            string UnBlinded = "";

            hdnIsSelectedUser.Value = "";

            if (isSelectedTeamUser)
            {
                if (selected.s_Blinded_Coordinator != "0" || backup_Blinded.Count != 0)
                {
                    Blinded = "B";
                }
                if (selected.s_Unblinded_Coordinator != "0" || backup_UnBlinded.Count != 0)
                {
                    UnBlinded = "U";
                }
                Co_Ordinator_Type = Blinded + UnBlinded;
            }
            else
            {

                if (selected.s_Blinded_Coordinator == LoginUserId || backup_Blinded.Contains(LoginUserId))
                {
                    Blinded = "B";
                }
                if (selected.s_Unblinded_Coordinator == LoginUserId || backup_UnBlinded.Contains(LoginUserId))
                {
                    UnBlinded = "U";
                }
                Co_Ordinator_Type = Blinded + UnBlinded;

            }

            //string Co_Ordinator_Type = selected.Co_Ordinator_Type;

            //===================================================================================================

            //Show/ Hide the Content as per login user 
            if (isSelectedTeamUser)
            {
                cordinatorContainer.Visible = true;


                if (selected.i_Notification_Mode != -1 && selected.i_Notification_Mode != 0 && (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "B"))
                {
                    //BlindedContainer.Visible = true;
                    //MonthlyDeatailContainer.Visible = true;
                    isBlindedUser = true;
                }

                if (selected.s_Drug_Name != "" && selected.s_Drug_Name != null && (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "U"))
                {
                    //UnBlindedContainer.Visible = true;
                    //MonthlyDeatailContainer.Visible = true;
                    isUnblinded = true;
                }

                if (selected.s_Blinded_Coordinator == LoginUserId || backup_Blinded.Contains(LoginUserId))
                {
                    isBlindedUser = true;
                }
                if (selected.s_Unblinded_Coordinator == LoginUserId || backup_UnBlinded.Contains(LoginUserId))
                {
                    isUnblinded = true;
                }

                if (isUnblinded) isBlindedUser = true;
                if (isBlindedUser) isUnblinded = true;

                hdnIsSelectedUser.Value = "Yes";
            }
            else
            {
                if (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "B")
                {
                    isBlindedUser = true;
                }
                if (Co_Ordinator_Type == "BU" || Co_Ordinator_Type == "U")
                {
                    isUnblinded = true;
                }

            }

            if (isBlindedUser)
            {
                BlindedSetting();
            }
            if (isUnblinded)
            {
                UnblindedSetting();
            }
            else
            {
                FillYearDDL();
            }

            if (selected.monthNames == "")
            {
                string currYear = DateTime.Now.Year.ToString();
                ddlYear.SelectedValue = currYear;
            }
            else
            {

                if (selected.dt_EntryForMonthBlinded == null)
                    return;


                string year = Convert.ToDateTime(selected.dt_EntryForMonthBlinded).Year.ToString();

                ddlYear.SelectedValue = year;

                if (selected.monthNames != "" && selected.monthNames != null)
                {
                    AppendMonth(selected.monthNames);
                }

            }

            //Bind project details
            string projectXML = selected.Project_Data;

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


                    }
                }
            }

            //Bind Project PIs
            if (selected.DEPT_PI != null)
                FillPIGrid(selected.DEPT_PI.ToList());

            //Fill Co-ordinator details
            ddlTeamNeeded.SelectedValue = (selected.b_IsTeam_Needed ? "1" : "0");

            txtSelectedStartDate.Text = selected.dt_Selected_Start_Date == DateTime.MinValue || selected.dt_Selected_Start_Date == null ? "" : Convert.ToDateTime(selected.dt_Selected_Start_Date).ToString("dd-MMM-yy");

            ddlBlindedCordinator.SelectedValue = selected.s_Blinded_Coordinator == "0" ? "-1" : selected.s_Blinded_Coordinator;

            ddlUnBlindedCordinator.SelectedValue = selected.s_Unblinded_Coordinator == "0" ? "-1" : selected.s_Unblinded_Coordinator;

            //List<string> backup_Blinded = new List<string>();
            //List<string> backup_UnBlinded = new List<string>();

            //string blinded_Unblined = selected.BLINDED_UNBLINDED_XML;

            ////Parse xml and bind project details
            //if (blinded_Unblined != string.Empty && blinded_Unblined != null)
            //{
            //    using (XmlReader reader = XmlReader.Create(new StringReader(blinded_Unblined)))
            //    {
            //        XmlDocument xml = new XmlDocument();

            //        xml.Load(reader);

            //        XmlNodeList xmlNodeList = xml.SelectNodes("/BU/BLINDED_UNBLINDED");

            //        foreach (XmlNode node in xmlNodeList)
            //        {

            //            if (node["Blinded_Unblinded"].InnerText == "U")
            //            {
            //                backup_UnBlinded.Add(node["i_Cordinator_Id"].InnerText);
            //            }
            //            else
            //            {
            //                backup_Blinded.Add(node["i_Cordinator_Id"].InnerText);
            //            }
            //        }
            //    }
            //}

            string displayNameBlinded = "";
            string displayNameUnBlinded = "";

            int count = 0;
            foreach (ListItem item in chkboxlistBlinded.Items)
            {

                if (backup_Blinded.Contains(item.Value))
                {
                    item.Selected = true;
                    if (count == 0)
                        displayNameBlinded += item.Text;
                    else
                        displayNameBlinded += ", " + item.Text;
                    count++;
                }
            }

            count = 0;
            foreach (ListItem item in chkboxlistUnBlinded.Items)
            {

                if (backup_UnBlinded.Contains(item.Value))
                {
                    item.Selected = true;
                    if (count == 0)
                        displayNameUnBlinded += item.Text;
                    else
                        displayNameUnBlinded += ", " + item.Text;
                    count++;
                }
            }
            if (displayNameBlinded != "")
            {
                SearchBlinded.Text = displayNameBlinded;
            }

            if (displayNameUnBlinded != "")
            {
                SearchUnBlinded.Text = displayNameUnBlinded;
            }

            txtUpdatedBy.Text = selected.s_ModifyBy_Name;

            txtDateUpdated.Text = (selected.dt_Modify_Date == DateTime.MinValue || selected.dt_Modify_Date == null ? "" : Convert.ToDateTime(selected.dt_Modify_Date).ToString("dd-MMM-yy"));


            FillMonthlyDetails(selected);

            return;

            /*Blinded Details*/

            //--SAE Details
            ddlSAEStatus.SelectedValue = (selected.b_SAE_Status ? "1" : "0");

            if (selected.i_Notification_Mode != 0)
                ddlModeofNotification.SelectedValue = Convert.ToString(selected.i_Notification_Mode);

            txtPatientStudyNo.Text = selected.i_Patient_Studyno;

            ddlReadmission.SelectedValue = (selected.b_IsReadmission ? "1" : "0");

            txtReadmissionDate.Text = selected.dt_Readmission_date == DateTime.MinValue || selected.dt_Readmission_date == null ? "" : Convert.ToDateTime(selected.dt_Readmission_date).ToString("dd-MMM-yy");

            txtDischargeDate.Text = selected.dt_Discharge_date == DateTime.MinValue || selected.dt_Discharge_date == null ? "" : Convert.ToDateTime(selected.dt_Discharge_date).ToString("dd-MMM-yy");

            txtdtCordinatorsKnowledge.Text = selected.dt_Knowledge_date == DateTime.MinValue || selected.dt_Knowledge_date == null ? "" : Convert.ToDateTime(selected.dt_Knowledge_date).ToString("dd-MMM-yy");

            //--CRO CRA Details

            //Study Section 
            if (selected.i_Study_Status_ID != 0)
                ddlStudyStatus.SelectedValue = Convert.ToString(selected.i_Study_Status_ID);

            if (selected.i_Project_Type_ID != 0)
                ddlTypeofStudy.SelectedValue = Convert.ToString(selected.i_Project_Type_ID);

            ddlApprovedStudyBugdet.SelectedValue = (selected.b_IsApproveProject ? "1" : "0");

            //Budget file grid

            //Archiving - Section 
            ddlAwaitingArchiving.SelectedValue = (selected.b_Awaiting_Archiving ? "1" : "0");

            txtEndDateArchiving.Text = selected.dt_Archiving_Enddate == DateTime.MinValue || selected.dt_Archiving_Enddate == null ? "" : Convert.ToDateTime(selected.dt_Archiving_Enddate).ToString("dd-MMM-yy");

            txtReason.Text = selected.s_Reason;

            txtOffSiteCompany.Text = selected.s_Offsite_Company;

            if (selected.s_AgreementFile != null && selected.s_AgreementFile != "")
            {

                btnDownIRBFile.Visible = true;

                hdnIRBFile.Value = selected.s_AgreementFile;

                string[] arr = selected.s_AgreementFile.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                btnDownIRBFile.Text = fName;

                btnDownIRBFile.Attributes.Add("filepath", selected.s_AgreementFile);
                //hdnIRBFileEnabled.Value = "Yes";
            }

            //Other Details 
            txtClinic1.Text = selected.s_Clinic1;

            txtClinic2.Text = selected.s_Clinic2;

            txtClinicDaysResearch.Text = Convert.ToString(selected.s_Research_Days);

            txtDurationofFollowups.Text = selected.s_Followup_Duratrion;

            txtRecruitStartDate.Text = selected.dt_Recruit_Start_Date == DateTime.MinValue || selected.dt_Recruit_Start_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_Start_Date).ToString("dd-MMM-yy");

            txtRecruitEndDate.Text = selected.dt_Recruit_End_Date == DateTime.MinValue || selected.dt_Recruit_End_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_End_Date).ToString("dd-MMM-yy");

            txtTargetforTTSH.Text = Convert.ToString(selected.i_TTSH_Target);

            txtScreened.Text = Convert.ToString(selected.i_Screen_No);

            txtScreenFailure.Text = Convert.ToString(selected.i_Screen_Failure);

            txtRandomized.Text = Convert.ToString(selected.i_Randomized);

            txtCompleted.Text = Convert.ToString(selected.i_Completed);

            txtWithdrawal.Text = Convert.ToString(selected.i_Withdrawl);

            txtIRB.Text = Convert.ToString(selected.s_IRB_No);

            txtIRBExpiryDate.Text = selected.dt_Expiry_date == DateTime.MinValue || selected.dt_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Expiry_date).ToString("dd-MMM-yy");

            txtCTMExpiryDate.Text = selected.dt_CTM_Expiry_date == DateTime.MinValue || selected.dt_CTM_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTM_Expiry_date).ToString("dd-MMM-yy");

            txtCTCExpiryDate.Text = selected.dt_CTC_Expiry_date == DateTime.MinValue || selected.dt_CTC_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTC_Expiry_date).ToString("dd-MMM-yy");

            ddlCTMStatus.SelectedValue = (selected.b_CTM_Status ? "1" : "0");

            //Cupboard - Section (Blinded) 
            txtExpectedMonth.Text = selected.dt_Extended_Month_Blinded == DateTime.MinValue || selected.dt_Extended_Month_Blinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_Blinded).ToString("dd-MMM-yy");


            if (selected.s_LastUpdated_By_Blinded != "" && selected.s_LastUpdated_By_Blinded != null)
                txtUpdatedByBlinded.Text = selected.s_LastUpdated_By_Blinded;
            else
                txtUpdatedByBlinded.Text = LoginUser;

            if (selected.dt_LastUpdated_By_Blinded != null && selected.dt_LastUpdated_By_Blinded != DateTime.MinValue)
                txtLastUpdatedBlinded.Text = selected.dt_LastUpdated_By_Blinded == DateTime.MinValue || selected.dt_LastUpdated_By_Blinded == null ? "" : Convert.ToDateTime(selected.dt_LastUpdated_By_Blinded).ToString("dd-MMM-yy");
            else
                txtLastUpdatedBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");


            ddlCupboardNoBlinded.SelectedValue = selected.i_CupBoardno_Blinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_Blinded);

            /*End of Blinded Details*/

            /*UnBlinded Details*/

            //Drug Location - Section 
            txtDrugName.Text = selected.s_Drug_Name;

            txtDose.Text = selected.s_Drug_Dose;

            txtDateofExpiry.Text = selected.dt_Drug_Expiry_date == DateTime.MinValue || selected.dt_Drug_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Drug_Expiry_date).ToString("dd-MMM-yy");

            if (selected.i_Drug_Location_ID != 0)
                ddlLocation.SelectedValue = Convert.ToString(selected.i_Drug_Location_ID);

            //Cupboard - Section (Un Blinded) 
            txtExpectedMonthUnBlinded.Text = selected.dt_Extended_Month_UnBlinded == DateTime.MinValue || selected.dt_Extended_Month_UnBlinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_UnBlinded).ToString("dd-MMM-yy");

            if (selected.s_LastUpdated_By_UnBlinded != "" && selected.s_LastUpdated_By_UnBlinded != null)
                txtUpdatedByUnblinded.Text = selected.s_LastUpdated_By_UnBlinded;
            else
                txtUpdatedByUnblinded.Text = LoginUser;

            if (selected.dt_LastUpdated_By_UnBlinded != null && selected.dt_LastUpdated_By_UnBlinded != DateTime.MinValue)
                txtLastUpdatedUnBlinded.Text = selected.dt_LastUpdated_By_UnBlinded == DateTime.MinValue || selected.dt_LastUpdated_By_UnBlinded == null ? "" : Convert.ToDateTime(selected.dt_LastUpdated_By_UnBlinded).ToString("dd-MMM-yy");
            else
                txtLastUpdatedUnBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");

            ddlCupboardNoUnblinded.SelectedValue = selected.i_CupBoardno_UnBlinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_UnBlinded);

            /*End of UnBlinded Details*/

            //Common Fields
            txtEntryMonth.Text = selected.dt_EntryForMonthBlinded == DateTime.MinValue || selected.dt_EntryForMonthBlinded == null ? "" : Convert.ToDateTime(selected.dt_EntryForMonthBlinded).ToString("dd-MMM-yy");


        }

        private void HideAllContainer()
        {
            MonthlyDeatailContainer.Visible = false;

            BlindedContainer.Visible = false;

            ProjectRequirementContainer.Visible = false;

            UnBlindedContainer.Visible = false;

        }

        protected void NewLink_Command(object sender, CommandEventArgs e)
        {
            projectGrid.Visible = false;

            //To reset all the controls
            ResetControls();

            HideAllContainer();

            SelectedContainer.Visible = true;

            monthButton_Container.InnerHtml = "";

            PopulateDropDown();

            btnUpdateSelected.Visible = false;

            btnSaveSelected.Visible = true;

            txtSelectedStartDate.Text = DateTime.Now.ToString("dd-MMM-yy");

            txtSelectedStartDate.Enabled = true;

            txtUpdatedBy.Text = LoginUser;

            txtDateUpdated.Text = DateTime.Now.ToString("dd-MMM-yy");

            txtDateUpdated.Enabled = false;

            txtUpdatedBy.Enabled = false;


            TTSHWCFReference.Project_Master project = new Project_Master();

            int project_id = Convert.ToInt32(e.CommandArgument);

            bProjectID.InnerText = "Project ID: " + project_id;

            HdnProjectID.Value = Convert.ToString(project_id);

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            project = client.GetProject_MasterDetailsByID(project_id);

            txtProjectTitle.Text = project.s_Project_Title;

            txtShortTitle.Text = project.s_Short_Title;

            ddlProjectCategory.SelectedValue = project.i_Project_Category_ID.ToString();

            txtAlias1.Text = project.s_Project_Alias1;

            txtAlias2.Text = project.s_Project_Alias2;

            FillPIGrid(project.DEPT_PI.ToList());

            //If Login User is Selected Team User
            if (isSelectedTeamUser)
            {
                SelectedSetting();
            }
            //End of If Login User is Selected Team User

            //If Login User is Blinded Team User
            if (isBlindedUser)
            {
                BlindedSetting();

            }
            //End of If Login User is Blinded Team User

            //If Login User is UnBlinded Team User
            if (isUnblinded)
            {
                UnblindedSetting();
            }
            //End of If Login User is UnBlinded Team User


        }

        public void BlindedSetting()
        {
            //To disable the Project details
            CallJS("DisableProjectDetails();");

            BlindedContainer.Visible = true;

            ProjectRequirementContainer.Visible = true;

            MonthlyDeatailContainer.Visible = true;

            FillYearDDL();
        }

        public void UnblindedSetting()
        {
            //To disable the Project details
            CallJS("DisableProjectDetails();");

            UnBlindedContainer.Visible = true;

            MonthlyDeatailContainer.Visible = true;

            FillYearDDL();
        }

        public void SelectedSetting()
        {
            cordinatorContainer.Visible = true;
        }

        private void FillPIGrid(List<PI_Master> listPI)
        {
            rptrPIDetails.DataSource = listPI;
            rptrPIDetails.DataBind();

        }

        public void FillGrid()
        {
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            List<Selected_Grid> gridData = new List<Selected_Grid>();

            //isSelectedTeamUser = true;

            gridData = client.Selected_FillGrid(LoginUserId, isSelectedTeamUser).ToList();
            try
            {
                string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                Project_DataOwner[] oDOList = client.GetProjectsByDO("SELECTED", UserID);
                DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                var AdminArray = (from s in oDataOwner select s.GUID).ToList();

                bool IsAdmin = AdminArray.Contains(UserID);

                if (IsAdmin == false)
                {
                    List<Selected_Grid> oNewGrid = new List<Selected_Grid>();
                    if (gridData != null && gridData.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                    {

                        oNewGrid = gridData.Where(z => z.Status.ToUpper() == "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
                        gridData.RemoveAll(z => z.Status.ToUpper() == "NEW");
                        gridData.AddRange(oNewGrid);
                        //var v1 = gridData.Where(z => z.Status.ToUpper().Trim() != "NEW" && z.cordinatorstatus.ToUpper().Trim() == "NEW").Count();
                        //var v2 = gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).Count();
                        //var v3 = gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => !oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).Count();

                        //gridData.Where(z => z.Status.ToUpper().Trim() != "NEW" && z.cordinatorstatus.ToUpper().Trim() == "NEW").ToList().ForEach(i => i.Status = "Edit");
                        //gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "View");
                        //gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => !oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList().ForEach(i => i.Status = "Edit");

                        var editNew = gridData.Where(z => z.Status.ToUpper().Trim() != "NEW" && z.cordinatorstatus.ToUpper().Trim() == "NEW").ToList();
                        var view =  gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList();
                        var Edit =  gridData.Where(z => z.Status.ToUpper() != "NEW" && z.cordinatorstatus.ToUpper() != "NEW").Where(z => !oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim().Equals(z.s_Display_Project_ID.ToUpper().Trim()))).ToList();

                        editNew.ForEach(i => i.Status = "Edit");
                        view.ForEach(i => i.Status = "View");
                        Edit.ForEach(i => i.Status = "Edit");

                        oNewGrid.AddRange(editNew);
                        oNewGrid.AddRange(view);
                        oNewGrid.AddRange(Edit);

                        gridData = oNewGrid;
                        //gridData.Where(z => "EDIT,VIEW".Contains(z.Status.ToUpper())).Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim() == z.s_Display_Project_ID.ToUpper().Trim())).ToList().ForEach(i => i.Status = "Edit");
                        gridData = gridData.OrderByDescending(z => z.Selected_ID).ToList();
                    }
                    else if (gridData != null && gridData.Count() > 0)
                    {
                        gridData.ForEach(x => x.Status = "View");
                        gridData.OrderByDescending(z => z.i_Project_ID);
                    }
                }
                else
                {
                    //gridData.Where(z => z.Status.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "View");
                    gridData.Where(z => z.Status.ToUpper() != "NEW").Where(z => z.cordinatorstatus.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "Edit");
                    gridData = gridData.OrderByDescending(z => z.i_Project_ID).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            rptrProjectDetail.DataSource = gridData;

            rptrProjectDetail.DataBind();

        }

        public void ResetControls()
        {
            //CallJS("ClearAll('new')");

            foreach (Control ctrl in txtSelectedStartDate.Parent.Controls)
            {

                if (ctrl.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)ctrl;
                    txt.Text = "";
                }

                if (ctrl.GetType() == typeof(HiddenField))
                {
                    HiddenField hdn = (HiddenField)ctrl;
                    hdn.Value = "";
                }

                if (ctrl.GetType() == typeof(DropDownList))
                {
                    DropDownList ddl = (DropDownList)ctrl;
                    ddl.ClearSelection();
                }

            }

            //To reset the download link
            //btnDownInsuranceFile.Text = "";
            //btnDownIRBFile.Text = "";

            //btnDownInsuranceFile.Visible = false;
            //btnDownIRBFile.Visible = false;

            ////To reset hidded fields
            //HdnDeptId.Value = "";
            //hdnInsuranceFile.Value = "";
            //hdnIRBFile.Value = "";
            //HdnNewDeptId.Value = "";
            //HdnPi_ID.Value = "";
            //HdnpiId.Value = "";

        }

        public void PopulateDropDown()
        {

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlBlindedCordinator, DropDownName.Coordinators, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlUnBlindedCordinator, DropDownName.Coordinators, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlTypeofStudy, DropDownName.Project_Type, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlStudyStatus, DropDownName.Study_Status, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlProjectCategory, DropDownName.Project_Category, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlModeofNotification, DropDownName.Mode_of_Notification, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlLocation, DropDownName.Drug_Location, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlCupboardNoBlinded, DropDownName.Cupboad_Number, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlCupboardNoUnblinded, DropDownName.Cupboad_Number, "");

            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<TTSHWCFReference.clsDropDown> ddllist = cl.GetDropDownData(DropDownName.Coordinators, "", "", "", "", "").ToList();

            chkboxlistBlinded.DataSource = ddllist;
            chkboxlistBlinded.DataTextField = "DisplayField";
            chkboxlistBlinded.DataValueField = "ValueField";
            chkboxlistBlinded.DataBind();

            chkboxlistUnBlinded.DataSource = ddllist;
            chkboxlistUnBlinded.DataTextField = "DisplayField";
            chkboxlistUnBlinded.DataValueField = "ValueField";
            chkboxlistUnBlinded.DataBind();



        }

        protected void backToGrid_Click(object sender, EventArgs e)
        {
            FillGrid();

            projectGrid.Visible = true;

            SelectedContainer.Visible = false;
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

                if (res.Split('|')[1] != "")
                {
                    if (res.Split('|')[1].ToLower().Trim() == "mcr exists")
                        CallJS("MessageBox('PI with same MCR Number already exists for selected Department');ApplyAutoComplete();");
                    else if (res.Split('|')[1].ToLower().Trim() == "email exists")
                        CallJS("MessageBox('PI with same Email already exists for selected Department');ApplyAutoComplete();");
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

        public void CallJS(string script, string Name = "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Name, script, true);
        }

        public void FillYearDDL()
        {
            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();
        }

        public List<string> GetYears()
        {

            List<string> years = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.Load(HttpContext.Current.Server.MapPath("~/selectedYear.xml"));

            int currYear = DateTime.Now.Year;

            XmlNodeList xmlNodeList = xml.SelectNodes("years/year");
            foreach (XmlNode node in xmlNodeList)
            {

                int year = Convert.ToInt32(node.InnerText);

                if (year <= currYear)
                    years.Add(node.InnerText);

            }

            return years;
        }

        protected void btnSaveNewCRO_Click(object sender, EventArgs e)
        {
            TTSHWCFReference.CRO_Master cro = new CRO_Master();

            cro.s_Name = txtCROName.Text;
            cro.s_CreatedBy_ID = "";

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            string res = client.CRO(cro);

            if (res.Split('|')[1] == "Exists")
            {
                CallJS("MessageBox('CRO with same name already exists');");
            }
            else
                CallJS("MessageBox('CRO saved successfully'); $('[id*=btnCancelNewCRO]').click();");
        }

        protected void SaveMoreCRO_Click(object sender, EventArgs e)
        {
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();
            TTSHWCFReference.CRA_Master CRA = new CRA_Master();

            CRA.i_CRO_ID = Convert.ToInt32(HdnCROId.Value);

            CRA.s_CreatedBy_ID = LoginUserId;

            CRA.s_Name = TxtCRAName.Text;

            CRA.s_Email = txtCRAEmail.Text;

            CRA.s_phone_no = txtCRAPhoneNo.Text;

            string res = client.CRA(CRA);
            int value;

            if (int.TryParse(res, out value))
            {
                CallJS("CRASaved('" + CRA.i_CRO_ID + "," + res + "');ApplyAutoComplete();");
            }
            else
            {
                CallJS("MessageBox('" + res + "');ApplyAutoComplete();");
            }
        }

        protected void btnSaveSelected_Click(object sender, EventArgs e)
        {
            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            int ProjID = Convert.ToInt32(HdnProjectID.Value);

            //int selectedID = Convert.ToInt32(hdnSelectedID.Value);

            //For Selected User
            if (isSelectedTeamUser)
            {

                selected.s_Blinded_Coordinator = ddlBlindedCordinator.SelectedValue == "-1" ? "0" : ddlBlindedCordinator.SelectedValue;
                selected.s_Blinded_Cordinator_name = ddlBlindedCordinator.SelectedValue == "-1" ? "" : ddlBlindedCordinator.SelectedItem.Text;
                selected.s_Unblinded_Coordinator = ddlUnBlindedCordinator.SelectedValue == "-1" ? "0" : ddlUnBlindedCordinator.SelectedValue;
                selected.s_Unblinded_Cordinator_name = ddlUnBlindedCordinator.SelectedValue == "-1" ? "" : ddlUnBlindedCordinator.SelectedItem.Text;

                selected.s_Project_Alias1 = txtAlias1.Text;
                selected.s_Project_Alias2 = txtAlias2.Text;
                selected.s_Short_Title = txtShortTitle.Text;

                //Project PIs
                List<Project_PI> PIs = new List<Project_PI>();
                string[] splitPiId = HdnPi_ID.Value.Split(',');
                foreach (string pi in splitPiId)
                {
                    PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = ProjID });
                }

                selected.Project_PIs = PIs.ToArray();


                //To get seletected Blinded and Un blinded Cordinators
                List<TTSHWCFReference.SelectedProject_BU_Details> backupCord = new List<SelectedProject_BU_Details>();
                foreach (ListItem item in chkboxlistBlinded.Items)
                {
                    if (item.Selected)
                    {
                        SelectedProject_BU_Details bu_Details = new SelectedProject_BU_Details();
                        bu_Details.i_Selected_Project_ID = ProjID;
                        bu_Details.s_Blinded_UnBlinded = "B";
                        bu_Details.s_Cordinator_Id = item.Value;
                        bu_Details.s_Cordinator_name = item.Text;

                        backupCord.Add(bu_Details);
                    }
                }

                foreach (ListItem item in chkboxlistUnBlinded.Items)
                {
                    if (item.Selected)
                    {
                        SelectedProject_BU_Details bu_DetailsUnblinded = new SelectedProject_BU_Details();
                        bu_DetailsUnblinded.i_Selected_Project_ID = ProjID;
                        bu_DetailsUnblinded.s_Blinded_UnBlinded = "U";
                        bu_DetailsUnblinded.s_Cordinator_Id = item.Value;
                        bu_DetailsUnblinded.s_Cordinator_name = item.Text;

                        backupCord.Add(bu_DetailsUnblinded);
                    }
                }
                //End of To get seletected Blinded and Un blinded Cordinators

                selected.BU_Details = backupCord.ToArray();

                selected.b_IsTeam_Needed = ddlTeamNeeded.SelectedValue == "0" || ddlTeamNeeded.SelectedValue == "-1" ? false : true;

                selected.dt_Selected_Start_Date = Convert.ToDateTime(txtSelectedStartDate.Text);

                selected.s_ModifyBy_ID = txtUpdatedBy.Text;

                selected.dt_Modify_Date = Convert.ToDateTime(txtDateUpdated.Text);

                selected.UserCId = LoginUserId;

                selected.Username = LoginUser;



            }

            //For Blinded User 
            if (isBlindedUser)
            {

                selected.b_SAE_Status = (ddlSAEStatus.SelectedValue == "-1" || ddlSAEStatus.SelectedValue == "0" ? false : true);

                selected.i_Notification_Mode = Convert.ToInt32(ddlModeofNotification.SelectedValue);

                selected.i_Patient_Studyno = txtPatientStudyNo.Text;

                if (txtReadmissionDate.Text.Trim() == "")
                    selected.dt_Readmission_date = null;
                else
                    selected.dt_Readmission_date = Convert.ToDateTime(txtReadmissionDate.Text);

                if (txtDischargeDate.Text.Trim() == "")
                    selected.dt_Discharge_date = null;
                else
                    selected.dt_Discharge_date = Convert.ToDateTime(txtDischargeDate.Text);

                if (txtdtCordinatorsKnowledge.Text.Trim() == "")
                    selected.dt_Knowledge_date = null;
                else
                    selected.dt_Knowledge_date = Convert.ToDateTime(txtdtCordinatorsKnowledge.Text);

                selected.b_IsReadmission = (ddlReadmission.SelectedValue == "-1" || ddlReadmission.SelectedValue == "0" ? false : true);

                //CRA Section
                if (hdnCROCRAIDs.Value != "")
                {
                    List<Selected_CRA_Details> CRAlist = new List<Selected_CRA_Details>();

                    string[] CRACRO = hdnCROCRAIDs.Value.Split(',');
                    foreach (string item in CRACRO)
                    {
                        Selected_CRA_Details CRA_Details = new Selected_CRA_Details();
                        CRA_Details.i_CRO_ID = Convert.ToInt32(item.Split('|')[0]);
                        CRA_Details.i_CRA_ID = Convert.ToInt32(item.Split('|')[1]);
                        CRA_Details.i_Project_ID = ProjID;
                        CRAlist.Add(CRA_Details);
                    }

                    selected.CRA_Details = CRAlist.ToArray();
                }
                //End of CRA Section

                //Study Section
                selected.i_Study_Status_ID = Convert.ToInt32(ddlStudyStatus.SelectedValue);

                selected.i_Project_Type_ID = Convert.ToInt32(ddlTypeofStudy.SelectedValue);

                selected.b_IsApproveProject = (ddlApprovedStudyBugdet.SelectedValue == "-1" || ddlApprovedStudyBugdet.SelectedValue == "0" ? false : true);

                List<Selected_Project_StudyBudgetFile> budgetFile = new List<Selected_Project_StudyBudgetFile>();


                //End of Study Section

                //Archiving - Section 
                selected.b_Awaiting_Archiving = (ddlAwaitingArchiving.SelectedValue == "-1" || ddlAwaitingArchiving.SelectedValue == "0" ? false : true);

                if (txtEndDateArchiving.Text.Trim() == "")
                    selected.dt_Archiving_Enddate = null;
                else
                    selected.dt_Archiving_Enddate = Convert.ToDateTime(txtEndDateArchiving.Text);

                if (txtDateSentForArchiving.Text.Trim() == "")
                    selected.dt_Date_Sent_for_Archiving = null;
                else
                    selected.dt_Date_Sent_for_Archiving = Convert.ToDateTime(txtDateSentForArchiving.Text);

                selected.s_Reason = txtReason.Text;

                selected.s_Offsite_Company = txtOffSiteCompany.Text;

                //End of Archiving - Section 

                //Other Details 

                selected.s_Clinic1 = txtClinic1.Text;

                selected.s_Clinic2 = txtClinic2.Text;

                if (txtClinicDaysResearch.Text.Trim() == "")
                    selected.s_Research_Days = "";
                else
                    selected.s_Research_Days = Convert.ToString(txtClinicDaysResearch.Text);

                selected.s_Followup_Duratrion = txtDurationofFollowups.Text;

                if (txtRecruitStartDate.Text.Trim() == "")
                    selected.dt_Recruit_Start_Date = null;
                else
                    selected.dt_Recruit_Start_Date = Convert.ToDateTime(txtRecruitStartDate.Text);

                if (txtRecruitEndDate.Text.Trim() == "")
                    selected.dt_Recruit_End_Date = null;
                else
                    selected.dt_Recruit_End_Date = Convert.ToDateTime(txtRecruitEndDate.Text);

                if (txtTargetforTTSH.Text.Trim() == "")
                    selected.i_TTSH_Target = 1;
                else
                    selected.i_TTSH_Target = Convert.ToInt32(txtTargetforTTSH.Text);

                if (txtScreened.Text.Trim() == "")
                    selected.i_Screen_No = 0;
                else
                    selected.i_Screen_No = Convert.ToInt32(txtScreened.Text);

                if (txtScreenFailure.Text.Trim() == "")
                    selected.i_Screen_Failure = 0;
                else
                    selected.i_Screen_Failure = Convert.ToInt32(txtScreenFailure.Text);

                if (txtRandomized.Text.Trim() == "")
                    selected.i_Randomized = 0;
                else
                    selected.i_Randomized = Convert.ToInt32(txtRandomized.Text);

                if (txtWithdrawal.Text.Trim() == "")
                    selected.i_Withdrawl = 0;
                else
                    selected.i_Withdrawl = Convert.ToInt32(txtWithdrawal.Text);

                if (txtCompleted.Text.Trim() == "")
                    selected.i_Completed = 0;
                else
                    selected.i_Completed = Convert.ToInt32(txtCompleted.Text);


                if (txtIRBExpiryDate.Text.Trim() == "")
                    selected.dt_Expiry_date = null;
                else
                    selected.dt_Expiry_date = Convert.ToDateTime(txtIRBExpiryDate.Text);

                if (txtIRB.Text != "")
                    selected.s_IRB_No = txtIRB.Text;

                if (txtCTCExpiryDate.Text.Trim() == "")
                    selected.dt_CTC_Expiry_date = null;
                else
                    selected.dt_CTC_Expiry_date = Convert.ToDateTime(txtCTCExpiryDate.Text);

                if (txtCTMExpiryDate.Text.Trim() == "")
                    selected.dt_CTM_Expiry_date = null;
                else
                    selected.dt_CTM_Expiry_date = Convert.ToDateTime(txtCTMExpiryDate.Text);

                selected.b_CTM_Status = (ddlCTMStatus.SelectedValue == "-1" || ddlCTMStatus.SelectedValue == "0" ? false : true);

                //Additional fields
                if (txtNumberOfBoxes.Text.Trim() == "")
                    selected.i_Number_of_Boxes = 0;
                else
                    selected.i_Number_of_Boxes = Convert.ToInt32(txtNumberOfBoxes.Text);

                selected.s_Amount = txtAmount.Text;

                selected.s_Agreement_Number = txtAgreementNumber.Text;

                //Additional fields

                //End of Other Details 

                //Cupboard - Section (Blinded) 
                if (txtExpectedMonth.Text.Trim() == "")
                    selected.dt_Extended_Month_Blinded = null;
                else
                    selected.dt_Extended_Month_Blinded = Convert.ToDateTime(txtExpectedMonth.Text);

                //if (txtLastUpdatedBlinded.Text.Trim() == "")
                //    selected.dt_EntryForMonthBlinded = null;
                //else
                //    selected.dt_EntryForMonthBlinded = Convert.ToDateTime(txtLastUpdatedBlinded.Text);

                selected.i_CupBoardno_Blinded = Convert.ToInt32(ddlCupboardNoBlinded.SelectedValue);

                //End of Cupboard - Section (Blinded) 

                selected.dt_LastUpdated_By_Blinded = Convert.ToDateTime(txtEntryMonth.Text);
                selected.s_LastUpdated_By_Blinded = LoginUser;


            }

            //For Un Blinded User
            if (isUnblinded)
            {
                //Drug Location - Section 
                selected.s_Drug_Name = txtDrugName.Text;

                selected.i_Drug_Location_ID = Convert.ToInt32(ddlLocation.SelectedValue);

                if (txtDateofExpiry.Text.Trim() == "")
                    selected.dt_Drug_Expiry_date = null;
                else
                    selected.dt_Drug_Expiry_date = Convert.ToDateTime(txtDateofExpiry.Text);

                selected.s_Drug_Dose = txtDose.Text;
                //End of Drug Location - Section 

                //Cupboard - Section (Un Blinded) 
                if (txtExpectedMonthUnBlinded.Text.Trim() == "")
                    selected.dt_Extended_Month_UnBlinded = null;
                else
                    selected.dt_Extended_Month_UnBlinded = Convert.ToDateTime(txtExpectedMonthUnBlinded.Text);

                //if (txtLastUpdatedUnBlinded.Text.Trim() == "")
                //    selected.dt_EntryForMonthUnBlinded = null;
                //else
                //    selected.dt_EntryForMonthUnBlinded = Convert.ToDateTime(txtLastUpdatedUnBlinded.Text);

                selected.i_CupBoardno_UnBlinded = Convert.ToInt32(ddlCupboardNoUnblinded.SelectedValue);
                //End of Cupboard - Section (Un Blinded) 

                selected.dt_LastUpdated_By_UnBlinded = Convert.ToDateTime(txtEntryMonth.Text);
                selected.s_LastUpdated_By_UnBlinded = LoginUser;
            }

            //Common fields
            selected.UserCId = LoginUserId;

            selected.Username = LoginUser;

            selected.s_ModifyBy_Name = LoginUser;

            selected.s_ModifyBy_ID = LoginUserId;

            selected.dt_Created_Date = DateTime.Now;

            selected.dt_Modify_Date = DateTime.Now;

            selected.i_Project_Id = ProjID;

            if (txtEntryMonth.Text.Trim() == "")
                selected.dt_EntryForMonthBlinded = null;
            else
                selected.dt_EntryForMonthBlinded = Convert.ToDateTime(txtEntryMonth.Text);


            //Call the service method to save the details
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            client.Selected_Project_Details(selected, Mode.Insert);

            FillGrid();

            projectGrid.Visible = true;

            SelectedContainer.Visible = false;

            CallJS("MessageBox('Saved Successfully');");
        }

        protected void btnUpdateSelected_Click(object sender, EventArgs e)
        {
            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            int ProjID = Convert.ToInt32(HdnProjectID.Value);

            int selectedID = Convert.ToInt32(hdnSelectedID.Value);

            //For Selected User
            if (true) //isSelectedTeamUser
            {

                selected.s_Blinded_Coordinator = ddlBlindedCordinator.SelectedValue == "-1" ? "0" : ddlBlindedCordinator.SelectedValue;
                selected.s_Blinded_Cordinator_name = ddlBlindedCordinator.SelectedValue == "-1" ? "" : ddlBlindedCordinator.SelectedItem.Text;
                selected.s_Unblinded_Coordinator = ddlUnBlindedCordinator.SelectedValue == "-1" ? "0" : ddlUnBlindedCordinator.SelectedValue;
                selected.s_Unblinded_Cordinator_name = ddlUnBlindedCordinator.SelectedValue == "-1" ? "" : ddlUnBlindedCordinator.SelectedItem.Text;

                selected.s_Project_Alias1 = txtAlias1.Text;
                selected.s_Project_Alias2 = txtAlias2.Text;
                selected.s_Short_Title = txtShortTitle.Text;

                //Project PIs
                List<Project_PI> PIs = new List<Project_PI>();
                string[] splitPiId = HdnPi_ID.Value.Split(',');
                foreach (string pi in splitPiId)
                {
                    PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = ProjID });
                }

                selected.Project_PIs = PIs.ToArray();


                //To get seletected Blinded and Un blinded Cordinators
                List<TTSHWCFReference.SelectedProject_BU_Details> backupCord = new List<SelectedProject_BU_Details>();
                foreach (ListItem item in chkboxlistBlinded.Items)
                {
                    if (item.Selected)
                    {
                        SelectedProject_BU_Details bu_Details = new SelectedProject_BU_Details();
                        bu_Details.i_Selected_Project_ID = ProjID;
                        bu_Details.s_Blinded_UnBlinded = "B";
                        bu_Details.s_Cordinator_Id = item.Value;
                        bu_Details.s_Cordinator_name = item.Text;

                        backupCord.Add(bu_Details);
                    }
                }

                foreach (ListItem item in chkboxlistUnBlinded.Items)
                {
                    if (item.Selected)
                    {
                        SelectedProject_BU_Details bu_DetailsUnblinded = new SelectedProject_BU_Details();
                        bu_DetailsUnblinded.i_Selected_Project_ID = ProjID;
                        bu_DetailsUnblinded.s_Blinded_UnBlinded = "U";
                        bu_DetailsUnblinded.s_Cordinator_Id = item.Value;
                        bu_DetailsUnblinded.s_Cordinator_name = item.Text;

                        backupCord.Add(bu_DetailsUnblinded);
                    }
                }
                //End of To get seletected Blinded and Un blinded Cordinators

                selected.BU_Details = backupCord.ToArray();

                selected.b_IsTeam_Needed = ddlTeamNeeded.SelectedValue == "0" || ddlTeamNeeded.SelectedValue == "-1" ? false : true;

                selected.dt_Selected_Start_Date = Convert.ToDateTime(txtSelectedStartDate.Text);

                if (txtUpdatedBy.Visible)
                {
                    selected.s_ModifyBy_ID = txtUpdatedBy.Text;

                    selected.dt_Modify_Date = Convert.ToDateTime(txtDateUpdated.Text);
                }

                selected.UserCId = LoginUserId;

                selected.Username = LoginUser;



            }

            //For Blinded User 
            if (true) //isBlindedUser
            {

                selected.b_SAE_Status = (ddlSAEStatus.SelectedValue == "-1" || ddlSAEStatus.SelectedValue == "0" ? false : true);

                selected.i_Notification_Mode = Convert.ToInt32(ddlModeofNotification.SelectedValue);

                selected.i_Patient_Studyno = txtPatientStudyNo.Text;

                if (txtReadmissionDate.Text.Trim() == "")
                    selected.dt_Readmission_date = null;
                else
                    selected.dt_Readmission_date = Convert.ToDateTime(txtReadmissionDate.Text);

                if (txtDischargeDate.Text.Trim() == "")
                    selected.dt_Discharge_date = null;
                else
                    selected.dt_Discharge_date = Convert.ToDateTime(txtDischargeDate.Text);

                if (txtdtCordinatorsKnowledge.Text.Trim() == "")
                    selected.dt_Knowledge_date = null;
                else
                    selected.dt_Knowledge_date = Convert.ToDateTime(txtdtCordinatorsKnowledge.Text);

                selected.b_IsReadmission = (ddlReadmission.SelectedValue == "-1" || ddlReadmission.SelectedValue == "0" ? false : true);

                //CRA Section
                if (hdnCROCRAIDs.Value != "")
                {
                    List<Selected_CRA_Details> CRAlist = new List<Selected_CRA_Details>();

                    string[] CRACRO = hdnCROCRAIDs.Value.Split(',');
                    foreach (string item in CRACRO)
                    {
                        Selected_CRA_Details CRA_Details = new Selected_CRA_Details();
                        CRA_Details.i_CRO_ID = Convert.ToInt32(item.Split('|')[0]);
                        CRA_Details.i_CRA_ID = Convert.ToInt32(item.Split('|')[1]);
                        CRA_Details.i_Project_ID = ProjID;
                        CRAlist.Add(CRA_Details);
                    }

                    selected.CRA_Details = CRAlist.ToArray();
                }
                //End of CRA Section

                //Study Section
                selected.i_Study_Status_ID = Convert.ToInt32(ddlStudyStatus.SelectedValue);

                selected.i_Project_Type_ID = Convert.ToInt32(ddlTypeofStudy.SelectedValue);

                selected.b_IsApproveProject = (ddlApprovedStudyBugdet.SelectedValue == "-1" || ddlApprovedStudyBugdet.SelectedValue == "0" ? false : true);


                List<Selected_Project_StudyBudgetFile> budgetFile = new List<Selected_Project_StudyBudgetFile>();

                //Agreement File
                hdnIRBFileEnabled.Value = "Yes";
                if (fuIRBFile.HasFile && hdnIRBFileEnabled.Value == "Yes")
                {
                    string[] arr = Common.UpLoadNew(fuIRBFile, Common.FolderLocation.EthicFiles);

                    if (arr.Count() != 0)
                    {
                        hdnIRBFile.Value = arr[0];
                    }
                }

                if (hdnIRBFile.Value != null && hdnIRBFile.Value != "")
                    selected.s_AgreementFile = hdnIRBFile.Value;

                //Agreement File



                //Budget file code

                if (hdnBudgetFileNotSaved.Value != "")
                {
                    string[] files = hdnBudgetFileNotSaved.Value.Split(',');

                    //string[] Comments = hdnUnSavedComments.Value.Split(',');
                    if (hdnSavedComments.Value == "")
                        hdnSavedComments.Value += hdnUnSavedComments.Value;
                    else
                        hdnSavedComments.Value += "," + hdnUnSavedComments.Value;

                    string[] fileNamesAfterSave = Common.GetFilesFromStringPathMultiple(files, Common.FolderLocation.SelectedFiles);

                    if (hdnBudgetFiles.Value == "")
                        hdnBudgetFiles.Value += string.Join(",", fileNamesAfterSave);
                    else
                        hdnBudgetFiles.Value += "," + string.Join(",", fileNamesAfterSave);


                }


                if (hdnBudgetFiles.Value != "")
                {
                    string[] files = hdnBudgetFiles.Value.Split(',');

                    string[] comments = hdnSavedComments.Value.Split(',');

                    for (int i = 0; i < files.Length; i++)
                    {
                        budgetFile.Add(new Selected_Project_StudyBudgetFile
                        {

                            i_Selected_Project_ID = selectedID,
                            s_Budget_Comments = comments[i],
                            s_Budget_Document_File = files[i]


                        });
                    }
                }

                selected.StudyBudgetFile = budgetFile.ToArray();
                //Budget file code

                //End of Study Section


                //Archiving - Section 
                selected.b_Awaiting_Archiving = (ddlAwaitingArchiving.SelectedValue == "-1" || ddlAwaitingArchiving.SelectedValue == "0" ? false : true);

                if (txtEndDateArchiving.Text.Trim() == "")
                    selected.dt_Archiving_Enddate = null;
                else
                    selected.dt_Archiving_Enddate = Convert.ToDateTime(txtEndDateArchiving.Text);

                if (txtDateSentForArchiving.Text.Trim() == "")
                    selected.dt_Date_Sent_for_Archiving = null;
                else
                    selected.dt_Date_Sent_for_Archiving = Convert.ToDateTime(txtDateSentForArchiving.Text);

                selected.s_Reason = txtReason.Text;

                selected.s_Offsite_Company = txtOffSiteCompany.Text;

                //End of Archiving - Section 

                //Other Details 

                selected.s_Clinic1 = txtClinic1.Text;

                selected.s_Clinic2 = txtClinic2.Text;

                if (txtClinicDaysResearch.Text.Trim() == "")
                    selected.s_Research_Days = "";
                else
                    selected.s_Research_Days = Convert.ToString(txtClinicDaysResearch.Text);

                selected.s_Followup_Duratrion = txtDurationofFollowups.Text;

                if (txtRecruitStartDate.Text.Trim() == "")
                    selected.dt_Recruit_Start_Date = null;
                else
                    selected.dt_Recruit_Start_Date = Convert.ToDateTime(txtRecruitStartDate.Text);

                if (txtRecruitEndDate.Text.Trim() == "")
                    selected.dt_Recruit_End_Date = null;
                else
                    selected.dt_Recruit_End_Date = Convert.ToDateTime(txtRecruitEndDate.Text);

                if (txtTargetforTTSH.Text.Trim() == "")
                    selected.i_TTSH_Target = 0;
                else
                    selected.i_TTSH_Target = Convert.ToInt32(txtTargetforTTSH.Text);

                if (txtScreened.Text.Trim() == "")
                    selected.i_Screen_No = 0;
                else
                    selected.i_Screen_No = Convert.ToInt32(txtScreened.Text);

                if (txtScreenFailure.Text.Trim() == "")
                    selected.i_Screen_Failure = 0;
                else
                    selected.i_Screen_Failure = Convert.ToInt32(txtScreenFailure.Text);

                if (txtRandomized.Text.Trim() == "")
                    selected.i_Randomized = 0;
                else
                    selected.i_Randomized = Convert.ToInt32(txtRandomized.Text);

                if (txtWithdrawal.Text.Trim() == "")
                    selected.i_Withdrawl = 0;
                else
                    selected.i_Withdrawl = Convert.ToInt32(txtWithdrawal.Text);

                if (txtCompleted.Text.Trim() == "")
                    selected.i_Completed = 0;
                else
                    selected.i_Completed = Convert.ToInt32(txtCompleted.Text);


                if (txtIRBExpiryDate.Text.Trim() == "")
                    selected.dt_Expiry_date = null;
                else
                    selected.dt_Expiry_date = Convert.ToDateTime(txtIRBExpiryDate.Text);

                if (txtIRB.Text != "")
                    selected.s_IRB_No = txtIRB.Text;

                if (txtCTCExpiryDate.Text.Trim() == "")
                    selected.dt_CTC_Expiry_date = null;
                else
                    selected.dt_CTC_Expiry_date = Convert.ToDateTime(txtCTCExpiryDate.Text);

                if (txtCTMExpiryDate.Text.Trim() == "")
                    selected.dt_CTM_Expiry_date = null;
                else
                    selected.dt_CTM_Expiry_date = Convert.ToDateTime(txtCTMExpiryDate.Text);

                selected.b_CTM_Status = (ddlCTMStatus.SelectedValue == "-1" || ddlCTMStatus.SelectedValue == "0" ? false : true);

                //Additional fields
                if (txtNumberOfBoxes.Text.Trim() == "")
                    selected.i_Number_of_Boxes = 0;
                else
                    selected.i_Number_of_Boxes = Convert.ToInt32(txtNumberOfBoxes.Text);

                selected.s_Amount = txtAmount.Text;

                selected.s_Agreement_Number = txtAgreementNumber.Text;

                //Additional fields



                //End of Other Details 

                //Cupboard - Section (Blinded) 
                if (txtExpectedMonth.Text.Trim() == "")
                    selected.dt_Extended_Month_Blinded = null;
                else
                    selected.dt_Extended_Month_Blinded = Convert.ToDateTime(txtExpectedMonth.Text);

                //if (txtLastUpdatedBlinded.Text.Trim() == "")
                //    selected.dt_EntryForMonthBlinded = null;
                //else
                //    selected.dt_EntryForMonthBlinded = Convert.ToDateTime(txtLastUpdatedBlinded.Text);

                selected.i_CupBoardno_Blinded = Convert.ToInt32(ddlCupboardNoBlinded.SelectedValue);

                //End of Cupboard - Section (Blinded) 

                if (txtLastUpdatedBlinded.Visible)
                {
                    selected.dt_LastUpdated_By_Blinded = Convert.ToDateTime(txtEntryMonth.Text);
                    selected.s_LastUpdated_By_Blinded = LoginUser;
                }

            }

            //For Un Blinded User
            if (true) //isUnblinded
            {
                //Drug Location - Section 
                selected.s_Drug_Name = txtDrugName.Text;

                selected.i_Drug_Location_ID = Convert.ToInt32(ddlLocation.SelectedValue);

                if (txtDateofExpiry.Text.Trim() == "")
                    selected.dt_Drug_Expiry_date = null;
                else
                    selected.dt_Drug_Expiry_date = Convert.ToDateTime(txtDateofExpiry.Text);

                selected.s_Drug_Dose = txtDose.Text;
                //End of Drug Location - Section 

                //Cupboard - Section (Un Blinded) 
                if (txtExpectedMonthUnBlinded.Text.Trim() == "")
                    selected.dt_Extended_Month_UnBlinded = null;
                else
                    selected.dt_Extended_Month_UnBlinded = Convert.ToDateTime(txtExpectedMonthUnBlinded.Text);

                //if (txtLastUpdatedUnBlinded.Text.Trim() == "")
                //    selected.dt_EntryForMonthUnBlinded = null;
                //else
                //    selected.dt_EntryForMonthUnBlinded = Convert.ToDateTime(txtLastUpdatedUnBlinded.Text);

                selected.i_CupBoardno_UnBlinded = Convert.ToInt32(ddlCupboardNoUnblinded.SelectedValue);
                //End of Cupboard - Section (Un Blinded) 

                if (txtLastUpdatedUnBlinded.Visible)
                {
                    selected.dt_LastUpdated_By_UnBlinded = Convert.ToDateTime(txtEntryMonth.Text);
                    selected.s_LastUpdated_By_UnBlinded = LoginUser;
                }


            }

            //Common fields
            selected.UserCId = LoginUserId;

            selected.Username = LoginUser;

            selected.s_ModifyBy_Name = LoginUser;

            selected.s_ModifyBy_ID = LoginUserId;

            selected.dt_Created_Date = DateTime.Now;

            selected.dt_Modify_Date = DateTime.Now;

            selected.i_Project_Id = ProjID;

            selected.i_ID = Convert.ToInt32(hdnSelectedID.Value);

            if (txtEntryMonth.Text.Trim() == "")
                selected.dt_EntryForMonthBlinded = null;
            else
                selected.dt_EntryForMonthBlinded = Convert.ToDateTime(txtEntryMonth.Text);


            //Call the service method to save the details
            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            client.Selected_Project_Details(selected, Mode.Update);

            FillGrid();

            projectGrid.Visible = true;

            SelectedContainer.Visible = false;

            string buttonText = "Updated";
            if (hdnUpdateBtnText.Value.Contains("save"))
            {
                buttonText = "Saved";
            }


            CallJS("MessageBox('" + buttonText + " Successfully');");
        }

        protected void btnCancelSelected_Click(object sender, EventArgs e)
        {
            FillGrid();

            projectGrid.Visible = true;

            SelectedContainer.Visible = false;
        }

        protected void month_Click_Command(object sender, CommandEventArgs e)
        {
            ResetMonthlyDetailControls();

            string selectedMonth = hdnCurrMonth.Value;

            string year = ddlYear.SelectedValue;

            int projectID = 0;

            if (HdnProjectID.Value != "")
                projectID = Convert.ToInt32(HdnProjectID.Value);

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            selected = client.GetSelected_Project_DetailsByID(projectID, year, selectedMonth);

            FillMonthlyDetails(selected);
            //CallJS("MessageBox('Hi');");

            //CallJS("monthClickProcessed();");

        }

        protected void AppendMonth(string monthNames)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(monthNames)))
            {
                XmlDocument xml = new XmlDocument();

                xml.Load(reader);

                XmlNodeList xmlNodeList = xml.SelectNodes("Month/Months");

                HtmlGenericControl div = new HtmlGenericControl("div");

                //int i = 1;
                string markup = "<div class='tabItems'>";
                //markup += "<ul>";
                foreach (XmlNode node in xmlNodeList)
                {

                    string month = node["s_MonthName"].InnerText.Trim();

                    markup += "<a month='" + month + "' onclick='triggerMonth(\"" + month + "\");'>" + month + "</a>";

                    //markup += "<input type='button' class='month1' value='" + month + "' onclick='triggerMonth(\"" + month + "\");'></input>";

                    //markup += "<li><a value='" + month + "' onclick='triggerMonth(\"" + month + "\");'></a></li>";

                    //markup += "<a class='months' value='" + month + "' onclick='triggerMonth(\"" + month + "\");'>" + month + "</a>";

                    //LinkButton btn = new LinkButton();
                    //btn.Text = month;
                    ////btn.CommandArgument = month;
                    ////btn.CommandName += new CommandEventHandler(month_Click_Command);
                    //btn.ID = "btn_" + i;
                    //i++;

                    //monthButton_Container.Controls.Add(btn);
                    //div.Controls.Add(btn);

                    //--------
                    //HiddenField hdn = new HiddenField();
                    //LinkButton lnk = new LinkButton();
                    ////HtmlGenericControl div = new HtmlGenericControl("div"); HtmlGenericControl a = new HtmlGenericControl("a");
                    //lnk.ID = "lnk" + i.ToString();
                    //hdn.Value = Convert.ToString(h.Rows[i][0]);
                    //hdn.ID = "hdn" + i.ToString();
                    //a.ID = "a~" + i.ToString();
                    //lnk.Text = h.Rows[i][0].ToString().GetFileName();
                    //a.InnerText = "X";

                    //lnk.OnClientClick = "return Callhandler('" + ResolveUrl("~/DownloadHandler.ashx") + "','" + hdn.Value + "')";

                    //div.Attributes.Add("class", "MultiFile-label");
                    //a.Attributes.Add("class", "MultiFile-remove");
                    //a.Style["margin-right"] = "5px";
                    //a.Attributes.Add("onclick", "return RemoveDownloadFile(this)");
                    //lnk.Attributes.Add("class", "MultiFile-title");
                    //div.Controls.Add(a);
                    //div.Controls.Add(lnk);
                    //div.Controls.Add(hdn);
                    //multilistdiv.Controls.Add(div);

                    //updtPnlMonthlyDetails.Triggers.Add(trigger);
                }
                markup += "</div>";
                //markup += "<a class='filler'>filler</a>";
                monthButton_Container.InnerHtml = markup;

            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            int projectID = Convert.ToInt32(HdnProjectID.Value);

            TTSHWCFReference.Selected_Project_Details selected = new Selected_Project_Details();

            TTSHWCFServiceClient client = new TTSHWCFServiceClient();

            selected = client.GetSelected_Project_DetailsByID(projectID, ddlYear.SelectedValue, "");

            monthButton_Container.InnerHtml = "";

            if (selected.monthNames != "" && selected.monthNames != null)
            {
                AppendMonth(selected.monthNames);
            }


            FillMonthlyDetails(selected);

            //monthButton_Container.InnerHtml = "";

            //if (selected.monthNames != "" && selected.monthNames != null)
            //{
            //    AppendMonth(selected.monthNames);
            //}

            //ResetMonthlyDetailControls();

            ////Fill the monthly details
            ///*Blinded Details*/

            ////--SAE Details
            //ddlSAEStatus.SelectedValue = (selected.b_SAE_Status ? "1" : "0");

            //if (selected.i_Notification_Mode != 0)
            //    ddlModeofNotification.SelectedValue = Convert.ToString(selected.i_Notification_Mode);

            //txtPatientStudyNo.Text = selected.i_Patient_Studyno;

            //ddlReadmission.SelectedValue = (selected.b_IsReadmission ? "1" : "0");

            //txtReadmissionDate.Text = selected.dt_Readmission_date == DateTime.MinValue || selected.dt_Readmission_date == null ? "" : Convert.ToDateTime(selected.dt_Readmission_date).ToString("dd-MMM-yy");

            //txtDischargeDate.Text = selected.dt_Discharge_date == DateTime.MinValue || selected.dt_Discharge_date == null ? "" : Convert.ToDateTime(selected.dt_Discharge_date).ToString("dd-MMM-yy");

            //txtdtCordinatorsKnowledge.Text = selected.dt_Knowledge_date == DateTime.MinValue || selected.dt_Knowledge_date == null ? "" : Convert.ToDateTime(selected.dt_Knowledge_date).ToString("dd-MMM-yy");

            ////--CRO CRA Details

            ////Study Section 
            //if (selected.i_Study_Status_ID != 0)
            //    ddlStudyStatus.SelectedValue = Convert.ToString(selected.i_Study_Status_ID);

            //if (selected.i_Project_Type_ID != 0)
            //    ddlTypeofStudy.SelectedValue = Convert.ToString(selected.i_Project_Type_ID);

            //ddlApprovedStudyBugdet.SelectedValue = (selected.b_IsApproveProject ? "1" : "0");

            ////Budget file grid

            ////Archiving - Section 
            //ddlAwaitingArchiving.SelectedValue = (selected.b_Awaiting_Archiving ? "1" : "0");

            //txtEndDateArchiving.Text = selected.dt_Archiving_Enddate == DateTime.MinValue || selected.dt_Archiving_Enddate == null ? "" : Convert.ToDateTime(selected.dt_Archiving_Enddate).ToString("dd-MMM-yy");

            //txtReason.Text = selected.s_Reason;

            //txtOffSiteCompany.Text = selected.s_Offsite_Company;

            ////Other Details 
            //txtClinic1.Text = selected.s_Clinic1;

            //txtClinic2.Text = selected.s_Clinic2;

            //txtClinicDaysResearch.Text = Convert.ToString(selected.s_Research_Days);

            //txtDurationofFollowups.Text = selected.s_Followup_Duratrion;

            //txtRecruitStartDate.Text = selected.dt_Recruit_Start_Date == DateTime.MinValue || selected.dt_Recruit_Start_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_Start_Date).ToString("dd-MMM-yy");

            //txtRecruitEndDate.Text = selected.dt_Recruit_End_Date == DateTime.MinValue || selected.dt_Recruit_End_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_End_Date).ToString("dd-MMM-yy");

            //txtTargetforTTSH.Text = Convert.ToString(selected.i_TTSH_Target);

            //txtScreened.Text = Convert.ToString(selected.i_Screen_No);

            //txtScreenFailure.Text = Convert.ToString(selected.i_Screen_Failure);

            //txtRandomized.Text = Convert.ToString(selected.s_Research_Days);

            //txtCompleted.Text = Convert.ToString(selected.i_Completed);

            //txtWithdrawal.Text = Convert.ToString(selected.i_Withdrawl);

            //txtIRB.Text = Convert.ToString(selected.s_IRB_No);

            //txtIRBExpiryDate.Text = selected.dt_Expiry_date == DateTime.MinValue || selected.dt_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Expiry_date).ToString("dd-MMM-yy");

            //txtCTMExpiryDate.Text = selected.dt_CTM_Expiry_date == DateTime.MinValue || selected.dt_CTM_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTM_Expiry_date).ToString("dd-MMM-yy");

            //txtCTCExpiryDate.Text = selected.dt_CTC_Expiry_date == DateTime.MinValue || selected.dt_CTC_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTC_Expiry_date).ToString("dd-MMM-yy");

            //ddlCTMStatus.SelectedValue = (selected.b_CTM_Status ? "1" : "0");

            ////Cupboard - Section (Blinded) 
            //txtExpectedMonth.Text = selected.dt_Extended_Month_Blinded == DateTime.MinValue || selected.dt_Extended_Month_Blinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_Blinded).ToString("dd-MMM-yy");


            //if (selected.s_LastUpdated_By_Blinded != "")
            //    txtUpdatedByBlinded.Text = selected.s_LastUpdated_By_Blinded;
            //else
            //    txtUpdatedByBlinded.Text = LoginUser;

            //if (selected.dt_LastUpdated_By_Blinded != null && selected.dt_LastUpdated_By_Blinded != DateTime.MinValue)
            //    txtLastUpdatedBlinded.Text = Convert.ToDateTime(selected.dt_LastUpdated_By_Blinded).ToString("dd-MMM-yy");
            //else
            //    txtLastUpdatedBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");


            //ddlCupboardNoBlinded.SelectedValue = selected.i_CupBoardno_Blinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_Blinded);

            ///*End of Blinded Details*/

            ///*UnBlinded Details*/

            ////Drug Location - Section 
            //txtDrugName.Text = selected.s_Drug_Name;

            //txtDose.Text = selected.s_Drug_Dose;

            //txtDateofExpiry.Text = selected.dt_Drug_Expiry_date == DateTime.MinValue || selected.dt_Drug_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Drug_Expiry_date).ToString("dd-MMM-yy");

            //if (selected.i_Drug_Location_ID != 0)
            //    ddlLocation.SelectedValue = Convert.ToString(selected.i_Drug_Location_ID);

            ////Cupboard - Section (Un Blinded) 
            //txtExpectedMonthUnBlinded.Text = selected.dt_Extended_Month_UnBlinded == DateTime.MinValue || selected.dt_Extended_Month_UnBlinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_UnBlinded).ToString("dd-MMM-yy");

            //if (selected.s_LastUpdated_By_UnBlinded != "")
            //    txtUpdatedByUnblinded.Text = selected.s_LastUpdated_By_UnBlinded;
            //else
            //    txtUpdatedByUnblinded.Text = LoginUser;

            //if (selected.dt_LastUpdated_By_UnBlinded != null && selected.dt_LastUpdated_By_UnBlinded != DateTime.MinValue)
            //    txtLastUpdatedUnBlinded.Text = Convert.ToDateTime(selected.dt_LastUpdated_By_UnBlinded).ToString("dd-MMM-yy");
            //else
            //    txtLastUpdatedUnBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");

            //ddlCupboardNoUnblinded.SelectedValue = selected.i_CupBoardno_UnBlinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_UnBlinded);

            ///*End of UnBlinded Details*/

            ////End of Fill the monthly details

            //txtEntryMonth.Text = selected.dt_EntryForMonthBlinded == DateTime.MinValue || selected.dt_EntryForMonthBlinded == null ? "" : Convert.ToDateTime(selected.dt_EntryForMonthBlinded).ToString("dd-MMM-yy");
        }

        private void ResetMonthlyDetailControls()
        {

            string a = txtScreenFailure.Parent.ToString();
            foreach (Control cntrl in txtScreenFailure.Parent.Controls)
            {
                if (cntrl.GetType() == typeof(TextBox))
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.Text = "";
                }

                if (cntrl.GetType() == typeof(DropDownList))
                {
                    DropDownList ddList = (DropDownList)cntrl;
                    ddList.SelectedIndex = 0;
                }
            }

            //txtScreenFailure.Text = "";
            //txtScreened.Text = "";
        }

        private void FillMonthlyDetails(Selected_Project_Details selected)
        {

            ResetMonthlyDetailControls();

            //Fill the monthly details
            /*Blinded Details*/

            //--SAE Details
            ddlSAEStatus.SelectedValue = (selected.b_SAE_Status ? "1" : "0");

            if (selected.i_Notification_Mode != 0)
                ddlModeofNotification.SelectedValue = Convert.ToString(selected.i_Notification_Mode);

            txtPatientStudyNo.Text = selected.i_Patient_Studyno;

            ddlReadmission.SelectedValue = (selected.b_IsReadmission ? "1" : "0");

            txtReadmissionDate.Text = selected.dt_Readmission_date == DateTime.MinValue || selected.dt_Readmission_date == null ? "" : Convert.ToDateTime(selected.dt_Readmission_date).ToString("dd-MMM-yy");

            txtDischargeDate.Text = selected.dt_Discharge_date == DateTime.MinValue || selected.dt_Discharge_date == null ? "" : Convert.ToDateTime(selected.dt_Discharge_date).ToString("dd-MMM-yy");

            txtdtCordinatorsKnowledge.Text = selected.dt_Knowledge_date == DateTime.MinValue || selected.dt_Knowledge_date == null ? "" : Convert.ToDateTime(selected.dt_Knowledge_date).ToString("dd-MMM-yy");

            //--CRO CRA Details
            string CRAXML = selected.CRA_XML;

            DataTable CRADetails = new DataTable();
            CRADetails.Columns.Add("CRAID");
            CRADetails.Columns.Add("CROID");
            CRADetails.Columns.Add("CRO");
            CRADetails.Columns.Add("CRA");
            CRADetails.Columns.Add("Email");
            CRADetails.Columns.Add("Phone");


            //Parse xml and bind CRO details
            if (CRAXML != string.Empty && CRAXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(CRAXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/NAME/CRA_NAME");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        CRADetails.Rows.Add(
                            node["i_CRA_ID"] == null ? "" : node["i_CRA_ID"].InnerText,
                            node["i_CRO_ID"] == null ? "" : node["i_CRO_ID"].InnerText,
                            node["CRO_NAME"] == null ? "" : node["CRO_NAME"].InnerText,
                            node["CRA_NAME"] == null ? "" : node["CRA_NAME"].InnerText,
                            node["CRA_Email"] == null ? "" : node["CRA_Email"].InnerText,
                            node["CRA_Phone"] == null ? "" : node["CRA_Phone"].InnerText
                          );
                    }
                }
            }

            rptrCRODetails.DataSource = CRADetails;
            rptrCRODetails.DataBind();

            //--CRO CRA Details


            //Study Section 
            if (selected.i_Study_Status_ID != 0)
                ddlStudyStatus.SelectedValue = Convert.ToString(selected.i_Study_Status_ID);

            if (selected.i_Project_Type_ID != 0)
                ddlTypeofStudy.SelectedValue = Convert.ToString(selected.i_Project_Type_ID);

            ddlApprovedStudyBugdet.SelectedValue = (selected.b_IsApproveProject ? "1" : "0");

            //Budget file grid
            string budgetFileXML = selected.STUDY_BUDGET_FILE_XML;
            //Parse xml and bind project details
            DataTable BudgetFileDetails = new DataTable();
            BudgetFileDetails.Columns.Add("s_Budget_Document_File");
            BudgetFileDetails.Columns.Add("s_Budget_Comments");
            BudgetFileDetails.Columns.Add("Name");




            //Parse xml and bind CRO details
            if (budgetFileXML != string.Empty && budgetFileXML != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(budgetFileXML)))
                {
                    XmlDocument xml = new XmlDocument();

                    xml.Load(reader);

                    XmlNodeList xmlNodeList = xml.SelectNodes("/SBF/STUDY_BUDGET_FILE");

                    foreach (XmlNode node in xmlNodeList)
                    {

                        BudgetFileDetails.Rows.Add(
                            node["s_Budget_Document_File"] == null ? "" : node["s_Budget_Document_File"].InnerText,
                            node["s_Budget_Comments"] == null ? "" : node["s_Budget_Comments"].InnerText,
                            node["s_Budget_Document_File"] == null ? "" : Common.GetFileName(node["s_Budget_Document_File"].InnerText)

                          );
                    }
                }
            }

            rptrBudgetFile.DataSource = BudgetFileDetails;
            rptrBudgetFile.DataBind();
            //End of Budget file grid


            //Archiving - Section 
            ddlAwaitingArchiving.SelectedValue = (selected.b_Awaiting_Archiving ? "1" : "0");

            txtEndDateArchiving.Text = selected.dt_Archiving_Enddate == DateTime.MinValue || selected.dt_Archiving_Enddate == null ? "" : Convert.ToDateTime(selected.dt_Archiving_Enddate).ToString("dd-MMM-yy");

            txtReason.Text = selected.s_Reason;

            txtOffSiteCompany.Text = selected.s_Offsite_Company;

            if (selected.s_AgreementFile != null && selected.s_AgreementFile != "")
            {

                btnDownIRBFile.Visible = true;

                hdnIRBFile.Value = selected.s_AgreementFile;

                string[] arr = selected.s_AgreementFile.Split('/');

                string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                btnDownIRBFile.Text = fName;

                btnDownIRBFile.Attributes.Add("filepath", selected.s_AgreementFile);
                //hdnIRBFileEnabled.Value = "Yes";
            }

            //Other Details 
            txtClinic1.Text = selected.s_Clinic1;

            txtClinic2.Text = selected.s_Clinic2;

            txtClinicDaysResearch.Text = Convert.ToString(selected.s_Research_Days);

            txtDurationofFollowups.Text = selected.s_Followup_Duratrion;

            txtRecruitStartDate.Text = selected.dt_Recruit_Start_Date == DateTime.MinValue || selected.dt_Recruit_Start_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_Start_Date).ToString("dd-MMM-yy");

            txtRecruitEndDate.Text = selected.dt_Recruit_End_Date == DateTime.MinValue || selected.dt_Recruit_End_Date == null ? "" : Convert.ToDateTime(selected.dt_Recruit_End_Date).ToString("dd-MMM-yy");

            txtTargetforTTSH.Text = Convert.ToString(selected.i_TTSH_Target);

            txtScreened.Text = Convert.ToString(selected.i_Screen_No);

            txtScreenFailure.Text = Convert.ToString(selected.i_Screen_Failure);

            txtRandomized.Text = Convert.ToString(selected.i_Randomized);

            txtCompleted.Text = Convert.ToString(selected.i_Completed);

            txtWithdrawal.Text = Convert.ToString(selected.i_Withdrawl);

            txtIRB.Text = Convert.ToString(selected.s_IRB_No);

            txtIRBExpiryDate.Text = selected.dt_Expiry_date == DateTime.MinValue || selected.dt_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Expiry_date).ToString("dd-MMM-yy");

            txtCTMExpiryDate.Text = selected.dt_CTM_Expiry_date == DateTime.MinValue || selected.dt_CTM_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTM_Expiry_date).ToString("dd-MMM-yy");

            txtCTCExpiryDate.Text = selected.dt_CTC_Expiry_date == DateTime.MinValue || selected.dt_CTC_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_CTC_Expiry_date).ToString("dd-MMM-yy");

            ddlCTMStatus.SelectedValue = (selected.b_CTM_Status ? "1" : "0");

            //Additional fields
            txtDateSentForArchiving.Text = selected.dt_Date_Sent_for_Archiving == DateTime.MinValue || selected.dt_Date_Sent_for_Archiving == null ? "" : Convert.ToDateTime(selected.dt_Date_Sent_for_Archiving).ToString("dd-MMM-yy");

            txtNumberOfBoxes.Text = Convert.ToString(selected.i_Number_of_Boxes);

            txtAmount.Text = selected.s_Amount;

            txtAgreementNumber.Text = selected.s_Agreement_Number;
            //Additional fields

            //Cupboard - Section (Blinded) 
            txtExpectedMonth.Text = selected.dt_Extended_Month_Blinded == DateTime.MinValue || selected.dt_Extended_Month_Blinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_Blinded).ToString("dd-MMM-yy");


            if (selected.s_LastUpdated_By_Blinded != "" && selected.s_LastUpdated_By_Blinded != null)
                txtUpdatedByBlinded.Text = selected.s_LastUpdated_By_Blinded;
            else
                txtUpdatedByBlinded.Text = LoginUser;

            if (selected.dt_LastUpdated_By_Blinded != null && selected.dt_LastUpdated_By_Blinded != DateTime.MinValue)
                txtLastUpdatedBlinded.Text = selected.dt_LastUpdated_By_Blinded == DateTime.MinValue || selected.dt_LastUpdated_By_Blinded == null ? "" : Convert.ToDateTime(selected.dt_LastUpdated_By_Blinded).ToString("dd-MMM-yy");
            else
                txtLastUpdatedBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");


            ddlCupboardNoBlinded.SelectedValue = selected.i_CupBoardno_Blinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_Blinded);

            /*End of Blinded Details*/

            /*UnBlinded Details*/

            //Drug Location - Section 
            txtDrugName.Text = selected.s_Drug_Name;

            txtDose.Text = selected.s_Drug_Dose;

            txtDateofExpiry.Text = selected.dt_Drug_Expiry_date == DateTime.MinValue || selected.dt_Drug_Expiry_date == null ? "" : Convert.ToDateTime(selected.dt_Drug_Expiry_date).ToString("dd-MMM-yy");

            if (selected.i_Drug_Location_ID != 0)
                ddlLocation.SelectedValue = Convert.ToString(selected.i_Drug_Location_ID);

            //Cupboard - Section (Un Blinded) 
            txtExpectedMonthUnBlinded.Text = selected.dt_Extended_Month_UnBlinded == DateTime.MinValue || selected.dt_Extended_Month_UnBlinded == null ? "" : Convert.ToDateTime(selected.dt_Extended_Month_UnBlinded).ToString("dd-MMM-yy");

            if (selected.s_LastUpdated_By_UnBlinded != "" && selected.s_LastUpdated_By_UnBlinded != null)
                txtUpdatedByUnblinded.Text = selected.s_LastUpdated_By_UnBlinded;
            else
                txtUpdatedByUnblinded.Text = LoginUser;

            if (selected.dt_LastUpdated_By_UnBlinded != null && selected.dt_LastUpdated_By_UnBlinded != DateTime.MinValue)
                txtLastUpdatedUnBlinded.Text = selected.dt_LastUpdated_By_UnBlinded == DateTime.MinValue || selected.dt_LastUpdated_By_UnBlinded == null ? "" : Convert.ToDateTime(selected.dt_LastUpdated_By_UnBlinded).ToString("dd-MMM-yy");
            else
                txtLastUpdatedUnBlinded.Text = DateTime.Now.ToString("dd-MMM-yy");

            ddlCupboardNoUnblinded.SelectedValue = selected.i_CupBoardno_UnBlinded == 0 ? "-1" : Convert.ToString(selected.i_CupBoardno_UnBlinded);

            /*End of UnBlinded Details*/

            //End of Fill the monthly details

            txtEntryMonth.Text = selected.dt_EntryForMonthBlinded == DateTime.MinValue || selected.dt_EntryForMonthBlinded == null ? "" : Convert.ToDateTime(selected.dt_EntryForMonthBlinded).ToString("dd-MMM-yy");

            CallJS("ApplyScript();validateOnEvents();monthClickProcessed();");
            //CallJS("DisableAllControl(1);");

        }

        protected void btnAddMoreBudgetFile_Click(object sender, EventArgs e)
        {

            Random ran = new Random(5);

            string randomNum = ran.ToString();

            //<asp:TextBox ID="txtBudgetFile1" class="txtUpload" runat="server"></asp:TextBox>
            //<asp:FileUpload ID="fuBudgetFile1" class="action" runat="server" />

            //TextBox
            TextBox txtBox = new TextBox();

            txtBox.Attributes.Add("class", "txtUpload Req");

            txtBox.ID = "txt" + randomNum;

            //FileUpload
            FileUpload fuCntrl = new FileUpload();
            fuCntrl.ID = "fu" + randomNum;
            fuCntrl.Attributes.Add("class", "action");



            string fileMarkup = "<p class='" + randomNum + "'>" +
                                    "<label>Study Budget Document <b>*</b></label>" +
                                    txtBox + //"<input class='txtUpload Req'  type='text' readonly='true'>"
                                    "<span class='btn btn-default btn-file action'>Browse..." +
                                     fuCntrl + //"<input title='Study Budget Document' class='action' type='file'>"
                                    "</span>" +
                                "</p>";


            string commentMarkup = "<p class='" + randomNum + "' >" +
                                 "<label>Comments for Budget</label>" +
                                 "<input title='Comments for Budget' class='ctlinput' type='text'>" +


                             "<a   onclick='deleteBudgetRow(" + randomNum + ") class='link' style='margin-left:5px;'>   Remove" +
                //"<img title="Delete" alt="" style="margin-left:10px;margin-top:-5px;background-color:grey;" src="../images/icon-delete.png">" +
                             "</a>" +
                        " </p>";

            string oldHtml = BudgetCommentContainer.InnerHtml + commentMarkup;

            BudgetCommentContainer.InnerHtml = oldHtml;

            oldHtml = "";

            oldHtml = BudgetFileContainer.InnerHtml + fileMarkup;

            BudgetFileContainer.InnerHtml = oldHtml;



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
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("SELECTED", UserID);
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
                                    if (element.cordinatorstatus.ToUpper() != "NEW")
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
                                        if (element.cordinatorstatus.ToUpper() != "NEW")
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
                                if (element.cordinatorstatus.ToUpper() != "NEW")
                                {
                                    element.Status = "Edit";
                                }
                                else
                                {
                                    element.Status = "New";
                                }
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

                //rptrProjectDetail.DataSource = lst; /*use the object according to your need*/
                //rptrProjectDetail.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("s_Display_Project_ID");
                dt.Columns.Add("s_Project_Title");
                dt.Columns.Add("s_Project_Category");
                dt.Columns.Add("Status");
                dt.Columns.Add("s_IRB_No");
                dt.Columns.Add("PI_Names");
                dt.Columns.Add("i_ID");
                dt.Columns.Add("Selected_ID");
                dt.Columns.Add("i_Project_ID");
                dt.Columns.Add("cordinatorstatus");
                dt.Columns.Add("Study_Status");

                rptrProjectDetail.DataSource = dt; /*use the object according to your need*/
                rptrProjectDetail.DataBind();

                //FillGrid();
            }

        }

        public void SearchBox_ClearClick(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void btnDownIRBFile_Click(object sender, EventArgs e)
        {
            try
            {
                Common.DownloadFileNew(hdnIRBFile.Value, Response);
                CallJS("hideLoading();");
            }
            catch (Exception ex)
            {

            }
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("DeleteSelected", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Selected Project Details Deleted Successfully..!!");
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