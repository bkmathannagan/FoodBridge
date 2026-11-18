<%@ Page Title="Register as Food Donor – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="RegisterDonor.aspx.vb" Inherits="FoodBridge.RegisterDonor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Register as a Food Donor on FoodBridge to donate surplus food from individuals, hotels, restaurants, marriage halls, and supermarkets." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container" style="max-width: 820px;">
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litAlert" runat="server" />
            </asp:Panel>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#127970; Food Donor Registration</span>
                    <a href="<%= ResolveUrl("~/Account/Login.aspx?role=Donor") %>" class="btn btn-outline btn-sm">Already Registered? Login</a>
                </div>
                <div class="card-body">
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="form-label">Donor / Organization Name *</label>
                            <asp:TextBox ID="txtDonorName" runat="server" CssClass="form-control" placeholder="e.g., Grand Royal Hotel or Individual Name" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Donor Type *</label>
                            <asp:DropDownList ID="ddlDonorType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Hotel / Restaurant" Value="Hotel" />
                                <asp:ListItem Text="Marriage / Banquet Hall" Value="Marriage Hall" />
                                <asp:ListItem Text="Catering Service" Value="Catering Service" />
                                <asp:ListItem Text="Supermarket / Grocery Store" Value="Supermarket" />
                                <asp:ListItem Text="Corporate Cafeteria" Value="Corporate" />
                                <asp:ListItem Text="Individual / Household" Value="Individual" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Contact Person Name *</label>
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" placeholder="Primary point of contact" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Mobile Number (10 digits) *</label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="e.g., 9845011223" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="donor@example.com" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">City *</label>
                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Complete Pickup Address *</label>
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Street address, landmark, building name" />
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

                    <asp:Button ID="btnRegister" runat="server" Text="Create Donor Account" CssClass="btn btn-primary btn-lg" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
