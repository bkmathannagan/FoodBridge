<%@ Page Title="FoodBridge – Save Food. Share Hope. Feed Lives." Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="FoodBridge._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="FoodBridge connects food donors, hotels, marriage halls, and supermarkets with NGOs and volunteers to rescue surplus food and distribute it to people in need." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Banner -->
    <section class="hero-banner">
        <div class="container hero-grid">
            <div>
                <span class="hero-tag">&#127793; National Food Rescue &amp; Donation Network</span>
                <h1 class="hero-title">Bridging Surplus Food with Hungry Hearts in Real Time</h1>
                <p class="hero-slogan">"Save Food. Share Hope. Feed Lives."</p>
                <p class="hero-desc">
                    Connect restaurants, hotels, marriage halls, supermarkets, catering services, and individual donors directly with verified NGOs and rapid-response volunteers to eliminate food wastage.
                </p>
                <div class="hero-buttons">
                    <a href="<%= ResolveUrl("~/Account/RegisterDonor.aspx") %>" class="btn btn-accent btn-lg">&#127858; Donate Food Now</a>
                    <a href="<%= ResolveUrl("~/NGO/AvailableFood.aspx") %>" class="btn btn-outline btn-lg" style="background: rgba(255,255,255,0.12); color: #FFFFFF; border-color: #FFFFFF;">&#128269; Find Available Food</a>
                    <a href="<%= ResolveUrl("~/Account/RegisterVolunteer.aspx") %>" class="btn btn-outline btn-lg" style="border-color: #FDE68A; color: #FDE68A;">&#128666; Become a Volunteer</a>
                </div>
            </div>

            <div class="hero-card">
                <h3 style="font-size: 20px; margin-bottom: 14px; color: #FDE68A;">Live Food Rescue Impact</h3>
                <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 14px;">
                    <div style="background: rgba(0,0,0,0.22); padding: 14px; border-radius: 10px;">
                        <div style="font-size: 26px; font-weight: 800;"><asp:Literal ID="litStatFoodKg" runat="server" Text="0" /> Kg</div>
                        <div style="font-size: 12px; color: #CBD5E1;">Total Food Rescued</div>
                    </div>
                    <div style="background: rgba(0,0,0,0.22); padding: 14px; border-radius: 10px;">
                        <div style="font-size: 26px; font-weight: 800;"><asp:Literal ID="litStatPeopleFed" runat="server" Text="0" />+</div>
                        <div style="font-size: 12px; color: #CBD5E1;">Beneficiaries Served</div>
                    </div>
                    <div style="background: rgba(0,0,0,0.22); padding: 14px; border-radius: 10px;">
                        <div style="font-size: 26px; font-weight: 800;"><asp:Literal ID="litStatDonors" runat="server" Text="0" /></div>
                        <div style="font-size: 12px; color: #CBD5E1;">Registered Donors</div>
                    </div>
                    <div style="background: rgba(0,0,0,0.22); padding: 14px; border-radius: 10px;">
                        <div style="font-size: 26px; font-weight: 800;"><asp:Literal ID="litStatNGOs" runat="server" Text="0" /></div>
                        <div style="font-size: 12px; color: #CBD5E1;">Partner NGOs &amp; Shelters</div>
                    </div>
                </div>
                <div style="margin-top: 16px; font-size: 13px; color: #E2E8F0; border-top: 1px solid rgba(255,255,255,0.2); padding-top: 12px;">
                    &#9889; Active Volunteers Ready: <strong><asp:Literal ID="litStatVolunteers" runat="server" Text="0" /></strong> | Completed Deliveries: <strong><asp:Literal ID="litStatDeliveries" runat="server" Text="0" /></strong>
                </div>
            </div>
        </div>
    </section>

    <!-- Announcements Strip -->
    <div class="container" style="margin-top: 26px;">
        <asp:Repeater ID="rptAnnouncements" runat="server">
            <ItemTemplate>
                <div class="alert alert-info" style="display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 10px;">
                    <div>
                        <span class="badge badge-info" style="margin-right: 8px;">ANNOUNCEMENT</span>
                        <strong><%# Eval("Title") %>:</strong> <%# Eval("Content") %>
                    </div>
                    <span style="font-size: 12px; color: #0369A1;"><%# Convert.ToDateTime(Eval("PublishDate")).ToString("dd MMM yyyy") %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Recent Available Food Donations Section -->
    <section class="section-padding">
        <div class="container">
            <div class="section-header">
                <h2 class="section-title">Recent Available Surplus Food Donations</h2>
                <p style="color: var(--text-muted);">Verified, fresh, and non-expired food donations ready for immediate NGO request and volunteer collection.</p>
            </div>

            <div class="card">
                <div class="card-header">
                    <span class="card-title">&#127858; Live Approved Food Inventory</span>
                    <a href="<%= ResolveUrl("~/NGO/AvailableFood.aspx") %>" class="btn btn-primary btn-sm">View &amp; Filter All Donations &rarr;</a>
                </div>
                <div class="card-body table-responsive">
                    <asp:GridView ID="gvRecentAvailable" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No available donations right now. Donors can log in to register surplus food!">
                        <Columns>
                            <asp:TemplateField HeaderText="Food Item">
                                <ItemTemplate>
                                    <div style="display: flex; align-items: center; gap: 10px;">
                                        <img src="<%# ResolveUrl(Convert.ToString(Eval("FoodImage"))) %>" alt="<%# Eval("FoodName") %>" style="width: 54px; height: 38px; border-radius: 6px; object-fit: cover;" />
                                        <div>
                                            <strong><%# Eval("FoodName") %></strong>
                                            <div style="font-size: 12px; color: var(--text-muted);"><%# Eval("FoodCategory") %> &bull; <%# Eval("FoodType") %></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DonorName" HeaderText="Donor / Organization" />
                            <asp:TemplateField HeaderText="Available Qty">
                                <ItemTemplate>
                                    <strong><%# Eval("AvailableQuantity") %> <%# Eval("QuantityUnit") %></strong>
                                    <div style="font-size: 11.5px; color: var(--text-muted);">Serves ~<%# Eval("NumberOfPersonsServed") %> people</div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="City" HeaderText="Location" />
                            <asp:TemplateField HeaderText="Expiry">
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
    </section>

    <!-- How It Works Section -->
    <section class="section-padding" style="background-color: #FFFFFF; border-top: 1px solid var(--border-color); border-bottom: 1px solid var(--border-color);">
        <div class="container">
            <div class="section-header">
                <h2 class="section-title">How FoodBridge Rescue Workflow Works</h2>
                <p style="color: var(--text-muted);">A transparent, six-step food rescue chain with strict expiry verification, quantity allocation, and real-time delivery tracking.</p>
            </div>

            <div class="feature-grid">
                <div class="feature-card">
                    <span class="badge badge-success" style="margin-bottom: 10px;">STEP 1</span>
                    <h3 style="margin-bottom: 8px; font-size: 18px;">1. Donor Registers Surplus Food</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Individuals, hotels, marriage halls, and supermarkets submit surplus food details including preparation time, expiry time, quantity, and pickup address.</p>
                </div>
                <div class="feature-card">
                    <span class="badge badge-info" style="margin-bottom: 10px;">STEP 2</span>
                    <h3 style="margin-bottom: 8px; font-size: 18px;">2. Admin Verifies &amp; Approves</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Central administrators verify food safety, expiry timestamps, and donor authenticity before publishing the donation to the live inventory.</p>
                </div>
                <div class="feature-card">
                    <span class="badge badge-warning" style="margin-bottom: 10px;">STEP 3</span>
                    <h3 style="margin-bottom: 8px; font-size: 18px;">3. Verified NGO Requests Food</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Charitable trusts and shelter homes search available donations by city and category and request required quantities for their beneficiaries.</p>
                </div>
                <div class="feature-card">
                    <span class="badge badge-success" style="margin-bottom: 10px;">STEP 4</span>
                    <h3 style="margin-bottom: 8px; font-size: 18px;">4. Volunteer Dispatch &amp; Delivery</h3>
                    <p style="color: var(--text-muted); font-size: 14px;">Admin approves the request and assigns an available rescue volunteer who collects the food from the donor, delivers it to the NGO, and confirms receipt.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- About FoodBridge & Role Registration CTA -->
    <section class="section-padding">
        <div class="container">
            <div class="feature-grid">
                <div class="feature-card" style="border-top: 4px solid var(--primary);">
                    <h3 style="font-size: 20px; margin-bottom: 10px;">&#127970; For Food Donors</h3>
                    <p style="color: var(--text-muted); margin-bottom: 18px;">Hotels, restaurants, wedding halls, caterers, and households can turn surplus food into life-saving meals in under 2 minutes.</p>
                    <a href="<%= ResolveUrl("~/Account/RegisterDonor.aspx") %>" class="btn btn-primary">Register as Donor</a>
                </div>
                <div class="feature-card" style="border-top: 4px solid var(--info);">
                    <h3 style="font-size: 20px; margin-bottom: 10px;">&#129309; For NGOs &amp; Shelters</h3>
                    <p style="color: var(--text-muted); margin-bottom: 18px;">Verified charitable organizations, orphanages, and community kitchens can request nutritious surplus food free of cost.</p>
                    <a href="<%= ResolveUrl("~/Account/RegisterNGO.aspx") %>" class="btn btn-primary">Register Organization</a>
                </div>
                <div class="feature-card" style="border-top: 4px solid var(--accent);">
                    <h3 style="font-size: 20px; margin-bottom: 10px;">&#128666; For Rescue Volunteers</h3>
                    <p style="color: var(--text-muted); margin-bottom: 18px;">Join our city-wide volunteer fleet to pick up food parcels from donors and deliver them directly to people in need.</p>
                    <a href="<%= ResolveUrl("~/Account/RegisterVolunteer.aspx") %>" class="btn btn-accent">Join as Volunteer</a>
                </div>
            </div>
        </div>
    </section>

    <!-- Testimonials Section -->
    <section class="section-padding" style="background-color: #FFFFFF; border-top: 1px solid var(--border-color);">
        <div class="container">
            <div class="section-header">
                <h2 class="section-title">Community Voices &amp; Testimonials</h2>
                <p style="color: var(--text-muted);">Hear from the hotels, NGOs, and volunteers making zero hunger a reality through FoodBridge.</p>
            </div>
            <div class="feature-grid">
                <div class="feature-card">
                    <p style="font-style: italic; color: var(--text-main); margin-bottom: 14px;">"Whenever our banquet hall has untouched buffet surplus after corporate events, FoodBridge dispatches a volunteer within 30 minutes. Zero waste, maximum dignity."</p>
                    <strong>— Vikram Malhotra</strong>
                    <div style="font-size: 12.5px; color: var(--primary);">Grand Royal Palace Hotel, Bengaluru</div>
                </div>
                <div class="feature-card">
                    <p style="font-style: italic; color: var(--text-main); margin-bottom: 14px;">"FoodBridge's real-time quantity tracking and expiry checks give us complete confidence in the freshness of meals we serve to our 250 shelter residents."</p>
                    <strong>— Sister Maria Thomas</strong>
                    <div style="font-size: 12.5px; color: var(--primary);">Asha Deepa Charitable Trust</div>
                </div>
                <div class="feature-card">
                    <p style="font-style: italic; color: var(--text-main); margin-bottom: 14px;">"The volunteer dashboard clearly shows donor pickup addresses, contact numbers, and NGO receiver locations so I can complete rescues on my way home."</p>
                    <strong>— Arjun Reddy</strong>
                    <div style="font-size: 12.5px; color: var(--primary);">Lead Rescue Volunteer</div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
