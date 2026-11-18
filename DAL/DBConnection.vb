Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text

Public Class DBConnection
    Public Shared Function GetConnectionString() As String
        Dim connSettings As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("FoodBridgeDB")
        If connSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(connSettings.ConnectionString) Then
            Return connSettings.ConnectionString
        End If
        Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\Food.mdb;"
    End Function

    Public Shared Function GetConnection() As OleDbConnection
        Return New OleDbConnection(GetConnectionString())
    End Function

    Public Shared Sub AddParam(ByVal cmd As OleDbCommand, ByVal name As String, ByVal value As Object)
        If value Is Nothing OrElse IsDBNull(value) Then
            cmd.Parameters.AddWithValue(name, DBNull.Value)
        ElseIf TypeOf value Is DateTime Then
            Dim dt As DateTime = DirectCast(value, DateTime)
            cmd.Parameters.AddWithValue(name, dt.ToString("yyyy-MM-dd HH:mm:ss"))
        ElseIf TypeOf value Is Boolean Then
            cmd.Parameters.AddWithValue(name, DirectCast(value, Boolean))
        Else
            cmd.Parameters.AddWithValue(name, value)
        End If
    End Sub

    Public Shared Function ExecuteDataTable(ByVal sql As String, Optional ByVal params As Object() = Nothing) As DataTable
        Dim dt As New DataTable()
        Using conn As OleDbConnection = GetConnection()
            Using cmd As New OleDbCommand(sql, conn)
                If params IsNot Nothing Then
                    For i As Integer = 0 To params.Length - 1
                        AddParam(cmd, "@p" & (i + 1).ToString(), params(i))
                    Next
                End If
                Using da As New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Shared Function ExecuteNonQuery(ByVal sql As String, Optional ByVal params As Object() = Nothing) As Integer
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                If params IsNot Nothing Then
                    For i As Integer = 0 To params.Length - 1
                        AddParam(cmd, "@p" & (i + 1).ToString(), params(i))
                    Next
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteScalar(ByVal sql As String, Optional ByVal params As Object() = Nothing) As Object
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            Using cmd As New OleDbCommand(sql, conn)
                If params IsNot Nothing Then
                    For i As Integer = 0 To params.Length - 1
                        AddParam(cmd, "@p" & (i + 1).ToString(), params(i))
                    Next
                End If
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

    Public Shared Function HashPassword(ByVal plainText As String) As String
        If String.IsNullOrEmpty(plainText) Then Return String.Empty
        Using sha As New SHA256Managed()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(plainText.Trim())
            Dim hash As Byte() = sha.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    Public Shared Sub LogTransaction(ByVal donationId As Integer, ByVal requestId As Integer, ByVal taskId As Integer, ByVal role As String, ByVal actorName As String, ByVal statusFrom As String, ByVal statusTo As String, ByVal qty As Double, ByVal remarks As String)
        Try
            Dim sql As String = "INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
            ExecuteNonQuery(sql, New Object() {donationId, requestId, taskId, role, actorName, statusFrom, statusTo, qty, remarks, DateTime.Now})
        Catch
            ' Ignore non-critical audit failure
        End Try
    End Sub

    Public Shared Sub LogUserLogin(ByVal email As String, ByVal role As String, ByVal userId As Integer, ByVal ipAddress As String, ByVal status As String)
        Try
            Dim sql As String = "INSERT INTO [UserLogin] ([EmailAddress], [UserRole], [ReferenceUserID], [LastLoginDate], [IPAddress], [LoginStatus]) VALUES (?, ?, ?, ?, ?, ?)"
            ExecuteNonQuery(sql, New Object() {email, role, userId, DateTime.Now, If(String.IsNullOrEmpty(ipAddress), "127.0.0.1", ipAddress), status})
        Catch
        End Try
    End Sub

    Public Shared Sub SyncExpiredDonations()
        Try
            Dim todayStr As String = DateTime.Today.ToString("yyyy-MM-dd")
            Dim sql As String = "UPDATE [FoodDonations] SET [ApprovalStatus] = 'Expired', [CollectionStatus] = 'Expired' WHERE [ExpiryDate] < ? AND [ApprovalStatus] IN ('Pending', 'Approved')"
            ExecuteNonQuery(sql, New Object() {todayStr})
        Catch
        End Try
    End Sub
End Class
