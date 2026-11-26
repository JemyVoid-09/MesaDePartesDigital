using System;
using System.Windows.Forms;
using System.IO;// Librería obligatoria para poder trabajar con archivos (FileStream, BinaryReader, BinaryWriter)

namespace MesaDePartesDigital
{
    public partial class FormLogin : Form
    {
        //aca creamos una variable con el nombre del archivo binario de usuarios
        private string rutaUsuarios = "usuarios.dat"; 

        //cuando se ejecuta o  abre el formulario lo primero que se ejecuta es este metodo
        //el formulario formlogin y el "InitialComponent()" hace que ponga en la pantalla
        // todos los botones , cajas de texto y etiquetas
        public FormLogin()
        {
            InitializeComponent();
        }

        // esta parte de hacer clic en el boton de "ingresar"
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            //los .trim() borra auntomaticamente los espacion vacios que nosotros 
            //por error hemos puesto por ejemplo: "admin "
            string usuarioIngresado = txtUsuario.Text.Trim();
            string claveIngresada = txtClave.Text.Trim();
            // aca se arranca en falso , pero esto cambiara si se encuentra al usuario
            bool accesoConcedido = false;
            string rolUsuario = "";

            // Aqui se valida si el usuario es el administrador del sistema
            if (usuarioIngresado == "admin" && claveIngresada == "12345")
            {
                accesoConcedido = true;
                rolUsuario = "Administrador";
            }
            // Si en caso no es el administrador, se busca dentro del archivo binario solo si el archivo 
            // existe para evitar crasheos
            else if (File.Exists(rutaUsuarios))
            {
                // BinaryReades sirve para leer los datos que estan guardados en formato binario 
                BinaryReader br = null;
                try
                {
                    // aca se habre el archivo para leer 
                    br = new BinaryReader(new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read));
                    // Hacemos un bucle while , la position es el numero de byte por el que va a leer el programa dentro del archivo
                    // y el length es el tamaño total del archivo en bytes ,cuanto pesa o mide
                    // va leendo registro por registro hasta agotar el archivo
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        // aca se lee exactamente en el mismo orden con lo que se escribio si no se cofunde el programa
                        string usuarioFile = br.ReadString();
                        string claveFile = br.ReadString();
                        string rolFile = br.ReadString();
                        string extraFile = br.ReadString(); 

                        // en esta parte comparamos lo que esta en el archivo con lo que escribio el usuario
                        if (usuarioFile == usuarioIngresado && claveFile == claveIngresada)
                        {
                            accesoConcedido = true;
                            // aca se guarda si es administrador o trabajador
                            rolUsuario = rolFile;
                            //el break es cuando ya encontramos lo que es el usuario y no hace falta seguir buscando
                            break; 
                        }
                    }
                }
                //esta parte se encarga de atrapar el error y muestra una ventanita de alerta ,evitando que se cierre de golpe
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //verifica si el lector es binario y no este vacio y el br.Close() cierra el archivo binario
                    //si lo olvidas cerrarlo
                    if (br != null) br.Close();
                }
            }
            // si paso a verdadero, se abre el menu principal pasandole su rol
            if (accesoConcedido)
            {
                MessageBox.Show($"¡Bienvenido, {usuarioIngresado}!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormPrincipal principal = new FormPrincipal(rolUsuario);
                principal.Show();
                this.Hide();// se oculta la ventana del login actual
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //cierra todo el programa por completo
            Application.Exit();
        }
        
        //parte donde se registra un nuevo usuario desde la pantalla
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();
            string rolPredeterminado = "Usuario"; 
            //se valida que no deje nada en blanco
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Escribe un usuario y contraseña para registrarte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //se corta la ejecucion para que no se pueda guardar nada que no este completo
                return;
            }

            //se revisa si ya hay usuarios existentes para evitar duplicados
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
                            //no se registra porque ya esta repetido
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
                //el FiloMode.Append hace que agregue a la persona registrada al ultimo respetando los que ya estaba 
                //guardado antes
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