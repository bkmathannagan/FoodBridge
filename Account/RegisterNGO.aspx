<%@ Page Title="NGO Registration – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="RegisterNGO.aspx.vb" Inherits="FoodBridge.RegisterNGO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Register your NGO, shelter home, orphanage, or charitable organization on FoodBridge to request surplus food donations." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container" style="max-width: 820px;">
            <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="litAlert" runat="server" />
            </asp:Panel>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#129309; NGO &amp; Charitable Organization Registration</span>
                    <a href="<%= ResolveUrl("~/Account/Login.aspx?role=NGO") %>" class="btn btn-outline btn-sm">Already Registered? Login</a>
                </div>
                <div class="card-body">
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="form-label">Organization / NGO Name *</label>
                            <asp:TextBox ID="txtOrgName" runat="server" CssClass="form-control" placeholder="e.g., Asha Deepa Charitable Trust" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Government Registration / Trust Number *</label>
                            <asp:TextBox ID="txtRegNumber" runat="server" CssClass="form-control" placeholder="e.g., NGO-KA-2024-1092" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Authorized Contact Person *</label>
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" placeholder="Coordinator name" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Mobile Number (10 digits) *</label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="e.g., 9900112233" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="ngo@example.org" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Daily Number of Beneficiaries *</label>
                            <asp:TextBox ID="txtBeneficiaries" runat="server" CssClass="form-control" Text="100" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">City *</label>
                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="form-label">Complete Organization / Shelter Address *</label>
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Full delivery address for volunteer food drop-off" />
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

                    <asp:Button ID="btnRegister" runat="server" Text="Register NGO Account" CssClass="btn btn-primary btn-lg" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
