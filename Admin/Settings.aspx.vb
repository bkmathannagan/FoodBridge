Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AdminSettings
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtAnnTitle As TextBox
    Protected WithEvents ddlTargetRole As DropDownList
    Protected WithEvents txtAnnContent As TextBox
    Protected WithEvents btnPublishAnn As Button
    Protected WithEvents gvAnnouncements As GridView
    Protected WithEvents txtCurrentPwd As TextBox
    Protected WithEvents txtNewPwd As TextBox
    Protected WithEvents txtConfirmPwd As TextBox
    Protected WithEvents btnChangePwd As Button
    Protected WithEvents gvFeedback As GridView
    Protected WithEvents gvComplaints As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindSettingsData()
        End If
    End Sub

    Private Sub BindSettingsData()
        Dim donBll As New DonationBLL()
        gvAnnouncements.DataSource = donBll.GetAllAnnouncements()
        gvAnnouncements.DataBind()

        gvFeedback.DataSource = donBll.GetAllFeedback()
        gvFeedback.DataBind()

        gvComplaints.DataSource = donBll.GetAllComplaints()
        gvComplaints.DataBind()
    End Sub

    Protected Sub btnPublishAnn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPublishAnn.Click
        If String.IsNullOrEmpty(txtAnnTitle.Text.Trim()) OrElse String.IsNullOrEmpty(txtAnnContent.Text.Trim()) Then
            ShowAlert("Please enter announcement title and message.", False)
            Return
        End If
        Dim donBll As New DonationBLL()
        donBll.SaveAnnouncement(0, txtAnnTitle.Text, txtAnnContent.Text, ddlTargetRole.SelectedValue, DateTime.Today.AddDays(60), True)
        txtAnnTitle.Text = ""
        txtAnnContent.Text = ""
        ShowAlert("Announcement published successfully.", True)
        BindSettingsData()
    End Sub

    Protected Sub gvAnnouncements_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAnnouncements.RowCommand
        If e.CommandName = "DeleteAnn" Then
            Dim donBll As New DonationBLL()
            donBll.DeleteAnnouncement(Convert.ToInt32(e.CommandArgument))
            ShowAlert("Announcement deleted.", True)
            BindSettingsData()
        End If
    End Sub

    Protected Sub gvComplaints_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvComplaints.RowCommand
        If e.CommandName = "ResolveComp" Then
            Dim donBll As New DonationBLL()
            donBll.ResolveComplaint(Convert.ToInt32(e.CommandArgument), "Resolved", "Reviewed and resolved by Administrator.")
            ShowAlert("Complaint marked as Resolved.", True)
            BindSettingsData()
        End If
    End Sub

    Protected Sub btnChangePwd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangePwd.Click
        If txtNewPwd.Text <> txtConfirmPwd.Text Then
            ShowAlert("New passwords do not match.", False)
            Return
        End If
        Dim adminId As Integer = Convert.ToInt32(Session("UserID"))
        Dim repBll As New ReportBLL()
        Dim msg As String = ""
        If repBll.ChangeAdminPassword(adminId, txtCurrentPwd.Text, txtNewPwd.Text, msg) Then
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
