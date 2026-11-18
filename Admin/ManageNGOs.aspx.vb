Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageNGOs
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents btnSearch As Button
    Protected WithEvents gvNGOs As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))
            BindNGOs()
        End If
    End Sub

    Private Sub BindNGOs()
        Dim bll As New NGOBLL()
        gvNGOs.DataSource = bll.GetAllNGOs(txtSearch.Text, ddlCity.SelectedValue, ddlStatus.SelectedValue)
        gvNGOs.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindNGOs()
    End Sub

    Protected Sub gvNGOs_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvNGOs.RowCommand
        Dim ngoId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim bll As New NGOBLL()
        If e.CommandName = "VerifyNGO" Then
            bll.UpdateVerificationStatus(ngoId, "Verified")
            pnlAlert.Visible = True
            litAlert.Text = "NGO #" & ngoId & " has been verified and authorized to request food."
        ElseIf e.CommandName = "RejectNGO" Then
            bll.UpdateVerificationStatus(ngoId, "Blocked")
            pnlAlert.Visible = True
            litAlert.Text = "NGO #" & ngoId & " status set to Blocked."
        End If
        BindNGOs()
    End Sub
End Class
