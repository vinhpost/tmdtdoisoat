
Public Class TimKiemForm
    Dim seek1, seek2, seek3, MaKhachHang As New DataTable
    Dim con As String = My.Settings.con

    Private Sub TimKiemForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Try
            Dim sohieu As String = ""
            If GetValueTimKiem(sohieu) Then  Else Exit Sub
            seek1 = SQL_GetDataFromSrv(String.Format("Select * from {1} where SoHieu in ('{0}')", sohieu, "TBL_CHAP_NHAN"), con)
            GridControl1.DataSource = Nothing
            GridView1.Columns.Clear()
            GridControl1.DataSource = seek1
            FormatGridColumn(GridView1, "")

            seek2 = SQL_GetDataFromSrv(String.Format("Select * from {1} where SoHieu in ('{0}')", sohieu, "TBL_TRANG_THAI"), con)
            GridControl2.DataSource = Nothing
            GridView2.Columns.Clear()
            GridControl2.DataSource = seek2
            FormatGridColumn(GridView2, "")

            seek3 = SQL_GetDataFromSrv(String.Format("Select * from {1} where SoHieu in ('{0}')", sohieu, "TBL_CHI_COD"), con)
            GridControl3.DataSource = Nothing
            GridView3.Columns.Clear()
            GridControl3.DataSource = seek3
            FormatGridColumn(GridView3, "")
        Catch ex As Exception

        End Try
        MaKhachHang = SQL_GetDataFromSrv(String.Format("Select Makh from TBL_DM_KH where MaUserDs = '{0}'", RunMe1st_form.pblUser), con)
    End Sub
    Private Sub GridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView1.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                Dim selectedRowHandles As Integer() = grdview.GetSelectedRows()
                If selectedRowHandles.Length > 0 Then
                    For i As Integer = 0 To selectedRowHandles.Length - 1
                        Dim r As DataRow = grdview.GetDataRow(selectedRowHandles(i))
                        For Each rr As DataRow In MaKhachHang.Rows
                            If String.Compare(r("MaKhachHang"), rr("makh"), True) Then
                                Tb("Không sửa được giao dịch của người khác quản lý")
                                Exit Sub
                            End If
                        Next
                    Next
                End If
                F2(grdview)
            Case Keys.F3
                Dim selectedRowHandles As Integer() = grdview.GetSelectedRows()
                If selectedRowHandles.Length > 0 Then
                    For i As Integer = 0 To selectedRowHandles.Length - 1
                        Dim r As DataRow = grdview.GetDataRow(selectedRowHandles(i))
                        For Each rr As DataRow In MaKhachHang.Rows
                            If String.Compare(r("MaKhachHang"), rr("makh"), True) Then
                                Tb("Không sửa được giao dịch của người khác quản lý")
                                Exit Sub
                            End If
                        Next
                    Next
                End If
                F3(grdview)
            Case Keys.F5
                F5(seek1)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData("TBL_CHAP_NHAN", seek1, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub
    Private Sub GridView2_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView2.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                F5(seek2)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData("TBL_TRANG_THAI", seek2, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub
    Private Sub GridView3_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView3.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                F5(seek3)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData("TBL_CHI_COD", seek3, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub

End Class