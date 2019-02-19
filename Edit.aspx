<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>New Booking</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">Edit Conference Reservation</div>
                    <asp:LinkButton ID="LinkButtonDelete" runat="server" OnClick="LinkButtonDelete_Click">Delete</asp:LinkButton>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">Start:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server"></asp:TextBox></td>
                <td align="right">Start Time:</td>
                <td><asp:TextBox ID="TextBoxStartTime" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">End:</td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server"></asp:TextBox></td>
                <td align="right">End Time:</td>
                <td><asp:TextBox ID="TextBoxEndTime" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Resource:</td>
                <td><asp:DropDownList ID="DropDownListRoom" runat="server" Width="200px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Name:</td>
                <td><asp:TextBox ID="TextBoxName" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">Reserved By:</td>
                <td><asp:TextBox ID="TextBoxUser" runat="server" Width="200px" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Status:</td>
                <td>
                    <asp:DropDownList ID="DropDownListStatus" runat="server">
                        <asp:ListItem Value="0">New</asp:ListItem>
                        <asp:ListItem Value="1">Confirmed</asp:ListItem>
                        <asp:ListItem Value="2">Arrived</asp:ListItem>
                        <asp:ListItem Value="3">Checked out</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>

            <!--
            <tr>
                <td align="right">
                    Paid:</td>
                <td>
                    <asp:DropDownList ID="DropDownListPaid" runat="server">
                        <asp:ListItem Value="0">0%</asp:ListItem>
                        <asp:ListItem Value="50">50%</asp:ListItem>
                        <asp:ListItem Value="100">100%</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            -->

            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="  OK  " />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
