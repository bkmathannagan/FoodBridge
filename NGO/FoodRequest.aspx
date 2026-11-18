<%@ Page Title="Submit Food Request – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="FoodRequest.aspx.vb" Inherits="FoodBridge.SubmitFoodRequestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Submit Surplus Food Request</h1>
            <p class="page-subtitle">Specify the required food quantity and number of beneficiaries for your shelter or charitable distribution.</p>
        </div>
        <a href="<%= ResolveUrl("~/NGO/AvailableFood.aspx") %>" class="btn btn-outline btn-sm">&larr; Back to Available Food</a>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="card">
        <div class="card-header">
            <span class="card-title">&#128221; NGO Food Request Form</span>
        </div>
        <div class="card-body">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Select Available Food Donation *</label>
                    <asp:DropDownList ID="ddlDonation" runat="server" CssClass="form-control" AutoPostBack="True" />
                </div>
                <div class="form-group">
                    <label class="form-label">Available Stock &amp; Expiry Details</label>
                    <asp:TextBox ID="txtDonationSummary" runat="server" CssClass="form-control" ReadOnly="True" />
                </div>
                <div class="form-group">
                    <label class="form-label">Requested Food Quantity *</label>
                    <asp:TextBox ID="txtRequestedQty" runat="server" CssClass="form-control" Text="10" />
                </div>
                <div class="form-group">
                    <label class="form-label">Number of Beneficiaries to be Fed *</label>
                    <asp:TextBox ID="txtBeneficiaries" runat="server" CssClass="form-control" Text="50" />
                </div>
                <div class="form-group">
                    <label class="form-label">Required Date (YYYY-MM-DD) *</label>
                    <asp:TextBox ID="txtRequiredDate" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">City *</label>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Receiver Contact Person *</label>
                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label class="form-label">Receiver Contact Mobile Number *</label>
                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div class="form-group">
                <label class="form-label">NGO Delivery Address *</label>
                <asp:TextBox ID="txtDeliveryAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label class="form-label">Request Remarks / Distribution Notes</label>
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="e.g., Needed for evening meal distribution at children's shelter" />
            </div>

            <asp:Button ID="btnSubmitRequest" runat="server" Text="Submit Food Request" CssClass="btn btn-primary btn-lg" />
        </div>
    </div>
</asp:Content>
