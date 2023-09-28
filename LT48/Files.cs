using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT48
{
    public static class Files
    {
        private static string rc = "\r\n";
        private static Encoding encoding = Encoding.UTF8;
        public static void setEncoding(Encoding e) { encoding = e; }

        public static string cargaFicheroTXTToString(string nombreFichero)
        {
            string resultado = "";
            string linea = "";
            if (System.IO.File.Exists(nombreFichero))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(nombreFichero, encoding);
                while ((linea = sr.ReadLine()) != null)
                {
                    resultado += linea + rc;
                } // end while
                sr.Close();
            } // end if

            return resultado;
        }

        public static int guardaFicheroTXT(string nombreFichero, string texto)
        {
            int resultado = 0;

            try
            {
                if (System.IO.File.Exists(nombreFichero))
                {
                    System.IO.File.Delete(nombreFichero);
                } //if

                System.IO.StreamWriter sw = new System.IO.StreamWriter(nombreFichero, false, encoding);
                sw.Write(texto, encoding);
                sw.Close();
                resultado = 1;
            }
            catch (Exception guardaFicheroTXTException)
            {
                Log.log(guardaFicheroTXTException.ToString());
            }

            return resultado;
        }

        public static int guardaFicheroTXTAppend(string nombreFichero, string texto)
        {
            int resultado = 0;

            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(nombreFichero, true, encoding);
                sw.Write(texto, encoding);
                sw.Close();
                resultado = 1;
            }
            catch (Exception guardaFicheroTXTException)
            {
                Log.log(guardaFicheroTXTException.ToString());
            }

            return resultado;
        }

    } // end class
} // end namespace
