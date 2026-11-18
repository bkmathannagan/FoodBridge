Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ManageVolunteers
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlAvailability As DropDownList
    Protected WithEvents btnSearch As Button
    Protected WithEvents gvVolunteers As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))
            BindVolunteers()
        End If
    End Sub

    Private Sub BindVolunteers()
        Dim bll As New VolunteerBLL()
        gvVolunteers.DataSource = bll.GetAllVolunteers(txtSearch.Text, ddlCity.SelectedValue, ddlAvailability.SelectedValue, "All")
        gvVolunteers.DataBind()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindVolunteers()
    End Sub

    Protected Sub gvVolunteers_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvVolunteers.RowCommand
        Dim volId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim bll As New VolunteerBLL()
        If e.CommandName = "ActivateVol" Then
            bll.UpdateAccountStatus(volId, "Active")
            pnlAlert.Visible = True
            litAlert.Text = "Volunteer #" & volId & " activated."
        ElseIf e.CommandName = "BlockVol" Then
            bll.UpdateAccountStatus(volId, "Inactive")
            pnlAlert.Visible = True
            litAlert.Text = "Volunteer #" & volId & " deactivated."
        End If
        BindVolunteers()
    End Sub
End Class
