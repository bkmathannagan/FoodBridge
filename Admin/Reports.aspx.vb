Imports System
Imports System.Data
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class AdminReports
    Inherits System.Web.UI.Page

    Protected WithEvents btnExportExcel As Button
    Protected WithEvents ddlReportType As DropDownList
    Protected WithEvents txtFromDate As TextBox
    Protected WithEvents txtToDate As TextBox
    Protected WithEvents ddlCategory As DropDownList
    Protected WithEvents ddlCity As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtSearch As TextBox
    Protected WithEvents btnGenerate As Button
    Protected WithEvents litReportName As Literal
    Protected WithEvents litTotalRecords As Literal
    Protected WithEvents litTotalQuantity As Literal
    Protected WithEvents gvReport As GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim donBll As New DonationBLL()
            ddlCategory.DataSource = donBll.GetCategories(True)
            ddlCategory.DataTextField = "CategoryName"
            ddlCategory.DataValueField = "CategoryName"
            ddlCategory.DataBind()
            ddlCategory.Items.Insert(0, New ListItem("All Categories", "All"))

            ddlCity.DataSource = donBll.GetCities(True)
            ddlCity.DataTextField = "CityName"
            ddlCity.DataValueField = "CityName"
            ddlCity.DataBind()
            ddlCity.Items.Insert(0, New ListItem("All Cities", "All"))

            BindReportGrid()
        End If
    End Sub

    Private Sub BindReportGrid()
        Dim fDate As Nullable(Of DateTime) = Nothing
        Dim tDate As Nullable(Of DateTime) = Nothing
        Dim parsedFrom As DateTime
        Dim parsedTo As DateTime
        If DateTime.TryParse(txtFromDate.Text.Trim(), parsedFrom) Then fDate = parsedFrom
        If DateTime.TryParse(txtToDate.Text.Trim(), parsedTo) Then tDate = parsedTo

        Dim repBll As New ReportBLL()
        Dim dt As DataTable = repBll.GenerateReport(ddlReportType.SelectedValue, fDate, tDate, ddlStatus.SelectedValue, ddlCategory.SelectedValue, ddlCity.SelectedValue, txtSearch.Text)

        litReportName.Text = Server.HtmlEncode(ddlReportType.SelectedItem.Text)
        litTotalRecords.Text = dt.Rows.Count.ToString()

        Dim sumQty As Double = 0
        For Each row As DataRow In dt.Rows
            If dt.Columns.Contains("Quantity") AndAlso Not IsDBNull(row("Quantity")) Then
                sumQty += Convert.ToDouble(row("Quantity"))
            ElseIf dt.Columns.Contains("TotalQuantity") AndAlso Not IsDBNull(row("TotalQuantity")) Then
                sumQty += Convert.ToDouble(row("TotalQuantity"))
            ElseIf dt.Columns.Contains("TotalQuantityKg") AndAlso Not IsDBNull(row("TotalQuantityKg")) Then
                sumQty += Convert.ToDouble(row("TotalQuantityKg"))
            End If
        Next
        litTotalQuantity.Text = sumQty.ToString("0.#")

        gvReport.DataSource = dt
        gvReport.DataBind()
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerate.Click
        BindReportGrid()
    End Sub

    Protected Sub ddlReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlReportType.SelectedIndexChanged
        BindReportGrid()
    End Sub

    Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportExcel.Click
        BindReportGrid()
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=FoodBridge_" & ddlReportType.SelectedValue & "_" & DateTime.Now.ToString("yyyyMMdd") & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                hw.Write("<h2>FoodBridge - " & Server.HtmlEncode(ddlReportType.SelectedItem.Text) & "</h2>")
                hw.Write("<p>Total Records: " & litTotalRecords.Text & " | Total Quantity Summary: " & litTotalQuantity.Text & "</p>")
                gvReport.RenderControl(hw)
                Response.Output.Write(sw.ToString())
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Required for GridView export to Excel in ASP.NET Web Forms
    End Sub
End Class
