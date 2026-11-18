Imports System

Public Class FoodDonation
    Public Property DonationID As Integer
    Public Property DonorID As Integer
    Public Property DonorName As String
    Public Property DonorMobile As String
    Public Property FoodName As String
    Public Property FoodCategory As String
    Public Property FoodDescription As String
    Public Property FoodType As String
    Public Property FoodQuantity As Double
    Public Property AvailableQuantity As Double
    Public Property QuantityUnit As String
    Public Property NumberOfPersonsServed As Integer
    Public Property FoodPreparationDate As DateTime
    Public Property FoodPreparationTime As String
    Public Property ExpiryDate As DateTime
    Public Property ExpiryTime As String
    Public Property PickupAddress As String
    Public Property City As String
    Public Property PINCode As String
    Public Property PreferredPickupDate As DateTime
    Public Property PreferredPickupTime As String
    Public Property SpecialInstructions As String
    Public Property FoodImage As String
    Public Property DonationDate As DateTime
    Public Property ApprovalStatus As String
    Public Property CollectionStatus As String
    Public Property AdminRemarks As String

    Public Sub New()
        FoodName = String.Empty
        FoodCategory = "Cooked Food"
        FoodDescription = String.Empty
        FoodType = "Veg"
        FoodQuantity = 0
        AvailableQuantity = 0
        QuantityUnit = "Kg"
        NumberOfPersonsServed = 0
        FoodPreparationDate = DateTime.Today
        FoodPreparationTime = "10:00"
        ExpiryDate = DateTime.Today.AddDays(1)
        ExpiryTime = "22:00"
        PickupAddress = String.Empty
        City = String.Empty
        PINCode = String.Empty
        PreferredPickupDate = DateTime.Today
        PreferredPickupTime = "14:00"
        SpecialInstructions = String.Empty
        FoodImage = String.Empty
        DonationDate = DateTime.Now
        ApprovalStatus = "Pending"
        CollectionStatus = "Pending"
        AdminRemarks = String.Empty
    End Sub
End Class
