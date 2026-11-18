Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class NGODAL
    Public Function EmailOrRegExists(ByVal email As String, ByVal regNo As String, Optional ByVal excludeNgoId As Integer = 0) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM [NGOs] WHERE (LCase([EmailAddress]) = LCase(?) OR LCase([RegistrationNumber]) = LCase(?)) AND [NGOID] <> ?"
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar(sql, New Object() {email.Trim(), regNo.Trim(), excludeNgoId}))
        Return count > 0
    End Function

    Public Function RegisterNGO(ByVal model As NGO) As Integer
        Dim sql As String = "INSERT INTO [NGOs] ([OrganizationName], [RegistrationNumber], [ContactPerson], [MobileNumber], [EmailAddress], [Address], [City], [NumberOfBeneficiaries], [PasswordHash], [RegistrationDate], [VerificationStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.OrganizationName.Trim(),
            model.RegistrationNumber.Trim(),
            model.ContactPerson.Trim(),
            model.MobileNumber.Trim(),
            model.EmailAddress.Trim().ToLower(),
            model.Address.Trim(),
            model.City.Trim(),
            model.NumberOfBeneficiaries,
            model.PasswordHash,
            DateTime.Now,
            If(String.IsNullOrEmpty(model.VerificationStatus), "Pending", model.VerificationStatus),
            If(String.IsNullOrEmpty(model.ProfileImage), "~/Content/images/avatar-ngo.svg", model.ProfileImage)
        })
    End Function

    Public Function AuthenticateNGO(ByVal email As String, ByVal passwordHash As String) As NGO
        Dim sql As String = "SELECT * FROM [NGOs] WHERE LCase([EmailAddress]) = LCase(?) AND [PasswordHash] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {email.Trim(), passwordHash})
        If dt.Rows.Count > 0 Then
            Return MapNGO(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function GetNGOById(ByVal ngoId As Integer) As NGO
        Dim sql As String = "SELECT * FROM [NGOs] WHERE [NGOID] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {ngoId})
        If dt.Rows.Count > 0 Then
            Return MapNGO(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function UpdateNGOProfile(ByVal model As NGO) As Boolean
        Dim sql As String = "UPDATE [NGOs] SET [OrganizationName] = ?, [RegistrationNumber] = ?, [ContactPerson] = ?, [MobileNumber] = ?, [Address] = ?, [City] = ?, [NumberOfBeneficiaries] = ?, [ProfileImage] = ? WHERE [NGOID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.OrganizationName.Trim(),
            model.RegistrationNumber.Trim(),
            model.ContactPerson.Trim(),
            model.MobileNumber.Trim(),
            model.Address.Trim(),
            model.City.Trim(),
            model.NumberOfBeneficiaries,
            model.ProfileImage,
            model.NGOID
        }) > 0
    End Function

    Public Function ChangePassword(ByVal ngoId As Integer, ByVal oldHash As String, ByVal newHash As String) As Boolean
        Dim sql As String = "UPDATE [NGOs] SET [PasswordHash] = ? WHERE [NGOID] = ? AND [PasswordHash] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {newHash, ngoId, oldHash}) > 0
    End Function

    Public Function GetAllNGOs(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        Dim sql As String = "SELECT n.*, (SELECT COUNT(*) FROM [FoodRequests] fr WHERE fr.[NGOID] = n.[NGOID]) AS [TotalRequests] FROM [NGOs] n WHERE 1=1"
        Dim params As New List(Of Object)()

        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (n.[OrganizationName] LIKE ? OR n.[RegistrationNumber] LIKE ? OR n.[ContactPerson] LIKE ? OR n.[EmailAddress] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND n.[City] = ?"
            params.Add(city)
        End If
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
            sql &= " AND n.[VerificationStatus] = ?"
            params.Add(status)
        End If
        sql &= " ORDER BY n.[NGOID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function UpdateVerificationStatus(ByVal ngoId As Integer, ByVal status As String) As Boolean
        Dim sql As String = "UPDATE [NGOs] SET [VerificationStatus] = ? WHERE [NGOID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {status, ngoId}) > 0
    End Function

    Public Function DeleteNGO(ByVal ngoId As Integer) As Boolean
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ?", New Object() {ngoId}))
        If count > 0 Then
            Return UpdateVerificationStatus(ngoId, "Blocked")
        End If
        Return DBConnection.ExecuteNonQuery("DELETE FROM [NGOs] WHERE [NGOID] = ?", New Object() {ngoId}) > 0
    End Function

    Public Function GetNGODashboardStats(ByVal ngoId As Integer) As Dictionary(Of String, Object)
        DBConnection.SyncExpiredDonations()
        Dim stats As New Dictionary(Of String, Object)()
        Dim todayStr As String = DateTime.Today.ToString("yyyy-MM-dd")
        stats("AvailableFoodDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [ApprovalStatus] = 'Approved' AND [AvailableQuantity] > 0 AND [ExpiryDate] >= ?", New Object() {todayStr}))
        stats("TotalRequests") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ?", New Object() {ngoId}))
        stats("PendingRequests") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ? AND [RequestStatus] = 'Pending'", New Object() {ngoId}))
        stats("ApprovedRequests") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ? AND [RequestStatus] IN ('Approved', 'Assigned', 'Processing')", New Object() {ngoId}))
        stats("ReceivedDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ? AND [RequestStatus] = 'Delivered'", New Object() {ngoId}))
        Dim recQtyObj As Object = DBConnection.ExecuteScalar("SELECT SUM([RequestedQuantity]) FROM [FoodRequests] WHERE [NGOID] = ? AND [RequestStatus] = 'Delivered'", New Object() {ngoId})
        stats("TotalQuantityReceived") = If(recQtyObj Is Nothing OrElse IsDBNull(recQtyObj), 0.0, Convert.ToDouble(recQtyObj))
        Return stats
    End Function

    Private Function MapNGO(ByVal row As DataRow) As NGO
        Dim n As New NGO()
        n.NGOID = Convert.ToInt32(row("NGOID"))
        n.OrganizationName = Convert.ToString(row("OrganizationName"))
        n.RegistrationNumber = Convert.ToString(row("RegistrationNumber"))
        n.ContactPerson = Convert.ToString(row("ContactPerson"))
        n.MobileNumber = Convert.ToString(row("MobileNumber"))
        n.EmailAddress = Convert.ToString(row("EmailAddress"))
        n.Address = Convert.ToString(row("Address"))
        n.City = Convert.ToString(row("City"))
        n.NumberOfBeneficiaries = If(IsDBNull(row("NumberOfBeneficiaries")), 0, Convert.ToInt32(row("NumberOfBeneficiaries")))
        n.PasswordHash = Convert.ToString(row("PasswordHash"))
        If Not IsDBNull(row("RegistrationDate")) Then n.RegistrationDate = Convert.ToDateTime(row("RegistrationDate"))
        n.VerificationStatus = Convert.ToString(row("VerificationStatus"))
        n.ProfileImage = If(IsDBNull(row("ProfileImage")), "~/Content/images/avatar-ngo.svg", Convert.ToString(row("ProfileImage")))
        Return n
    End Function
End Class
