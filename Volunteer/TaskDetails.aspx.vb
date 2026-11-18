Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class VolunteerProfileTasks
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
    Protected WithEvents btnSaveProfile As Button
    Protected WithEvents txtCurrentPwd As TextBox
    Protected WithEvents txtNewPwd As TextBox
    Protected WithEvents btnChangePwd As Button
    Protected WithEvents gvCompletedHistory As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()

            Dim volId As Integer = Convert.ToInt32(Session("UserID"))
            Dim volBll As New VolunteerBLL()
            Dim v As Volunteer = volBll.GetVolunteerById(volId)
            If v IsNot Nothing Then
                txtName.Text = v.VolunteerName
                If ddlGender.Items.FindByValue(v.Gender) IsNot Nothing Then ddlGender.SelectedValue = v.Gender
                txtMobile.Text = v.MobileNumber
                txtEmail.Text = v.EmailAddress
                If ddlCity.Items.FindByValue(v.City) IsNot Nothing Then ddlCity.SelectedValue = v.City
                If ddlAvailability.Items.FindByValue(v.AvailabilityStatus) IsNot Nothing Then ddlAvailability.SelectedValue = v.AvailabilityStatus
                txtAddress.Text = v.Address
            End If

            Dim assignBll As New AssignmentBLL()
            gvCompletedHistory.DataSource = assignBll.GetTasksByVolunteer(volId, "Successfully Delivered")
            gvCompletedHistory.DataBind()
        End If
    End Sub

    Protected Sub btnSaveProfile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveProfile.Click
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim volBll As New VolunteerBLL()
        Dim v As Volunteer = volBll.GetVolunteerById(volId)
        If v IsNot Nothing Then
            v.VolunteerName = txtName.Text
            v.Gender = ddlGender.SelectedValue
            v.MobileNumber = txtMobile.Text
            v.City = ddlCity.SelectedValue
            v.AvailabilityStatus = ddlAvailability.SelectedValue
            v.Address = txtAddress.Text
            Dim msg As String = ""
            If volBll.UpdateProfile(v, msg) Then
                Session("UserName") = v.VolunteerName
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        End If
    End Sub

    Protected Sub btnChangePwd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangePwd.Click
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim volBll As New VolunteerBLL()
        Dim msg As String = ""
        If volBll.ChangePassword(volId, txtCurrentPwd.Text, txtNewPwd.Text, msg) Then
            ShowAlert(msg, True)
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
