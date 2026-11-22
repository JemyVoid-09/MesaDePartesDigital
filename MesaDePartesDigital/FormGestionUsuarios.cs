using System;
using System.IO;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormGestionUsuarios : Form
    {
        private string rutaUsuarios = "usuarios.txt";

        public FormGestionUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        
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
            if (System.IO.File.Exists("usuarios.txt"))
            {
                string[] lineas = System.IO.File.ReadAllLines("usuarios.txt");
                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split('|');
                    if (datos.Length == 4)
                    {
                        dgvUsuarios.Rows.Add(datos[0], datos[1], datos[2], datos[3]);
                    }
                }
            }
        }

        
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (usuario == "" || clave == "")
            {
                MessageBox.Show("Por favor completa todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            using (StreamWriter sw = File.AppendText(rutaUsuarios))
            {
                sw.WriteLine($"{usuario},{clave}");
            }

            MessageBox.Show("Usuario registrado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtUsuario.Clear();
            txtClave.Clear();
            CargarUsuarios();
        }

        
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow != null)
            {
                string usuarioSeleccionado = dgvUsuarios.CurrentRow.Cells["Usuario"].Value.ToString();

                
                if (usuarioSeleccionado.ToLower() == "admin")
                {
                    MessageBox.Show("¡Acción denegada! El usuario administrador principal no se puede eliminar.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                DialogResult resultado = MessageBox.Show($"¿Deseas eliminar al usuario '{usuarioSeleccionado}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    dgvUsuarios.Rows.Remove(dgvUsuarios.CurrentRow);

                    
                    using (StreamWriter sw = new StreamWriter(rutaUsuarios, false))
                    {
                        foreach (DataGridViewRow row in dgvUsuarios.Rows)
                        {
                            if (row.Cells["Usuario"].Value != null)
                            {
                                sw.WriteLine($"{row.Cells["Usuario"].Value},{row.Cells["Clave"].Value}");
                            }
                        }
                    }

                    MessageBox.Show("Usuario eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario de la tabla para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void FormGestionUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow != null && dgvUsuarios.CurrentRow.Cells[0].Value != null)
            {
                string usuarioSeleccionado = dgvUsuarios.CurrentRow.Cells[0].Value.ToString();

                if (usuarioSeleccionado.ToLower() == "admin")
                {
                    MessageBox.Show("¡Acción denegada! El usuario administrador principal no se puede eliminar.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult resultado = MessageBox.Show($"¿Deseas eliminar al usuario '{usuarioSeleccionado}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    dgvUsuarios.Rows.Remove(dgvUsuarios.CurrentRow);

                    using (StreamWriter sw = new StreamWriter(rutaUsuarios, false))
                    {
                        foreach (DataGridViewRow row in dgvUsuarios.Rows)
                        {
                            if (!row.IsNewRow && row.Cells[0].Value != null)
                            {
                                string user = row.Cells[0].Value.ToString();
                                string pass = row.Cells[1].Value?.ToString() ?? "";
                                string rol = row.Cells[2].Value?.ToString() ?? "";
                                string correo = row.Cells[3].Value?.ToString() ?? "";

                                sw.WriteLine($"{user}|{pass}|{rol}|{correo}");
                            }
                        }
                    }
                    MessageBox.Show("Usuario eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtClave.Text.Trim();
            string rol = comboBox1.SelectedItem.ToString();
            string correo = txtCorreo.Text.Trim();

            
            string nuevaLinea = $"{usuario}|{contrasena}|{rol}|{correo}";

            try
            {
               
                System.IO.File.AppendAllText("usuarios.txt", nuevaLinea + Environment.NewLine);

                MessageBox.Show("¡Usuario registrado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtUsuario.Clear();
                txtClave.Clear();
                comboBox1.SelectedIndex = -1;
                txtCorreo.Clear();

                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}