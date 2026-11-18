<%@ Page Title="NGO Organization Profile – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Profile.aspx.vb" Inherits="FoodBridge.NGOProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage NGO Profile &amp; Feedback</h1>
            <p class="page-subtitle">Verification Status: <asp:Literal ID="litVerificationBadge" runat="server" /></p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#129309; Organization Details</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Organization Name *</label>
                    <asp:TextBox ID="txtOrgName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Registration Number *</label>
                    <asp:TextBox ID="txtRegNumber" runat="server" CssClass="form-control" />
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
                    <label class="form-label">Email Address (Read-only)</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="True" />
                </div>
                <div class="form-group">
                    <label class="form-label">Number of Beneficiaries *</label>
                    <asp:TextBox ID="txtBeneficiaries" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">City *</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Delivery Address *</label>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                </div>
                <asp:Button ID="btnUpdateProfile" runat="server" Text="Save NGO Profile" CssClass="btn btn-primary" />
            </div>
        </div>

        <div>
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
                    <asp:Button ID="btnChangePwd" runat="server" Text="Change Password" CssClass="btn btn-outline" />
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#128172; Submit NGO Feedback</span>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label class="form-label">Subject *</label>
                        <asp:TextBox ID="txtFbSubject" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Feedback Message *</label>
                        <asp:TextBox ID="txtFbMessage" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnSubmitFeedback" runat="server" Text="Submit Feedback" CssClass="btn btn-accent" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
