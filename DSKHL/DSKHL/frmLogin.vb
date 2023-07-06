Imports System.Net
Public Class frmLogin
    Private Sub frmLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If CheckEdit1.EditValue = True Then
            SaveConfig("LOGINNAME", TextEdit1.EditValue)
            SaveConfig("LOGINPASS", TextEdit2.EditValue)
        End If
        My.Settings.LOGINSAVE = CheckEdit1.EditValue
        My.Settings.Save()
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckEdit1.EditValue = My.Settings.LOGINSAVE
        'check option save password
        If CheckEdit1.EditValue Then
            TextEdit1.EditValue = GetConFig("LOGINNAME")
            TextEdit2.EditValue = GetConFig("LOGINPASS")
        Else
            TextEdit1.EditValue = ""
            TextEdit2.EditValue = ""
        End If
        'frmMain.CHECKUPDATE()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If TextEdit1.Text.Trim = "" Or TextEdit2.Text.Trim = "" Then
            MsgBox("Nhập đầy đủ username và password", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Đăng nhập")
            Me.TextEdit1.Focus()
            Me.TextEdit1.SelectAll()
            Exit Sub
        End If
        Me.Hide()
    End Sub

End Class