<%@ Page Title="Assign Volunteers & Track Deliveries – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="AssignVolunteers.aspx.vb" Inherits="FoodBridge.AssignVolunteers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Volunteer Assignment &amp; Delivery Coordination</h1>
            <p class="page-subtitle">Assign or reassign available volunteers to approved food requests and monitor collection/delivery status.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128666; Dispatch / Reassign Volunteer to Food Request</span>
        </div>
        <div class="card-body">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Select NGO Food Request *</label>
                    <asp:DropDownList ID="ddlRequests" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Select Available Volunteer *</label>
                    <asp:DropDownList ID="ddlVolunteers" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="form-label">Assignment / Dispatch Instructions</label>
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="e.g., Collect insulated boxes from donor kitchen and deliver to shelter before 7 PM" />
            </div>
            <asp:Button ID="btnAssign" runat="server" Text="Assign Volunteer for Collection &amp; Delivery" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128203; Active &amp; Historical Volunteer Assignments</span>
            <div style="display: flex; gap: 8px;">
                <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="True">
                    <asp:ListItem Text="All Task Statuses" Value="All" />
                    <asp:ListItem Text="Assigned" Value="Assigned" />
                    <asp:ListItem Text="Accepted" Value="Accepted" />
                    <asp:ListItem Text="Food Collected" Value="Food Collected" />
                    <asp:ListItem Text="Out for Delivery" Value="Out for Delivery" />
                    <asp:ListItem Text="Successfully Delivered" Value="Successfully Delivered" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />
                    <asp:ListItem Text="Reassigned" Value="Reassigned" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvAssignments" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No volunteer assignments found.">
                <Columns>
                    <asp:BoundField DataField="TaskID" HeaderText="Task #" />
                    <asp:TemplateField HeaderText="Volunteer">
                        <ItemTemplate>
                            <strong><%# Eval("VolunteerName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("VolunteerMobile") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Food &amp; Quantity">
                        <ItemTemplate>
                            <strong><%# Eval("FoodName") %></strong>
                            <div style="font-size: 12px; color: var(--primary);"><%# Eval("AssignedQuantity") %> <%# Eval("QuantityUnit") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DonorAddress" HeaderText="Pickup Address" />
                    <asp:BoundField DataField="ReceiverAddress" HeaderText="NGO Delivery Address" />
                    <asp:TemplateField HeaderText="Task Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("TaskStatus") %>"><%# Eval("TaskStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CollectionRemarks" HeaderText="Collection / Delivery Notes" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
