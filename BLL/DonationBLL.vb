Imports System
Imports System.Data

Public Class DonationBLL
    Private ReadOnly _dal As New DonationDAL()

    Public Function SubmitDonation(ByVal model As FoodDonation, ByVal donorName As String, ByRef message As String) As Integer
        If model.DonorID <= 0 Then
            message = "Only registered donors can submit food donations."
            Return 0
        End If
        If String.IsNullOrEmpty(model.FoodName) OrElse String.IsNullOrEmpty(model.PickupAddress) OrElse String.IsNullOrEmpty(model.City) OrElse String.IsNullOrEmpty(model.PINCode) Then
            message = "Please enter Food Name, Pickup Address, City, and PIN Code."
            Return 0
        End If
        If model.FoodQuantity <= 0 Then
            message = "Food quantity must be greater than zero."
            Return 0
        End If
        If model.NumberOfPersonsServed <= 0 Then
            message = "Estimated number of persons served must be greater than zero."
            Return 0
        End If
        ' Validate food expiry information - do not allow expired food donations
        If model.ExpiryDate.Date < DateTime.Today Then
            message = "Expired food donations are not permitted. Expiry date must be today or a future date."
            Return 0
        End If
        If model.ExpiryDate.Date < model.FoodPreparationDate.Date Then
            message = "Expiry date cannot be earlier than the food preparation date."
            Return 0
        End If

        Dim newId As Integer = _dal.AddDonation(model, donorName)
        If newId > 0 Then
            message = "Food donation #" & newId & " submitted successfully! Awaiting administrator verification."
            Return newId
        End If
        message = "Failed to register food donation."
        Return 0
    End Function

    Public Function EditDonationBeforeApproval(ByVal model As FoodDonation, ByVal donorName As String, ByRef message As String) As Boolean
        If model.FoodQuantity <= 0 OrElse model.NumberOfPersonsServed <= 0 Then
            message = "Quantity and persons served must be greater than zero."
            Return False
        End If
        If model.ExpiryDate.Date < DateTime.Today Then
            message = "Expiry date cannot be in the past."
            Return False
        End If
        If _dal.UpdateDonationBeforeApproval(model, donorName) Then
            message = "Donation details updated successfully."
            Return True
        End If
        message = "Only pending donations owned by you can be edited."
        Return False
    End Function

    Public Function CancelDonation(ByVal donationId As Integer, ByVal donorId As Integer, ByVal donorName As String, ByRef message As String) As Boolean
        If _dal.CancelDonationByDonor(donationId, donorId, donorName) Then
            message = "Donation request #" & donationId & " has been cancelled."
            Return True
        End If
        message = "Cannot cancel a donation that has already been collected or delivered."
        Return False
    End Function

    Public Function ApproveOrRejectDonation(ByVal donationId As Integer, ByVal status As String, ByVal remarks As String, ByVal adminName As String, ByRef message As String) As Boolean
        If status = "Approved" Then
            Dim don As FoodDonation = _dal.GetDonationById(donationId)
            If don IsNot Nothing AndAlso don.ExpiryDate.Date < DateTime.Today Then
                message = "Cannot approve an expired food donation."
                Return False
            End If
        End If
        If _dal.UpdateApprovalStatus(donationId, status, remarks, adminName) Then
            message = "Donation #" & donationId & " marked as " & status & "."
            Return True
        End If
        message = "Failed to update donation status."
        Return False
    End Function

    Public Function GetDonationById(ByVal donationId As Integer) As FoodDonation
        Return _dal.GetDonationById(donationId)
    End Function

    Public Function GetDonationsByDonor(ByVal donorId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        Return _dal.GetDonationsByDonor(donorId, statusFilter)
    End Function

    Public Function GetAvailableDonationsForNGO(Optional ByVal category As String = "", Optional ByVal city As String = "", Optional ByVal foodType As String = "", Optional ByVal search As String = "") As DataTable
        Return _dal.GetAvailableDonationsForNGO(category, city, foodType, search)
    End Function

    Public Function GetAllDonationsForAdmin(Optional ByVal search As String = "", Optional ByVal category As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        Return _dal.GetAllDonationsForAdmin(search, category, city, status)
    End Function

    Public Function GetDonationTimeline(ByVal donationId As Integer) As DataTable
        Return _dal.GetDonationTimeline(donationId)
    End Function

    Public Function GetCategories(Optional ByVal activeOnly As Boolean = True) As DataTable
        Return _dal.GetAllCategories(activeOnly)
    End Function

    Public Function SaveCategory(ByVal id As Integer, ByVal name As String, ByVal desc As String, ByVal isActive As Boolean) As Boolean
        Return _dal.SaveCategory(id, name, desc, isActive)
    End Function

    Public Function DeleteCategory(ByVal id As Integer) As Boolean
        Return _dal.DeleteCategory(id)
    End Function

    Public Function GetCities(Optional ByVal activeOnly As Boolean = True) As DataTable
        Return _dal.GetAllCities(activeOnly)
    End Function

    Public Function SaveCity(ByVal id As Integer, ByVal cityName As String, ByVal stateName As String, ByVal pinPrefix As String, ByVal isActive As Boolean) As Boolean
        Return _dal.SaveCity(id, cityName, stateName, pinPrefix, isActive)
    End Function

    Public Function DeleteCity(ByVal id As Integer) As Boolean
        Return _dal.DeleteCity(id)
    End Function

    Public Function GetActiveAnnouncements(Optional ByVal role As String = "All") As DataTable
        Return _dal.GetActiveAnnouncements(role)
    End Function

    Public Function GetAllAnnouncements() As DataTable
        Return _dal.GetAllAnnouncements()
    End Function

    Public Function SaveAnnouncement(ByVal id As Integer, ByVal title As String, ByVal content As String, ByVal targetRole As String, ByVal expDate As DateTime, ByVal isActive As Boolean) As Boolean
        Return _dal.SaveAnnouncement(id, title, content, targetRole, expDate, isActive)
    End Function

    Public Function DeleteAnnouncement(ByVal id As Integer) As Boolean
        Return _dal.DeleteAnnouncement(id)
    End Function

    Public Function SubmitFeedback(ByVal role As String, ByVal userId As Integer, ByVal userName As String, ByVal email As String, ByVal rating As Integer, ByVal subject As String, ByVal msg As String) As Boolean
        Return _dal.SubmitFeedback(role, userId, userName, email, rating, subject, msg)
    End Function

    Public Function GetAllFeedback() As DataTable
        Return _dal.GetAllFeedback()
    End Function

    Public Function ReplyFeedback(ByVal feedbackId As Integer, ByVal reply As String) As Boolean
        Return _dal.ReplyFeedback(feedbackId, reply)
    End Function

    Public Function SubmitComplaint(ByVal role As String, ByVal userId As Integer, ByVal userName As String, ByVal donationId As Integer, ByVal subject As String, ByVal description As String, ByVal priority As String) As Boolean
        Return _dal.SubmitComplaint(role, userId, userName, donationId, subject, description, priority)
    End Function

    Public Function GetAllComplaints() As DataTable
        Return _dal.GetAllComplaints()
    End Function

    Public Function ResolveComplaint(ByVal complaintId As Integer, ByVal status As String, ByVal notes As String) As Boolean
        Return _dal.ResolveComplaint(complaintId, status, notes)
    End Function
End Class
