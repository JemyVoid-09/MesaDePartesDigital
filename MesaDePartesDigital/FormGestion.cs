using System;
using System.IO;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormGestion : Form
    {
        private string rutaExpedientes = "expedientes.dat"; 

        public FormGestion()
        {
            //dibuja la tabla DataGridView y los botones
            InitializeComponent();
        }

        //parte que se ejecuta cuando se abre la ventana
        private void FormGestion_Load(object sender, EventArgs e)
        {
            ConfigurarColumnas();
            CargarTabla();
        }

        //se crea las columnas dentro de la tabla
        private void ConfigurarColumnas()
        {
            if (dgvExpedientes.Columns.Count == 0)
            {
                dgvExpedientes.Columns.Add("colCodigo", "Código");
                dgvExpedientes.Columns.Add("colDNI", "DNI");
                dgvExpedientes.Columns.Add("colNombres", "Nombres");
                dgvExpedientes.Columns.Add("colTipoDoc", "Tipo Doc");
                dgvExpedientes.Columns.Add("colAsunto", "Asunto");
                dgvExpedientes.Columns.Add("colFecha", "Fecha");
            }
        }

        //esto lee todo el archivo binario expediente por expediente y lo muestra en la tabla
        private void CargarTabla()
        {
            //se limpia la tabla para que no haya datos viejos
            dgvExpedientes.Rows.Clear();

            if (File.Exists(rutaExpedientes))
            {
                BinaryReader br = null;
                try
                {
                    br = new BinaryReader(new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read));
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {

                        //lee los 6 datos hasta llegar al final del archivo
                        string codigo = br.ReadString();
                        string dni = br.ReadString();
                        string nombres = br.ReadString();
                        string tipoDoc = br.ReadString();
                        string asunto = br.ReadString();
                        string fecha = br.ReadString();

                        
                        dgvExpedientes.Rows.Add(codigo, dni, nombres, tipoDoc, asunto, fecha);
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

        //revisa el archivo binario uno por uno buscando si hay algun dato que sea doble (dni o código)
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscarDNI.Text.Trim();
            if (string.IsNullOrEmpty(busqueda))
            {
                MessageBox.Show("Ingrese un DNI o Código para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //se limpia la tabla para mostrar solamente el usuario encontrado
            dgvExpedientes.Rows.Clear();
            if (File.Exists(rutaExpedientes))
            {
                BinaryReader br = null;
                try
                {
                    //abre el archivo binario para poder buscar desde adentro
                    br = new BinaryReader(new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read));
                    bool encontrado = false;

                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        string codigo = br.ReadString();
                        string dni = br.ReadString();
                        string nombres = br.ReadString();
                        string tipoDoc = br.ReadString();
                        string asunto = br.ReadString();
                        string fecha = br.ReadString();

                        //compara si el codigo o el DNI coincide con lo que el usuario busco, y si conincide, lo muestra
                        if (dni == busqueda || codigo == busqueda)
                        {
                            dgvExpedientes.Rows.Add(codigo, dni, nombres, tipoDoc, asunto, fecha);
                            encontrado = true;
                        }
                    }
                    br.Close();

                    //si leyó y no encontro nada, avisa al usuario y le recarga la tabla completa
                    if (!encontrado)
                    {
                        MessageBox.Show("No se encontró ningún expediente con el dato ingresado.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarTabla(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtBuscarDNI.Clear();
            //carga todos los regsitros del archivo original
            CargarTabla();
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            
            //cantidad de filas actuales en la tabla
            int n = dgvExpedientes.Rows.Count;
            //1B: controla cuantas vueltas daremos a la tabla
            for (int i = 0; i < n - 1; i++)
            {
                //2B: aqui compara cada fila
                for (int j = 0; j < n - 1 - i; j++)
                {
                    //aqui validamos que no este tocando en la ultima fila vacia que windows forms da por defecto
                    if (!dgvExpedientes.Rows[j].IsNewRow && !dgvExpedientes.Rows[j + 1].IsNewRow)
                    {
                        string codigo1 = dgvExpedientes.Rows[j].Cells[0].Value?.ToString() ?? "";
                        string codigo2 = dgvExpedientes.Rows[j + 1].Cells[0].Value?.ToString() ?? "";

                        //aca se compara alfabetica y numéricamente entre codigo1 y codigo2, el mayor va después
                        if (string.Compare(codigo1, codigo2) > 0)
                        {

                            //se recorre las columnas para cambiar toda la fila de lugar
                            for (int c = 0; c < dgvExpedientes.Columns.Count; c++)
                            {
                                //aca se guarda temporalmente el valor de j(filas) en temp
                                object temp = dgvExpedientes.Rows[j].Cells[c].Value;
                                //se pasa el de aca abajo hacia arriba
                                dgvExpedientes.Rows[j].Cells[c].Value = dgvExpedientes.Rows[j + 1].Cells[c].Value;
                                //y ponemos aca el valor que estaba en temp
                                dgvExpedientes.Rows[j + 1].Cells[c].Value = temp;
                            }
                        }
                    }
                }
            }

            MessageBox.Show("Expedientes ordenados por código exitosamente.", "Ordenamiento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBuscarDNI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
