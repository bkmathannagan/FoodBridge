<%@ Page Title="Submit Surplus Food Donation – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="AddDonation.aspx.vb" Inherits="FoodBridge.AddDonation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title"><asp:Literal ID="litFormTitle" runat="server" Text="Submit New Surplus Food Donation" /></h1>
            <p class="page-subtitle">Please ensure food is hygienic, fresh, and not expired. All fields marked * are mandatory.</p>
        </div>
        <a href="<%= ResolveUrl("~/Donor/MyDonations.aspx") %>" class="btn btn-outline btn-sm">&larr; Back to My Donations</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="card">
        <div class="card-header">
            <span class="card-title">&#127858; Food Donation Registration Form</span>
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfEditDonationId" runat="server" Value="0" />
            <asp:HiddenField ID="hfExistingImage" runat="server" Value="~/Content/images/food-meals.svg" />

            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Food Name / Dish Title *</label>
                    <asp:TextBox ID="txtFoodName" runat="server" CssClass="form-control" placeholder="e.g., Fresh Veg Biryani, Chapatis &amp; Dal" />
                </div>
                <div class="form-group">
                    <label class="form-label">Food Category *</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Food Type (Veg / Non-Veg) *</label>
                    <asp:DropDownList ID="ddlFoodType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Vegetarian (Veg)" Value="Veg" />
                        <asp:ListItem Text="Non-Vegetarian (Non-Veg)" Value="Non-Veg" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="form-label">Food Quantity *</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text="15" />
                </div>
                <div class="form-group">
                    <label class="form-label">Quantity Unit *</label>
                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Kilograms (Kg)" Value="Kg" />
                        <asp:ListItem Text="Packets" Value="Packets" />
                        <asp:ListItem Text="Plates" Value="Plates" />
                        <asp:ListItem Text="Litres" Value="Litres" />
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="form-label">Estimated Number of Persons Served *</label>
                    <asp:TextBox ID="txtPersonsServed" runat="server" CssClass="form-control" Text="50" />
                </div>
                <div class="form-group">
                    <label class="form-label">Food Preparation Date (YYYY-MM-DD) *</label>
                    <asp:TextBox ID="txtPrepDate" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Food Preparation Time (HH:MM) *</label>
                    <asp:TextBox ID="txtPrepTime" runat="server" CssClass="form-control" Text="11:30" />
                </div>
                <div class="form-group">
                    <label class="form-label">Expiry Date (YYYY-MM-DD) *</label>
                    <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Expiry Time (HH:MM) *</label>
                    <asp:TextBox ID="txtExpiryTime" runat="server" CssClass="form-control" Text="21:00" />
                </div>
                <div class="form-group">
                    <label class="form-label">Preferred Pickup Date (YYYY-MM-DD) *</label>
                    <asp:TextBox ID="txtPickupDate" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Preferred Pickup Time (HH:MM) *</label>
                    <asp:TextBox ID="txtPickupTime" runat="server" CssClass="form-control" Text="15:00" />
                </div>
                <div class="form-group">
                    <label class="form-label">City *</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">PIN Code *</label>
                    <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Text="560001" />
                </div>
                <div class="form-group">
                    <label class="form-label">Food Photo Upload (JPG, PNG, GIF, SVG - Max 5MB)</label>
                    <asp:FileUpload ID="fuFoodImage" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div class="form-group">
                <label class="form-label">Food Description &amp; Ingredients *</label>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Describe the food items, packaging condition, and storage temperature" />
            </div>

            <div class="form-group">
                <label class="form-label">Complete Pickup Address *</label>
                <asp:TextBox ID="txtPickupAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Building name, floor, landmark, gate number" />
            </div>

            <div class="form-group">
                <label class="form-label">Special Instructions for Volunteer</label>
                <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="e.g., Bring insulated containers, contact kitchen manager at gate 2" />
            </div>

            <asp:Button ID="btnSaveDonation" runat="server" Text="Submit Food Donation" CssClass="btn btn-primary btn-lg" />
        </div>
    </div>
</asp:Content>
