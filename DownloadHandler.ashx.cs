using System;
using System.Web;
using System.Web.UI;

namespace TTSHWeb
{
    /// <summary>
    /// Summary description for DownloadHandler
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string file = "";

            // get the file name from the querystring
            if (context.Request.QueryString["fileName"] != null)
            {
                file = context.Request.QueryString["fileName"].ToString();
            }

            string filename = context.Server.MapPath("~/" + file);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);

            try
            {
                if (fileInfo.Exists)
                {



                    string FName = fileInfo.Name.Split('~')[0] + "." + fileInfo.Name.Split('.')[fileInfo.Name.Split('.').Length - 1];
                    context.Response.Clear();
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + FName + "\"");
                    context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.TransmitFile(fileInfo.FullName);
                    context.Response.Flush();
                    context.Response.Close();


                }
                else
                {

                }
            }
            catch
            {


            }
            finally
            {
                context.Response.End();
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