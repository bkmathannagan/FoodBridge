Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageDonations
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCategory As DropDownList
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents btnSearch As Button
    Protected WithEvents gvDonations As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCategory.DataSource = donBll.GetCategories(True)
            ddlCategory.DataTextField = "CategoryName"
            ddlCategory.DataValueField = "CategoryName"
            ddlCategory.DataBind()
            ddlCategory.Items.Insert(0, New ListItem("All Categories", "All"))

            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))

            Dim qStatus As String = Request.QueryString("status")
            If Not String.IsNullOrEmpty(qStatus) AndAlso ddlStatus.Items.FindByValue(qStatus) IsNot Nothing Then
                ddlStatus.SelectedValue = qStatus
            End If

            BindDonations()
        End If
    End Sub

    Private Sub BindDonations()
        Dim donBll As New DonationBLL()
        gvDonations.DataSource = donBll.GetAllDonationsForAdmin(txtSearch.Text, ddlCategory.SelectedValue, ddlCity.SelectedValue, ddlStatus.SelectedValue)
        gvDonations.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindDonations()
    End Sub

    Protected Sub gvDonations_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvDonations.RowCommand
        Dim donId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim adminName As String = Convert.ToString(Session("UserName"))
        Dim bll As New DonationBLL()
        Dim msg As String = ""

        If e.CommandName = "ApproveDon" Then
            If bll.ApproveOrRejectDonation(donId, "Approved", "Verified and approved by Administrator.", adminName, msg) Then
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        ElseIf e.CommandName = "RejectDon" Then
            If bll.ApproveOrRejectDonation(donId, "Rejected", "Rejected by Administrator during verification.", adminName, msg) Then
                ShowAlert(msg, True)
            Else
                ShowAlert(msg, False)
            End If
        End If
        BindDonations()
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
