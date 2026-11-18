Imports System
Imports System.Collections.Generic
Imports System.Data

Public Class NGOBLL
    Private ReadOnly _dal As New NGODAL()

    Public Function RegisterNGO(ByVal model As NGO, ByVal rawPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.OrganizationName) OrElse String.IsNullOrEmpty(model.RegistrationNumber) OrElse String.IsNullOrEmpty(model.ContactPerson) OrElse String.IsNullOrEmpty(model.Address) OrElse String.IsNullOrEmpty(model.City) Then
            message = "Please complete all required NGO registration fields."
            Return False
        End If
        If Not DonorBLL.IsValidEmail(model.EmailAddress) Then
            message = "Please enter a valid organization email address."
            Return False
        End If
        If Not DonorBLL.IsValidMobile(model.MobileNumber) Then
            message = "Please enter a valid 10-digit mobile number."
            Return False
        End If
        If model.NumberOfBeneficiaries <= 0 Then
            message = "Number of beneficiaries must be greater than zero."
            Return False
        End If
        If String.IsNullOrEmpty(rawPassword) OrElse rawPassword.Trim().Length < 6 Then
            message = "Password must be at least 6 characters long."
            Return False
        End If
        If _dal.EmailOrRegExists(model.EmailAddress, model.RegistrationNumber) Then
            message = "An NGO with this email address or registration number already exists."
            Return False
        End If

        model.PasswordHash = DBConnection.HashPassword(rawPassword)
        model.VerificationStatus = "Verified"
        If _dal.RegisterNGO(model) > 0 Then
            message = "NGO registration completed successfully! You can now log in."
            Return True
        End If
        message = "Registration failed. Please try again."
        Return False
    End Function

    Public Function LoginNGO(ByVal email As String, ByVal rawPassword As String, ByVal ipAddress As String, ByRef message As String) As NGO
        Dim hash As String = DBConnection.HashPassword(rawPassword)
        Dim n As NGO = _dal.AuthenticateNGO(email, hash)
        If n Is Nothing Then
            message = "Invalid NGO email or password."
            Return Nothing
        End If
        If n.VerificationStatus = "Blocked" OrElse n.VerificationStatus = "Rejected" Then
            message = "Your NGO account status is '" & n.VerificationStatus & "'. Please contact administrator."
            Return Nothing
        End If
        DBConnection.LogUserLogin(n.EmailAddress, "NGO", n.NGOID, ipAddress, "Success")
        message = "Login successful."
        Return n
    End Function

    Public Function GetNGOById(ByVal ngoId As Integer) As NGO
        Return _dal.GetNGOById(ngoId)
    End Function

    Public Function UpdateProfile(ByVal model As NGO, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.OrganizationName) OrElse String.IsNullOrEmpty(model.ContactPerson) Then
            message = "Organization Name and Contact Person are required."
            Return False
        End If
        If Not DonorBLL.IsValidMobile(model.MobileNumber) Then
            message = "Please enter a valid mobile number."
            Return False
        End If
        If _dal.UpdateNGOProfile(model) Then
            message = "Organization profile updated successfully."
            Return True
        End If
        message = "Failed to update NGO profile."
        Return False
    End Function

    Public Function ChangePassword(ByVal ngoId As Integer, ByVal currentPassword As String, ByVal newPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(newPassword) OrElse newPassword.Trim().Length < 6 Then
            message = "New password must be at least 6 characters."
            Return False
        End If
        If _dal.ChangePassword(ngoId, DBConnection.HashPassword(currentPassword), DBConnection.HashPassword(newPassword)) Then
            message = "Password updated successfully."
            Return True
        End If
        message = "Current password is incorrect."
        Return False
    End Function

    Public Function GetAllNGOs(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        Return _dal.GetAllNGOs(search, city, status)
    End Function

    Public Function UpdateVerificationStatus(ByVal ngoId As Integer, ByVal status As String) As Boolean
        Return _dal.UpdateVerificationStatus(ngoId, status)
    End Function

    Public Function DeleteNGO(ByVal ngoId As Integer) As Boolean
        Return _dal.DeleteNGO(ngoId)
    End Function

    Public Function GetDashboardStats(ByVal ngoId As Integer) As Dictionary(Of String, Object)
        Return _dal.GetNGODashboardStats(ngoId)
    End Function
End Class
