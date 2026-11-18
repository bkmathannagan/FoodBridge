<%@ Page Title="Donor Profile & Feedback – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Profile.aspx.vb" Inherits="FoodBridge.DonorProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage Donor Profile, Password &amp; Feedback</h1>
            <p class="page-subtitle">Keep your contact person and pickup address up to date for seamless volunteer collection.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <!-- Profile Update Card -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#127970; Donor Profile Information</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Donor / Organization Name *</label>
                    <asp:TextBox ID="txtDonorName" runat="server" CssClass="form-control" />
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
                    <label class="form-label">Contact Person *</label>
                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Mobile Number *</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Registered Email (Read-only)</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="True" />
                </div>
                <div class="form-group">
                    <label class="form-label">City *</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Default Pickup Address *</label>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                </div>
                <asp:Button ID="btnUpdateProfile" runat="server" Text="Save Profile Changes" CssClass="btn btn-primary" />
            </div>
        </div>

        <div>
            <!-- Change Password Card -->
            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#128274; Change Password</span>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label class="form-label">Current Password *</label>
                        <asp:TextBox ID="txtCurrentPwd" runat="server" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">New Password *</label>
                        <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnChangePwd" runat="server" Text="Update Password" CssClass="btn btn-outline" />
                </div>
            </div>

            <!-- Submit Feedback Card -->
            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#128172; Submit Donor Feedback</span>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label class="form-label">Rating</label>
                        <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-control">
                            <asp:ListItem Text="5 - Excellent" Value="5" />
                            <asp:ListItem Text="4 - Good" Value="4" />
                            <asp:ListItem Text="3 - Average" Value="3" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Subject *</label>
                        <asp:TextBox ID="txtFbSubject" runat="server" CssClass="form-control" placeholder="e.g., Smooth pickup experience" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Message *</label>
                        <asp:TextBox ID="txtFbMessage" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnSubmitFeedback" runat="server" Text="Send Feedback" CssClass="btn btn-accent" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
