Imports DevExpress.XtraReports.UI
Public Class QuanLyBc_form
    Dim dt, dt1, dt2 As New DataTable, loi As String = "", con As String = My.Settings.con
    Dim TrangThaiTBL As New DataTable, fieldtable As New DataTable, table As String = ""
    Dim makharray As New List(Of String)
    Dim TN As String = "", TN2 As String = Nothing, Nhom As String = "", Ngay1 As DateTime = Date.Today, Ngay2 As DateTime = Date.Today
    Private Sub main_form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveAllValueInForm(Me)
    End Sub
    Private Sub main_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBoxEdit4.Properties.DataSource = SQL_GetDataFromSrv("exec SP_TK_BC '', 0,''", con)
        ComboBoxEdit4.Properties.DisplayMember = "ChucNang"
        ComboBoxEdit4.Properties.ValueMember = "ChucNang"
        ComboBoxEdit4.Properties.View.Columns.AddVisible("ChucNang")
        RestoreAllValueInform(Me)


    End Sub

    Private Sub GridView6_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView6.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F9
                '        Select Case ComboBoxEdit4.EditValue.ToString.Split(".")(0)
                '            Case 1
                '                BAOCAO1()
                '            Case 2
                '                BaoCao2()
                '            Case 5
                '                BaoCao5()
                '            Case 6
                '                BaoCao6()
                '        End Select
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Try
            If ComboBoxEdit4.EditValue.ToString.ToLower.Contains("tháng") Then If GetThangNam(TN) Then  Else Exit Sub
            If ComboBoxEdit4.EditValue.ToString.ToLower.Contains("ngày") Then
                If GetTuNgay(Ngay1, Ngay2) Then
                    TN = Ngay_Thang_Nam(Ngay1) + Ngay_Thang_Nam(Ngay2)
                Else
                    Exit Sub
                End If
            End If
            If ChonNhieuTrongDanhSach(Nhom, "TBL_DM_KH", "MaKh,TenKh,Nhom", con, 0, " MaUserDs ='" + RunMe1st_form.pblUser + "'") Then  Else Exit Sub

            GridView6.Columns.Clear()
            Dim kq As New DataTable
            kq = SQL_GetDataFromSrv(String.Format("EXEC SP_TK_BC '{0}','{1}','{2}'", TN, ComboBoxEdit4.EditValue.ToString.Split(".")(0), Nhom.Replace("'", "")), con)
            GridControl6.DataSource = kq
            FormatGridColumn(GridView6, "cSOHIEU")
            GridView6.Focus()
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Private Sub GridView6_DoubleClick(sender As Object, e As EventArgs) Handles GridView6.DoubleClick
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If grdview.RowCount > 0 Then
            grdview.SelectRow(grdview.FocusedRowHandle)
            SendKeys.Send("{F2}")
        End If
    End Sub

End Class