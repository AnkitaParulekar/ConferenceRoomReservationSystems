/*
 * 
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DayPilot.Web.Ui;
using DayPilot.Web.Ui.Data;
using DayPilot.Web.Ui.Enums;
using DayPilot.Web.Ui.Enums.Scheduler;
using DayPilot.Web.Ui.Events.Scheduler;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // prevent invalid ViewState errors in Firefox        
        if (Request.Browser.Browser == "Firefox") Response.Cache.SetNoStore();

        DayPilotScheduler1.Separators.Clear();
        DayPilotScheduler1.Separators.Add(DateTime.Now, Color.Red);

        if (!IsPostBack)
        {

            //DayPilotScheduler1.StartDate = new DateTime(DateTime.Today.Year, 1, 1);

            DayPilotScheduler1.Scale = TimeScale.Manual;
            DateTime start = new DateTime(DateTime.Today.Year, 1, 1, 12, 0, 0);
            DateTime end = start.AddYears(1);

            DayPilotScheduler1.Timeline.Clear();
            for (DateTime cell = start; cell < end; cell = cell.AddDays(1))
            {
                DayPilotScheduler1.Timeline.Add(cell, cell.AddDays(1));
            }

            LoadResourcesAndEvents();

            // scroll to this month
            DateTime firstOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DayPilotScheduler1.SetScrollX(DateTime.Today);
        }
    }

    protected void DayPilotScheduler1_EventMove(object sender, DayPilot.Web.Ui.Events.EventMoveEventArgs e)
    {
        string id = e.Value;
        DateTime start = e.NewStart;
        DateTime end = e.NewEnd;
        string resource = e.NewResource;
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

        string message = null;

        if (!dbIsFree(id, start, end, resource))
        {
            message = "The reservation cannot overlap with an existing reservation.";
        }
        else if (e.OldEnd <= DateTime.Today)
        {
            message = "This reservation cannot be changed anymore.";
        }
        else if (e.OldStart < DateTime.Today)
        {
            if (e.OldResource != e.NewResource)
            {
                message = "The room cannot be changed anymore.";
            }
            else
            {
                message = "The reservation start cannot be changed anymore.";
            }
        }
        else if (e.NewStart < DateTime.Today)
        {
            message = "The reservation cannot be moved to the past.";
        }
        else
        {
            dbUpdateEvent(id, start, end, resource, username);
            //message = "Reservation moved.";
        }

        LoadResourcesAndEvents();
        DayPilotScheduler1.UpdateWithMessage(message);
    }

    private DataTable dbGetEvents(DateTime start, DateTime end)
    {
        //SqlDataAdapter da = new SqlDataAdapter("SELECT [ReservationId], [ReservationName], [ReservationStart], [ReservationEnd], [RoomId], [ReservationStatus], [ReservationPaid] FROM [Reservation] WHERE NOT (([ReservationEnd] <= @start) OR ([ReservationStart] >= @end))", ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter("SELECT [ReservationId], [ReservationName], [ReservationStart], [ReservationEnd], [RoomId], [ReservationStatus] FROM [Reservation] WHERE NOT (([ReservationEnd] <= @start) OR ([ReservationStart] >= @end))", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", end);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    private void dbUpdateEvent(string id, DateTime start, DateTime end, string resource, string username)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["srrs"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [Reservation] SET ReservationStart = @start, ReservationEnd = @end, RoomId = @resource, UserName = @username WHERE ReservationId = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("resource", resource);
            cmd.Parameters.AddWithValue("username", username);
            cmd.ExecuteNonQuery();
        }
    }

    private bool dbIsFree(string id, DateTime start, DateTime end, string resource)
    {
        // event with the specified id will be ignored
        SqlDataAdapter da = new SqlDataAdapter("SELECT count(ReservationId) as count FROM [Reservation] WHERE NOT (([ReservationEnd] <= @start) OR ([ReservationStart] >= @end)) AND RoomId = @resource AND ReservationId <> @id", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("id", id);
        da.SelectCommand.Parameters.AddWithValue("start", start);
        da.SelectCommand.Parameters.AddWithValue("end", end);
        da.SelectCommand.Parameters.AddWithValue("resource", resource);
        DataTable dt = new DataTable();
        da.Fill(dt);

        int count = Convert.ToInt32(dt.Rows[0]["count"]);
        return count == 0;
    }


    private void LoadResources()
    {
        DayPilotScheduler1.Resources.Clear();

        string roomFilter = "0";
        if (DayPilotScheduler1.ClientState["filter"] != null)
        {
            roomFilter = (string)DayPilotScheduler1.ClientState["filter"]["room"];
        }

        SqlDataAdapter da = new SqlDataAdapter("SELECT [RoomId], [RoomName], [RoomStatus], [RoomSize], [Capacity], [Location], [Projector] FROM [Room] WHERE RoomType = @rt or @rt = '0' ORDER BY RoomId ASC", ConfigurationManager.ConnectionStrings["srrs"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("rt", roomFilter);
        DataTable dt = new DataTable();
        da.Fill(dt);

        foreach (DataRow r in dt.Rows)
        {
            string name = (string)r["RoomName"];
            string id = Convert.ToString(r["RoomId"]);
            string status = (string)r["RoomStatus"];
            int sqft = Convert.ToInt32(r["RoomSize"]);
            string sqftFormatted = (sqft == 1) ? "1 sqft" : String.Format("{0} sqfts", sqft);

            int capacity = Convert.ToInt32(r["Capacity"]);
            string capacityFormatted = (capacity == 1) ? "1 Person" : String.Format("{0} Persons", capacity);
            string location = (string)r["Location"];
            string projector = (string)r["Projector"];

            Resource res = new Resource(name, id);
            res.DataItem = r;
            res.Columns.Add(new ResourceColumn(id));
            res.Columns.Add(new ResourceColumn(sqftFormatted));
            res.Columns.Add(new ResourceColumn(capacityFormatted));
            res.Columns.Add(new ResourceColumn(projector));
            res.Columns.Add(new ResourceColumn(location));
            res.Columns.Add(new ResourceColumn(status));
            DayPilotScheduler1.Resources.Add(res);
        }
    }

    protected void DayPilotScheduler1_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
    {
        switch (e.Command)
        {
            case "refresh":
                LoadResourcesAndEvents();
                break;
            case "filter":
                LoadResourcesAndEvents();
                break;
        }
    }

    private void LoadResourcesAndEvents()
    {
        LoadResources();
        DayPilotScheduler1.DataSource = dbGetEvents(DayPilotScheduler1.VisibleStart, DayPilotScheduler1.VisibleEnd);
        DayPilotScheduler1.DataBind();
        DayPilotScheduler1.Update();
    }

    protected void DayPilotScheduler1_EventResize(object sender, DayPilot.Web.Ui.Events.EventResizeEventArgs e)
    {
        string id = e.Value;
        DateTime start = e.NewStart;
        DateTime end = e.NewEnd;
        string resource = e.Resource;

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

        string message = null;

        if (!dbIsFree(id, start, end, resource))
        {
            message = "The reservation cannot overlap with an existing reservation.";
        }
        else if (e.OldEnd <= DateTime.Today)
        {
            message = "This reservation cannot be changed anymore.";
        }
        else if (e.OldStart != e.NewStart)
        {
            if (e.OldStart < DateTime.Today)
            {
                message = "The reservation start cannot be changed anymore.";
            }
            else if (e.NewStart < DateTime.Today)
            {
                message = "The reservation cannot be moved to the past.";
            }
        }
        else
        {
            dbUpdateEvent(id, start, end, resource, username);
            //message = "Reservation updated.";
        }

        LoadResourcesAndEvents();
        DayPilotScheduler1.UpdateWithMessage(message);
    }

    protected void DayPilotScheduler1_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
    {
        e.InnerHTML = String.Format("{0} ({1:d} - {2:d})", e.Text, e.Start, e.End);
        int status = Convert.ToInt32(e.Tag["ReservationStatus"]);

        switch (status)
        {
            case 0: // new
                if (e.Start < DateTime.Today.AddDays(2)) // must be confirmed two day in advance
                {
                    e.DurationBarColor = "red";
                    e.ToolTip = "Expired (not confirmed in time)";
                }
                else
                {
                    e.DurationBarColor = "orange";
                    e.ToolTip = "New";
                }
                break;
            case 1:  // confirmed
                if (e.Start < DateTime.Today || (e.Start == DateTime.Today && DateTime.Now.TimeOfDay.Hours > 18))  // must arrive before 6 pm
                {
                    e.DurationBarColor = "#f41616";  // red
                    e.ToolTip = "Late arrival";
                }
                else
                {
                    e.DurationBarColor = "green";
                    e.ToolTip = "Confirmed";
                }
                break;
            case 2: // arrived
                if (e.End < DateTime.Today || (e.End == DateTime.Today && DateTime.Now.TimeOfDay.Hours > 11))  // must checkout before 10 am
                {
                    e.DurationBarColor = "#f41616"; // red
                    e.ToolTip = "Late checkout";
                }
                else
                {
                    e.DurationBarColor = "#1691f4";  // blue
                    e.ToolTip = "Arrived";
                }
                break;
            case 3: // checked out
                e.DurationBarColor = "gray";
                e.ToolTip = "Checked out";
                break;
            default:
                throw new ArgumentException("Unexpected status.");
        }

        e.InnerHTML = e.InnerHTML + String.Format("<br /><span style='color:gray'>{0}</span>", e.ToolTip);

        int paid = 0; // Convert.ToInt32(e.DataItem["ReservationPaid"]);
        string paidColor = "#aaaaaa";

        //e.Areas.Add(new Area().Bottom(10).Right(4).Html("<div style='color:" + paidColor + "; font-size: 8pt;'>Paid: " + paid + "%</div>").Visibility(AreaVisibility.Visible));
        //e.Areas.Add(new Area().Left(4).Bottom(8).Right(4).Height(2).Html("<div style='background-color:" + paidColor + "; height: 100%; width:" + paid + "%'></div>").Visibility(AreaVisibility.Visible));
    }

    protected void DayPilotScheduler1_BeforeCellRender(object sender, DayPilot.Web.Ui.Events.BeforeCellRenderEventArgs e)
    {
        if (e.IsBusiness)
        {
            e.BackgroundColor = "#ffffff";
        }
        else
        {
            e.BackgroundColor = "#f8f8f8";
        }
    }

    protected void DayPilotScheduler1_BeforeResHeaderRender(object sender, BeforeResHeaderRenderEventArgs e)
    {
        string status = (string)e.DataItem["RoomStatus"];
        switch (status)
        {
            case "Dirty":
                e.CssClass = "status_dirty";
                break;
            case "Cleanup":
                e.CssClass = "status_cleanup";
                break;
        }
    }
}
