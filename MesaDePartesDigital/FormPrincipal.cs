using System;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormPrincipal : Form
    {
        private string rolActual;
        public FormPrincipal(string rol)
        {
            InitializeComponent();
            rolActual = rol;

            
            if (rolActual == "Trabajador")
            {
                btnGestionUsuarios.Visible = false;
            }
        }
        public FormPrincipal()
        {
            InitializeComponent();
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            FormRegistro reg = new FormRegistro();
            reg.ShowDialog();
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            FormGestion gest = new FormGestion();
            gest.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            FormGestionUsuarios frm = new FormGestionUsuarios();
            frm.ShowDialog();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
