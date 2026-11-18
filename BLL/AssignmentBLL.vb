Imports System
Imports System.Data

Public Class AssignmentBLL
    Private ReadOnly _dal As New AssignmentDAL()

    Public Function AssignVolunteer(ByVal requestId As Integer, ByVal volunteerId As Integer, ByVal remarks As String, ByVal adminName As String, ByRef message As String) As Integer
        If requestId <= 0 OrElse volunteerId <= 0 Then
            message = "Please select both a food request and an available volunteer."
            Return 0
        End If
        Try
            Dim taskId As Integer = _dal.AssignVolunteerToRequest(requestId, volunteerId, remarks, adminName)
            If taskId > 0 Then
                message = "Volunteer task #" & taskId & " assigned successfully!"
                Return taskId
            End If
            message = "Failed to assign volunteer."
            Return 0
        Catch ex As Exception
            message = ex.Message
            Return 0
        End Try
    End Function

    Public Function UpdateTaskStatus(ByVal taskId As Integer, ByVal volunteerId As Integer, ByVal newStatus As String, ByVal collectionRemarks As String, ByVal deliveryRemarks As String, ByVal receivedByPerson As String, ByVal deliveryCondition As String, ByVal volunteerName As String, ByRef message As String) As Boolean
        Try
            If _dal.UpdateTaskStatusByVolunteer(taskId, volunteerId, newStatus, collectionRemarks, deliveryRemarks, receivedByPerson, deliveryCondition, volunteerName) Then
                message = "Task #" & taskId & " status updated to '" & newStatus & "'."
                Return True
            End If
            message = "Unable to update task status."
            Return False
        Catch ex As Exception
            message = ex.Message
            Return False
        End Try
    End Function

    Public Function GetTasksByVolunteer(ByVal volunteerId As Integer, Optional ByVal statusFilter As String = "") As DataTable
        Return _dal.GetTasksByVolunteer(volunteerId, statusFilter)
    End Function

    Public Function GetTaskDetailsById(ByVal taskId As Integer) As DataRow
        Return _dal.GetTaskDetailsById(taskId)
    End Function

    Public Function GetAllAssignmentsForAdmin(Optional ByVal statusFilter As String = "", Optional ByVal search As String = "") As DataTable
        Return _dal.GetAllAssignmentsForAdmin(statusFilter, search)
    End Function
End Class
