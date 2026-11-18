<%@ Page Title="My Food Donations – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyDonations.aspx.vb" Inherits="FoodBridge.MyDonations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">My Submitted Food Donations &amp; History</h1>
            <p class="page-subtitle">View approval status, edit pending donations, cancel requests, or track volunteer collection.</p>
        </div>
        <a href="<%= ResolveUrl("~/Donor/AddDonation.aspx") %>" class="btn btn-primary">&#10133; Submit New Donation</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Filter by Donation / Collection Status</label>
            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="True">
                <asp:ListItem Text="All Donations" Value="All" />
                <asp:ListItem Text="Pending Approval" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Assigned" Value="Assigned" />
                <asp:ListItem Text="Collected" Value="Collected" />
                <asp:ListItem Text="Delivered" Value="Delivered" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
                <asp:ListItem Text="Expired" Value="Expired" />
            </asp:DropDownList>
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvMyDonations" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" DataKeyNames="DonationID" EmptyDataText="No donations found matching the selected status.">
                <Columns>
                    <asp:BoundField DataField="DonationID" HeaderText="ID #" />
                    <asp:TemplateField HeaderText="Food Item">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("FoodCategory") %> (<%# Eval("FoodType") %>)</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total / Avail Qty">
                        <ItemTemplate>
                            <%# Eval("FoodQuantity") %> <%# Eval("QuantityUnit") %>
                            <div style="font-size: 11.5px; color: var(--primary);">Rem: <%# Eval("AvailableQuantity") %> <%# Eval("QuantityUnit") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry Date">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd MMM yyyy") %><br />
                            <small><%# Eval("ExpiryTime") %></small>
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
                    <asp:BoundField DataField="AssignedVolunteerInfo" HeaderText="Assigned Volunteer" NullDisplayText="Awaiting Assignment" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <a href="<%# ResolveUrl("~/Donor/DonationDetails.aspx?id=" & Eval("DonationID")) %>" class="btn btn-outline btn-sm">Details</a>
                                <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# "~/Donor/AddDonation.aspx?editId=" & Eval("DonationID") %>' CssClass="btn btn-accent btn-sm" Text="Edit" Visible='<%# Convert.ToString(Eval("ApprovalStatus")) = "Pending" %>' />
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="CancelDonation" CommandArgument='<%# Eval("DonationID") %>' CssClass="btn btn-danger btn-sm" Text="Cancel" Visible='<%# Convert.ToString(Eval("ApprovalStatus")) = "Pending" OrElse Convert.ToString(Eval("ApprovalStatus")) = "Approved" %>' OnClientClick="return confirm('Are you sure you want to cancel this food donation?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
