/*
 * 
 * WDD Project
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class New : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxStart.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortDateString();
            TextBoxStartTime.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortTimeString();
            TextBoxEndTime.Text = Convert.ToDateTime(Request.QueryString["end"]).ToShortTimeString();

            TextBoxName.Focus();

            DropDownList1.DataSource = dbGetResources();
            DropDownList1.DataTextField = "RoomName";
            DropDownList1.DataValueField = "RoomId";
            DropDownList1.SelectedValue = Request.QueryString["r"];
            DropDownList1.DataBind();
        }
    }
    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        DateTime start = Convert.ToDateTime(TextBoxStart.Text).Date.AddHours(12);
        DateTime end = Convert.ToDateTime(TextBoxEnd.Text).Date.AddHours(12);
        string sTime = TextBoxStartTime.Text;
        string eTime = TextBoxEndTime.Text;
        string name = TextBoxName.Text;
        string resource = DropDownList1.SelectedValue;

        string username = "guest";
        //Check weather session variable null or not
        if (Session["UserName"] != null)
        {
            //Retrieving UserName from Session
            username = (string)(Session["UserName"]);
        }
        else
        {
            //Do Something else
        }

        dbInsertEvent1(start, end, sTime, eTime, name, resource, username, 0);
        //dbInsertEvent(start, end, name, resource, 0);
        Modal.Close(this, "OK");
    }

    private DataTable dbGetResources()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT [RoomId], [RoomName] FROM [Room]", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    private void dbInsertEvent(DateTime start, DateTime end, string name, string resource, int status)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO [Reservation] (ReservationStart, ReservationEnd, ReservationName, RoomId, ReservationStatus) VALUES(@start, @end, @name, @resource, @status)", con);
            //cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("status", status);
            cmd.ExecuteNonQuery();
        }
    }

    private void dbInsertEvent1(DateTime start, DateTime end, string sTime, string eTime, string name, string resource, string username, int status)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO [Reservation] (ReservationStart, ReservationEnd, StartTime, EndTime, ReservationName, RoomId, UserName, ReservationStatus) VALUES(@start, @end, @stime, @etime, @name, @resource, @username, @status)", con);
            //cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("stime", sTime);
            cmd.Parameters.AddWithValue("etime", eTime);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("status", status);
            cmd.ExecuteNonQuery();
        }
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        //Modal.Close(this);
    }
}
