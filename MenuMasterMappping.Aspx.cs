using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TTSHWeb.TTSHWCFReference;
using System.Web.UI.HtmlControls;
namespace TTSHWeb
{
    public partial class MenuMasterMappping : System.Web.UI.Page
    {


        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            // btnSave.Attributes.Add("OnClientClick", "javascript:return FinalValidation();");
            if (!IsPostBack)
            {
                ddlGroupName.FillCombo(DropDownName.MenuMapping);
            }
        }

        #endregion

        #region Control Events

        protected void ddlGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindTreeView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder xml;
          
            try
            {
                List<string> accRights = HidMenuAccess.Value.Split(',').ToList();
                xml = new System.Text.StringBuilder();
                xml.Append("<items>");

                foreach (string item in accRights)
                {
                    xml.AppendFormat("<item menuid='{0}'/>", item);
                }
                        
                xml.Append("</items>");
                TTSHWCFServiceClient c1 = new TTSHWCFServiceClient();
                if (c1.SaveAccess(xml.ToString(), Convert.ToInt32(ddlGroupName.SelectedValue), 1))
                {
                    
                    ddlGroupName.SelectedIndex = 0;
                    this.MsgBox(btnSave.Text.ToLower() == "Save Access".ToLower() ? " Access Rights Saved Successfully." : " Access Rights Updated Successfully.");
                    btnSave.Text = "Save Access";
                }
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
                //throw;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
              
                ddlGroupName.SelectedIndex = 0;
                chkall.Checked = false;
                btnSave.Text = "Save Access";
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }
        }
        #endregion

        #region User Defined Function
        protected void BindTreeView()
        {
            bool isupdate = false;
            try
            {
                TTSHWeb.TTSHWCFReference.UserMenuRights userMenuRights;
                TTSHWCFServiceClient c1 = new TTSHWCFServiceClient();
                userMenuRights = c1.GetAllMenus(ddlGroupName.SelectedItem.ToString());
                //   tvAccess.Nodes.Clear();
                chkall.Checked = false;
                //tvAccess.Nodes.Add(new TreeNode("Select All", "-1"));
                HtmlGenericControl divCol ;
                HtmlGenericControl divUserAcc;
                HtmlGenericControl h3;
                HtmlInputCheckBox chk;
                HtmlGenericControl ul;
                HtmlGenericControl label;
                foreach (ParentMenuAccess menuMaster in userMenuRights.parentMenuAccess)
                {
                    divCol = new HtmlGenericControl("div");
                    divCol.Attributes.Add("class", "col-md-4 col-sm-6");

                    divUserAcc = new HtmlGenericControl("div");
                    divUserAcc.Attributes.Add("class", "user-access");
                   
                    label = new HtmlGenericControl("label");

                    h3 = new HtmlGenericControl("h3");
                    ul = new HtmlGenericControl("ul");
                    chk = new HtmlInputCheckBox();
                    chk.Name = menuMaster.MenuId.ToString();
                    chk.Attributes.Add("class", "parentcheckbox");
                    chk.Attributes.Add("onclick","BindParentCheck(this)");
                    chk.Value = menuMaster.MenuId.ToString(); ;
                    label.InnerText = menuMaster.MenuName;

                    h3.Controls.Add(chk);
                    h3.Controls.Add(label);
                    divUserAcc.Controls.Add(h3);
                    divUserAcc.Controls.Add(ul);
                    divCol.Controls.Add(divUserAcc);

                    TreeNode root = new TreeNode();
                    root.Text = menuMaster.MenuName;
                    root.Value = menuMaster.MenuId.ToString();
                    //root.ToolTip = "Main";
                    root.SelectAction = TreeNodeSelectAction.None;
                    //Added Code To Show Parent Menu Checked When No Child Menu Is There
                    if (menuMaster.AccessRights)
                        root.Checked = true;
                    
                    foreach (MenuAccessRights menuAccessRights in userMenuRights.menuAccessRights)
                    {
                        if (menuMaster.MenuId == menuAccessRights.Parent && menuAccessRights.Parent > 0)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            HtmlGenericControl labelchild = new HtmlGenericControl("label");
                            HtmlInputCheckBox chkchild = new HtmlInputCheckBox();
                            chkchild.Value = menuAccessRights.MenuId.ToString();
                            chkchild.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                            chkchild.Name = menuAccessRights.MenuId.ToString();
                            labelchild.InnerText = menuAccessRights.MenuName;
                            li.Controls.Add(chkchild);
                            li.Controls.Add(labelchild);
                            ul.Controls.Add(li);
                            TreeNode cn = new TreeNode();
                            cn.Text = menuAccessRights.MenuName;
                            cn.Value = menuAccessRights.MenuId.ToString();
                            cn.SelectAction = TreeNodeSelectAction.None;

                            //root.ChildNodes.Add(cn);
                            if (menuAccessRights.AccessRights)
                            {
                                chk.Checked = true;
                                chkchild.Checked = true;
                                
                            }
                        }
                    }
                    row.Controls.Add(divCol);
                   
                }
                
                if (userMenuRights.menuAccessRights.Count(x => x.AccessRights == true) > 0 || userMenuRights.parentMenuAccess.Count(x => x.AccessRights == true) > 0)
                    isupdate = true;
                chkall.Checked = (userMenuRights.menuAccessRights.Count(x => x.AccessRights == true) + userMenuRights.parentMenuAccess.Count(x => x.AccessRights == true)) == (userMenuRights.menuAccessRights.Count() + userMenuRights.parentMenuAccess.Count());
                this.CallJs("DisableDashBoard();");
                btnSave.Text = isupdate ? "Update Access" : "Save Access";
            }
            catch (Exception ex)
            {
                this.MsgBox(ex.Message);
            }

        }
        /// <summary>
        /// To Check Or Uncheck the Nodes
        /// </summary>
        /// <param name="trNodeCollection"></param>
        /// <param name="isCheck"></param>
        private void CheckUncheckTreeNode(TreeNodeCollection trNodeCollection, bool isCheck)
        {
            try
            {
                foreach (TreeNode trNode in trNodeCollection)
                {
                    trNode.Checked = isCheck;
                    if (trNode.ChildNodes.Count > 0)
                        CheckUncheckTreeNode(trNode.ChildNodes, isCheck);
                }
            }
            catch (Exception ex)
            {

                this.MsgBox(ex.Message);
            }
        }
        #endregion
    }
}