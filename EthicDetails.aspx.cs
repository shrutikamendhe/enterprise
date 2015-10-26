using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TTSHWeb.TTSHWCFReference;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;





namespace TTSHWeb
{


    public partial class EthicDetails : System.Web.UI.Page
    {

        int projectID = 0;
        string LoginUser = "";
        string LoginUserId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.ETHICS; /*setting filtercriteria*/
            SearchBox.ButtonSearchClick += new EventHandler(SearchBox_ButtonClick); /*adding events*/
            SearchBox.ButtonClearClick += new EventHandler(SearchBox_ClearClick); /*adding events*/

            if (!IsPostBack)
            {
                HdnId.Value = "0";
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
            try
            {



                TTSHWCFServiceClient client = new TTSHWCFServiceClient();


                List<Ethics_Grid> gridData = new List<Ethics_Grid>();
                gridData = client.Ethics_FillGrid().ToList();

                try
                {
                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("ETHICS", UserID);
                    DataOwner_Entity[] oDataOwner = client.GetAllDataOwner("TAdmin");

                    var AdminArray = (from s in oDataOwner
                                      select s.GUID).ToList();

                    bool IsAdmin = AdminArray.Contains(UserID);

                    if (IsAdmin == false)
                    {
                        List<Ethics_Grid> oNewGrid = new List<Ethics_Grid>();
                        if (gridData != null && gridData.Count() > 0 && oDOList != null && oDOList.Count() > 0)
                        {
                            var v = gridData.Select(z => z.Status).Distinct().ToList();
                            oNewGrid = gridData.Where(z => z.Status.ToUpper() == "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID == z.s_Display_Project_ID)).ToList();
                            gridData.RemoveAll(z => z.Status.ToUpper() == "NEW");
                            gridData.AddRange(oNewGrid);
                            gridData.Where(z => z.Status.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim() != z.s_Display_Project_ID.ToUpper().Trim())).ToList().ForEach(i => i.Status = "View");
                            gridData.Where(z => z.Status.ToUpper() != "NEW").Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim() == z.s_Display_Project_ID.ToUpper().Trim())).ToList().ForEach(i => i.Status = "Edit");
                            //gridData.Where(z => "EDIT,VIEW".Contains(z.Status.ToUpper())).Where(z => oDOList.Any(x => x.s_DisplayProject_ID.ToUpper().Trim() == z.s_Display_Project_ID.ToUpper().Trim())).ToList().ForEach(i => i.Status = "Edit");
                            gridData = gridData.OrderByDescending(z => z.i_ID).ToList();
                        }
                        else if (gridData != null && gridData.Count() > 0)
                        {
                            gridData.ForEach(x => x.Status = "View");
                            gridData.OrderByDescending(z => z.i_ID);
                        }
                    }
                    else
                    {
                        gridData.Where(z => z.Status.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "View");
                        gridData.Where(z => z.Status.ToUpper() != "NEW").ToList().ForEach(i => i.Status = "Edit");
                        gridData = gridData.OrderByDescending(z => z.i_ID).ToList();
                    }
                }
                catch (Exception ex1)
                {

                }

                rptrProjectDetail.DataSource = gridData;
                rptrProjectDetail.DataBind();
            }
            catch (Exception ex)
            {


            }

        }

        public void PopulateDropDown()
        {

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlIRBStatus, DropDownName.IRB_Status, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlIRBType, DropDownName.IRB_Type, "");

            //  Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlProjectStatus, DropDownName.Project_Status, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlProjectCategory, DropDownName.Project_Category, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlEthicsReview, DropDownName.Ethics_Review, "");

            Common.FillComboNew((System.Web.UI.WebControls.DropDownList)ddlPeriodOfInsurance, DropDownName.Period_of_Insurance, "");





        }

        protected void btnSaveEthics_Click(object sender, EventArgs e)
        {

            try
            {


                TTSHWCFReference.Ethics_Details ethics = new Ethics_Details();

                //File UpLoadNew
                string msg = "";
                if (fuIRBFile.HasFile && hdnIRBFileEnabled.Value == "Yes")
                {
                    string[] arr = Common.UpLoadNew(fuIRBFile, Common.FolderLocation.EthicFiles);

                    if (arr.Count() != 0)
                    {
                        hdnIRBFile.Value = arr[0];
                    }
                }

                if (fuInsuranceFile.HasFile && hdnInsuranceFileEnabled.Value == "Yes")
                {
                    string[] arr = Common.UpLoadNew(fuInsuranceFile, Common.FolderLocation.EthicFiles);

                    if (arr.Count() != 0)
                    {
                        hdnInsuranceFile.Value = arr[0];
                    }
                }
                //End of File UpLoadNew

                ethics.b_CRIO_culled = (ddlCRIOCulled.SelectedValue == "-1" || ddlCRIOCulled.SelectedValue == "0" ? false : true);

                if (ddlChildBearing.SelectedValue == "-1") ethics.b_IsChildBearing = null;
                else ethics.b_IsChildBearing = (ddlChildBearing.SelectedValue == "1") ? true : false;

                ethics.b_IsClinical_Trial_Insurance = (ddlClinicalTrialInsurance.SelectedValue == "-1" || ddlClinicalTrialInsurance.SelectedValue == "0" ? false : true);

                if (ddlRenewal.SelectedValue == "-1") ethics.b_IsRenewal = null;
                else ethics.b_IsRenewal = (ddlRenewal.SelectedValue == "1") ? true : false;

                ethics.dt_Created_Date = DateTime.Now;

                ethics.s_CreatedBy_ID = User.Identity.Name;

                if (txtCRIOCulledDate.Text.Trim() == "" || hdnRecordCulledEnabled.Value == "No")
                    ethics.dt_CRIO_culled_date = null;
                else
                    ethics.dt_CRIO_culled_date = Convert.ToDateTime(txtCRIOCulledDate.Text);

                ethics.s_Insurance_Period = ddlPeriodOfInsurance.SelectedValue == "-1" ? "" : ddlPeriodOfInsurance.SelectedValue;

                if (txtIRBApproveDate.Text.Trim() == "" || hdnIRBFileEnabled.Value == "No")
                    ethics.dt_IRB_Approve_Date = null;
                else
                    ethics.dt_IRB_Approve_Date = Convert.ToDateTime(txtIRBApproveDate.Text);


                if (txtIRBApproveEndDate.Text.Trim() == "" || hdnIRBFileEnabled.Value == "No")
                    ethics.dt_IRB_Approve_Enddate = null;
                else
                    ethics.dt_IRB_Approve_Enddate = Convert.ToDateTime(txtIRBApproveEndDate.Text);


                if (txtNewStudyEndDate.Text.Trim() == "" || hdnNewStudyEndDtEnabled.Value == "No")
                    ethics.dt_NewStudy_End_date = null;
                else
                    ethics.dt_NewStudy_End_date = Convert.ToDateTime(txtNewStudyEndDate.Text);


                //if (dtCompleted_Withdrawn.Text.Trim() == "" || hdnDtTerminatedEnabled.Value == "No")
                //    ethics.dt_Project_Status_date = null;
                //else
                ethics.dt_Project_Status_date = Convert.ToDateTime(dtCompleted_Withdrawn.Text);


                ethics.i_IRB_Status_ID = (ddlIRBStatus.SelectedValue == "-1" || ddlIRBStatus.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlIRBStatus.SelectedValue));

                ethics.i_IRB_Type_ID = (ddlIRBType.SelectedValue == "-1" || ddlIRBType.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlIRBType.SelectedValue));

                //  ethics.i_Project_Status_ID = (ddlProjectStatus.SelectedValue == "-1" || ddlProjectStatus.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlProjectStatus.SelectedValue));

                ethics.i_EthicsReview_ID = (ddlEthicsReview.SelectedValue == "-1" || ddlEthicsReview.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlEthicsReview.SelectedValue));

                ethics.i_Sub_Recruited = (txtGlobalSubRecruited.Text.Trim() == "" ? 0 : Convert.ToInt32(txtGlobalSubRecruited.Text));

                ethics.i_Sub_targeted = (txtGlobalSubTargeted.Text.Trim() == "" ? 0 : Convert.ToInt32(txtGlobalSubTargeted.Text));

                ethics.i_Sub_Targeted_TTSH = (txtSubTargeted.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSubTargeted.Text));

                ethics.i_Sub_Recruited_TTSH = (txtSubRecruited.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSubRecruited.Text));

                ethics.s_Comments = txtComments.Text;

                if (hdnInsuranceFile.Value != null && hdnInsuranceFile.Value != "")
                    ethics.s_Insurance_file = hdnInsuranceFile.Value;

                if (hdnIRBFile.Value != null && hdnIRBFile.Value != "")
                    ethics.s_IRB_File = hdnIRBFile.Value;
                if (txtIRBNumber.Text != "")
                {
                    hdnIRBFileEnabled.Value = "Yes";
                }

                ethics.s_IRB_No = hdnIRBFileEnabled.Value == "Yes" ? txtIRBNumber.Text : "";

                ethics.s_Remarks = txtEthicalNotes.Text;

                ethics.CO_Investigator = txtCoInvestigator.Text;

                int ProjID = Convert.ToInt32(HttpContext.Current.Session["ProjectID"]);

                ethics.i_Project_ID = ProjID;

                ethics.i_Project_Category_ID = Convert.ToInt32(ddlProjectCategory.SelectedValue);

                ethics.s_Project_Alias1 = txtAlias1.Text;

                ethics.s_Project_Alias2 = txtAlias2.Text;

                ethics.s_Short_Title = txtShortTitle.Text;

                List<Project_PI> PIs = new List<Project_PI>();
                string[] splitPiId = HdnPi_ID.Value.Split(',');
                foreach (string pi in splitPiId)
                {
                    PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = ProjID });
                }

                ethics.Project_PIs = PIs.ToArray();

                ethics.dt_Ethics_Start_Date = Convert.ToDateTime(txtEthicsStartDate.Text);

                //--------UID and UName----
                ethics.UName = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString();
                ethics.UID = Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString();
                //------END

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                string result = client.Ethics_Details(ethics, Mode.Insert);
                if (CheckDML(result))
                {
                    EthicsContainer.Visible = false;

                    projectGrid.Visible = true;

                    FillGrid();

                    //CallJS("MessageBoxEvent('Ethics saved successfully','PerformCancel();');");
                    TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                    t.Text = "";
                    CallJS("MessageBox('Ethics saved successfully');");
                    HdnId.Value = "0";
                }
              
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected bool CheckDML(string result)
        {
            try
            {


                var x = (result.Length > 0) ? result.Split('|')[0] : "";
                var y = (result.Length > 1) ? result.Split('|')[1] : "";
                if (x.Trim().ToLower() == "success" && y.CheckInt() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        protected void ddlDeptAddMorePI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddl = (DropDownList)sender;
            //Common.FillComboNew((DropDownList)ddlPIAddMorePI, DropDownName.PI, ddl.SelectedValue);
        }

        protected void EditLink_Command(object sender, CommandEventArgs e)
        {


            try
            {




                projectGrid.Visible = false;

                EthicsContainer.Visible = true;

                txtEthicsStartDate.Enabled = false;

                ResetControls();

                PopulateDropDown();

                btnUpdateEthics.Visible = true;
                btnSaveEthics.Visible = false;

                TTSHWCFReference.Ethics_Details ethics = new Ethics_Details();

                int ethicsID = Convert.ToInt32(e.CommandArgument);
                HdnId.Value = Convert.ToString(ethicsID);
                // Session["EthicsID"] = ethicsID;

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                ethics = client.GetEthics_DetailsByID(ethicsID);
                HttpContext.Current.Session["ProjectID"] = ethics.i_Project_ID;

                /*Bind values to controls*/

                //Set Text fields
                string CoInvest = ethics.CO_Investigator.ToString();

                //Parse xml and bind co investigator details
                if (CoInvest != string.Empty)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(CoInvest)))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(reader);

                        XmlNodeList xmlNodeList = xml.SelectNodes("/COINVESTIGATOR_D/COINVESTIGATOR");
                        foreach (XmlNode node in xmlNodeList)
                        {

                            string id = node["ID"] == null ? "" : node["ID"].InnerText;

                            if (id == "" || id == "0")
                            {
                                txtCoInvestigator.Enabled = false;
                            }
                            else
                            {
                                txtCoInvestigator.Text = node["Coinves_name"] == null ? "" : node["Coinves_name"].InnerText;
                                txtCoInvestigator.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    txtCoInvestigator.Enabled = false;
                }



                txtComments.Text = ethics.s_Comments;

                txtCRIOCulledDate.Text = (ethics.dt_CRIO_culled_date == null || ethics.dt_CRIO_culled_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_CRIO_culled_date).ToString("dd-MMM-yyyy"));

                txtEthicalNotes.Text = ethics.s_Remarks;

                txtGlobalSubRecruited.Text = ethics.i_Sub_Recruited.ToString();

                txtGlobalSubTargeted.Text = ethics.i_Sub_targeted.ToString();

                txtSubRecruited.Text = ethics.i_Sub_Recruited_TTSH.ToString();

                txtSubTargeted.Text = ethics.i_Sub_Targeted_TTSH.ToString();

                ddlPeriodOfInsurance.SelectedValue = ethics.s_Insurance_Period == "" || ethics.s_Insurance_Period == null ? "-1" : ethics.s_Insurance_Period;

                txtIRBApproveDate.Text = (ethics.dt_IRB_Approve_Date == null || ethics.dt_IRB_Approve_Date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_IRB_Approve_Date).ToString("dd-MMM-yyyy"));

                txtIRBApproveEndDate.Text = (ethics.dt_IRB_Approve_Enddate == null || ethics.dt_IRB_Approve_Enddate == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_IRB_Approve_Enddate).ToString("dd-MMM-yyyy"));

                txtIRBNumber.Text = ethics.s_IRB_No;

                //   dtCompleted_Withdrawn.Text = (ethics.dt_Project_Status_date == null || ethics.dt_Project_Status_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_Project_Status_date).ToString("dd-MMM-yyyy"));
                dtCompleted_Withdrawn.Text = Convert.ToDateTime(ethics.dt_Project_Status_date).ToString("dd-MMM-yyyy");
                if (ethics.s_IRB_File != null && ethics.s_IRB_File != "")
                {

                    btnDownIRBFile.Visible = true;

                    hdnIRBFile.Value = ethics.s_IRB_File;

                    string[] arr = ethics.s_IRB_File.Split('/');

                    string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                    btnDownIRBFile.Text = fName;
                    hdnIRBFileEnabled.Value = "Yes";
                }

                if (ethics.s_Insurance_file != null && ethics.s_Insurance_file != "")
                {
                    btnDownInsuranceFile.Visible = true;

                    hdnInsuranceFile.Value = ethics.s_Insurance_file;

                    string[] arr = ethics.s_Insurance_file.Split('/');

                    string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                    btnDownInsuranceFile.Text = fName;

                }

                txtNewStudyEndDate.Text = (ethics.dt_NewStudy_End_date == null || ethics.dt_NewStudy_End_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_NewStudy_End_date).ToString("dd-MMM-yyyy"));

                // HttpContext.Current.Session["ProjectID"] = ethics.i_Project_ID;

                // bProjectID.InnerText = "Project ID: " + ethics.i_Project_ID;

                string projectXML = ethics.Project_Data;

                //Parse xml and bind project details
                if (projectXML != string.Empty)
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
                            bProjectID.InnerText = "Project ID: " + node["s_Display_Project_ID"].InnerText;

                        }
                    }
                }

                //Set Drop drown fields
                // ddlProjectStatus.SelectedValue = ethics.i_Project_Status_ID.ToString();

                ddlEthicsReview.SelectedValue = ethics.i_EthicsReview_ID.ToString();

                ddlIRBStatus.SelectedValue = ethics.i_IRB_Status_ID.ToString();

                ddlIRBType.SelectedValue = ethics.i_IRB_Type_ID.ToString();

                ddlRenewal.SelectedValue = (ethics.b_IsRenewal == null) ? "-1" : (ethics.b_IsRenewal == true) ? "1" : "0";

                ddlCRIOCulled.SelectedValue = ethics.b_CRIO_culled ? "1" : "0";

                ddlClinicalTrialInsurance.SelectedValue = ethics.b_IsClinical_Trial_Insurance ? "1" : "0";

                ddlChildBearing.SelectedValue = (ethics.b_IsChildBearing == null) ? "-1" : ethics.b_IsChildBearing == true ? "1" : "0";

                txtEthicsStartDate.Text = Convert.ToDateTime(ethics.dt_Ethics_Start_Date).ToString("dd-MMM-yyyy");

                FillPIGrid(ethics.Dept_PI.ToList());

            }
            catch
            {

            }


        }

        private void FillPIGrid(List<PI_Master> listPI)
        {
            rptrPIDetails.DataSource = listPI;
            rptrPIDetails.DataBind();

        }

        protected void NewLink_Command(object sender, CommandEventArgs e)
        {
            try
            {
                projectGrid.Visible = false;

                //To reset all the controls
                ResetControls();

                EthicsContainer.Visible = true;

                PopulateDropDown();

                btnUpdateEthics.Visible = false;

                btnSaveEthics.Visible = true;

                txtEthicsStartDate.Enabled = true;

                TTSHWCFReference.Project_Master project = new Project_Master();

                int project_id = Convert.ToInt32(e.CommandArgument);

                HttpContext.Current.Session["ProjectID"] = project_id;


                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                project = client.GetProject_MasterDetailsByID(project_id);

                txtProjectTitle.Text = project.s_Project_Title;

                txtShortTitle.Text = project.s_Short_Title;

                ddlProjectCategory.SelectedValue = project.i_Project_Category_ID.ToString();

                txtAlias1.Text = project.s_Project_Alias1;

                txtAlias2.Text = project.s_Project_Alias2;
                bProjectID.InnerText = "Project ID: " + project.s_Display_Project_ID;
                txtSubRecruited.Text = "0";

                txtSubTargeted.Text = "0";

                txtGlobalSubRecruited.Text = "0";

                txtGlobalSubTargeted.Text = "0";

                txtEthicsStartDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                string CoInvest = project.s_Coinvestigator.ToString();

                //Parse xml and bind co investigator details
                if (CoInvest != string.Empty)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(CoInvest)))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(reader);

                        XmlNodeList xmlNodeList = xml.SelectNodes("/COINVESTIGATOR_D/COINVESTIGATOR");
                        foreach (XmlNode node in xmlNodeList)
                        {

                            string id = node["ID"] == null ? "" : node["ID"].InnerText;

                            if (id == "" || id == "0")
                            {
                                txtCoInvestigator.Enabled = false;
                            }
                            else
                            {
                                txtCoInvestigator.Text = node["Coinves_name"] == null ? "" : node["Coinves_name"].InnerText;
                                txtCoInvestigator.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    txtCoInvestigator.Enabled = false;
                }

                txtIRBNumber.Text = project.s_IRB_No.ToString();

                FillPIGrid(project.DEPT_PI.ToList());

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void ResetControls()
        {
            //CallJS("ClearAll('new')");

            foreach (Control ctrl in txtGlobalSubTargeted.Parent.Controls)
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
            btnDownInsuranceFile.Text = "";
            btnDownIRBFile.Text = "";

            btnDownInsuranceFile.Visible = false;
            btnDownIRBFile.Visible = false;

            //To reset hidded fields
            HdnDeptId.Value = "";
            hdnInsuranceFile.Value = "";
            hdnIRBFile.Value = "";
            HdnNewDeptId.Value = "";
            HdnPi_ID.Value = "";
            HdnpiId.Value = "";

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }

        protected void btnUpdateEthics_Click(object sender, EventArgs e)
        {
            try
            {

                TTSHWCFReference.Ethics_Details ethics = new Ethics_Details();

                //File UpLoadNew
                string msg = "";
                if (fuIRBFile.HasFile && hdnIRBFileEnabled.Value == "Yes")
                {
                    string[] arr = Common.UpLoadNew(fuIRBFile, Common.FolderLocation.EthicFiles);

                    if (arr.Count() != 0)
                    {
                        hdnIRBFile.Value = arr[0];
                    }
                }

                if (fuInsuranceFile.HasFile && hdnInsuranceFileEnabled.Value == "Yes")
                {
                    string[] arr = Common.UpLoadNew(fuInsuranceFile, Common.FolderLocation.EthicFiles);

                    if (arr.Count() != 0)
                    {
                        hdnInsuranceFile.Value = arr[0];
                    }
                }
                //End of File UpLoadNew

                ethics.b_CRIO_culled = (ddlCRIOCulled.SelectedValue == "-1" || ddlCRIOCulled.SelectedValue == "0" ? false : true);

                if (ddlChildBearing.SelectedValue == "-1") ethics.b_IsChildBearing = null;
                else ethics.b_IsChildBearing = (ddlChildBearing.SelectedValue == "1") ? true : false;

                ethics.b_IsClinical_Trial_Insurance = (ddlClinicalTrialInsurance.SelectedValue == "-1" || ddlClinicalTrialInsurance.SelectedValue == "0" ? false : true);

                if (ddlRenewal.SelectedValue == "-1") ethics.b_IsRenewal = null;
                else ethics.b_IsRenewal = (ddlRenewal.SelectedValue == "1") ? true : false;

                ethics.dt_Created_Date = DateTime.Now;

                ethics.s_CreatedBy_ID = User.Identity.Name;

                if (txtCRIOCulledDate.Text.Trim() == "" || hdnRecordCulledEnabled.Value == "No")
                    ethics.dt_CRIO_culled_date = null;
                else
                    ethics.dt_CRIO_culled_date = Convert.ToDateTime(txtCRIOCulledDate.Text);

                ethics.s_Insurance_Period = ddlPeriodOfInsurance.SelectedValue == "-1" ? "" : ddlPeriodOfInsurance.SelectedValue;

                if (txtIRBApproveDate.Text.Trim() == "" || hdnIRBFileEnabled.Value == "No")
                    ethics.dt_IRB_Approve_Date = null;
                else
                    ethics.dt_IRB_Approve_Date = Convert.ToDateTime(txtIRBApproveDate.Text);


                if (txtIRBApproveEndDate.Text.Trim() == "" || hdnIRBFileEnabled.Value == "No")
                    ethics.dt_IRB_Approve_Enddate = null;
                else
                    ethics.dt_IRB_Approve_Enddate = Convert.ToDateTime(txtIRBApproveEndDate.Text);


                if (txtNewStudyEndDate.Text.Trim() == "" || hdnNewStudyEndDtEnabled.Value == "No")
                    ethics.dt_NewStudy_End_date = null;
                else
                    ethics.dt_NewStudy_End_date = Convert.ToDateTime(txtNewStudyEndDate.Text);


                //if (dtCompleted_Withdrawn.Text.Trim() == "" || hdnDtTerminatedEnabled.Value == "No")
                //    ethics.dt_Project_Status_date = null;
                //else
                ethics.dt_Project_Status_date = Convert.ToDateTime(dtCompleted_Withdrawn.Text);

                ethics.i_ID = Convert.ToInt32(HdnId.Value);

                ethics.i_IRB_Status_ID = (ddlIRBStatus.SelectedValue == "-1" || ddlIRBStatus.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlIRBStatus.SelectedValue));

                ethics.i_IRB_Type_ID = (ddlIRBType.SelectedValue == "-1" || ddlIRBType.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlIRBType.SelectedValue));

                //    ethics.i_Project_Status_ID = (ddlProjectStatus.SelectedValue == "-1" || ddlProjectStatus.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlProjectStatus.SelectedValue));

                ethics.i_EthicsReview_ID = (ddlEthicsReview.SelectedValue == "-1" || ddlEthicsReview.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlEthicsReview.SelectedValue));

                ethics.i_Sub_Recruited = (txtGlobalSubRecruited.Text.Trim() == "" ? 0 : Convert.ToInt32(txtGlobalSubRecruited.Text));

                ethics.i_Sub_targeted = (txtGlobalSubTargeted.Text.Trim() == "" ? 0 : Convert.ToInt32(txtGlobalSubTargeted.Text));

                ethics.i_Sub_Targeted_TTSH = (txtSubTargeted.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSubTargeted.Text));

                ethics.i_Sub_Recruited_TTSH = (txtSubRecruited.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSubRecruited.Text));

                ethics.s_Comments = txtComments.Text;

                if (hdnInsuranceFile.Value != null && hdnInsuranceFile.Value != "")
                    ethics.s_Insurance_file = hdnInsuranceFile.Value;

                if (hdnIRBFile.Value != null && hdnIRBFile.Value != "")
                    ethics.s_IRB_File = hdnIRBFile.Value;

                if (txtIRBNumber.Text != "")
                {
                    hdnIRBFileEnabled.Value = "Yes";
                }
                ethics.s_IRB_No = hdnIRBFileEnabled.Value == "Yes" ? txtIRBNumber.Text : "";

                ethics.s_Remarks = txtEthicalNotes.Text;

                ethics.CO_Investigator = txtCoInvestigator.Text;

                int ProjID = Convert.ToInt32(HttpContext.Current.Session["ProjectID"]);

                ethics.i_Project_ID = ProjID;

                ethics.i_Project_Category_ID = Convert.ToInt32(ddlProjectCategory.SelectedValue);

                ethics.s_Project_Alias1 = txtAlias1.Text;

                ethics.s_Project_Alias2 = txtAlias2.Text;

                ethics.s_Short_Title = txtShortTitle.Text;

                List<Project_PI> PIs = new List<Project_PI>();

                string[] splitPiId = HdnPi_ID.Value.Split(',');

                foreach (string pi in splitPiId)
                {
                    PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = ProjID });

                }

                ethics.Project_PIs = PIs.ToArray();

                ethics.dt_Ethics_Start_Date = Convert.ToDateTime(txtEthicsStartDate.Text);


                TTSHWCFServiceClient client = new TTSHWCFServiceClient();


                string result = client.Ethics_Details(ethics, Mode.Update);
                if (CheckDML(result))
                {
                    EthicsContainer.Visible = false;

                    projectGrid.Visible = true;

                    FillGrid();

                    //CallJS("MessageBoxEvent('Ethics saved successfully','PerformCancel();');");
                    TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
                    t.Text = "";
                    CallJS("MessageBox('Ethics updated successfully');");
                    HdnId.Value = "0";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// UpLoadNew IRB File.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIRBFile_Click(object sender, EventArgs e)
        {
            try
            {

                string msg = Common.SaveFile(fuIRBFile);

                if (msg.Split('|')[0].ToLower() == "file exists")
                {
                    CallJS("alert('File with same name already exists')");
                }
                else if (msg.Split('|')[0].ToLower() == "invalid file")
                {

                }
                else if (msg.Split('|')[0].ToLower() == "select file")
                {

                }
                else if (msg.Split('|')[0].ToLower() == "file saved")
                {
                    string fileLoc = msg.Split('|')[1];

                    hdnIRBFile.Value = fileLoc;

                    string[] arr = fileLoc.Split('/');

                    btnDownIRBFile.Text = arr[arr.Length - 1];
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnDownIRBFile1_Click(object sender, EventArgs e)
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

        protected void btnInsuranceFile_Click(object sender, EventArgs e)
        {


            try
            {

                string msg = Common.SaveFile(fuInsuranceFile);
                if (msg.Split('|')[0].ToLower() == "file exists")
                {
                    CallJS("alert('File with same name already exists')");
                }
                else if (msg.Split('|')[0].ToLower() == "invalid file")
                {

                }
                else if (msg.Split('|')[0].ToLower() == "file saved")
                {
                    string fileLoc = msg.Split('|')[1];
                    hdnInsuranceFile.Value = fileLoc;
                    string[] arr = fileLoc.Split('/');
                    btnDownInsuranceFile.Text = arr[arr.Length - 1];
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnCancelEthics_Click(object sender, EventArgs e)
        {
            EthicsContainer.Visible = false;
            projectGrid.Visible = true;
            CallJS("ClearAll('new')");
            TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            t.Text = "";
            FillGrid();

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

        [WebMethod]
        [ScriptMethod]
        public static string GetPI_MasterDetailsByID(int ID)
        {
            string Result = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                Result = cl.GetPI_MasterDetailsByID(ID);
            }
            catch (Exception)
            {

                Result = "";
            }
            return Result;
        }
        #endregion

        protected void btnSaveNewPI_Click(object sender, EventArgs e)
        {

            try
            {
                //To Restrict the Save/Update/Delete on page refresh.
                //if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
                //{
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

        protected void ViewLink_Command(object sender, CommandEventArgs e)
        {
            try
            {
                projectGrid.Visible = false;

                EthicsContainer.Visible = true;

                ResetControls();

                PopulateDropDown();

                btnUpdateEthics.Visible = true;

                btnSaveEthics.Visible = false;

                TTSHWCFReference.Ethics_Details ethics = new Ethics_Details();

                int ethicsID = Convert.ToInt32(e.CommandArgument);
                HdnId.Value = Convert.ToString(ethicsID);
                // Session["EthicsID"] = ethicsID;

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                ethics = client.GetEthics_DetailsByID(ethicsID);

                /*Bind the values to controls*/

                //Set Text fields
                string CoInvest = ethics.CO_Investigator.ToString();

                //Parse xml and bind co investigator details
                if (CoInvest != string.Empty)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(CoInvest)))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(reader);

                        XmlNodeList xmlNodeList = xml.SelectNodes("/COINVESTIGATOR_D/COINVESTIGATOR");
                        foreach (XmlNode node in xmlNodeList)
                        {

                            string id = node["ID"] == null ? "" : node["ID"].InnerText;

                            if (id == "" || id == "0")
                            {
                                txtCoInvestigator.Enabled = false;
                            }
                            else
                            {
                                txtCoInvestigator.Text = node["Coinves_name"] == null ? "" : node["Coinves_name"].InnerText;
                                txtCoInvestigator.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    txtCoInvestigator.Enabled = false;
                }

                txtComments.Text = ethics.s_Comments;

                txtCRIOCulledDate.Text = (ethics.dt_CRIO_culled_date == null || ethics.dt_CRIO_culled_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_CRIO_culled_date).ToString("dd-MMM-yyyy"));

                txtEthicalNotes.Text = ethics.s_Remarks;

                txtGlobalSubRecruited.Text = ethics.i_Sub_Recruited.ToString();

                txtGlobalSubTargeted.Text = ethics.i_Sub_targeted.ToString();

                txtSubRecruited.Text = ethics.i_Sub_Recruited_TTSH.ToString();

                txtSubTargeted.Text = ethics.i_Sub_Targeted_TTSH.ToString();

                ddlPeriodOfInsurance.SelectedValue = ethics.s_Insurance_Period == "" || ethics.s_Insurance_Period == null ? "-1" : ethics.s_Insurance_Period;

                txtIRBApproveDate.Text = (ethics.dt_IRB_Approve_Date == null || ethics.dt_IRB_Approve_Date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_IRB_Approve_Date).ToString("dd-MMM-yyyy"));

                txtIRBApproveEndDate.Text = (ethics.dt_IRB_Approve_Enddate == null || ethics.dt_IRB_Approve_Enddate == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_IRB_Approve_Enddate).ToString("dd-MMM-yyyy"));

                txtIRBNumber.Text = ethics.s_IRB_No;

                //dtCompleted_Withdrawn.Text = (ethics.dt_Project_Status_date == null || ethics.dt_Project_Status_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_Project_Status_date).ToString("dd-MMM-yyyy"));
                dtCompleted_Withdrawn.Text = Convert.ToDateTime(ethics.dt_Project_Status_date).ToString("dd-MMM-yyyy");
                if (ethics.s_IRB_File != null && ethics.s_IRB_File != "")
                {

                    btnDownIRBFile.Visible = true;

                    hdnIRBFile.Value = ethics.s_IRB_File;

                    string[] arr = ethics.s_IRB_File.Split('/');

                    string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                    btnDownIRBFile.Text = fName;
                    hdnIRBFileEnabled.Value = "Yes";
                }

                if (ethics.s_Insurance_file != null && ethics.s_Insurance_file != "")
                {
                    btnDownInsuranceFile.Visible = true;

                    hdnInsuranceFile.Value = ethics.s_Insurance_file;

                    string[] arr = ethics.s_Insurance_file.Split('/');

                    string fName = arr[arr.Length - 1].Split('~')[0] + "." + arr[arr.Length - 1].Split('.')[arr[arr.Length - 1].Split('.').Length - 1];

                    btnDownInsuranceFile.Text = fName;

                }

                txtNewStudyEndDate.Text = (ethics.dt_NewStudy_End_date == null || ethics.dt_NewStudy_End_date == DateTime.MinValue ? "" : Convert.ToDateTime(ethics.dt_NewStudy_End_date).ToString("dd-MMM-yyyy"));

                HttpContext.Current.Session["ProjectID"] = ethics.i_Project_ID;



                string projectXML = ethics.Project_Data;

                //Parse xml and bind project details
                if (projectXML != string.Empty)
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
                            bProjectID.InnerText = "Project ID: " + node["s_Display_Project_ID"].InnerText;

                        }
                    }
                }

                //Set Drop drown fields
                //  ddlProjectStatus.SelectedValue = ethics.i_Project_Status_ID.ToString();

                ddlEthicsReview.SelectedValue = ethics.i_EthicsReview_ID.ToString();

                ddlIRBStatus.SelectedValue = ethics.i_IRB_Status_ID.ToString();

                ddlIRBType.SelectedValue = ethics.i_IRB_Type_ID.ToString();
                ddlRenewal.SelectedValue = (ethics.b_IsRenewal == null) ? "-1" : (ethics.b_IsRenewal == true) ? "1" : "0";

                ddlCRIOCulled.SelectedValue = ethics.b_CRIO_culled ? "1" : "0";

                ddlClinicalTrialInsurance.SelectedValue = ethics.b_IsClinical_Trial_Insurance ? "1" : "0";

                ddlChildBearing.SelectedValue = (ethics.b_IsChildBearing == null) ? "-1" : ethics.b_IsChildBearing == true ? "1" : "0";

                txtEthicsStartDate.Text = Convert.ToDateTime(ethics.dt_Ethics_Start_Date).ToString("dd-MMM-yyyy");

                FillPIGrid(ethics.Dept_PI.ToList());

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableAllControl", "DisableAllControl();", true);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void backToGrid_Click(object sender, EventArgs e)
        {
            //TextBox t = ((TextBox)(SearchBox.FindControl("txtSearch")));
            //t.Text = "";
            HdnId.Value = "0";
            EthicsContainer.Visible = false;
            projectGrid.Visible = true;
        }

        public void CallJS(string script, string Name = "")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), Name, script, true);
        }

        protected void btnDownInsuranceFile_Click(object sender, EventArgs e)
        {

            try
            {
                Common.DownloadFileNew(hdnInsuranceFile.Value, Response);
                CallJS("hideLoading();");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnAddNewProject_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmProject_Master.aspx?NewPage=1");
        }

        protected void SearchBox_ButtonClick(object sender, EventArgs e)
        {
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text; /*alternate way of getting search value */

            if (string.IsNullOrEmpty(SearchBox.ErrorString)) /* errors will be available in ErrorString property*/
            {
                TTSHWCFReference.Search[] lst = SearchBox.SearchOutput; /*to get list of output*/
                /*Data Owner wise search*/
                TTSHWCFServiceClient client = new TTSHWCFServiceClient();
                client.Open();

                try
                {


                    string UserID = Convert.ToString(Session["UserID"]).ToUpper();
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("ETHICS", UserID);
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
                            if (oDOList == null && oDOList.Count() > 0)
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

                        rptrProjectDetail.DataSource = oOldSearch; /*use the object according to your need*/
                        rptrProjectDetail.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                client.Close();
                /*Data Owner wise search*/
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("s_Display_Project_ID");
                dt.Columns.Add("s_Project_Title");
                dt.Columns.Add("s_Project_Category");
                dt.Columns.Add("Project_Status");
                dt.Columns.Add("s_IRB_No");
                dt.Columns.Add("PI_Names");
                dt.Columns.Add("i_ID");
                dt.Columns.Add("Ethics_ID");

                rptrProjectDetail.DataSource = dt; /*use the object according to your need*/
                rptrProjectDetail.DataBind();

                //FillGrid();
            }

        }

        public void SearchBox_ClearClick(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            string rs = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                rs = cl.GetValidate("DeleteEthics", Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserID"]), "").ToString(), Common.iffBlank(Convert.ToString(HttpContext.Current.Session["UserName"]), "").ToString(), HdnId.Value, "");
                if (rs != "")
                {
                    this.MsgBox("Ethics Details Deleted Successfully..!!");
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