using System;
using System.IO;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormRegistro : Form
    {
        //nombre del archivo donde se guardada todo los expedientes
        private string rutaExpedientes = "expedientes.dat"; 

        public FormRegistro()
        {
            //dibuja los botones del formulario
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //se valida los campos vacios, el IsNullOrWWhiteSpace revisa que no haya dejado espacios en blanco
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(cboTipoDoc.Text) ||
                string.IsNullOrWhiteSpace(txtAsunto.Text))
            {
                MessageBox.Show("Por favor complete todos los campos requeridos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //se detiene el codigo si ve que falta rellenar algo
                return;
            }

            //validacion del DNI, aca se mide cuantos cifras tiene el texto del DNI con .length
            //si o si tiene que tener 8 digitos
            if (txtDNI.Text.Length != 8)
            {
                MessageBox.Show("El DNI debe tener exactamente 8 dígitos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //se guarda los datos en varibles normales
            string codigo = txtCodigo.Text.Trim();
            string dni = txtDNI.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string tipoDoc = cboTipoDoc.Text.Trim();
            string asunto = txtAsunto.Text.Trim();
            //DateTime.Now.ToString verifica la fecha y hora exacta de la pc en ese momento
            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            
            BinaryWriter bw = null;
            try
            {
                //FileMode.Append hace que se habra el archivo dat y que coloque el nuevo expediente al final
                //sin que se borre o elimine los anteriores
                bw = new BinaryWriter(new FileStream(rutaExpedientes, FileMode.Append, FileAccess.Write));
                //se escribe los datos uno por uno en formato binario
                bw.Write(codigo);
                bw.Write(dni);
                bw.Write(nombres);
                bw.Write(tipoDoc);
                bw.Write(asunto);
                bw.Write(fecha);

                MessageBox.Show("¡Expediente registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //se borra las casillas para un nuevo registro
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el expediente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //se cierra el archivo al terminar
                if (bw != null) bw.Close();
            }
        }

        //metodo para limpiar las casillas despues de guardar
        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtDNI.Clear();
            txtNombres.Clear();
            // se selecciona cualquier tipo de cita del ComBox
            cboTipoDoc.SelectedIndex = -1;
            txtAsunto.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //se cierra la ventana y regresa la menú
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
