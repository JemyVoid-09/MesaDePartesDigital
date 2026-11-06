using System;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "admin" && txtClave.Text == "12345")
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
    }
}