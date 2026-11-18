Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Contact
    Inherits System.Web.UI.Page

    Protected WithEvents pnlMessage As Panel
    Protected WithEvents litMessage As Literal
    Protected WithEvents txtName As TextBox
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents ddlRole As DropDownList
    Protected WithEvents ddlRating As DropDownList
    Protected WithEvents txtSubject As TextBox
    Protected WithEvents txtMessage As TextBox
    Protected WithEvents btnSubmitFeedback As Button
    Protected WithEvents rptFeedback As Repeater

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("UserName") IsNot Nothing Then txtName.Text = Convert.ToString(Session("UserName"))
            If Session("UserEmail") IsNot Nothing Then txtEmail.Text = Convert.ToString(Session("UserEmail"))
            If Session("UserRole") IsNot Nothing Then
                Dim r As String = Convert.ToString(Session("UserRole"))
                If ddlRole.Items.FindByValue(r) IsNot Nothing Then ddlRole.SelectedValue = r
            End If
            BindRecentFeedback()
        End If
    End Sub

    Private Sub BindRecentFeedback()
        Dim bll As New DonationBLL()
        rptFeedback.DataSource = bll.GetAllFeedback()
        rptFeedback.DataBind()
    End Sub

    Protected Sub btnSubmitFeedback_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitFeedback.Click
        pnlMessage.Visible = True
        If String.IsNullOrEmpty(txtName.Text.Trim()) OrElse String.IsNullOrEmpty(txtEmail.Text.Trim()) OrElse String.IsNullOrEmpty(txtSubject.Text.Trim()) OrElse String.IsNullOrEmpty(txtMessage.Text.Trim()) Then
            pnlMessage.CssClass = "alert alert-danger"
            litMessage.Text = "Please fill in your Name, Email, Subject, and Message."
            Return
        End If
        If Not DonorBLL.IsValidEmail(txtEmail.Text) Then
            pnlMessage.CssClass = "alert alert-danger"
            litMessage.Text = "Please enter a valid email address."
            Return
        End If

        Dim bll As New DonationBLL()
        Dim uid As Integer = If(Session("UserID") IsNot Nothing, Convert.ToInt32(Session("UserID")), 0)
        If bll.SubmitFeedback(ddlRole.SelectedValue, uid, txtName.Text, txtEmail.Text, Convert.ToInt32(ddlRating.SelectedValue), txtSubject.Text, txtMessage.Text) Then
            pnlMessage.CssClass = "alert alert-success"
            litMessage.Text = "Thank you! Your feedback has been recorded and sent to the FoodBridge coordination team."
            txtSubject.Text = ""
            txtMessage.Text = ""
            BindRecentFeedback()
        Else
            pnlMessage.CssClass = "alert alert-danger"
            litMessage.Text = "Unable to submit feedback right now. Please try again."
        End If
    End Sub
End Class
