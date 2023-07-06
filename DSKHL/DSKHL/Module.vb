Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Linq
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.Office.Interop

Module Modules
    Public NGUOIDUNG As String, FULLNAME As String, CAPNGUOIDUNG As String
    Public tmpPath As String = Application.StartupPath + "\TMP"
    Public Function FieldContains(Field As String, WordList As String) As Boolean
        If Len(WordList) = 0 Or Len(Field) = 0 Then Return True
        For Each Val As String In WordList.Split(",")
            If Field.ToLower.Contains(Val.ToLower) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function nvl(i As Object, j As Object) As Object
        Return If(IsDBNull(i), j, i)
    End Function
    Public Function GetAllControl(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllControl(ctrl, type)).Concat(controls).Where(Function(c) c.GetType() Is type)
    End Function
    ''' <param name="grv">Gridview name.</param>
    ''' <remarks>
    ''' </remarks>
    Public Sub FormatGridColumn(grv As GridView, Fields As String)
        Try
            For Each column As DevExpress.XtraGrid.Columns.GridColumn In grv.Columns
                If column.FieldName.ToUpper = "ID" Or column.FieldName.ToUpper = "IDROW" Then
                    column.Visible = False
                    Continue For
                End If
                If column.ColumnType.ToString = "System.Decimal" Or column.ColumnType.ToString = "System.Int32" Then
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                    column.DisplayFormat.FormatString = "{0:n0}"
                    column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                    column.SummaryItem.DisplayFormat = "{0:n0}"
                End If
                Select Case column.FieldName.ToLower
                    Case "tl", "trongluong", "khoiluong", "kl_min", "kl_max", "klmin", "klmax"
                        column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                        column.DisplayFormat.FormatString = "{0:n}"
                        column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                        column.SummaryItem.DisplayFormat = "{0:n}"
                    Case Else
                        If FieldContains(column.FieldName, "luong,weight,tyle,usd,sdr,eur") Then
                            column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            column.DisplayFormat.FormatString = "{0:n}"
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                            column.SummaryItem.DisplayFormat = "{0:n}"
                        End If
                        If FieldContains(column.FieldName, "sohieu,vandon") Then
                            column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                            column.DisplayFormat.FormatString = "{0:n}"
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
                            column.SummaryItem.DisplayFormat = "{0:n}"
                        End If
                        If column.ColumnType.ToString = "System.Date" Or column.ColumnType.ToString = "System.DateTime" Then
                            column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                            column.DisplayFormat.FormatString = "{0:dd-MM-yyyy}"
                        End If
                End Select
                Dim f() As String = Fields.Split(",")
                For i = 0 To f.Count - 1
                    Dim Field As String = Mid(f(i), 2).ToUpper, type As String = Mid(f(i), 1, 1).ToString.ToUpper
                    If column.FieldName.ToString.ToUpper = Field Then
                        Select Case type
                            Case "I" 'có thập phân
                                column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                column.DisplayFormat.FormatString = "{0:n}"
                                column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                                column.SummaryItem.DisplayFormat = "{0:n}"
                            Case "D" 'có thập phân
                                column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
                                column.DisplayFormat.FormatString = "{0:n}"
                                column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                                column.SummaryItem.DisplayFormat = "{0:n}"
                            Case "C"
                                column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
                                column.SummaryItem.DisplayFormat = "{0:n0}"
                            Case "H" 'cột ẩn
                                column.Visible = False
                        End Select
                    End If
                Next
            Next
            grv.BestFitColumns(100)
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Function CheckForAlphaCharacters(ByVal StringToCheck As String) As Boolean
        For i = 0 To StringToCheck.Length - 1
            If Char.IsLetter(StringToCheck.Chars(i)) Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function AddDay(ByVal dat As Date, SoNgay As Integer) As Date
        Return dat.AddDays(SoNgay)
    End Function
    Private Sub ShowAll(ByVal parentCtr As Control)
        Try
            For Each ctr As Control In parentCtr.Controls
                Console.Write(ctr.Name)
                ShowAll(ctr)
            Next
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Sub SaveAllValueInForm(ByRef frm As XtraForm)
        Dim c = GetAllControl(frm, GetType(ComboBoxEdit)), VALUE2SAVE As String = ""
        Try
            For i = 0 To c.Count - 1
                Dim CI As ComboBoxEdit = TryCast(c(i), ComboBoxEdit)
                VALUE2SAVE += String.Format(";{0},{1}", CI.Name, CI.EditValue)
            Next
        Catch ex As Exception

        End Try

        Dim d = GetAllControl(frm, GetType(DateEdit))
        Try
            For i = 0 To d.Count - 1
                Dim DI As DateEdit = TryCast(d(i), DateEdit)
                Dim ngay As Date = DI.EditValue
                VALUE2SAVE += String.Format(";{0},{1:MM/dd/yyyy}", DI.Name, ngay)
            Next
        Catch ex As Exception

        End Try

        Dim e = GetAllControl(frm, GetType(TextEdit))
        Try
            For i = 0 To e.Count - 1
                Dim EI As TextEdit = TryCast(e(i), TextEdit)
                VALUE2SAVE += String.Format(";{0},{1}", EI.Name, EI.EditValue)
            Next
        Catch ex As Exception

        End Try
        Dim F = GetAllControl(frm, GetType(RadioGroup))
        Try
            For i = 0 To F.Count - 1
                Dim CI As RadioGroup = TryCast(F(i), RadioGroup)
                VALUE2SAVE += String.Format(";{0},{1}", CI.Name, CI.EditValue)
            Next
        Catch ex As Exception

        End Try

        Dim G = GetAllControl(frm, GetType(LookUpEdit))
        Try
            For i = 0 To G.Count - 1
                Dim GI As LookUpEdit = TryCast(G(i), LookUpEdit)
                VALUE2SAVE += String.Format(";{0},{1}", GI.Name, GI.Text)
            Next
        Catch ex As Exception

        End Try

        Dim G1 = GetAllControl(frm, GetType(SearchLookUpEdit))
        Try
            For i = 0 To G1.Count - 1
                Dim GI1 As SearchLookUpEdit = TryCast(G1(i), SearchLookUpEdit)
                VALUE2SAVE += String.Format(";{0},{1}", GI1.Name, GI1.EditValue)
            Next
        Catch ex As Exception

        End Try


        SaveConfig(frm.Name, VALUE2SAVE.TrimStart(";"))
    End Sub
    Public Sub RestoreAllValueInform(ByRef frm As XtraForm)
        Try
            Dim CON As String() = GetConFig(frm.Name).ToString.Split(";")
            For I = 0 To CON.Length - 1
                Dim CON2 As String() = CON(I).Split(",")
                Dim CONNAME As String = CON2(0), CONVALUE As String = CON2(1)
                Try
                    Dim cc = GetAllControl(frm, GetType(ComboBoxEdit))
                    For j = 0 To cc.Count - 1
                        Dim ccc As ComboBoxEdit = TryCast(cc(j), ComboBoxEdit)
                        If String.Compare(ccc.Name, CONNAME, True) = 0 Then
                            ccc.EditValue = CONVALUE
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
                Try
                    Dim d = GetAllControl(frm, GetType(DateEdit))
                    For j = 0 To d.Count - 1
                        Dim cj As DateEdit = TryCast(d(j), DateEdit)
                        If String.Compare(cj.Name, CONNAME, True) = 0 Then
                            cj.EditValue = DateTime.Parse(CONVALUE)
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
                Try
                    Dim e = GetAllControl(frm, GetType(TextEdit))
                    For j = 0 To e.Count - 1
                        Dim ej As TextEdit = TryCast(e(j), TextEdit)
                        If String.Compare(ej.Name, CONNAME, True) = 0 Then
                            ej.EditValue = CONVALUE
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
                Try
                    Dim F = GetAllControl(frm, GetType(RadioGroup))
                    For j = 0 To F.Count - 1
                        Dim Fj As RadioGroup = TryCast(F(j), RadioGroup)
                        If String.Compare(Fj.Name, CONNAME, True) = 0 Then
                            Fj.EditValue = CONVALUE
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
                Try
                    Dim g = GetAllControl(frm, GetType(DevExpress.XtraEditors.LookUpEdit))
                    For j = 0 To g.Count - 1
                        Dim gj As LookUpEdit = TryCast(g(j), DevExpress.XtraEditors.LookUpEdit)
                        If String.Compare(gj.Name, CONNAME, True) = 0 Then
                            gj.Text = CONVALUE
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
                'Try
                '    Dim h = GetAllControl(frm, GetType(DevExpress.XtraEditors.SearchLookUpEdit))
                '    For j = 0 To h.Count - 1
                '        Dim hj As SearchLookUpEdit = TryCast(h(j), DevExpress.XtraEditors.SearchLookUpEdit)
                '        If String.Compare(hj.Name, CONNAME, True) = 0 Then
                '            hj.EditValue = CONVALUE
                '            Exit For
                '        End If
                '    Next
                'Catch ex As Exception

                'End Try
            Next
        Catch ex As Exception
            'Tb(ex.Message)
        End Try
    End Sub
    Public Function Inlist(ByVal sValue As String, slist As String) As Boolean
        Try
            sValue = sValue.ToUpper.Replace(" ", "")
            slist = slist.ToUpper.Replace(" ", "")
            Dim cc As String() = slist.Split(",")
            If cc.Contains(sValue) Then
                Return True
            End If
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return False
    End Function
    Public Sub XOA_DONG_DA_NAP(ByRef TABLE As System.Data.DataTable, Optional ByVal f As String = "NAPOK")
        Try
            Dim DTTAM As System.Data.DataTable = TABLE.Select(f & "=FALSE").CopyToDataTable
            TABLE.Clear()
            TABLE.Merge(DTTAM)
            DTTAM.Dispose()
        Catch ex As Exception
            TABLE.Clear()
            WriteErrorLog(ex.Message)
        End Try
    End Sub

    Public Function Excel2Excel(ByVal FileName As String) As String
        If IO.Directory.Exists("Excel") Then  Else IO.Directory.CreateDirectory("Excel")
        Dim officeType As Type = Type.GetTypeFromProgID("Excel.Application")

        ' Excel is not installed.
        ' Show message or alert that Excel is not installed.
        If officeType Is Nothing Then
            TBL("Excel 2010 chưa được cài đặt")
            Return ""
        End If
        Dim newFileName As String = ""
        Dim app As New Microsoft.Office.Interop.Excel.Application()
        Try
            ' Excel is installed.
            ' Let us continue our work on Excel file conversion. 
            app.DisplayAlerts = False
            ' Now we open the upload file in Excel Workbook. 
            Dim excelWorkbook As Microsoft.Office.Interop.Excel.Workbook = app.Workbooks.Open(FileName)

            newFileName = IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Excel")
            newFileName = IO.Path.Combine(newFileName, IO.Path.GetFileName(FileName) + ".xlsx")
            ' Now save this file as EXCEL file.
            excelWorkbook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12)
            'excelWorkbook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.)

            ' Close the Workbook and Quit the Excel Application at the end. 
            excelWorkbook.Close()
            app.Quit()
        Catch ex As Exception
            app.Quit()
            Return ""
        End Try
        Return newFileName
    End Function
    Public Function Get_Files_Name(Optional ByVal path As String = "") As List(Of String)
        Dim ret As New List(Of String)
        Try
            Dim ofd As New OpenFileDialog, filename As String = ""
            ofd.InitialDirectory = path
            ofd.Multiselect = True
            ofd.Title = "Hãy chọn file..."
            ofd.Filter = "All files (*.*)|*.*"
            If ofd.ShowDialog = DialogResult.OK Then
                For Each f As String In ofd.FileNames
                    ret.Add(f)
                Next
            End If
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return ret
    End Function
    Public Function Get_Files_Name_Excel(Optional ByVal DefaultPath As String = "") As String
        Try
            Dim ofd As New OpenFileDialog, filename As String = ""
            ofd.Title = "Hãy chọn file Excel"
            ofd.Filter = "Excel files (*.xls, *.xlsx, *.xlsb)|*.xls;*.xlsx;*.xlsb|All files (*.*)|*.*"
            ofd.InitialDirectory = DefaultPath
            ofd.Multiselect = True
            If ofd.ShowDialog = DialogResult.OK Then
                For Each f As String In ofd.FileNames
                    filename = String.Format(filename + "{0},", f)
                Next
            End If
            filename = filename.TrimEnd(",")
            Return filename
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return ""
    End Function
    Public Function Get_Files_Name_Zip(Optional ByVal DefaultPath As String = "") As String
        Try
            Dim ofd As New OpenFileDialog, filename As String = ""
            ofd.Title = "Hãy chọn file nén ZIP"
            ofd.Filter = "Excel files (*.zip)|*.zip|All files (*.*)|*.*"
            ofd.Multiselect = True
            ofd.InitialDirectory = DefaultPath
            If ofd.ShowDialog = DialogResult.OK Then
                For Each f As String In ofd.FileNames
                    filename = String.Format(filename + "{0},", f)
                Next
            End If
            filename = filename.TrimEnd(",")
            Return filename
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return ""
    End Function
    Public Function Get_Files_Name_All(Optional ByVal DefaultPath As String = "") As String
        Dim ofd As New OpenFileDialog, filename As String = ""
        ofd.Title = "Hãy chọn file..."
        ofd.Filter = "All files (*.*)|*.*"
        ofd.Multiselect = True
        ofd.InitialDirectory = DefaultPath
        If ofd.ShowDialog = DialogResult.OK Then
            For Each f As String In ofd.FileNames
                filename = String.Format(filename + "{0},", f)
            Next
        End If
        Try
            filename = filename.TrimEnd(",")
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return filename
    End Function
    Public Sub Add_Col(ByRef table As System.Data.DataTable, ByVal TEN_CAC_COT As String)
        'ký tự đầu tiên của TEN_CAC_COT mô tả định dạnh của cột
        'vi du: "sMABC,dSOTIEN,iSTT"
        Dim c As String() = TEN_CAC_COT.Split(",")
        For i = 0 To c.Length - 1
            Dim type As String = Left(c(i), 1)
            Dim column As DataColumn = New DataColumn(Mid(c(i), 2))
            Select Case type
                Case "D", "d"
                    'column.DataType = System.Type.GetType("System.Decimal")
                    column.DataType = System.Type.GetType("System.Decimal")
                    column.DefaultValue = 0
                Case "t", "T"
                    column.DataType = System.Type.GetType("System.DateTime")
                Case "B", "b"
                    column.DataType = System.Type.GetType("System.Boolean")
                    column.DefaultValue = False
                Case Else
                    column.DataType = System.Type.GetType("System.String")
                    column.DefaultValue = ""
            End Select
            table.Columns.Add(column)
        Next
    End Sub
    Public Function GetConFig(ByVal key As String)
        Dim fileName As String = System.Windows.Forms.Application.StartupPath + "\setting.xml"
        If Not File.Exists(fileName) Then Return ""
        Dim config As New System.Data.DataTable
        config.ReadXml(fileName)
        Dim value As String = ""
        For Each r As DataRow In config.Rows
            If r("key").ToString = key.ToString Then
                value = r("value")
                Exit For
            End If
        Next
        Return value
    End Function
    Public Function SaveConfig(ByVal key As String, ByVal value As Object) As Boolean
        Dim fileName As String = System.Windows.Forms.Application.StartupPath + "\setting.xml"
        Try
            If Not File.Exists(fileName) Then
                Dim cf As New System.Data.DataTable("Config")
                cf.Columns.Add("Key")
                cf.Columns.Add("Value", Type.GetType("System.Object"))
                cf.WriteXml(fileName, XmlWriteMode.WriteSchema)
                cf.Dispose()
            End If
            Dim config As New System.Data.DataTable
            config.ReadXml(fileName)
            Dim found As Boolean = False
            For Each r As DataRow In config.Rows
                If String.Compare(r("key").ToString, key.ToString, True) = 0 Then
                    r("value") = value
                    found = True
                    Exit For
                End If
            Next
            If Not found Then config.Rows.Add(key, value)
            config.WriteXml(fileName, XmlWriteMode.WriteSchema)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub Delete_Files(ByVal Folder As String)
        If Not Directory.Exists(Folder) Then Directory.CreateDirectory(Folder)
        For Each filepath As String In Directory.GetFiles(Folder)
            File.Delete(filepath)
        Next

    End Sub
    Public Function UnZip(ByVal ZipToUnpack As String, Optional ByVal TargetDir As String = "")
        If TargetDir = "" Then TargetDir = String.Format("{0}\TMP", System.Windows.Forms.Application.StartupPath)
        If Not Directory.Exists(TargetDir) Then Directory.CreateDirectory(TargetDir)
        Try
            For Each filepath As String In Directory.GetFiles(TargetDir, "*.*", IO.SearchOption.AllDirectories)
                File.Delete(filepath)
            Next
            If String.Compare(Path.GetExtension(ZipToUnpack), ".zip", True) < 0 Then
                File.Copy(ZipToUnpack, Path.Combine(TargetDir, Path.GetFileName(ZipToUnpack)))
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try

        Try
            Using zip1 As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(ZipToUnpack)
                ' here, we extract every entry, but we could extract    
                ' based on entry name, size, date, etc.   
                For Each e11 As Ionic.Zip.ZipEntry In zip1
                    e11.Extract(TargetDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                Next
            End Using
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Public Sub WaitCreate()
        Try
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(GetType(WaitForm))
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("Hệ thống đang bận...")
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WaitClose()
        Try
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(False)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WaitCaption(ByVal Caption As String)
        Try
            DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption(Caption)
        Catch ex As Exception

        End Try
    End Sub
    Function GetDataMDB(ByVal sfile As String, ByVal sTablename As String) As System.Data.DataTable
        If Not DoesTableExist(sTablename, sfile) Then Return Nothing

        Try
            Dim Table_ As String = "Table1"
            Dim query As String = "SELECT * FROM " & sTablename
            'Dim MDBConnString_ As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & sfile
            Dim MDBConnString_ As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sfile

            Dim ds As New DataSet
            Dim cnn As System.Data.OleDb.OleDbConnection = New System.Data.OleDb.OleDbConnection(MDBConnString_)
            cnn.Open()
            Dim cmd As New System.Data.OleDb.OleDbCommand(query, cnn)
            Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)
            da.Fill(ds, Table_)
            cnn.Close()
            Dim t1 As System.Data.DataTable = ds.Tables(Table_)
            Return t1
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function DoesTableExist(ByVal tblName As String, ByVal sfilename As String) As Boolean
        ' For Access Connection String,
        ' use "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" &
        ' accessFilePathAndName

        ' Open connection to the database
        Dim dbConn As New System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sfilename)
        dbConn.Open()

        Dim restrictions(3) As String
        restrictions(2) = tblName
        Dim dbTbl As System.Data.DataTable = dbConn.GetSchema("Tables", restrictions)

        DoesTableExist = True
        If dbTbl.Rows.Count = 0 Then DoesTableExist = False

        dbTbl.Dispose()
        dbConn.Close()
        dbConn.Dispose()

    End Function
    Public Function DoesFieldExist(ByVal tblName As String, ByVal fldName As String, ByVal sfile As String) As Boolean
        ' For Access Connection String,
        ' use "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" &
        ' accessFilePathAndName

        ' Open connection to the database
        Dim dbConn As New System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sfile)
        dbConn.Open()
        Dim dbTbl As New System.Data.DataTable

        ' Get the table definition loaded in a table adapter
        Dim strSql As String = "Select TOP 1 * from " & tblName
        Dim dbAdapater As New System.Data.OleDb.OleDbDataAdapter(strSql, dbConn)
        dbAdapater.Fill(dbTbl)

        ' Get the index of the field name
        Dim i As Integer = dbTbl.Columns.IndexOf(fldName)
        DoesFieldExist = True
        If i = -1 Then DoesFieldExist = False

        dbTbl.Dispose()
        dbConn.Close()
        dbConn.Dispose()
    End Function
    Public Sub ToExCel(ByVal View As DevExpress.XtraGrid.Views.Grid.GridView, Optional FilePath As String = "")
        Try
            If FilePath = "" Then
                Dim SaveFileDialog1 As New System.Windows.Forms.SaveFileDialog
                If View.RowCount > 999999 Then
                    SaveFileDialog1.Filter = "ExcelFile (*.csv)|*.csv"
                    If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then View.ExportToCsv(SaveFileDialog1.FileName)
                Else
                    SaveFileDialog1.Filter = "ExcelFile (*.xlsx)|*.xlsx"
                    If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then View.ExportToXlsx(SaveFileDialog1.FileName)
                End If
            Else
                View.ExportToXlsx(FilePath)
            End If
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Sub ToCSV(ByVal View As DevExpress.XtraGrid.Views.Grid.GridView)
        Try
            Dim SaveFileDialog1 As New System.Windows.Forms.SaveFileDialog
            SaveFileDialog1.Filter = "CSV File (*.csv)|*.csv"
            If SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then View.ExportToXlsx(SaveFileDialog1.FileName)
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Sub BackupDeleteSelectedRows(ByVal View As DevExpress.XtraGrid.Views.Grid.GridView)
        Try
            Dim path As String = (System.Windows.Forms.Application.StartupPath + "\DELBAK")
            If Not Directory.Exists(path) Then Directory.CreateDirectory(path)
            View.OptionsPrint.PrintSelectedRowsOnly = True
            Dim fileDateTime As String = String.Format("{0:yyyyMMdd}_{1:HHmmss}", DateTime.Now, DateTime.Now)
            View.ExportToXlsx(String.Format("{0}\{1}.xlsx", path, fileDateTime))
            View.OptionsPrint.PrintSelectedRowsOnly = False
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Sub DeleteSelectedRows(ByVal View As DevExpress.XtraGrid.Views.Grid.GridView)
        BackupDeleteSelectedRows(View)
        View.DeleteSelectedRows()
    End Sub

    Public Sub Tb(Optional ByVal s As String = "ĐÃ XONG!")
        Try
            WaitClose()
        Catch ex As Exception

        End Try
        If s = "" Then Exit Sub
        DevExpress.XtraEditors.XtraMessageBox.Show(s, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Sub TBL(ByVal s As String)
        Try
            WaitClose()
        Catch ex As Exception

        End Try
        DevExpress.XtraEditors.XtraMessageBox.Show(s, "Có lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
    Public Function TB_KETTHUC(ByVal s As String) As Boolean
        Try
            WaitClose()
        Catch ex As Exception

        End Try
        DevExpress.XtraEditors.XtraMessageBox.Show(s, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True
    End Function
    Public Function HoiLuaChon(ByVal s As String) As Boolean
        Try
            WaitClose()
        Catch ex As Exception

        End Try
        If DevExpress.XtraEditors.XtraMessageBox.Show(s, "Lựa chọn", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then Return False
        Return True
    End Function
    Public Function HoiXoa(Optional ByVal s As String = "Xóa các dòng được chọn?")
        Try
            WaitClose()
        Catch ex As Exception

        End Try
        If DevExpress.XtraEditors.XtraMessageBox.Show(s, "Cẩn thận đấy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then Return False
        Return True
    End Function

    Public Function ToDate(ByVal edate As Object, Optional format As String = "Thang/Ngay/Nam") As Date
        Dim i As Date = New Date(1900, 1, 1)
        If format = "n" Then
            Return i.AddDays(edate - 2)
        End If
        'edate = edate.ToString.Split(" ")(0)

        Dim datestring As String = edate.ToString.ToUpper.Replace("  ", " ").Replace(" ", "/").Replace(".", "/").Replace("-", "/").Replace("_", "/").Replace("*", "/").Replace("//", "/")
        format = format.ToLower.Replace(".", "/").Replace("-", "/").Replace("_", "/").Replace("*", "/").Replace("mdy", "m/d/y").Replace("myd", "m/y/d").Replace("dmy", "d/m/y").Replace("dym", "d/y/m").Replace("ymd", "y/m/d").Replace("ydm", "y/d/m")
        Try
            Dim ngay As String = "", thang As String = "", nam As String = ""

            datestring = datestring.Replace("JAN", "01").Replace("FEB", "02").Replace("MAR", "03").Replace("APR", "04").Replace("MAY", "05").Replace("JUN", "06").Replace("JUL", "07").Replace("AUG", "08").Replace("SEP", "09").Replace("OCT", "10").Replace("NOV", "11").Replace("DEC", "12")
            Select Case format
                Case "Thang/Ngay/Nam", "m/d/y"
                    If datestring.ToString.Contains("/") Then
                        thang = datestring.ToString.Split("/")(0)
                        ngay = datestring.ToString.Split("/")(1)
                        nam = datestring.ToString.Split("/")(2)
                    Else
                        thang = Mid(datestring.ToString, 1, 2)
                        ngay = Mid(datestring.ToString, 3, 2)
                        nam = Mid(datestring.ToString, 5, 4)
                    End If
                Case "Ngay/Thang/Nam", "d/m/y"
                    ngay = datestring.ToString.Split("/")(0)
                    thang = datestring.ToString.Split("/")(1)
                    nam = datestring.ToString.Split("/")(2)
                Case "Nam/Thang/Ngay", "y/m/d"
                    If datestring.ToString.Contains("/") Then
                        nam = datestring.ToString.Split("/")(0)
                        thang = datestring.ToString.Split("/")(1)
                        ngay = datestring.ToString.Split("/")(2)
                    Else
                        If datestring.ToString.Split("/")(0).Length = 8 Then
                            nam = Mid(datestring.ToString, 1, 4)
                            thang = Mid(datestring.ToString, 5, 2)
                            ngay = Mid(datestring.ToString, 7, 2)
                        End If
                    End If
                Case "Nam/Ngay/Thang", "y/d/m"
                    If edate.ToString.Contains("/") Then
                        nam = datestring.ToString.Split("/")(0)
                        ngay = datestring.ToString.Split("/")(1)
                        thang = datestring.ToString.Split("/")(2)
                    Else
                        nam = Mid(datestring.ToString, 1, 4)
                        ngay = Mid(datestring.ToString, 5, 2)
                        thang = Mid(datestring.ToString, 7, 2)
                    End If
                Case "Ngay/Nam/Thang", "d/y/m"
                    ngay = datestring.ToString.Split("/")(0)
                    nam = datestring.ToString.Split("/")(1)
                    thang = datestring.ToString.Split("/")(2)
            End Select

            If Len(nam) = 2 Then nam = "20" + nam Else nam = Strings.Left(nam, 4)
            i = New DateTime(Integer.Parse(nam), Integer.Parse(thang), Integer.Parse(ngay))
        Catch ex As Exception
            'i = New DateTime(1900, 1, 1)
            i = Nothing
        End Try
        Return i
    End Function
    Public Function ThayThe(Chuoi As String, ListCharOld As String, CharNew As String)
        For i = 0 To ListCharOld.Split("|").Length - 1
            Chuoi = Chuoi.Replace(ListCharOld.Split("|")(i), CharNew)
        Next
        Return Chuoi
    End Function
    Public Function ToDecimal(ByVal s As Object, Optional DecimalSymbol As String = ".") As Decimal
        Dim i As Decimal = 0
        Try
            If DecimalSymbol = "." Then s = s.ToString.Replace(",", "") Else s = s.ToString.Replace(".", "")

            Decimal.TryParse(s.ToString.Trim({"*"c, "$"c, """"c, "("c, ")"c, "%"c, "'"c, " "c}), i)

            If s.ToString.Contains("(") And s.ToString.Contains(")") Then
                i = i * -1
            Else
                i = i * 1
            End If
        Catch ex As Exception

        End Try
        Return i
    End Function
    Public Function ToInt(ByVal s As Object) As Integer
        Dim i As Integer = 0
        Try
            Integer.TryParse(s.ToString.Trim({"*"c, ","c, "$"c, """"c, "("c, ")"c, "%"c}).Replace(" ", ""), i)
            If s.ToString.Contains("(") And s.ToString.Contains(")") Then
                i = i * -1
            Else
                i = i * 1
            End If
        Catch ex As Exception

        End Try
        Return i
    End Function

    Private Sub AddUpdateConnectionString(ByVal name As String, ByVal dbname As String, ByVal catalog As String, ByVal userid As String, ByVal password As String)
        Dim isNew As Boolean = False
        Dim path As String = IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "DoiSoat.exe.Config")
        Dim doc As New System.Xml.XmlDocument()
        doc.Load(path)
        Dim list As System.Xml.XmlNodeList = doc.DocumentElement.SelectNodes(String.Format("connectionStrings/add[@name='{0}']", name))
        Dim node As System.Xml.XmlNode
        isNew = list.Count = 0
        If isNew Then
            node = doc.CreateNode(System.Xml.XmlNodeType.Element, "add", Nothing)
            Dim attribute As System.Xml.XmlAttribute = doc.CreateAttribute("name")
            attribute.Value = name
            node.Attributes.Append(attribute)

            attribute = doc.CreateAttribute("connectionString")
            attribute.Value = ""
            node.Attributes.Append(attribute)

            attribute = doc.CreateAttribute("providerName")
            attribute.Value = "System.Data.SqlClient"
            node.Attributes.Append(attribute)
        Else
            node = list(0)
        End If
        Dim conString As String = node.Attributes("connectionString").Value
        Dim conStringBuilder As New System.Data.SqlClient.SqlConnectionStringBuilder(conString)
        conStringBuilder.InitialCatalog = catalog
        conStringBuilder.DataSource = dbname
        conStringBuilder.IntegratedSecurity = False
        conStringBuilder.UserID = userid
        conStringBuilder.Password = password
        node.Attributes("connectionString").Value = conStringBuilder.ConnectionString
        If isNew Then doc.DocumentElement.SelectNodes("connectionStrings")(0).AppendChild(node)
        doc.Save(path)
    End Sub
    Public Function Text_To_DataTable1(ByVal pathfile As String, ByVal delimitter As Char, ByVal header As Boolean) As System.Data.DataTable
        Dim source As String = String.Empty
        Dim dt As System.Data.DataTable = New System.Data.DataTable
        If IO.File.Exists(pathfile) Then
            source = IO.File.ReadAllText(pathfile)
        Else
            Throw New IO.FileNotFoundException("Could not find the file at " & pathfile, pathfile)
        End If
        Dim rows() As String = source.Split({Environment.NewLine}, StringSplitOptions.None)
        If rows(0).Split(delimitter).Length > 250 Then
            Throw New Exception("File có quá nhiều cột ")
        End If
        For i As Integer = 0 To rows(0).Split(delimitter).Length - 1
            Dim column As String = rows(0).Split(delimitter)(i)
            dt.Columns.Add(If(header, column, "column" & i + 1))
        Next
        For i As Integer = If(header, 1, 0) To rows.Length - 1
            Dim dr As DataRow = dt.NewRow
            For x As Integer = 0 To rows(i).Split(delimitter).Length - 1
                If x > dt.Columns.Count - 1 Then
                    Try
                        For j As Integer = dt.Columns.Count To rows(i).Split(delimitter).Length - 1
                            'Dim column As String = rows(i).Split(delimitter)(i)
                            dt.Columns.Add("column" & j + 1)
                        Next
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                End If

                Try
                    dr(x) = rows(i).Split(delimitter)(x)
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            Next
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Public Function Text_To_DataTable(ByVal pathfile As String, ByVal delimitter As Char, ByVal header As Boolean) As System.Data.DataTable
        Dim dt As System.Data.DataTable = New System.Data.DataTable
        If Not IO.File.Exists(pathfile) Then Return dt
        Dim sr As New IO.StreamReader(pathfile)
        Dim ColumnCreate As Boolean = False

        Do While Not sr.EndOfStream
            Dim s As String = sr.ReadLine
            If ColumnCreate Then
                For i As Integer = 0 To s.Split(delimitter).Length - 1
                    Dim column As String = s.Split(delimitter)(i)
                    dt.Columns.Add(IIf(header, column, "column" & i + 1))
                Next
                ColumnCreate = True
            End If

            Dim dr As DataRow = dt.NewRow
            For x As Integer = 0 To s.Split(delimitter).Length - 1
                If x > dt.Columns.Count - 1 Then
                    Try
                        For j As Integer = dt.Columns.Count To s.Split(delimitter).Length - 1
                            'Dim column As String = rows(i).Split(delimitter)(i)
                            dt.Columns.Add("column" & j + 1)
                        Next
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                End If

                Try
                    dr(x) = s.Split(delimitter)(x)
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            Next
            dt.Rows.Add(dr)
        Loop
        sr.Close()
        Return dt
    End Function
    Public Sub DataTable_To_Text(ByVal table As System.Data.DataTable, ByVal pathfile As String, ByVal header As Boolean, ByVal delimiter As Char)
        If table.Columns.Count < 0 OrElse table.Rows.Count < 0 Then Exit Sub

        Using sw As IO.StreamWriter = New IO.StreamWriter(pathfile)
            If header Then
                For i As Integer = 0 To table.Columns.Count - 2
                    sw.Write(table.Columns(i).ColumnName & delimiter)
                Next
                sw.Write(table.Columns(table.Columns.Count - 1).ColumnName & Environment.NewLine)
            End If

            For row As Integer = 0 To table.Rows.Count - 2
                For col As Integer = 0 To table.Columns.Count - 2
                    sw.Write(table.Rows(row).Item(col).ToString & delimiter)
                Next
                sw.Write(table.Rows(row).Item(table.Columns.Count - 1).ToString & Environment.NewLine)
            Next

            For col As Integer = 0 To table.Columns.Count - 2
                sw.Write(table.Rows(table.Rows.Count - 1).Item(col).ToString & delimiter)
            Next
            sw.Write(table.Rows(table.Rows.Count - 1).Item(table.Columns.Count - 1).ToString)
        End Using

    End Sub
    Function ten2matinh(ByVal tentinh As String) As String
        Dim dsten As String() = {"angiang", "bàrịa-vũngtàu", "vũngtàu", "bắccạn", "bắcgiang", "bạcliêu", "bắcninh", "bếntre", "bìnhđịnh", "bìnhdương",
        "bìnhphước", "bìnhthuận", "càmau", "cầnthơ", "caobằng", "đànẵng", "đắclắc", "đắknông", "điệnbiên", "đồngnai", "đồngtháp", "gialai", "hàgiang", "hànam",
        "hànội", "hàtĩnh", "hảidương", "hảiphòng", "hậugiang", "hòabình", "hưngyên", "khánhhòa", "kiêngiang", "kontum", "laichâu", "lâmđồng", "lạngsơn",
        "làocai", "longan", "namđịnh", "nghệan", "ninhbình", "ninhthuận", "phúthọ", "phúyên", "quảngbình", "quảngnam", "quảngngãi", "quảngninh", "quảngtrị",
        "sóctrăng", "sơnla", "tâyninh", "tháibình", "tháinguyên", "thanhhóa", "thừathiên-huế", "huế", "tiềngiang", "tphồchíminh", "hồchíminh", "tràvinh",
        "tuyênquang", "vĩnhlong", "vĩnhphúc", "yênbái", "bàrịavũngtàu", "quãngngãi", "đăclăk", "đăklăk"}
        Dim dsma As String() = {"88", "79", "79", "26", "23", "96", "22", "93", "59", "82", "83", "80", "97", "90", "27", "55", "63", "64", "38", "81", "87",
        "60", "31", "40", "10", "48", "17", "18", "91", "35", "16", "65", "92", "58", "39", "67", "24", "33", "85", "42", "46", "43", "66", "29", "62", "51", "56", "57",
        "20", "52", "95", "36", "84", "41", "25", "44", "53", "53", "86", "70", "70", "94", "30", "89", "28", "32", "79", "57", "63", "63"}
        Dim tinh As String = tentinh.ToLower.Replace(" ", "")
        For i As Integer = 0 To dsten.Length - 1
            If String.Compare(dsten(i), tinh) = 0 Then Return dsma(i)
        Next
        Return "00"
    End Function
    Public Sub WriteErrorLog(ByRef loi As String)
        If String.IsNullOrEmpty(loi) Then Exit Sub

        Dim fileStream As FileStream, streamWriter As StreamWriter
        Dim strPath As String, year As String = Date.Now.Year.ToString, time As String = Date.Now.ToString
        'strPath = Application.StartupPath & "\Error_" & year & ".log"
        strPath = System.Windows.Forms.Application.StartupPath & "\Error.log"
        If System.IO.File.Exists(strPath) Then fileStream = New FileStream(strPath, FileMode.Truncate, FileAccess.Write) Else fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)

        streamWriter = New StreamWriter(fileStream)
        '
        streamWriter.WriteLine(time & ":" & vbNewLine & loi)
        '
        streamWriter.Close()
        fileStream.Close()
        loi = ""
    End Sub
    Public Sub WriteActiLog(ByVal strComments As String)
        If String.IsNullOrEmpty(strComments) Then Exit Sub
        Dim fileStream As FileStream, streamWriter As StreamWriter
        Dim strPath As String, year As String = Date.Now.Year.ToString, time As String = Date.Now.ToString
        strPath = System.Windows.Forms.Application.StartupPath & "\ACTIVYTY_" & year & ".log"
        If System.IO.File.Exists(strPath) Then fileStream = New FileStream(strPath, FileMode.Append, FileAccess.Write) Else fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)

        streamWriter = New StreamWriter(fileStream)
        '
        streamWriter.WriteLine(time & ":" & strComments)
        '
        streamWriter.Close()
        fileStream.Close()
    End Sub
    Public Sub GHI_KQ_DS(ByVal strComments As String, Optional SFILE As String = "")
        Dim fileStream As FileStream, streamWriter As StreamWriter

        Dim strPath As String = System.Windows.Forms.Application.StartupPath & "\KQDS.log"

        If Not String.IsNullOrEmpty(SFILE) Then strPath = SFILE

        If System.IO.File.Exists(strPath) Then fileStream = New FileStream(strPath, FileMode.Truncate, FileAccess.Write) Else fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)

        streamWriter = New StreamWriter(fileStream)
        '
        streamWriter.WriteLine(strComments)
        '
        streamWriter.Close()
        fileStream.Close()
    End Sub
    Public Sub GHI_TEXT(ByVal strComments As String, FILE As String)
        Dim fileStream As FileStream, streamWriter As StreamWriter
        Dim strPath As String = System.Windows.Forms.Application.StartupPath & FILE
        If System.IO.File.Exists(strPath) Then fileStream = New FileStream(strPath, FileMode.Truncate, FileAccess.Write) Else fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)

        streamWriter = New StreamWriter(fileStream)
        '
        streamWriter.WriteLine(strComments)
        '
        streamWriter.Close()
        fileStream.Close()
    End Sub
    ''' <summary>
    ''' Configuration Gridview
    ''' </summary>
    ''' <param name="sender">Gridview name</param>
    ''' <param name="nColumn">Số cột trên form Edit</param>
    ''' <remarks></remarks>
    Public Sub FormatGridColumn(sender As DevExpress.XtraGrid.Views.Grid.GridView, Optional ByVal nColumn As Integer = 1)
        sender.OptionsBehavior.Editable = True
        sender.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace
        sender.OptionsView.EnableAppearanceEvenRow = True
        sender.OptionsView.ShowAutoFilterRow = True
        sender.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.False
        sender.OptionsEditForm.ShowOnEnterKey = DevExpress.Utils.DefaultBoolean.False
        sender.OptionsEditForm.ShowOnF2Key = DevExpress.Utils.DefaultBoolean.False
        sender.OptionsEditForm.EditFormColumnCount = nColumn
        sender.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
        sender.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace
        sender.OptionsSelection.MultiSelect = True
        'sender.OptionsSelection.MultiSelectMode = Views.Grid.GridMultiSelectMode.CheckBoxRowSelect
        sender.OptionsSelection.ShowCheckBoxSelectorInPrintExport = DevExpress.Utils.DefaultBoolean.False

        sender.OptionsSelection.CheckBoxSelectorColumnWidth = 30

    End Sub
    Public Function DoUpdateTable(ByVal dataAdapter As Object, ByVal dataTable As System.Data.DataSet) As Boolean
        Try
            dataAdapter.Update(dataTable)
        Catch ex As Exception
            TBL(ex.Message)
            Return False
        End Try
        Return True
    End Function
    ''' <summary>
    ''' paste from clipboard to grid
    ''' </summary>
    ''' <param name="dt">Datatable</param>
    ''' <remarks></remarks>
    Public Sub PasteFromClipboard(ByRef dt As System.Data.DataTable)
        Dim data() As String = ClipboardData.Split(ControlChars.Lf)
        If data.Length < 1 Then Return
        For Each row As String In data
            If row = String.Empty Then Return
            Dim rowData() As String = row.Split(New Char() {ControlChars.Cr, ChrW(&H9)})
            Dim newRow As DataRow = dt.NewRow()
            For i As Integer = 0 To rowData.Length - 1
                If i >= dt.Columns.Count Then Exit For
                Try
                    newRow(i) = rowData(i)
                Catch ex As Exception

                End Try
            Next i
            Try
                dt.Rows.Add(newRow)
            Catch ex As Exception

            End Try
        Next row
    End Sub


    Public Function PasteClipboard(ByRef dt As System.Data.DataTable) As String
        Dim data() As String = ClipboardData.Split(ControlChars.Lf)
        If data.Length < 1 Then Return "Không có dữ liệu"
        For Each row As String In data
            If row = String.Empty Then Return ""
            Dim rowData() As String = row.Split(New Char() {ControlChars.Cr, ChrW(&H9)})
            Dim newRow As DataRow = dt.NewRow()
            For i As Integer = 0 To rowData.Length - 1
                If i >= dt.Columns.Count Then Exit For
                Try
                    newRow(i) = rowData(i)
                Catch ex As Exception

                End Try
            Next i
            Try
                dt.Rows.Add(newRow)
            Catch ex As Exception
                Return ex.Message
            End Try
        Next row
        Return ""
    End Function
    Private Sub AddRow(ByVal data As String, ByRef dt As System.Data.DataTable)
        If data = String.Empty Then
            Return
        End If
        Dim rowData() As String = data.Split(New Char() {ControlChars.Cr, ChrW(&H9)})
        Dim newRow As DataRow = dt.NewRow()
        For i As Integer = 0 To rowData.Length - 1
            If i >= dt.Columns.Count Then
                Exit For
            End If
            Try
                newRow(i) = rowData(i)
            Catch ex As Exception

            End Try
        Next i
        Try
            dt.Rows.Add(newRow)
        Catch ex As Exception

        End Try
    End Sub
    Public Property ClipboardData() As String
        Get
            Dim iData As IDataObject = Clipboard.GetDataObject()
            If iData Is Nothing Then
                Return ""
            End If

            If iData.GetDataPresent(DataFormats.UnicodeText) Then
                Return CStr(iData.GetData(DataFormats.UnicodeText))
            End If
            Return ""
        End Get
        Set(ByVal value As String)
            Clipboard.SetDataObject(value)
        End Set
    End Property
    Public Function TCVN3toUNICODE(vnstr As String) As String
        Dim OUT As String = "", c As String, i As Integer
        For i = 1 To Len(vnstr)
            c = Mid(vnstr, i, 1)
            Select Case c
                Case "a" : c = ChrW(97)
                Case "¸" : c = ChrW(225)
                Case "µ" : c = ChrW(224)
                Case "¶" : c = ChrW(7843)
                Case "·" : c = ChrW(227)
                Case "¹" : c = ChrW(7841)
                Case "¨" : c = ChrW(259)
                Case "¾" : c = ChrW(7855)
                Case "»" : c = ChrW(7857)
                Case "¼" : c = ChrW(7859)
                Case "½" : c = ChrW(7861)
                Case "Æ" : c = ChrW(7863)
                Case "©" : c = ChrW(226)
                Case "Ê" : c = ChrW(7845)
                Case "Ç" : c = ChrW(7847)
                Case "È" : c = ChrW(7849)
                Case "É" : c = ChrW(7851)
                Case "Ë" : c = ChrW(7853)
                Case "e" : c = ChrW(101)
                Case "Ð" : c = ChrW(233)
                Case "Ì" : c = ChrW(232)
                Case "Î" : c = ChrW(7867)
                Case "Ï" : c = ChrW(7869)
                Case "Ñ" : c = ChrW(7865)
                Case "ª" : c = ChrW(234)
                Case "Õ" : c = ChrW(7871)
                Case "Ò" : c = ChrW(7873)
                Case "Ó" : c = ChrW(7875)
                Case "Ô" : c = ChrW(7877)
                Case "Ö" : c = ChrW(7879)
                Case "o" : c = ChrW(111)
                Case "ã" : c = ChrW(243)
                Case "ß" : c = ChrW(242)
                Case "á" : c = ChrW(7887)
                Case "â" : c = ChrW(245)
                Case "ä" : c = ChrW(7885)
                Case "«" : c = ChrW(244)
                Case "è" : c = ChrW(7889)
                Case "å" : c = ChrW(7891)
                Case "æ" : c = ChrW(7893)
                Case "ç" : c = ChrW(7895)
                Case "é" : c = ChrW(7897)
                Case "¬" : c = ChrW(417)
                Case "í" : c = ChrW(7899)
                Case "ê" : c = ChrW(7901)
                Case "ë" : c = ChrW(7903)
                Case "ì" : c = ChrW(7905)
                Case "î" : c = ChrW(7907)
                Case "i" : c = ChrW(105)
                Case "Ý" : c = ChrW(237)
                Case "×" : c = ChrW(236)
                Case "Ø" : c = ChrW(7881)
                Case "Ü" : c = ChrW(297)
                Case "Þ" : c = ChrW(7883)
                Case "u" : c = ChrW(117)
                Case "ó" : c = ChrW(250)
                Case "ï" : c = ChrW(249)
                Case "ñ" : c = ChrW(7911)
                Case "ò" : c = ChrW(361)
                Case "ô" : c = ChrW(7909)
                Case "­" : c = ChrW(432)
                Case "ø" : c = ChrW(7913)
                Case "õ" : c = ChrW(7915)
                Case "ö" : c = ChrW(7917)
                Case "÷" : c = ChrW(7919)
                Case "ù" : c = ChrW(7921)
                Case "y" : c = ChrW(121)
                Case "ý" : c = ChrW(253)
                Case "ú" : c = ChrW(7923)
                Case "û" : c = ChrW(7927)
                Case "ü" : c = ChrW(7929)
                Case "þ" : c = ChrW(7925)
                Case "®" : c = ChrW(273)
                Case "A" : c = ChrW(65)
                Case "¡" : c = ChrW(258)
                Case "¢" : c = ChrW(194)
                Case "E" : c = ChrW(69)
                Case "£" : c = ChrW(202)
                Case "O" : c = ChrW(79)
                Case "¤" : c = ChrW(212)
                Case "¥" : c = ChrW(416)
                Case "I" : c = ChrW(73)
                Case "U" : c = ChrW(85)
                Case "¦" : c = ChrW(431)
                Case "Y" : c = ChrW(89)
                Case "§" : c = ChrW(272)
            End Select
            OUT = OUT + c
        Next i
        Return OUT
    End Function
    Public Function ThangNam(ByVal dat As Date) As String
        Return dat.ToString("MM-yyyy")
    End Function
    Public Function Ngay_Thang_Nam(ByVal dat As Date) As String
        Return dat.ToString("dd-MM-yyyy")
    End Function
    Public Function DateToString(ByVal dat As Date) As String
        Return dat.ToShortDateString
    End Function
    Public Function AddMonth(tn As String, i As Integer) As String
        Try
            Dim format() = {"dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy"}
            Dim ngay As Date = Date.ParseExact("01-" + tn, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)
            ngay = ngay.AddMonths(i)
            Return ngay.ToString("MM-yyyy")
        Catch ex As Exception
            WriteErrorLog(ex.Message)
        End Try
        Return tn
    End Function
    Public Function SQL_GetDataFromSrv(ByVal SqlCmd As String, connection As String) As System.Data.DataTable
        Dim dt As New System.Data.DataTable
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(connection)
                If cn.State <> ConnectionState.Open Then cn.Open()
                Dim comm As New System.Data.SqlClient.SqlCommand(SqlCmd, cn)
                comm.CommandTimeout = 0
                Dim reader As System.Data.SqlClient.SqlDataReader = comm.ExecuteReader
                dt.Load(reader)
                If cn.State <> ConnectionState.Closed Then cn.Close()
            End Using
        Catch ex As Exception
            WriteErrorLog(ex.Message)
        End Try
        Return dt
    End Function
    Public Sub SQL_ExecCmd(ByVal SqlCmd As String, connection As String)
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(connection)
                If cn.State <> ConnectionState.Open Then cn.Open()
                Dim comm As New System.Data.SqlClient.SqlCommand(SqlCmd, cn)
                comm.CommandTimeout = 0
                comm.ExecuteNonQuery()
                If cn.State <> ConnectionState.Closed Then cn.Close()
            End Using
        Catch ex As Exception
            WriteErrorLog(ex.Message)
        End Try
    End Sub
    Public Function SQL_ExecCmdFunction(ByVal SqlCmd As String, connection As String) As String
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(connection)
                If cn.State <> ConnectionState.Open Then cn.Open()
                Dim comm As New System.Data.SqlClient.SqlCommand(SqlCmd, cn)
                comm.CommandTimeout = 0
                comm.ExecuteNonQuery()
                If cn.State <> ConnectionState.Closed Then cn.Close()
            End Using
        Catch ex As Exception
            Return ex.Message
        End Try
        Return ""
    End Function
    Public Function SQL_GetValueFromSrv(ByVal SqlCmd As String, connection As String) As String
        Dim Val As Object, re As String = ""
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(connection)
                If cn.State <> ConnectionState.Open Then cn.Open()
                Dim comm As New System.Data.SqlClient.SqlCommand(SqlCmd, cn)
                comm.CommandTimeout = 0
                'Dim reader As System.Data.SqlClient.SqlDataReader = comm.ExecuteReader
                Val = comm.ExecuteScalar
                If Not IsNothing(Val) Then re = Val.ToString
                If cn.State <> ConnectionState.Closed Then cn.Close()
            End Using
        Catch ex As Exception
            WriteErrorLog(ex.Message)
        End Try
        Return re
    End Function
    Function GET_NGUOI_LAP(Optional ByVal FORMNAME As String = "")
        Dim s As String = ""
        Try
            s = GetConFig("NGUOI_LAP_BIEU")
        Catch ex As Exception
            Tb("Người lập chưa được chọn")
        End Try
        Return s
    End Function
    Function GET_NGUOI_KY()
        Dim NGKY As String = ""
        Try
            NGKY = GetConFig("NGUOI_KY")
        Catch ex As Exception
            Tb("Lấy lại thông tin người ký")
        End Try
        Return NGKY
    End Function
    Function GET_NOI_NHAN()
        Dim s As String = ""
        Try
            s = GetConFig("NOI_NHAN")
        Catch ex As Exception
            Tb("Lấy lại thông tin nơi nhận")
        End Try
        Return s
    End Function
    Public Function TienBangChu(ByVal sSoTien As String) As String
        If String.IsNullOrEmpty(sSoTien.Trim) Or sSoTien.Trim = "" Then
            Return ""
        End If
        If ToDecimal(sSoTien) = 0 Then
            Return "Không "
        End If
        Dim DonVi() As String = {"", "nghìn ", "triệu ", "tỷ ", "nghìn ", "triệu "}
        Dim so As String
        Dim chuoi As String = ""
        Dim temp As String
        Dim id As Byte

        Do While (Not sSoTien.Equals(""))
            If sSoTien.Length <> 0 Then
                so = getNum(sSoTien)
                sSoTien = Left(sSoTien, sSoTien.Length - so.Length)
                temp = setNum(so)
                so = temp
                If Not so.Equals("") Then
                    temp = temp + DonVi(id)
                    chuoi = temp + chuoi
                End If
                id = id + 1
            End If
        Loop
        temp = UCase(Left(chuoi, 1))

        Return temp & Right(chuoi, Len(chuoi) - 1)
    End Function
    Private Function getNum(ByVal sSoTien As String) As String
        Dim so As String

        If sSoTien.Length >= 3 Then
            so = Right(sSoTien, 3)
        Else
            so = Right(sSoTien, sSoTien.Length)
        End If
        Return so
    End Function
    Private Function setNum(ByVal sSoTien As String) As String
        Dim chuoi As String = ""
        Dim flag0 As Boolean
        Dim flag1 As Boolean
        Dim temp As String

        temp = sSoTien
        Dim kyso() As String = {"không ", "một ", "hai ", "ba ", "bốn ", "năm ", "sáu ", "bảy ", "tám ", "chín "}
        'Xet hang tram
        If sSoTien.Length = 3 Then
            If Not (Left(sSoTien, 1) = 0 And Left(Right(sSoTien, 2), 1) = 0 And Right(sSoTien, 1) = 0) Then
                chuoi = kyso(Left(sSoTien, 1)) + "trăm "
            End If
            sSoTien = Right(sSoTien, 2)
        End If
        'Xet hang chuc
        If sSoTien.Length = 2 Then
            If Left(sSoTien, 1) = 0 Then
                If Right(sSoTien, 1) <> 0 Then
                    chuoi = chuoi + "linh "
                End If
                flag0 = True
            Else
                If Left(sSoTien, 1) = 1 Then
                    chuoi = chuoi + "mười "
                Else
                    chuoi = String.Format("{0}{1}mươi ", chuoi, kyso(Left(sSoTien, 1)))
                    flag1 = True
                End If
            End If
            sSoTien = Right(sSoTien, 1)
        End If
        'Xet hang don vi
        If Right(sSoTien, 1) <> 0 Then
            If Left(sSoTien, 1) = 5 And Not flag0 Then
                If temp.Length = 1 Then
                    chuoi = chuoi + "năm "
                Else
                    chuoi = chuoi + "lăm "
                End If
            Else
                If Left(sSoTien, 1) = 1 And Not (Not flag1 Or flag0) And chuoi <> "" Then
                    chuoi = chuoi + "mốt "
                Else
                    chuoi = chuoi + kyso(Left(sSoTien, 1)) + ""
                End If
            End If
        Else
        End If
        Return chuoi
    End Function
    'Mã hóa: sKetQua = MaHoa(sChuoiCanMaHoa, sMatKhau)
    Function MaHoa(ByVal src As String, ByVal FStr As String) As String
        Dim result As String = ""
        Dim i%, j%
        j = 1
        For i = 1 To Len(src)
            result = result & Chr(Asc(Mid$(src, i, 1)) + Asc(Mid$(FStr, j, 1)))
            j = IIf((j = Len(FStr)), 1, j + 1)
        Next i
        Return result
    End Function

    'Giải mã: sKetQua = GiaiMa(sChuoiDuocMaHoa, sMatKhau)
    Function GiaiMa(ByVal src As String, ByVal FStr As String) As String
        Dim result As String = ""
        Dim i%, j%
        j = 1
        For i = 1 To Len(src)
            result = result & Chr(Asc(Mid$(src, i, 1)) - Asc(Mid$(FStr, j, 1)))
            j = IIf((j = Len(FStr)), 1, j + 1)
        Next i
        Return result
    End Function
    Public Sub SQL_TableInsert(ByVal dt As System.Data.DataTable, SQLTable As String, Connection As String, ListFieldUnique As String)
        ListFieldUnique = ListFieldUnique.TrimEnd(",")
        SQL_ExecCmd("BEGIN TRY DROP TABLE IMPORT_TMP END TRY BEGIN CATCH END CATCH; SELECT * INTO IMPORT_TMP FROM " + SQLTable + " WHERE 1=2", Connection)
        SQL_BulkInsert(dt, "IMPORT_TMP", Connection)
        SQL_ExecCmd(String.Format("DELETE FROM IMPORT_TMP WHERE ID NOT IN (SELECT MIN(ID) FROM IMPORT_TMP GROUP BY {0})", ListFieldUnique), Connection)
        dt.Clear()
        Dim DK = ""
        For i = 0 To ListFieldUnique.Split(",").Length - 1
            DK += String.Format(" A.{0} = B.{0} ", ListFieldUnique.Split(",")(i))
            If i < ListFieldUnique.Split(",").Length - 1 Then DK += " AND "
        Next
        Dim cmd = "SELECT A.* FROM IMPORT_TMP A INNER JOIN " + SQLTable + " B ON " + DK
        dt.Merge(SQL_GetDataFromSrv(cmd, Connection))
        If dt.Rows.Count > 0 Then
            Tb("KIỂM TRA CÁC GIAO DỊCH TRÙNG KHÔNG ĐƯỢC CẬP NHẬT")
        End If
        SQL_ExecCmd("DELETE A FROM IMPORT_TMP A INNER JOIN " + SQLTable + " B ON " + DK, Connection)
        SQL_ExecCmd("ALTER TABLE IMPORT_TMP DROP COLUMN ID", Connection)
        SQL_ExecCmd("ALTER TABLE IMPORT_TMP DROP COLUMN IDROW", Connection)
        SQL_ExecCmd("INSERT INTO " + SQLTable + " SELECT * FROM IMPORT_TMP", Connection)
    End Sub
    Public Sub SQL_BulkInsert(ByVal dt As System.Data.DataTable, SQLTable As String, Connection As String)
        Using Conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection(Connection)
            Conn.Open()
            Using s As System.Data.SqlClient.SqlBulkCopy = New System.Data.SqlClient.SqlBulkCopy(Conn)
                s.DestinationTableName = SQLTable
                s.BulkCopyTimeout = 0
                s.WriteToServer(dt)
                s.Close()
            End Using
            Conn.Close()
        End Using
    End Sub
    Public Function ConvertJulianDate(ByVal JulianDate As Decimal) As DateTime
        Dim julianRoot As DateTime
        julianRoot = "01/01/1900 00:00:00"
        ConvertJulianDate = julianRoot.AddDays(JulianDate)
        Return ConvertJulianDate
    End Function

    Public Function XtraGrid2Datatable(Grid As DevExpress.XtraGrid.Views.Grid.GridView)
        Dim dt As New System.Data.DataTable
        Try
            For Each column As DevExpress.XtraGrid.Columns.GridColumn In Grid.VisibleColumns
                dt.Columns.Add(column.FieldName, column.ColumnType)
            Next column
            For i As Integer = 0 To Grid.DataRowCount - 1
                Dim row As DataRow = dt.NewRow()
                For Each column As DevExpress.XtraGrid.Columns.GridColumn In Grid.VisibleColumns
                    row(column.FieldName) = Grid.GetRowCellValue(i, column)
                Next column
                dt.Rows.Add(row)
            Next i

        Catch ex As Exception

        End Try
        Return dt
    End Function
    Public Function XtraGrid2TextDatatable(Grid As DevExpress.XtraGrid.Views.Grid.GridView)
        Dim dt As New System.Data.DataTable
        Try
            For Each column As DevExpress.XtraGrid.Columns.GridColumn In Grid.VisibleColumns
                dt.Columns.Add(column.FieldName, System.Type.GetType("System.String"))
            Next column
            For i As Integer = 0 To Grid.DataRowCount - 1
                Dim row As DataRow = dt.NewRow()
                For Each column As DevExpress.XtraGrid.Columns.GridColumn In Grid.VisibleColumns
                    row(column.FieldName) = Grid.GetRowCellDisplayText(i, column)
                Next column
                dt.Rows.Add(row)
            Next i

        Catch ex As Exception

        End Try
        Return dt
    End Function
    Public Function CheckThangNam(TN As String) As Boolean
        If Mid(TN, 3, 1) <> "-" Then Return False
        If Inlist(Mid(TN, 1, 2), "01,02,03,04,05,06,07,08,09,10,11,12") Then
            If Inlist(Mid(TN, 4, 4), "2020,2021,2022,2023,2024,2025,2026,2027,2028,20229,2030") Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Function GetThangNam(ByRef Value As String, Optional ByVal tieude As String = "") As Boolean
        Try
            Dim f As New GetThangNam_Form
            If tieude <> "" Then f.Text = tieude
            If Value.Trim(" ") = "" Then
                Value = Date.Today.AddMonths(-1).ToString("MM-yyyy")
            End If
            f.RadioGroup1.EditValue = Strings.Left(Value.TrimStart("@"), 2)
            f.RadioGroup2.EditValue = Strings.Right(Value, 4)
            f.ShowDialog()
            Value = String.Format("{0}-{1}", f.RadioGroup1.EditValue, f.RadioGroup2.EditValue)
            If f.CheckEdit3.EditValue Then Value = ""
            f.Close()
            If f.CheckEdit1.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetTuNgay(ByRef Ngay1 As DateTime, ByRef Ngay2 As DateTime, Optional ByVal tieude As String = "") As Boolean
        Try
            Dim f As New GetTuNgay_form
            If tieude <> "" Then f.Text = tieude
            f.DateEdit1.EditValue = Ngay1
            f.DateEdit2.EditValue = Ngay2
            f.ShowDialog()
            Ngay1 = f.DateEdit1.EditValue
            Ngay2 = f.DateEdit2.EditValue
            f.Close()
            If f.CheckEdit1.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetBooleanValue(ByRef Value As Boolean) As Boolean
        Try
            Dim f As New GetBooleanValue_form
            f.CheckEdit1.EditValue = Value
            f.CheckEdit1.Text = If(Value, "Chọn", "Không chọn")
            f.ShowDialog()
            Value = f.CheckEdit1.EditValue
            f.Close()
            If f.CheckEdit2.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetNgayValue(ByRef Ngay As Date, Optional stext As String = "Chọn ngày") As Boolean
        Try
            Dim f As New GetNgayValue_form
            f.Text = stext
            f.DateEdit1.EditValue = Ngay
            f.ShowDialog()
            Ngay = f.DateEdit1.EditValue
            f.Close()
            If f.CheckEdit1.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetValueTimKiem(ByRef SoHieu As String, Optional showtext As String = "Nhập số cần tìm") As Boolean
        Try
            Dim f As New TimKiem
            f.Text = showtext
            f.MemoEdit1.Text = SoHieu
            f.ShowDialog()
            SoHieu = f.MemoEdit1.Text.TrimEnd(vbNewLine).TrimEnd(vbTab).TrimEnd(vbCr).TrimEnd(vbLf).TrimEnd(vbCrLf).TrimEnd(" ")
            SoHieu = SoHieu.Replace(vbNewLine, "','")
            SoHieu = SoHieu.Replace("''", "'")
            f.Close()
            If f.CheckEdit1.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetStringValue(ByRef Value As String, Optional showtext As String = "Nhập giá tri") As Boolean
        Try
            Dim f As New GetStringValue_form
            f.Text = showtext
            f.MemoEdit1.EditValue = Value
            f.ShowDialog()
            Value = f.MemoEdit1.EditValue.ToString.TrimEnd(vbNewLine).TrimEnd(vbTab).TrimEnd(vbCr).TrimEnd(vbLf).TrimEnd(vbCrLf).TrimEnd(" ")
            f.Close()
            If f.CheckEdit1.EditValue Then Return True Else Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetNumberValue(ByRef Value As Decimal, Optional showtext As String = "Nhập giá trị") As Boolean
        Try
            Dim f As New GetNumberValue_form
            f.Text = showtext
            f.SpinEdit1.Properties.Mask.MaskType = Mask.MaskType.Numeric
            f.SpinEdit1.Properties.Mask.EditMask = "N"
            f.SpinEdit1.EditValue = Value
            f.ShowDialog()
            Value = f.SpinEdit1.EditValue
            f.Close()
            If f.CheckEdit1.EditValue Then Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Function GetSheetsName(ByVal fname As String) As List(Of String)
        Dim xlApp As New Microsoft.Office.Interop.Excel.Application
        Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim strSheetName As New List(Of String)
        Try
            xlApp.Workbooks.Open(fname, 0, True)

            For Each xlWorkSheet In xlApp.Sheets
                strSheetName.Add(xlWorkSheet.Name)
            Next
            xlApp.Quit()
            releaseObject(xlApp)
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return strSheetName
    End Function

    Public Function excel2unicodetext(ByVal FileName As String, Optional SheetName As String = "") As String
        If IO.Directory.Exists(Application.StartupPath + "\Excel\") Then
            Delete_Files(Application.StartupPath + "\Excel\")
        Else
            IO.Directory.CreateDirectory(Application.StartupPath + "\Excel\")
        End If
        Dim newFileName As String = String.Format("{0}\Excel\{1}.txt", Application.StartupPath, IO.Path.GetFileNameWithoutExtension(FileName))
        Try
            Dim xlApp As Microsoft.Office.Interop.Excel.Application
            Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
            Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
            xlApp = New Microsoft.Office.Interop.Excel.Application
            xlApp.DisplayAlerts = False
            xlWorkBook = xlApp.Workbooks.Open(FileName)
            xlWorkSheet = If(SheetName = "", xlWorkBook.Worksheets(1), xlWorkBook.Worksheets(SheetName))
            xlWorkSheet.Activate()
            xlWorkSheet.Cells.Replace(vbTab, " ")
            xlWorkSheet.Cells.Replace(vbCr, " ")
            xlWorkSheet.Cells.Replace(vbLf, " ")
            xlWorkSheet.Cells.Replace("  ", " ")
            xlWorkBook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlUnicodeText)
            xlWorkBook.Close()
            xlApp.Quit()
            releaseObject(xlApp)
            releaseObject(xlWorkBook)
            releaseObject(xlWorkSheet)
            Return newFileName
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return ""
    End Function
    Public Function ImportExceltoDatatable(filepath As String, Optional SheetName As String = "") As System.Data.DataTable
        Dim dt As New System.Data.DataTable
        Try
            Dim f As String = excel2unicodetext(filepath, If(SheetName = "", "", SheetName))
            dt = Text_To_DataTable(f, vbTab, True)
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Critical)
        End Try
        Return dt
    End Function
    Public Sub AdapterUpdateData(RemoteSQLTable As String, LocalDataTable As System.Data.DataTable, Connection As String)
        Try
            Using conn As SqlConnection = New SqlConnection()
                conn.ConnectionString = Connection
                conn.Open()
                Dim myTrans As SqlTransaction
                Using cmd As SqlCommand = New SqlCommand(String.Format("Select * from {0} where 1=2", RemoteSQLTable), conn)
                    Using adapter As SqlDataAdapter = New SqlDataAdapter(cmd)
                        Dim builder As SqlCommandBuilder = New SqlCommandBuilder(adapter)
                        Try
                            myTrans = conn.BeginTransaction()
                            adapter.SelectCommand.Transaction = myTrans
                            adapter.Update(LocalDataTable)
                            Try
                                myTrans.Commit()
                            Catch ex As Exception
                                myTrans.Rollback()
                            End Try
                        Catch ex As Exception
                            WriteErrorLog(ex.Message)
                        End Try
                    End Using
                End Using
                conn.Close()
            End Using
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Function ChonTrongDanhSach(ByRef Value As String, SqlTableName As String, SqlFieldName As String, con As String, Optional ThemSoCot As Integer = 0, Optional SqlDieuKien As String = "") As Boolean
        Dim form As New ChonDm_Form
        Try
            Dim cmd As String = String.Format("Select distinct {0} from {1}", SqlFieldName, SqlTableName)
            If SqlDieuKien <> "" Then cmd += String.Format(" Where {0}", SqlDieuKien)
            Dim ds As System.Data.DataTable = SQL_GetDataFromSrv(cmd, con)
            For i = 0 To ThemSoCot - 1
                ds.Rows.Add(i.ToString)
            Next
            If ds.Rows.Count < 1 Then Return Nothing
            form.GridControl1.DataSource = ds
            FormatGridColumn(form.GridView1, "")
            form.GridView1.OptionsSelection.MultiSelect = False
            form.ShowDialog()
            If form.CheckHuy.EditValue Then Return False
            Value = form.GridView1.GetFocusedDataRow(SqlFieldName.Split(",")(0))
        Catch ex As Exception
            Return False
        End Try
        form.Dispose()
        Return True
    End Function
    Public Function ChonNhieuTrongDanhSach(ByRef Value As String, SqlTableName As String, SqlFieldName As String, con As String, Optional ThemSoCot As Integer = 0, Optional SqlDieuKien As String = "") As Boolean
        Dim form As New ChonDm_Form
        Try
            Dim cmd As String = String.Format("Select distinct {0} from {1}", SqlFieldName, SqlTableName)
            If SqlDieuKien <> "" Then cmd += String.Format(" Where {0}", SqlDieuKien)
            Dim ds As System.Data.DataTable = SQL_GetDataFromSrv(cmd, con)
            For i = 0 To ThemSoCot - 1
                ds.Rows.Add(i.ToString)
            Next
            If ds.Rows.Count < 1 Then Return Nothing
            form.GridControl1.DataSource = ds
            FormatGridColumn(form.GridView1, "")
            form.ShowDialog()
            If form.CheckHuy.EditValue Then Return False
            Dim selectedRowHandles As Integer() = form.GridView1.GetSelectedRows()
            Dim J As Integer
            For J = 0 To selectedRowHandles.Length - 1
                Dim selectedRowHandle As Integer = selectedRowHandles(J)
                If (selectedRowHandle >= 0) Then
                    value += String.Format("'{0}',", form.GridView1.GetRowCellDisplayText(selectedRowHandle, SqlFieldName.Split(",")(0)))
                End If
            Next
            Value = Value.TrimEnd(",")
        Catch ex As Exception
            Tb(ex.Message)
            Return False
        End Try

        form.Dispose()
        Return True
    End Function
    Public Sub F2sub(grdview As DevExpress.XtraGrid.Views.Grid.GridView, Value As Object)
        Dim Rows As New ArrayList(), id As String = ""

        ' Add the selected rows to the list.
        Dim selectedRowHandles As Int32() = grdview.GetSelectedRows()
        Dim I As Integer
        For I = 0 To selectedRowHandles.Length - 1
            Dim selectedRowHandle As Int32 = selectedRowHandles(I)
            If (selectedRowHandle >= 0) Then
                Rows.Add(grdview.GetDataRow(selectedRowHandle))
            End If
        Next
        Try
            grdview.BeginUpdate()
            For I = 0 To Rows.Count - 1
                Dim R As DataRow = CType(Rows(I), DataRow)
                Try
                    R(grdview.FocusedColumn.FieldName) = Value
                Catch ex As Exception
                    WriteErrorLog(ex.Message)
                End Try
            Next
        Finally
            grdview.EndUpdate()
        End Try

        grdview.ClearSelection()
    End Sub
    Public Sub F2(grdview As DevExpress.XtraGrid.Views.Grid.GridView)
        Try
            If grdview.SelectedRowsCount < 1 Then
                Tb("Chưa có dòng được chọn")
                Exit Sub
            End If
            Dim value As String = grdview.GetFocusedDisplayText()
            Select Case grdview.FocusedColumn.ColumnType.ToString
                Case "System.Boolean"
                    Dim KQ As Boolean = False
                    If value = "Indeterminater" Or value = "Unchecked" Then KQ = False Else KQ = True
                    If GetBooleanValue(KQ) Then  Else Exit Sub
                    F2sub(grdview, kq)
                Case "System.String"
                    If grdview.FocusedColumn.FieldName.ToLower.Contains("thang") Then
                        If GetThangNam(value, String.Format("Nhập giá trị {0}", grdview.FocusedColumn.FieldName)) Then  Else Exit Sub
                    Else
                        If GetStringValue(value, String.Format("Gia tri mới của {0}", grdview.FocusedColumn.FieldName)) Then  Else Exit Sub
                    End If
                    F2sub(grdview, value)
                Case "System.DateTime"
                    Dim KQ As Date
                    If value.Contains("-") Then KQ = ToDate(value, "dmy")
                    If GetNgayValue(KQ, String.Format("Gia tri mới của {0}", grdview.FocusedColumn.FieldName)) Then  Else Exit Sub
                    F2sub(grdview, KQ)
                Case Else
                    Dim KQ As Decimal
                    KQ = If(value = "", 0, value)
                    If GetNumberValue(KQ) Then  Else Exit Sub
                    F2sub(grdview, KQ)
            End Select
            SendKeys.Send("{F6}")
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub

    Public Sub F3(grdview As DevExpress.XtraGrid.Views.Grid.GridView)
        Try
            If Not HoiXoa("Xoá các dòng được chọn?") Then Exit Sub
            Dim CheckIDDS As Boolean = False
            For Each col As DevExpress.XtraGrid.Columns.GridColumn In grdview.Columns
                If col.FieldName.ToLower = "idds" Then
                    CheckIDDS = True
                End If
            Next
            If CheckIDDS Then
                Dim selectedRowHandles As Integer() = grdview.GetSelectedRows()
                If selectedRowHandles.Length > 0 Then
                    For i As Integer = 0 To selectedRowHandles.Length - 1
                        Dim r As DataRow = grdview.GetDataRow(selectedRowHandles(i))
                        If nvl(r("idds"), 0) > 0 Then
                            Tb("Không xoá được do giao dịch được đối soát")
                            Exit Sub
                        End If
                    Next
                End If
            End If
            DeleteSelectedRows(grdview)
            SendKeys.Send("{F6}")
        Catch ex As Exception
            Tb(ex.Message)
        End Try
    End Sub
    Public Sub F4(grdview As DevExpress.XtraGrid.Views.Grid.GridView)
        grdview.AddNewRow()
        Dim rowHandle As Integer = grdview.GetRowHandle(grdview.DataRowCount)
        If (grdview.IsNewItemRow(rowHandle)) Then
            For Each col As DevExpress.XtraGrid.Columns.GridColumn In grdview.Columns
                Select Case col.ColumnType.ToString
                    Case "System.Boolean"
                        Dim value As Boolean = True
                        GetBooleanValue(value)
                        grdview.SetRowCellValue(rowHandle, col, value)
                    Case "System.String"
                        Dim value As String = ""
                        If col.FieldName.ToLower = "thangnam" Then
                            GetThangNam(value, String.Format("Nhập giá trị {0}", col.FieldName))
                        Else
                            GetStringValue(value, String.Format("Nhập giá trị {0}", col.FieldName))
                        End If
                        grdview.SetRowCellValue(rowHandle, col, value)
                    Case "System.DateTime"
                        Dim value As Date = Date.Today
                        GetNgayValue(value, String.Format("Nhập giá trị {0}", col.FieldName))
                        grdview.SetRowCellValue(rowHandle, col, value)
                    Case Else
                        If col.FieldName <> "Id" Then
                            Dim value As Decimal = 0
                            If col.FieldName.ToLower = "idds" Then
                                value = 0
                            Else
                                GetNumberValue(value, String.Format("Nhập giá trị {0}", col.FieldName))
                            End If
                            grdview.SetRowCellValue(rowHandle, col, value)
                        End If
                End Select
            Next
        End If
        grdview.UpdateCurrentRow()

        SendKeys.Send("{F6}")
    End Sub
    Public Sub F5(ByVal DT As DataTable)
        Tb(PasteClipboard(DT))
        SendKeys.Send("{F6}")
    End Sub
    Public Sub F10(grdview As DevExpress.XtraGrid.Views.Grid.GridView, ByRef e As KeyEventArgs)
        Dim sohieu = ""
        Dim selectedRowHandles As Integer() = grdview.GetSelectedRows()
        For i As Integer = 0 To selectedRowHandles.Length - 1
            sohieu += grdview.GetRowCellDisplayText(selectedRowHandles(i), grdview.FocusedColumn.FieldName).ToString + vbNewLine
        Next
        Clipboard.SetText(sohieu)
        e.Handled = True
    End Sub
    Public Function GetColunmValue(grdview As DevExpress.XtraGrid.Views.Grid.GridView) As Boolean
        Try
            Dim r As DataRow = grdview.GetFocusedDataRow
            Dim kq As String = ""
            Select Case r("FieldName")
                Case "ThangNam", "ThangPhat", "ThangChapNhan", "ThangHoan", "ThangNhan"
                    Dim Value As String = r("ColumeNumber")
                    If GetThangNam(Value) Then kq = "@" + Value Else Return False
                Case "LoaiTien"
                    Dim val As String = ""
                    If ChonTrongDanhSach(val, "TBL_DM_LOAI_PHI", "MaLoaiPhi,TenLoaiPhi", My.Settings.con) Then  Else Return False
                    kq = "@" + val
                Case Else
                    Dim val As String = r("ColumeNumber")
                    If Not GetStringValue(val, "Nhập giá trị: " + vbNewLine + "Lưu ý: Giá trị cố định bắt đầu bằng @," + vbNewLine + "với cột bắt buộc có giá trị bắt đầu là *") Then Return False
                    kq = val
                    If r("ColumType") = "System.DateTime" Then
                        If kq <> "" Then
                            If kq.Contains("/") Then
                                If Inlist(kq.Split("/")(1), "dmy,dym,mdy,myd,ymd,ydm,n") Then  Else If TB_KETTHUC("Chưa có mô tả định dạng ngày") Then Return False
                            Else
                                Tb("Chưa có mô tả định dạng ngày")
                                Return False
                            End If
                        End If
                    End If
            End Select
            r("ColumeNumber") = kq
            Return True
        Catch ex As Exception
            Tb(ex.Message)
        End Try
        Return False
    End Function
    Sub GetTrangThaiData(ByVal DTlist As DataTable, con As String, table As String)
        Dim TBLTT As String = "TBL_TRANG_THAI", TBL = "", tnam = ""
        If Not HoiLuaChon("Cập nhật trạng thái không?") Then Exit Sub
        If GetThangNam(tnam, "Chọn tháng năm trạng thái") Then  Else Exit Sub
        WaitCreate()
        Dim tt = 0
        Dim dtRequest As New DataTable
        dtRequest.TableName = "dtRequest"
        Add_Col(dtRequest, "iStt,sData")
        Dim dsReturn As New DataSet

        For i = DTlist.Rows.Count - 1 To 0 Step -1
            Dim ttrow As Int16 = dtRequest.Rows.Count + 1
            dtRequest.Rows.Add(ttrow, DTlist.Rows(i)("SOHIEU"))
            If dtRequest.Rows.Count = 500 Or i = 0 Then
                WaitCaption(i.ToString)
                Try
                    Dim TBLTRANGTHAI As DataTable = SQL_GetDataFromSrv(String.Format("SELECT * FROM {0} WHERE 1=2", TBLTT), con)
                    webget(dtRequest, dsReturn)
                    If dsReturn.Tables(0).Rows.Count > 0 Then
                        For Each R As DataRow In dsReturn.Tables(0).Rows
                            Dim DR As DataRow = TBLTRANGTHAI.NewRow
                            DR("SoHieu") = R("ID")
                            DR("TrangThai") = If(R("STATE") = "2", "PTC", If(R("STATE") = "4", "PHTC", ""))
                            'DR("MaKhachHang") = R("CUSTOMERCODE").ToString.Split(":")(1).Split("/")(0).Replace(" ", "")
                            DR("MaBcPhat") = R("BC_PHAT").ToString.Split("-")(0).TrimEnd(" ")
                            DR("NgayTrangThai") = ToDate(R("NGAY_PHAT").ToString.Split(" ")(0), "dmy")
                            DR("THANGNAM") = tnam
                            DR("IDDS") = 0
                            If DR("TrangThai") <> "" Then TBLTRANGTHAI.Rows.Add(DR)
                        Next
                        SQL_TableInsert(TBLTRANGTHAI, TBLTT, con, "SoHieu")
                        TBLTRANGTHAI.Clear()
                    End If

                Catch ex As Exception
                    tbl += ex.Message + vbNewLine
                End Try
                dtRequest.Clear()
                tt = 0
            End If
        Next
        WriteErrorLog(tbl)
        WaitClose()
    End Sub
    Sub webget(ByRef dtRequest As DataTable, ByRef dsReturn As DataSet)


        Dim uc As New vn.vnpost.ws.UserCredentical
        uc.user = "ttds"
        uc.pass = "ttdsws@12#"
        Using rpt As New vn.vnpost.ws.TrackAndTrace
            rpt.PreAuthenticate = True
            rpt.UserCredenticalValue = uc
            rpt.EnableDecompression = False
            rpt.AllowAutoRedirect = True
            rpt.Timeout = 100000000
            dsReturn = rpt.TrackAndTrace_Items_Arr_wTable(dtRequest)
        End Using
    End Sub

End Module

