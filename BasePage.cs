using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TTSHWeb
	{
	public class BasePage : System.Web.UI.Page
		{
		protected void Page_PreInit(object sender, EventArgs e)
			{
			if ( Convert.ToString(Common.iffBlank(Request.QueryString["Newpage"], "")) != "" )
				{
				this.Page.MasterPageFile = "~/TTSHMasterPage/TTSH.Master";
				}
			
				{
				this.Page.MasterPageFile = "~/TTSHMasterPage/NewMasterPage.Master";
				}
			}
		}
	}