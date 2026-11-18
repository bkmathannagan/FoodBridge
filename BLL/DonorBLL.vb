Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Text.RegularExpressions

Public Class DonorBLL
    Private ReadOnly _dal As New DonorDAL()

    Public Shared Function IsValidEmail(ByVal email As String) As Boolean
        If String.IsNullOrEmpty(email) Then Return False
        Return Regex.IsMatch(email.Trim(), "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
    End Function

    Public Shared Function IsValidMobile(ByVal mobile As String) As Boolean
        If String.IsNullOrEmpty(mobile) Then Return False
        Return Regex.IsMatch(mobile.Trim(), "^[0-9+\-\s]{10,15}$")
    End Function

    Public Function RegisterDonor(ByVal model As Donor, ByVal rawPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.DonorName) OrElse String.IsNullOrEmpty(model.ContactPerson) OrElse String.IsNullOrEmpty(model.Address) OrElse String.IsNullOrEmpty(model.City) Then
            message = "Please fill in all required donor registration fields."
            Return False
        End If
        If Not IsValidEmail(model.EmailAddress) Then
            message = "Please enter a valid email address."
            Return False
        End If
        If Not IsValidMobile(model.MobileNumber) Then
            message = "Please enter a valid 10-digit mobile number."
            Return False
        End If
        If String.IsNullOrEmpty(rawPassword) OrElse rawPassword.Trim().Length < 6 Then
            message = "Password must be at least 6 characters long."
            Return False
        End If
        If _dal.EmailExists(model.EmailAddress) Then
            message = "An account with this email address is already registered."
            Return False
        End If

        model.PasswordHash = DBConnection.HashPassword(rawPassword)
        Dim res As Integer = _dal.RegisterDonor(model)
        If res > 0 Then
            message = "Donor registration successful! You may now log in."
            Return True
        End If
        message = "Unable to complete registration. Please try again."
        Return False
    End Function

    Public Function LoginDonor(ByVal email As String, ByVal rawPassword As String, ByVal ipAddress As String, ByRef message As String) As Donor
        Dim hash As String = DBConnection.HashPassword(rawPassword)
        Dim d As Donor = _dal.AuthenticateDonor(email, hash)
        If d Is Nothing Then
            message = "Invalid donor email or password."
            Return Nothing
        End If
        If d.AccountStatus <> "Active" Then
            message = "Your donor account is currently " & d.AccountStatus & ". Please contact the administrator."
            Return Nothing
        End If
        DBConnection.LogUserLogin(d.EmailAddress, "Donor", d.DonorID, ipAddress, "Success")
        message = "Login successful."
        Return d
    End Function

    Public Function GetDonorById(ByVal donorId As Integer) As Donor
        Return _dal.GetDonorById(donorId)
    End Function

    Public Function UpdateProfile(ByVal model As Donor, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.DonorName) OrElse String.IsNullOrEmpty(model.ContactPerson) Then
            message = "Donor Name and Contact Person are required."
            Return False
        End If
        If Not IsValidMobile(model.MobileNumber) Then
            message = "Please enter a valid 10-digit mobile number."
            Return False
        End If
        If _dal.UpdateDonorProfile(model) Then
            message = "Profile updated successfully."
            Return True
        End If
        message = "Failed to update donor profile."
        Return False
    End Function

    Public Function ChangePassword(ByVal donorId As Integer, ByVal currentPassword As String, ByVal newPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(newPassword) OrElse newPassword.Trim().Length < 6 Then
            message = "New password must be at least 6 characters long."
            Return False
        End If
        Dim oldHash As String = DBConnection.HashPassword(currentPassword)
        Dim newHash As String = DBConnection.HashPassword(newPassword)
        If _dal.ChangePassword(donorId, oldHash, newHash) Then
            message = "Password changed successfully."
            Return True
        End If
        message = "Current password is incorrect."
        Return False
    End Function

    Public Function GetAllDonors(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        Return _dal.GetAllDonors(search, city, status)
    End Function

    Public Function UpdateStatus(ByVal donorId As Integer, ByVal status As String) As Boolean
        Return _dal.UpdateAccountStatus(donorId, status)
    End Function

    Public Function DeleteDonor(ByVal donorId As Integer) As Boolean
        Return _dal.DeleteDonor(donorId)
    End Function

    Public Function GetDashboardStats(ByVal donorId As Integer) As Dictionary(Of String, Object)
        Return _dal.GetDonorDashboardStats(donorId)
    End Function
End Class
