Imports System
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AddDonation
    Inherits System.Web.UI.Page

    Protected WithEvents litFormTitle As Literal
    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents hfEditDonationId As HiddenField
    Protected WithEvents hfExistingImage As HiddenField
    Protected WithEvents txtFoodName As TextBox
    Protected WithEvents ddlCategory As DropDownList
    Protected WithEvents ddlFoodType As DropDownList
    Protected WithEvents txtQuantity As TextBox
    Protected WithEvents ddlUnit As DropDownList
    Protected WithEvents txtPersonsServed As TextBox
    Protected WithEvents txtPrepDate As TextBox
    Protected WithEvents txtPrepTime As TextBox
    Protected WithEvents txtExpiryDate As TextBox
    Protected WithEvents txtExpiryTime As TextBox
    Protected WithEvents txtPickupDate As TextBox
    Protected WithEvents txtPickupTime As TextBox
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents txtPinCode As TextBox
    Protected WithEvents fuFoodImage As FileUpload
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtPickupAddress As TextBox
    Protected WithEvents txtInstructions As TextBox
    Protected WithEvents btnSaveDonation As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCategory.DataSource = donBll.GetCategories(True)
            ddlCategory.DataTextField = "CategoryName"
            ddlCategory.DataValueField = "CategoryName"
            ddlCategory.DataBind()

            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()

            txtPrepDate.Text = DateTime.Today.ToString("yyyy-MM-dd")
            txtExpiryDate.Text = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")
            txtPickupDate.Text = DateTime.Today.ToString("yyyy-MM-dd")

            ' Pre-populate donor address & city
            Dim donorId As Integer = Convert.ToInt32(Session("UserID"))
            Dim donorBll As New DonorBLL()
            Dim d As Donor = donorBll.GetDonorById(donorId)
            If d IsNot Nothing Then
                txtPickupAddress.Text = d.Address
                If ddlCity.Items.FindByValue(d.City) IsNot Nothing Then ddlCity.SelectedValue = d.City
            End If

            ' Check if editing existing pending donation
            Dim editId As Integer = 0
            If Integer.TryParse(Request.QueryString("editId"), editId) AndAlso editId > 0 Then
                Dim existing As FoodDonation = donBll.GetDonationById(editId)
                If existing IsNot Nothing AndAlso existing.DonorID = donorId AndAlso existing.ApprovalStatus = "Pending" Then
                    hfEditDonationId.Value = existing.DonationID.ToString()
                    hfExistingImage.Value = existing.FoodImage
                    litFormTitle.Text = "Edit Pending Food Donation #" & existing.DonationID
                    btnSaveDonation.Text = "Update Food Donation"
                    txtFoodName.Text = existing.FoodName
                    If ddlCategory.Items.FindByValue(existing.FoodCategory) IsNot Nothing Then ddlCategory.SelectedValue = existing.FoodCategory
                    If ddlFoodType.Items.FindByValue(existing.FoodType) IsNot Nothing Then ddlFoodType.SelectedValue = existing.FoodType
                    txtQuantity.Text = existing.FoodQuantity.ToString()
                    If ddlUnit.Items.FindByValue(existing.QuantityUnit) IsNot Nothing Then ddlUnit.SelectedValue = existing.QuantityUnit
                    txtPersonsServed.Text = existing.NumberOfPersonsServed.ToString()
                    txtPrepDate.Text = existing.FoodPreparationDate.ToString("yyyy-MM-dd")
                    txtPrepTime.Text = existing.FoodPreparationTime
                    txtExpiryDate.Text = existing.ExpiryDate.ToString("yyyy-MM-dd")
                    txtExpiryTime.Text = existing.ExpiryTime
                    txtPickupDate.Text = existing.PreferredPickupDate.ToString("yyyy-MM-dd")
                    txtPickupTime.Text = existing.PreferredPickupTime
                    If ddlCity.Items.FindByValue(existing.City) IsNot Nothing Then ddlCity.SelectedValue = existing.City
                    txtPinCode.Text = existing.PINCode
                    txtDescription.Text = existing.FoodDescription
                    txtPickupAddress.Text = existing.PickupAddress
                    txtInstructions.Text = existing.SpecialInstructions
                End If
            End If
        End If
    End Sub

    Protected Sub btnSaveDonation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDonation.Click
        Dim qty As Double = 0
        Dim persons As Integer = 0
        Dim prepDate As DateTime
        Dim expDate As DateTime
        Dim pickDate As DateTime

        If Not Double.TryParse(txtQuantity.Text.Trim(), qty) OrElse qty <= 0 Then
            ShowAlert("Please enter a valid positive number for Food Quantity.", False)
            Return
        End If
        If Not Integer.TryParse(txtPersonsServed.Text.Trim(), persons) OrElse persons <= 0 Then
            ShowAlert("Please enter a valid positive integer for Number of Persons Served.", False)
            Return
        End If
        If Not DateTime.TryParse(txtPrepDate.Text.Trim(), prepDate) Then
            ShowAlert("Please enter a valid Preparation Date (YYYY-MM-DD).", False)
            Return
        End If
        If Not DateTime.TryParse(txtExpiryDate.Text.Trim(), expDate) Then
            ShowAlert("Please enter a valid Expiry Date (YYYY-MM-DD).", False)
            Return
        End If
        If Not DateTime.TryParse(txtPickupDate.Text.Trim(), pickDate) Then
            ShowAlert("Please enter a valid Preferred Pickup Date (YYYY-MM-DD).", False)
            Return
        End If

        Dim imgPath As String = hfExistingImage.Value
        If fuFoodImage.HasFile Then
            Dim ext As String = Path.GetExtension(fuFoodImage.FileName).ToLowerInvariant()
            Dim allowed As String() = {".jpg", ".jpeg", ".png", ".gif", ".svg"}
            If Array.IndexOf(allowed, ext) < 0 Then
                ShowAlert("Invalid file extension. Only JPG, PNG, GIF, and SVG images are allowed.", False)
                Return
            End If
            If fuFoodImage.PostedFile.ContentLength > 5242880 Then
                ShowAlert("Uploaded image exceeds the maximum allowed size of 5 MB.", False)
                Return
            End If
            Dim fileName As String = "food_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ext
            Dim uploadFolder As String = Server.MapPath("~/Uploads/Food/")
            If Not Directory.Exists(uploadFolder) Then Directory.CreateDirectory(uploadFolder)
            fuFoodImage.SaveAs(Path.Combine(uploadFolder, fileName))
            imgPath = "~/Uploads/Food/" & fileName
        End If

        Dim model As New FoodDonation()
        model.DonationID = Convert.ToInt32(hfEditDonationId.Value)
        model.DonorID = Convert.ToInt32(Session("UserID"))
        model.FoodName = txtFoodName.Text
        model.FoodCategory = ddlCategory.SelectedValue
        model.FoodType = ddlFoodType.SelectedValue
        model.FoodQuantity = qty
        model.AvailableQuantity = qty
        model.QuantityUnit = ddlUnit.SelectedValue
        model.NumberOfPersonsServed = persons
        model.FoodPreparationDate = prepDate
        model.FoodPreparationTime = txtPrepTime.Text
        model.ExpiryDate = expDate
        model.ExpiryTime = txtExpiryTime.Text
        model.PreferredPickupDate = pickDate
        model.PreferredPickupTime = txtPickupTime.Text
        model.City = ddlCity.SelectedValue
        model.PINCode = txtPinCode.Text
        model.FoodDescription = txtDescription.Text
        model.PickupAddress = txtPickupAddress.Text
        model.SpecialInstructions = txtInstructions.Text
        model.FoodImage = imgPath

        Dim donBll As New DonationBLL()
        Dim msg As String = ""
        Dim donorName As String = Convert.ToString(Session("UserName"))

        If model.DonationID > 0 Then
            If donBll.EditDonationBeforeApproval(model, donorName, msg) Then
                Response.Redirect("~/Donor/MyDonations.aspx?msg=" & Server.UrlEncode(msg), True)
            Else
                ShowAlert(msg, False)
            End If
        Else
            Dim newId As Integer = donBll.SubmitDonation(model, donorName, msg)
            If newId > 0 Then
                Response.Redirect("~/Donor/MyDonations.aspx?msg=" & Server.UrlEncode(msg), True)
            Else
                ShowAlert(msg, False)
            End If
        End If
    End Sub

    Private Sub ShowAlert(ByVal msg As String, ByVal isSuccess As Boolean)
        pnlAlert.Visible = True
        pnlAlert.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        litAlert.Text = Server.HtmlEncode(msg)
    End Sub
End Class
