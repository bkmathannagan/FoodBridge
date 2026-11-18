Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class RegisterVolunteer
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtName As TextBox
    Protected WithEvents ddlGender As DropDownList
    Protected WithEvents txtMobile As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlAvailability As DropDownList
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
            ShowAlert("Passwords do not match.")
            Return
        End If

        Dim model As New Volunteer()
        model.VolunteerName = txtName.Text
        model.Gender = ddlGender.SelectedValue
        model.MobileNumber = txtMobile.Text
        model.EmailAddress = txtEmail.Text
        model.City = ddlCity.SelectedValue
        model.AvailabilityStatus = ddlAvailability.SelectedValue
        model.Address = txtAddress.Text

        Dim bll As New VolunteerBLL()
        Dim msg As String = ""
        If bll.RegisterVolunteer(model, txtPassword.Text, msg) Then
            Response.Redirect("~/Account/Login.aspx?role=Volunteer&msg=" & Server.UrlEncode(msg), True)
        Else
            ShowAlert(msg)
        End If
    End Sub

    Private Sub ShowAlert(ByVal msg As String)
        pnlAlert.Visible = True
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
