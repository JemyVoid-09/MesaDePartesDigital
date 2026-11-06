using System;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormRegistro : Form
    {
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

            Expediente nuevoExp = new Expediente
            {
                Codigo = txtCodigo.Text,
                DNI = txtDNI.Text,
                Nombres = txtNombres.Text,
                TipoDoc = cboTipoDoc.Text,
                Asunto = txtAsunto.Text,
                Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            Expediente.GuardarArchivo(nuevoExp);
            MessageBox.Show("¡Expediente registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarCampos();
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
