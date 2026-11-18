Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class About
    Inherits System.Web.UI.Page

    Protected WithEvents litDonors As Literal
    Protected WithEvents litNGOs As Literal
    Protected WithEvents litVolunteers As Literal
    Protected WithEvents litFoodKg As Literal

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim bll As New ReportBLL()
            Dim stats As Dictionary(Of String, Object) = bll.GetAdminDashboardStats()
            litDonors.Text = Convert.ToString(stats("TotalDonors"))
            litNGOs.Text = Convert.ToString(stats("TotalNGOs"))
            litVolunteers.Text = Convert.ToString(stats("TotalVolunteers"))
            litFoodKg.Text = Convert.ToDouble(stats("TotalFoodRescuedKg")).ToString("0.#")
        End If
    End Sub
End Class
