using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using TTSHWeb.TTSHWCFReference;

namespace TTSHWeb
{
    public partial class ReportProjectDeptPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdownVal();
            }
            btnViewRpt.Attributes.Add("onclick", "return validate();");
        }

        private void BindDropdownVal()
        {
            try
            {
                TTSHWCFServiceClient client = new TTSHWCFServiceClient();

                RptProjectCategory[] ProjectCategory = client.ListRptProjectCategory();
                if (ProjectCategory != null && ProjectCategory.Count() > 0)
                {
                    chkCategory.DataSource = ProjectCategory;
                    chkCategory.DataValueField = "CategoryId";
                    chkCategory.DataTextField = "CategoryName";
                    chkCategory.DataBind();
                }
                else
                {
                    chkCategory.DataSource = null;
                    chkCategory.DataBind();
                }

                RptProjectType[] ProjectType = client.ListRptProjectType();
                if (ProjectType != null && ProjectType.Count() > 0)
                {
                    chkType.DataSource = ProjectType;
                    chkType.DataValueField = "TypeId";
                    chkType.DataTextField = "TypeName";
                    chkType.DataBind();
                }
                else
                {
                    chkType.DataSource = null;
                    chkType.DataBind();
                }


                RptDepartment[] DepartmentList = client.ListRptDepartment();
                if (DepartmentList != null && DepartmentList.Count() > 0)
                {
                    chkDept.DataSource = DepartmentList;
                    chkDept.DataValueField = "DepartmentId";
                    chkDept.DataTextField = "DepartmentName";
                    chkDept.DataBind();
                }
                else
                {
                    chkDept.DataSource = null;
                    chkDept.DataBind();
                }
                client.Close();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnViewRpt_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Highliter", "ApplyScript();", true);

                if (!string.IsNullOrEmpty(hidPI.Value))
                {
                    txtPI.Text = hidPI.Value;
                }

                RViewer.Visible = true;

                string ReportUserName = System.Configuration.ConfigurationManager.AppSettings["ReportUserName"].ToString();
                string ReportPassword = System.Configuration.ConfigurationManager.AppSettings["ReportPassword"].ToString();
                string ReportDomain = System.Configuration.ConfigurationManager.AppSettings["ReportDomain"].ToString();
                string ReportURI = System.Configuration.ConfigurationManager.AppSettings["ReportURI"].ToString();


                RViewer.ShowCredentialPrompts = false;

                RViewer.ServerReport.ReportServerCredentials = new TTSHWeb.Reports.ReportCredentials(ReportUserName, ReportPassword, ReportDomain);
                RViewer.ServerReport.ReportServerUrl = new Uri(ReportURI);
                RViewer.ServerReport.ReportPath = "/TTSHREPORT/ProjectBYDeptPI";
                RViewer.ProcessingMode = ProcessingMode.Remote;
                RViewer.ShowParameterPrompts = false;
                RViewer.ShowPromptAreaButton = false;

                string StartDate = "";
                string EndDate = "";
                string[] Category =  chkCategory.Items.Cast<ListItem>().Where(z => z.Selected).Select(z => z.Value).ToArray();
                string[] Type = chkType.Items.Cast<ListItem>().Where(z => z.Selected).Select(z => z.Value).ToArray();
                string[] Dept = chkDept.Items.Cast<ListItem>().Where(z => z.Selected).Select(z => z.Value).ToArray();
                string[] PI = chkPI.Items.Cast<ListItem>().Where(z => z.Selected).Select(z => z.Value).ToArray();


                StartDate = (txtStartDate.Text == "") ? null : Convert.ToDateTime(txtStartDate.Text).ToString("MM-dd-yyyy");
                EndDate = (txtEndDate.Text == "") ? null : Convert.ToDateTime(txtEndDate.Text).ToString("MM-dd-yyyy");

                ReportParameter[] reportParameterCollection = new ReportParameter[6];
                reportParameterCollection[0] = new ReportParameter();
                reportParameterCollection[0].Name = "Startdate";
                reportParameterCollection[0].Values.Add(StartDate);                     

                reportParameterCollection[1] = new ReportParameter();
                reportParameterCollection[1].Name = "Enddate";
                reportParameterCollection[1].Values.Add(EndDate);                       

                reportParameterCollection[2] = new ReportParameter();
                reportParameterCollection[2].Name = "Category";
                reportParameterCollection[2].Values.AddRange(Category);

                reportParameterCollection[3] = new ReportParameter();
                reportParameterCollection[3].Name = "Type";
                reportParameterCollection[3].Values.AddRange(Type);

                reportParameterCollection[4] = new ReportParameter();
                reportParameterCollection[4].Name = "Department";
                reportParameterCollection[4].Values.AddRange(Dept);

                reportParameterCollection[5] = new ReportParameter();
                reportParameterCollection[5].Name = "PINAME";
                reportParameterCollection[5].Values.AddRange(PI);

                RViewer.ServerReport.SetParameters(reportParameterCollection);
                RViewer.ServerReport.Refresh();

            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Highliter", "ApplyScript();", true);

                    txtType.Text = "";
                    txtPI.Text = "";
                    txtDept.Text = "";
                    txtCategory.Text = "";
                    txtEndDate.Text = "";
                    txtStartDate.Text = "";

                    foreach (ListItem item in chkCategory.Items)
                    {
                        item.Selected = false;
                    }
                    foreach (ListItem item in chkDept.Items)
                    {
                        item.Selected = false;
                    }
                    foreach (ListItem item in chkPI.Items)
                    {
                        item.Selected = false;
                    }
                    foreach (ListItem item in chkType.Items)
                    {
                        item.Selected = false;
                    }
                    hidCategory.Value = "";
                    hidDept.Value = "";
                    hidPI.Value = "";
                    hidType.Value = "";

                    RViewer.Reset();
                    RViewer.Visible = false;
            
            
            }
            catch (Exception ex)
            {

            }
        }

        protected void chkDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               

                if (!string.IsNullOrEmpty(hidCategory.Value))
                {
                    txtCategory.Text = hidCategory.Value;
                }

                if (!string.IsNullOrEmpty(hidType.Value))
                {
                    txtType.Text = hidType.Value;
                }

                if (!string.IsNullOrEmpty(hidDept.Value))
                {
                    txtDept.Text = hidDept.Value;
                }

                txtPI.Text = "";

               


                TTSHWCFServiceClient client = new TTSHWCFServiceClient();
                string Dept = string.Join(",", chkDept.Items.Cast<ListItem>().Where(z => z.Selected).Select(z => z.Value).ToList());
                if (!string.IsNullOrEmpty(Dept))
                {
                    RptPIName[] PiName = client.ListRptPINameByDepartment(Dept);

                    if (PiName != null && PiName.Count() > 0)
                    {
                        chkPI.DataSource = PiName;
                        chkPI.DataValueField = "PIId";
                        chkPI.DataTextField = "PIName";
                        chkPI.DataBind();
                    }
                    else
                    {
                        chkPI.DataSource = null;
                        chkPI.DataValueField = "PIId";
                        chkPI.DataTextField = "PIName";
                        chkPI.Items.Clear();
                        chkPI.DataBind();
                    }
                    
                }
                else
                {
                    chkPI.DataSource = null;
                    chkPI.DataValueField = "PIId";
                    chkPI.DataTextField = "PIName";
                    chkPI.Items.Clear();
                    chkPI.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Highliter", "ApplyScript();", true);

                client.Close();

            }
            catch (Exception ex)
            {

            }
            
        }
    }
}