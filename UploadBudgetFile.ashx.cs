using System;
using System.IO;
using System.Web;

namespace TTSHWeb
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    public class UploadBudgetFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string fpath = "";
            int iUploadedCnt = 0;
            string location = "Selected";
            //  string filePath = context.Request.QueryString["fileName"].ToString();
            string[] paths = new string[context.Request.Files.Count];

            if (context.Request.Files.Count <= 0)
            {
              
                
                context.Response.Write("No file uploaded");




            }
            else
            {
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    HttpPostedFile file = context.Request.Files[i];
                    string fileName = Path.GetFileName(file.FileName);
                    string path = HttpContext.Current.Server.MapPath("~/" + location.ToString() + "/" + DateTime.Now.Date.ToString("dd/MM/yyyy").Replace("/", "-"));

                    string ext = Path.GetExtension(file.FileName);

                    string random = Common.GetRandomString();
                    string fullPath = path + "/" + fileName.Replace(ext, "") + "~" + random + ext;
                    string Shortpath = location.ToString() + "/" + DateTime.Now.Date.ToString("dd/MM/yyyy").Replace("/", "-") + "/" + fileName.Replace(ext, "") + "~" + random + ext;
                    if (file.ContentLength > 0)
                    {


                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        if (!File.Exists(fullPath))
                        {
                            file.SaveAs(fullPath);
                            paths[i] = Shortpath + "|" + fileName;
                            iUploadedCnt += 1;
                        }
                        else
                        {
                            File.Delete(fullPath);
                            file.SaveAs(fullPath);
                            paths[i] = Shortpath + "|" + fileName;
                        }
                        context.Response.Write(paths[i]);
                    }
                }
            }







        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}