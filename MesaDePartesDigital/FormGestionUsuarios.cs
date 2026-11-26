using System;
using System.Windows.Forms;
using System.IO;

namespace MesaDePartesDigital
{
    public partial class FormGestionUsuarios : Form
    {
        private string rutaUsuarios = "usuarios.dat"; 

        public FormGestionUsuarios()
        {
            //se dibuja la tabla y los controles
            InitializeComponent();
            CargarUsuarios();
        }

        //lee el archivo binario y dibuja los usuarios en la tabla 
        private void CargarUsuarios()
        {
            dgvUsuarios.Rows.Clear();
            if (dgvUsuarios.Columns.Count == 0)
            {
                dgvUsuarios.Columns.Add("colUsuario", "Usuario");
                dgvUsuarios.Columns.Add("colContrasena", "Contraseña");
                dgvUsuarios.Columns.Add("colRol", "Rol");
                dgvUsuarios.Columns.Add("colCorreo", "Correo");
            }

            if (File.Exists(rutaUsuarios))
            {
                BinaryReader br = null;
                try
                {
                    br = new BinaryReader(new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read));
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        string usuario = br.ReadString();
                        string clave = br.ReadString();
                        string rol = br.ReadString();
                        string correo = br.ReadString();

                        dgvUsuarios.Rows.Add(usuario, clave, rol, correo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo binario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (br != null) br.Close();
                }
            }
        }

        //se registra un nuevo usuario poniendole un nuevo rol de administrador o trabajador
        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text) ||
                comboBox1.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //trim() quita espacios de mas al inicio o final para que se guarden limpios
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtClave.Text.Trim();
            string rol = comboBox1.SelectedItem.ToString();
            string correo = txtCorreo.Text.Trim();

            try
            {

                //aca se libera automaticamente cuando se termina de escribir 
                using (BinaryWriter bw = new BinaryWriter(new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write)))
                {
                    bw.Write(usuario);
                    bw.Write(contrasena);
                    bw.Write(rol);
                    bw.Write(correo);
                }

                MessageBox.Show("¡Usuario registrado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtUsuario.Clear();
                txtClave.Clear();
                comboBox1.SelectedIndex = -1;
                txtCorreo.Clear();

                //aca se recarga de nuevo la tabla para mostrar al nuevo usuario
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow != null && dgvUsuarios.CurrentRow.Cells[0].Value != null)
            {
                string usuarioSeleccionado = dgvUsuarios.CurrentRow.Cells[0].Value.ToString();

                //aca se evita por completo que eliminen al administrador principal o administradores
                if (usuarioSeleccionado.ToLower() == "admin")
                {
                    MessageBox.Show("¡Acción denegada! El usuario administrador principal no se puede eliminar.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult resultado = MessageBox.Show($"¿Deseas eliminar al usuario '{usuarioSeleccionado}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    //se elimina la fila de la tabla
                    dgvUsuarios.Rows.Remove(dgvUsuarios.CurrentRow);

                    // se sobreescribe el archivo binario con las filas restantes
                    try
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(rutaUsuarios, FileMode.Create, FileAccess.Write)))
                        {
                            foreach (DataGridViewRow row in dgvUsuarios.Rows)
                            {
                                //row.IsNewRow esta parte sirve para ignorar la fila en blanco que da windows forms
                                //row.Cells[0].Value != null aca verifica que las casillas del usuario este con datos reales y no vacio
                                if (!row.IsNewRow && row.Cells[0].Value != null)
                                {
                                    //los ?? "" ayuda a que si alguna celda estuviera vacia ,evita que el programa falle de golpe
                                    string user = row.Cells[0].Value.ToString();
                                    string pass = row.Cells[1].Value?.ToString() ?? "";
                                    string rol = row.Cells[2].Value?.ToString() ?? "";
                                    string correo = row.Cells[3].Value?.ToString() ?? "";

                                    bw.Write(user);
                                    bw.Write(pass);
                                    bw.Write(rol);
                                    bw.Write(correo);
                                }
                            }
                        }
                        MessageBox.Show("Usuario eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario de la tabla para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormGestionUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        
        private void btnRegistrar_Click(object sender, EventArgs e) { }
        private void btnEliminar_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}