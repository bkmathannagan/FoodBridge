Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class DonationDetails
    Inherits System.Web.UI.Page

    Protected WithEvents litDonationId As Literal
    Protected WithEvents litStatusBadge As Literal
    Protected WithEvents litFoodName As Literal
    Protected WithEvents litCategory As Literal
    Protected WithEvents litQuantity As Literal
    Protected WithEvents litAvailableQty As Literal
    Protected WithEvents litPrep As Literal
    Protected WithEvents litExpiry As Literal
    Protected WithEvents litAddress As Literal
    Protected WithEvents litCollectionStatus As Literal
    Protected WithEvents litAdminRemarks As Literal
    Protected WithEvents rptTimeline As Repeater

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim id As Integer = 0
            If Integer.TryParse(Request.QueryString("id"), id) AndAlso id > 0 Then
                Dim bll As New DonationBLL()
                Dim d As FoodDonation = bll.GetDonationById(id)
                If d IsNot Nothing Then
                    litDonationId.Text = d.DonationID.ToString()
                    litStatusBadge.Text = "<span class='badge badge-" & d.ApprovalStatus & "'>" & Server.HtmlEncode(d.ApprovalStatus) & "</span>"
                    litFoodName.Text = Server.HtmlEncode(d.FoodName)
                    litCategory.Text = Server.HtmlEncode(d.FoodCategory & " (" & d.FoodType & ")")
                    litQuantity.Text = d.FoodQuantity & " " & Server.HtmlEncode(d.QuantityUnit) & " (~" & d.NumberOfPersonsServed & " persons)"
                    litAvailableQty.Text = d.AvailableQuantity & " " & Server.HtmlEncode(d.QuantityUnit)
                    litPrep.Text = d.FoodPreparationDate.ToString("dd MMM yyyy") & " " & Server.HtmlEncode(d.FoodPreparationTime)
                    litExpiry.Text = d.ExpiryDate.ToString("dd MMM yyyy") & " " & Server.HtmlEncode(d.ExpiryTime)
                    litAddress.Text = Server.HtmlEncode(d.PickupAddress & ", " & d.City & " - " & d.PINCode)
                    litCollectionStatus.Text = Server.HtmlEncode(d.CollectionStatus)
                    litAdminRemarks.Text = Server.HtmlEncode(d.AdminRemarks)

                    rptTimeline.DataSource = bll.GetDonationTimeline(id)
                    rptTimeline.DataBind()
                End If
            Else
                Response.Redirect("~/Donor/MyDonations.aspx", True)
            End If
        End If
    End Sub
End Class
