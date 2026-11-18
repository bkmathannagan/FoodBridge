<%@ Page Title="Manage Food Categories & Cities – FoodBridge Admin" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="ManageCategories.aspx.vb" Inherits="FoodBridge.ManageCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <div>
            <h1 class="page-title">Manage Food Categories &amp; Service Cities</h1>
            <p class="page-subtitle">Add, update, or remove food categories and operating locations.</p>
        </div>
    </div>

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-success">
        <asp:Literal ID="litAlert" runat="server" />
    </asp:Panel>

    <div class="form-grid">
        <!-- Food Categories Management -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#127858; Food Categories</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">Category Name *</label>
                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="e.g., Baby Food / Beverages" />
                </div>
                <div class="form-group">
                    <label class="form-label">Description</label>
                    <asp:TextBox ID="txtCategoryDesc" runat="server" CssClass="form-control" placeholder="Description of items in this category" />
                </div>
                <asp:Button ID="btnAddCategory" runat="server" Text="Add Food Category" CssClass="btn btn-primary btn-sm" />

                <div class="table-responsive" style="margin-top: 18px;">
                    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="CategoryID" HeaderText="ID" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelCat" runat="server" CommandName="DeleteCat" CommandArgument='<%# Eval("CategoryID") %>' CssClass="btn btn-danger btn-sm" Text="Delete" OnClientClick="return confirm('Delete this category?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Cities / Locations Management -->
        <div class="card">
            <div class="card-header">
                <span class="card-title">&#127961; Operating Cities &amp; Locations</span>
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label class="form-label">City Name *</label>
                    <asp:TextBox ID="txtCityName" runat="server" CssClass="form-control" placeholder="e.g., Mysuru" />
                </div>
                <div class="form-group">
                    <label class="form-label">State Name *</label>
                    <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" placeholder="e.g., Karnataka" />
                </div>
                <div class="form-group">
                    <label class="form-label">PIN Prefix</label>
                    <asp:TextBox ID="txtPinPrefix" runat="server" CssClass="form-control" placeholder="e.g., 570" />
                </div>
                <asp:Button ID="btnAddCity" runat="server" Text="Add Service City" CssClass="btn btn-primary btn-sm" />

                <div class="table-responsive" style="margin-top: 18px;">
                    <asp:GridView ID="gvCities" runat="server" AutoGenerateColumns="False" CssClass="data-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="CityID" HeaderText="ID" />
                            <asp:BoundField DataField="CityName" HeaderText="City" />
                            <asp:BoundField DataField="StateName" HeaderText="State" />
                            <asp:BoundField DataField="PINPrefix" HeaderText="PIN Prefix" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelCity" runat="server" CommandName="DeleteCity" CommandArgument='<%# Eval("CityID") %>' CssClass="btn btn-danger btn-sm" Text="Delete" OnClientClick="return confirm('Delete this city?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
