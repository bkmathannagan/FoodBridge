Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class UpdateDelivery
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlTasks As DropDownList
    Protected WithEvents ddlNewStatus As DropDownList
    Protected WithEvents txtCollectionRemarks As TextBox
    Protected WithEvents txtReceivedBy As TextBox
    Protected WithEvents ddlCondition As DropDownList
    Protected WithEvents txtDeliveryRemarks As TextBox
    Protected WithEvents btnUpdateStatus As Button
    Protected WithEvents litTaskSummary As Literal

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindTasksDropdown()
            Dim qTaskId As String = Request.QueryString("taskId")
            If Not String.IsNullOrEmpty(qTaskId) AndAlso ddlTasks.Items.FindByValue(qTaskId) IsNot Nothing Then
                ddlTasks.SelectedValue = qTaskId
            End If
            LoadSelectedTaskDetails()
        End If
    End Sub

    Private Sub BindTasksDropdown()
        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New AssignmentBLL()
        Dim dt As DataTable = bll.GetTasksByVolunteer(volId)
        ddlTasks.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim txt As String = "Task #" & row("TaskID").ToString() & " - " & row("FoodName").ToString() & " (" & row("AssignedQuantity").ToString() & " " & row("QuantityUnit").ToString() & ") [" & row("TaskStatus").ToString() & "]"
            ddlTasks.Items.Add(New ListItem(txt, row("TaskID").ToString()))
        Next
        If ddlTasks.Items.Count = 0 Then
            ddlTasks.Items.Add(New ListItem("-- No Assigned Tasks --", "0"))
        End If
    End Sub

    Private Sub LoadSelectedTaskDetails()
        Dim taskId As Integer = 0
        If Integer.TryParse(ddlTasks.SelectedValue, taskId) AndAlso taskId > 0 Then
            Dim bll As New AssignmentBLL()
            Dim row As DataRow = bll.GetTaskDetailsById(taskId)
            If row IsNot Nothing Then
                txtCollectionRemarks.Text = Convert.ToString(row("CollectionRemarks"))
                txtDeliveryRemarks.Text = Convert.ToString(row("DeliveryRemarks"))
                txtReceivedBy.Text = Convert.ToString(row("ReceiverContact"))

                Dim sb As New System.Text.StringBuilder()
                sb.Append("<p style='margin-bottom:8px;'><strong>Current Status:</strong> <span class='badge badge-" & row("TaskStatus").ToString() & "'>" & Server.HtmlEncode(row("TaskStatus").ToString()) & "</span></p>")
                sb.Append("<p style='margin-bottom:8px;'><strong>Food Item:</strong> " & Server.HtmlEncode(row("FoodName").ToString()) & " (" & row("AssignedQuantity").ToString() & " " & row("QuantityUnit").ToString() & ")</p>")
                sb.Append("<hr style='margin:12px 0;border:0;border-top:1px solid #E2E8F0;'/>")
                sb.Append("<h4 style='color:#15803D;margin-bottom:4px;'>1. Pickup From Donor</h4>")
                sb.Append("<p style='margin-bottom:4px;'><strong>" & Server.HtmlEncode(row("DonorName").ToString()) & "</strong></p>")
                sb.Append("<p style='margin-bottom:4px;'>Contact: " & Server.HtmlEncode(row("DonorContact").ToString()) & " | Phone: <strong>" & Server.HtmlEncode(row("DonorMobile").ToString()) & "</strong></p>")
                sb.Append("<p style='margin-bottom:8px;'>Address: " & Server.HtmlEncode(row("DonorAddress").ToString()) & "</p>")
                sb.Append("<hr style='margin:12px 0;border:0;border-top:1px solid #E2E8F0;'/>")
                sb.Append("<h4 style='color:#0284C7;margin-bottom:4px;'>2. Deliver To NGO Receiver</h4>")
                sb.Append("<p style='margin-bottom:4px;'><strong>" & Server.HtmlEncode(row("NGOName").ToString()) & "</strong></p>")
                sb.Append("<p style='margin-bottom:4px;'>Contact: " & Server.HtmlEncode(row("ReceiverContact").ToString()) & " | Phone: <strong>" & Server.HtmlEncode(row("ReceiverMobile").ToString()) & "</strong></p>")
                sb.Append("<p style='margin-bottom:4px;'>Delivery Address: " & Server.HtmlEncode(row("ReceiverAddress").ToString()) & "</p>")
                litTaskSummary.Text = sb.ToString()
            End If
        End If
    End Sub

    Protected Sub ddlTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTasks.SelectedIndexChanged
        LoadSelectedTaskDetails()
    End Sub

    Protected Sub btnUpdateStatus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateStatus.Click
        Dim taskId As Integer = 0
        Integer.TryParse(ddlTasks.SelectedValue, taskId)
        If taskId <= 0 Then
            pnlAlert.Visible = True
            pnlAlert.CssClass = "alert alert-danger"
            litAlert.Text = "Please select a valid assigned task."
            Return
        End If

        Dim volId As Integer = Convert.ToInt32(Session("UserID"))
        Dim volName As String = Convert.ToString(Session("UserName"))
        Dim bll As New AssignmentBLL()
        Dim msg As String = ""

        If bll.UpdateTaskStatus(taskId, volId, ddlNewStatus.SelectedValue, txtCollectionRemarks.Text, txtDeliveryRemarks.Text, txtReceivedBy.Text, ddlCondition.SelectedValue, volName, msg) Then
            Response.Redirect("~/Volunteer/MyTasks.aspx?msg=" & Server.UrlEncode(msg), True)
        Else
            pnlAlert.Visible = True
            pnlAlert.CssClass = "alert alert-danger"
            litAlert.Text = Server.HtmlEncode(msg)
        End If
    End Sub
End Class
