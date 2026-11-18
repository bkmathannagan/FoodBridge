Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageDonors
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents btnSearch As Button
    Protected WithEvents gvDonors As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))
            BindDonors()
        End If
    End Sub

    Private Sub BindDonors()
        Dim bll As New DonorBLL()
        gvDonors.DataSource = bll.GetAllDonors(txtSearch.Text, ddlCity.SelectedValue, ddlStatus.SelectedValue)
        gvDonors.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindDonors()
    End Sub

    Protected Sub gvDonors_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvDonors.RowCommand
        Dim donorId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim bll As New DonorBLL()
        If e.CommandName = "SetActive" Then
            bll.UpdateStatus(donorId, "Active")
            pnlAlert.Visible = True
            litAlert.Text = "Donor #" & donorId & " account activated."
        ElseIf e.CommandName = "SetBlocked" Then
            bll.UpdateStatus(donorId, "Blocked")
            pnlAlert.Visible = True
            litAlert.Text = "Donor #" & donorId & " account blocked."
        End If
        BindDonors()
    End Sub
End Class
