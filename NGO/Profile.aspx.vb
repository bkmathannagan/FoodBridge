Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class NGOProfile
    Inherits System.Web.UI.Page

    Protected WithEvents litVerificationBadge As Literal
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
    Protected WithEvents btnUpdateProfile As Button
    Protected WithEvents txtCurrentPwd As TextBox
    Protected WithEvents txtNewPwd As TextBox
    Protected WithEvents btnChangePwd As Button
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

            Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
            Dim bll As New NGOBLL()
            Dim n As NGO = bll.GetNGOById(ngoId)
            If n IsNot Nothing Then
                litVerificationBadge.Text = "<span class='badge badge-" & n.VerificationStatus & "'>" & Server.HtmlEncode(n.VerificationStatus) & "</span>"
                txtOrgName.Text = n.OrganizationName
                txtRegNumber.Text = n.RegistrationNumber
                txtContactPerson.Text = n.ContactPerson
                txtMobile.Text = n.MobileNumber
                txtEmail.Text = n.EmailAddress
                txtBeneficiaries.Text = n.NumberOfBeneficiaries.ToString()
                If ddlCity.Items.FindByValue(n.City) IsNot Nothing Then ddlCity.SelectedValue = n.City
                txtAddress.Text = n.Address
            End If
        End If
    End Sub

    Protected Sub btnUpdateProfile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateProfile.Click
        Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New NGOBLL()
        Dim n As NGO = bll.GetNGOById(ngoId)
        If n IsNot Nothing Then
            n.OrganizationName = txtOrgName.Text
            n.RegistrationNumber = txtRegNumber.Text
            n.ContactPerson = txtContactPerson.Text
            n.MobileNumber = txtMobile.Text
            Dim benef As Integer = 100
            Integer.TryParse(txtBeneficiaries.Text.Trim(), benef)
            n.NumberOfBeneficiaries = benef
            n.City = ddlCity.SelectedValue
            n.Address = txtAddress.Text
            Dim msg As String = ""
            If bll.UpdateProfile(n, msg) Then
                Session("UserName") = n.OrganizationName
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        End If
    End Sub

    Protected Sub btnChangePwd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangePwd.Click
        Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New NGOBLL()
        Dim msg As String = ""
        If bll.ChangePassword(ngoId, txtCurrentPwd.Text, txtNewPwd.Text, msg) Then
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
        donBll.SubmitFeedback("NGO", Convert.ToInt32(Session("UserID")), Convert.ToString(Session("UserName")), txtEmail.Text, 5, txtFbSubject.Text, txtFbMessage.Text)
        txtFbSubject.Text = ""
        txtFbMessage.Text = ""
        ShowAlert("Thank you! Your NGO feedback has been recorded.", True)
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
