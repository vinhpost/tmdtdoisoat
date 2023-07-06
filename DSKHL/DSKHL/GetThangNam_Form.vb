Public Class GetThangNam_Form

    Private Sub GetThangNam_Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveConfig(Me.Name, RadioGroup2.Properties.Items(0).Description + ";" + RadioGroup2.Properties.Items(1).Description + ";" + RadioGroup2.Properties.Items(2).Description + ";" + RadioGroup2.Properties.Items(3).Description)
    End Sub

    Private Sub GetThangNam_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            RadioGroup2.Properties.Items(0).Description = GetConFig(Me.Name).ToString.Split(";")(0)
            RadioGroup2.Properties.Items(1).Description = GetConFig(Me.Name).ToString.Split(";")(1)
            RadioGroup2.Properties.Items(2).Description = GetConFig(Me.Name).ToString.Split(";")(2)
            RadioGroup2.Properties.Items(3).Description = GetConFig(Me.Name).ToString.Split(";")(3)
        Catch ex As Exception
            RadioGroup2.Properties.Items(0).Description = "2020"
            RadioGroup2.Properties.Items(1).Description = "2021"
            RadioGroup2.Properties.Items(2).Description = "2022"
            RadioGroup2.Properties.Items(3).Description = "2023"
        End Try

        RadioGroup2.Properties.Items(0).Value = RadioGroup2.Properties.Items(0).Description
        RadioGroup2.Properties.Items(1).Value = RadioGroup2.Properties.Items(1).Description
        RadioGroup2.Properties.Items(2).Value = RadioGroup2.Properties.Items(2).Description
        RadioGroup2.Properties.Items(3).Value = RadioGroup2.Properties.Items(3).Description
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        CheckEdit1.EditValue = True
        Me.Hide()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        CheckEdit1.EditValue = False
        Me.Hide()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        RadioGroup2.Properties.Items(0).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(0).Description) + 1)
        RadioGroup2.Properties.Items(0).Value = RadioGroup2.Properties.Items(0).Description
        RadioGroup2.Properties.Items(1).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(1).Description) + 1)
        RadioGroup2.Properties.Items(1).Value = RadioGroup2.Properties.Items(1).Description
        RadioGroup2.Properties.Items(2).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(2).Description) + 1)
        RadioGroup2.Properties.Items(2).Value = RadioGroup2.Properties.Items(2).Description
        RadioGroup2.Properties.Items(3).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(3).Description) + 1)
        RadioGroup2.Properties.Items(3).Value = RadioGroup2.Properties.Items(3).Description
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        RadioGroup2.Properties.Items(0).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(0).Description) - 1)
        RadioGroup2.Properties.Items(0).Value = RadioGroup2.Properties.Items(0).Description
        RadioGroup2.Properties.Items(1).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(1).Description) - 1)
        RadioGroup2.Properties.Items(1).Value = RadioGroup2.Properties.Items(1).Description
        RadioGroup2.Properties.Items(2).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(2).Description) - 1)
        RadioGroup2.Properties.Items(2).Value = RadioGroup2.Properties.Items(2).Description
        RadioGroup2.Properties.Items(3).Description = String.Format("{0:00}", ToInt(RadioGroup2.Properties.Items(3).Description) - 1)
        RadioGroup2.Properties.Items(3).Value = RadioGroup2.Properties.Items(3).Description
    End Sub

End Class