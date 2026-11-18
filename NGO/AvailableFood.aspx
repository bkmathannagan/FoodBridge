<%@ Page Title="Available Surplus Food Donations – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="AvailableFood.aspx.vb" Inherits="FoodBridge.AvailableFood" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Search and request available verified food donations by city and category on FoodBridge." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="padding-top: 24px; padding-bottom: 24px;">
        <div class="page-header">
            <div>
                <h1 class="page-title">Available Surplus Food Donations</h1>
                <p class="page-subtitle">Only administrator-approved, non-expired food donations with available quantity are displayed below.</p>
            </div>
        </div>

        <!-- Search & Filter Bar -->
        <div class="filter-bar">
            <div class="filter-item">
                <label class="form-label">Search Keyword</label>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Dish name, donor, locality..." />
            </div>
            <div class="filter-item">
                <label class="form-label">Food Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
            </div>
            <div class="filter-item">
                <label class="form-label">City / Location</label>
                <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
            </div>
            <div class="filter-item">
                <label class="form-label">Food Type</label>
                <asp:DropDownList ID="ddlFoodType" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All Types" Value="All" />
                    <asp:ListItem Text="Vegetarian (Veg)" Value="Veg" />
                    <asp:ListItem Text="Non-Vegetarian (Non-Veg)" Value="Non-Veg" />
                </asp:DropDownList>
            </div>
            <div>
                <asp:Button ID="btnFilter" runat="server" Text="Search Food" CssClass="btn btn-primary" />
            </div>
        </div>

        <div class="card">
            <div class="card-body table-responsive">
                <asp:GridView ID="gvAvailableFood" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No available food donations match your filter criteria right now.">
                    <Columns>
                        <asp:TemplateField HeaderText="Food Item">
                            <ItemTemplate>
                                <div style="display: flex; align-items: center; gap: 10px;">
                                    <img src="<%# ResolveUrl(Convert.ToString(Eval("FoodImage"))) %>" alt="<%# Eval("FoodName") %>" style="width: 56px; height: 40px; border-radius: 6px; object-fit: cover;" />
                                    <div>
                                        <strong><%# Eval("FoodName") %></strong>
                                        <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("FoodCategory") %> &bull; <%# Eval("FoodType") %></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Donor &amp; Pickup Address">
                            <ItemTemplate>
                                <strong><%# Eval("DonorName") %></strong> (<%# Eval("DonorType") %>)
                                <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("PickupAddress") %>, <%# Eval("City") %> - <%# Eval("PINCode") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remaining Available Qty">
                            <ItemTemplate>
                                <strong style="color: var(--primary); font-size: 15px;"><%# Eval("AvailableQuantity") %> <%# Eval("QuantityUnit") %></strong>
                                <div style="font-size: 11.5px; color: var(--text-muted);">of <%# Eval("FoodQuantity") %> <%# Eval("QuantityUnit") %> (~<%# Eval("NumberOfPersonsServed") %> people)</div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expiry &amp; Urgency">
                            <ItemTemplate>
                                <div><%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd MMM yyyy") %> (<%# Eval("ExpiryTime") %>)</div>
                                <span class="badge badge-<%# Eval("UrgencyTag") %>"><%# Eval("UrgencyTag") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href="<%# ResolveUrl("~/NGO/FoodRequest.aspx?donationId=" & Eval("DonationID")) %>" class="btn btn-primary btn-sm">Request Food</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
