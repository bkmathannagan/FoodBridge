<%@ Page Title="Announcements, Feedback & Settings – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Settings.aspx.vb" Inherits="FoodBridge.AdminSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Announcements, Feedback, Complaints &amp; Admin Settings</h1>
            <p class="page-subtitle">Publish website announcements, review user feedback/complaints, and change administrator password.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <!-- Publish Website Announcement -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128226; Publish Website Announcement</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Announcement Title *</label>
                    <asp:TextBox ID="txtAnnTitle" runat="server" CssClass="form-control" placeholder="e.g., Monsoon Food Rescue Drive" />
                </div>
                <div class="form-group">
                    <label class="form-label">Target Audience</label>
                    <asp:DropDownList ID="ddlTargetRole" runat="server" CssClass="form-control">
                        <asp:ListItem Text="All Visitors &amp; Users" Value="All" />
                        <asp:ListItem Text="Food Donors" Value="Donor" />
                        <asp:ListItem Text="NGOs" Value="NGO" />
                        <asp:ListItem Text="Volunteers" Value="Volunteer" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="form-label">Announcement Message *</label>
                    <asp:TextBox ID="txtAnnContent" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
                </div>
                <asp:Button ID="btnPublishAnn" runat="server" Text="Publish Announcement" CssClass="btn btn-primary btn-sm" />

                <div class="table-responsive" style="margin-top: 16px;">
                    <asp:GridView ID="gvAnnouncements" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="AnnouncementID" HeaderText="ID" />
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="TargetRole" HeaderText="Target" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelAnn" runat="server" CommandName="DeleteAnn" CommandArgument='<%# Eval("AnnouncementID") %>' CssClass="btn btn-danger btn-sm" Text="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Change Administrator Password -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128274; Change Administrator Password</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Current Admin Password *</label>
                    <asp:TextBox ID="txtCurrentPwd" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">New Password (min 6 chars) *</label>
                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Confirm New Password *</label>
                    <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <asp:Button ID="btnChangePwd" runat="server" Text="Update Admin Password" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>

    <!-- Feedback & Complaints Management -->
    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128172; User Feedback &amp; Ratings</span>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No feedback entries.">
                <Columns>
                    <asp:BoundField DataField="FeedbackID" HeaderText="ID #" />
                    <asp:BoundField DataField="UserName" HeaderText="Submitted By" />
                    <asp:BoundField DataField="UserRole" HeaderText="Role" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                    <asp:BoundField DataField="Message" HeaderText="Message" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="card">
        <div class="card-header">
            <span class="card-title">&#9888; Logged Complaints &amp; Issue Resolution</span>
        </div>
        <div class="card-body table-responsive">
            <asp:GridView ID="gvComplaints" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No complaints logged.">
                <Columns>
                    <asp:BoundField DataField="ComplaintID" HeaderText="ID #" />
                    <asp:BoundField DataField="UserName" HeaderText="User" />
                    <asp:BoundField DataField="UserRole" HeaderText="Role" />
                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Priority" HeaderText="Priority" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("Status") %>"><%# Eval("Status") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnResolve" runat="server" CommandName="ResolveComp" CommandArgument='<%# Eval("ComplaintID") %>' CssClass="btn btn-primary btn-sm" Text="Mark Resolved" Visible='<%# Convert.ToString(Eval("Status")) <> "Resolved" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
