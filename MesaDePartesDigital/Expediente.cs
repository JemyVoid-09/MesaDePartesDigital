using System.Collections.Generic;
using System.IO;

namespace MesaDePartesDigital
{
    // Cambiamos 'class' por 'struct' según los requisitos del profesor
    internal struct Expediente
    {
        // Campos públicos (sin get ni set, tal como lo exige el sílabo)
        public string Codigo;
        public string DNI;
        public string Nombres;
        public string TipoDoc;
        public string Asunto;
        public string Fecha;

        // Ruta del archivo binario (.dat)
        private static string rutaArchivo = "expedientes.dat";

        // Método estático para guardar un nuevo expediente en el archivo binario
        public static void GuardarArchivo(Expediente exp)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(rutaArchivo, FileMode.Append, FileAccess.Write)))
            {
                bw.Write(exp.Codigo);
                bw.Write(exp.DNI);
                bw.Write(exp.Nombres);
                bw.Write(exp.TipoDoc);
                bw.Write(exp.Asunto);
                bw.Write(exp.Fecha);
            }
        }

        // Método estático para cargar y leer todos los expedientes del archivo binario
        public static List<Expediente> CargarTodos()
        {
            List<Expediente> lista = new List<Expediente>();

            // Si el archivo no existe, retornamos la lista vacía
            if (!File.Exists(rutaArchivo)) return lista;

            using (BinaryReader br = new BinaryReader(new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read)))
            {
                // Recorremos el archivo binario de forma secuencial
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    Expediente exp = new Expediente
                    {
                        Codigo = br.ReadString(),
                        DNI = br.ReadString(),
                        Nombres = br.ReadString(),
                        TipoDoc = br.ReadString(),
                        Asunto = br.ReadString(),
                        Fecha = br.ReadString()
                    };
                    lista.Add(exp);
                }
            }
            return lista;
        }
    }
}
