using System;
using System.IO;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormRegistro : Form
    {
        private string rutaExpedientes = "expedientes.dat"; // Archivo binario directo

        public FormRegistro()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(cboTipoDoc.Text) ||
                string.IsNullOrWhiteSpace(txtAsunto.Text))
            {
                MessageBox.Show("Por favor complete todos los campos requeridos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtDNI.Text.Length != 8)
            {
                MessageBox.Show("El DNI debe tener exactamente 8 dígitos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Capturamos los datos en variables simples (sin POO ni clases externas)
            string codigo = txtCodigo.Text.Trim();
            string dni = txtDNI.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string tipoDoc = cboTipoDoc.Text.Trim();
            string asunto = txtAsunto.Text.Trim();
            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Guardamos directamente en el archivo binario usando BinaryWriter
            BinaryWriter bw = null;
            try
            {
                bw = new BinaryWriter(new FileStream(rutaExpedientes, FileMode.Append, FileAccess.Write));
                bw.Write(codigo);
                bw.Write(dni);
                bw.Write(nombres);
                bw.Write(tipoDoc);
                bw.Write(asunto);
                bw.Write(fecha);

                MessageBox.Show("¡Expediente registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el expediente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bw != null) bw.Close();
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtDNI.Clear();
            txtNombres.Clear();
            cboTipoDoc.SelectedIndex = -1;
            txtAsunto.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
