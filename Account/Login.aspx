<%@ Page Title="Portal Login – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="FoodBridge.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Log in to FoodBridge as Administrator, Food Donor, Verified NGO, or Rescue Volunteer." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container" style="max-width: 960px;">
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litAlert" runat="server" />
            </asp:Panel>

            <div class="form-grid" style="grid-template-columns: 1.1fr 0.9fr;">
                <div class="card">
                    <div class="card-header">
                        <span class="card-title">&#128274; FoodBridge Role-Based Login</span>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label class="form-label">Select Your User Role *</label>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Food Donor (Individual / Hotel / Hall)" Value="Donor" />
                                <asp:ListItem Text="NGO / Charitable Organization" Value="NGO" />
                                <asp:ListItem Text="Rescue Volunteer" Value="Volunteer" />
                                <asp:ListItem Text="System Administrator" Value="Admin" />
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter registered email address" />
                        </div>

                        <div class="form-group">
                            <label class="form-label">Password *</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" />
                        </div>

                        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 18px;">
                            <a href="<%= ResolveUrl("~/Account/ForgotPassword.aspx") %>" style="font-size: 13.5px;">Forgot Password?</a>
                        </div>

                        <asp:Button ID="btnLogin" runat="server" Text="Sign In to Portal" CssClass="btn btn-primary" style="width: 100%; padding: 11px;" />

                        <div style="margin-top: 18px; border-top: 1px solid var(--border-color); padding-top: 14px; font-size: 13.5px; text-align: center;">
                            New to FoodBridge? Register as:
                            <div style="margin-top: 8px; display: flex; justify-content: center; gap: 8px; flex-wrap: wrap;">
                                <a href="<%= ResolveUrl("~/Account/RegisterDonor.aspx") %>" class="btn btn-outline btn-sm">Donor</a>
                                <a href="<%= ResolveUrl("~/Account/RegisterNGO.aspx") %>" class="btn btn-outline btn-sm">NGO</a>
                                <a href="<%= ResolveUrl("~/Account/RegisterVolunteer.aspx") %>" class="btn btn-outline btn-sm">Volunteer</a>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Default Testing Credentials Card -->
                <div class="card" style="border-top: 4px solid var(--accent);">
                    <div class="card-header">
                        <span class="card-title">&#9889; Default Testing Credentials</span>
                    </div>
                    <div class="card-body">
                        <p style="font-size: 13px; color: var(--text-muted); margin-bottom: 14px;">
                            Click any role below to auto-fill credentials (Password for all testing accounts: <code>FoodBridge@123</code>):
                        </p>

                        <div style="margin-bottom: 12px; padding: 10px; background: #F8FAFC; border: 1px solid var(--border-color); border-radius: 8px;">
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <strong>1. Administrator</strong>
                                <button type="button" class="btn btn-primary btn-sm" onclick="return fillDemoCredentials('admin@foodbridge.com','FoodBridge@123','Admin');">Fill Admin</button>
                            </div>
                            <div style="font-size: 12.5px; color: var(--text-muted);">Email: <code>admin@foodbridge.com</code> | Pass: <code>FoodBridge@123</code></div>
                        </div>

                        <div style="margin-bottom: 12px; padding: 10px; background: #F8FAFC; border: 1px solid var(--border-color); border-radius: 8px;">
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <strong>2. Food Donor</strong>
                                <button type="button" class="btn btn-primary btn-sm" onclick="return fillDemoCredentials('donor@foodbridge.com','FoodBridge@123','Donor');">Fill Donor</button>
                            </div>
                            <div style="font-size: 12.5px; color: var(--text-muted);">Email: <code>donor@foodbridge.com</code> | Pass: <code>FoodBridge@123</code></div>
                        </div>

                        <div style="margin-bottom: 12px; padding: 10px; background: #F8FAFC; border: 1px solid var(--border-color); border-radius: 8px;">
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <strong>3. NGO / Receiver</strong>
                                <button type="button" class="btn btn-primary btn-sm" onclick="return fillDemoCredentials('ngo@foodbridge.com','FoodBridge@123','NGO');">Fill NGO</button>
                            </div>
                            <div style="font-size: 12.5px; color: var(--text-muted);">Email: <code>ngo@foodbridge.com</code> | Pass: <code>FoodBridge@123</code></div>
                        </div>

                        <div style="padding: 10px; background: #F8FAFC; border: 1px solid var(--border-color); border-radius: 8px;">
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <strong>4. Volunteer</strong>
                                <button type="button" class="btn btn-primary btn-sm" onclick="return fillDemoCredentials('volunteer@foodbridge.com','FoodBridge@123','Volunteer');">Fill Volunteer</button>
                            </div>
                            <div style="font-size: 12.5px; color: var(--text-muted);">Email: <code>volunteer@foodbridge.com</code> | Pass: <code>FoodBridge@123</code></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
