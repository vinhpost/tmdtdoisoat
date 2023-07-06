Public Class ChonDm_Form 
    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        GridView1.SelectRow(GridView1.FocusedRowHandle)
        SimpleButton1.PerformClick()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'strValue = GridView1.GetFocusedDisplayText
        CheckHuy.EditValue = False
        Me.Hide()
    End Sub

    Private Sub ChonDm_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Me.Hide()
        CheckHuy.EditValue = True
    End Sub
End Class
