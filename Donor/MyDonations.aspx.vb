Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class MyDonations
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlStatusFilter As DropDownList
    Protected WithEvents gvMyDonations As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim msg As String = Request.QueryString("msg")
            If Not String.IsNullOrEmpty(msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
            BindDonations()
        End If
    End Sub

    Private Sub BindDonations()
        Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
        Dim bll As New DonationBLL()
        gvMyDonations.DataSource = bll.GetDonationsByDonor(donorId, ddlStatusFilter.SelectedValue)
        gvMyDonations.DataBind()
    End Sub

    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStatusFilter.SelectedIndexChanged
        BindDonations()
    End Sub

    Protected Sub gvMyDonations_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvMyDonations.RowCommand
        If e.CommandName = "CancelDonation" Then
            Dim donId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
            Dim donorName As String = Convert.ToString(Session("UserName"))
            Dim bll As New DonationBLL()
            Dim msg As String = ""
            If bll.CancelDonation(donId, donorId, donorName, msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            Else
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-danger"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
            BindDonations()
        End If
    End Sub
End Class
