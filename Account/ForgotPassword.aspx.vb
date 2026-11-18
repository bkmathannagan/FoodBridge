Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected WithEvents pnlAlert As Panel
    Protected WithEvents litAlert As Literal
    Protected WithEvents ddlRole As DropDownList
    Protected WithEvents txtEmail As TextBox
    Protected WithEvents txtMobile As TextBox
    Protected WithEvents txtNewPassword As TextBox
    Protected WithEvents btnReset As Button

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        Dim bll As New ReportBLL()
        Dim msg As String = ""
        If bll.ResetUserPassword(ddlRole.SelectedValue, txtEmail.Text, txtMobile.Text, txtNewPassword.Text, msg) Then
            Response.Redirect("~/Account/Login.aspx?role=" & ddlRole.SelectedValue & "&msg=" & Server.UrlEncode(msg), True)
        Else
            pnlAlert.Visible = True
            litAlert.Text = Server.HtmlEncode(msg)
        End If
    End Sub
End Class
