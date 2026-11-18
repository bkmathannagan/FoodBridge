Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class DonationDAL
    Public Function AddDonation(ByVal model As FoodDonation, ByVal actorName As String) As Integer
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    Dim sql As String = "INSERT INTO [FoodDonations] ([DonorID], [FoodName], [FoodCategory], [FoodDescription], [FoodType], [FoodQuantity], [AvailableQuantity], [QuantityUnit], [NumberOfPersonsServed], [FoodPreparationDate], [FoodPreparationTime], [ExpiryDate], [ExpiryTime], [PickupAddress], [City], [PINCode], [PreferredPickupDate], [PreferredPickupTime], [SpecialInstructions], [FoodImage], [DonationDate], [ApprovalStatus], [CollectionStatus], [AdminRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
                    Using cmd As New OleDbCommand(sql, conn, trans)
                        DBConnection.AddParam(cmd, "@p1", model.DonorID)
                        DBConnection.AddParam(cmd, "@p2", model.FoodName.Trim())
                        DBConnection.AddParam(cmd, "@p3", model.FoodCategory.Trim())
                        DBConnection.AddParam(cmd, "@p4", model.FoodDescription.Trim())
                        DBConnection.AddParam(cmd, "@p5", model.FoodType.Trim())
                        DBConnection.AddParam(cmd, "@p6", model.FoodQuantity)
                        DBConnection.AddParam(cmd, "@p7", model.FoodQuantity)
                        DBConnection.AddParam(cmd, "@p8", model.QuantityUnit.Trim())
                        DBConnection.AddParam(cmd, "@p9", model.NumberOfPersonsServed)
                        DBConnection.AddParam(cmd, "@p10", model.FoodPreparationDate.ToString("yyyy-MM-dd"))
                        DBConnection.AddParam(cmd, "@p11", model.FoodPreparationTime.Trim())
                        DBConnection.AddParam(cmd, "@p12", model.ExpiryDate.ToString("yyyy-MM-dd"))
                        DBConnection.AddParam(cmd, "@p13", model.ExpiryTime.Trim())
                        DBConnection.AddParam(cmd, "@p14", model.PickupAddress.Trim())
                        DBConnection.AddParam(cmd, "@p15", model.City.Trim())
                        DBConnection.AddParam(cmd, "@p16", model.PINCode.Trim())
                        DBConnection.AddParam(cmd, "@p17", model.PreferredPickupDate.ToString("yyyy-MM-dd"))
                        DBConnection.AddParam(cmd, "@p18", model.PreferredPickupTime.Trim())
                        DBConnection.AddParam(cmd, "@p19", model.SpecialInstructions.Trim())
                        DBConnection.AddParam(cmd, "@p20", If(String.IsNullOrEmpty(model.FoodImage), "~/Content/images/food-meals.svg", model.FoodImage))
                        DBConnection.AddParam(cmd, "@p21", DateTime.Now)
                        DBConnection.AddParam(cmd, "@p22", "Pending")
                        DBConnection.AddParam(cmd, "@p23", "Pending")
                        DBConnection.AddParam(cmd, "@p24", "Awaiting Administrator Verification")
                        cmd.ExecuteNonQuery()
                    End Using

                    Dim newId As Integer = 0
                    Using idCmd As New OleDbCommand("SELECT @@IDENTITY", conn, trans)
                        newId = Convert.ToInt32(idCmd.ExecuteScalar())
                    End Using

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, 0, 0, 'Donor', ?, 'New', 'Pending', ?, 'Surplus food donation submitted by donor.', ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", newId)
                        DBConnection.AddParam(logCmd, "@p2", actorName)
                        DBConnection.AddParam(logCmd, "@p3", model.FoodQuantity)
                        DBConnection.AddParam(logCmd, "@p4", DateTime.Now)
                        logCmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    Return newId
                Catch
                    trans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function UpdateDonationBeforeApproval(ByVal model As FoodDonation, ByVal actorName As String) As Boolean
        Dim sql As String = "UPDATE [FoodDonations] SET [FoodName] = ?, [FoodCategory] = ?, [FoodDescription] = ?, [FoodType] = ?, [FoodQuantity] = ?, [AvailableQuantity] = ?, [QuantityUnit] = ?, [NumberOfPersonsServed] = ?, [FoodPreparationDate] = ?, [FoodPreparationTime] = ?, [ExpiryDate] = ?, [ExpiryTime] = ?, [PickupAddress] = ?, [City] = ?, [PINCode] = ?, [PreferredPickupDate] = ?, [PreferredPickupTime] = ?, [SpecialInstructions] = ?, [FoodImage] = ? WHERE [DonationID] = ? AND [DonorID] = ? AND [ApprovalStatus] = 'Pending'"
        Dim rows As Integer = DBConnection.ExecuteNonQuery(sql, New Object() {
            model.FoodName.Trim(),
            model.FoodCategory.Trim(),
            model.FoodDescription.Trim(),
            model.FoodType.Trim(),
            model.FoodQuantity,
            model.FoodQuantity,
            model.QuantityUnit.Trim(),
            model.NumberOfPersonsServed,
            model.FoodPreparationDate.ToString("yyyy-MM-dd"),
            model.FoodPreparationTime.Trim(),
            model.ExpiryDate.ToString("yyyy-MM-dd"),
            model.ExpiryTime.Trim(),
            model.PickupAddress.Trim(),
            model.City.Trim(),
            model.PINCode.Trim(),
            model.PreferredPickupDate.ToString("yyyy-MM-dd"),
            model.PreferredPickupTime.Trim(),
            model.SpecialInstructions.Trim(),
            model.FoodImage,
            model.DonationID,
            model.DonorID
        })
        If rows > 0 Then
            DBConnection.LogTransaction(model.DonationID, 0, 0, "Donor", actorName, "Pending", "Pending (Edited)", model.FoodQuantity, "Donation details updated before admin approval.")
        End If
        Return rows > 0
    End Function

    Public Function CancelDonationByDonor(ByVal donationId As Integer, ByVal donorId As Integer, ByVal actorName As String) As Boolean
        Dim sql As String = "UPDATE [FoodDonations] SET [ApprovalStatus] = 'Cancelled', [CollectionStatus] = 'Cancelled' WHERE [DonationID] = ? AND [DonorID] = ? AND [ApprovalStatus] IN ('Pending', 'Approved')"
        Dim rows As Integer = DBConnection.ExecuteNonQuery(sql, New Object() {donationId, donorId})
        If rows > 0 Then
            DBConnection.LogTransaction(donationId, 0, 0, "Donor", actorName, "Pending/Approved", "Cancelled", 0, "Donation cancelled by donor.")
        End If
        Return rows > 0
    End Function

    Public Function UpdateApprovalStatus(ByVal donationId As Integer, ByVal newStatus As String, ByVal remarks As String, ByVal adminName As String) As Boolean
        Dim colStatus As String = If(newStatus = "Rejected" OrElse newStatus = "Cancelled", newStatus, "Pending")
        Dim sql As String = "UPDATE [FoodDonations] SET [ApprovalStatus] = ?, [CollectionStatus] = ?, [AdminRemarks] = ? WHERE [DonationID] = ?"
        Dim rows As Integer = DBConnection.ExecuteNonQuery(sql, New Object() {newStatus, colStatus, remarks, donationId})
        If rows > 0 Then
            DBConnection.LogTransaction(donationId, 0, 0, "Admin", adminName, "Pending", newStatus, 0, remarks)
        End If
        Return rows > 0
    End Function

    Public Function GetDonationById(ByVal donationId As Integer) As FoodDonation
        DBConnection.SyncExpiredDonations()
        Dim sql As String = "SELECT fd.*, d.[DonorName], d.[MobileNumber] AS [DonorMobile], d.[DonorType] FROM [FoodDonations] fd INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE fd.[DonationID] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {donationId})
        If dt.Rows.Count > 0 Then
            Return MapDonation(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function GetDonationsByDonor(ByVal donorId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        DBConnection.SyncExpiredDonations()
        Dim sql As String = "SELECT fd.*, (SELECT TOP 1 v.[VolunteerName] & ' (' & v.[MobileNumber] & ')' FROM [VolunteerAssignments] va INNER JOIN [Volunteers] v ON va.[VolunteerID] = v.[VolunteerID] WHERE va.[DonationID] = fd.[DonationID] ORDER BY va.[TaskID] DESC) AS [AssignedVolunteerInfo] FROM [FoodDonations] fd WHERE fd.[DonorID] = ?"
        Dim params As New List(Of Object)()
        params.Add(donorId)
        If Not String.IsNullOrEmpty(statusFilter) AndAlso statusFilter <> "All" Then
            sql &= " AND (fd.[ApprovalStatus] = ? OR fd.[CollectionStatus] = ?)"
            params.Add(statusFilter)
            params.Add(statusFilter)
        End If
        sql &= " ORDER BY fd.[DonationID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetAvailableDonationsForNGO(Optional ByVal category As String = "", Optional ByVal city As String = "", Optional ByVal foodType As String = "", Optional ByVal search As String = "") As DataTable
        DBConnection.SyncExpiredDonations()
        Dim todayStr As String = DateTime.Today.ToString("yyyy-MM-dd")
        Dim tomorrowStr As String = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")
        Dim sql As String = "SELECT fd.*, d.[DonorName], d.[DonorType], d.[MobileNumber] AS [DonorMobile], IIf(fd.[ExpiryDate] <= #" & tomorrowStr & "#, 'URGENT', 'Standard') AS [UrgencyTag] FROM [FoodDonations] fd INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE fd.[ApprovalStatus] = 'Approved' AND fd.[AvailableQuantity] > 0 AND fd.[ExpiryDate] >= ?"
        Dim params As New List(Of Object)()
        params.Add(todayStr)

        If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
            sql &= " AND fd.[FoodCategory] = ?"
            params.Add(category)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND fd.[City] = ?"
            params.Add(city)
        End If
        If Not String.IsNullOrEmpty(foodType) AndAlso foodType <> "All" Then
            sql &= " AND fd.[FoodType] = ?"
            params.Add(foodType)
        End If
        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (fd.[FoodName] LIKE ? OR fd.[FoodDescription] LIKE ? OR fd.[PickupAddress] LIKE ? OR d.[DonorName] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        sql &= " ORDER BY fd.[ExpiryDate] ASC, fd.[DonationID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetAllDonationsForAdmin(Optional ByVal search As String = "", Optional ByVal category As String = "", Optional ByVal city As String = "", Optional ByVal status As String = "") As DataTable
        DBConnection.SyncExpiredDonations()
        Dim tomorrowStr As String = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")
        Dim sql As String = "SELECT fd.*, d.[DonorName], d.[DonorType], d.[MobileNumber] AS [DonorMobile], IIf(fd.[ExpiryDate] <= #" & tomorrowStr & "# AND fd.[ApprovalStatus] IN ('Pending','Approved'), 'URGENT', 'Normal') AS [UrgencyTag] FROM [FoodDonations] fd INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE 1=1"
        Dim params As New List(Of Object)()

        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (fd.[FoodName] LIKE ? OR d.[DonorName] LIKE ? OR fd.[PickupAddress] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        If Not String.IsNullOrEmpty(category) AndAlso category <> "All" Then
            sql &= " AND fd.[FoodCategory] = ?"
            params.Add(category)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND fd.[City] = ?"
            params.Add(city)
        End If
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
            sql &= " AND (fd.[ApprovalStatus] = ? OR fd.[CollectionStatus] = ?)"
            params.Add(status)
            params.Add(status)
        End If
        sql &= " ORDER BY fd.[DonationID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetDonationTimeline(ByVal donationId As Integer) As DataTable
        Dim sql As String = "SELECT * FROM [DonationTransactions] WHERE [DonationID] = ? ORDER BY [TransactionID] ASC"
        Return DBConnection.ExecuteDataTable(sql, New Object() {donationId})
    End Function

    ' Categories & Cities Management
    Public Function GetAllCategories(Optional ByVal activeOnly As Boolean = True) As DataTable
        Dim sql As String = "SELECT * FROM [FoodCategories]"
        If activeOnly Then sql &= " WHERE [IsActive] = True"
        sql &= " ORDER BY [CategoryName] ASC"
        Return DBConnection.ExecuteDataTable(sql)
    End Function

    Public Function SaveCategory(ByVal catId As Integer, ByVal name As String, ByVal desc As String, ByVal isActive As Boolean) As Boolean
        If catId = 0 Then
            Return DBConnection.ExecuteNonQuery("INSERT INTO [FoodCategories] ([CategoryName], [Description], [IsActive]) VALUES (?, ?, ?)", New Object() {name.Trim(), desc.Trim(), isActive}) > 0
        Else
            Return DBConnection.ExecuteNonQuery("UPDATE [FoodCategories] SET [CategoryName] = ?, [Description] = ?, [IsActive] = ? WHERE [CategoryID] = ?", New Object() {name.Trim(), desc.Trim(), isActive, catId}) > 0
        End If
    End Function

    Public Function DeleteCategory(ByVal catId As Integer) As Boolean
        Return DBConnection.ExecuteNonQuery("DELETE FROM [FoodCategories] WHERE [CategoryID] = ?", New Object() {catId}) > 0
    End Function

    Public Function GetAllCities(Optional ByVal activeOnly As Boolean = True) As DataTable
        Dim sql As String = "SELECT * FROM [Cities]"
        If activeOnly Then sql &= " WHERE [IsActive] = True"
        sql &= " ORDER BY [CityName] ASC"
        Return DBConnection.ExecuteDataTable(sql)
    End Function

    Public Function SaveCity(ByVal cityId As Integer, ByVal cityName As String, ByVal stateName As String, ByVal pinPrefix As String, ByVal isActive As Boolean) As Boolean
        If cityId = 0 Then
            Return DBConnection.ExecuteNonQuery("INSERT INTO [Cities] ([CityName], [StateName], [PINPrefix], [IsActive]) VALUES (?, ?, ?, ?)", New Object() {cityName.Trim(), stateName.Trim(), pinPrefix.Trim(), isActive}) > 0
        Else
            Return DBConnection.ExecuteNonQuery("UPDATE [Cities] SET [CityName] = ?, [StateName] = ?, [PINPrefix] = ?, [IsActive] = ? WHERE [CityID] = ?", New Object() {cityName.Trim(), stateName.Trim(), pinPrefix.Trim(), isActive, cityId}) > 0
        End If
    End Function

    Public Function DeleteCity(ByVal cityId As Integer) As Boolean
        Return DBConnection.ExecuteNonQuery("DELETE FROM [Cities] WHERE [CityID] = ?", New Object() {cityId}) > 0
    End Function

    ' Announcements, Feedback & Complaints
    Public Function GetActiveAnnouncements(Optional ByVal role As String = "All") As DataTable
        Dim sql As String = "SELECT * FROM [Announcements] WHERE [IsActive] = True AND ([TargetRole] = 'All' OR [TargetRole] = ?) ORDER BY [AnnouncementID] DESC"
        Return DBConnection.ExecuteDataTable(sql, New Object() {role})
    End Function

    Public Function GetAllAnnouncements() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT * FROM [Announcements] ORDER BY [AnnouncementID] DESC")
    End Function

    Public Function SaveAnnouncement(ByVal id As Integer, ByVal title As String, ByVal content As String, ByVal targetRole As String, ByVal expDate As DateTime, ByVal isActive As Boolean) As Boolean
        If id = 0 Then
            Return DBConnection.ExecuteNonQuery("INSERT INTO [Announcements] ([Title], [Content], [TargetRole], [PublishDate], [ExpiryDate], [IsActive]) VALUES (?, ?, ?, ?, ?, ?)", New Object() {title.Trim(), content.Trim(), targetRole, DateTime.Now, expDate, isActive}) > 0
        Else
            Return DBConnection.ExecuteNonQuery("UPDATE [Announcements] SET [Title] = ?, [Content] = ?, [TargetRole] = ?, [ExpiryDate] = ?, [IsActive] = ? WHERE [AnnouncementID] = ?", New Object() {title.Trim(), content.Trim(), targetRole, expDate, isActive, id}) > 0
        End If
    End Function

    Public Function DeleteAnnouncement(ByVal id As Integer) As Boolean
        Return DBConnection.ExecuteNonQuery("DELETE FROM [Announcements] WHERE [AnnouncementID] = ?", New Object() {id}) > 0
    End Function

    Public Function SubmitFeedback(ByVal role As String, ByVal userId As Integer, ByVal userName As String, ByVal email As String, ByVal rating As Integer, ByVal subject As String, ByVal message As String) As Boolean
        Dim sql As String = "INSERT INTO [Feedback] ([UserRole], [UserID], [UserName], [EmailAddress], [Rating], [Subject], [Message], [SubmittedDate], [AdminReply], [Status]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, '', 'New')"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {role, userId, userName.Trim(), email.Trim(), rating, subject.Trim(), message.Trim(), DateTime.Now}) > 0
    End Function

    Public Function GetAllFeedback() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT * FROM [Feedback] ORDER BY [FeedbackID] DESC")
    End Function

    Public Function ReplyFeedback(ByVal feedbackId As Integer, ByVal reply As String) As Boolean
        Return DBConnection.ExecuteNonQuery("UPDATE [Feedback] SET [AdminReply] = ?, [Status] = 'Reviewed' WHERE [FeedbackID] = ?", New Object() {reply.Trim(), feedbackId}) > 0
    End Function

    Public Function SubmitComplaint(ByVal role As String, ByVal userId As Integer, ByVal userName As String, ByVal donationId As Integer, ByVal subject As String, ByVal description As String, ByVal priority As String) As Boolean
        Dim sql As String = "INSERT INTO [Complaints] ([UserRole], [UserID], [UserName], [DonationID], [Subject], [Description], [Priority], [Status], [SubmittedDate], [ResolutionNotes]) VALUES (?, ?, ?, ?, ?, ?, ?, 'Open', ?, '')"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {role, userId, userName.Trim(), donationId, subject.Trim(), description.Trim(), priority, DateTime.Now}) > 0
    End Function

    Public Function GetAllComplaints() As DataTable
        Return DBConnection.ExecuteDataTable("SELECT * FROM [Complaints] ORDER BY [ComplaintID] DESC")
    End Function

    Public Function ResolveComplaint(ByVal complaintId As Integer, ByVal status As String, ByVal notes As String) As Boolean
        Return DBConnection.ExecuteNonQuery("UPDATE [Complaints] SET [Status] = ?, [ResolutionNotes] = ? WHERE [ComplaintID] = ?", New Object() {status, notes.Trim(), complaintId}) > 0
    End Function

    Private Function MapDonation(ByVal row As DataRow) As FoodDonation
        Dim d As New FoodDonation()
        d.DonationID = Convert.ToInt32(row("DonationID"))
        d.DonorID = Convert.ToInt32(row("DonorID"))
        If row.Table.Columns.Contains("DonorName") AndAlso Not IsDBNull(row("DonorName")) Then d.DonorName = Convert.ToString(row("DonorName"))
        If row.Table.Columns.Contains("DonorMobile") AndAlso Not IsDBNull(row("DonorMobile")) Then d.DonorMobile = Convert.ToString(row("DonorMobile"))
        d.FoodName = Convert.ToString(row("FoodName"))
        d.FoodCategory = Convert.ToString(row("FoodCategory"))
        d.FoodDescription = If(IsDBNull(row("FoodDescription")), "", Convert.ToString(row("FoodDescription")))
        d.FoodType = Convert.ToString(row("FoodType"))
        d.FoodQuantity = Convert.ToDouble(row("FoodQuantity"))
        d.AvailableQuantity = Convert.ToDouble(row("AvailableQuantity"))
        d.QuantityUnit = Convert.ToString(row("QuantityUnit"))
        d.NumberOfPersonsServed = If(IsDBNull(row("NumberOfPersonsServed")), 0, Convert.ToInt32(row("NumberOfPersonsServed")))
        If Not IsDBNull(row("FoodPreparationDate")) Then d.FoodPreparationDate = Convert.ToDateTime(row("FoodPreparationDate"))
        d.FoodPreparationTime = If(IsDBNull(row("FoodPreparationTime")), "", Convert.ToString(row("FoodPreparationTime")))
        If Not IsDBNull(row("ExpiryDate")) Then d.ExpiryDate = Convert.ToDateTime(row("ExpiryDate"))
        d.ExpiryTime = If(IsDBNull(row("ExpiryTime")), "", Convert.ToString(row("ExpiryTime")))
        d.PickupAddress = Convert.ToString(row("PickupAddress"))
        d.City = Convert.ToString(row("City"))
        d.PINCode = Convert.ToString(row("PINCode"))
        If Not IsDBNull(row("PreferredPickupDate")) Then d.PreferredPickupDate = Convert.ToDateTime(row("PreferredPickupDate"))
        d.PreferredPickupTime = If(IsDBNull(row("PreferredPickupTime")), "", Convert.ToString(row("PreferredPickupTime")))
        d.SpecialInstructions = If(IsDBNull(row("SpecialInstructions")), "", Convert.ToString(row("SpecialInstructions")))
        d.FoodImage = If(IsDBNull(row("FoodImage")), "~/Content/images/food-meals.svg", Convert.ToString(row("FoodImage")))
        If Not IsDBNull(row("DonationDate")) Then d.DonationDate = Convert.ToDateTime(row("DonationDate"))
        d.ApprovalStatus = Convert.ToString(row("ApprovalStatus"))
        d.CollectionStatus = Convert.ToString(row("CollectionStatus"))
        d.AdminRemarks = If(IsDBNull(row("AdminRemarks")), "", Convert.ToString(row("AdminRemarks")))
        Return d
    End Function
End Class
