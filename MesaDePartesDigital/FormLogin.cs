using System;
using System.Windows.Forms;
using System.IO;

namespace MesaDePartesDigital
{
    public partial class FormLogin : Form
    {
        private string rutaUsuarios = "usuarios.dat"; 

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text.Trim();
            string claveIngresada = txtClave.Text.Trim();
            bool accesoConcedido = false;
            string rolUsuario = "";

            
            if (usuarioIngresado == "admin" && claveIngresada == "12345")
            {
                accesoConcedido = true;
                rolUsuario = "Administrador";
            }
            else if (File.Exists(rutaUsuarios))
            {
                
                BinaryReader br = null;
                try
                {
                    br = new BinaryReader(new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read));
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        string usuarioFile = br.ReadString();
                        string claveFile = br.ReadString();
                        string rolFile = br.ReadString();
                        string extraFile = br.ReadString(); 

                        if (usuarioFile == usuarioIngresado && claveFile == claveIngresada)
                        {
                            accesoConcedido = true;
                            rolUsuario = rolFile;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (br != null) br.Close();
                }
            }

            if (accesoConcedido)
            {
                MessageBox.Show($"¡Bienvenido, {usuarioIngresado}!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormPrincipal principal = new FormPrincipal(rolUsuario);
                principal.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();
            string rolPredeterminado = "Usuario"; 

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Escribe un usuario y contraseña para registrarte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (File.Exists(rutaUsuarios))
            {
                BinaryReader br = null;
                try
                {
                    br = new BinaryReader(new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read));
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        string usuarioFile = br.ReadString();
                        string claveFile = br.ReadString();
                        string rolFile = br.ReadString();
                        string extraFile = br.ReadString();

                        if (usuarioFile == usuario)
                        {
                            MessageBox.Show("Ese nombre de usuario ya existe. Elige otro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            br.Close();
                            return;
                        }
                    }
                    br.Close();
                }
                catch (Exception)
                {
                    
                }
            }

            
            BinaryWriter bw = null;
            try
            {
                bw = new BinaryWriter(new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write));
                bw.Write(usuario);
                bw.Write(clave);
                bw.Write(rolPredeterminado);
                bw.Write(""); 
                bw.Close();

                MessageBox.Show("¡Usuario registrado con éxito! Ahora dale clic a Ingresar.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (bw != null) bw.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}