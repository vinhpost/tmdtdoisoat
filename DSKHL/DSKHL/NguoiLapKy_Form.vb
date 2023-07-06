Public Class NguoiLapKy_Form 
    Dim TBL_NGUOI_KY As New DataTable, con As String = My.Settings.DoiSoatConnectionString

    Private Sub NguoiLapKy_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TBL_NGUOI_KY = SQL_GetDataFromSrv(String.Format("select * from TBL_NGUOI_KY where UserName = '{0}'", frmMain.NGUOIDUNG), con)
        GridControl1.DataSource = TBL_NGUOI_KY
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim row As DataRow = TBL_NGUOI_KY.NewRow
        row("NguoiLap") = GetStringValue("Người lập biểu")
        row("NguoiKy") = GetStringValue("Người ký")
        row("NoiNhan") = GetStringValue("Nơi nhận")
        If row("NguoiLap") = "" And row("NguoiKy") = "" And row("NoiNhan") = "" Then Exit Sub
        row("UserName") = NGUOIDUNG
        TBL_NGUOI_KY.Rows.Add(row)
        AdapterUpdateData("TBL_NGUOI_KY", TBL_NGUOI_KY, con)
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Dim R As DataRow = GridView1.GetFocusedDataRow
        SaveConfig("NGUOI_LAP_BIEU", R("NguoiLap"))
        SaveConfig("NGUOI_KY", R("NguoiKy"))
        SaveConfig("NOI_NHAN", R("NoiNhan"))
        'R("Chọn") = True
        'DoUpdateTable(TBL_NGUOI_KYTableAdapter, Main_data)
        Me.Close()
    End Sub

    Private Sub GridView_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView1.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                Tb(PasteClipboard(TBL_NGUOI_KY))
                SendKeys.Send("{F6}")
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData("TBL_NGUOI_KY", TBL_NGUOI_KY, con)
        End Select
    End Sub
    Private Sub GridView_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If grdview.RowCount > 0 Then
            grdview.SelectRow(grdview.FocusedRowHandle)
            SendKeys.Send("{F2}")
        End If
    End Sub
End Class