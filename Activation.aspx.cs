using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Activation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
            string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318fa01g5;Persist Security Info=True;User ID=c563318fa01g5;Password=9668856";
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["uid"]) ? Request.QueryString["uid"] : Guid.Empty.ToString();
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("delete from UserActivation where ActivationCode=@code", con))
                {
                    cmd.Parameters.AddWithValue("@code", activationCode);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Literal1.Text = "Activation Successfull";
                        Response.Redirect("http://dcm.uhcl.edu/c563318fa01g5/pages/Login.aspx");
                    }
                    else
                    {
                        Literal1.Text = "Invalid Activation Code";
                    }
                }
            }
        }
    }
}