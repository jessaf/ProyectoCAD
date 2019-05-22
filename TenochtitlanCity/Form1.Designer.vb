<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ConexiónConAutoCADToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonTrafico = New System.Windows.Forms.Button()
        Me.ButtonViento = New System.Windows.Forms.Button()
        Me.dwgActual = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConexiónConAutoCADToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 3, 0, 3)
        Me.MenuStrip1.Size = New System.Drawing.Size(654, 55)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ConexiónConAutoCADToolStripMenuItem
        '
        Me.ConexiónConAutoCADToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ConexiónConAutoCADToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ConexiónConAutoCADToolStripMenuItem.Font = New System.Drawing.Font("Gabriola", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ConexiónConAutoCADToolStripMenuItem.Name = "ConexiónConAutoCADToolStripMenuItem"
        Me.ConexiónConAutoCADToolStripMenuItem.Size = New System.Drawing.Size(107, 49)
        Me.ConexiónConAutoCADToolStripMenuItem.Text = "Conexión "
        '
        'ButtonTrafico
        '
        Me.ButtonTrafico.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonTrafico.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ButtonTrafico.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ButtonTrafico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonTrafico.Font = New System.Drawing.Font("Gabriola", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonTrafico.Location = New System.Drawing.Point(251, 146)
        Me.ButtonTrafico.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.ButtonTrafico.Name = "ButtonTrafico"
        Me.ButtonTrafico.Size = New System.Drawing.Size(178, 63)
        Me.ButtonTrafico.TabIndex = 1
        Me.ButtonTrafico.Text = "Simulador Tráfico"
        Me.ButtonTrafico.UseVisualStyleBackColor = False
        '
        'ButtonViento
        '
        Me.ButtonViento.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonViento.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ButtonViento.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonViento.Font = New System.Drawing.Font("Gabriola", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonViento.Location = New System.Drawing.Point(251, 243)
        Me.ButtonViento.Margin = New System.Windows.Forms.Padding(2, 4, 2, 4)
        Me.ButtonViento.Name = "ButtonViento"
        Me.ButtonViento.Size = New System.Drawing.Size(178, 62)
        Me.ButtonViento.TabIndex = 2
        Me.ButtonViento.Text = "Simulador Viento"
        Me.ButtonViento.UseVisualStyleBackColor = False
        '
        'dwgActual
        '
        Me.dwgActual.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dwgActual.AutoSize = True
        Me.dwgActual.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.dwgActual.Location = New System.Drawing.Point(468, 9)
        Me.dwgActual.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.dwgActual.Name = "dwgActual"
        Me.dwgActual.Size = New System.Drawing.Size(157, 37)
        Me.dwgActual.TabIndex = 3
        Me.dwgActual.Text = "... Esperando Conexión"
        Me.dwgActual.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 37.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.BackgroundImage = Global.TenochtitlanCity.My.Resources.Resources.image
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(654, 492)
        Me.Controls.Add(Me.dwgActual)
        Me.Controls.Add(Me.ButtonViento)
        Me.Controls.Add(Me.ButtonTrafico)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Gabriola", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(3, 7, 3, 7)
        Me.Name = "Form1"
        Me.Text = "CitySimulator"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ConexiónConAutoCADToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ButtonTrafico As Button
    Friend WithEvents ButtonViento As Button
    Friend WithEvents dwgActual As Label
End Class
