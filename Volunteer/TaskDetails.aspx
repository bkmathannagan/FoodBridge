<%@ Page Title="Volunteer Profile & Activity History – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="TaskDetails.aspx.vb" Inherits="FoodBridge.VolunteerProfileTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Volunteer Profile &amp; Completed Activity History</h1>
            <p class="page-subtitle">Manage your personal details, availability status, password, and view completed rescue missions.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128100; Personal Profile &amp; Availability</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Volunteer Name *</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
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
                    <label class="form-label">Mobile Number *</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Email Address (Read-only)</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="True" />
                </div>
                <div class="form-group">
                    <label class="form-label">City *</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Availability Status *</label>
                    <asp:DropDownList ID="ddlAvailability" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Available" Value="Available" />
                        <asp:ListItem Text="Unavailable" Value="Unavailable" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="form-label">Residential Address *</label>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                </div>
                <asp:Button ID="btnSaveProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary" />
            </div>
        </div>

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
    </div>

    <!-- Completed Volunteer Activity History -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#127942; Completed Volunteer Deliveries &amp; Activity History</span>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvCompletedHistory" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No completed deliveries yet.">
                <Columns>
                    <asp:BoundField DataField="TaskID" HeaderText="Task #" />
                    <asp:BoundField DataField="FoodName" HeaderText="Food Item" />
                    <asp:BoundField DataField="DonorName" HeaderText="Collected From" />
                    <asp:BoundField DataField="NGOName" HeaderText="Delivered To" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate><%# Eval("AssignedQuantity") %> <%# Eval("QuantityUnit") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TaskStatus" HeaderText="Status" />
                    <asp:BoundField DataField="DeliveryRemarks" HeaderText="Delivery Remarks" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
