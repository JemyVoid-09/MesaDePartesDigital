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
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();
            bool accesoConcedido = false;

            // 1. Mantiene tu usuario administrador original
            if (usuario == "admin" && clave == "12345")
            {
                accesoConcedido = true;
            }
            // 2. Busca si el usuario se registró en usuarios.txt
            else if (File.Exists(rutaUsuarios))
            {
                string[] lineas = File.ReadAllLines(rutaUsuarios);
                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split('|');
                    if (datos.Length == 2 && datos[0] == usuario && datos[1] == clave)
                    {
                        accesoConcedido = true;
                        break;
                    }
                }
            }

            if (accesoConcedido)
            {
                MessageBox.Show("¡Bienvenido al Sistema de Mesa de Partes!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormPrincipal principal = new FormPrincipal();
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
    }
}