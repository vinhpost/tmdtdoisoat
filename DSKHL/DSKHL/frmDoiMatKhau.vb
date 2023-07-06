Public Class frmDoiMatKhau 
    Dim con As String = My.Settings.con
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If TextEdit2.EditValue <> TextEdit3.EditValue Then
            Tb("Gõ lại mật khẩu mới không khớp")
            Exit Sub
        End If
        CheckHuy.EditValue = False
        Me.Hide()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        CheckHuy.EditValue = True
        Me.Hide()
    End Sub

End Class