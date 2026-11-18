Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class RequestDAL
    Public Function HasDuplicateActiveRequest(ByVal ngoId As Integer, ByVal donationId As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM [FoodRequests] WHERE [NGOID] = ? AND [DonationID] = ? AND [RequestStatus] IN ('Pending', 'Approved', 'Assigned', 'Processing')"
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar(sql, New Object() {ngoId, donationId}))
        Return count > 0
    End Function

    Public Function SubmitFoodRequest(ByVal model As FoodRequest, ByVal ngoName As String) As Integer
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    ' Check available quantity and expiry in transaction
                    Dim checkSql As String = "SELECT [AvailableQuantity], [ApprovalStatus], [ExpiryDate] FROM [FoodDonations] WHERE [DonationID] = ?"
                    Dim availQty As Double = 0
                    Dim appStatus As String = ""
                    Dim expDate As DateTime = DateTime.MinValue

                    Using chkCmd As New OleDbCommand(checkSql, conn, trans)
                        DBConnection.AddParam(chkCmd, "@p1", model.DonationID)
                        Using rdr As OleDbDataReader = chkCmd.ExecuteReader()
                            If rdr.Read() Then
                                availQty = Convert.ToDouble(rdr("AvailableQuantity"))
                                appStatus = Convert.ToString(rdr("ApprovalStatus"))
                                expDate = Convert.ToDateTime(rdr("ExpiryDate"))
                            Else
                                Throw New ApplicationException("Selected donation record does not exist.")
                            End If
                        End Using
                    End Using

                    If appStatus <> "Approved" Then
                        Throw New ApplicationException("Only approved donations can be requested.")
                    End If
                    If expDate < DateTime.Today Then
                        Throw New ApplicationException("This food donation has expired and cannot be requested.")
                    End If
                    If model.RequestedQuantity > availQty Then
                        Throw New ApplicationException("Requested quantity (" & model.RequestedQuantity & ") exceeds available quantity (" & availQty & ").")
                    End If

                    ' Reserve quantity upon request submission so multiple NGOs cannot over-allocate
                    Dim newAvail As Double = availQty - model.RequestedQuantity
                    Using updCmd As New OleDbCommand("UPDATE [FoodDonations] SET [AvailableQuantity] = ? WHERE [DonationID] = ?", conn, trans)
                        DBConnection.AddParam(updCmd, "@p1", newAvail)
                        DBConnection.AddParam(updCmd, "@p2", model.DonationID)
                        updCmd.ExecuteNonQuery()
                    End Using

                    Dim insSql As String = "INSERT INTO [FoodRequests] ([NGOID], [DonationID], [RequestedQuantity], [NumberOfBeneficiaries], [RequiredDate], [DeliveryAddress], [City], [ContactPerson], [ContactNumber], [RequestRemarks], [RequestDate], [RequestStatus], [AdminRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 'Pending', 'Awaiting Administrator Approval')"
                    Using insCmd As New OleDbCommand(insSql, conn, trans)
                        DBConnection.AddParam(insCmd, "@p1", model.NGOID)
                        DBConnection.AddParam(insCmd, "@p2", model.DonationID)
                        DBConnection.AddParam(insCmd, "@p3", model.RequestedQuantity)
                        DBConnection.AddParam(insCmd, "@p4", model.NumberOfBeneficiaries)
                        DBConnection.AddParam(insCmd, "@p5", model.RequiredDate.ToString("yyyy-MM-dd"))
                        DBConnection.AddParam(insCmd, "@p6", model.DeliveryAddress.Trim())
                        DBConnection.AddParam(insCmd, "@p7", model.City.Trim())
                        DBConnection.AddParam(insCmd, "@p8", model.ContactPerson.Trim())
                        DBConnection.AddParam(insCmd, "@p9", model.ContactNumber.Trim())
                        DBConnection.AddParam(insCmd, "@p10", model.RequestRemarks.Trim())
                        DBConnection.AddParam(insCmd, "@p11", DateTime.Now)
                        insCmd.ExecuteNonQuery()
                    End Using

                    Dim newReqId As Integer = 0
                    Using idCmd As New OleDbCommand("SELECT @@IDENTITY", conn, trans)
                        newReqId = Convert.ToInt32(idCmd.ExecuteScalar())
                    End Using

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, 0, 'NGO', ?, 'Available', 'Request Pending', ?, ?, ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", model.DonationID)
                        DBConnection.AddParam(logCmd, "@p2", newReqId)
                        DBConnection.AddParam(logCmd, "@p3", ngoName)
                        DBConnection.AddParam(logCmd, "@p4", model.RequestedQuantity)
                        DBConnection.AddParam(logCmd, "@p5", "Requested " & model.RequestedQuantity & " for " & model.NumberOfBeneficiaries & " beneficiaries.")
                        DBConnection.AddParam(logCmd, "@p6", DateTime.Now)
                        logCmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    Return newReqId
                Catch
                    trans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function UpdateRequestStatusByAdmin(ByVal requestId As Integer, ByVal newStatus As String, ByVal adminRemarks As String, ByVal adminName As String) As Boolean
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    Dim donId As Integer = 0
                    Dim reqQty As Double = 0
                    Dim curStatus As String = ""

                    Using getCmd As New OleDbCommand("SELECT [DonationID], [RequestedQuantity], [RequestStatus] FROM [FoodRequests] WHERE [RequestID] = ?", conn, trans)
                        DBConnection.AddParam(getCmd, "@p1", requestId)
                        Using rdr As OleDbDataReader = getCmd.ExecuteReader()
                            If rdr.Read() Then
                                donId = Convert.ToInt32(rdr("DonationID"))
                                reqQty = Convert.ToDouble(rdr("RequestedQuantity"))
                                curStatus = Convert.ToString(rdr("RequestStatus"))
                            Else
                                Return False
                            End If
                        End Using
                    End Using

                    If curStatus = newStatus Then Return True

                    ' If rejecting or cancelling a previously active request, release the reserved quantity back to FoodDonations
                    If (newStatus = "Rejected" OrElse newStatus = "Cancelled") AndAlso (curStatus = "Pending" OrElse curStatus = "Approved") Then
                        Using relCmd As New OleDbCommand("UPDATE [FoodDonations] SET [AvailableQuantity] = [AvailableQuantity] + ? WHERE [DonationID] = ?", conn, trans)
                            DBConnection.AddParam(relCmd, "@p1", reqQty)
                            DBConnection.AddParam(relCmd, "@p2", donId)
                            relCmd.ExecuteNonQuery()
                        End Using
                    End If

                    Using updCmd As New OleDbCommand("UPDATE [FoodRequests] SET [RequestStatus] = ?, [AdminRemarks] = ? WHERE [RequestID] = ?", conn, trans)
                        DBConnection.AddParam(updCmd, "@p1", newStatus)
                        DBConnection.AddParam(updCmd, "@p2", adminRemarks.Trim())
                        DBConnection.AddParam(updCmd, "@p3", requestId)
                        updCmd.ExecuteNonQuery()
                    End Using

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, 0, 'Admin', ?, ?, ?, ?, ?, ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", donId)
                        DBConnection.AddParam(logCmd, "@p2", requestId)
                        DBConnection.AddParam(logCmd, "@p3", adminName)
                        DBConnection.AddParam(logCmd, "@p4", curStatus)
                        DBConnection.AddParam(logCmd, "@p5", newStatus)
                        DBConnection.AddParam(logCmd, "@p6", reqQty)
                        DBConnection.AddParam(logCmd, "@p7", adminRemarks.Trim())
                        DBConnection.AddParam(logCmd, "@p8", DateTime.Now)
                        logCmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    Return True
                Catch
                    trans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function CancelRequestByNGO(ByVal requestId As Integer, ByVal ngoId As Integer, ByVal ngoName As String) As Boolean
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    Dim donId As Integer = 0
                    Dim reqQty As Double = 0
                    Dim curStatus As String = ""

                    Using getCmd As New OleDbCommand("SELECT [DonationID], [RequestedQuantity], [RequestStatus] FROM [FoodRequests] WHERE [RequestID] = ? AND [NGOID] = ?", conn, trans)
                        DBConnection.AddParam(getCmd, "@p1", requestId)
                        DBConnection.AddParam(getCmd, "@p2", ngoId)
                        Using rdr As OleDbDataReader = getCmd.ExecuteReader()
                            If rdr.Read() Then
                                donId = Convert.ToInt32(rdr("DonationID"))
                                reqQty = Convert.ToDouble(rdr("RequestedQuantity"))
                                curStatus = Convert.ToString(rdr("RequestStatus"))
                            Else
                                Return False
                            End If
                        End Using
                    End Using

                    If curStatus <> "Pending" AndAlso curStatus <> "Approved" Then
                        Throw New ApplicationException("Cannot cancel a request after volunteer collection has started.")
                    End If

                    ' Release reserved quantity
                    Using relCmd As New OleDbCommand("UPDATE [FoodDonations] SET [AvailableQuantity] = [AvailableQuantity] + ? WHERE [DonationID] = ?", conn, trans)
                        DBConnection.AddParam(relCmd, "@p1", reqQty)
                        DBConnection.AddParam(relCmd, "@p2", donId)
                        relCmd.ExecuteNonQuery()
                    End Using

                    Using updCmd As New OleDbCommand("UPDATE [FoodRequests] SET [RequestStatus] = 'Cancelled', [AdminRemarks] = 'Cancelled by NGO; reserved quantity released.' WHERE [RequestID] = ?", conn, trans)
                        DBConnection.AddParam(updCmd, "@p1", requestId)
                        updCmd.ExecuteNonQuery()
                    End Using

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, 0, 'NGO', ?, ?, 'Cancelled', ?, 'Cancelled by NGO; quantity released.', ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", donId)
                        DBConnection.AddParam(logCmd, "@p2", requestId)
                        DBConnection.AddParam(logCmd, "@p3", ngoName)
                        DBConnection.AddParam(logCmd, "@p4", curStatus)
                        DBConnection.AddParam(logCmd, "@p5", reqQty)
                        DBConnection.AddParam(logCmd, "@p6", DateTime.Now)
                        logCmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    Return True
                Catch
                    trans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function GetRequestsByNGO(ByVal ngoId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        Dim sql As String = "SELECT fr.*, fd.[FoodName], fd.[FoodCategory], fd.[FoodType], fd.[QuantityUnit], fd.[PickupAddress], d.[DonorName], d.[MobileNumber] AS [DonorMobile], " & _
            "(SELECT TOP 1 v.[VolunteerName] & ' (' & v.[MobileNumber] & ')' FROM [VolunteerAssignments] va INNER JOIN [Volunteers] v ON va.[VolunteerID] = v.[VolunteerID] WHERE va.[RequestID] = fr.[RequestID] ORDER BY va.[TaskID] DESC) AS [VolunteerInfo], " & _
            "(SELECT TOP 1 va.[TaskStatus] FROM [VolunteerAssignments] va WHERE va.[RequestID] = fr.[RequestID] ORDER BY va.[TaskID] DESC) AS [DeliveryStatus] " & _
            "FROM ([FoodRequests] fr INNER JOIN [FoodDonations] fd ON fr.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE fr.[NGOID] = ?"
        Dim params As New List(Of Object)()
        params.Add(ngoId)
        If Not String.IsNullOrEmpty(statusFilter) AndAlso statusFilter <> "All" Then
            sql &= " AND fr.[RequestStatus] = ?"
            params.Add(statusFilter)
        End If
        sql &= " ORDER BY fr.[RequestID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetAllRequestsForAdmin(Optional ByVal search As String = "", Optional ByVal status As String = "", Optional ByVal city As String = "") As DataTable
        Dim sql As String = "SELECT fr.*, n.[OrganizationName] AS [NGOName], n.[VerificationStatus], fd.[FoodName], fd.[FoodCategory], fd.[QuantityUnit], fd.[PickupAddress], d.[DonorName], " & _
            "(SELECT TOP 1 v.[VolunteerName] FROM [VolunteerAssignments] va INNER JOIN [Volunteers] v ON va.[VolunteerID] = v.[VolunteerID] WHERE va.[RequestID] = fr.[RequestID] ORDER BY va.[TaskID] DESC) AS [AssignedVolunteer] " & _
            "FROM (([FoodRequests] fr INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID]) INNER JOIN [FoodDonations] fd ON fr.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID] WHERE 1=1"
        Dim params As New List(Of Object)()
        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (n.[OrganizationName] LIKE ? OR fd.[FoodName] LIKE ? OR fr.[ContactPerson] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
            sql &= " AND fr.[RequestStatus] = ?"
            params.Add(status)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND fr.[City] = ?"
            params.Add(city)
        End If
        sql &= " ORDER BY fr.[RequestID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetApprovedRequestsForAssignment() As DataTable
        Dim sql As String = "SELECT fr.[RequestID], 'REQ #' & fr.[RequestID] & ' - ' & n.[OrganizationName] & ' (' & fd.[FoodName] & ' - ' & fr.[RequestedQuantity] & ' ' & fd.[QuantityUnit] & ', ' & fr.[City] & ') [' & fr.[RequestStatus] & ']' AS [DisplayText], fr.*, n.[OrganizationName] AS [NGOName], fd.[FoodName], fd.[PickupAddress], fd.[City] AS [PickupCity], fd.[QuantityUnit] " & _
            "FROM ([FoodRequests] fr INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID]) INNER JOIN [FoodDonations] fd ON fr.[DonationID] = fd.[DonationID] " & _
            "WHERE fr.[RequestStatus] IN ('Approved', 'Pending', 'Assigned') ORDER BY fr.[RequestID] DESC"
        Return DBConnection.ExecuteDataTable(sql)
    End Function
End Class
