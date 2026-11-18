Imports System
Imports System.Data

Public Class RequestBLL
    Private ReadOnly _dal As New RequestDAL()
    Private ReadOnly _ngoDal As New NGODAL()

    Public Function SubmitRequest(ByVal model As FoodRequest, ByVal ngoName As String, ByRef message As String) As Integer
        If model.NGOID <= 0 Then
            message = "Please log in with a valid NGO account to request food."
            Return 0
        End If
        Dim ngoObj As NGO = _ngoDal.GetNGOById(model.NGOID)
        If ngoObj Is Nothing OrElse ngoObj.VerificationStatus <> "Verified" Then
            message = "Only verified NGOs are permitted to submit food requests. Your current verification status is '" & If(ngoObj Is Nothing, "Unknown", ngoObj.VerificationStatus) & "'."
            Return 0
        End If
        If model.DonationID <= 0 Then
            message = "Please select a valid available food donation."
            Return 0
        End If
        If model.RequestedQuantity <= 0 Then
            message = "Requested food quantity must be greater than zero."
            Return 0
        End If
        If model.NumberOfBeneficiaries <= 0 Then
            message = "Number of beneficiaries must be greater than zero."
            Return 0
        End If
        If String.IsNullOrEmpty(model.DeliveryAddress) OrElse String.IsNullOrEmpty(model.ContactPerson) OrElse Not DonorBLL.IsValidMobile(model.ContactNumber) Then
            message = "Please provide a valid delivery address, contact person, and 10-digit contact number."
            Return 0
        End If
        If _dal.HasDuplicateActiveRequest(model.NGOID, model.DonationID) Then
            message = "Your organization already has an active request for this food donation."
            Return 0
        End If

        Try
            Dim reqId As Integer = _dal.SubmitFoodRequest(model, ngoName)
            If reqId > 0 Then
                message = "Food request #" & reqId & " submitted successfully! Quantity has been reserved awaiting administrator confirmation."
                Return reqId
            End If
            message = "Failed to submit food request."
            Return 0
        Catch ex As Exception
            message = ex.Message
            Return 0
        End Try
    End Function

    Public Function ApproveOrRejectRequest(ByVal requestId As Integer, ByVal newStatus As String, ByVal remarks As String, ByVal adminName As String, ByRef message As String) As Boolean
        Try
            If _dal.UpdateRequestStatusByAdmin(requestId, newStatus, remarks, adminName) Then
                message = "Food request #" & requestId & " updated to " & newStatus & "."
                Return True
            End If
            message = "Could not update request status."
            Return False
        Catch ex As Exception
            message = ex.Message
            Return False
        End Try
    End Function

    Public Function CancelRequestByNGO(ByVal requestId As Integer, ByVal ngoId As Integer, ByVal ngoName As String, ByRef message As String) As Boolean
        Try
            If _dal.CancelRequestByNGO(requestId, ngoId, ngoName) Then
                message = "Food request #" & requestId & " cancelled and reserved quantity released."
                Return True
            End If
            message = "Unable to cancel request."
            Return False
        Catch ex As Exception
            message = ex.Message
            Return False
        End Try
    End Function

    Public Function GetRequestsByNGO(ByVal ngoId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        Return _dal.GetRequestsByNGO(ngoId, statusFilter)
    End Function

    Public Function GetAllRequestsForAdmin(Optional ByVal search As String = "", Optional ByVal status As String = "", Optional ByVal city As String = "") As DataTable
        Return _dal.GetAllRequestsForAdmin(search, status, city)
    End Function

    Public Function GetApprovedRequestsForAssignment() As DataTable
        Return _dal.GetApprovedRequestsForAssignment()
    End Function
End Class
