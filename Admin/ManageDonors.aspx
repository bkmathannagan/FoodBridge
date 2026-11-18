<%@ Page Title="Manage Food Donors – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageDonors.aspx.vb" Inherits="FoodBridge.ManageDonors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage Registered Food Donors</h1>
            <p class="page-subtitle">View, search, filter, activate, or block food donors across all cities.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Search Donor / Email / Mobile</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search keyword..." />
        </div>
        <div class="filter-item">
            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Account Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Statuses" Value="All" />
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Blocked" Value="Blocked" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Filter Donors" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvDonors" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No donors found.">
                <Columns>
                    <asp:BoundField DataField="DonorID" HeaderText="ID #" />
                    <asp:TemplateField HeaderText="Donor / Organization">
                        <ItemTemplate>
                            <strong><%# Eval("DonorName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("DonorType") %> &bull; Contact: <%# Eval("ContactPerson") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="TotalDonations" HeaderText="Donations" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("AccountStatus") %>"><%# Eval("AccountStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px;">
                                <asp:LinkButton ID="btnActivate" runat="server" CommandName="SetActive" CommandArgument='<%# Eval("DonorID") %>' CssClass="btn btn-primary btn-sm" Text="Activate" Visible='<%# Convert.ToString(Eval("AccountStatus")) <> "Active" %>' />
                                <asp:LinkButton ID="btnBlock" runat="server" CommandName="SetBlocked" CommandArgument='<%# Eval("DonorID") %>' CssClass="btn btn-danger btn-sm" Text="Block" Visible='<%# Convert.ToString(Eval("AccountStatus")) = "Active" %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
