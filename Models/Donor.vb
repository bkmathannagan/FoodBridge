Imports System

Public Class Donor
    Public Property DonorID As Integer
    Public Property DonorName As String
    Public Property DonorType As String
    Public Property ContactPerson As String
    Public Property MobileNumber As String
    Public Property EmailAddress As String
    Public Property Address As String
    Public Property City As String
    Public Property PasswordHash As String
    Public Property RegistrationDate As DateTime
    Public Property AccountStatus As String
    Public Property ProfileImage As String

    Public Sub New()
        DonorName = String.Empty
        DonorType = "Individual"
        ContactPerson = String.Empty
        MobileNumber = String.Empty
        EmailAddress = String.Empty
        Address = String.Empty
        City = String.Empty
        PasswordHash = String.Empty
        RegistrationDate = DateTime.Now
        AccountStatus = "Active"
        ProfileImage = String.Empty
    End Sub
End Class
