<%@ Page Title="How It Works – FoodBridge Workflow" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="HowItWorks.aspx.vb" Inherits="FoodBridge.HowItWorks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Understand how the FoodBridge 6-stage food rescue workflow connects donors, administrators, NGOs, and volunteers." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container">
            <div class="section-header">
                <span class="badge badge-info">END-TO-END WORKFLOW</span>
                <h1 class="section-title" style="margin-top: 8px;">How the FoodBridge Rescue Process Works</h1>
                <p style="color: var(--text-muted);">From surplus kitchen preparation to verified NGO delivery in 6 coordinated stages.</p>
            </div>

            <div class="card">
                <div class="card-body">
                    <div class="timeline">
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 1: Food Donor Registers Surplus Food</h3>
                            <p style="color: var(--text-muted);">Registered individuals, hotels, marriage halls, catering services, and supermarkets log surplus food items with category (Cooked Food, Raw Vegetables, Fruits, Bakery Products, Packed Food, Groceries, Dairy Products, Other), Veg/Non-Veg type, quantity, preparation time, expiry time, and pickup address.</p>
                        </div>
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 2: Administrator Verification &amp; Approval</h3>
                            <p style="color: var(--text-muted);">The Administrator reviews the donation, validates expiry timelines, and approves safe donations so they immediately appear on the Available Food board with an Urgency indicator.</p>
                        </div>
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 3: Verified NGO Submits Food Request</h3>
                            <p style="color: var(--text-muted);">Verified NGOs search donations by City and Category and submit a request specifying required quantity and number of beneficiaries. Multiple NGOs can request portions of a large donation as long as quantity remains available.</p>
                        </div>
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 4: Request Approval &amp; Volunteer Assignment</h3>
                            <p style="color: var(--text-muted);">The Administrator approves the NGO's request and assigns an available Volunteer from the same city, generating a tracked collection and delivery task.</p>
                        </div>
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 5: Volunteer Accepts &amp; Collects Food</h3>
                            <p style="color: var(--text-muted);">The assigned Volunteer views donor contact details and pickup address, accepts the task, inspects and collects the food parcel, and updates the status to <em>Food Collected</em> and <em>Out for Delivery</em>.</p>
                        </div>
                        <div class="timeline-item">
                            <h3 style="color: var(--primary);">Stage 6: Delivery Confirmation &amp; Impact Recording</h3>
                            <p style="color: var(--text-muted);">Upon handing over the food to the NGO contact person, the Volunteer confirms <em>Successfully Delivered</em>, recording the delivery in the ledger and updating rescued food statistics.</p>
                        </div>
                    </div>
                </div>
            </div>

            <div style="text-align: center; margin-top: 28px;">
                <a href="<%= ResolveUrl("~/Account/RegisterDonor.aspx") %>" class="btn btn-primary btn-lg">Start Donating Food</a>
                <a href="<%= ResolveUrl("~/Account/Login.aspx") %>" class="btn btn-outline btn-lg" style="margin-left: 12px;">Login to Portal</a>
            </div>
        </div>
    </section>
</asp:Content>
