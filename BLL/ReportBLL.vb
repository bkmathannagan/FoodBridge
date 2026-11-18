Imports System
Imports System.Collections.Generic
Imports System.Data

Public Class ReportBLL
    Private ReadOnly _dal As New ReportDAL()

    Public Function LoginAdmin(ByVal email As String, ByVal rawPassword As String, ByVal ipAddress As String, ByRef message As String) As DataRow
        Dim hash As String = DBConnection.HashPassword(rawPassword)
        Dim row As DataRow = _dal.AuthenticateAdmin(email, hash)
        If row Is Nothing Then
            message = "Invalid administrator email or password."
            Return Nothing
        End If
        DBConnection.LogUserLogin(Convert.ToString(row("EmailAddress")), "Admin", Convert.ToInt32(row("AdminID")), ipAddress, "Success")
        message = "Administrator login successful."
        Return row
    End Function

    Public Function ChangeAdminPassword(ByVal adminId As Integer, ByVal currentPassword As String, ByVal newPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(newPassword) OrElse newPassword.Trim().Length < 6 Then
            message = "New password must be at least 6 characters long."
            Return False
        End If
        If _dal.ChangeAdminPassword(adminId, DBConnection.HashPassword(currentPassword), DBConnection.HashPassword(newPassword)) Then
            message = "Administrator password changed successfully."
            Return True
        End If
        message = "Current administrator password is incorrect."
        Return False
    End Function

    Public Function ResetUserPassword(ByVal role As String, ByVal email As String, ByVal mobile As String, ByVal newPassword As String, ByRef message As String) As Boolean
        If String.IsNullOrEmpty(newPassword) OrElse newPassword.Trim().Length < 6 Then
            message = "New password must be at least 6 characters long."
            Return False
        End If
        If _dal.ResetUserPasswordByEmail(role, email, mobile, DBConnection.HashPassword(newPassword)) Then
            message = "Password has been reset successfully! You can now log in with your new password."
            Return True
        End If
        message = "No matching account found with the provided Role, Email Address, and Mobile Number."
        Return False
    End Function

    Public Function GetAdminDashboardStats() As Dictionary(Of String, Object)
        Return _dal.GetAdminDashboardStats()
    End Function

    Public Function GetDonationStatusBreakdown() As DataTable
        Return _dal.GetDonationStatusBreakdown()
    End Function

    Public Function GetFoodCategoryBreakdown() As DataTable
        Return _dal.GetFoodCategoryBreakdown()
    End Function

    Public Function GetMonthlyRescueSummary() As DataTable
        Return _dal.GetMonthlyRescueSummary()
    End Function

    Public Function GenerateReport(ByVal reportType As String, ByVal fromDate As Nullable(Of DateTime), ByVal toDate As Nullable(Of DateTime), ByVal status As String, ByVal category As String, ByVal city As String, ByVal search As String) As DataTable
        Return _dal.GenerateReport(reportType, fromDate, toDate, status, category, city, search)
    End Function
End Class
