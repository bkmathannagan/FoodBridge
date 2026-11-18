Imports System
Imports System.Web
Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Synchronize expired food donations on application start
        DBConnection.SyncExpiredDonations()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' SEO-friendly clean URL rewriting compatible with VS 2010 and IIS
        Dim path As String = Request.AppRelativeCurrentExecutionFilePath.ToLowerInvariant()
        Select Case path
            Case "~/home"
                Context.RewritePath("~/Default.aspx")
            Case "~/about", "~/about-us"
                Context.RewritePath("~/About.aspx")
            Case "~/how-it-works"
                Context.RewritePath("~/HowItWorks.aspx")
            Case "~/contact", "~/contact-us"
                Context.RewritePath("~/Contact.aspx")
            Case "~/donate-food"
                Context.RewritePath("~/Account/RegisterDonor.aspx")
            Case "~/find-food"
                Context.RewritePath("~/NGO/AvailableFood.aspx")
            Case "~/become-a-volunteer"
                Context.RewritePath("~/Account/RegisterVolunteer.aspx")
            Case "~/ngo-registration"
                Context.RewritePath("~/Account/RegisterNGO.aspx")
            Case "~/login"
                Context.RewritePath("~/Account/Login.aspx")
        End Select
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub
End Class
