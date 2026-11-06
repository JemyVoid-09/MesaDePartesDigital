using System.Collections.Generic;
using System.IO;

namespace MesaDePartesDigital
{
    internal class Expediente
    {
        public string Codigo { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string TipoDoc { get; set; }
        public string Asunto { get; set; }
        public string Fecha { get; set; }

        private static string rutaArchivo = "expedientes.txt";

        public static void GuardarArchivo(Expediente exp)
        {
            using (StreamWriter sw = new StreamWriter(rutaArchivo, true))
            {
                sw.WriteLine($"{exp.Codigo}|{exp.DNI}|{exp.Nombres}|{exp.TipoDoc}|{exp.Asunto}|{exp.Fecha}");
            }
        }

        public static List<Expediente> CargarTodos()
        {
            List<Expediente> lista = new List<Expediente>();
            if (!File.Exists(rutaArchivo)) return lista;

            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                string[] partes = linea.Split('|');
                if (partes.Length == 6)
                {
                    lista.Add(new Expediente
                    {
                        Codigo = partes[0],
                        DNI = partes[1],
                        Nombres = partes[2],
                        TipoDoc = partes[3],
                        Asunto = partes[4],
                        Fecha = partes[5]
                    });
                }
            }
            return lista;
        }
    }
}
