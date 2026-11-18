Imports System

Public Class VolunteerAssignment
    Public Property TaskID As Integer
    Public Property DonationID As Integer
    Public Property RequestID As Integer
    Public Property VolunteerID As Integer
    Public Property VolunteerName As String
    Public Property VolunteerMobile As String
    Public Property FoodName As String
    Public Property DonorName As String
    Public Property DonorMobile As String
    Public Property DonorAddress As String
    Public Property NGOName As String
    Public Property ReceiverContact As String
    Public Property ReceiverMobile As String
    Public Property ReceiverAddress As String
    Public Property AssignedQuantity As Double
    Public Property QuantityUnit As String
    Public Property AssignmentDate As DateTime
    Public Property CollectionDate As Nullable(Of DateTime)
    Public Property DeliveryDate As Nullable(Of DateTime)
    Public Property TaskStatus As String
    Public Property CollectionRemarks As String
    Public Property DeliveryRemarks As String

    Public Sub New()
        VolunteerName = String.Empty
        VolunteerMobile = String.Empty
        FoodName = String.Empty
        DonorName = String.Empty
        DonorMobile = String.Empty
        DonorAddress = String.Empty
        NGOName = String.Empty
        ReceiverContact = String.Empty
        ReceiverMobile = String.Empty
        ReceiverAddress = String.Empty
        QuantityUnit = "Kg"
        AssignmentDate = DateTime.Now
        TaskStatus = "Assigned"
        CollectionRemarks = String.Empty
        DeliveryRemarks = String.Empty
    End Sub
End Class

Public Class FoodCategory
    Public Property CategoryID As Integer
    Public Property CategoryName As String
    Public Property Description As String
    Public Property IsActive As Boolean
End Class

Public Class CityModel
    Public Property CityID As Integer
    Public Property CityName As String
    Public Property StateName As String
    Public Property PINPrefix As String
    Public Property IsActive As Boolean
End Class

Public Class FeedbackModel
    Public Property FeedbackID As Integer
    Public Property UserRole As String
    Public Property UserID As Integer
    Public Property UserName As String
    Public Property EmailAddress As String
    Public Property Rating As Integer
    Public Property Subject As String
    Public Property Message As String
    Public Property SubmittedDate As DateTime
    Public Property AdminReply As String
    Public Property Status As String
End Class

Public Class ComplaintModel
    Public Property ComplaintID As Integer
    Public Property UserRole As String
    Public Property UserID As Integer
    Public Property UserName As String
    Public Property DonationID As Integer
    Public Property Subject As String
    Public Property Description As String
    Public Property Priority As String
    Public Property Status As String
    Public Property SubmittedDate As DateTime
    Public Property ResolutionNotes As String
End Class

Public Class AnnouncementModel
    Public Property AnnouncementID As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property TargetRole As String
    Public Property PublishDate As DateTime
    Public Property ExpiryDate As DateTime
    Public Property IsActive As Boolean
End Class
