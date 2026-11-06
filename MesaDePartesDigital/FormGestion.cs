using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MesaDePartesDigital
{
    public partial class FormGestion : Form
    {
        public FormGestion()
        {
            InitializeComponent();
        }
        private void FormGestion_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void CargarTabla()
        {
            List<Expediente> lista = Expediente.CargarTodos();
            dgvExpedientes.DataSource = null;
            dgvExpedientes.DataSource = lista;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscarDNI.Text.Trim();
            List<Expediente> listaCompleta = Expediente.CargarTodos();
            List<Expediente> listaFiltrada = new List<Expediente>();

            foreach (Expediente exp in listaCompleta)
            {
                if (exp.DNI == busqueda || exp.Codigo == busqueda)
                {
                    listaFiltrada.Add(exp);
                }
            }

            if (listaFiltrada.Count > 0)
            {
                dgvExpedientes.DataSource = null;
                dgvExpedientes.DataSource = listaFiltrada;
            }
            else
            {
                MessageBox.Show("No se encontró ningún expediente con el dato ingresado.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtBuscarDNI.Clear();
            CargarTabla();
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            List<Expediente> lista = Expediente.CargarTodos();


            for (int i = 0; i < lista.Count - 1; i++)
            {
                for (int j = 0; j < lista.Count - 1 - i; j++)
                {
                    if (string.Compare(lista[j].Codigo, lista[j + 1].Codigo) > 0)
                    {
                        Expediente temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                    }
                }
            }

            dgvExpedientes.DataSource = null;
            dgvExpedientes.DataSource = lista;
            MessageBox.Show("Expedientes ordenados por código exitosamente.", "Ordenamiento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
