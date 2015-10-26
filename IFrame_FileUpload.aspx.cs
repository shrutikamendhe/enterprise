using System;

namespace TTSHWeb
{
    public partial class IFrame_FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            try

            {
                txtFileName.Text = System.IO.Path.GetFileName(file1.PostedFile.FileName.ToString());
                string[] file = Common.UpLoadNew(file1, Common.FolderLocation.Others);
                txtSavePath.Text = file[0].ToString();
            }
            catch (Exception ex)
            {
            }
        }
    }
}