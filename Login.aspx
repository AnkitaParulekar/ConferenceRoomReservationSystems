﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="CSS/bootstrap.css" rel="stylesheet" />
    <style>
        body {
            background-image:url('media/SR01.jpg');
            background-size: cover;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4" style="margin-top:100px;">
                    <h2>Conference Room Reservation System</h2><br />
                    <div class="form-group">
                        <label>Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Username Required" ForeColor="Red" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password Required" ForeColor="Red" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                    </div>
                    <div class="checkbox">
                        <label><asp:CheckBox ID="chkRemember" runat="server" CssClass="checkbox"/>Remember me</label>
                    </div>
                    <asp:Button ID="Button1" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="Validateuser"/>
                    <asp:LinkButton runat="server" id="SomeLinkButton" href="Register.aspx" CssClass="btn btn-primary">New User Register Here!</asp:LinkButton>
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <div class="col-md-4"></div>
            </div>
        </div>                
    </div>
    </form>
</body>
</html>
