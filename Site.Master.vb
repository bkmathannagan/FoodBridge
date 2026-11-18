Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls

Public Class SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected WithEvents litTopBarUser As Literal
    Protected WithEvents pnlGuestActions As Panel
    Protected WithEvents pnlUserActions As Panel
    Protected WithEvents hlMyDashboard As HyperLink
    Protected WithEvents divLayoutWrapper As HtmlGenericControl
    Protected WithEvents pnlSidebar As Panel
    Protected WithEvents divMainContentArea As HtmlGenericControl
    Protected WithEvents litSidebarRole As Literal
    Protected WithEvents litSidebarUser As Literal
    Protected WithEvents pnlAdminMenu As Panel
    Protected WithEvents pnlDonorMenu As Panel
    Protected WithEvents pnlNGOMenu As Panel
    Protected WithEvents pnlVolunteerMenu As Panel

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim appRelPath As String = Request.AppRelativeCurrentExecutionFilePath.ToLowerInvariant()
        Dim userRole As String = Convert.ToString(Session("UserRole"))

        ' Enforce role-based access control on role folders
        If appRelPath.StartsWith("~/admin/") Then
            If userRole <> "Admin" Then
                Response.Redirect("~/Account/Login.aspx?role=Admin&returnUrl=" & Server.UrlEncode(Request.RawUrl), True)
                Return
            End If
        ElseIf appRelPath.StartsWith("~/donor/") Then
            If userRole <> "Donor" Then
                Response.Redirect("~/Account/Login.aspx?role=Donor&returnUrl=" & Server.UrlEncode(Request.RawUrl), True)
                Return
            End If
        ElseIf appRelPath.StartsWith("~/ngo/") Then
            ' Allow public browsing of AvailableFood.aspx, but protect all other NGO pages
            If Not appRelPath.EndsWith("/availablefood.aspx") AndAlso userRole <> "NGO" Then
                Response.Redirect("~/Account/Login.aspx?role=NGO&returnUrl=" & Server.UrlEncode(Request.RawUrl), True)
                Return
            End If
        ElseIf appRelPath.StartsWith("~/volunteer/") Then
            If userRole <> "Volunteer" Then
                Response.Redirect("~/Account/Login.aspx?role=Volunteer&returnUrl=" & Server.UrlEncode(Request.RawUrl), True)
                Return
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim appRelPath As String = Request.AppRelativeCurrentExecutionFilePath.ToLowerInvariant()
        Dim userRole As String = Convert.ToString(Session("UserRole"))
        Dim userName As String = Convert.ToString(Session("UserName"))

        If Not String.IsNullOrEmpty(userRole) Then
            pnlGuestActions.Visible = False
            pnlUserActions.Visible = True
            litTopBarUser.Text = "Signed in as: <strong>" & Server.HtmlEncode(userName) & " (" & Server.HtmlEncode(userRole) & ")</strong>"

            Select Case userRole
                Case "Admin"
                    hlMyDashboard.NavigateUrl = "~/Admin/Dashboard.aspx"
                Case "Donor"
                    hlMyDashboard.NavigateUrl = "~/Donor/Dashboard.aspx"
                Case "NGO"
                    hlMyDashboard.NavigateUrl = "~/NGO/Dashboard.aspx"
                Case "Volunteer"
                    hlMyDashboard.NavigateUrl = "~/Volunteer/Dashboard.aspx"
            End Select
        Else
            pnlGuestActions.Visible = True
            pnlUserActions.Visible = False
            litTopBarUser.Text = "<a href='" & ResolveUrl("~/Account/Login.aspx") & "'>Portal Login</a> | <a href='" & ResolveUrl("~/Account/RegisterDonor.aspx") & "'>Donate Surplus Food</a>"
        End If

        ' Activate Dashboard Sidebar Layout for Role Modules
        Dim isRoleModule As Boolean = appRelPath.StartsWith("~/admin/") OrElse appRelPath.StartsWith("~/donor/") OrElse appRelPath.StartsWith("~/volunteer/") OrElse (appRelPath.StartsWith("~/ngo/") AndAlso userRole = "NGO")
        If isRoleModule AndAlso Not String.IsNullOrEmpty(userRole) Then
            divLayoutWrapper.Attributes("class") = "dashboard-wrapper"
            pnlSidebar.Visible = True
            divMainContentArea.Attributes("class") = "dashboard-main"
            litSidebarRole.Text = Server.HtmlEncode(userRole.ToUpper() & " PORTAL")
            litSidebarUser.Text = Server.HtmlEncode(userName)

            pnlAdminMenu.Visible = (userRole = "Admin")
            pnlDonorMenu.Visible = (userRole = "Donor")
            pnlNGOMenu.Visible = (userRole = "NGO")
            pnlVolunteerMenu.Visible = (userRole = "Volunteer")
        Else
            divLayoutWrapper.Attributes("class") = ""
            pnlSidebar.Visible = False
            divMainContentArea.Attributes("class") = ""
        End If
    End Sub
End Class
