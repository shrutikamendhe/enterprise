using System;
using System.Web.UI.WebControls;

namespace TTSHWeb
{
    public partial class TestWeb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SearchBox.SearchFilterCriteria = TTSHWeb.SearchBox.FilterCriteria.CONTRACT;
            SearchBox.ButtonSearchClick += new EventHandler(SearchBox_ButtonClick);
            SearchBox.ButtonClearClick += new EventHandler(SearchBox_ClearClick);

            if (!IsPostBack)
            {

                //SearchBox.SearchInputValue = "A,1,2";
                //SearchBox.searchText();

                //string s = SearchBox.ErrorString;
                //TTSHSearch.TTSHWCFService.Search[] lst = SearchBox.SearchOutput;
                //grdSearch.DataSource = lst;
                //grdSearch.DataBind();
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                //switch (ddlSelect.SelectedValue)
                //{
                //    case "FEASIBALITY":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.FEASIBALITY;
                //        break;

                //    case "ETHICS":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.ETHICS;
                //        break;

                //    case "SELECTED":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.SELECTED;
                //        break;

                //    case "REGULATORY":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.REGULATORY;
                //        break;

                //    case "GRANT":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.GRANT;
                //        break;

                //    case "CONTRACT":
                //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.CONTRACT;
                //        break;

                //}
            }
            catch (Exception ex)
            {

            }
        }

        protected void SearchBox_ButtonClick(object sender, EventArgs e)
        {
            lblErr.Text = "";
            SearchBox.SearchInputValue = ((TextBox)(SearchBox.FindControl("txtSearch"))).Text;


            if (string.IsNullOrEmpty(SearchBox.ErrorString))
            {
                TTSHWeb.TTSHWCFReference.Search[] lst = SearchBox.SearchOutput;
                grdSearch.DataSource = lst;
                grdSearch.DataBind();
            }
            else
            {
                lblErr.Text = SearchBox.ErrorString;
                grdSearch.DataSource = null;
                grdSearch.DataBind();
            }

        }

        protected void SearchBox_ClearClick(object sender, EventArgs e)
        {
            grdSearch.DataSource = null;
            grdSearch.DataBind();
        }

        protected void ddlSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (ddlSelect.SelectedValue)
            //{
            //    case "FEASIBALITY":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.FEASIBALITY;
            //        break;

            //    case "ETHICS":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.ETHICS;
            //        break;

            //    case "SELECTED":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.SELECTED;
            //        break;

            //    case "REGULATORY":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.REGULATORY;
            //        break;

            //    case "GRANT":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.GRANT;
            //        break;

            //    case "CONTRACT":
            //        SearchBox.SearchFilterCriteria = TTSHSearch.SearchBox.FilterCriteria.CONTRACT;
            //        break;

            //}
        }
    }
}