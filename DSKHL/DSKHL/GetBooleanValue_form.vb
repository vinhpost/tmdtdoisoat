Public Class GetBooleanValue_form 

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        CheckEdit2.EditValue = True
        Me.Hide()
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        CheckEdit2.EditValue = False
        Me.Hide()
    End Sub

    Private Sub CheckEdit1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit1.CheckedChanged
        CheckEdit1.Text = If(CheckEdit1.EditValue, "Chọn", "Không chọn")
    End Sub

    Private Sub GetBooleanValue_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class