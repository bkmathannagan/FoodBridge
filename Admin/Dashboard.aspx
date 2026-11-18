<%@ Page Title="Administrator Dashboard – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="FoodBridge.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Central Administrator Dashboard</h1>
            <p class="page-subtitle">Real-time operational overview of food donors, NGOs, rescue volunteers, donations, and deliveries.</p>
        </div>
        <div style="display: flex; gap: 8px; flex-wrap: wrap;">
            <a href="<%= ResolveUrl("~/Admin/ManageDonations.aspx") %>" class="btn btn-primary btn-sm">Verify Donations</a>
            <a href="<%= ResolveUrl("~/Admin/ManageRequests.aspx") %>" class="btn btn-accent btn-sm">Approve Requests</a>
            <a href="<%= ResolveUrl("~/Admin/AssignVolunteers.aspx") %>" class="btn btn-outline btn-sm">Dispatch Volunteer</a>
            <a href="<%= ResolveUrl("~/Admin/Reports.aspx") %>" class="btn btn-outline btn-sm">Generate Reports</a>
        </div>
    </div>

    <!-- All 10 Mandatory Administrator Statistic Cards -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-label">Total Registered Donors</div>
            <div class="stat-value"><asp:Literal ID="litTotalDonors" runat="server" Text="0" /></div>
            <div class="stat-meta"><a href="<%= ResolveUrl("~/Admin/ManageDonors.aspx") %>">Manage Donors &rarr;</a></div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Total NGOs</div>
            <div class="stat-value"><asp:Literal ID="litTotalNGOs" runat="server" Text="0" /></div>
            <div class="stat-meta"><a href="<%= ResolveUrl("~/Admin/ManageNGOs.aspx") %>">Manage NGOs &rarr;</a></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Total Volunteers</div>
            <div class="stat-value"><asp:Literal ID="litTotalVolunteers" runat="server" Text="0" /></div>
            <div class="stat-meta"><a href="<%= ResolveUrl("~/Admin/ManageVolunteers.aspx") %>">Manage Fleet &rarr;</a></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Total Donations</div>
            <div class="stat-value"><asp:Literal ID="litTotalDonations" runat="server" Text="0" /></div>
            <div class="stat-meta">All Logged Surplus</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Pending Donations</div>
            <div class="stat-value"><asp:Literal ID="litPendingDonations" runat="server" Text="0" /></div>
            <div class="stat-meta"><a href="<%= ResolveUrl("~/Admin/ManageDonations.aspx?status=Pending") %>">Review Now &rarr;</a></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Approved Donations</div>
            <div class="stat-value"><asp:Literal ID="litApprovedDonations" runat="server" Text="0" /></div>
            <div class="stat-meta">Active Inventory</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Total Food Rescued (Kg)</div>
            <div class="stat-value"><asp:Literal ID="litTotalFoodKg" runat="server" Text="0" /></div>
            <div class="stat-meta">Kilograms Saved</div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Completed Deliveries</div>
            <div class="stat-value"><asp:Literal ID="litCompletedDeliveries" runat="server" Text="0" /></div>
            <div class="stat-meta">Confirmed Handovers</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Pending Requests</div>
            <div class="stat-value"><asp:Literal ID="litPendingRequests" runat="server" Text="0" /></div>
            <div class="stat-meta"><a href="<%= ResolveUrl("~/Admin/ManageRequests.aspx?status=Pending") %>">Approve Requests &rarr;</a></div>
        </div>
        <div class="stat-card danger">
            <div class="stat-label">Expired Donations</div>
            <div class="stat-value"><asp:Literal ID="litExpiredDonations" runat="server" Text="0" /></div>
            <div class="stat-meta">Auto-Filtered Out</div>
        </div>
    </div>

    <!-- Charts & Monthly Food Rescue Summary -->
    <div class="form-grid" style="grid-template-columns: 1fr 1fr 1.2fr;">
        <!-- Donation Status Chart -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128202; Donation Status Chart</span>
            </div>
            <div class="card-body">
                <asp:Repeater ID="rptStatusChart" runat="server">
                    <ItemTemplate>
                        <div class="chart-row">
                            <div class="chart-label-row">
                                <span><%# Eval("StatusName") %></span>
                                <span><%# Eval("TotalCount") %> (<%# Eval("TotalQuantity") %> Kg)</span>
                            </div>
                            <div class="chart-bar-track">
                                <div class="chart-bar-fill" style="width: <%# Math.Min(100, Convert.ToInt32(Eval("TotalCount")) * 20) %>%;"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- Food Category Chart -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#129367; Food Category Chart</span>
            </div>
            <div class="card-body">
                <asp:Repeater ID="rptCategoryChart" runat="server">
                    <ItemTemplate>
                        <div class="chart-row">
                            <div class="chart-label-row">
                                <span><%# Eval("FoodCategory") %></span>
                                <span><%# Eval("DonationCount") %> items (<%# Eval("TotalQuantity") %> Kg)</span>
                            </div>
                            <div class="chart-bar-track">
                                <div class="chart-bar-fill" style="width: <%# Math.Min(100, Convert.ToInt32(Eval("DonationCount")) * 22) %>%;"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- Monthly Food Rescue Summary -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128197; Monthly Food Rescue Summary</span>
            </div>
            <div class="card-body table-responsive">
                <asp:GridView ID="gvMonthlySummary" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="MonthPeriod" HeaderText="Month" />
                        <asp:BoundField DataField="TotalDonations" HeaderText="Donations" />
                        <asp:BoundField DataField="TotalQuantityKg" HeaderText="Total Kg" />
                        <asp:BoundField DataField="TotalBeneficiaries" HeaderText="People Fed" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- Recent Donations Table -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#127858; Recent Food Donations</span>
            <a href="<%= ResolveUrl("~/Admin/ManageDonations.aspx") %>" class="btn btn-outline btn-sm">Manage All Donations &rarr;</a>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRecentDonations" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="DonationID" HeaderText="ID #" />
                    <asp:BoundField DataField="FoodName" HeaderText="Food Item" />
                    <asp:BoundField DataField="DonorName" HeaderText="Donor" />
                    <asp:BoundField DataField="FoodCategory" HeaderText="Category" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate><%# Eval("FoodQuantity") %> <%# Eval("QuantityUnit") %> (Avail: <%# Eval("AvailableQuantity") %>)</ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:TemplateField HeaderText="Approval">
                        <ItemTemplate><span class="badge badge-<%# Eval("ApprovalStatus") %>"><%# Eval("ApprovalStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Recent Food Requests & Latest Volunteer Assignments -->
    <div class="form-grid">
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128203; Recent Food Requests</span>
                <a href="<%= ResolveUrl("~/Admin/ManageRequests.aspx") %>" class="btn btn-outline btn-sm">View All</a>
            </div>
            <div class="card-body table-responsive">
                <asp:GridView ID="gvRecentRequests" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="RequestID" HeaderText="Req #" />
                        <asp:BoundField DataField="NGOName" HeaderText="NGO" />
                        <asp:BoundField DataField="FoodName" HeaderText="Food" />
                        <asp:BoundField DataField="RequestedQuantity" HeaderText="Qty" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate><span class="badge badge-<%# Eval("RequestStatus") %>"><%# Eval("RequestStatus") %></span></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128666; Latest Volunteer Assignments</span>
                <a href="<%= ResolveUrl("~/Admin/AssignVolunteers.aspx") %>" class="btn btn-outline btn-sm">View All</a>
            </div>
            <div class="card-body table-responsive">
                <asp:GridView ID="gvLatestAssignments" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="TaskID" HeaderText="Task #" />
                        <asp:BoundField DataField="VolunteerName" HeaderText="Volunteer" />
                        <asp:BoundField DataField="FoodName" HeaderText="Food Item" />
                        <asp:BoundField DataField="NGOName" HeaderText="Receiver NGO" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate><span class="badge badge-<%# Eval("TaskStatus") %>"><%# Eval("TaskStatus") %></span></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
