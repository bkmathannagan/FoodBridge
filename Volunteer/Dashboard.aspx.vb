Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class VolunteerDashboard
    Inherits System.Web.UI.Page

    Protected WithEvents litVolunteerName As Literal
    Protected WithEvents litAvailBadge As Literal
    Protected WithEvents btnToggleAvailability As Button
    Protected WithEvents litTotalTasks As Literal
    Protected WithEvents litPendingTasks As Literal
    Protected WithEvents litAcceptedTasks As Literal
    Protected WithEvents litCompleted As Literal
    Protected WithEvents litDeliveredQty As Literal
    Protected WithEvents gvRecentTasks As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindDashboard()
        End If
    End Sub

    Private Sub BindDashboard()
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        litVolunteerName.Text = Server.HtmlEncode(Convert.ToString(Session("UserName")))

        Dim volBll As New VolunteerBLL()
        Dim stats As Dictionary(Of String, Object) = volBll.GetDashboardStats(volId)
        litTotalTasks.Text = Convert.ToString(stats("TotalAssignedTasks"))
        litPendingTasks.Text = Convert.ToString(stats("PendingTasks"))
        litAcceptedTasks.Text = Convert.ToString(stats("AcceptedTasks"))
        litCompleted.Text = Convert.ToString(stats("CompletedDeliveries"))
        litDeliveredQty.Text = Convert.ToDouble(stats("TotalQuantityDelivered")).ToString("0.#")

        Dim avail As String = Convert.ToString(stats("AvailabilityStatus"))
        litAvailBadge.Text = "<span class='badge badge-" & avail & "'>" & Server.HtmlEncode(avail) & "</span>"
        btnToggleAvailability.CommandArgument = If(avail = "Available", "Unavailable", "Available")
        btnToggleAvailability.Text = "Switch to " & btnToggleAvailability.CommandArgument

        Dim assignBll As New AssignmentBLL()
        gvRecentTasks.DataSource = assignBll.GetTasksByVolunteer(volId)
        gvRecentTasks.DataBind()
    End Sub

    Protected Sub btnToggleAvailability_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnToggleAvailability.Click
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim nextStatus As String = btnToggleAvailability.CommandArgument
        Dim volBll As New VolunteerBLL()
        volBll.ToggleAvailability(volId, nextStatus)
        BindDashboard()
    End Sub
End Class
