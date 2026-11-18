<%@ Page Title="Manage Volunteers – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageVolunteers.aspx.vb" Inherits="FoodBridge.ManageVolunteers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage Rescue Volunteers</h1>
            <p class="page-subtitle">Monitor volunteer availability, city coverage, completed tasks, and account status.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Search Volunteer</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Name, email, or phone..." />
        </div>
        <div class="filter-item">
            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Availability</label>
            <asp:DropDownList ID="ddlAvailability" runat="server" CssClass="form-control">
                <asp:ListItem Text="All" Value="All" />
                <asp:ListItem Text="Available" Value="Available" />
                <asp:ListItem Text="Unavailable" Value="Unavailable" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Filter Volunteers" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvVolunteers" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No volunteers found.">
                <Columns>
                    <asp:BoundField DataField="VolunteerID" HeaderText="ID #" />
                    <asp:BoundField DataField="VolunteerName" HeaderText="Volunteer Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="TotalTasks" HeaderText="Assigned Tasks" />
                    <asp:TemplateField HeaderText="Availability">
                        <ItemTemplate><span class="badge badge-<%# Eval("AvailabilityStatus") %>"><%# Eval("AvailabilityStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Account Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("AccountStatus") %>"><%# Eval("AccountStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnActivate" runat="server" CommandName="ActivateVol" CommandArgument='<%# Eval("VolunteerID") %>' CssClass="btn btn-primary btn-sm" Text="Activate" Visible='<%# Convert.ToString(Eval("AccountStatus")) <> "Active" %>' />
                            <asp:LinkButton ID="btnBlock" runat="server" CommandName="BlockVol" CommandArgument='<%# Eval("VolunteerID") %>' CssClass="btn btn-danger btn-sm" Text="Deactivate" Visible='<%# Convert.ToString(Eval("AccountStatus")) = "Active" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
