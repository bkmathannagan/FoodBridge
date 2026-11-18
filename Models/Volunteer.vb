Imports System

Public Class Volunteer
    Public Property VolunteerID As Integer
    Public Property VolunteerName As String
    Public Property Gender As String
    Public Property MobileNumber As String
    Public Property EmailAddress As String
    Public Property Address As String
    Public Property City As String
    Public Property AvailabilityStatus As String
    Public Property PasswordHash As String
    Public Property RegistrationDate As DateTime
    Public Property AccountStatus As String
    Public Property ProfileImage As String

    Public Sub New()
        VolunteerName = String.Empty
        Gender = "Male"
        MobileNumber = String.Empty
        EmailAddress = String.Empty
        Address = String.Empty
        City = String.Empty
        AvailabilityStatus = "Available"
        PasswordHash = String.Empty
        RegistrationDate = DateTime.Now
        AccountStatus = "Active"
        ProfileImage = String.Empty
    End Sub
End Class
