Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class SubmitFoodRequestPage
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlDonation As DropDownList
    Protected WithEvents txtDonationSummary As TextBox
    Protected WithEvents txtRequestedQty As TextBox
    Protected WithEvents txtBeneficiaries As TextBox
    Protected WithEvents txtRequiredDate As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents txtContactPerson As TextBox
    Protected WithEvents txtContactNumber As TextBox
    Protected WithEvents txtDeliveryAddress As TextBox
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents btnSubmitRequest As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()

            txtRequiredDate.Text = DateTime.Today.ToString("yyyy-MM-dd")

            ' Populate NGO default delivery details
            Dim ngoId As Integer = Convert.ToInt32(Session("UserID"))
            Dim ngoBll As New NGOBLL()
            Dim n As NGO = ngoBll.GetNGOById(ngoId)
            If n IsNot Nothing Then
                txtContactPerson.Text = n.ContactPerson
                txtContactNumber.Text = n.MobileNumber
                txtDeliveryAddress.Text = n.Address
                txtBeneficiaries.Text = n.NumberOfBeneficiaries.ToString()
                If ddlCity.Items.FindByValue(n.City) IsNot Nothing Then ddlCity.SelectedValue = n.City
            End If

            BindDonationsDropdown()

            Dim qId As String = Request.QueryString("donationId")
            If Not String.IsNullOrEmpty(qId) AndAlso ddlDonation.Items.FindByValue(qId) IsNot Nothing Then
                ddlDonation.SelectedValue = qId
            End If
            UpdateDonationSummary()
        End If
    End Sub

    Private Sub BindDonationsDropdown()
        Dim donBll As New DonationBLL()
        Dim dt As DataTable = donBll.GetAvailableDonationsForNGO()
        ddlDonation.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim txt As String = "#" & row("DonationID").ToString() & " - " & row("FoodName").ToString() & " (" & row("AvailableQuantity").ToString() & " " & row("QuantityUnit").ToString() & " avail, " & row("City").ToString() & ")"
            ddlDonation.Items.Add(New ListItem(txt, row("DonationID").ToString()))
        Next
        If ddlDonation.Items.Count = 0 Then
            ddlDonation.Items.Add(New ListItem("-- No Available Donations Right Now --", "0"))
        End If
    End Sub

    Private Sub UpdateDonationSummary()
        Dim donId As Integer = 0
        If Integer.TryParse(ddlDonation.SelectedValue, donId) AndAlso donId > 0 Then
            Dim donBll As New DonationBLL()
            Dim d As FoodDonation = donBll.GetDonationById(donId)
            If d IsNot Nothing Then
                txtDonationSummary.Text = "Available: " & d.AvailableQuantity & " " & d.QuantityUnit & " | Expires: " & d.ExpiryDate.ToString("dd MMM yyyy") & " " & d.ExpiryTime & " | Donor: " & d.DonorName
                txtRequestedQty.Text = d.AvailableQuantity.ToString()
            End If
        Else
            txtDonationSummary.Text = "Please select an available food donation."
        End If
    End Sub

    Protected Sub ddlDonation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDonation.SelectedIndexChanged
        UpdateDonationSummary()
    End Sub

    Protected Sub btnSubmitRequest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitRequest.Click
        Dim donId As Integer = 0
        Integer.TryParse(ddlDonation.SelectedValue, donId)
        Dim reqQty As Double = 0
        Double.TryParse(txtRequestedQty.Text.Trim(), reqQty)
        Dim benef As Integer = 0
        Integer.TryParse(txtBeneficiaries.Text.Trim(), benef)
        Dim reqDate As DateTime = DateTime.Today
        DateTime.TryParse(txtRequiredDate.Text.Trim(), reqDate)

        Dim model As New FoodRequest()
        model.NGOID = Convert.ToInt32(Session("UserID"))
        model.DonationID = donId
        model.RequestedQuantity = reqQty
        model.NumberOfBeneficiaries = benef
        model.RequiredDate = reqDate
        model.City = ddlCity.SelectedValue
        model.ContactPerson = txtContactPerson.Text
        model.ContactNumber = txtContactNumber.Text
        model.DeliveryAddress = txtDeliveryAddress.Text
        model.RequestRemarks = txtRemarks.Text

        Dim reqBll As New RequestBLL()
        Dim msg As String = ""
        Dim newReqId As Integer = reqBll.SubmitRequest(model, Convert.ToString(Session("UserName")), msg)
        If newReqId > 0 Then
            Response.Redirect("~/NGO/RequestHistory.aspx?msg=" & Server.UrlEncode(msg), True)
        Else
            pnlAlert.Visible = True
            pnlAlert.CssClass = "alert alert-danger"
            litAlert.Text = Server.HtmlEncode(msg)
        End If
    End Sub
End Class
