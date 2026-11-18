<%@ Page Title="Volunteer Dashboard – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="FoodBridge.VolunteerDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Rescue Volunteer Dashboard</h1>
            <p class="page-subtitle">Welcome, <strong><asp:Literal ID="litVolunteerName" runat="server" /></strong>! Coordinate food pickups and NGO deliveries.</p>
        </div>
        <div style="display: flex; gap: 10px; align-items: center;">
            <span>Current Status: <asp:Literal ID="litAvailBadge" runat="server" /></span>
            <asp:Button ID="btnToggleAvailability" runat="server" Text="Toggle Availability" CssClass="btn btn-outline btn-sm" />
        </div>
    </div>

    <!-- Volunteer Statistic Cards -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-label">Total Assigned Tasks</div>
            <div class="stat-value"><asp:Literal ID="litTotalTasks" runat="server" Text="0" /></div>
            <div class="stat-meta">All Rescue Assignments</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Pending Tasks</div>
            <div class="stat-value"><asp:Literal ID="litPendingTasks" runat="server" Text="0" /></div>
            <div class="stat-meta">Awaiting Your Acceptance</div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Accepted / Active Tasks</div>
            <div class="stat-value"><asp:Literal ID="litAcceptedTasks" runat="server" Text="0" /></div>
            <div class="stat-meta">Collection / In Transit</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Completed Deliveries</div>
            <div class="stat-value"><asp:Literal ID="litCompleted" runat="server" Text="0" /></div>
            <div class="stat-meta"><asp:Literal ID="litDeliveredQty" runat="server" Text="0" /> Kg Delivered</div>
        </div>
    </div>

    <!-- Recent Assignments -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128666; Recent Collection &amp; Delivery Assignments</span>
            <a href="<%= ResolveUrl("~/Volunteer/MyTasks.aspx") %>" class="btn btn-primary btn-sm">Manage All Tasks &rarr;</a>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRecentTasks" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No collection tasks assigned yet.">
                <Columns>
                    <asp:BoundField DataField="TaskID" HeaderText="Task #" />
                    <asp:TemplateField HeaderText="Food Item &amp; Quantity">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--primary);"><%# Eval("AssignedQuantity") %> <%# Eval("QuantityUnit") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pickup (Donor)">
                        <ItemTemplate>
                            <strong><%# Eval("DonorName") %></strong> (<%# Eval("DonorMobile") %>)
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("DonorAddress") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delivery (NGO Receiver)">
                        <ItemTemplate>
                            <strong><%# Eval("NGOName") %></strong> (<%# Eval("ReceiverMobile") %>)
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("ReceiverAddress") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Task Status">
                        <ItemTemplate>
                            <span class="badge badge-<%# Eval("TaskStatus") %>"><%# Eval("TaskStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <a href="<%# ResolveUrl("~/Volunteer/UpdateDelivery.aspx?taskId=" & Eval("TaskID")) %>" class="btn btn-primary btn-sm">Update Status</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
