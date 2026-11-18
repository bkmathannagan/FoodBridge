<%@ Page Title="Become a Volunteer – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="RegisterVolunteer.aspx.vb" Inherits="FoodBridge.RegisterVolunteer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Register as a Food Rescue Volunteer on FoodBridge to collect surplus food from donors and deliver it to NGOs and people in need." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container" style="max-width: 820px;">
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litAlert" runat="server" />
            </asp:Panel>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#128666; Volunteer Registration</span>
                    <a href="<%= ResolveUrl("~/Account/Login.aspx?role=Volunteer") %>" class="btn btn-outline btn-sm">Already Registered? Login</a>
                </div>
                <div class="card-body">
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="form-label">Full Name *</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Gender *</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Mobile Number (10 digits) *</label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="e.g., 9740112233" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="volunteer@example.com" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">City *</label>
                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Initial Availability Status *</label>
                            <asp:DropDownList ID="ddlAvailability" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Available for Rescue Tasks" Value="Available" />
                                <asp:ListItem Text="Weekends / Evenings Only" Value="Available" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Residential Address *</label>
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Locality and street address" />
                    </div>

                    <div class="form-grid">
                        <div class="form-group">
                            <label class="form-label">Password (min 6 chars) *</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Confirm Password *</label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        </div>
                    </div>

                    <asp:Button ID="btnRegister" runat="server" Text="Register as Rescue Volunteer" CssClass="btn btn-primary btn-lg" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
