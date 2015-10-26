using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TTSHWeb.TTSHWCFReference;
namespace TTSHWeb
{
    public partial class frmGrantSeniorCSCS : System.Web.UI.Page
    {
        #region " Page Load "

        string LoginUser = "";
        string LoginUserId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearHDN();
                ShowPanel(); FillMainGrid();
            }

            if (HttpContext.Current.Session["UserName"] != null)
            {
                LoginUser = HttpContext.Current.Session["UserName"].ToString();

            }
            if (HttpContext.Current.Session["UserID"] != null)
            {
                LoginUserId = HttpContext.Current.Session["UserID"].ToString();

            }
        }
        #endregion


        #region " Methods and Fucntions "
        protected void ShowPanel(string type = "Main")
        {
            DivMain.Style["display"] = "block";
            DivEntry.Style["display"] = "block";
            btnSave.Visible = true;
            btnSave.Text = "Save";
            FillCombo();
            if (type.ToLower() == "entry")
            {

                DivMain.Style["display"] = "none";

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
        protected void ClearHDN()
        {
            HdnDeptId.Value = "0";
            HdnDeptTxt.Value = "";
            HdnFldAwardLetter.Value = "";
            HdnGranDId.Value = "0";
            HdnMode.Value = "Insert";
            HdnpiId.Value = "0";
            HdnPITxt.Value = "";
            HdnProjectId.Value = "0";
            HdnPi_ID.Value = "";

        }
        protected void FillMainGrid()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            List<Senior_CSCS_Details> lstcscs = new List<Senior_CSCS_Details>();
            try
            {
                lstcscs = cl.FillGrantSeniorCSCSGrid().ToList();
                RptGrantGridSeniorCSCS.DataSource = lstcscs;
                RptGrantGridSeniorCSCS.DataBind();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString().Replace("'", ""));
            }
        }
        protected void FillCombo()
        {
            ddlAwardOrg.FillCombo(DropDownName.GrantAwardingOrganization);
            ddlDurationofGrant.FillCombo(DropDownName.GrantDuration);
            ddlGrantExtendPeriod.FillCombo(DropDownName.GrantDuration);

            ddlGrantName.FillCombo(DropDownName.SeniorSCSCGrantName);
        }
        protected void FillControl()
        {
            int grantID = Convert.ToInt32(HdnGranDId.Value);


            if (HdnMode.Value == "Update" || HdnMode.Value == "View")
            {

                Senior_CSCS_Details details = new Senior_CSCS_Details();

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                details = client.GetSenior_CSCS_DetailsByID(grantID);

                TxtApprDate.Text = details.dt_Approval_Date == null || details.dt_Approval_Date == DateTime.MinValue ? "" : Convert.ToDateTime(details.dt_Approval_Date).ToString("MMM-dd-yyyy");

                TxtAwrdLetterDate.Text = details.dt_AwardLetter_Date == null || details.dt_AwardLetter_Date == DateTime.MinValue ? "" : Convert.ToDateTime(details.dt_AwardLetter_Date).ToString("MMM-dd-yyyy");

                TxtgrantExpDate.Text = details.dt_Grant_Expiry_Date == null || details.dt_Grant_Expiry_Date == DateTime.MinValue ? "" : Convert.ToDateTime(details.dt_Grant_Expiry_Date).ToString("MMM-dd-yyyy");

                TxtNExpDate.Text = details.dt_NewGrantExpiry_Date == null || details.dt_NewGrantExpiry_Date == DateTime.MinValue ? "" : Convert.ToDateTime(details.dt_NewGrantExpiry_Date).ToString("MMM-dd-yyyy");

                TxtGrantNo.Text = details.s_Grant_No;

                TxtprotectedTime.Text = Convert.ToString(details.d_Protected_time);

                TxtReaserchIO.Text = details.s_Reaserch_IO;

                TxtStartDate.Text = details.dt_StartDate == null || details.dt_StartDate == DateTime.MinValue ? "" : Convert.ToDateTime(details.dt_StartDate).ToString("MMM-dd-yyyy");                

                string Dept_PI = details.Dept_PI_XML;

                /*Dept PI*/
                List<PI_Master> piList = new List<PI_Master>();

                string xmlDept_PI = Dept_PI;
                if (xmlDept_PI != string.Empty)
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlDept_PI)))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(reader);
                        XmlNodeList xmlNodeList = xml.SelectNodes("DEPT_PI/DEPT");

                        //xmlNodeList.ConvertXmlNodeListToDataTable();
                        foreach (XmlNode node in xmlNodeList)
                        {
                            PI_Master pi = new PI_Master();
                            if (node["i_Dept_ID"] != null)
                                pi.i_Dept_ID = Convert.ToInt32(node["i_Dept_ID"].InnerText);
                            if (node["i_ID"] != null)
                                pi.i_ID = Convert.ToInt32(node["i_ID"].InnerText);
                            if (node["s_Email"] != null)
                                pi.s_Email = (node["s_Email"].InnerText);
                            if (node["s_Phone_no"] != null)
                                pi.s_Phone_no = (node["s_Phone_no"].InnerText);
                            if (node["s_MCR_No"] != null)
                                pi.s_MCR_No = (node["s_MCR_No"].InnerText);
                            if (node["Dept_Name"] != null)
                                pi.s_DeptName = (node["Dept_Name"].InnerText);
                            if (node["s_PI_Name"] != null)
                                pi.s_PIName = (node["s_PI_Name"].InnerText);
                            piList.Add(pi);
                        }
                    }
                }
                FillPIGrid(piList);
                /*Dept PI*/

                /*Budget details processing*/
                string budgetDetails = details.s_Budget_Details_String;
                hdnQuarterlyBudget.Value = budgetDetails;


                /*End of Budget details processing*/
            }
        }
        protected bool Save()
        {
            try
            {

            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString().Replace("'", ""));
            }
            return true;
        }

        private void FillPIGrid(List<PI_Master> listPI)
        {
            rptrPIDetails.DataSource = listPI;
            rptrPIDetails.DataBind();

        }
        #endregion

        #region " Button Event "
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ShowPanel("entry");
        }
        protected void lnkback_Click(object sender, EventArgs e)
        {
            ClearHDN();
            ShowPanel();
        }
        #endregion

        #region " Repeater Event "
        protected void RptGrantGridSeniorCSCS_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        HdnGranDId.Value = e.CommandArgument.ToString();
                        FillControl();

                    }
                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString().Replace("'", ""));
            }
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Senior_CSCS_Details seniorCSCS = new Senior_CSCS_Details();

            seniorCSCS.UID = LoginUserId;
            seniorCSCS.UName = LoginUser;

            if (HdnMode.Value != "Delete") //To bind all the parameter values in Insert and Edit mode
            {



                //Bind Grant details
                seniorCSCS.i_Award_org_ID = Convert.ToInt32(ddlAwardOrg.SelectedValue);

                seniorCSCS.i_GrantName = Convert.ToInt32(ddlGrantName.SelectedValue);

                seniorCSCS.s_Grant_No = TxtGrantNo.Text;

                seniorCSCS.s_Grant_Duration = ddlDurationofGrant.SelectedValue;

                seniorCSCS.b_IsGrant_Extented = ddlGrantExtended.SelectedValue == "-1" || ddlGrantExtended.SelectedValue == "0" ? false : true;

                if (TxtApprDate.Text.Trim() != "")
                    seniorCSCS.dt_Approval_Date = Convert.ToDateTime(TxtApprDate.Text);

                if (TxtNExpDate.Text.Trim() != "")
                    seniorCSCS.dt_NewGrantExpiry_Date = Convert.ToDateTime(TxtNExpDate.Text);

                if (TxtgrantExpDate.Text.Trim() != "")
                    seniorCSCS.dt_Grant_Expiry_Date = Convert.ToDateTime(TxtgrantExpDate.Text);

                seniorCSCS.s_Reaserch_IO = TxtReaserchIO.Text;

                seniorCSCS.s_GrantExtended_period = ddlGrantExtendPeriod.SelectedValue;

                if (TxtAwrdLetterDate.Text.Trim() != "")
                    seniorCSCS.dt_AwardLetter_Date = Convert.ToDateTime(TxtAwrdLetterDate.Text);

                //Award letter file
                //Agreement File

                if (fldAwardLetter.HasFile)
                {
                    string[] arr = Common.UpLoadNew(fldAwardLetter, Common.FolderLocation.Grant);

                    if (arr.Count() != 0)
                    {
                        HdnFldAwardLetter.Value = arr[0];
                    }
                }

                if (HdnFldAwardLetter.Value != null && HdnFldAwardLetter.Value != "")
                    seniorCSCS.dt_AwardLetter_File = HdnFldAwardLetter.Value;

                //Agreement File
                //End of Award letter file

                if (TxtStartDate.Text.Trim() != "")
                    seniorCSCS.dt_StartDate = Convert.ToDateTime(TxtStartDate.Text);

                if (TxtprotectedTime.Text.Trim() != "")
                    seniorCSCS.d_Protected_time = Convert.ToDouble(TxtprotectedTime.Text);

                //PI Details
                List<Project_PI> PIs = new List<Project_PI>();
                string[] splitPiId = HdnPi_ID.Value.Split(',');
                foreach (string pi in splitPiId)
                {
                    PIs.Add(new Project_PI { i_PI_ID = Convert.ToInt32(pi), i_Project_ID = Convert.ToInt32(HdnGranDId.Value) });
                }

                seniorCSCS.Dept_PI = PIs.ToArray();
                //End of PI Details

                seniorCSCS.i_Selected_PI_ID = Convert.ToInt32(hdnSelectedPI.Value);

                //End of Bind Grant details

                //Budget details 
                List<Senior_CSCS_Budget_Allocation_Details> budgetDetails = new List<Senior_CSCS_Budget_Allocation_Details>();

                string[] FactorsArr = new string[] { "Man Power", "Consumables", "Equipment", "Miscellaneuos" };

                if (hdnYearlyBudget.Value != "")
                {
                    foreach (string item in hdnYearlyBudget.Value.Split('~'))
                    {
                        string year = item.Split('|')[0];

                        string allocated = item.Split('|')[1];

                        string actual = item.Split('|')[2];

                        int id = Convert.ToInt32(HdnGranDId.Value);


                        for (int i = 0; i < actual.Split(',').Length; i++)
                        {
                            budgetDetails.Add(new Senior_CSCS_Budget_Allocation_Details()
                            {

                                i_Senior_CSCS_ID = id,
                                s_Years = year,
                                s_Yearly_Quaterly = "Y",
                                i_Budget_Allocation = Convert.ToDouble(allocated.Split(',')[i]),
                                i_Budget_Utilized = Convert.ToDouble(actual.Split(',')[i]),
                                s_Factors = FactorsArr[i],

                            });
                        }

                    }
                }
                seniorCSCS.budgetDetails = budgetDetails.ToArray();
                //Budget details 

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                string result = client.Senior_CSCS_Details_DML(seniorCSCS, HdnMode.Value);

            }// End of To bind all the parameter values in Insert and Edit mode
            else // To bind only ID in case of delete
            {
                seniorCSCS.i_ID = Convert.ToInt32(HdnGranDId.Value);

                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                client.Senior_CSCS_Details_DML(seniorCSCS, HdnMode.Value);

            }

        }


    }


}