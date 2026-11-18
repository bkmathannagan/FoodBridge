<%@ Page Title="About Us – FoodBridge Food Rescue System" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="About.aspx.vb" Inherits="FoodBridge.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Learn about FoodBridge – Food Rescue and Donation Management System, our mission to reduce food wastage, and our four-role coordination ecosystem." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container">
            <div class="section-header">
                <span class="badge badge-success">ABOUT FOODBRIDGE</span>
                <h1 class="section-title" style="margin-top: 8px;">Eliminating Hunger by Rescuing Surplus Food</h1>
                <p style="color: var(--text-muted);">"Save Food. Share Hope. Feed Lives."</p>
            </div>

            <div class="card">
                <div class="card-body">
                    <h2 style="font-size: 22px; margin-bottom: 12px; color: var(--primary);">Our Mission &amp; Vision</h2>
                    <p style="margin-bottom: 16px;">
                        Every day, tonnes of wholesome, freshly prepared food go unsold or unconsumed at marriage halls, hotels, restaurants, catering events, and supermarkets. At the same time, charitable shelters, orphanages, and underprivileged communities struggle for nutritious meals.
                    </p>
                    <p style="margin-bottom: 16px;">
                        <strong>FoodBridge – Food Rescue and Donation Management System</strong> bridges this critical gap through a centralized, three-tier web platform that coordinates <strong>Food Donors</strong>, <strong>Administrators</strong>, <strong>Verified NGOs</strong>, and <strong>Volunteers</strong> with strict expiry validation, accurate quantity reservation, and end-to-end delivery confirmation.
                    </p>

                    <div class="stats-grid" style="margin-top: 28px;">
                        <div class="stat-card">
                            <div class="stat-label">Registered Donors</div>
                            <div class="stat-value"><asp:Literal ID="litDonors" runat="server" /></div>
                            <div class="stat-meta">Hotels, Halls, Supermarkets &amp; Citizens</div>
                        </div>
                        <div class="stat-card info">
                            <div class="stat-label">Verified NGOs</div>
                            <div class="stat-value"><asp:Literal ID="litNGOs" runat="server" /></div>
                            <div class="stat-meta">Charitable Trusts &amp; Shelters</div>
                        </div>
                        <div class="stat-card accent">
                            <div class="stat-label">Active Volunteers</div>
                            <div class="stat-value"><asp:Literal ID="litVolunteers" runat="server" /></div>
                            <div class="stat-meta">Field Collection &amp; Delivery Fleet</div>
                        </div>
                        <div class="stat-card">
                            <div class="stat-label">Food Rescued (Kg)</div>
                            <div class="stat-value"><asp:Literal ID="litFoodKg" runat="server" /></div>
                            <div class="stat-meta">Safe &amp; Hygienic Distribution</div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="feature-grid">
                <div class="feature-card">
                    <h3 style="color: var(--primary); margin-bottom: 8px;">Strict Expiry Validation</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Every donation is checked against preparation and expiry timestamps. Expired items are automatically excluded from available listings.</p>
                </div>
                <div class="feature-card">
                    <h3 style="color: var(--primary); margin-bottom: 8px;">Smart Quantity Allocation</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Partial and full NGO requests dynamically deduct from available stock using ACID database transactions, preventing over-allocation.</p>
                </div>
                <div class="feature-card">
                    <h3 style="color: var(--primary); margin-bottom: 8px;">Complete Audit Trail</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Every status transition—from Donor Registration to Volunteer Collection and NGO Delivery—is logged in the DonationTransactions ledger.</p>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
