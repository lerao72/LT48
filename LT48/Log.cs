using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT48
{
    public static class Log
    {
        public enum ModoGuardado { PorPeticion, Directo }
        private static ModoGuardado modo = ModoGuardado.Directo;

        public static void setModoGuardado(ModoGuardado valor) { modo = valor; }
        public static ModoGuardado getModoGuardado() { return modo; }

        private static string getNombreFichero() { return "log" + DateTime.Now.ToString("yyyyMMdd") + ".log"; }

        private static string txtLog = Files.cargaFicheroTXTToString(getNombreFichero());

        public static string getLog()
        {
            if (modo == ModoGuardado.PorPeticion)
                return txtLog;
            else
                try { return Files.cargaFicheroTXTToString(getNombreFichero()); }
                catch { return "Error al cargar el archivo de log"; }
        }

        public static void log(string mensaje)
        {
            string aux = DateTime.Now.ToString("HH:mm:ss") + " " + mensaje + "\r\n";

            if (modo == ModoGuardado.Directo)
                try { Files.guardaFicheroTXTAppend(getNombreFichero(), aux); }
                catch {; }
            else
                txtLog += aux;
        }

        public static void log(object obj, Exception ex)
        {
            log(obj.ToString() + ": " + ex.Message);
        }

        public static void guardaLog()
        {
            if (modo == ModoGuardado.PorPeticion)
                Files.guardaFicheroTXT(getNombreFichero(), txtLog);
        }

        public static void deleteLog()
        {
            txtLog = "";
        }

        public static void borrarArchivoLog()
        {
            try { System.IO.File.Delete(getNombreFichero()); }
            catch {; }
        }

    } // end class
}
