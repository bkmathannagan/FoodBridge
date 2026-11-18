Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AvailableFood
    Inherits System.Web.UI.Page

    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCategory As DropDownList
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlFoodType As DropDownList
    Protected WithEvents btnFilter As Button
    Protected WithEvents gvAvailableFood As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCategory.DataSource = donBll.GetCategories(True)
            ddlCategory.DataTextField = "CategoryName"
            ddlCategory.DataValueField = "CategoryName"
            ddlCategory.DataBind()
            ddlCategory.Items.Insert(0, New ListItem("All Categories", "All"))

            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))

            BindAvailableDonations()
        End If
    End Sub

    Private Sub BindAvailableDonations()
        Dim donBll As New DonationBLL()
        gvAvailableFood.DataSource = donBll.GetAvailableDonationsForNGO(ddlCategory.SelectedValue, ddlCity.SelectedValue, ddlFoodType.SelectedValue, txtSearch.Text)
        gvAvailableFood.DataBind()
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        BindAvailableDonations()
    End Sub
End Class
