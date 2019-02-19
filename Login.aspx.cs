using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Validateuser(object sender, EventArgs e)
    {
        int userId = 0;
        string Username = txtUsername.Text;
        string Password = txtPassword.Text;
        //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
        string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318fa01g5;Persist Security Info=True;User ID=c563318fa01g5;Password=9668856";
        using (SqlConnection con = new SqlConnection(cs))
        {
            using (SqlCommand cmd = new SqlCommand("validate_user", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);
                con.Open();
                userId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            switch (userId)
            {
                case -1:
                    lblError.Text = "Invalid Account";
                    break;
                case -2:
                    lblError.Text = "Account has not been activated";
                    break;
                default:
                    //FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, chkRemember.Checked);
                    Session["UserName"] = txtUsername.Text;
                    Response.Redirect("Default1.aspx");
                    break;
            }
        }
    }
}