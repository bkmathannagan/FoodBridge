Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class RegisterNGO
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtOrgName As TextBox
    Protected WithEvents txtRegNumber As TextBox
    Protected WithEvents txtContactPerson As TextBox
    Protected WithEvents txtMobile As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents txtBeneficiaries As TextBox
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
            ShowAlert("Passwords do not match.")
            Return
        End If

        Dim benef As Integer = 0
        Integer.TryParse(txtBeneficiaries.Text.Trim(), benef)

        Dim model As New NGO()
        model.OrganizationName = txtOrgName.Text
        model.RegistrationNumber = txtRegNumber.Text
        model.ContactPerson = txtContactPerson.Text
        model.MobileNumber = txtMobile.Text
        model.EmailAddress = txtEmail.Text
        model.NumberOfBeneficiaries = benef
        model.City = ddlCity.SelectedValue
        model.Address = txtAddress.Text

        Dim bll As New NGOBLL()
        Dim msg As String = ""
        If bll.RegisterNGO(model, txtPassword.Text, msg) Then
            Response.Redirect("~/Account/Login.aspx?role=NGO&msg=" & Server.UrlEncode(msg), True)
        Else
            ShowAlert(msg)
        End If
    End Sub

    Private Sub ShowAlert(ByVal msg As String)
        pnlAlert.Visible = True
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
