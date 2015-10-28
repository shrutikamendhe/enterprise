using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Response.Redirect("Home.aspx");
            return;
        }

        //try
        //{
        //    string cs = ConfigurationManager.ConnectionStrings["SQLServerDBConnectionString"].ToString();
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();

        //        SqlDataReader reader = null;
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM FirstTable", con);

        //        reader = cmd.ExecuteReader();

        //        reader.Read();

        //        Response.Write("Database response: " + reader["StringText"].ToString());

        //        con.Close();

        //    }


        //    WebClient proxy = new WebClient();
        //    byte[] data = proxy.DownloadData("http://192.168.0.15:9090/RestfullService.svc/dowork");
        //    Stream stream = new MemoryStream(data);
        //    DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
        //    string result = obj.ReadObject(stream).ToString();
        //    Response.Write("<br/> Service response: " + result.ToString());
        //}
        //catch (Exception ex)
        //{
        //    Response.Write(ex.Message);
        //}

    }
}