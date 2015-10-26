using System;
using System.Web.UI.WebControls;
using  TTSHWeb.TTSHWCFReference;

namespace TTSHWeb
{
    public partial class TestMaster : System.Web.UI.Page
    {
        #region Declarations
    
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlGroupName.FillCombo(DropDownName.MenuMapping);
            }
        }

       

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
        protected void BindTreeView()
        {

            try
            {
                UserMenuRights userMenuRights;
                TTSHWCFServiceClient c1 = new TTSHWCFServiceClient();
                userMenuRights = c1.GetAllMenus(ddlGroupName.SelectedItem.ToString());

                tvAccess.Nodes.Clear();
                foreach (ParentMenuAccess menuMaster in userMenuRights.parentMenuAccess)
                {
                    TreeNode root = new TreeNode();
                    root.Text = menuMaster.MenuName;
                    root.Value = menuMaster.MenuId.ToString();
                    root.ToolTip = "Main";
                    root.SelectAction = TreeNodeSelectAction.None;

                    foreach (MenuAccessRights menuAccessRights in userMenuRights.menuAccessRights)
                    {
                        if (menuMaster.MenuId == menuAccessRights.Parent && menuAccessRights.Parent > 0)
                        {
                            TreeNode cn = new TreeNode();
                            cn.Text = menuAccessRights.MenuName;
                            cn.Value = menuAccessRights.MenuId.ToString();
                            cn.SelectAction = TreeNodeSelectAction.None;

                            root.ChildNodes.Add(cn);
                            if (menuAccessRights.AccessRights)
                            {
                               // root.Selected = true;
                               // cn.Checked = true;
                            }
                        }
                    }
                    tvAccess.Nodes.Add(root);
                }
                tvAccess.CollapseAll();
            }
            catch (Exception ex)
            {
            }

        }
    }
}