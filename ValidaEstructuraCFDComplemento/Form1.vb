Imports System.IO

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.TextBox1.Text = ""
        Me.OpenFileDialog1.Filter = "Archivos XML (*.xml)|*.xml"
        Me.OpenFileDialog1.FilterIndex = 1
        Me.OpenFileDialog1.RestoreDirectory = True

        If Me.OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                Me.TextBox1.Text = OpenFileDialog1.FileName
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally

            End Try
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim context1 As New System.Xml.XmlParserContext(Nothing, Nothing, "", System.Xml.XmlSpace.None)
        Dim settings As System.Xml.XmlReaderSettings = New System.Xml.XmlReaderSettings()
        Dim reader As System.Xml.XmlReader
        Dim DIR_SAT As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location) & "\ArchivosSAT\"
        Dim xmlDoc As New System.Xml.XmlDocument
        Dim Comprobante As System.Xml.XmlNode

        Dim NamespaceAddenda As String = ""
        Dim NamespaceAddendaURL As String = ""

        Try
            xmlDoc.Load(Me.TextBox1.Text)

            Comprobante = xmlDoc.Item("cfdi:Comprobante")
            If Not Comprobante("Addenda") Is Nothing Then                
                If Comprobante("Addenda").ChildNodes.Count > 0 Then
                    NamespaceAddenda = Comprobante("Addenda").ChildNodes(0).NamespaceURI
                    NamespaceAddendaURL = (Comprobante.Attributes("xsi:schemaLocation").Value)
                    NamespaceAddendaURL = NamespaceAddendaURL.Replace("http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv2.xsd", "")
                    NamespaceAddendaURL = NamespaceAddendaURL.Replace("http://www.sat.gob.mx/cfd/2", "")
                    NamespaceAddendaURL = NamespaceAddendaURL.Replace(NamespaceAddenda, "").Trim
                End If
            End If
            settings.Schemas.Add("http://www.sat.gob.mx/cfd/3", DIR_SAT & "cfdv3.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/detallista", DIR_SAT & "detallista.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/divisas", DIR_SAT & "Divisas.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/donat", DIR_SAT & "donat.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/ecb", DIR_SAT & "ecb.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/ecc", DIR_SAT & "ecc.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/implocal", DIR_SAT & "implocal.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/iedu", DIR_SAT & "iedu.xsd")
            settings.Schemas.Add("http://www.sat.gob.mx/TimbreFiscalDigital", DIR_SAT & "TimbreFiscalDigital.xsd")            

            If NamespaceAddenda <> "" And NamespaceAddendaURL <> "" Then
                settings.Schemas.Add(NamespaceAddenda, NamespaceAddenda + NamespaceAddendaURL)
            End If
            settings.ValidationType = System.Xml.ValidationType.Schema
            settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.ProcessInlineSchema
            settings.XmlResolver = Nothing

            reader = System.Xml.XmlReader.Create(Me.TextBox1.Text, settings)

            While (reader.Read())
                'If reader.Name = "Addenda" Then Exit While
            End While
            Me.TextBox2.Text = "Estructura Valida...."
        Catch ex As System.Xml.Schema.XmlSchemaValidationException
            Me.TextBox2.Text = ex.Message
        Finally
            reader = Nothing
            settings = Nothing
        End Try

    End Sub
End Class
