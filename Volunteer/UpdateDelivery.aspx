<%@ Page Title="Update Food Collection & Delivery – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="UpdateDelivery.aspx.vb" Inherits="FoodBridge.UpdateDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Update Collection &amp; Delivery Status</h1>
            <p class="page-subtitle">Record food pickup from donor, transit updates, and final delivery confirmation to NGO.</p>
        </div>
        <a href="<%= ResolveUrl("~/Volunteer/MyTasks.aspx") %>" class="btn btn-outline btn-sm">&larr; Back to My Tasks</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#9989; Update Task Status</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Select Assigned Task *</label>
                    <asp:DropDownList ID="ddlTasks" runat="server" CssClass="form-control" AutoPostBack="True" />
                </div>

                <div class="form-group">
                    <label class="form-label">Update Task Status To *</label>
                    <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Accepted (En Route to Donor)" Value="Accepted" />
                        <asp:ListItem Text="Food Collected from Donor" Value="Food Collected" />
                        <asp:ListItem Text="Out for Delivery to NGO" Value="Out for Delivery" />
                        <asp:ListItem Text="Successfully Delivered to NGO" Value="Successfully Delivered" />
                        <asp:ListItem Text="Rejected (Cannot Pickup)" Value="Rejected" />
                        <asp:ListItem Text="Failed Delivery" Value="Failed" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label class="form-label">Collection Remarks (Food Packaging &amp; Pickup Notes)</label>
                    <asp:TextBox ID="txtCollectionRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="e.g., Collected 15 Kg in hot insulated containers at 3:15 PM" />
                </div>

                <div class="form-group">
                    <label class="form-label">NGO Receiver Person Name (For Delivery Confirmation)</label>
                    <asp:TextBox ID="txtReceivedBy" runat="server" CssClass="form-control" placeholder="Name of NGO representative receiving food" />
                </div>

                <div class="form-group">
                    <label class="form-label">Food Condition at Delivery</label>
                    <asp:DropDownList ID="ddlCondition" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Fresh, Hygienic &amp; Ready to Serve" Value="Fresh &amp; Hygienic" />
                        <asp:ListItem Text="Warm &amp; Sealed Properly" Value="Warm &amp; Sealed" />
                        <asp:ListItem Text="Dry Ration / Unopened Packs" Value="Intact Dry Packs" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label class="form-label">Delivery Confirmation Remarks</label>
                    <asp:TextBox ID="txtDeliveryRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="e.g., Handed over to shelter coordinator; beneficiaries being served." />
                </div>

                <asp:Button ID="btnUpdateStatus" runat="server" Text="Save Status Update &amp; Confirm" CssClass="btn btn-primary btn-lg" />
            </div>
        </div>

        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128205; Selected Task Route &amp; Contact Sheet</span>
            </div>
            <div class="card-body">
                <asp:Literal ID="litTaskSummary" runat="server" Text="Select a task to view donor pickup and NGO receiver addresses." />
            </div>
        </div>
    </div>
</asp:Content>
