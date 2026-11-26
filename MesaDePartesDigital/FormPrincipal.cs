using System;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormPrincipal : Form
    {
        // se crea una variable (string) que actua como memoria que guarda
        //temporalmente el rol del usuario que inicio sesion
        private string rolActual;  
        
        // se recibe el rol que le mando el FormLogin al abrirse
        public FormPrincipal(string rol)
        {
            // se dibuja los botones a la pantalla
            InitializeComponent();
            rolActual = rol;

            // si la persona que ingresa en un "trabajador" se oculta el boton de gestion de usuarios
            // para evitar que pueda borrar o crear cuentas ,por seguridad
            if (rolActual == "Trabajador")
            {
                // vuelve invisible el boton si es trabajador
                btnGestionUsuarios.Visible = false;
            }
        }

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //el ShowDialog() abre la ventana de registro tapando la anterior ventana hasta que cierra
            FormRegistro reg = new FormRegistro();
            reg.ShowDialog();
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            // se abre la ventana donde esta la lista, la busqueda y el ordenamiento de los expedientes
            FormGestion gest = new FormGestion();
            gest.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // aca vuelve a mostrar el login y lo cierra por completo
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            // esta parte solo es exclusivo para los administradores que solo ellos pueden ver esta ventana
            FormGestionUsuarios frm = new FormGestionUsuarios();
            frm.ShowDialog();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
