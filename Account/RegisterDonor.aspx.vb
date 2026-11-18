Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class RegisterDonor
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtDonorName As TextBox
    Protected WithEvents ddlDonorType As DropDownList
    Protected WithEvents txtContactPerson As TextBox
    Protected WithEvents txtMobile As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents txtAddress As TextBox
    Protected WithEvents txtPassword As TextBox
    Protected WithEvents txtConfirmPassword As TextBox
    Protected WithEvents btnRegister As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
        End If
    End Sub

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click
        If txtPassword.Text <> txtConfirmPassword.Text Then
            ShowAlert("Passwords do not match.", False)
            Return
        End If

        Dim model As New Donor()
        model.DonorName = txtDonorName.Text
        model.DonorType = ddlDonorType.SelectedValue
        model.ContactPerson = txtContactPerson.Text
        model.MobileNumber = txtMobile.Text
        model.EmailAddress = txtEmail.Text
        model.City = ddlCity.SelectedValue
        model.Address = txtAddress.Text

        Dim bll As New DonorBLL()
        Dim msg As String = ""
        If bll.RegisterDonor(model, txtPassword.Text, msg) Then
            Response.Redirect("~/Account/Login.aspx?role=Donor&msg=" & Server.UrlEncode(msg), True)
        Else
            ShowAlert(msg, False)
        End If
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
