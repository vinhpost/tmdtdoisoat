Public Class RunMe1st_form
    Public pblUser As String = "", con As String = My.Settings.con
    Private Sub RunMe1st_form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("Bạn có chắc chắn?", "Kết thúc", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            e.Cancel = True
        Else
            CloseOpenForms()
            Application.ExitThread()
        End If
    End Sub
    Sub runform(ByVal form As DevExpress.XtraEditors.XtraForm)
        Try
            form.MdiParent = Me
            form.Icon = Me.Icon
            form.Show()
            form.Select()
        Catch ex As Exception
            TBL(ex.Message)
        End Try
    End Sub
    Private Sub RunMe1st_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IO.Directory.Exists("TMP") Then IO.Directory.CreateDirectory("TMP")
        If Not IO.Directory.Exists("Backup") Then IO.Directory.CreateDirectory("Backup")
        If Not IO.Directory.Exists("Excel") Then IO.Directory.CreateDirectory("Excel")
        SQL_GetValueFromSrv("if (Select count(*) From TBL_DM_USER) = 0  Insert into TBL_DM_USER (UserName,MatKhau,HoatDong,QuyenAdmin) values ('Admin','123',1,1)", con)
        Me.Hide()
        For i = 1 To 5
            Dim f As New frmLogin
            f.ShowDialog()
            Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT HoatDong FROM TBL_DM_USER WHERE USERNAME='{0}' AND MatKhau = '{1}'", f.TextEdit1.Text.Trim, f.TextEdit2.Text.Trim), con)
            If ok = "True" Then
                pblUser = f.TextEdit1.Text.Trim
                BarStaticItem1.Caption = "Nhân viên: " + pblUser
                Exit For
            End If
            f.Dispose()
        Next
        If pblUser = "" Then
            Application.ExitThread()
        Else
            Me.Show()
        End If
    End Sub
    Private Sub CloseOpenForms()
        Dim openForm As Form = Nothing
        For index As Integer = My.Application.OpenForms.Count - 1 To 0 Step -1
            openForm = My.Application.OpenForms.Item(index)
            If openForm IsNot Me AndAlso Not TypeOf openForm Is DevExpress.Utils.Win.TopFormBase Then
                openForm.Close()
                openForm.Dispose()
                openForm = Nothing
            End If
        Next
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Me.Close()
    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT QuyenAdmin FROM TBL_DM_USER WHERE USERNAME='{0}'", pblUser), con)
        If ok = "True" Then
            runform(User_form)
        Else
            Tb("Không có quyền")
        End If
    End Sub

    Private Sub BarButtonItem8_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem8.ItemClick
        Dim f As New frmDoiMatKhau
        f.ShowDialog()
        If f.CheckHuy.EditValue Then

        Else
            SQL_ExecCmd(String.Format("update TBL_DM_USER set MatKhau = '{0}' where USERNAME='{1}'", f.TextEdit3.EditValue, pblUser), con)
            Tb()
        End If
        f.Close()

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT QuyenQlData FROM TBL_DM_USER WHERE USERNAME='{0}'", pblUser), con)
        If ok = "True" Then
            runform(QuanLyData_form)
        Else
            Tb("Không có quyền")
        End If
    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem5.ItemClick
        Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT QuyenQlDm FROM TBL_DM_USER WHERE USERNAME='{0}'", pblUser), con)
        If ok = "True" Then
            runform(QuanLyDanhMuc_form)
        Else
            Tb("Không có quyền")
        End If
    End Sub

    Private Sub BarButtonItem4_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem4.ItemClick
        Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT QuyenNap FROM TBL_DM_USER WHERE USERNAME='{0}'", pblUser), con)
        If ok = "True" Then
            runform(NapSoLieu_form)
        Else
            Tb("Không có quyền")
        End If
    End Sub

    Private Sub BarButtonItem6_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem6.ItemClick
        Dim ok As String = SQL_GetValueFromSrv(String.Format("SELECT QuyenTk FROM TBL_DM_USER WHERE USERNAME='{0}'", pblUser), con)
        If ok = "True" Then
            runform(QuanLyBc_form)
        Else
            Tb("Không có quyền")
        End If
    End Sub

    Private Sub BarButtonItem9_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem9.ItemClick
        runform(TimKiemForm)
    End Sub
End Class