using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RegisterUser(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int userId = 0;
            //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
            string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318fa01g5;Persist Security Info=True;User ID=c563318fa01g5;Password=9668856";
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("insert_user"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            switch (userId)
            {
                case -1:
                    Label1.Text = "Username already exists";
                    break;
                case -2:
                    Label1.Text = "Email already exists";
                    break;
                default:
                    SendActivationEmail(userId);
                    break;
            }
        }
    }

    private void SendActivationEmail(int userId)
    {
        //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
        string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318fa01g5;Persist Security Info=True;User ID=c563318fa01g5;Password=9668856";
        string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(cs))
        {
            using (SqlCommand cmd = new SqlCommand("insert into UserActivation values (@UserId, @ActCode)", con))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ActCode", activationCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        string toUser = txtEmail.Text;
        using (MailMessage mail = new MailMessage("uhclconferenceroom@gmail.com", toUser))
        {
            mail.Subject = "CRRS - Activate Account";
            string body = "Hello " + txtUsername.Text.Trim() + ", ";
            body += "<br/><br/>Welcome to Conference Room Reservation System";
            body += "<br/><br/><a href='" + Request.Url.AbsoluteUri.Replace("Register.aspx", "Activation.aspx?uid=" + activationCode + "'>Click here to activate your account</a>");
            body += "<br/><br/>Thank You";
            body += "<br/>Team CRRS";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("uhclconferenceroom@gmail.com", "$1234567890A");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credentials;
            smtp.Port = 587;
            smtp.Send(mail);

            //Go to Login
            Response.Redirect("Login.aspx");
            // Response.Redirect("http://dcm.uhcl.edu/c563318fa01g5/Login.aspx");
        }
    }
}