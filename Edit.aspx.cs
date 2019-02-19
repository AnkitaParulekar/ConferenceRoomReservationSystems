/* Copyright © 2005 - 2013 Annpoint, s.r.o.
   Use of this software is subject to license terms. 
   http://www.daypilot.org/

   If you have purchased a DayPilot Pro license, you are allowed to use this 
   code under the conditions of DayPilot Pro License Agreement:

   http://www.daypilot.org/files/LicenseAgreement.pdf

   Otherwise, you are allowed to use it for evaluation purposes only under 
   the conditions of DayPilot Pro Trial License Agreement:
   
   http://www.daypilot.org/files/LicenseAgreementTrial.pdf
   
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class Edit : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            DataRow dr = dbGetEvent(Request.QueryString["id"]);

            if (dr == null)
            {
                throw new Exception("The event was not found");
            }

            TextBoxStart.Text = Convert.ToDateTime(dr["ReservationStart"]).ToShortDateString();
            TextBoxEnd.Text = Convert.ToDateTime(dr["ReservationEnd"]).ToShortDateString();
            TextBoxStartTime.Text = dr["StartTime"] as string;
            TextBoxEndTime.Text = dr["EndTime"] as string;
            TextBoxName.Text = dr["ReservationName"] as string;
            TextBoxUser.Text = dr["UserName"] as string;

            DropDownListRoom.DataSource = dbGetResources();
            DropDownListRoom.DataTextField = "RoomName";
            DropDownListRoom.DataValueField = "RoomId";
            DropDownListRoom.SelectedValue = Convert.ToString(dr["RoomId"]);
            DropDownListRoom.DataBind();

            DropDownListStatus.SelectedValue = Convert.ToString(dr["ReservationStatus"]);
            //DropDownListPaid.SelectedValue = Convert.ToString(dr["ReservationPaid"]);
            TextBoxName.Focus();
        }
    }
    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        DateTime start = Convert.ToDateTime(TextBoxStart.Text).Date.AddHours(12);
        DateTime end = Convert.ToDateTime(TextBoxEnd.Text).Date.AddHours(12);
        string sTime = TextBoxStartTime.Text;
        string eTime = TextBoxEndTime.Text;
        string name = TextBoxName.Text;
        string resource = DropDownListRoom.SelectedValue;
        string id = Request.QueryString["id"];
        int status = Convert.ToInt32(DropDownListStatus.SelectedValue);
        int paid = Convert.ToInt32(DropDownListPaid.SelectedValue);

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

        dbUpdateEvent1(id, start, end, sTime, eTime, name, resource, status, paid, username);
        //dbUpdateEvent(id, start, end, name, resource, status, paid, );
        
        Modal.Close(this, "OK");
    }

    private DataTable dbGetResources()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT [RoomId], [RoomName] FROM [Room]", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    private DataRow dbGetEvent(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Reservation] WHERE ReservationId = @id", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    private void dbUpdateEvent(string id, DateTime start, DateTime end, string name, string resource, int status, int paid)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            //SqlCommand cmd = new SqlCommand("UPDATE [Reservation] SET ReservationStart = @start, ReservationEnd = @end, ReservationName = @name, RoomId = @resource, ReservationStatus = @status, ReservationPaid = @paid WHERE ReservationId = @id", con);
            SqlCommand cmd = new SqlCommand("UPDATE [Reservation] SET ReservationStart = @start, ReservationEnd = @end, ReservationName = @name, RoomId = @resource, ReservationStatus = @status WHERE ReservationId = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("paid", paid);
            cmd.ExecuteNonQuery();
        }
    }

    private void dbUpdateEvent1(string id, DateTime start, DateTime end, string sTime, string eTime, string name, string resource, int status, int paid, string username)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            //SqlCommand cmd = new SqlCommand("UPDATE [Reservation] SET ReservationStart = @start, ReservationEnd = @end, ReservationName = @name, RoomId = @resource, ReservationStatus = @status, ReservationPaid = @paid WHERE ReservationId = @id", con);
            SqlCommand cmd = new SqlCommand("UPDATE [Reservation] SET ReservationStart = @start, ReservationEnd = @end, StartTime = @stime, EndTime = @etime, ReservationName = @name, RoomId = @resource, ReservationStatus = @status, UserName=@username WHERE ReservationId = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("stime", sTime);
            cmd.Parameters.AddWithValue("etime", eTime);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("paid", paid);
            cmd.Parameters.AddWithValue("username", username);
            cmd.ExecuteNonQuery();
        }
    }

    private void dbDeleteEvent(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM [Reservation] WHERE ReservationId = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Modal.Close(this);
    }
    protected void LinkButtonDelete_Click(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        dbDeleteEvent(id);
        Modal.Close(this, "OK");
    }
}
