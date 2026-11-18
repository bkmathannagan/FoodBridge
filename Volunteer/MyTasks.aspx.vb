Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class VolunteerTasks
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlStatusFilter As DropDownList
    Protected WithEvents gvTasks As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim msg As String = Request.QueryString("msg")
            If Not String.IsNullOrEmpty(msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
            BindTasks()
        End If
    End Sub

    Private Sub BindTasks()
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New AssignmentBLL()
        gvTasks.DataSource = bll.GetTasksByVolunteer(volId, ddlStatusFilter.SelectedValue)
        gvTasks.DataBind()
    End Sub

    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStatusFilter.SelectedIndexChanged
        BindTasks()
    End Sub

    Protected Sub gvTasks_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTasks.RowCommand
        Dim taskId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim volName As String = Convert.ToString(Session("UserName"))
        Dim bll As New AssignmentBLL()
        Dim msg As String = ""

        If e.CommandName = "QuickAccept" Then
            bll.UpdateTaskStatus(taskId, volId, "Accepted", "Volunteer accepted collection task.", "", "", "", volName, msg)
            pnlAlert.Visible = True
            pnlAlert.CssClass = "alert alert-success"
            litAlert.Text = Server.HtmlEncode(msg)
            BindTasks()
        ElseIf e.CommandName = "QuickReject" Then
            bll.UpdateTaskStatus(taskId, volId, "Rejected", "Volunteer unavailable for this slot.", "", "", "", volName, msg)
            pnlAlert.Visible = True
            pnlAlert.CssClass = "alert alert-warning"
            litAlert.Text = Server.HtmlEncode(msg)
            BindTasks()
        End If
    End Sub
End Class
