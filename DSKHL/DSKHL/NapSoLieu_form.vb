Public Class NapSoLieu_form 
    Dim dt, fieldtable, TrangThaiTBL As New DataTable, table As String = "", con As String = My.Settings.con
    Dim makharray As New List(Of String)
    Dim TN As String = "", TN2 As String = Nothing, Nhom As String = "", Ngay1 As DateTime = Date.Today, Ngay2 As DateTime = Date.Today
    Private Sub NapSoLieu_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBoxEdit2.Properties.DataSource = SQL_GetDataFromSrv("exec SP_NAP_SL '',0", con)
        ComboBoxEdit2.Properties.DisplayMember = "ChucNang"
        ComboBoxEdit2.Properties.ValueMember = "ChucNang"
        ComboBoxEdit2.Properties.View.Columns.AddVisible("ChucNang")
    End Sub
    Private Sub ButtonEdit1_Click(sender As Object, e As EventArgs) Handles ButtonEdit1.Click
        Dim files As String = Get_Files_Name_Excel()
        If files.Split(",").Length < 1 Then Exit Sub
        ButtonEdit1.EditValue = files
        LookUpEdit1.Enabled = False

        If Inlist(IO.Path.GetExtension(ButtonEdit1.EditValue.ToString.Split(",")(0)), ".xls,.xlsx,.xlsb,.csv") Then
            LookUpEdit1.Enabled = True
            LookUpEdit1.Properties.DataSource = GetSheetsName(ButtonEdit1.EditValue.ToString.Split(",")(0))
            LookUpEdit1.ItemIndex = 0
        End If
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        WaitCreate()
        Try
            Dim DTTAM As New DataTable, chara As String = ","
            GridView2.Columns.Clear()
            Select Case IO.Path.GetExtension(ButtonEdit1.EditValue.ToString.Split(",")(0))
                Case ".xls", ".xlsx", ".xlsb"
                    DTTAM = ImportExceltoDatatable(ButtonEdit1.EditValue.ToString.Split(",")(0), LookUpEdit1.EditValue)
                Case ".csv", ".txt"
                    chara = InputBox("Nhập ký tự", "Ký tự phân tách các trường số liệu", ",")
                    If chara = "" Then Exit Sub
                    DTTAM = Text_To_DataTable(ButtonEdit1.EditValue.ToString.Split(",")(0), chara, False)
                Case Else
                    Tb("Không hỗ trợ file dạng " + IO.Path.GetExtension(ButtonEdit1.EditValue.ToString.Split(",")(0)))
                    Exit Sub
            End Select

            If ButtonEdit1.EditValue.ToString.Split(",").Length > 1 Then
                For i = 1 To ButtonEdit1.EditValue.ToString.Split(",").Length - 1
                    WaitCaption(ButtonEdit1.EditValue.ToString.Split(",")(i))
                    Select Case IO.Path.GetExtension(ButtonEdit1.EditValue.ToString.Split(",")(i))
                        Case ".xls", ".xlsx", ".xlsb"
                            DTTAM.Merge(ImportExceltoDatatable(ButtonEdit1.EditValue.ToString.Split(",")(i), LookUpEdit1.EditValue))
                        Case ".csv", ".txt"
                            DTTAM.Merge(Text_To_DataTable(ButtonEdit1.EditValue.ToString.Split(",")(i), chara, False))
                    End Select
                Next
            End If

            GridControl2.DataSource = DTTAM
        Catch ex As Exception
            TBL(ex.Message)
            WaitClose()
            Exit Sub
        End Try
        WaitClose()
        Dim stt As Integer = 0
        For Each item As DevExpress.XtraGrid.Columns.GridColumn In GridView2.Columns
            item.FieldName = stt.ToString
            item.Caption = item.FieldName
            stt += 1
        Next
        GridView2.BestFitColumns()
    End Sub

    Private Sub ComboBoxEdit2_EditValueChanged(sender As Object, e As EventArgs) Handles ComboBoxEdit2.EditValueChanged
        dt.Reset()
        GridView3.Columns.Clear()
        table = ComboBoxEdit2.Properties.View.GetFocusedDataRow("Bang")
        dt = SQL_GetDataFromSrv(String.Format("Select * from {0} where 1=2", table), con)
        GridControl3.DataSource = dt
        FormatGridColumn(GridView3, "hIdds")
        For Each column As DataColumn In dt.Columns
            If Not Inlist(column.ColumnName, "ID,IDDS") Then
                SQL_ExecCmd(String.Format("IF NOT EXISTS(SELECT TOP 1 * FROM TBL_DM_TABLE_COLUME WHERE TableName = '{0}' AND FieldName = '{1}' And MenuId = '{2}') INSERT INTO TBL_DM_TABLE_COLUME (TableName,FieldName,ColumeNumber,MenuId,ColumType,MaxWidth,IsNull) VALUES ('{0}','{1}','','{2}','{3}',{4},'{5}')", table, column.ColumnName, ComboBoxEdit2.EditValue.ToString.Split(".")(0), column.DataType, column.MaxLength, column.AllowDBNull), con)
            End If
        Next
        fieldtable = SQL_GetDataFromSrv(String.Format("SELECT * FROM TBL_DM_TABLE_COLUME WHERE TableName='{0}' And MenuId = '{1}' Order By Id", table, ComboBoxEdit2.EditValue.ToString.Split(".")(0)), con)
        GridControl4.DataSource = fieldtable
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Dim dtkh As DataTable = SQL_GetDataFromSrv("Select Lower(MaKh) as MaKh From TBL_DM_KH", con)
        makharray.Clear()
        For Each r As DataRow In dtkh.Rows
            makharray.Add(r("MaKh"))
        Next
        Dim NewOk As Boolean = True
        AdapterUpdateData("TBL_DM_TABLE_COLUME", fieldtable, con)
        For Each r As DataRow In fieldtable.Rows
            If r("isNull") = False And r("ColumeNumber") = "" Then If TB_KETTHUC("Phải có cột " + r("FieldName")) Then Exit Sub
        Next
        '-------------------
        TrangThaiTBL = SQL_GetDataFromSrv("Select MaTrangThai,MoTa From TBL_DM_TRANG_THAI", con)
        dt.Clear()
        Dim ishow As Integer = GridView2.RowCount / 100, j As Integer = 0, loi As String = "", MaDonVi As String = "", NacTL As String = "", ThangNam As String = ""
        WaitCreate()
        For i = GridView2.RowCount - 1 To 0 Step -1
            NewOk = True
            j += 1
            If j = ishow Then
                WaitCaption(i.ToString)
                j = 0
            End If
            Dim r As DataRow = GridView2.GetDataRow(i)
            Try
                Dim dr As DataRow = dt.NewRow
                For jj = 0 To GridView4.RowCount - 1
                    Dim rr As DataRow = GridView4.GetDataRow(jj)
                    If rr("ColumeNumber").ToString = "" Then Continue For
                    Dim value = ""
                    If rr("ColumeNumber").ToString.StartsWith("@") Then
                        value = rr("ColumeNumber").ToString.Split("/")(0).TrimStart("@")
                    Else
                        value = r(ToInt(rr("ColumeNumber").ToString.Split("/")(0).TrimStart("*").TrimStart("@"))).ToString.TrimStart("""").TrimEnd("""")
                    End If
                    Select Case rr("ColumType")
                        Case "System.String"
                            Select Case rr("FieldName")
                                Case "TrangThai"
                                    dr(rr("FieldName")) = GetMa(value)
                                Case "MaKhachHang"
                                    dr(rr("FieldName")) = ""
                                    If CheckMAKH(value.ToLower) Then dr(rr("FieldName")) = value Else NewOk = False
                                Case Else
                                    dr(rr("FieldName")) = Mid(Trim(value), 1, rr("MaxWidth"))
                            End Select
                            If rr("ColumeNumber").ToString.StartsWith("*") And dr(rr("FieldName")) = "" Then NewOk = False
                        Case "System.DateTime"
                            'dạng date
                            Dim Dinhdang As String = rr("ColumeNumber").ToString.Split("/")(1)
                            Dim d As Date = ToDate(value, Dinhdang)

                            If rr("ColumeNumber").ToString.StartsWith("*") Then
                                If d.ToString("dd-MM-yyyy") = "01-01-0001" Then NewOk = False
                            End If
                            If d.ToString("dd-MM-yyyy") <> "01-01-0001" Then dr(rr("FieldName")) = d
                        Case Else
                            dr(rr("FieldName")) = ToDecimal(value)
                            If rr("ColumeNumber").ToString.StartsWith("*") And dr(rr("FieldName")) = 0 Then NewOk = False
                    End Select
                Next
                If NewOk Then
                    dt.Rows.Add(dr)
                    r.Delete()
                End If
            Catch ex As Exception
                loi += ex.Message + vbNewLine
            End Try
        Next
        WriteErrorLog(loi)
        ' GridControl3.DataSource = dt
        GridView3.BestFitColumns(200)
        WaitClose()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Try
            If GridView3.RowCount > 0 Then  Else Exit Sub
            Dim KhoaTrung = ComboBoxEdit2.Properties.View.GetFocusedDataRow("KhoaTrung")
            If ComboBoxEdit2.EditValue.ToString.Split(".")(0) > 0 Then  Else Exit Sub
            Dim thang As String = GridView3.GetFocusedRowCellValue("ThangNam")
            Dim sl As Integer = ToInt(SQL_GetValueFromSrv(String.Format("Select count(*) From {0} where ThangNam = '{1}'", table, thang), con))
            If sl > 0 Then If Not HoiLuaChon(String.Format("Đã có {0:n0} dòng số liệu, tiếp tục?", sl)) Then Exit Sub

            If ComboBoxEdit2.EditValue.ToString.Contains("Cập nhật") Then
                Dim BANGTAM As String = String.Format("{0}_TMP", table)
                SQL_ExecCmd(String.Format("DROP TABLE {0}_TMP SELECT TOP 0 * INTO {0}_TMP FROM {0}", table), con) 'ALTER TABLE {0}_TMP DROP COLUMN ID ALTER TABLE {0}_TMP ADD ID NUMERIC(18) IDENTITY(1,1) PRIMARY KEY
                SQL_BulkInsert(dt, BANGTAM, con)
                dt.Clear()
                Dim DKJOIN As String = "", FIELD As String = ""
                For i = 0 To GridView4.RowCount - 1
                    Dim r As DataRow = GridView4.GetDataRow(i)
                    If r("ColumeNumber") = "" Then Continue For
                    If r("ColumeNumber").ToString.Contains("!") Then
                        DKJOIN += String.Format(" a.{0} = b.{0} AND ", r("FieldName"))
                    End If
                    If r("ColumeNumber").ToString.Contains("!") Then
                        FIELD += String.Format(" a.{0} = b.{0},", r("FieldName"))
                    End If
                Next
                If DKJOIN <> "" Then DKJOIN = Mid(DKJOIN, 1, Len(DKJOIN) - 5) Else If TB_KETTHUC("Chưa có điều kiện join dòng") Then Exit Sub
                If FIELD <> "" Then FIELD = FIELD.TrimEnd(",") Else If TB_KETTHUC("Chưa có cột được ghép") Then Exit Sub
                Dim cmd = String.Format("TMP a set {3} from {0} a inner join {1} b on {2} ", table, BANGTAM, DKJOIN, FIELD)

                SQL_ExecCmd(cmd, con)
            Else
                SQL_TableInsert(dt, table, con, KhoaTrung)
                'SQL_ExecCmd("exec SP_SUVU_TMP", con)
            End If

        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Private Sub GridView4_RowCellClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles GridView4.RowCellClick
        If e.Button = Windows.Forms.MouseButtons.Right Then If GetColunmValue(GridView4) Then Exit Sub
    End Sub
    Private Sub GridView4_KeyDown(sender As Object, e As KeyEventArgs) Handles GridView4.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.F2 'them
                    Dim MenuId As String = ""
                    If GetStringValue(MenuId, "Đặt tên cho Profile mới") Then  Else Exit Sub
                    If MenuId = "" Then Exit Sub
                    For Each column As DataColumn In dt.Columns
                        If Not Inlist(column.ColumnName, "ID,IDDS") Then
                            SQL_ExecCmd(String.Format("IF NOT EXISTS(SELECT TOP 1 * FROM TBL_DM_TABLE_COLUME WHERE TableName = '{0}' AND FieldName = '{1}' And MenuId = '{2}') INSERT INTO TBL_DM_TABLE_COLUME (TableName,FieldName,ColumeNumber,MenuId,ColumType,MaxWidth,IsNull) VALUES ('{0}','{1}','','{2}','{3}',{4},'{5}')", table, column.ColumnName, MenuId, column.DataType, column.MaxLength, column.AllowDBNull), con)
                        End If
                    Next
                    SendKeys.Send("{F4}")
                Case Keys.F3 'xoa
                    Dim ImpProfile As String = ""
                    If ChonTrongDanhSach(ImpProfile, "TBL_DM_TABLE_COLUME", "MenuId", con, 0, String.Format("TableName = '{0}'", table)) Then  Else Exit Sub
                    If HoiXoa("Muốn xoá Profile cấu hình " + ImpProfile.TrimStart("@")) Then
                        SQL_ExecCmd(String.Format("Delete From TBL_DM_TABLE_COLUME Where TableName='{0}' And MenuId = '{1}'", table, ImpProfile.TrimStart("@")), con)
                    End If
                    SendKeys.Send("{F4}")
                Case Keys.F4 'chọn
                    Dim ImpProfile As String = ""
                    If ChonTrongDanhSach(ImpProfile, "TBL_DM_TABLE_COLUME", "MenuId", con, 0, String.Format("TableName = '{0}'", table)) Then  Else Exit Sub
                    fieldtable.Clear()
                    fieldtable.Merge(SQL_GetDataFromSrv(String.Format("SELECT * FROM TBL_DM_TABLE_COLUME WHERE TableName='{0}' And MenuId = N'{1}' Order By Id", table, ImpProfile.TrimStart("@")), con))
            End Select
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetMa(mota As String) As String
        For Each r As DataRow In TrangThaiTBL.Rows
            For Each rr As String In r(1).ToString.Split(",")
                If String.Compare(mota, rr, True) = 0 Then Return r(0)
            Next
        Next
        Return ""
    End Function
    Function CheckMAKH(Makh) As Boolean
        If makharray.Contains(Makh) Then Return True Else Return False
    End Function
    Private Function CheckKhoa(Thang As String) As Boolean
        Dim TN As Decimal = 0
        TN = ToDecimal(SQL_GetValueFromSrv(String.Format("Select isnull(count(*),0) From {1} where ThangNam = '{0}'", Thang, table), con))
        If ToDecimal(TN) > 0 Then
            If HoiLuaChon(String.Format("Đã có {0} dòng, tiếp tục cập nhật", TN)) Then Return True Else Return False
        End If
        Return True
    End Function

End Class