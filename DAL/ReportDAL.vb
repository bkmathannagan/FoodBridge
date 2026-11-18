Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class ReportDAL
    Public Function AuthenticateAdmin(ByVal email As String, ByVal passwordHash As String) As DataRow
        Dim sql As String = "SELECT * FROM [Admin] WHERE LCase([EmailAddress]) = LCase(?) AND [PasswordHash] = ? AND [IsActive] = True"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {email.Trim(), passwordHash})
        If dt.Rows.Count > 0 Then
            DBConnection.ExecuteNonQuery("UPDATE [Admin] SET [LastLogin] = ? WHERE [AdminID] = ?", New Object() {DateTime.Now, dt.Rows(0)("AdminID")})
            Return dt.Rows(0)
        End If
        Return Nothing
    End Function

    Public Function ChangeAdminPassword(ByVal adminId As Integer, ByVal oldHash As String, ByVal newHash As String) As Boolean
        Dim sql As String = "UPDATE [Admin] SET [PasswordHash] = ? WHERE [AdminID] = ? AND [PasswordHash] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {newHash, adminId, oldHash}) > 0
    End Function

    Public Function ResetUserPasswordByEmail(ByVal role As String, ByVal email As String, ByVal mobile As String, ByVal newHash As String) As Boolean
        Dim sql As String = ""
        Select Case role
            Case "Donor"
                sql = "UPDATE [Donors] SET [PasswordHash] = ? WHERE LCase([EmailAddress]) = LCase(?) AND [MobileNumber] = ?"
            Case "NGO"
                sql = "UPDATE [NGOs] SET [PasswordHash] = ? WHERE LCase([EmailAddress]) = LCase(?) AND [MobileNumber] = ?"
            Case "Volunteer"
                sql = "UPDATE [Volunteers] SET [PasswordHash] = ? WHERE LCase([EmailAddress]) = LCase(?) AND [MobileNumber] = ?"
            Case "Admin"
                sql = "UPDATE [Admin] SET [PasswordHash] = ? WHERE LCase([EmailAddress]) = LCase(?) AND [MobileNumber] = ?"
            Case Else
                Return False
        End Select
        Dim updated As Integer = DBConnection.ExecuteNonQuery(sql, New Object() {newHash, email.Trim(), mobile.Trim()})
        If updated > 0 Then
            Try
                DBConnection.ExecuteNonQuery("INSERT INTO [PasswordReset] ([EmailAddress], [UserRole], [ResetToken], [RequestDate], [ExpiryDate], [IsUsed]) VALUES (?, ?, ?, ?, ?, True)", New Object() {email.Trim(), role, Guid.NewGuid().ToString("N"), DateTime.Now, DateTime.Now.AddHours(1)})
            Catch
            End Try
        End If
        Return updated > 0
    End Function

    Public Function GetAdminDashboardStats() As Dictionary(Of String, Object)
        DBConnection.SyncExpiredDonations()
        Dim s As New Dictionary(Of String, Object)()
        s("TotalDonors") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [Donors]"))
        s("TotalNGOs") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [NGOs]"))
        s("TotalVolunteers") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [Volunteers]"))
        s("TotalDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations]"))
        s("PendingDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [ApprovalStatus] = 'Pending'"))
        s("ApprovedDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [ApprovalStatus] = 'Approved'"))
        Dim rescuedObj As Object = DBConnection.ExecuteScalar("SELECT SUM([DeliveredQuantity]) FROM [FoodDelivery]")
        Dim rescuedKg As Double = If(rescuedObj Is Nothing OrElse IsDBNull(rescuedObj), 0.0, Convert.ToDouble(rescuedObj))
        Dim allocObj As Object = DBConnection.ExecuteScalar("SELECT SUM([FoodQuantity]) FROM [FoodDonations] WHERE [ApprovalStatus] IN ('Approved', 'Delivered')")
        Dim totalFoodKg As Double = If(allocObj Is Nothing OrElse IsDBNull(allocObj), 0.0, Convert.ToDouble(allocObj))
        s("TotalFoodRescuedKg") = If(rescuedKg > 0, rescuedKg, totalFoodKg)
        s("CompletedDeliveries") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [TaskStatus] = 'Successfully Delivered'"))
        s("PendingRequests") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodRequests] WHERE [RequestStatus] = 'Pending'"))
        s("ExpiredDonations") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [FoodDonations] WHERE [ApprovalStatus] = 'Expired'"))
        Dim peopleObj As Object = DBConnection.ExecuteScalar("SELECT SUM([NumberOfPersonsServed]) FROM [FoodDonations] WHERE [ApprovalStatus] IN ('Approved', 'Delivered')")
        s("TotalPeopleFed") = If(peopleObj Is Nothing OrElse IsDBNull(peopleObj), 0, Convert.ToInt32(peopleObj))
        Return s
    End Function

    Public Function GetDonationStatusBreakdown() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT [ApprovalStatus] AS [StatusName], COUNT(*) AS [TotalCount], SUM([FoodQuantity]) AS [TotalQuantity] FROM [FoodDonations] GROUP BY [ApprovalStatus]")
    End Function

    Public Function GetFoodCategoryBreakdown() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT [FoodCategory], COUNT(*) AS [DonationCount], SUM([FoodQuantity]) AS [TotalQuantity] FROM [FoodDonations] GROUP BY [FoodCategory] ORDER BY COUNT(*) DESC")
    End Function

    Public Function GetMonthlyRescueSummary() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT Format([DonationDate], 'yyyy-mm') AS [MonthPeriod], COUNT(*) AS [TotalDonations], SUM([FoodQuantity]) AS [TotalQuantityKg], SUM([NumberOfPersonsServed]) AS [TotalBeneficiaries] FROM [FoodDonations] GROUP BY Format([DonationDate], 'yyyy-mm') ORDER BY Format([DonationDate], 'yyyy-mm') DESC")
    End Function

    Public Function GenerateReport(ByVal reportType As String, ByVal fromDate As Nullable(Of DateTime), ByVal toDate As Nullable(Of DateTime), ByVal status As String, ByVal category As String, ByVal city As String, ByVal search As String) As DataTable
        DBConnection.SyncExpiredDonations()
        Dim sql As String = ""
        Dim params As New List(Of Object)()

        Select Case reportType
            Case "DonorRegistration"
                sql = "SELECT [DonorID], [DonorName], [DonorType], [ContactPerson], [MobileNumber], [EmailAddress], [City], [RegistrationDate], [AccountStatus] FROM [Donors] WHERE 1=1"
                If fromDate.HasValue Then
                    sql &= " AND [RegistrationDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND [RegistrationDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
                    sql &= " AND [AccountStatus] = ?"
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
                    sql &= " AND [City] = ?"
                    params.Add(city)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND ([DonorName] LIKE ? OR [EmailAddress] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY [DonorID] DESC"

            Case "NGORegistration"
                sql = "SELECT [NGOID], [OrganizationName], [RegistrationNumber], [ContactPerson], [MobileNumber], [EmailAddress], [City], [NumberOfBeneficiaries], [RegistrationDate], [VerificationStatus] FROM [NGOs] WHERE 1=1"
                If fromDate.HasValue Then
                    sql &= " AND [RegistrationDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND [RegistrationDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
                    sql &= " AND [VerificationStatus] = ?"
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
                    sql &= " AND [City] = ?"
                    params.Add(city)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND ([OrganizationName] LIKE ? OR [RegistrationNumber] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY [NGOID] DESC"

            Case "VolunteerRegistration"
                sql = "SELECT [VolunteerID], [VolunteerName], [Gender], [MobileNumber], [EmailAddress], [City], [AvailabilityStatus], [RegistrationDate], [AccountStatus] FROM [Volunteers] WHERE 1=1"
                If fromDate.HasValue Then
                    sql &= " AND [RegistrationDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND [RegistrationDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
                    sql &= " AND ([AccountStatus] = ? OR [AvailabilityStatus] = ?)"
                    params.Add(status)
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
                    sql &= " AND [City] = ?"
                    params.Add(city)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND ([VolunteerName] LIKE ? OR [EmailAddress] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY [VolunteerID] DESC"

            Case "FoodDonation", "PendingDonation", "ApprovedDonation", "RejectedDonation", "ExpiredFood"
                sql = "SELECT fd.[DonationID], d.[DonorName], fd.[FoodName], fd.[FoodCategory], fd.[FoodType], fd.[FoodQuantity] AS [Quantity], fd.[AvailableQuantity], fd.[QuantityUnit], fd.[NumberOfPersonsServed] AS [Beneficiaries], fd.[City], fd.[DonationDate], fd.[ExpiryDate], fd.[ApprovalStatus], fd.[CollectionStatus] FROM [FoodDonations] fd INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE 1=1"
                If reportType = "PendingDonation" Then sql &= " AND fd.[ApprovalStatus] = 'Pending'"
                If reportType = "ApprovedDonation" Then sql &= " AND fd.[ApprovalStatus] = 'Approved'"
                If reportType = "RejectedDonation" Then sql &= " AND fd.[ApprovalStatus] = 'Rejected'"
                If reportType = "ExpiredFood" Then sql &= " AND fd.[ApprovalStatus] = 'Expired'"

                If fromDate.HasValue Then
                    sql &= " AND fd.[DonationDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND fd.[DonationDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" AndAlso reportType = "FoodDonation" Then
                    sql &= " AND fd.[ApprovalStatus] = ?"
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
                    sql &= " AND fd.[FoodCategory] = ?"
                    params.Add(category)
                End If
                If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
                    sql &= " AND fd.[City] = ?"
                    params.Add(city)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND (fd.[FoodName] LIKE ? OR d.[DonorName] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY fd.[DonationID] DESC"

            Case "FoodRequest"
                sql = "SELECT fr.[RequestID], n.[OrganizationName] AS [NGOName], fd.[FoodName], fd.[FoodCategory], fr.[RequestedQuantity] AS [Quantity], fd.[QuantityUnit], fr.[NumberOfBeneficiaries] AS [Beneficiaries], fr.[City], fr.[RequestDate], fr.[RequiredDate], fr.[RequestStatus] FROM ([FoodRequests] fr INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID]) INNER JOIN [FoodDonations] fd ON fr.[DonationID] = fd.[DonationID] WHERE 1=1"
                If fromDate.HasValue Then
                    sql &= " AND fr.[RequestDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND fr.[RequestDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
                    sql &= " AND fr.[RequestStatus] = ?"
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
                    sql &= " AND fd.[FoodCategory] = ?"
                    params.Add(category)
                End If
                If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
                    sql &= " AND fr.[City] = ?"
                    params.Add(city)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND (n.[OrganizationName] LIKE ? OR fd.[FoodName] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY fr.[RequestID] DESC"

            Case "VolunteerAssignment", "FoodCollection"
                sql = "SELECT va.[TaskID], v.[VolunteerName], fd.[FoodName], d.[DonorName], n.[OrganizationName] AS [NGOName], va.[AssignedQuantity] AS [Quantity], va.[QuantityUnit], va.[AssignmentDate], va.[CollectionDate], va.[DeliveryDate], va.[TaskStatus], va.[CollectionRemarks] FROM (((([VolunteerAssignments] va INNER JOIN [Volunteers] v ON va.[VolunteerID] = v.[VolunteerID]) INNER JOIN [FoodDonations] fd ON va.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID]) INNER JOIN [FoodRequests] fr ON va.[RequestID] = fr.[RequestID]) INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID] WHERE 1=1"
                If reportType = "FoodCollection" Then
                    sql &= " AND va.[TaskStatus] IN ('Food Collected', 'Out for Delivery', 'Successfully Delivered')"
                End If
                If fromDate.HasValue Then
                    sql &= " AND va.[AssignmentDate] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND va.[AssignmentDate] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
                    sql &= " AND va.[TaskStatus] = ?"
                    params.Add(status)
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND (v.[VolunteerName] LIKE ? OR fd.[FoodName] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY va.[TaskID] DESC"

            Case "FoodDelivery"
                sql = "SELECT del.[DeliveryID], del.[TaskID], fd.[FoodName], v.[VolunteerName], n.[OrganizationName] AS [NGOName], del.[DeliveredQuantity] AS [Quantity], del.[QuantityUnit], del.[ReceivedByPerson], del.[DeliveryCondition], del.[DeliveryTimestamp] FROM ((([FoodDelivery] del INNER JOIN [FoodDonations] fd ON del.[DonationID] = fd.[DonationID]) INNER JOIN [Volunteers] v ON del.[VolunteerID] = v.[VolunteerID]) INNER JOIN [NGOs] n ON del.[NGOID] = n.[NGOID]) WHERE 1=1"
                If fromDate.HasValue Then
                    sql &= " AND del.[DeliveryTimestamp] >= ?"
                    params.Add(fromDate.Value.Date)
                End If
                If toDate.HasValue Then
                    sql &= " AND del.[DeliveryTimestamp] <= ?"
                    params.Add(toDate.Value.Date.AddDays(1).AddSeconds(-1))
                End If
                If Not String.IsNullOrEmpty(search) Then
                    sql &= " AND (n.[OrganizationName] LIKE ? OR v.[VolunteerName] LIKE ? OR fd.[FoodName] LIKE ?)"
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                    params.Add("%" & search.Trim() & "%")
                End If
                sql &= " ORDER BY del.[DeliveryID] DESC"

            Case "FoodCategory"
                Return GetFoodCategoryBreakdown()

            Case "MonthlyFoodRescue"
                Return GetMonthlyRescueSummary()

            Case Else ' OverallFoodRescueSummary
                sql = "SELECT fd.[DonationID], d.[DonorName], fd.[FoodName], fd.[FoodCategory], fd.[FoodQuantity] AS [Quantity], fd.[AvailableQuantity], fd.[QuantityUnit], fd.[NumberOfPersonsServed] AS [Beneficiaries], fd.[City], fd.[DonationDate], fd.[ApprovalStatus], fd.[CollectionStatus] FROM [FoodDonations] fd INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] ORDER BY fd.[DonationID] DESC"
        End Select

        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function
End Class
