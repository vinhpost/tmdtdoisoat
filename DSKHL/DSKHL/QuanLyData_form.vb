Public Class QuanLyData_form 
    Dim dt, dt1, dt2 As New DataTable, loi As String = "", con As String = My.Settings.con
    Dim TrangThaiTBL As New DataTable, fieldtable As New DataTable, table As String = ""
    Dim TN As String = "", TN2 As String = Nothing, Nhom As String = "", Ngay1 As DateTime = Date.Today, Ngay2 As DateTime = Date.Today
    Private Sub QuanLyData_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBoxEdit3.Properties.DataSource = SQL_GetDataFromSrv("exec SP_QL '',0,''", con)
        ComboBoxEdit3.Properties.DisplayMember = "ChucNang"
        ComboBoxEdit3.Properties.ValueMember = "ChucNang"
        ComboBoxEdit3.Properties.View.Columns.AddVisible("ChucNang")

    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        WaitCreate()
        Try
            If ComboBoxEdit3.EditValue.ToString.ToLower.Contains("tháng") Then If GetThangNam(TN) Then  Else Exit Sub
            If ComboBoxEdit3.EditValue.ToString.ToLower.Contains("ngày") Then
                If GetTuNgay(Ngay1, Ngay2) Then
                    TN = Ngay_Thang_Nam(Ngay1) + Ngay_Thang_Nam(Ngay2)
                Else
                    Exit Sub
                End If
            End If
            ' SQL_ExecCmd(String.Format("BEGIN TRY DROP TABLE nhomtmp END TRY BEGIN CATCH END CATCH; select distinct a.MaKhachHang,b.PhanNhom into nhomtmp from TBL_CHAP_NHAN a left join TBL_DM_MAKH b on a.MaKhachHang = b.MaKh where thangnam = '{0}'", TN), con)
            If ChonNhieuTrongDanhSach(Nhom, "TBL_DM_KH", "MaKh,TenKh,Nhom", con, 0, " MaUserDs ='" + RunMe1st_form.pblUser + "'") Then  Else Exit Sub
            GridControl5.DataSource = Nothing
            GridView5.Columns.Clear()
            table = ComboBoxEdit3.Properties.View.GetFocusedDataRow("Bang")

            dt1 = SQL_GetDataFromSrv(String.Format("EXEC SP_QL  '{0}','{1}','{2}'", TN, ComboBoxEdit3.EditValue.ToString.Split(".")(0), Nhom.Replace("'", "")), con)
            GridControl5.DataSource = dt1
            FormatGridColumn(GridView5, "cSoHieu")
            GridView5.BestFitColumns()
        Catch ex As Exception
            WaitClose()
            TBL(ex.Message)
        End Try
        WaitClose()
    End Sub
    Private Sub GridView5_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView5.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                f5(dt1)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData(table, dt1, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub
    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs)
        Try
            table = ComboBoxEdit3.Properties.View.GetFocusedDataRow("Bang")
            If table = "" Then Exit Sub
            Dim sohieu As String = ""
            If GetValueTimKiem(sohieu) Then  Else Exit Sub
            dt1 = SQL_GetDataFromSrv(String.Format("Select * from {1} where SoHieu in ('{0}')", sohieu, table), con)
            GridControl5.DataSource = Nothing
            GridView5.Columns.Clear()
            GridControl5.DataSource = dt1
            FormatGridColumn(GridView5, "")
        Catch ex As Exception

        End Try

    End Sub
    Private Sub GridView5_DoubleClick(sender As Object, e As EventArgs) Handles GridView5.DoubleClick
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If grdview.RowCount > 0 Then
            grdview.SelectRow(grdview.FocusedRowHandle)
            SendKeys.Send("{F2}")
        End If
    End Sub
End Class