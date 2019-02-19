<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="New" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>New event</title>
    <link href='css/main.css' type="text/css" rel="stylesheet" /> 
</head>
<body class="dialog">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="4" cellpadding="0">
            <tr>
                <td align="right"></td>
                <td>
                    <div class="header">New Reservation</div>
                </td>
            </tr>
            <tr>
                <td align="right">Start:</td>
                <td><asp:TextBox ID="TextBoxStart" runat="server"></asp:TextBox></td>
                <td align="right">Start Time:</td>
                <td><asp:TextBox ID="TextBoxStartTime" runat="server"></asp:TextBox></td>
                <!--<td><select id="StartTime" name="StartTime">
                    <option value=""></option>
                    <option value="8.00">8:00</option>
                    <option value="9.00">9:00</option>
                    <option value="10.00">10:00</option>
                    <option value="11.00">11:00</option>
                    <option value="12.00">12:00</option>
                    <option value="13.00">13:00</option>
                    <option value="14.00">14:00</option>
                    <option value="15.00">15:00</option>
                    <option value="16.00">16:00</option>
                    <option value="17.00">17:00</option>
                    <option value="18.00">18:00</option>
                    <option value="19.00">19:00</option>
                    <option value="20.00">20:00</option>
                </select></td>-->
            </tr>
            <tr>
                <td align="right">End:</td>
                <td><asp:TextBox ID="TextBoxEnd" runat="server"></asp:TextBox></td>
                <td align="right">End Time:</td>
                <td><asp:TextBox ID="TextBoxEndTime" runat="server"></asp:TextBox></td>
                <!--
                <td><select id="EndTime" name="EndTime">
                    <option value=""></option>
                    <option value="8.00">8:00</option>
                    <option value="9.00">9:00</option>
                    <option value="10.00">10:00</option>
                    <option value="11.00">11:00</option>
                    <option value="12.00">12:00</option>
                    <option value="13.00">13:00</option>
                    <option value="14.00">14:00</option>
                    <option value="15.00">15:00</option>
                    <option value="16.00">16:00</option>
                    <option value="17.00">17:00</option>
                    <option value="18.00">18:00</option>
                    <option value="19.00">19:00</option>
                    <option value="20.00">20:00</option>
                </select></td>-->
            </tr>
            <tr>
                <td align="right">Room:</td>
                <td><asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right">Name:</td>
                <td><asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="OK" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" />
                </td>
            </tr>
        </table>
        
        </div>
    </form>
</body>
</html>
