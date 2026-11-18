<%@ Page Title="My Assigned Collection Tasks – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyTasks.aspx.vb" Inherits="FoodBridge.VolunteerTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">My Assigned Food Collection &amp; Delivery Tasks</h1>
            <p class="page-subtitle">Accept or reject new assignments, view donor and receiver contact details, and update delivery progress.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Filter by Task Status</label>
            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="True">
                <asp:ListItem Text="All Tasks" Value="All" />
                <asp:ListItem Text="Assigned (New)" Value="Assigned" />
                <asp:ListItem Text="Accepted" Value="Accepted" />
                <asp:ListItem Text="Food Collected" Value="Food Collected" />
                <asp:ListItem Text="Out for Delivery" Value="Out for Delivery" />
                <asp:ListItem Text="Successfully Delivered" Value="Successfully Delivered" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
            </asp:DropDownList>
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No tasks found for the selected filter.">
                <Columns>
                    <asp:BoundField DataField="TaskID" HeaderText="Task #" />
                    <asp:TemplateField HeaderText="Food &amp; Quantity">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--primary);"><%# Eval("AssignedQuantity") %> <%# Eval("QuantityUnit") %> (<%# Eval("FoodCategory") %>)</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Donor Contact &amp; Collection Address">
                        <ItemTemplate>
                            <strong><%# Eval("DonorName") %></strong><br />
                            Contact: <%# Eval("DonorContact") %> (<%# Eval("DonorMobile") %>)
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("DonorAddress") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NGO Receiver &amp; Delivery Address">
                        <ItemTemplate>
                            <strong><%# Eval("NGOName") %></strong><br />
                            Contact: <%# Eval("ReceiverContact") %> (<%# Eval("ReceiverMobile") %>)
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("ReceiverAddress") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="badge badge-<%# Eval("TaskStatus") %>"><%# Eval("TaskStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <asp:LinkButton ID="btnAccept" runat="server" CommandName="QuickAccept" CommandArgument='<%# Eval("TaskID") %>' CssClass="btn btn-primary btn-sm" Text="Accept" Visible='<%# Convert.ToString(Eval("TaskStatus")) = "Assigned" OrElse Convert.ToString(Eval("TaskStatus")) = "Reassigned" %>' />
                                <asp:LinkButton ID="btnReject" runat="server" CommandName="QuickReject" CommandArgument='<%# Eval("TaskID") %>' CssClass="btn btn-danger btn-sm" Text="Reject" Visible='<%# Convert.ToString(Eval("TaskStatus")) = "Assigned" OrElse Convert.ToString(Eval("TaskStatus")) = "Reassigned" %>' OnClientClick="return confirm('Reject this task so Admin can reassign another volunteer?');" />
                                <a href="<%# ResolveUrl("~/Volunteer/UpdateDelivery.aspx?taskId=" & Eval("TaskID")) %>" class="btn btn-accent btn-sm">Update Delivery</a>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
