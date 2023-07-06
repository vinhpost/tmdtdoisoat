Public Class GetStringValue_form

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        CheckEdit1.EditValue = True
        Me.Hide()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        CheckEdit1.EditValue = False
        Me.Hide()
    End Sub

    Private Sub GetValue_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MemoEdit1.Focus()
    End Sub
End Class