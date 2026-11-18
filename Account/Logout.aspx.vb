Imports System
Imports System.Web.Security
Imports System.Web.UI

Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session.Clear()
        Session.Abandon()
        FormsAuthentication.SignOut()
        Response.Redirect("~/Account/Login.aspx?msg=" & Server.UrlEncode("You have been logged out securely."), True)
    End Sub
End Class
