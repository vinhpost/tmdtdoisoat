Public Class QuanLyDanhMuc_form 
    Dim dt, dt1, dt2 As New DataTable, loi As String = "", con As String = My.Settings.con, table As String = ""
    Private Sub QuanLyDanhMuc_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SearchLookUpEdit1.Properties.DataSource = SQL_GetDataFromSrv("exec SP_QL_DM 0", con)
        SearchLookUpEdit1.Properties.DisplayMember = "ChucNang"
        SearchLookUpEdit1.Properties.ValueMember = "ChucNang"
        SearchLookUpEdit1.Properties.View.Columns.AddVisible("ChucNang")

        RestoreAllValueInform(Me)
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Try
            GridControl10.DataSource = Nothing
            GridView10.Columns.Clear()
            table = SearchLookUpEdit1.Properties.View.GetFocusedDataRow("Bang")

            dt2 = SQL_GetDataFromSrv(String.Format("SP_QL_DM {0}", SearchLookUpEdit1.EditValue.ToString.Split(".")(0)), con)
            GridControl10.DataSource = dt2
            FormatGridColumn(GridView10, "cSoHieu")
            GridView10.BestFitColumns()
        Catch ex As Exception
            WaitClose()
            TBL(ex.Message)
        End Try
    End Sub
    Private Sub GridView10_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView10.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                F5(dt2)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData(table, dt2, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub
End Class