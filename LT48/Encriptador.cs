using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT48
{
    public static class Encriptador
    {
        /*
         * ADVERTENCIA: Cambiar los patrones. Puede utilizarse la aplicación mezclaCadena para, una vez establecido
         * un juego de caractéres iniciales, obtener una combinación de ellos (o si no se le especifica una 
         * cadena, la función utilizará un juego predeterminado).
         */
        private static string patron_busqueda = "E!5@-LF.;6QIwbó)KeM 1#zú¿8AÍ,jCZuqTWngpNB94çDUOrPxí0Éh*_¡vÚñÑly:7kX?Hm/áÓis(VcéJ+GoÇÁfadSt2RY3&";
        private static string patron_encripta = "CG_úl;nqc,-+)á(L34éíÍ¡aNÓÑÚ9:Dó#zbdxw¿Y1r*MS&TQ?jiX8tFABpk çKÇoPJHh/!2ñ50yÉu@UOI.RfWe6EgZm7VvÁs";

        /// <summary>
        /// Si queremos definir algún tipo de patrón provisional distinto.
        /// </summary>
        public static void establecerPatrones(string patron_busquedaParam, string patron_encriptaParam)
        {
            patron_busqueda = patron_busquedaParam;
            patron_encripta = patron_encriptaParam;
        }

        /// <summary>
        /// Herramienta, que utilizada de forma externa nos permitiría desordenar una cadena de caractéres
        /// para poder obtener patrones.
        /// 
        /// La cadena de entrada no puede contener caractéres repetidos y si los contiene, devolverá como 
        /// error una cadena vacía.
        /// </summary>
        public static string mezclaCadena(string cadenaParam)
        {
            string cadena = cadenaParam == "" ?
                "abcçdefghijklmnñopqrstuvwxyzáéíóúABCÇDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚ0123456789,.;:@#-_()+*/¿?¡!&" :
                cadenaParam;

            // Comprobamos que no hay repetidos
            bool encontrado = false;
            Int32 i = 0;
            Int32 tamanioCadena = cadena.Length;
            while (!encontrado && i < tamanioCadena - 1)
            {
                encontrado = cadena.IndexOf(cadena.Substring(i, 1), i + 1) >= 0;
                i++;
            }

            if (!encontrado)
            {
                string nuevaCadena;
                for (int j = 0; j <= new Random().Next(5, 10); j++)
                {
                    nuevaCadena = "";
                    while (cadena.Length > 0)
                    {
                        Int32 posicion = new Random().Next(0, cadena.Length);
                        string caracter = cadena.Substring(posicion, 1);
                        nuevaCadena += caracter;
                        cadena = cadena.Replace(caracter, "");
                    }
                    cadena = nuevaCadena;
                }
            }
            else
                cadena = "";

            return cadena;
        }

        /// <summary>
        /// Encripta/codifica la cadena de entrada según los dos patrones.
        /// </summary>
        public static string encriptarCadena(string cadena)
        {
            int idx;
            string result = "";

            for (idx = 0; idx < cadena.Length; idx++)
            {
                result += encriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            }

            return result;
        }

        private static string encriptarCaracter(string caracter, int variable, int a_indice)
        {
            int indice;

            if (patron_busqueda.IndexOf(caracter) >= 0)
            {
                indice = (patron_busqueda.IndexOf(caracter) + variable + a_indice) % patron_busqueda.Length;
                return patron_encripta.Substring(indice, 1);
            }

            return caracter;
        }

        /// <summary>
        /// Desencripta/decodifica una cadena previamente codificada para obtener la cadena original
        /// </summary>
        public static string desEncriptarCadena(string cadena)
        {
            int idx;
            string result = "";
            for (idx = 0; idx < cadena.Length; idx++)
            {
                result += desEncriptarCaracter(cadena.Substring(idx, 1), cadena.Length, idx);
            }
            return result;
        }

        private static string desEncriptarCaracter(string caracter, int variable, int a_indice)
        {
            int indice;
            if (patron_encripta.IndexOf(caracter) >= 0)
            {
                if (patron_encripta.IndexOf(caracter) - variable - a_indice > 0)
                {
                    indice = (patron_encripta.IndexOf(caracter) - variable - a_indice) % patron_encripta.Length;
                }
                else
                {
                    indice = (patron_busqueda.Length) + ((patron_encripta.IndexOf(caracter) - variable - a_indice) % patron_encripta.Length);
                }
                indice = indice % patron_encripta.Length;
                return patron_busqueda.Substring(indice, 1);
            }
            else
            {
                return caracter;
            }
        }


    } // end class
} // end namespace
