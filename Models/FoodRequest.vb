Imports System

Public Class FoodRequest
    Public Property RequestID As Integer
    Public Property NGOID As Integer
    Public Property NGOName As String
    Public Property DonationID As Integer
    Public Property FoodName As String
    Public Property RequestedQuantity As Double
    Public Property QuantityUnit As String
    Public Property NumberOfBeneficiaries As Integer
    Public Property RequiredDate As DateTime
    Public Property DeliveryAddress As String
    Public Property City As String
    Public Property ContactPerson As String
    Public Property ContactNumber As String
    Public Property RequestRemarks As String
    Public Property RequestDate As DateTime
    Public Property RequestStatus As String
    Public Property AdminRemarks As String

    Public Sub New()
        NGOName = String.Empty
        FoodName = String.Empty
        RequestedQuantity = 0
        QuantityUnit = "Kg"
        NumberOfBeneficiaries = 0
        RequiredDate = DateTime.Today
        DeliveryAddress = String.Empty
        City = String.Empty
        ContactPerson = String.Empty
        ContactNumber = String.Empty
        RequestRemarks = String.Empty
        RequestDate = DateTime.Now
        RequestStatus = "Pending"
        AdminRemarks = String.Empty
    End Sub
End Class
