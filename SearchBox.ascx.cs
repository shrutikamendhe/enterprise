using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using TTSHWeb.TTSHWCFReference;


namespace TTSHWeb
{
    public partial class SearchBox : System.Web.UI.UserControl
    {
        TTSHWCFServiceClient proxy = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            // btnSearch.Attributes.Add("onclick", "return validateSearchBox();");
        }

        public event EventHandler ButtonSearchClick;
        public event EventHandler ButtonClearClick;
        public event EventHandler ButtonExportClick;


        public enum FilterCriteria { FEASIBALITY, ETHICS, SELECTED, REGULATORY, GRANT, CONTRACT, ALLPROJECTS, CONTRACT_MGMT, Grant_Master }

        //private FilterCriteria _FilterCriteria = null;
        public FilterCriteria SearchFilterCriteria
        {
            get;
            set;
        }

        private string _InputString = string.Empty;
        public string SearchInputValue
        {
            get { return _InputString; }
            set { _InputString = value.Trim(); }
        }

        private string _ErrorString = string.Empty;
        public string ErrorString
        {
            get { return _ErrorString; }
        }

        private Search[] _lstSearch = null;
        public Search[] SearchOutput { get { return _lstSearch; } }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //if (!"0,1,2,3".Contains(hidCnt.Value))
                //{
                //      _ErrorString = "Error: Can not enter more than 3 fields";
                //       return;
                //}
                SearchInputValue = txtSearch.Text;
                searchText();
                ButtonSearchClick(sender, e);
                // hidCnt.Value = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearSearch();
                ButtonClearClick(sender, e);
            }
            catch (Exception ex)
            {
                
            }
            
        }

        public void searchText()
        {

            if (string.IsNullOrEmpty(SearchFilterCriteria.ToString()))
            {
                _ErrorString = "Error:Please enter Search filter criteria";
                return;
            }

            if (string.IsNullOrEmpty(SearchInputValue))
            {
                _ErrorString = "Error:Please enter Search text";
                return;
            }

            int cntParticipators = SearchInputValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Count();
            if (cntParticipators > 3)
            {
                _ErrorString = "Error: Can not enter more than 3 fields";
                return;
            }

            try
            {
                string s = "";

                //For Seleted Project
                string isSelectedTeamUser = "";
                string UserId = "";
                if (HttpContext.Current.Session["UserGroups"] != null)
                {
                    if (HttpContext.Current.Session["UserGroups"].ToString().Contains("TProjectMonitoring") || HttpContext.Current.Session["UserGroups"].ToString().Contains("TAdmin"))
                    {
                        isSelectedTeamUser = "1";
                    }

                }
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    UserId = HttpContext.Current.Session["UserID"].ToString();
                }
                //For Seleted Project

                proxy = new TTSHWCFServiceClient();
                Dictionary<string, Search[]> dictSearch = proxy.GetSearchData(SearchInputValue, SearchFilterCriteria.ToString(), UserId, isSelectedTeamUser);
                Search[] lstSearch = null;
                //Search[] lstSearch = proxy.GetSearchData(SearchInputValue, SearchFilterCriteria.ToString());
                if (dictSearch.ContainsKey("SUCCESS"))
                {
                    lstSearch = dictSearch["SUCCESS"];
                }
                else
                {
                    List<string> lstKey = new List<string>(dictSearch.Keys);
                    string err = lstKey[0].ToString();
                    if (err == "0")
                    {
                        _ErrorString = "No Data Found";
                    }
                    else
                    {
                        _ErrorString = lstKey[0].ToString();
                    }
                }

                _lstSearch = lstSearch;

            }
            catch (Exception ex)
            {

            }
        }

        public void clearSearch()
        {
            txtSearch.Text = "";
            SearchInputValue = "";
            lblErr.Text = "";
            _lstSearch = null;
            //hidCnt.Value = "";
        }

        public void setFocus()
        {
            txtSearch.Focus();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                _ErrorString = "Not Implemented";
                ButtonExportClick(sender, e);
            }
            catch (Exception ex)
            {

            }
        }
    }
}