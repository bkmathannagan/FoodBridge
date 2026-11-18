<%@ Page Title="NGO Dashboard – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="FoodBridge.NGODashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">NGO &amp; Receiver Dashboard</h1>
            <p class="page-subtitle">Welcome, <strong><asp:Literal ID="litNGOName" runat="server" /></strong>! Browse available food donations and track your requests.</p>
        </div>
        <div>
            <a href="<%= ResolveUrl("~/NGO/AvailableFood.aspx") %>" class="btn btn-primary">&#127858; Browse Available Food</a>
        </div>
    </div>

    <!-- NGO Statistic Cards -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-label">Available Food Donations</div>
            <div class="stat-value"><asp:Literal ID="litAvailable" runat="server" Text="0" /></div>
            <div class="stat-meta">Ready for Request</div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Total Requests</div>
            <div class="stat-value"><asp:Literal ID="litTotalReq" runat="server" Text="0" /></div>
            <div class="stat-meta">All Submitted Requests</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Pending Requests</div>
            <div class="stat-value"><asp:Literal ID="litPendingReq" runat="server" Text="0" /></div>
            <div class="stat-meta">Awaiting Admin Approval</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Approved Requests</div>
            <div class="stat-value"><asp:Literal ID="litApprovedReq" runat="server" Text="0" /></div>
            <div class="stat-meta">Assigned / In Transit</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Received Donations</div>
            <div class="stat-value"><asp:Literal ID="litReceived" runat="server" Text="0" /></div>
            <div class="stat-meta"><asp:Literal ID="litReceivedQty" runat="server" Text="0" /> Kg Received</div>
        </div>
    </div>

    <!-- Recent Food Requests -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128203; Recent Food Requests &amp; Delivery Tracking</span>
            <a href="<%= ResolveUrl("~/NGO/RequestHistory.aspx") %>" class="btn btn-outline btn-sm">View Complete History &rarr;</a>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRecentRequests" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="Your organization has not submitted any food requests yet.">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Req #" />
                    <asp:BoundField DataField="FoodName" HeaderText="Food Item" />
                    <asp:BoundField DataField="DonorName" HeaderText="Donor" />
                    <asp:TemplateField HeaderText="Requested Qty">
                        <ItemTemplate>
                            <strong><%# Eval("RequestedQuantity") %> <%# Eval("QuantityUnit") %></strong>
                            <div style="font-size: 11.5px; color: var(--text-muted);"><%# Eval("NumberOfBeneficiaries") %> beneficiaries</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Status">
                        <ItemTemplate>
                            <span class="badge badge-<%# Eval("RequestStatus") %>"><%# Eval("RequestStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VolunteerInfo" HeaderText="Assigned Volunteer" NullDisplayText="Pending Assignment" />
                    <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" NullDisplayText="Awaiting Dispatch" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
