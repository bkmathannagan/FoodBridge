Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class DonorProfile
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
    Protected WithEvents btnUpdateProfile As Button
    Protected WithEvents txtCurrentPwd As TextBox
    Protected WithEvents txtNewPwd As TextBox
    Protected WithEvents btnChangePwd As Button
    Protected WithEvents ddlRating As DropDownList
    Protected WithEvents txtFbSubject As TextBox
    Protected WithEvents txtFbMessage As TextBox
    Protected WithEvents btnSubmitFeedback As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()

            Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
            Dim bll As New DonorBLL()
            Dim d As Donor = bll.GetDonorById(donorId)
            If d IsNot Nothing Then
                txtDonorName.Text = d.DonorName
                If ddlDonorType.Items.FindByValue(d.DonorType) IsNot Nothing Then ddlDonorType.SelectedValue = d.DonorType
                txtContactPerson.Text = d.ContactPerson
                txtMobile.Text = d.MobileNumber
                txtEmail.Text = d.EmailAddress
                If ddlCity.Items.FindByValue(d.City) IsNot Nothing Then ddlCity.SelectedValue = d.City
                txtAddress.Text = d.Address
            End If
        End If
    End Sub

    Protected Sub btnUpdateProfile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateProfile.Click
        Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New DonorBLL()
        Dim d As Donor = bll.GetDonorById(donorId)
        If d IsNot Nothing Then
            d.DonorName = txtDonorName.Text
            d.DonorType = ddlDonorType.SelectedValue
            d.ContactPerson = txtContactPerson.Text
            d.MobileNumber = txtMobile.Text
            d.City = ddlCity.SelectedValue
            d.Address = txtAddress.Text
            Dim msg As String = ""
            If bll.UpdateProfile(d, msg) Then
                Session("UserName") = d.DonorName
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        End If
    End Sub

    Protected Sub btnChangePwd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangePwd.Click
        Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New DonorBLL()
        Dim msg As String = ""
        If bll.ChangePassword(donorId, txtCurrentPwd.Text, txtNewPwd.Text, msg) Then
            ShowAlert(msg, True)
        Else
            ShowAlert(msg, False)
        End If
    End Sub

    Protected Sub btnSubmitFeedback_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitFeedback.Click
        If String.IsNullOrEmpty(txtFbSubject.Text.Trim()) OrElse String.IsNullOrEmpty(txtFbMessage.Text.Trim()) Then
            ShowAlert("Please enter feedback subject and message.", False)
            Return
        End If
        Dim donBll As New DonationBLL()
        donBll.SubmitFeedback("Donor", Convert.ToInt32(Session("UserID")), Convert.ToString(Session("UserName")), txtEmail.Text, Convert.ToInt32(ddlRating.SelectedValue), txtFbSubject.Text, txtFbMessage.Text)
        txtFbSubject.Text = ""
        txtFbMessage.Text = ""
        ShowAlert("Thank you! Your donor feedback has been submitted.", True)
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
