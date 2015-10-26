using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;
namespace TTSHWeb
{
    public partial class frmGrantDetailsForm : System.Web.UI.Page
    {
        #region " Page Load "
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.GRANT;
            SearchBox.ButtonSearchClick += SearchBox_ButtonSearchClick;
            SearchBox.ButtonClearClick += SearchBox_ButtonClearClick;
            if (!IsPostBack)
            {
                ShowPanel(); FillGridMain();


            }
            
        }
        #endregion

        #region " Methods & Functions "
        protected void ShowPanel(string type = "Main")
        {
            DivMain.Style["display"] = "block";
            DivEntry.Style["display"] = "block";
            btnSave.Visible = true;
            btnSave.Text = "Save";
           
            BindCombo();
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
        protected void FillGridMain()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                List<Grant_Details> lstgrantDetail = new List<Grant_Details>();
                lstgrantDetail = cl.FillGrantDetailGrid().ToList();
                RptGrantGrid.DataSource = lstgrantDetail;
                RptGrantGrid.DataBind();
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message.ToString().Replace("'", " "));

            }
        }
        protected void ClearHDN()
        {
            HdnAwardLetterFile.Value = "";
            HdnGranDId.Value = "0";
            HdnMode.Value = "Insert";
            HdnProjectId.Value = "0";
            HdnDuration.Value = "";
            HdnExtension.Value = "";
            HdnCheckfld.Value = "";
        }
        protected void BindCombo()
        {
            ddlGrantDetailStatus.FillCombo(DropDownName.GrantDetailStatus);
            ddlDuration.FillCombo(DropDownName.GrantDuration);
            ddlExtDuration.FillCombo(DropDownName.GrantDuration);
        }

       
        protected void FillControl()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            Grant_Details GDList = new Grant_Details();
            List<Project_Master> Gpjlist = new List<Project_Master>();
            List<Project_Master> Gchildpmlist = new List<Project_Master>();
            List<PI_Master> Gpilist = new List<PI_Master>();
            List<PI_Master> Gchildpilist = new List<PI_Master>();
            List<Grant_Master> GgmList = new List<Grant_Master>();
            try
            {
                GDList = cl.GetGrantDetailsById(Convert.ToInt32(HdnGranDId.Value));
                if (GDList != null)
                {
                    if (GDList.PMList != null)
                    {
                        Gpjlist = GDList.PMList.ToList();
                        DataTable dt = new DataTable();
                        dt = Gpjlist.ListToDatatable();
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DispProjectId.InnerText = Convert.ToString(dt.Rows[0]["s_Display_Project_ID"]);
                                TxtProjTitle.Text = Convert.ToString(dt.Rows[0]["s_Project_Title"]);
                                TxtAlias1.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias1"]);
                                TxtAlias2.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias2"]);
                                TxtShortTitle.Text = Convert.ToString(dt.Rows[0]["s_Short_Title"]);
                                TxtprojCategory.Text = Convert.ToString(dt.Rows[0]["Project_Category_Name"]);
                                TxtIrbNo.Text = Convert.ToString(dt.Rows[0]["s_IRB_No"]);
                            }
                        }
                    }
                    if (GDList.PIList!=null)
                    {
                        Gpilist = GDList.PIList.ToList();
                        rptrPIDetails.DataSource = Gpilist;
                        rptrPIDetails.DataBind();

                        //********** Bind SIngle Pi ********
                        ddlPIName.DataSource = Gpilist;
                        ddlPIName.DataTextField = "s_PIName";
                        ddlPIName.DataValueField = "i_ID";
                        ddlPIName.DataBind();
                        //********** END *******************
                    }
                    if (GDList.GMList != null)
                    {
                        GgmList = GDList.GMList.ToList(); //----------- Grant Master Data
                        DataTable dt = new DataTable();
                        dt = GgmList.ListToDatatable();
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                TxtGrantType.Text = Convert.ToString(dt.Rows[0]["GRANT_TYPE"]);
                                TxtGrantSubType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE1"]);
                                TxtGrantSSType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE2"]);
                                TxtGrantSSSType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE3"]);
                                TxtDateofAwardLetter.Text = Convert.ToString(dt.Rows[0]["dt_AwardDate"]);
                                HdnDuration.Value = Convert.ToString(dt.Rows[0]["s_Duration"]);
                            }
                        }
                    }
                    if (GDList.CHildProjectList != null)
                    {
                        Gchildpmlist = GDList.CHildProjectList.ToList(); // Child Project List
                        RptrChildProject.DataSource = Gchildpmlist;
                        RptrChildProject.DataBind();
                        
                        
                        //********* Bind CHild Project**********
                        ddlChildProject.DataSource = Gchildpmlist;
                        ddlChildPI.DataTextField = "s_Project_Title";
                        ddlChildPI.DataValueField = "i_Project_ID";
                        ddlChildProject.DataBind();
                        //******* END*****************************
                    }
                    if (GDList.CHILDPIList!=null)
                    {
                        //********** Bind Child PI***********
                        Gchildpilist = GDList.CHILDPIList.ToList();
                        ddlChildPI.DataSource = Gchildpilist;
                        ddlChildPI.DataTextField = "s_PIName";
                        ddlChildPI.DataValueField = "i_ID";
                        ddlChildPI.DataBind();
                        //********** END ************************
                    }
                }
            }
            catch (Exception ex)
            {

                this.MsgBox("Something Went Wrong..!!" + ex.Message.ToString().Replace("'", ""));
            }
        }
        protected void FillProjectDataForNewEntry()
        {
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            GrantNewProjectEntry GNP = new GrantNewProjectEntry();
            List<PI_Master> GpiList = new List<PI_Master>();
            List<Project_Master> GpjList = new List<Project_Master>();
            List<Grant_Master> GgmList = new List<Grant_Master>();
            List<Project_Master> Gchildpmlist = new List<Project_Master>();
            List<PI_Master> Gchildpilist = new List<PI_Master>();
            try
            {
                GNP = cl.FillGrantDetailNewProject(Convert.ToInt32(HdnProjectId.Value));
                if (GNP != null)
                {

                    if (GNP.PMList != null)
                    {
                        GpjList = GNP.PMList.ToList(); //----------- Project Master List
                        DataTable dt = new DataTable();
                        dt = GpjList.ListToDatatable();
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DispProjectId.InnerText = Convert.ToString(dt.Rows[0]["s_Display_Project_ID"]);
                                TxtProjTitle.Text = Convert.ToString(dt.Rows[0]["s_Project_Title"]);
                                TxtAlias1.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias1"]);
                                TxtAlias2.Text = Convert.ToString(dt.Rows[0]["s_Project_Alias2"]);
                                TxtShortTitle.Text = Convert.ToString(dt.Rows[0]["s_Short_Title"]);
                                TxtprojCategory.Text = Convert.ToString(dt.Rows[0]["Project_Category_Name"]);
                                TxtIrbNo.Text = Convert.ToString(dt.Rows[0]["s_IRB_No"]);
                            }
                        }
                    }
                    if (GNP.PIList != null)
                    {
                        GpiList = GNP.PIList.ToList(); //------------- Dept Pi List
                        rptrPIDetails.DataSource = GpiList;
                        rptrPIDetails.DataBind();

                        //********** Bind SIngle Pi ********
                        ddlPIName.DataSource = GpiList;
                        ddlPIName.DataTextField = "s_PIName";
                        ddlPIName.DataValueField = "i_ID";
                        ddlPIName.DataBind();
                        //********** END *******************
                    }
                    if (GNP.GMList != null)
                    {
                        GgmList = GNP.GMList.ToList(); //----------- Grant Master Data
                        DataTable dt = new DataTable();
                        dt = GgmList.ListToDatatable();
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                TxtGrantType.Text = Convert.ToString(dt.Rows[0]["GRANT_TYPE"]);
                                TxtGrantSubType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE1"]);
                                TxtGrantSSType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE2"]);
                                TxtGrantSSSType.Text = Convert.ToString(dt.Rows[0]["GRANT_SUB_TYPE3"]);
                                TxtDateofAwardLetter.Text = Convert.ToString(dt.Rows[0]["dt_AwardDate"]);
                                HdnDuration.Value = Convert.ToString(dt.Rows[0]["s_Duration"]);
                            }
                        }
                    }
                    if (GNP.CHildProjectList != null)
                    {
                        Gchildpmlist = GNP.CHildProjectList.ToList(); // Child Project List
                        RptrChildProject.DataSource = Gchildpmlist;
                        RptrChildProject.DataBind();
                    }
                    if (GNP.CHILDpilist != null)
                    {
                        //********** Bind Child PI***********
                        Gchildpilist = GNP.CHILDpilist.ToList();
                        ddlChildPI.DataSource = Gchildpilist;
                        ddlChildPI.DataTextField = "s_PIName";
                        ddlChildPI.DataValueField = "i_ID";
                        ddlChildPI.DataBind();
                        //********** END ************************
                    }

                }
            }
            catch (Exception ex)
            {

                this.MsgBox("Something Went Wrong..!!" + ex.Message.ToString().Replace("'", ""));
            }
        }
        #endregion

        #region " Events "
        void SearchBox_ButtonClearClick(object sender, EventArgs e)
        {
            FillGridMain();
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
                    Project_DataOwner[] oDOList = client.GetProjectsByDO("Grant", UserID);
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

                            RptGrantGrid.DataSource = oOldSearch; /*use the object according to your need*/
                            RptGrantGrid.DataBind();
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

                            RptGrantGrid.DataSource = lst; /*use the object according to your need*/
                            RptGrantGrid.DataBind();
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

                        RptGrantGrid.DataSource = oOldSearch; /*use the object according to your need*/
                        RptGrantGrid.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                client.Close();


            }
            else
            {

                RptGrantGrid.DataSource = null;
                RptGrantGrid.DataBind();
            }
        }
        protected void lnkback_Click(object sender, EventArgs e)
        {
            ClearHDN(); ShowPanel();
        }


        #endregion

        #region " Repeater Event "
        protected void RptGrantGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
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


                        if (HdnMode.Value.ToLower() == "insert")
                        {
                            FillProjectDataForNewEntry();
                        }
                        else
                        {
                            HdnGranDId.Value = e.CommandArgument.ToString();
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



    }
}