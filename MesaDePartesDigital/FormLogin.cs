using System;
using System.Windows.Forms;
using System.IO;

namespace MesaDePartesDigital
{
    public partial class FormLogin : Form
    {
        private string rutaUsuarios = "usuarios.txt";

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
            else if (File.Exists("usuarios.txt"))
            {
                string[] lineas = File.ReadAllLines("usuarios.txt");
                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split('|');
                    
                    if (datos.Length == 4)
                    {
                        string usuarioFile = datos[0].Trim();
                        string claveFile = datos[1].Trim();
                        string rolFile = datos[2].Trim();

                        if (usuarioFile == usuarioIngresado && claveFile == claveIngresada)
                        {
                            accesoConcedido = true;
                            rolUsuario = rolFile;
                            break;
                        }
                    }
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

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Escribe un usuario y contraseña para registrarte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (File.Exists(rutaUsuarios))
            {
                string[] lineas = File.ReadAllLines(rutaUsuarios);
                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split('|');
                    if (datos.Length > 0 && datos[0] == usuario)
                    {
                        MessageBox.Show("Ese nombre de usuario ya existe. Elige otro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            File.AppendAllText(rutaUsuarios, usuario + "|" + clave + Environment.NewLine);
            MessageBox.Show("¡Usuario registrado con éxito! Ahora dale clic a Ingresar.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}