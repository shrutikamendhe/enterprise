using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string cs = ConfigurationManager.ConnectionStrings["SQLServerDBConnectionString"].ToString();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM FirstTable", con);

                reader = cmd.ExecuteReader();

                reader.Read();

                Response.Write(reader["StringText"].ToString());

                con.Close();

            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        //if (!IsPostBack)
        //{
        //    Response.Redirect("Home.aspx");
        //}
    }
}