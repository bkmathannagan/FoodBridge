Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AssignVolunteers
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlRequests As DropDownList
    Protected WithEvents ddlVolunteers As DropDownList
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents btnAssign As Button
    Protected WithEvents ddlStatusFilter As DropDownList
    Protected WithEvents gvAssignments As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindDropdowns()
            Dim qReqId As String = Request.QueryString("reqId")
            If Not String.IsNullOrEmpty(qReqId) AndAlso ddlRequests.Items.FindByValue(qReqId) IsNot Nothing Then
                ddlRequests.SelectedValue = qReqId
            End If
            BindAssignments()
        End If
    End Sub

    Private Sub BindDropdowns()
        Dim reqBll As New RequestBLL()
        ddlRequests.DataSource = reqBll.GetApprovedRequestsForAssignment()
        ddlRequests.DataTextField = "DisplayText"
        ddlRequests.DataValueField = "RequestID"
        ddlRequests.DataBind()
        If ddlRequests.Items.Count = 0 Then
            ddlRequests.Items.Add(New ListItem("-- No Pending/Approved Requests --", "0"))
        End If

        Dim volBll As New VolunteerBLL()
        ddlVolunteers.DataSource = volBll.GetAvailableVolunteers()
        ddlVolunteers.DataTextField = "DisplayInfo"
        ddlVolunteers.DataValueField = "VolunteerID"
        ddlVolunteers.DataBind()
    End Sub

    Private Sub BindAssignments()
        Dim assignBll As New AssignmentBLL()
        gvAssignments.DataSource = assignBll.GetAllAssignmentsForAdmin(ddlStatusFilter.SelectedValue)
        gvAssignments.DataBind()
    End Sub

    Protected Sub btnAssign_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssign.Click
        Dim reqId As Integer = 0
        Dim volId As Integer = 0
        Integer.TryParse(ddlRequests.SelectedValue, reqId)
        Integer.TryParse(ddlVolunteers.SelectedValue, volId)

        Dim assignBll As New AssignmentBLL()
        Dim msg As String = ""
        Dim taskId As Integer = assignBll.AssignVolunteer(reqId, volId, txtRemarks.Text, Convert.ToString(Session("UserName")), msg)
        pnlAlert.Visible = True
        If taskId > 0 Then
            pnlAlert.CssClass = "alert alert-success"
            litAlert.Text = Server.HtmlEncode(msg)
            txtRemarks.Text = ""
            BindDropdowns()
            BindAssignments()
        Else
            pnlAlert.CssClass = "alert alert-danger"
            litAlert.Text = Server.HtmlEncode(msg)
        End If
    End Sub

    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStatusFilter.SelectedIndexChanged
        BindAssignments()
    End Sub
End Class
