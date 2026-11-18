Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class DonorDAL
    Public Function EmailExists(ByVal email As String, Optional ByVal excludeDonorId As Integer = 0) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM [Donors] WHERE LCase([EmailAddress]) = LCase(?) AND [DonorID] <> ?"
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar(sql, New Object() {email.Trim(), excludeDonorId}))
        Return count > 0
    End Function

    Public Function RegisterDonor(ByVal model As Donor) As Integer
        Dim sql As String = "INSERT INTO [Donors] ([DonorName], [DonorType], [ContactPerson], [MobileNumber], [EmailAddress], [Address], [City], [PasswordHash], [RegistrationDate], [AccountStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.DonorName.Trim(),
            model.DonorType.Trim(),
            model.ContactPerson.Trim(),
            model.MobileNumber.Trim(),
            model.EmailAddress.Trim().ToLower(),
            model.Address.Trim(),
            model.City.Trim(),
            model.PasswordHash,
            DateTime.Now,
            "Active",
            If(String.IsNullOrEmpty(model.ProfileImage), "~/Content/images/avatar-donor.svg", model.ProfileImage)
        })
    End Function

    Public Function AuthenticateDonor(ByVal email As String, ByVal passwordHash As String) As Donor
        Dim sql As String = "SELECT * FROM [Donors] WHERE LCase([EmailAddress]) = LCase(?) AND [PasswordHash] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {email.Trim(), passwordHash})
        If dt.Rows.Count > 0 Then
            Return MapDonor(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function GetDonorById(ByVal donorId As Integer) As Donor
        Dim sql As String = "SELECT * FROM [Donors] WHERE [DonorID] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {donorId})
        If dt.Rows.Count > 0 Then
            Return MapDonor(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function UpdateDonorProfile(ByVal model As Donor) As Boolean
        Dim sql As String = "UPDATE [Donors] SET [DonorName] = ?, [DonorType] = ?, [ContactPerson] = ?, [MobileNumber] = ?, [Address] = ?, [City] = ?, [ProfileImage] = ? WHERE [DonorID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.DonorName.Trim(),
            model.DonorType.Trim(),
            model.ContactPerson.Trim(),
            model.MobileNumber.Trim(),
            model.Address.Trim(),
            model.City.Trim(),
            model.ProfileImage,
            model.DonorID
        }) > 0
    End Function

    Public Function ChangePassword(ByVal donorId As Integer, ByVal oldHash As String, ByVal newHash As String) As Boolean
        Dim sql As String = "UPDATE [Donors] SET [PasswordHash] = ? WHERE [DonorID] = ? AND [PasswordHash] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {newHash, donorId, oldHash}) > 0
    End Function

    Public Function GetAllDonors(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        Dim sql As String = "SELECT d.*, (SELECT COUNT(*) FROM [FoodDonations] fd WHERE fd.[DonorID] = d.[DonorID]) AS [TotalDonations] FROM [Donors] d WHERE 1=1"
        Dim params As New List(Of Object)()

        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (d.[DonorName] LIKE ? OR d.[ContactPerson] LIKE ? OR d.[EmailAddress] LIKE ? OR d.[MobileNumber] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND d.[City] = ?"
            params.Add(city)
        End If
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
            sql &= " AND d.[AccountStatus] = ?"
            params.Add(status)
        End If
        sql &= " ORDER BY d.[DonorID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function UpdateAccountStatus(ByVal donorId As Integer, ByVal status As String) As Boolean
        Dim sql As String = "UPDATE [Donors] SET [AccountStatus] = ? WHERE [DonorID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {status, donorId}) > 0
    End Function

    Public Function DeleteDonor(ByVal donorId As Integer) As Boolean
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ?", New Object() {donorId}))
        If count > 0 Then
            Return UpdateAccountStatus(donorId, "Inactive")
        End If
        Return DBConnection.ExecuteNonQuery("DELETE FROM [Donors] WHERE [DonorID] = ?", New Object() {donorId}) > 0
    End Function

    Public Function GetDonorDashboardStats(ByVal donorId As Integer) As Dictionary(Of String, Object)
        DBConnection.SyncExpiredDonations()
        Dim stats As New Dictionary(Of String, Object)()
        stats("TotalDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ?", New Object() {donorId}))
        stats("PendingDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ? AND [ApprovalStatus] = 'Pending'", New Object() {donorId}))
        stats("ApprovedDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ? AND [ApprovalStatus] IN ('Approved', 'Assigned')", New Object() {donorId}))
        stats("CollectedDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ? AND [CollectionStatus] IN ('Collected', 'Food Collected', 'Out for Delivery')", New Object() {donorId}))
        stats("DeliveredDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [DonorID] = ? AND ([ApprovalStatus] = 'Delivered' OR [CollectionStatus] = 'Delivered')", New Object() {donorId}))
        Dim totalQtyObj As Object = DBConnection.ExecuteScalar("SELECT SUM([FoodQuantity]) FROM [FoodDonations] WHERE [DonorID] = ? AND [ApprovalStatus] <> 'Cancelled' AND [ApprovalStatus] <> 'Rejected'", New Object() {donorId})
        stats("TotalQuantityDonated") = If(totalQtyObj Is Nothing OrElse IsDBNull(totalQtyObj), 0.0, Convert.ToDouble(totalQtyObj))
        Dim totalPeopleObj As Object = DBConnection.ExecuteScalar("SELECT SUM([NumberOfPersonsServed]) FROM [FoodDonations] WHERE [DonorID] = ? AND [ApprovalStatus] <> 'Cancelled' AND [ApprovalStatus] <> 'Rejected'", New Object() {donorId})
        stats("TotalPeopleServed") = If(totalPeopleObj Is Nothing OrElse IsDBNull(totalPeopleObj), 0, Convert.ToInt32(totalPeopleObj))
        Return stats
    End Function

    Private Function MapDonor(ByVal row As DataRow) As Donor
        Dim d As New Donor()
        d.DonorID = Convert.ToInt32(row("DonorID"))
        d.DonorName = Convert.ToString(row("DonorName"))
        d.DonorType = Convert.ToString(row("DonorType"))
        d.ContactPerson = Convert.ToString(row("ContactPerson"))
        d.MobileNumber = Convert.ToString(row("MobileNumber"))
        d.EmailAddress = Convert.ToString(row("EmailAddress"))
        d.Address = Convert.ToString(row("Address"))
        d.City = Convert.ToString(row("City"))
        d.PasswordHash = Convert.ToString(row("PasswordHash"))
        If Not IsDBNull(row("RegistrationDate")) Then d.RegistrationDate = Convert.ToDateTime(row("RegistrationDate"))
        d.AccountStatus = Convert.ToString(row("AccountStatus"))
        d.ProfileImage = If(IsDBNull(row("ProfileImage")), "~/Content/images/avatar-donor.svg", Convert.ToString(row("ProfileImage")))
        Return d
    End Function
End Class
