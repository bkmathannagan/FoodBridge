<%@ Page Title="Donor Dashboard – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="FoodBridge.DonorDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Food Donor Dashboard</h1>
            <p class="page-subtitle">Welcome back, <strong><asp:Literal ID="litDonorName" runat="server" /></strong>! Manage your surplus food donations and track collections.</p>
        </div>
        <div>
            <a href="<%= ResolveUrl("~/Donor/AddDonation.aspx") %>" class="btn btn-primary">&#10133; Quick Add Food Donation</a>
        </div>
    </div>

    <!-- Donor Statistic Cards -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-label">Total Donations</div>
            <div class="stat-value"><asp:Literal ID="litTotal" runat="server" Text="0" /></div>
            <div class="stat-meta"><asp:Literal ID="litTotalQty" runat="server" Text="0" /> Kg Donated</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Pending Donations</div>
            <div class="stat-value"><asp:Literal ID="litPending" runat="server" Text="0" /></div>
            <div class="stat-meta">Awaiting Verification</div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Approved Donations</div>
            <div class="stat-value"><asp:Literal ID="litApproved" runat="server" Text="0" /></div>
            <div class="stat-meta">Live for NGOs</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Collected Donations</div>
            <div class="stat-value"><asp:Literal ID="litCollected" runat="server" Text="0" /></div>
            <div class="stat-meta">Picked Up by Volunteer</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Delivered Donations</div>
            <div class="stat-value"><asp:Literal ID="litDelivered" runat="server" Text="0" /></div>
            <div class="stat-meta">~<asp:Literal ID="litPeopleServed" runat="server" Text="0" /> People Fed</div>
        </div>
    </div>

    <!-- Recent Donation History -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#127858; Recent Donation History</span>
            <a href="<%= ResolveUrl("~/Donor/MyDonations.aspx") %>" class="btn btn-outline btn-sm">View All My Donations &rarr;</a>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRecentDonations" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="You have not submitted any food donations yet. Click 'Quick Add Food Donation' to begin!">
                <Columns>
                    <asp:BoundField DataField="DonationID" HeaderText="ID #" />
                    <asp:BoundField DataField="FoodName" HeaderText="Food Item" />
                    <asp:BoundField DataField="FoodCategory" HeaderText="Category" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <strong><%# Eval("FoodQuantity") %> <%# Eval("QuantityUnit") %></strong>
                            <div style="font-size: 11.5px; color: var(--text-muted);">Avail: <%# Eval("AvailableQuantity") %> <%# Eval("QuantityUnit") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd MMM yyyy") %> (<%# Eval("ExpiryTime") %>)
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approval Status">
                        <ItemTemplate>
                            <span class="badge badge-<%# Eval("ApprovalStatus") %>"><%# Eval("ApprovalStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Collection Status">
                        <ItemTemplate>
                            <span class="badge badge-info"><%# Eval("CollectionStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AssignedVolunteerInfo" HeaderText="Assigned Volunteer" NullDisplayText="Not Assigned Yet" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <a href="<%# ResolveUrl("~/Donor/DonationDetails.aspx?id=" & Eval("DonationID")) %>" class="btn btn-outline btn-sm">Track</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
