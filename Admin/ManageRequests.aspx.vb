Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageRequests
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents btnSearch As Button
    Protected WithEvents gvRequests As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))

            Dim qStatus As String = Request.QueryString("status")
            If Not String.IsNullOrEmpty(qStatus) AndAlso ddlStatus.Items.FindByValue(qStatus) IsNot Nothing Then
                ddlStatus.SelectedValue = qStatus
            End If

            BindRequests()
        End If
    End Sub

    Private Sub BindRequests()
        Dim reqBll As New RequestBLL()
        gvRequests.DataSource = reqBll.GetAllRequestsForAdmin(txtSearch.Text, ddlStatus.SelectedValue, ddlCity.SelectedValue)
        gvRequests.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindRequests()
    End Sub

    Protected Sub gvRequests_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRequests.RowCommand
        Dim reqId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim adminName As String = Convert.ToString(Session("UserName"))
        Dim reqBll As New RequestBLL()
        Dim msg As String = ""

        If e.CommandName = "ApproveReq" Then
            If reqBll.ApproveOrRejectRequest(reqId, "Approved", "Approved by Administrator; ready for volunteer dispatch.", adminName, msg) Then
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        ElseIf e.CommandName = "RejectReq" Then
            If reqBll.ApproveOrRejectRequest(reqId, "Rejected", "Rejected by Administrator; reserved quantity released.", adminName, msg) Then
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        End If
        BindRequests()
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
