Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageCategories
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtCategoryName As TextBox
    Protected WithEvents txtCategoryDesc As TextBox
    Protected WithEvents btnAddCategory As Button
    Protected WithEvents gvCategories As GridView
    Protected WithEvents txtCityName As TextBox
    Protected WithEvents txtStateName As TextBox
    Protected WithEvents txtPinPrefix As TextBox
    Protected WithEvents btnAddCity As Button
    Protected WithEvents gvCities As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim donBll As New DonationBLL()
        gvCategories.DataSource = donBll.GetCategories(False)
        gvCategories.DataBind()

        gvCities.DataSource = donBll.GetCities(False)
        gvCities.DataBind()
    End Sub

    Protected Sub btnAddCategory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddCategory.Click
        If String.IsNullOrEmpty(txtCategoryName.Text.Trim()) Then Return
        Dim donBll As New DonationBLL()
        Try
            donBll.SaveCategory(0, txtCategoryName.Text, txtCategoryDesc.Text, True)
            txtCategoryName.Text = ""
            txtCategoryDesc.Text = ""
            ShowAlert("Food category added successfully.")
            BindData()
        Catch ex As Exception
            ShowAlert("Category already exists or invalid input.")
        End Try
    End Sub

    Protected Sub gvCategories_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCategories.RowCommand
        If e.CommandName = "DeleteCat" Then
            Dim donBll As New DonationBLL()
            donBll.DeleteCategory(Convert.ToInt32(e.CommandArgument))
            ShowAlert("Food category removed.")
            BindData()
        End If
    End Sub

    Protected Sub btnAddCity_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddCity.Click
        If String.IsNullOrEmpty(txtCityName.Text.Trim()) OrElse String.IsNullOrEmpty(txtStateName.Text.Trim()) Then Return
        Dim donBll As New DonationBLL()
        Try
            donBll.SaveCity(0, txtCityName.Text, txtStateName.Text, txtPinPrefix.Text, True)
            txtCityName.Text = ""
            txtStateName.Text = ""
            txtPinPrefix.Text = ""
            ShowAlert("Service city added successfully.")
            BindData()
        Catch ex As Exception
            ShowAlert("City already exists or invalid input.")
        End Try
    End Sub

    Protected Sub gvCities_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCities.RowCommand
        If e.CommandName = "DeleteCity" Then
            Dim donBll As New DonationBLL()
            donBll.DeleteCity(Convert.ToInt32(e.CommandArgument))
            ShowAlert("City removed.")
            BindData()
        End If
    End Sub

    Private Sub ShowAlert(ByVal msg As String)
        pnlAlert.Visible = True
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
