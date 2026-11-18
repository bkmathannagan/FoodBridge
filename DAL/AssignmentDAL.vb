Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class AssignmentDAL
    Public Function AssignVolunteerToRequest(ByVal requestId As Integer, ByVal volunteerId As Integer, ByVal remarks As String, ByVal adminName As String) As Integer
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    Dim donationId As Integer = 0
                    Dim reqQty As Double = 0
                    Dim unit As String = "Kg"
                    Dim donorAddr As String = ""
                    Dim recvAddr As String = ""
                    Dim volName As String = ""

                    Dim infoSql As String = "SELECT fr.[DonationID], fr.[RequestedQuantity], fr.[DeliveryAddress], fr.[City] AS [RecvCity], fd.[PickupAddress], fd.[City] AS [DonorCity], fd.[PINCode], fd.[QuantityUnit] FROM [FoodRequests] fr INNER JOIN [FoodDonations] fd ON fr.[DonationID] = fd.[DonationID] WHERE fr.[RequestID] = ?"
                    Using getCmd As New OleDbCommand(infoSql, conn, trans)
                        DBConnection.AddParam(getCmd, "@p1", requestId)
                        Using rdr As OleDbDataReader = getCmd.ExecuteReader()
                            If rdr.Read() Then
                                donationId = Convert.ToInt32(rdr("DonationID"))
                                reqQty = Convert.ToDouble(rdr("RequestedQuantity"))
                                unit = Convert.ToString(rdr("QuantityUnit"))
                                donorAddr = Convert.ToString(rdr("PickupAddress")) & ", " & Convert.ToString(rdr("DonorCity")) & " - " & Convert.ToString(rdr("PINCode"))
                                recvAddr = Convert.ToString(rdr("DeliveryAddress")) & ", " & Convert.ToString(rdr("RecvCity"))
                            Else
                                Throw New ApplicationException("Food Request not found.")
                            End If
                        End Using
                    End Using

                    Using vCmd As New OleDbCommand("SELECT [VolunteerName] FROM [Volunteers] WHERE [VolunteerID] = ?", conn, trans)
                        DBConnection.AddParam(vCmd, "@p1", volunteerId)
                        Dim vObj As Object = vCmd.ExecuteScalar()
                        volName = If(vObj Is Nothing OrElse IsDBNull(vObj), "Volunteer #" & volunteerId, Convert.ToString(vObj))
                    End Using

                    ' Check if an active task already exists for this request (reassignment)
                    Dim existingTaskId As Integer = 0
                    Using chkTask As New OleDbCommand("SELECT TOP 1 [TaskID] FROM [VolunteerAssignments] WHERE [RequestID] = ? AND [TaskStatus] NOT IN ('Successfully Delivered', 'Failed')", conn, trans)
                        DBConnection.AddParam(chkTask, "@p1", requestId)
                        Dim tObj As Object = chkTask.ExecuteScalar()
                        If tObj IsNot Nothing AndAlso Not IsDBNull(tObj) Then
                            existingTaskId = Convert.ToInt32(tObj)
                        End If
                    End Using

                    Dim taskId As Integer = 0
                    If existingTaskId > 0 Then
                        Using updTask As New OleDbCommand("UPDATE [VolunteerAssignments] SET [VolunteerID] = ?, [AssignmentDate] = ?, [TaskStatus] = 'Reassigned', [CollectionRemarks] = ? WHERE [TaskID] = ?", conn, trans)
                            DBConnection.AddParam(updTask, "@p1", volunteerId)
                            DBConnection.AddParam(updTask, "@p2", DateTime.Now)
                            DBConnection.AddParam(updTask, "@p3", "Reassigned to " & volName & ". " & remarks)
                            DBConnection.AddParam(updTask, "@p4", existingTaskId)
                            updTask.ExecuteNonQuery()
                        End Using
                        taskId = existingTaskId
                    Else
                        Dim insSql As String = "INSERT INTO [VolunteerAssignments] ([DonationID], [RequestID], [VolunteerID], [DonorAddress], [ReceiverAddress], [AssignedQuantity], [QuantityUnit], [AssignmentDate], [TaskStatus], [CollectionRemarks], [DeliveryRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'Assigned', ?, '')"
                        Using insTask As New OleDbCommand(insSql, conn, trans)
                            DBConnection.AddParam(insTask, "@p1", donationId)
                            DBConnection.AddParam(insTask, "@p2", requestId)
                            DBConnection.AddParam(insTask, "@p3", volunteerId)
                            DBConnection.AddParam(insTask, "@p4", donorAddr)
                            DBConnection.AddParam(insTask, "@p5", recvAddr)
                            DBConnection.AddParam(insTask, "@p6", reqQty)
                            DBConnection.AddParam(insTask, "@p7", unit)
                            DBConnection.AddParam(insTask, "@p8", DateTime.Now)
                            DBConnection.AddParam(insTask, "@p9", remarks.Trim())
                            insTask.ExecuteNonQuery()
                        End Using
                        Using idCmd As New OleDbCommand("SELECT @@IDENTITY", conn, trans)
                            taskId = Convert.ToInt32(idCmd.ExecuteScalar())
                        End Using
                    End If

                    Using updReq As New OleDbCommand("UPDATE [FoodRequests] SET [RequestStatus] = 'Assigned', [AdminRemarks] = ? WHERE [RequestID] = ?", conn, trans)
                        DBConnection.AddParam(updReq, "@p1", "Assigned volunteer: " & volName)
                        DBConnection.AddParam(updReq, "@p2", requestId)
                        updReq.ExecuteNonQuery()
                    End Using

                    Using updDon As New OleDbCommand("UPDATE [FoodDonations] SET [CollectionStatus] = 'Assigned' WHERE [DonationID] = ?", conn, trans)
                        DBConnection.AddParam(updDon, "@p1", donationId)
                        updDon.ExecuteNonQuery()
                    End Using

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, ?, 'Admin', ?, 'Approved', 'Assigned', ?, ?, ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", donationId)
                        DBConnection.AddParam(logCmd, "@p2", requestId)
                        DBConnection.AddParam(logCmd, "@p3", taskId)
                        DBConnection.AddParam(logCmd, "@p4", adminName)
                        DBConnection.AddParam(logCmd, "@p5", reqQty)
                        DBConnection.AddParam(logCmd, "@p6", "Assigned volunteer " & volName & ". " & remarks)
                        DBConnection.AddParam(logCmd, "@p7", DateTime.Now)
                        logCmd.ExecuteNonQuery()
                    End Using

                    trans.Commit()
                    Return taskId
                Catch
                    trans.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Public Function UpdateTaskStatusByVolunteer(ByVal taskId As Integer, ByVal volunteerId As Integer, ByVal newStatus As String, ByVal collectionRemarks As String, ByVal deliveryRemarks As String, ByVal receivedByPerson As String, ByVal deliveryCondition As String, ByVal volunteerName As String) As Boolean
        Using conn As OleDbConnection = DBConnection.GetConnection()
            conn.Open()
            Using trans As OleDbTransaction = conn.BeginTransaction()
                Try
                    Dim donationId As Integer = 0
                    Dim requestId As Integer = 0
                    Dim ngoId As Integer = 0
                    Dim qty As Double = 0
                    Dim unit As String = "Kg"
                    Dim curStatus As String = ""
                    Dim recvPhone As String = ""

                    Dim sqlGet As String = "SELECT va.[DonationID], va.[RequestID], va.[AssignedQuantity], va.[QuantityUnit], va.[TaskStatus], fr.[NGOID], fr.[ContactNumber] FROM [VolunteerAssignments] va INNER JOIN [FoodRequests] fr ON va.[RequestID] = fr.[RequestID] WHERE va.[TaskID] = ? AND va.[VolunteerID] = ?"
                    Using cmdGet As New OleDbCommand(sqlGet, conn, trans)
                        DBConnection.AddParam(cmdGet, "@p1", taskId)
                        DBConnection.AddParam(cmdGet, "@p2", volunteerId)
                        Using rdr As OleDbDataReader = cmdGet.ExecuteReader()
                            If rdr.Read() Then
                                donationId = Convert.ToInt32(rdr("DonationID"))
                                requestId = Convert.ToInt32(rdr("RequestID"))
                                qty = Convert.ToDouble(rdr("AssignedQuantity"))
                                unit = If(IsDBNull(rdr("QuantityUnit")), "Kg", Convert.ToString(rdr("QuantityUnit")))
                                curStatus = Convert.ToString(rdr("TaskStatus"))
                                ngoId = Convert.ToInt32(rdr("NGOID"))
                                recvPhone = If(IsDBNull(rdr("ContactNumber")), "", Convert.ToString(rdr("ContactNumber")))
                            Else
                                Return False
                            End If
                        End Using
                    End Using

                    If newStatus = "Food Collected" Then
                        Using cmdUpd As New OleDbCommand("UPDATE [VolunteerAssignments] SET [TaskStatus] = ?, [CollectionDate] = ?, [CollectionRemarks] = ? WHERE [TaskID] = ?", conn, trans)
                            DBConnection.AddParam(cmdUpd, "@p1", newStatus)
                            DBConnection.AddParam(cmdUpd, "@p2", DateTime.Now)
                            DBConnection.AddParam(cmdUpd, "@p3", collectionRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p4", taskId)
                            cmdUpd.ExecuteNonQuery()
                        End Using
                        Using cmdDon As New OleDbCommand("UPDATE [FoodDonations] SET [CollectionStatus] = 'Collected' WHERE [DonationID] = ?", conn, trans)
                            DBConnection.AddParam(cmdDon, "@p1", donationId)
                            cmdDon.ExecuteNonQuery()
                        End Using
                        Using cmdReq As New OleDbCommand("UPDATE [FoodRequests] SET [RequestStatus] = 'Processing' WHERE [RequestID] = ?", conn, trans)
                            DBConnection.AddParam(cmdReq, "@p1", requestId)
                            cmdReq.ExecuteNonQuery()
                        End Using
                    ElseIf newStatus = "Successfully Delivered" Then
                        Using cmdUpd As New OleDbCommand("UPDATE [VolunteerAssignments] SET [TaskStatus] = 'Successfully Delivered', [DeliveryDate] = ?, [CollectionRemarks] = IIf(?='', [CollectionRemarks], ?), [DeliveryRemarks] = ? WHERE [TaskID] = ?", conn, trans)
                            DBConnection.AddParam(cmdUpd, "@p1", DateTime.Now)
                            DBConnection.AddParam(cmdUpd, "@p2", collectionRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p3", collectionRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p4", deliveryRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p5", taskId)
                            cmdUpd.ExecuteNonQuery()
                        End Using

                        ' Prevent duplicate delivery record
                        Dim delivCount As Integer = 0
                        Using chkDel As New OleDbCommand("SELECT COUNT(*) FROM [FoodDelivery] WHERE [TaskID] = ?", conn, trans)
                            DBConnection.AddParam(chkDel, "@p1", taskId)
                            delivCount = Convert.ToInt32(chkDel.ExecuteScalar())
                        End Using

                        If delivCount = 0 Then
                            Dim insDel As String = "INSERT INTO [FoodDelivery] ([TaskID], [DonationID], [RequestID], [VolunteerID], [NGOID], [DeliveredQuantity], [QuantityUnit], [ReceivedByPerson], [ReceiverContact], [DeliveryCondition], [DeliveryRemarks], [DeliveryTimestamp]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
                            Using cmdDel As New OleDbCommand(insDel, conn, trans)
                                DBConnection.AddParam(cmdDel, "@p1", taskId)
                                DBConnection.AddParam(cmdDel, "@p2", donationId)
                                DBConnection.AddParam(cmdDel, "@p3", requestId)
                                DBConnection.AddParam(cmdDel, "@p4", volunteerId)
                                DBConnection.AddParam(cmdDel, "@p5", ngoId)
                                DBConnection.AddParam(cmdDel, "@p6", qty)
                                DBConnection.AddParam(cmdDel, "@p7", unit)
                                DBConnection.AddParam(cmdDel, "@p8", If(String.IsNullOrEmpty(receivedByPerson), "NGO Authorized Receiver", receivedByPerson.Trim()))
                                DBConnection.AddParam(cmdDel, "@p9", recvPhone)
                                DBConnection.AddParam(cmdDel, "@p10", If(String.IsNullOrEmpty(deliveryCondition), "Fresh & Hygienic", deliveryCondition))
                                DBConnection.AddParam(cmdDel, "@p11", deliveryRemarks.Trim())
                                DBConnection.AddParam(cmdDel, "@p12", DateTime.Now)
                                cmdDel.ExecuteNonQuery()
                            End Using
                        End If

                        Using cmdReq As New OleDbCommand("UPDATE [FoodRequests] SET [RequestStatus] = 'Delivered' WHERE [RequestID] = ?", conn, trans)
                            DBConnection.AddParam(cmdReq, "@p1", requestId)
                            cmdReq.ExecuteNonQuery()
                        End Using

                        ' Check if remaining AvailableQuantity is 0 and no pending requests remain; mark donation Delivered
                        Dim remAvail As Double = 0
                        Using chkDon As New OleDbCommand("SELECT [AvailableQuantity] FROM [FoodDonations] WHERE [DonationID] = ?", conn, trans)
                            DBConnection.AddParam(chkDon, "@p1", donationId)
                            remAvail = Convert.ToDouble(chkDon.ExecuteScalar())
                        End Using

                        If remAvail <= 0 Then
                            Using cmdDon As New OleDbCommand("UPDATE [FoodDonations] SET [ApprovalStatus] = 'Delivered', [CollectionStatus] = 'Delivered' WHERE [DonationID] = ?", conn, trans)
                                DBConnection.AddParam(cmdDon, "@p1", donationId)
                                cmdDon.ExecuteNonQuery()
                            End Using
                        Else
                            Using cmdDon As New OleDbCommand("UPDATE [FoodDonations] SET [CollectionStatus] = 'Partially Delivered' WHERE [DonationID] = ?", conn, trans)
                                DBConnection.AddParam(cmdDon, "@p1", donationId)
                                cmdDon.ExecuteNonQuery()
                            End Using
                        End If
                    Else
                        ' Accepted, Rejected, Out for Delivery, Failed
                        Using cmdUpd As New OleDbCommand("UPDATE [VolunteerAssignments] SET [TaskStatus] = ?, [CollectionRemarks] = IIf(?='', [CollectionRemarks], ?), [DeliveryRemarks] = IIf(?='', [DeliveryRemarks], ?) WHERE [TaskID] = ?", conn, trans)
                            DBConnection.AddParam(cmdUpd, "@p1", newStatus)
                            DBConnection.AddParam(cmdUpd, "@p2", collectionRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p3", collectionRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p4", deliveryRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p5", deliveryRemarks.Trim())
                            DBConnection.AddParam(cmdUpd, "@p6", taskId)
                            cmdUpd.ExecuteNonQuery()
                        End Using

                        If newStatus = "Accepted" Then
                            Using cmdDon As New OleDbCommand("UPDATE [FoodDonations] SET [CollectionStatus] = 'Volunteer Accepted' WHERE [DonationID] = ?", conn, trans)
                                DBConnection.AddParam(cmdDon, "@p1", donationId)
                                cmdDon.ExecuteNonQuery()
                            End Using
                        ElseIf newStatus = "Out for Delivery" Then
                            Using cmdDon As New OleDbCommand("UPDATE [FoodDonations] SET [CollectionStatus] = 'Out for Delivery' WHERE [DonationID] = ?", conn, trans)
                                DBConnection.AddParam(cmdDon, "@p1", donationId)
                                cmdDon.ExecuteNonQuery()
                            End Using
                        End If
                    End If

                    Dim logSql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, ?, 'Volunteer', ?, ?, ?, ?, ?, ?)"
                    Using logCmd As New OleDbCommand(logSql, conn, trans)
                        DBConnection.AddParam(logCmd, "@p1", donationId)
                        DBConnection.AddParam(logCmd, "@p2", requestId)
                        DBConnection.AddParam(logCmd, "@p3", taskId)
                        DBConnection.AddParam(logCmd, "@p4", volunteerName)
                        DBConnection.AddParam(logCmd, "@p5", curStatus)
                        DBConnection.AddParam(logCmd, "@p6", newStatus)
                        DBConnection.AddParam(logCmd, "@p7", qty)
                        DBConnection.AddParam(logCmd, "@p8", If(Not String.IsNullOrEmpty(deliveryRemarks), deliveryRemarks, collectionRemarks))
                        DBConnection.AddParam(logCmd, "@p9", DateTime.Now)
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

    Public Function GetTasksByVolunteer(ByVal volunteerId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        Dim sql As String = "SELECT va.*, fd.[FoodName], fd.[FoodCategory], fd.[FoodType], fd.[PreferredPickupTime], d.[DonorName], d.[ContactPerson] AS [DonorContact], d.[MobileNumber] AS [DonorMobile], n.[OrganizationName] AS [NGOName], fr.[ContactPerson] AS [ReceiverContact], fr.[ContactNumber] AS [ReceiverMobile] " & _
            "FROM ((([VolunteerAssignments] va INNER JOIN [FoodDonations] fd ON va.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID]) INNER JOIN [FoodRequests] fr ON va.[RequestID] = fr.[RequestID]) INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID] " & _
            "WHERE va.[VolunteerID] = ?"
        Dim params As New List(Of Object)()
        params.Add(volunteerId)
        If Not String.IsNullOrEmpty(statusFilter) AndAlso statusFilter <> "All" Then
            sql &= " AND va.[TaskStatus] = ?"
            params.Add(statusFilter)
        End If
        sql &= " ORDER BY va.[TaskID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetTaskDetailsById(ByVal taskId As Integer) As DataRow
        Dim sql As String = "SELECT va.*, fd.[FoodName], fd.[FoodCategory], fd.[FoodType], fd.[ExpiryDate], fd.[ExpiryTime], fd.[PreferredPickupDate], fd.[PreferredPickupTime], fd.[SpecialInstructions], d.[DonorName], d.[ContactPerson] AS [DonorContact], d.[MobileNumber] AS [DonorMobile], n.[OrganizationName] AS [NGOName], fr.[ContactPerson] AS [ReceiverContact], fr.[ContactNumber] AS [ReceiverMobile], fr.[NumberOfBeneficiaries], fr.[RequestRemarks] " & _
            "FROM ((([VolunteerAssignments] va INNER JOIN [FoodDonations] fd ON va.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID]) INNER JOIN [FoodRequests] fr ON va.[RequestID] = fr.[RequestID]) INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID] " & _
            "WHERE va.[TaskID] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {taskId})
        If dt.Rows.Count > 0 Then Return dt.Rows(0)
        Return Nothing
    End Function

    Public Function GetAllAssignmentsForAdmin(Optional ByVal statusFilter As String = "", Optional ByVal search As String = "") As DataTable
        Dim sql As String = "SELECT va.*, v.[VolunteerName], v.[MobileNumber] AS [VolunteerMobile], fd.[FoodName], d.[DonorName], n.[OrganizationName] AS [NGOName] " & _
            "FROM (((([VolunteerAssignments] va INNER JOIN [Volunteers] v ON va.[VolunteerID] = v.[VolunteerID]) INNER JOIN [FoodDonations] fd ON va.[DonationID] = fd.[DonationID]) INNER JOIN [Donors] d ON fd.[DonorID] = d.[DonorID]) INNER JOIN [FoodRequests] fr ON va.[RequestID] = fr.[RequestID]) INNER JOIN [NGOs] n ON fr.[NGOID] = n.[NGOID] WHERE 1=1"
        Dim params As New List(Of Object)()
        If Not String.IsNullOrEmpty(statusFilter) AndAlso statusFilter <> "All" Then
            sql &= " AND va.[TaskStatus] = ?"
            params.Add(statusFilter)
        End If
        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (v.[VolunteerName] LIKE ? OR fd.[FoodName] LIKE ? OR n.[OrganizationName] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        sql &= " ORDER BY va.[TaskID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function
End Class
