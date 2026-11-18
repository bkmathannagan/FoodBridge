<%@ Page Title="Manage Food Requests – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageRequests.aspx.vb" Inherits="FoodBridge.ManageRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage NGO Food Requests</h1>
            <p class="page-subtitle">Approve or reject NGO food allocation requests and dispatch volunteers for pickup and delivery.</p>
        </div>
        <a href="<%= ResolveUrl("~/Admin/AssignVolunteers.aspx") %>" class="btn btn-primary btn-sm">&#128666; Go to Assign Volunteers</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Search NGO / Food Item</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search keyword..." />
        </div>
        <div class="filter-item">
            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Request Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Requests" Value="All" />
                <asp:ListItem Text="Pending Approval" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Assigned" Value="Assigned" />
                <asp:ListItem Text="Processing" Value="Processing" />
                <asp:ListItem Text="Delivered" Value="Delivered" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Filter Requests" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No NGO food requests found.">
                <Columns>
                    <asp:BoundField DataField="RequestID" HeaderText="Req #" />
                    <asp:TemplateField HeaderText="NGO / Organization">
                        <ItemTemplate>
                            <strong><%# Eval("NGOName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("ContactPerson") %> (<%# Eval("ContactNumber") %>)</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Requested Food &amp; Donor">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);">Donor: <%# Eval("DonorName") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Requested Qty">
                        <ItemTemplate>
                            <strong><%# Eval("RequestedQuantity") %> <%# Eval("QuantityUnit") %></strong>
                            <div style="font-size: 11.5px; color: var(--text-muted);"><%# Eval("NumberOfBeneficiaries") %> beneficiaries</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:TemplateField HeaderText="Request Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("RequestStatus") %>"><%# Eval("RequestStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AssignedVolunteer" HeaderText="Volunteer" NullDisplayText="Unassigned" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <asp:LinkButton ID="btnApprove" runat="server" CommandName="ApproveReq" CommandArgument='<%# Eval("RequestID") %>' CssClass="btn btn-primary btn-sm" Text="Approve" Visible='<%# Convert.ToString(Eval("RequestStatus")) = "Pending" %>' />
                                <asp:LinkButton ID="btnReject" runat="server" CommandName="RejectReq" CommandArgument='<%# Eval("RequestID") %>' CssClass="btn btn-danger btn-sm" Text="Reject" Visible='<%# Convert.ToString(Eval("RequestStatus")) = "Pending" %>' OnClientClick="return confirm('Reject this request and release reserved food quantity?');" />
                                <a href="<%# ResolveUrl("~/Admin/AssignVolunteers.aspx?reqId=" & Eval("RequestID")) %>" class="btn btn-accent btn-sm">Assign Volunteer</a>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
