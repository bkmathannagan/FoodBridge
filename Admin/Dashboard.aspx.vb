Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AdminDashboard
    Inherits System.Web.UI.Page

    Protected WithEvents litTotalDonors As Literal
    Protected WithEvents litTotalNGOs As Literal
    Protected WithEvents litTotalVolunteers As Literal
    Protected WithEvents litTotalDonations As Literal
    Protected WithEvents litPendingDonations As Literal
    Protected WithEvents litApprovedDonations As Literal
    Protected WithEvents litTotalFoodKg As Literal
    Protected WithEvents litCompletedDeliveries As Literal
    Protected WithEvents litPendingRequests As Literal
    Protected WithEvents litExpiredDonations As Literal
    Protected WithEvents rptStatusChart As Repeater
    Protected WithEvents rptCategoryChart As Repeater
    Protected WithEvents gvMonthlySummary As GridView
    Protected WithEvents gvRecentDonations As GridView
    Protected WithEvents gvRecentRequests As GridView
    Protected WithEvents gvLatestAssignments As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim repBll As New ReportBLL()
            Dim stats As Dictionary(Of String, Object) = repBll.GetAdminDashboardStats()

            litTotalDonors.Text = Convert.ToString(stats("TotalDonors"))
            litTotalNGOs.Text = Convert.ToString(stats("TotalNGOs"))
            litTotalVolunteers.Text = Convert.ToString(stats("TotalVolunteers"))
            litTotalDonations.Text = Convert.ToString(stats("TotalDonations"))
            litPendingDonations.Text = Convert.ToString(stats("PendingDonations"))
            litApprovedDonations.Text = Convert.ToString(stats("ApprovedDonations"))
            litTotalFoodKg.Text = Convert.ToDouble(stats("TotalFoodRescuedKg")).ToString("0.#")
            litCompletedDeliveries.Text = Convert.ToString(stats("CompletedDeliveries"))
            litPendingRequests.Text = Convert.ToString(stats("PendingRequests"))
            litExpiredDonations.Text = Convert.ToString(stats("ExpiredDonations"))

            rptStatusChart.DataSource = repBll.GetDonationStatusBreakdown()
            rptStatusChart.DataBind()

            rptCategoryChart.DataSource = repBll.GetFoodCategoryBreakdown()
            rptCategoryChart.DataBind()

            gvMonthlySummary.DataSource = repBll.GetMonthlyRescueSummary()
            gvMonthlySummary.DataBind()

            Dim donBll As New DonationBLL()
            gvRecentDonations.DataSource = donBll.GetAllDonationsForAdmin()
            gvRecentDonations.DataBind()

            Dim reqBll As New RequestBLL()
            gvRecentRequests.DataSource = reqBll.GetAllRequestsForAdmin()
            gvRecentRequests.DataBind()

            Dim assignBll As New AssignmentBLL()
            gvLatestAssignments.DataSource = assignBll.GetAllAssignmentsForAdmin()
            gvLatestAssignments.DataBind()
        End If
    End Sub
End Class
