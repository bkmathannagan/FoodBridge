Imports System

Public Class NGO
    Public Property NGOID As Integer
    Public Property OrganizationName As String
    Public Property RegistrationNumber As String
    Public Property ContactPerson As String
    Public Property MobileNumber As String
    Public Property EmailAddress As String
    Public Property Address As String
    Public Property City As String
    Public Property NumberOfBeneficiaries As Integer
    Public Property PasswordHash As String
    Public Property RegistrationDate As DateTime
    Public Property VerificationStatus As String
    Public Property ProfileImage As String

    Public Sub New()
        OrganizationName = String.Empty
        RegistrationNumber = String.Empty
        ContactPerson = String.Empty
        MobileNumber = String.Empty
        EmailAddress = String.Empty
        Address = String.Empty
        City = String.Empty
        NumberOfBeneficiaries = 0
        PasswordHash = String.Empty
        RegistrationDate = DateTime.Now
        VerificationStatus = "Pending"
        ProfileImage = String.Empty
    End Sub
End Class
