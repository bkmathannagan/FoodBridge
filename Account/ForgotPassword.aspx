<%@ Page Title="Forgot Password – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ForgotPassword.aspx.vb" Inherits="FoodBridge.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container" style="max-width: 540px;">
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litAlert" runat="server" />
            </asp:Panel>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#128273; Reset Account Password</span>
                </div>
                <div class="card-body">
                    <p style="font-size: 13.5px; color: var(--text-muted); margin-bottom: 16px;">
                        Verify your registered Email Address and Mobile Number to set a new password.
                    </p>
                    <div class="form-group">
                        <label class="form-label">Select Role *</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Food Donor" Value="Donor" />
                            <asp:ListItem Text="NGO / Receiver" Value="NGO" />
                            <asp:ListItem Text="Volunteer" Value="Volunteer" />
                            <asp:ListItem Text="Administrator" Value="Admin" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Registered Email Address *</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Registered Mobile Number *</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">New Password *</label>
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnReset" runat="server" Text="Verify &amp; Reset Password" CssClass="btn btn-primary" style="width: 100%;" />
                    <div style="text-align: center; margin-top: 14px;">
                        <a href="<%= ResolveUrl("~/Account/Login.aspx") %>">&larr; Back to Login</a>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
