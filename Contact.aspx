<%@ Page Title="Contact Us & Feedback – FoodBridge" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Contact.aspx.vb" Inherits="FoodBridge.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="description" content="Contact the FoodBridge coordination team or submit feedback and complaints regarding food donations and deliveries." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="section-padding">
        <div class="container">
            <div class="section-header">
                <span class="badge badge-success">GET IN TOUCH</span>
                <h1 class="section-title" style="margin-top: 8px;">Contact FoodBridge &amp; Submit Feedback</h1>
                <p style="color: var(--text-muted);">Have a question, urgent large banquet surplus, or feedback? Reach out to our 24x7 coordination desk.</p>
            </div>

            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-success">
                <asp:Literal ID="litMessage" runat="server" />
            </asp:Panel>

            <div class="form-grid">
                <div class="card">
                    <div class="card-header">
                        <span class="card-title">&#128172; Send Feedback or Inquiry</span>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label class="form-label">Your Name / Organization *</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Email Address *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@example.com" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Role Category</label>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Food Donor" Value="Donor" />
                                <asp:ListItem Text="NGO / Receiver" Value="NGO" />
                                <asp:ListItem Text="Volunteer" Value="Volunteer" />
                                <asp:ListItem Text="Public Visitor" Value="Public" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Rating</label>
                            <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-control">
                                <asp:ListItem Text="5 Stars - Excellent" Value="5" />
                                <asp:ListItem Text="4 Stars - Very Good" Value="4" />
                                <asp:ListItem Text="3 Stars - Average" Value="3" />
                                <asp:ListItem Text="2 Stars - Needs Improvement" Value="2" />
                                <asp:ListItem Text="1 Star - Urgent Issue" Value="1" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Subject *</label>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Brief subject of your message" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Message / Feedback Details *</label>
                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Write your feedback or inquiry here..." />
                        </div>
                        <asp:Button ID="btnSubmitFeedback" runat="server" Text="Submit Feedback" CssClass="btn btn-primary" />
                    </div>
                </div>

                <div>
                    <div class="card">
                        <div class="card-header">
                            <span class="card-title">&#127970; Central Food Rescue Hubs</span>
                        </div>
                        <div class="card-body">
                            <p style="margin-bottom: 14px;"><strong>National Headquarters:</strong><br />#42 Green Tech Park, MG Road, Bengaluru - 560001, Karnataka</p>
                            <p style="margin-bottom: 14px;"><strong>24x7 Helpline:</strong><br />+91 800-123-FOOD (3663) / +91 98765 43210</p>
                            <p style="margin-bottom: 14px;"><strong>Official Email:</strong><br />support@foodbridge.com | admin@foodbridge.com</p>
                            <p style="margin-bottom: 0;"><strong>Operating Hours:</strong><br />24 Hours / 7 Days a Week (Emergency Banquet &amp; Event Pickups)</p>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header">
                            <span class="card-title">&#127775; Recent Community Feedback</span>
                        </div>
                        <div class="card-body">
                            <asp:Repeater ID="rptFeedback" runat="server">
                                <ItemTemplate>
                                    <div style="border-bottom: 1px solid var(--border-color); padding-bottom: 10px; margin-bottom: 10px;">
                                        <div style="display: flex; justify-content: space-between;">
                                            <strong><%# Eval("Subject") %></strong>
                                            <span class="badge badge-success"><%# Eval("Rating") %> / 5 &#9733;</span>
                                        </div>
                                        <p style="font-size: 13.5px; color: var(--text-muted); margin: 4px 0;"><%# Eval("Message") %></p>
                                        <div style="font-size: 12px; color: var(--primary);">— <%# Eval("UserName") %> (<%# Eval("UserRole") %>)</div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
