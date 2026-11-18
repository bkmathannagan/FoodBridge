Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class NGODashboard
    Inherits System.Web.UI.Page

    Protected WithEvents litNGOName As Literal
    Protected WithEvents litAvailable As Literal
    Protected WithEvents litTotalReq As Literal
    Protected WithEvents litPendingReq As Literal
    Protected WithEvents litApprovedReq As Literal
    Protected WithEvents litReceived As Literal
    Protected WithEvents litReceivedQty As Literal
    Protected WithEvents gvRecentRequests As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
            litNGOName.Text = Server.HtmlEncode(Convert.ToString(Session("UserName")))

            Dim ngoBll As New NGOBLL()
            Dim stats As Dictionary(Of String, Object) = ngoBll.GetDashboardStats(ngoId)
            litAvailable.Text = Convert.ToString(stats("AvailableFoodDonations"))
            litTotalReq.Text = Convert.ToString(stats("TotalRequests"))
            litPendingReq.Text = Convert.ToString(stats("PendingRequests"))
            litApprovedReq.Text = Convert.ToString(stats("ApprovedRequests"))
            litReceived.Text = Convert.ToString(stats("ReceivedDonations"))
            litReceivedQty.Text = Convert.ToDouble(stats("TotalQuantityReceived")).ToString("0.#")

            Dim reqBll As New RequestBLL()
            gvRecentRequests.DataSource = reqBll.GetRequestsByNGO(ngoId)
            gvRecentRequests.DataBind()
        End If
    End Sub
End Class
