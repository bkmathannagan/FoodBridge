Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class _Default
    Inherits System.Web.UI.Page

    Protected WithEvents litStatFoodKg As Literal
    Protected WithEvents litStatPeopleFed As Literal
    Protected WithEvents litStatDonors As Literal
    Protected WithEvents litStatNGOs As Literal
    Protected WithEvents litStatVolunteers As Literal
    Protected WithEvents litStatDeliveries As Literal
    Protected WithEvents rptAnnouncements As Repeater
    Protected WithEvents gvRecentAvailable As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadHomeData()
        End If
    End Sub

    Private Sub LoadHomeData()
        Try
            Dim reportBll As New ReportBLL()
            Dim stats As Dictionary(Of String, Object) = reportBll.GetAdminDashboardStats()
            litStatFoodKg.Text = Convert.ToDouble(stats("TotalFoodRescuedKg")).ToString("0.#")
            litStatPeopleFed.Text = Convert.ToString(stats("TotalPeopleFed"))
            litStatDonors.Text = Convert.ToString(stats("TotalDonors"))
            litStatNGOs.Text = Convert.ToString(stats("TotalNGOs"))
            litStatVolunteers.Text = Convert.ToString(stats("TotalVolunteers"))
            litStatDeliveries.Text = Convert.ToString(stats("CompletedDeliveries"))

            Dim donBll As New DonationBLL()
            rptAnnouncements.DataSource = donBll.GetActiveAnnouncements("All")
            rptAnnouncements.DataBind()

            gvRecentAvailable.DataSource = donBll.GetAvailableDonationsForNGO()
            gvRecentAvailable.DataBind()
        Catch
        End Try
    End Sub
End Class
