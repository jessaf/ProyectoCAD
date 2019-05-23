Public Class Form1
    Private Sub ConexiónConAutoCADToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConexiónConAutoCADToolStripMenuItem.Click
        inicializa_conexion("2019")
        If Not DOCUMENTO Is Nothing Then
            dwgActual.Text = "Plano Conectado"
        End If
    End Sub

    Private Sub ButtonTrafico_Click(sender As Object, e As EventArgs) Handles ButtonTrafico.Click
        seguimientoCalle()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'DetenerSimulacion()
    End Sub
End Class
