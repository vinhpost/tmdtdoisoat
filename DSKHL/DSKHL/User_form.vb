Public Class User_form 
    Dim con As String = My.Settings.con
    Dim dt As New DataTable
    Private Sub User_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dt = SQL_GetDataFromSrv("select * from Tbl_Dm_User", con)
        GridControl1.DataSource = dt
    End Sub
    Private Sub GridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView1.KeyDown
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Select Case e.KeyCode
            Case Keys.F2
                F2(grdview)
            Case Keys.F3
                F3(grdview)
            Case Keys.F5
                F5(dt)
            Case Keys.F8
                ToExCel(grdview)
            Case Keys.F6
                AdapterUpdateData("Tbl_Dm_User", dt, con)
            Case Keys.C And e.Modifiers = Keys.Shift
                F10(grdview, e)
        End Select
    End Sub
    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim grdview As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If grdview.RowCount > 0 Then
            grdview.SelectRow(grdview.FocusedRowHandle)
            SendKeys.Send("{F2}")
        End If
    End Sub
End Class