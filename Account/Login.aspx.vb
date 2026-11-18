Imports System
Imports System.Data
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Login
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlRole As DropDownList
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents txtPassword As TextBox
    Protected WithEvents btnLogin As Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim reqRole As String = Request.QueryString("role")
            If Not String.IsNullOrEmpty(reqRole) AndAlso ddlRole.Items.FindByValue(reqRole) IsNot Nothing Then
                ddlRole.SelectedValue = reqRole
            End If
            Dim msg As String = Request.QueryString("msg")
            If Not String.IsNullOrEmpty(msg) Then
                pnlAlert.Visible = True
                pnlAlert.CssClass = "alert alert-success"
                litAlert.Text = Server.HtmlEncode(msg)
            End If
        End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()
        Dim role As String = ddlRole.SelectedValue

        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then
            ShowError("Please enter both your email address and password.")
            Return
        End If

        Dim msg As String = ""
        Dim ip As String = Request.UserHostAddress

        Select Case role
            Case "Admin"
                Dim repBll As New ReportBLL()
                Dim adminRow As DataRow = repBll.LoginAdmin(email, password, ip, msg)
                If adminRow IsNot Nothing Then
                    Session("UserID") = Convert.ToInt32(adminRow("AdminID"))
                    Session("UserName") = Convert.ToString(adminRow("FullName"))
                    Session("UserEmail") = Convert.ToString(adminRow("EmailAddress"))
                    Session("UserRole") = "Admin"
                    FormsAuthentication.SetAuthCookie(email, False)
                    Response.Redirect("~/Admin/Dashboard.aspx", True)
                    Return
                End If

            Case "Donor"
                Dim donorBll As New DonorBLL()
                Dim d As Donor = donorBll.LoginDonor(email, password, ip, msg)
                If d IsNot Nothing Then
                    Session("UserID") = d.DonorID
                    Session("UserName") = d.DonorName
                    Session("UserEmail") = d.EmailAddress
                    Session("UserCity") = d.City
                    Session("UserRole") = "Donor"
                    FormsAuthentication.SetAuthCookie(email, False)
                    Response.Redirect("~/Donor/Dashboard.aspx", True)
                    Return
                End If

            Case "NGO"
                Dim ngoBll As New NGOBLL()
                Dim n As NGO = ngoBll.LoginNGO(email, password, ip, msg)
                If n IsNot Nothing Then
                    Session("UserID") = n.NGOID
                    Session("UserName") = n.OrganizationName
                    Session("UserEmail") = n.EmailAddress
                    Session("UserCity") = n.City
                    Session("UserRole") = "NGO"
                    FormsAuthentication.SetAuthCookie(email, False)
                    Response.Redirect("~/NGO/Dashboard.aspx", True)
                    Return
                End If

            Case "Volunteer"
                Dim volBll As New VolunteerBLL()
                Dim v As Volunteer = volBll.LoginVolunteer(email, password, ip, msg)
                If v IsNot Nothing Then
                    Session("UserID") = v.VolunteerID
                    Session("UserName") = v.VolunteerName
                    Session("UserEmail") = v.EmailAddress
                    Session("UserCity") = v.City
                    Session("UserRole") = "Volunteer"
                    FormsAuthentication.SetAuthCookie(email, False)
                    Response.Redirect("~/Volunteer/Dashboard.aspx", True)
                    Return
                End If
        End Select

        ShowError(msg)
    End Sub

    Private Sub ShowError(ByVal message As String)
        pnlAlert.Visible = True
        pnlAlert.CssClass = "alert alert-danger"
        litAlert.Text = Server.HtmlEncode(message)
    End Sub
End Class
