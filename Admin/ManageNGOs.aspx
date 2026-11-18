<%@ Page Title="Manage NGOs – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageNGOs.aspx.vb" Inherits="FoodBridge.ManageNGOs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage NGOs &amp; Charitable Organizations</h1>
            <p class="page-subtitle">Verify NGO registration numbers, approve charitable organizations to request food, or block accounts.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="filter-bar">
        <div class="filter-item">
            <label class="form-label">Search NGO / Reg No / Contact</label>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search keyword..." />
        </div>
        <div class="filter-item">
            <label class="form-label">City</label>
            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" />
        </div>
        <div class="filter-item">
            <label class="form-label">Verification Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Statuses" Value="All" />
                <asp:ListItem Text="Verified" Value="Verified" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
                <asp:ListItem Text="Blocked" Value="Blocked" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="Filter NGOs" CssClass="btn btn-primary" />
        </div>
    </div>

    <div class="card">
        <div class="card-body table-responsive">
            <asp:GridView ID="gvNGOs" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None" EmptyDataText="No NGOs found.">
                <Columns>
                    <asp:BoundField DataField="NGOID" HeaderText="ID #" />
                    <asp:TemplateField HeaderText="Organization &amp; Reg No">
                        <ItemTemplate>
                            <strong><%# Eval("OrganizationName") %></strong>
                            <div style="font-size: 12px; color: var(--text-muted);">Reg: <%# Eval("RegistrationNumber") %> &bull; Contact: <%# Eval("ContactPerson") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="NumberOfBeneficiaries" HeaderText="Beneficiaries" />
                    <asp:TemplateField HeaderText="Verification Status">
                        <ItemTemplate><span class="badge badge-<%# Eval("VerificationStatus") %>"><%# Eval("VerificationStatus") %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div style="display: flex; gap: 6px; flex-wrap: wrap;">
                                <asp:LinkButton ID="btnVerify" runat="server" CommandName="VerifyNGO" CommandArgument='<%# Eval("NGOID") %>' CssClass="btn btn-primary btn-sm" Text="Verify" Visible='<%# Convert.ToString(Eval("VerificationStatus")) <> "Verified" %>' />
                                <asp:LinkButton ID="btnReject" runat="server" CommandName="RejectNGO" CommandArgument='<%# Eval("NGOID") %>' CssClass="btn btn-danger btn-sm" Text="Block / Reject" Visible='<%# Convert.ToString(Eval("VerificationStatus")) = "Verified" OrElse Convert.ToString(Eval("VerificationStatus")) = "Pending" %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
