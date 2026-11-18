Imports System
Imports System.Collections.Generic
Imports System.Data

Public Class VolunteerBLL
    Private ReadOnly _dal As New VolunteerDAL()

    Public Function RegisterVolunteer(ByVal model As Volunteer, ByVal rawPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.VolunteerName) OrElse String.IsNullOrEmpty(model.Address) OrElse String.IsNullOrEmpty(model.City) Then
            message = "Please fill in all required volunteer fields."
            Return False
        End If
        If Not DonorBLL.IsValidEmail(model.EmailAddress) Then
            message = "Please enter a valid email address."
            Return False
        End If
        If Not DonorBLL.IsValidMobile(model.MobileNumber) Then
            message = "Please enter a valid 10-digit mobile number."
            Return False
        End If
        If String.IsNullOrEmpty(rawPassword) OrElse rawPassword.Trim().Length < 6 Then
            message = "Password must be at least 6 characters long."
            Return False
        End If
        If _dal.EmailExists(model.EmailAddress) Then
            message = "A volunteer with this email address is already registered."
            Return False
        End If

        model.PasswordHash = DBConnection.HashPassword(rawPassword)
        If _dal.RegisterVolunteer(model) > 0 Then
            message = "Volunteer registration successful! You can now log in."
            Return True
        End If
        message = "Failed to register volunteer."
        Return False
    End Function

    Public Function LoginVolunteer(ByVal email As String, ByVal rawPassword As String, ByVal ipAddress As String, ByRef message As String) As Volunteer
        Dim hash As String = DBConnection.HashPassword(rawPassword)
        Dim v As Volunteer = _dal.AuthenticateVolunteer(email, hash)
        If v Is Nothing Then
            message = "Invalid volunteer email or password."
            Return Nothing
        End If
        If v.AccountStatus <> "Active" Then
            message = "Your volunteer account is currently " & v.AccountStatus & ". Please contact the administrator."
            Return Nothing
        End If
        DBConnection.LogUserLogin(v.EmailAddress, "Volunteer", v.VolunteerID, ipAddress, "Success")
        message = "Login successful."
        Return v
    End Function

    Public Function GetVolunteerById(ByVal volunteerId As Integer) As Volunteer
        Return _dal.GetVolunteerById(volunteerId)
    End Function

    Public Function UpdateProfile(ByVal model As Volunteer, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(model.VolunteerName) OrElse Not DonorBLL.IsValidMobile(model.MobileNumber) Then
            message = "Please enter valid volunteer name and mobile number."
            Return False
        End If
        If _dal.UpdateVolunteerProfile(model) Then
            message = "Volunteer profile updated successfully."
            Return True
        End If
        message = "Unable to update profile."
        Return False
    End Function

    Public Function ToggleAvailability(ByVal volunteerId As Integer, ByVal availability As String) As Boolean
        Return _dal.UpdateAvailability(volunteerId, availability)
    End Function

    Public Function ChangePassword(ByVal volunteerId As Integer, ByVal currentPassword As String, ByVal newPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(newPassword) OrElse newPassword.Trim().Length < 6 Then
            message = "New password must be at least 6 characters."
            Return False
        End If
        If _dal.ChangePassword(volunteerId, DBConnection.HashPassword(currentPassword), DBConnection.HashPassword(newPassword)) Then
            message = "Password updated successfully."
            Return True
        End If
        message = "Current password is incorrect."
        Return False
    End Function

    Public Function GetAllVolunteers(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal availability As String = "", Optional ByVal status As String = "") As DataTable
        Return _dal.GetAllVolunteers(search, city, availability, status)
    End Function

    Public Function GetAvailableVolunteers(Optional ByVal city As String = "") As DataTable
        Return _dal.GetAvailableVolunteers(city)
    End Function

    Public Function UpdateAccountStatus(ByVal volunteerId As Integer, ByVal status As String) As Boolean
        Return _dal.UpdateAccountStatus(volunteerId, status)
    End Function

    Public Function DeleteVolunteer(ByVal volunteerId As Integer) As Boolean
        Return _dal.DeleteVolunteer(volunteerId)
    End Function

    Public Function GetDashboardStats(ByVal volunteerId As Integer) As Dictionary(Of String, Object)
        Return _dal.GetVolunteerDashboardStats(volunteerId)
    End Function
End Class
