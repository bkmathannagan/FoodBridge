<%@ Page Title="NGO Request & Receipt History – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="RequestHistory.aspx.vb" Inherits="FoodBridge.RequestHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Food Request &amp; Received History</h1>
            <p class="page-subtitle">Track approval status, assigned volunteers, delivery progress, and received food records.</p>
        </div>
        <a href="<%= ResolveUrl("~/NGO/AvailableFood.aspx") %>" class="btn btn-primary">&#10133; New Food Request</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Filter by Request Status</label>
            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="True">
                <asp:ListItem Text="All Requests" Value="All" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Assigned" Value="Assigned" />
                <asp:ListItem Text="Processing" Value="Processing" />
                <asp:ListItem Text="Delivered (Received)" Value="Delivered" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
            </asp:DropDownList>
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No food requests found.">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Req #" />
                    <asp:TemplateField HeaderText="Food Donation">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);">Donor: <%# Eval("DonorName") %> (<%# Eval("DonorMobile") %>)</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Requested Qty">
                        <ItemTemplate>
                            <strong><%# Eval("RequestedQuantity") %> <%# Eval("QuantityUnit") %></strong>
                            <div style="font-size: 11.5px; color: var(--text-muted);"><%# Eval("NumberOfBeneficiaries") %> beneficiaries</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Date">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("RequestDate")).ToString("dd MMM yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Status">
                        <ItemTemplate>
                            <span class="badge badge-<%# Eval("RequestStatus") %>"><%# Eval("RequestStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VolunteerInfo" HeaderText="Assigned Volunteer" NullDisplayText="Not Assigned" />
                    <asp:BoundField DataField="DeliveryStatus" HeaderText="Collection / Delivery Status" NullDisplayText="Pending" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnCancelReq" runat="server" CommandName="CancelReq" CommandArgument='<%# Eval("RequestID") %>' CssClass="btn btn-danger btn-sm" Text="Cancel Request" Visible='<%# Convert.ToString(Eval("RequestStatus")) = "Pending" OrElse Convert.ToString(Eval("RequestStatus")) = "Approved" %>' OnClientClick="return confirm('Cancel this food request and release the reserved quantity?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
