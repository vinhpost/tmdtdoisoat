<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QuanLyData_form
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LayoutControl3 = New DevExpress.XtraLayout.LayoutControl()
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
        Me.GridControl5 = New DevExpress.XtraGrid.GridControl()
        Me.GridView5 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colNgayChi = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSoTienChi = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSoGiaoDich = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSoHieuCod = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colTrangThai = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colId = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ComboBoxEdit3 = New DevExpress.XtraEditors.SearchLookUpEdit()
        Me.GridView8 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.LayoutControlGroup3 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem13 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem12 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        CType(Me.LayoutControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl3.SuspendLayout()
        CType(Me.GridControl5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBoxEdit3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LayoutControl3
        '
        Me.LayoutControl3.Controls.Add(Me.SimpleButton4)
        Me.LayoutControl3.Controls.Add(Me.GridControl5)
        Me.LayoutControl3.Controls.Add(Me.ComboBoxEdit3)
        Me.LayoutControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControl3.Name = "LayoutControl3"
        Me.LayoutControl3.Root = Me.LayoutControlGroup3
        Me.LayoutControl3.Size = New System.Drawing.Size(983, 557)
        Me.LayoutControl3.TabIndex = 4
        Me.LayoutControl3.Text = "LayoutControl3"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Location = New System.Drawing.Point(892, 12)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(79, 22)
        Me.SimpleButton4.StyleController = Me.LayoutControl3
        Me.SimpleButton4.TabIndex = 9
        Me.SimpleButton4.Text = "Ok"
        '
        'GridControl5
        '
        Me.GridControl5.Location = New System.Drawing.Point(12, 54)
        Me.GridControl5.MainView = Me.GridView5
        Me.GridControl5.Name = "GridControl5"
        Me.GridControl5.Size = New System.Drawing.Size(959, 491)
        Me.GridControl5.TabIndex = 5
        Me.GridControl5.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView5})
        '
        'GridView5
        '
        Me.GridView5.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colNgayChi, Me.colSoTienChi, Me.colSoGiaoDich, Me.colSoHieuCod, Me.colTrangThai, Me.colId})
        Me.GridView5.GridControl = Me.GridControl5
        Me.GridView5.Name = "GridView5"
        Me.GridView5.OptionsBehavior.Editable = False
        Me.GridView5.OptionsSelection.CheckBoxSelectorColumnWidth = 25
        Me.GridView5.OptionsSelection.MultiSelect = True
        Me.GridView5.OptionsView.ColumnAutoWidth = False
        Me.GridView5.OptionsView.ShowAutoFilterRow = True
        Me.GridView5.OptionsView.ShowFooter = True
        Me.GridView5.OptionsView.ShowGroupPanel = False
        '
        'colNgayChi
        '
        Me.colNgayChi.FieldName = "NgayChi"
        Me.colNgayChi.Name = "colNgayChi"
        Me.colNgayChi.Visible = True
        Me.colNgayChi.VisibleIndex = 0
        '
        'colSoTienChi
        '
        Me.colSoTienChi.FieldName = "SoTienChi"
        Me.colSoTienChi.Name = "colSoTienChi"
        Me.colSoTienChi.Visible = True
        Me.colSoTienChi.VisibleIndex = 1
        '
        'colSoGiaoDich
        '
        Me.colSoGiaoDich.FieldName = "SoGiaoDich"
        Me.colSoGiaoDich.Name = "colSoGiaoDich"
        Me.colSoGiaoDich.Visible = True
        Me.colSoGiaoDich.VisibleIndex = 2
        '
        'colSoHieuCod
        '
        Me.colSoHieuCod.FieldName = "SoHieuCod"
        Me.colSoHieuCod.Name = "colSoHieuCod"
        Me.colSoHieuCod.Visible = True
        Me.colSoHieuCod.VisibleIndex = 3
        '
        'colTrangThai
        '
        Me.colTrangThai.FieldName = "TrangThai"
        Me.colTrangThai.Name = "colTrangThai"
        Me.colTrangThai.Visible = True
        Me.colTrangThai.VisibleIndex = 4
        '
        'colId
        '
        Me.colId.FieldName = "Id"
        Me.colId.Name = "colId"
        Me.colId.Visible = True
        Me.colId.VisibleIndex = 5
        '
        'ComboBoxEdit3
        '
        Me.ComboBoxEdit3.Location = New System.Drawing.Point(196, 12)
        Me.ComboBoxEdit3.Name = "ComboBoxEdit3"
        Me.ComboBoxEdit3.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBoxEdit3.Properties.NullText = ""
        Me.ComboBoxEdit3.Properties.PopupSizeable = False
        Me.ComboBoxEdit3.Properties.View = Me.GridView8
        Me.ComboBoxEdit3.Size = New System.Drawing.Size(692, 20)
        Me.ComboBoxEdit3.StyleController = Me.LayoutControl3
        Me.ComboBoxEdit3.TabIndex = 8
        '
        'GridView8
        '
        Me.GridView8.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridView8.Name = "GridView8"
        Me.GridView8.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView8.OptionsView.ShowGroupPanel = False
        '
        'LayoutControlGroup3
        '
        Me.LayoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup3.GroupBordersVisible = False
        Me.LayoutControlGroup3.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem13, Me.LayoutControlItem12, Me.LayoutControlItem1})
        Me.LayoutControlGroup3.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup3.Name = "Root"
        Me.LayoutControlGroup3.Size = New System.Drawing.Size(983, 557)
        Me.LayoutControlGroup3.TextVisible = False
        '
        'LayoutControlItem13
        '
        Me.LayoutControlItem13.Control = Me.GridControl5
        Me.LayoutControlItem13.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItem13.Name = "LayoutControlItem11"
        Me.LayoutControlItem13.Size = New System.Drawing.Size(963, 511)
        Me.LayoutControlItem13.Text = "Dữ liệu   F2: Sửa,  F3: Xóa,  F8: Excel"
        Me.LayoutControlItem13.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem13.TextSize = New System.Drawing.Size(181, 13)
        '
        'LayoutControlItem12
        '
        Me.LayoutControlItem12.Control = Me.ComboBoxEdit3
        Me.LayoutControlItem12.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem12.Name = "LayoutControlItem12"
        Me.LayoutControlItem12.Size = New System.Drawing.Size(880, 26)
        Me.LayoutControlItem12.Text = "Chọn loại dữ liệu"
        Me.LayoutControlItem12.TextSize = New System.Drawing.Size(181, 13)
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.SimpleButton4
        Me.LayoutControlItem1.Location = New System.Drawing.Point(880, 0)
        Me.LayoutControlItem1.MaxSize = New System.Drawing.Size(83, 26)
        Me.LayoutControlItem1.MinSize = New System.Drawing.Size(83, 26)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(83, 26)
        Me.LayoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'QuanLyData_form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(983, 557)
        Me.Controls.Add(Me.LayoutControl3)
        Me.Name = "QuanLyData_form"
        Me.Text = "Quản lý số liệu"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.LayoutControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl3.ResumeLayout(False)
        CType(Me.GridControl5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBoxEdit3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LayoutControl3 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GridControl5 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView5 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colNgayChi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSoTienChi As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSoGiaoDich As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSoHieuCod As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTrangThai As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ComboBoxEdit3 As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents GridView8 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup3 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem13 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem12 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
End Class
