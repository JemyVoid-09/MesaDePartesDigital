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
            InitializeComponent();
        }

        private void FormGestion_Load(object sender, EventArgs e)
        {
            ConfigurarColumnas();
            CargarTabla();
        }

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

        private void CargarTabla()
        {
            dgvExpedientes.Rows.Clear();

            if (File.Exists(rutaExpedientes))
            {
                BinaryReader br = null;
                try
                {
                    br = new BinaryReader(new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read));
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscarDNI.Text.Trim();
            if (string.IsNullOrEmpty(busqueda))
            {
                MessageBox.Show("Ingrese un DNI o Código para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvExpedientes.Rows.Clear();
            if (File.Exists(rutaExpedientes))
            {
                BinaryReader br = null;
                try
                {
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

                        
                        if (dni == busqueda || codigo == busqueda)
                        {
                            dgvExpedientes.Rows.Add(codigo, dni, nombres, tipoDoc, asunto, fecha);
                            encontrado = true;
                        }
                    }
                    br.Close();

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
            CargarTabla();
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            
            int n = dgvExpedientes.Rows.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (!dgvExpedientes.Rows[j].IsNewRow && !dgvExpedientes.Rows[j + 1].IsNewRow)
                    {
                        string codigo1 = dgvExpedientes.Rows[j].Cells[0].Value?.ToString() ?? "";
                        string codigo2 = dgvExpedientes.Rows[j + 1].Cells[0].Value?.ToString() ?? "";

                        if (string.Compare(codigo1, codigo2) > 0)
                        {
                            
                            for (int c = 0; c < dgvExpedientes.Columns.Count; c++)
                            {
                                object temp = dgvExpedientes.Rows[j].Cells[c].Value;
                                dgvExpedientes.Rows[j].Cells[c].Value = dgvExpedientes.Rows[j + 1].Cells[c].Value;
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
