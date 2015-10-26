using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using TTSHWeb.TTSHWCFReference;
namespace TTSHWeb
{
    public partial class PageMethods : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }

        [WebMethod()]
        [ScriptMethod()]
        public static string GetValidate(string _ModuleName, string _A, string _B, string _C, string _D)
        {
            string Result = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                Result = cl.GetValidate(_ModuleName, _A, _B, _C, _D);
            }
            catch (Exception)
            {

                Result = "#Error";
            }
            return Result;
        }


        [WebMethod]
        [ScriptMethod]
        public static string[] GetText(string Prefix, int count, string ContextKey)
        {
            TTSHWCFServiceClient sc = new TTSHWCFServiceClient();
            List<string> lst = new List<string>();
            lst.AddRange(sc.GetText(Prefix, count, ContextKey));
            return lst.ToArray();
        }

        [WebMethod]
        [ScriptMethod]
        public static string GetPI_MasterDetailsByID(int ID)
        {
            string Result = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                Result = cl.GetPI_MasterDetailsByID(ID);
            }
            catch (Exception)
            {

                Result = "";
            }
            return Result;
        }

        [WebMethod]
        [ScriptMethod]
        public static string GetCollobrator_MasterDetailByID(int ID)
        {
            string Result = "";
            TTSHWCFServiceClient cl = new TTSHWCFServiceClient();
            try
            {
                Result = cl.GetCollobrator_MasterDetailByID(ID);
            }
            catch (Exception)
            {

                Result = "";
            }
            return Result;
        }


       
    }
}