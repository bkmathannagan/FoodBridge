<%@ Page Title="Comprehensive Reports & Export – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Reports.aspx.vb" Inherits="FoodBridge.AdminReports" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">FoodBridge Comprehensive Reports &amp; Analytics</h1>
            <p class="page-subtitle">Generate date-wise, category-wise, city-wise, and status-filtered reports with Print and Excel Export.</p>
        </div>
        <div class="no-print" style="display: flex; gap: 10px;">
            <button type="button" class="btn btn-outline" onclick="return printReportArea();">&#128424; Print Report</button>
            <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel (.xls)" CssClass="btn btn-accent" />
        </div>
    </div>

    <!-- Filter Controls -->
    <div class="filter-bar no-print">
        <div class="filter-item" style="min-width: 240px;">
            <label class="form-label">Select Report Type (15 Built-in Reports) *</label>
            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" AutoPostBack="True">
                <asp:ListItem Text="1. Overall Food Rescue Summary" Value="OverallFoodRescueSummary" />
                <asp:ListItem Text="2. Food Donation Report (All)" Value="FoodDonation" />
                <asp:ListItem Text="3. Pending Donation Report" Value="PendingDonation" />
                <asp:ListItem Text="4. Approved Donation Report" Value="ApprovedDonation" />
                <asp:ListItem Text="5. Rejected Donation Report" Value="RejectedDonation" />
                <asp:ListItem Text="6. Expired Food Report" Value="ExpiredFood" />
                <asp:ListItem Text="7. Food Request Report" Value="FoodRequest" />
                <asp:ListItem Text="8. Volunteer Assignment Report" Value="VolunteerAssignment" />
                <asp:ListItem Text="9. Food Collection Report" Value="FoodCollection" />
                <asp:ListItem Text="10. Food Delivery Report" Value="FoodDelivery" />
                <asp:ListItem Text="11. Food Category Report" Value="FoodCategory" />
                <asp:ListItem Text="12. Monthly Food Rescue Report" Value="MonthlyFoodRescue" />
                <asp:ListItem Text="13. Donor Registration Report" Value="DonorRegistration" />
                <asp:ListItem Text="14. NGO Registration Report" Value="NGORegistration" />
                <asp:ListItem Text="15. Volunteer Registration Report" Value="VolunteerRegistration" />
            </asp:DropDownList>
        </div>
        <div class="filter-item">
            <label class="form-label">From Date (YYYY-MM-DD)</label>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="e.g., 2026-01-01" />
        </div>
        <div class="filter-item">
            <label class="form-label">To Date (YYYY-MM-DD)</label>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="e.g., 2026-12-31" />
        </div>
        <div class="filter-item">
            <label class="form-label">Filter by Category</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Filter by City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Filter by Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Statuses" Value="All" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Assigned" Value="Assigned" />
                <asp:ListItem Text="Delivered" Value="Delivered" />
                <asp:ListItem Text="Verified" Value="Verified" />
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Expired" Value="Expired" />
            </asp:DropDownList>
        </div>
        <div class="filter-item">
            <label class="form-label">Search Keyword</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Name or food..." />
        </div>
        <div>
            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary" />
        </div>
    </div>

    <!-- Summary Totals Ribbon -->
    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-label">Report Title</div>
            <div style="font-size: 17px; font-weight: 800; margin-top: 6px;"><asp:Literal ID="litReportName" runat="server" /></div>
            <div class="stat-meta">Generated on <%= DateTime.Now.ToString("dd MMM yyyy HH:mm") %></div>
        </div>
        <div class="stat-card info">
            <div class="stat-label">Total Records Found</div>
            <div class="stat-value"><asp:Literal ID="litTotalRecords" runat="server" Text="0" /></div>
            <div class="stat-meta">Matching Filter Criteria</div>
        </div>
        <div class="stat-card accent">
            <div class="stat-label">Total Food Quantity Summary</div>
            <div class="stat-value"><asp:Literal ID="litTotalQuantity" runat="server" Text="0" /></div>
            <div class="stat-meta">Cumulative Units / Kg</div>
        </div>
    </div>

    <!-- Report GridView -->
    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True" CssClass="data-table" GridLines="None" EmptyDataText="No records found matching the selected report filters." />
        </div>
    </div>
</asp:Content>
