<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Default1.aspx.cs" Inherits="_Default" MasterPageFile="./Site.master" Title="Conference Room Reservation System" %>
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="js/modal.js"></script>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
	<script type="text/javascript">

	    var modal = new DayPilot.Modal();
	    modal.border = "10px solid #ccc";
	    modal.closed = function () {
	        if (this.result == "OK") {
	            dps.commandCallBack('refresh');
	        }
	    };

        function createEvent(start, end, resource) {
	        modal.height = 250;
	        modal.showUrl("New.aspx?start=" + start.toStringSortable() + "&end=" + end.toStringSortable() + "&r=" + resource);
	    }

	    function editEvent(id) {
	        modal.height = 300;
	        modal.showUrl("Edit.aspx?id=" + id);
	    }

	    function afterRender(data) {
	    };

	    function filter(property, value) {
            if (!dps.clientState.filter) {
                console.log("1",value)
	            dps.clientState.filter = {};
	        }
	        if (dps.clientState.filter[property] != value) { // only refresh when the value has changed
                dps.clientState.filter[property] = value;
                console.log("2",value)

	            dps.commandCallBack('filter');
	        }
	    }
	
	</script>
    <style type="text/css">
        .scheduler_default_rowheader .scheduler_default_rowheader_inner 
        {
            border-right: 1px solid #aaa;
        }
        .scheduler_default_rowheader.scheduler_default_rowheadercol2
        {
            background: White;
        }
        .scheduler_default_rowheadercol2 .scheduler_default_rowheader_inner 
        {
            top: 2px;
            bottom: 2px;
            left: 2px;
            background-color: transparent;
            border-left: 5px solid #1a9d13; /* green */
            border-right: 0px none;
        }
        .status_dirty.scheduler_default_rowheadercol2 .scheduler_default_rowheader_inner
        {
            border-left: 5px solid #ea3624; /* red */
        }
        .status_cleanup.scheduler_default_rowheadercol2 .scheduler_default_rowheader_inner
        {
            border-left: 5px solid #f9ba25; /* orange */
        }
    </style>	
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="margin-bottom: 5px;">
    Show rooms:
        <asp:DropDownList ID="DropDownListFilter" runat="server" onchange="filter('room', this.value)">
        <asp:ListItem Text="All" Value="0"></asp:ListItem>
        <asp:ListItem Text="Conference Room 1" Value="1"></asp:ListItem>
        <asp:ListItem Text="Conference Room 2" Value="2"></asp:ListItem>
        <asp:ListItem Text="Conference Room 3" Value="3"></asp:ListItem>
        <asp:ListItem Text="Conference Room 4" Value="4"></asp:ListItem>
        <asp:ListItem Text="Conference Room 5" Value="5"></asp:ListItem>
        <asp:ListItem Text="Conference Room 6" Value="6"></asp:ListItem>
        <asp:ListItem Text="Conference Room 7" Value="7"></asp:ListItem>
        <asp:ListItem Text="Conference Room 8" Value="8"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <DayPilot:DayPilotScheduler 
        ID="DayPilotScheduler1" 
        runat="server" 
        
        DataStartField="ReservationStart" 
        DataEndField="ReservationEnd" 
        DataTextField="ReservationName" 
        DataValueField="ReservationId" 
        DataResourceField="RoomId" 
        DataTagFields="ReservationStatus"
        
        ClientObjectName="dps"
        
        CellGroupBy="Month"
        CellDuration="1440"
        Days="365"

        CellWidth = "100"
        
        HeightSpec="Max"
        Height="550"
        Width="100%"
        HeaderFontSize="8pt"
        EventFontSize="8pt"
        
        EventMoveHandling="CallBack" 
        OnEventMove="DayPilotScheduler1_EventMove" 
        
        EventResizeHandling="CallBack"
        OnEventResize="DayPilotScheduler1_EventResize"
        
        TimeRangeSelectedHandling="JavaScript"
        TimeRangeSelectedJavaScript="createEvent(start, end, column);" 
        
        OnCommand="DayPilotScheduler1_Command"
        
        EventClickHandling="JavaScript"
        EventClickJavaScript="editEvent(e.value());" 
        
        AfterRenderJavaScript="afterRender(data);" 

        
        OnBeforeEventRender="DayPilotScheduler1_BeforeEventRender" OnBeforeCellRender="DayPilotScheduler1_BeforeCellRender"

        RowHeaderWidthAutoFit="true"
        EventHeight="50"
        DurationBarVisible="true"
        SyncResourceTree="false"
        
        OnBeforeResHeaderRender="DayPilotScheduler1_BeforeResHeaderRender"
        >

        <TimeHeaders>
            <DayPilot:TimeHeader GroupBy="Month" Format="MMMM yyyy" />
            <DayPilot:TimeHeader GroupBy="Day" />
        </TimeHeaders>
        <HeaderColumns>
            <DayPilot:RowHeaderColumn Title="Room" Width="80" />
            <DayPilot:RowHeaderColumn Title="RoomCode" Width="60" />
            <DayPilot:RowHeaderColumn Title="Size" Width="80" />
            <DayPilot:RowHeaderColumn Title="Capacity" Width="80" />
            <DayPilot:RowHeaderColumn Title="Projector" Width="60" />
            <DayPilot:RowHeaderColumn Title="Location" Width="80" />
            <DayPilot:RowHeaderColumn Title="Status" Width="60" />
        </HeaderColumns>
    </DayPilot:DayPilotScheduler>

    <br />
</asp:Content>