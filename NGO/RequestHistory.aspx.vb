Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class RequestHistory
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlStatusFilter As DropDownList
    Protected WithEvents gvRequests As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim msg As String = Request.QueryString("msg")
            If Not String.IsNullOrEmpty(msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
            BindRequests()
        End If
    End Sub

    Private Sub BindRequests()
        Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
        Dim reqBll As New RequestBLL()
        gvRequests.DataSource = reqBll.GetRequestsByNGO(ngoId, ddlStatusFilter.SelectedValue)
        gvRequests.DataBind()
    End Sub

    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStatusFilter.SelectedIndexChanged
        BindRequests()
    End Sub

    Protected Sub gvRequests_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRequests.RowCommand
        If e.CommandName = "CancelReq" Then
            Dim reqId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
            Dim ngoName As String = Convert.ToString(Session("UserName"))
            Dim reqBll As New RequestBLL()
            Dim msg As String = ""
            If reqBll.CancelRequestByNGO(reqId, ngoId, ngoName, msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            Else
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-danger"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
            BindRequests()
        End If
    End Sub
End Class
