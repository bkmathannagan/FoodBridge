<%@ Page Title="Donation Tracking & Details – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="DonationDetails.aspx.vb" Inherits="FoodBridge.DonationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Food Donation #<asp:Literal ID="litDonationId" runat="server" /> Tracking &amp; Details</h1>
            <p class="page-subtitle">Real-time rescue status, volunteer assignment, and transaction audit timeline.</p>
        </div>
        <a href="<%= ResolveUrl("~/Donor/MyDonations.aspx") %>" class="btn btn-outline btn-sm">&larr; Back to My Donations</a>
    </div>

    <div class="form-grid">
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#127858; Donation Information</span>
                <asp:Literal ID="litStatusBadge" runat="server" />
            </div>
            <div class="card-body">
                <p style="margin-bottom: 8px;"><strong>Food Name:</strong> <asp:Literal ID="litFoodName" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Category &amp; Type:</strong> <asp:Literal ID="litCategory" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Total Quantity:</strong> <asp:Literal ID="litQuantity" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Remaining Available Quantity:</strong> <asp:Literal ID="litAvailableQty" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Preparation Date &amp; Time:</strong> <asp:Literal ID="litPrep" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Expiry Date &amp; Time:</strong> <asp:Literal ID="litExpiry" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Pickup Location:</strong> <asp:Literal ID="litAddress" runat="server" /></p>
                <p style="margin-bottom: 8px;"><strong>Collection Status:</strong> <asp:Literal ID="litCollectionStatus" runat="server" /></p>
                <p style="margin-bottom: 0;"><strong>Admin Remarks:</strong> <asp:Literal ID="litAdminRemarks" runat="server" /></p>
            </div>
        </div>

        <div class="card">
            <div class="card-header">
                <span class="card-title">&#128338; Donation Tracking Timeline</span>
            </div>
            <div class="card-body">
                <div class="timeline">
                    <asp:Repeater ID="rptTimeline" runat="server">
                        <ItemTemplate>
                            <div class="timeline-item">
                                <div style="display: flex; justify-content: space-between;">
                                    <strong><%# Eval("StatusTo") %></strong>
                                    <span style="font-size: 12px; color: var(--text-muted);"><%# Convert.ToDateTime(Eval("TransactionDate")).ToString("dd MMM yyyy HH:mm") %></span>
                                </div>
                                <div style="font-size: 13px; color: var(--text-muted);"><%# Eval("Remarks") %></div>
                                <div style="font-size: 12px; color: var(--primary);">By: <%# Eval("ActionByName") %> (<%# Eval("ActionByRole") %>)</div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
