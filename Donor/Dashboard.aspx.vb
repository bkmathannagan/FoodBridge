Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class DonorDashboard
    Inherits System.Web.UI.Page

    Protected WithEvents litDonorName As Literal
    Protected WithEvents litTotal As Literal
    Protected WithEvents litTotalQty As Literal
    Protected WithEvents litPending As Literal
    Protected WithEvents litApproved As Literal
    Protected WithEvents litCollected As Literal
    Protected WithEvents litDelivered As Literal
    Protected WithEvents litPeopleServed As Literal
    Protected WithEvents gvRecentDonations As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
            litDonorName.Text = Server.HtmlEncode(Convert.ToString(Session("UserName")))

            Dim donorBll As New DonorBLL()
            Dim stats As Dictionary(Of String, Object) = donorBll.GetDashboardStats(donorId)
            litTotal.Text = Convert.ToString(stats("TotalDonations"))
            litTotalQty.Text = Convert.ToDouble(stats("TotalQuantityDonated")).ToString("0.#")
            litPending.Text = Convert.ToString(stats("PendingDonations"))
            litApproved.Text = Convert.ToString(stats("ApprovedDonations"))
            litCollected.Text = Convert.ToString(stats("CollectedDonations"))
            litDelivered.Text = Convert.ToString(stats("DeliveredDonations"))
            litPeopleServed.Text = Convert.ToString(stats("TotalPeopleServed"))

            Dim donBll As New DonationBLL()
            gvRecentDonations.DataSource = donBll.GetDonationsByDonor(donorId)
            gvRecentDonations.DataBind()
        End If
    End Sub
End Class
