Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb

Public Class VolunteerDAL
    Public Function EmailExists(ByVal email As String, Optional ByVal excludeVolId As Integer = 0) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM [Volunteers] WHERE LCase([EmailAddress]) = LCase(?) AND [VolunteerID] <> ?"
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar(sql, New Object() {email.Trim(), excludeVolId}))
        Return count > 0
    End Function

    Public Function RegisterVolunteer(ByVal model As Volunteer) As Integer
        Dim sql As String = "INSERT INTO [Volunteers] ([VolunteerName], [Gender], [MobileNumber], [EmailAddress], [Address], [City], [AvailabilityStatus], [PasswordHash], [RegistrationDate], [AccountStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.VolunteerName.Trim(),
            model.Gender.Trim(),
            model.MobileNumber.Trim(),
            model.EmailAddress.Trim().ToLower(),
            model.Address.Trim(),
            model.City.Trim(),
            If(String.IsNullOrEmpty(model.AvailabilityStatus), "Available", model.AvailabilityStatus),
            model.PasswordHash,
            DateTime.Now,
            "Active",
            If(String.IsNullOrEmpty(model.ProfileImage), "~/Content/images/avatar-volunteer.svg", model.ProfileImage)
        })
    End Function

    Public Function AuthenticateVolunteer(ByVal email As String, ByVal passwordHash As String) As Volunteer
        Dim sql As String = "SELECT * FROM [Volunteers] WHERE LCase([EmailAddress]) = LCase(?) AND [PasswordHash] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {email.Trim(), passwordHash})
        If dt.Rows.Count > 0 Then
            Return MapVolunteer(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function GetVolunteerById(ByVal volunteerId As Integer) As Volunteer
        Dim sql As String = "SELECT * FROM [Volunteers] WHERE [VolunteerID] = ?"
        Dim dt As DataTable = DBConnection.ExecuteDataTable(sql, New Object() {volunteerId})
        If dt.Rows.Count > 0 Then
            Return MapVolunteer(dt.Rows(0))
        End If
        Return Nothing
    End Function

    Public Function UpdateVolunteerProfile(ByVal model As Volunteer) As Boolean
        Dim sql As String = "UPDATE [Volunteers] SET [VolunteerName] = ?, [Gender] = ?, [MobileNumber] = ?, [Address] = ?, [City] = ?, [AvailabilityStatus] = ?, [ProfileImage] = ? WHERE [VolunteerID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {
            model.VolunteerName.Trim(),
            model.Gender.Trim(),
            model.MobileNumber.Trim(),
            model.Address.Trim(),
            model.City.Trim(),
            model.AvailabilityStatus,
            model.ProfileImage,
            model.VolunteerID
        }) > 0
    End Function

    Public Function UpdateAvailability(ByVal volunteerId As Integer, ByVal availability As String) As Boolean
        Dim sql As String = "UPDATE [Volunteers] SET [AvailabilityStatus] = ? WHERE [VolunteerID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {availability, volunteerId}) > 0
    End Function

    Public Function ChangePassword(ByVal volunteerId As Integer, ByVal oldHash As String, ByVal newHash As String) As Boolean
        Dim sql As String = "UPDATE [Volunteers] SET [PasswordHash] = ? WHERE [VolunteerID] = ? AND [PasswordHash] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {newHash, volunteerId, oldHash}) > 0
    End Function

    Public Function GetAllVolunteers(Optional ByVal search As String = "", Optional ByVal city As String = "", Optional ByVal availability As String = "", Optional ByVal status As String = "") As DataTable
        Dim sql As String = "SELECT v.*, (SELECT COUNT(*) FROM [VolunteerAssignments] va WHERE va.[VolunteerID] = v.[VolunteerID]) AS [TotalTasks] FROM [Volunteers] v WHERE 1=1"
        Dim params As New List(Of Object)()

        If Not String.IsNullOrEmpty(search) Then
            sql &= " AND (v.[VolunteerName] LIKE ? OR v.[EmailAddress] LIKE ? OR v.[MobileNumber] LIKE ?)"
            Dim kw As String = "%" & search.Trim() & "%"
            params.Add(kw)
            params.Add(kw)
            params.Add(kw)
        End If
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND v.[City] = ?"
            params.Add(city)
        End If
        If Not String.IsNullOrEmpty(availability) AndAlso availability <> "All" Then
            sql &= " AND v.[AvailabilityStatus] = ?"
            params.Add(availability)
        End If
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then
            sql &= " AND v.[AccountStatus] = ?"
            params.Add(status)
        End If
        sql &= " ORDER BY v.[VolunteerID] DESC"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function GetAvailableVolunteers(Optional ByVal city As String = "") As DataTable
        Dim sql As String = "SELECT [VolunteerID], [VolunteerName] & ' (' & [City] & ' - ' & [MobileNumber] & ') [' & [AvailabilityStatus] & ']' AS [DisplayInfo], [VolunteerName], [City], [MobileNumber], [AvailabilityStatus] FROM [Volunteers] WHERE [AccountStatus] = 'Active'"
        Dim params As New List(Of Object)()
        If Not String.IsNullOrEmpty(city) AndAlso city <> "All" Then
            sql &= " AND [City] = ?"
            params.Add(city)
        End If
        sql &= " ORDER BY IIf([AvailabilityStatus]='Available', 0, 1), [VolunteerName]"
        Return DBConnection.ExecuteDataTable(sql, params.ToArray())
    End Function

    Public Function UpdateAccountStatus(ByVal volunteerId As Integer, ByVal status As String) As Boolean
        Dim sql As String = "UPDATE [Volunteers] SET [AccountStatus] = ? WHERE [VolunteerID] = ?"
        Return DBConnection.ExecuteNonQuery(sql, New Object() {status, volunteerId}) > 0
    End Function

    Public Function DeleteVolunteer(ByVal volunteerId As Integer) As Boolean
        Dim count As Integer = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [VolunteerID] = ?", New Object() {volunteerId}))
        If count > 0 Then
            Return UpdateAccountStatus(volunteerId, "Inactive")
        End If
        Return DBConnection.ExecuteNonQuery("DELETE FROM [Volunteers] WHERE [VolunteerID] = ?", New Object() {volunteerId}) > 0
    End Function

    Public Function GetVolunteerDashboardStats(ByVal volunteerId As Integer) As Dictionary(Of String, Object)
        Dim stats As New Dictionary(Of String, Object)()
        stats("TotalAssignedTasks") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [VolunteerID] = ?", New Object() {volunteerId}))
        stats("PendingTasks") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [VolunteerID] = ? AND [TaskStatus] = 'Assigned'", New Object() {volunteerId}))
        stats("AcceptedTasks") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [VolunteerID] = ? AND [TaskStatus] IN ('Accepted', 'Food Collected', 'Out for Delivery')", New Object() {volunteerId}))
        stats("CompletedDeliveries") = Convert.ToInt32(DBConnection.ExecuteScalar("SELECT COUNT(*) FROM [VolunteerAssignments] WHERE [VolunteerID] = ? AND [TaskStatus] = 'Successfully Delivered'", New Object() {volunteerId}))
        Dim availObj As Object = DBConnection.ExecuteScalar("SELECT [AvailabilityStatus] FROM [Volunteers] WHERE [VolunteerID] = ?", New Object() {volunteerId})
        stats("AvailabilityStatus") = If(availObj Is Nothing OrElse IsDBNull(availObj), "Available", Convert.ToString(availObj))
        Dim totalQtyObj As Object = DBConnection.ExecuteScalar("SELECT SUM([AssignedQuantity]) FROM [VolunteerAssignments] WHERE [VolunteerID] = ? AND [TaskStatus] = 'Successfully Delivered'", New Object() {volunteerId})
        stats("TotalQuantityDelivered") = If(totalQtyObj Is Nothing OrElse IsDBNull(totalQtyObj), 0.0, Convert.ToDouble(totalQtyObj))
        Return stats
    End Function

    Private Function MapVolunteer(ByVal row As DataRow) As Volunteer
        Dim v As New Volunteer()
        v.VolunteerID = Convert.ToInt32(row("VolunteerID"))
        v.VolunteerName = Convert.ToString(row("VolunteerName"))
        v.Gender = Convert.ToString(row("Gender"))
        v.MobileNumber = Convert.ToString(row("MobileNumber"))
        v.EmailAddress = Convert.ToString(row("EmailAddress"))
        v.Address = Convert.ToString(row("Address"))
        v.City = Convert.ToString(row("City"))
        v.AvailabilityStatus = Convert.ToString(row("AvailabilityStatus"))
        v.PasswordHash = Convert.ToString(row("PasswordHash"))
        If Not IsDBNull(row("RegistrationDate")) Then v.RegistrationDate = Convert.ToDateTime(row("RegistrationDate"))
        v.AccountStatus = Convert.ToString(row("AccountStatus"))
        v.ProfileImage = If(IsDBNull(row("ProfileImage")), "~/Content/images/avatar-volunteer.svg", Convert.ToString(row("ProfileImage")))
        Return v
    End Function
End Class
