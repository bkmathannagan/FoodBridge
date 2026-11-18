<%@ Page Title="Manage Food Donations – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageDonations.aspx.vb" Inherits="FoodBridge.ManageDonations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage &amp; Approve Surplus Food Donations</h1>
            <p class="page-subtitle">Verify food expiry timelines, approve safe donations for NGO listing, or reject unsuitable entries.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Search Food / Donor</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Dish name or donor..." />
        </div>
        <div class="filter-item">
            <label class="form-label">Category</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Statuses" Value="All" />
                <asp:ListItem Text="Pending Approval" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Assigned" Value="Assigned" />
                <asp:ListItem Text="Collected" Value="Collected" />
                <asp:ListItem Text="Delivered" Value="Delivered" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
                <asp:ListItem Text="Expired" Value="Expired" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvDonations" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No food donations match your filter.">
                <Columns>
                    <asp:BoundField DataField="DonationID" HeaderText="ID #" />
                    <asp:TemplateField HeaderText="Food Details">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <span class="badge badge-<%# Eval("UrgencyTag") %>"><%# Eval("UrgencyTag") %></span>
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("FoodCategory") %> (<%# Eval("FoodType") %>) &bull; ~<%# Eval("NumberOfPersonsServed") %> persons</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Donor &amp; Location">
                        <ItemTemplate>
                            <strong><%# Eval("DonorName") %></strong> (<%# Eval("DonorMobile") %>)
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("PickupAddress") %>, <%# Eval("City") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total / Avail Qty">
                        <ItemTemplate>
                            <strong><%# Eval("FoodQuantity") %> <%# Eval("QuantityUnit") %></strong>
                            <div style="font-size: 11.5px; color: var(--primary);">Avail: <%# Eval("AvailableQuantity") %> <%# Eval("QuantityUnit") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd MMM yyyy") %><br />
                            <small><%# Eval("ExpiryTime") %></small>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approval Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("ApprovalStatus") %>"><%# Eval("ApprovalStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Collection Status">
                        <ItemTemplate><span class="badge badge-info"><%# Eval("CollectionStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admin Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <asp:LinkButton ID="btnApprove" runat="server" CommandName="ApproveDon" CommandArgument='<%# Eval("DonationID") %>' CssClass="btn btn-primary btn-sm" Text="Approve" Visible='<%# Convert.ToString(Eval("ApprovalStatus")) = "Pending" %>' />
                                <asp:LinkButton ID="btnReject" runat="server" CommandName="RejectDon" CommandArgument='<%# Eval("DonationID") %>' CssClass="btn btn-danger btn-sm" Text="Reject" Visible='<%# Convert.ToString(Eval("ApprovalStatus")) = "Pending" OrElse Convert.ToString(Eval("ApprovalStatus")) = "Approved" %>' OnClientClick="return confirm('Reject this food donation?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
