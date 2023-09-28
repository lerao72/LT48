using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT48
{
    public static class Conversiones
    {
        public static bool esInt(object numero)
        {
            bool aux = false;
            try
            {
                int i = Convert.ToInt32(numero);
                aux = true;
            }
            catch {; }
            return aux;
        }

        public static bool esDouble(object numero)
        {
            bool aux = false;
            try
            {
                double i = Convert.ToDouble(numero);
                aux = true;
            }
            catch {; }
            return aux;
        }

        public static bool esFechaUniversal(object fecha)
        {
            if (fecha.ToString().Length != 8)
            {
                return false;
            }
            else
            {
                bool aux = false;
                try
                {
                    int year = Convert.ToInt32(fecha.ToString().Substring(0, 4));
                    int mes = Convert.ToInt32(fecha.ToString().Substring(4, 2));
                    int dia = Convert.ToInt32(fecha.ToString().Substring(6, 2));
                    DateTime dt = new DateTime(year, mes, dia);
                    aux = true;
                }
                catch {; }
                return aux;
            }
        }

        public static bool esFechaUniversalConHora(object fechaHora)
        {
            if (fechaHora.ToString().Length != 14)
            {
                return false;
            }
            else
            {
                bool aux = false;
                try
                {
                    //yyyyMMddHHmmss
                    //01234567890123
                    int year = Convert.ToInt32(fechaHora.ToString().Substring(0, 4));
                    int mes = Convert.ToInt32(fechaHora.ToString().Substring(4, 2));
                    int dia = Convert.ToInt32(fechaHora.ToString().Substring(6, 2));

                    int hora = Convert.ToInt32(fechaHora.ToString().Substring(8, 2));
                    int minuto = Convert.ToInt32(fechaHora.ToString().Substring(10, 2));
                    int segundo = Convert.ToInt32(fechaHora.ToString().Substring(12, 2));

                    DateTime dt = new DateTime(year, mes, dia, hora, minuto, segundo);
                    aux = true;
                }
                catch {; }
                return aux;
            }
        }



        #region Conversiones fecha

        public static DateTime fechaUniversal_to_DateTime(object fecha, object hora, bool orNow)
        {
            // Devuelve una cadena con formato YYYYMMDD en un datetime DD/MM/YYYY
            string _fecha = fecha.ToString();
            string _hora = hora.ToString();

            if (_fecha.ToString().Length != 8)
            {
                if (orNow)
                {
                    _fecha = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    throw new Exception("Formato de fecha universal incorrecto (tamaño incorrecto)");
                }
            }

            if (_hora.Length != 0 && _hora.Length != 6)
            {
                if (orNow)
                {
                    _hora = DateTime.Now.ToString("HHmmss");
                }
                else
                {
                    throw new Exception("Formato de hora universal incorrecto (tamaño incorrecto)");
                }
            }

            DateTime aux;
            while (_hora.Length < 6) _hora += "0";

            try
            {
                int year = Convert.ToInt32(_fecha.ToString().Substring(0, 4));
                int mes = Convert.ToInt32(_fecha.ToString().Substring(4, 2));
                int dia = Convert.ToInt32(_fecha.ToString().Substring(6, 2));
                int horaInt = Convert.ToInt32(_hora.ToString().Substring(0, 2));
                int minuto = Convert.ToInt32(_hora.ToString().Substring(2, 2));
                int segundo = Convert.ToInt32(_hora.ToString().Substring(4, 2));

                aux = new DateTime(year, mes, dia, horaInt, minuto, segundo);
            }
            catch { throw new Exception("Formato de fecha universal incorrecto"); }

            return aux;
        }

        public static DateTime fechaUniversal_to_DateTime(object fecha, object hora)
        {
            return fechaUniversal_to_DateTime(fecha, hora, false);
        }

        private static DateTime fechaUniversal_to_DateTimeOrNow(object fecha)
        {
            return fechaUniversal_to_DateTime(fecha, "", true);
        }

        public static DateTime fechaUniversal_to_DateTime(object fecha)
        {
            return fechaUniversal_to_DateTime(fecha, "", false);
        }



        /// <summary>
        /// Convierte un objeto (string) con formato dd/MM/yyyy en Datetime. Si no, devuelve la fecha actual
        /// </summary>
        /// <param name="fecha">Cadena a convertir</param>
        /// <returns></returns>
        public static DateTime fechaESP_to_DateTime(object fecha)
        {
            string f = fecha.ToString();
            if (f.Length == 10)
            {
                // dd/mm/yyyy
                // 0123456789
                Int32 dia = Conversiones.stringToInt(f.Substring(0, 2));
                Int32 mes = Conversiones.stringToInt(f.Substring(3, 2));
                Int32 anio = Conversiones.stringToInt(f.Substring(6, 4));
                return new DateTime(anio, mes, dia);
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Devuelve un string con formato dd/MM/yyyy.
        /// Si hay error devuelve ??/??/????
        /// </summary>
        /// <param name="fecha">string con formato yyyyMMdd</param>
        /// <returns></returns>
        public static string fechaUniversal_to_fechaESP(object fecha)
        {
            if (esFechaUniversal(fecha))
            {
                string f = fecha.ToString();
                return f.Substring(6, 2) + "/" + f.Substring(4, 2) + "/" + f.Substring(0, 4);
            }
            else
            {
                return "??/??/????";
            }
        }



        /// <summary>
        /// Devuelve un Datetime o string en un string de fecha universal yyyyMMdd.
        /// En caso de error devuelve la fecha actual en formato yyyyMMdd
        /// Entradas válidas: DateTime, 'dd/MM/yyyy' o 'yyyyMMdd'
        /// </summary>
        /// <param name="dateTimeOrString">Puede ser un Datetime o un string con formato dd/MM/yyyy o yyyyMMdd</param>
        /// <returns></returns>
        public static string toFechaUniversal(object dateTimeOrString)
        {
            string aux;
            if (dateTimeOrString.GetType() == typeof(DateTime))
            {
                try { aux = ((DateTime)dateTimeOrString).ToString("yyyyMMdd"); }
                catch { aux = DateTime.Now.ToString("yyyyMMdd"); }
            }
            else
            {
                aux = dateTimeOrString.ToString();
                if (aux.Length == 10)
                {
                    aux = fechaESP_to_DateTime(aux).ToString("yyyyMMdd");
                }
                else if (aux.Length != 8)
                {
                    aux = DateTime.Now.ToString("yyyyMMdd");
                }
            }
            return aux;
        }

        /// <summary>
        /// Dado un objeto, lo convierte a DateTime
        /// </summary>
        /// <param name="fecha">Puede ser un DateTime o un string de fecha en formato universal (yyyyMMdd) o una cadena
        /// con la fecha en formato españo (dd/MM/yyyy)</param>
        /// <returns></returns>
        public static DateTime toDateTime(object fecha)
        {
            DateTime f = DateTime.Now;
            if (fecha.GetType() == typeof(DateTime))
            {
                f = (DateTime)fecha;
            }
            else if (fecha.GetType() == typeof(string))
            {
                Int32 longitudDeCadena = fecha.ToString().Length;
                if (longitudDeCadena == 8)
                {
                    f = fechaUniversal_to_DateTime(fecha);
                }
                else if (longitudDeCadena == 10)
                {
                    f = fechaESP_to_DateTime(fecha);
                }
            }
            return f;
        }



        /// <summary>
        /// Devuelve el día de la semana en español
        /// </summary>
        /// <param name="fecha">Un datetieme o fecha universal</param>
        /// <returns></returns>
        public static string getDiaDeLaSemana(object fecha)
        {
            string[] ds = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            DateTime f;
            if (fecha.GetType() == typeof(DateTime))
            {
                f = (DateTime)fecha;
            }
            else if (fecha.GetType() == typeof(string) && fecha.ToString().Length == 8)
            {
                f = fechaUniversal_to_DateTimeOrNow(fecha);
            }
            else
            {
                f = DateTime.Now;
            }
            try { return ds[(Int32)f.DayOfWeek]; }
            catch { return "ND"; }
        }



        #region Conversiones de fecha y hora

        public static DateTime fechaHoraUniversal_to_DateTime(string fechaHoraUniversal)
        {
            DateTime f = DateTime.Now;
            try { f = fechaUniversal_to_DateTime(fechaHoraUniversal.Substring(0, 8), fechaHoraUniversal.Substring(8)); }
            catch {; }
            return f;
        }

        /// <summary>
        /// Convierte un datetime, o una cadena de caracteres tipo dd/MM/yyyy HH:mm:ss o yyyyMMddHHmmss en una fecha
        /// en formato universalyyyyMMddHHmmss
        /// </summary>
        /// <param name="dateTimeHoraOrString"></param>
        /// <returns></returns>
        public static string toFechaHoraUniversal(object dateTimeHoraOrString)
        {
            string aux;
            if (dateTimeHoraOrString.GetType() == typeof(DateTime))
            {
                try { aux = ((DateTime)dateTimeHoraOrString).ToString("yyyyMMddHHmmss"); }
                catch { aux = DateTime.Now.ToString("yyyyMMddHHmmss"); }
            }
            else
            {
                aux = dateTimeHoraOrString.ToString();
                if (aux.IndexOf("/") >= 0)
                {
                    string[] separadores = { "/", ":", " " };
                    string[] partes = aux.Split(separadores, StringSplitOptions.RemoveEmptyEntries);
                    Int32[] pa = { DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, 0, 0, 0 };

                    for (Int32 i = 0; i < Math.Min(partes.Length, pa.Length); i++)
                    {
                        pa[i] = Conversiones.toInt(partes[i]);
                    }

                    aux = new DateTime(pa[2], pa[1], pa[0], pa[3], pa[4], pa[5]).ToString("yyyyMMddHHmmss");
                }
                else
                {
                    if (aux.Length > 14 || aux.Length < 8)
                    {
                        aux = DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        while (aux.Length < 14) aux += "0";
                    }
                }
            }
            return aux;
        }

        public static string horaUniversal_to_StrHora(object hora)
        {
            try
            {
                string s = hora.ToString();
                if (s.Length == 6)
                {
                    return s.Substring(0, 2) + ":" + s.Substring(2, 2) + ":" + s.Substring(4, 2);
                }
                else
                {
                    return "??:??:??";
                }
            }
            catch { return "??:??:??"; }
        }

        /// <summary>
        /// Convierte una cadena del tipo yyyyMMddHHmmss en dd/MM/yyyy HH:mm:ss
        /// </summary>
        /// <param name="fechaHora">Fecha hora en formato universal (yyyyMMddHHmmss)</param>
        /// <returns></returns>
        public static string fechaHoraUniversal_to_FechaESPyHora(object fechaHora)
        {
            string _fs;
            try
            {
                string _fechaHora = fechaHora.ToString();
                _fs = Conversiones.fechaUniversal_to_fechaESP(_fechaHora.Substring(0, 8)) + " " +
                    Conversiones.horaUniversal_to_StrHora(_fechaHora.Substring(8, 6));
            }
            catch
            {
                _fs = "??/??/???? ??:??:??";
            }
            return _fs;
        }

        /// <summary>
        ///  Devuelve un datetime. Si hora tiene el formato hhmmss con fecha 1/1/2000. si no tiene el formato correcto devuelve la hora actual
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static DateTime horaUniversal_to_DateTime(string hora)
        {
            if (hora.Length == 6)
            {
                Int32 h = Conversiones.stringToInt(hora.Substring(0, 2));
                Int32 m = Conversiones.stringToInt(hora.Substring(2, 2));
                Int32 s = Conversiones.stringToInt(hora.Substring(4, 2));
                return new DateTime(2000, 1, 1, h, m, s);
            }
            else return DateTime.Now;
        }

        #endregion



        #endregion



        #region Conversiones de enteros

        /// <summary>
        /// Convierte una cadena de caractéres en un número entero. Si hubiera fallo devuelve 0
        /// </summary>
        /// <param name="numero">Cadena de texto con el número a convertir</param>
        /// <returns>Si es correcto (Int32)numero, si no 0</returns>
        public static Int32 stringToInt(string numero)
        {
            Int32 aux;
            try
            {
                aux = Convert.ToInt32(numero.Replace(" ", ""));
            }
            catch (Exception ex)
            {
                aux = 0;
            }
            return aux;
        }

        /// <summary>
        /// Convierte una cadena de caractéres en un número entero. Si hubiera fallo devuelve -1
        /// </summary>
        /// <param name="numero">Cadena de texto con el número a convertir</param>
        /// <returns>Si es correcto (Int32)numero, si no -1</returns>
        public static Int32 stringToIntWN(string numero)
        {
            /* A diferencia de stringToInt, stringToIntWN (string to int con nulo) devuelve -1 si no se puede hacer la conversión */
            Int32 aux;
            try
            {
                aux = Convert.ToInt32(numero.Replace(" ", ""));
            }
            catch
            {
                aux = -1;
            }
            return aux;
        }

        /// <summary>
        /// Devuelve el valor de numero o 0
        /// </summary>
        /// <returns></returns>
        public static Int32 toInt(object numero)
        {
            Int32 aux;
            try { aux = Convert.ToInt32(numero); }
            catch { aux = Conversiones.stringToInt(numero.ToString()); }
            return aux;
        }

        /// <summary>
        /// Devuelve el valor de numero o -1
        /// </summary>
        /// <returns></returns>
        public static Int32 toIntWN(object numero)
        {
            Int32 aux;
            try { aux = Convert.ToInt32(numero); }
            catch { aux = stringToIntWN(numero.ToString()); }
            return aux;
        }

        #endregion



        #region Conversiones con reales

        /// <summary>
        /// Coje una cadena de caractéres y trata de convertirla en double. Si da error devuelve 0.0
        /// Admite a la entrada una cadena con separadores de miles y separador de decimales:
        /// 1.000,0 devuelve 1000.0
        /// 1,000.0 devuelve 1000.0
        /// 1.000   devuelve 1.0
        /// 1,000   devuelve 1.0
        /// 1.0     devuelve 1.0
        /// 1,0     devuelve 1.0
        /// </summary>
        /// <param name="valor">string a convertir</param>
        /// <returns></returns>
        public static double stringToDouble(string valor)
        {
            string valorUsar = valor.Replace(" ", "");

            Int32 longitud = valorUsar.Length;
            Int32 valorSinPuntos = valorUsar.Replace(".", "").Length;
            Int32 valorSinComas = valorUsar.Replace(",", "").Length;

            /*
             * Valor            l   sp  sc
             * 1.000.000,00     12  10  11
             * 1.0              3   2   3
             * 1,0              3   3   2
             * 1.000,00         8   7   7         
             */

            bool hayPuntosYcomas = longitud != valorSinPuntos && longitud != valorSinComas;

            // Si hay puntos y comas, hemos de quitar el que esté colocado como separador de miles (será el primero)
            if (hayPuntosYcomas)
            {
                Int32 posPunto = valorUsar.IndexOf(".");
                Int32 posComa = valorUsar.IndexOf(",");
                if (posPunto < posComa)
                {
                    // Se usa el punto como separador de miles. Quitamos los puntos
                    valorUsar = valorUsar.Replace(".", "");
                }
                else
                {
                    // Se usa la coma como separador de miles. Quitamos las comas
                    valorUsar = valorUsar.Replace(",", "");
                }
            }

            string comaDecimal = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            string punto; // el separador decimal real
            string desde; // el separador decimal que buscaremos para sustituir
            if (comaDecimal == ",")
            {
                punto = ",";
                desde = ".";
            }
            else
            {
                punto = ".";
                desde = ",";
            }

            double resultado = 0.0;
            try
            {
                resultado = Convert.ToDouble(valorUsar.Replace(desde, punto));
            }
            catch {; }

            return resultado;
        }

        /// <summary>
        /// Devuelve valor como número real. Es igual a stringToInt(valor.toString())
        /// </summary>
        /// <returns></returns>
        public static double toDouble(object valor)
        {
            double aux;
            try { aux = stringToDouble(valor.ToString()); }
            catch { aux = 0.0; }
            return aux;
        }

        #endregion



        #region Conversiones con booleanos

        /// <summary>
        /// Devuelve true si snValue es S. En otro caso (incluido que sea valor nulo) devuelve false
        /// </summary>
        /// <returns></returns>
        public static bool stringSN_ToBool(object snValue)
        {
            if (snValue == null)
            {
                return false;
            }
            else
            {
                return snValue.ToString().ToUpper() == "S";
            }
        }

        /// <summary>
        /// Devuelve true si bValue es TRUE o "s"
        /// </summary>
        /// <param name="bValue">Puede ser un bool o un string</param>
        /// <returns></returns>
        public static bool objectToBool(object bValue)
        {
            bool aux;
            try
            {
                if (bValue.GetType() == typeof(string))
                {
                    aux = ((string)bValue).ToUpper() == "S";
                }
                else if (bValue.GetType() == typeof(bool))
                {
                    aux = (bool)bValue;
                }
                else
                {
                    aux = false;
                }
            }
            catch { aux = false; }
            return aux;
        }

        /// <summary>
        /// Devuelve la cadena "s" o "n" dependiendo del valor
        /// </summary>
        public static string boolToSNString(bool valor) { return valor ? "s" : "n"; }


        #endregion



        #region Varios con cadenas de texto

        /// <summary>
        /// Devuelve una cadena de longitud menor o igual a la especificada
        /// </summary>
        public static string getStringWidhtLenghtLessThan(string cadena, int longitud)
        {
            return cadena.Length <= longitud ? cadena : cadena.Substring(0, longitud);
        }

        /// <summary>
        /// Devuelve una cadena con la longitud especificada.
        /// 
        /// Si la cadena es mayor, trunca a izquierdas o a derechas según se especifique.
        /// 
        /// Si por el contrario, la cadena es menor, rellena con caracterRelleno (a derechas o a izquierdas
        /// según se haya especificado el parámetro a izquierdas) hasta llegar a la longitud especificada.
        /// </summary>
        public static string getStringWidthLength(string cadena, int longitud, string caracterRelleno, bool aIzquierdas)
        {
            string nuevaCadena;
            if (cadena.Length > longitud)
            {
                nuevaCadena = aIzquierdas ?
                    cadena.Substring(0, longitud) :
                    cadena.Substring(cadena.Length - longitud);
            }
            else
            {
                nuevaCadena = cadena;

                if (aIzquierdas)
                    while (nuevaCadena.Length < longitud)
                        nuevaCadena = caracterRelleno + nuevaCadena;
                else
                    while (nuevaCadena.Length < longitud)
                        nuevaCadena += caracterRelleno;
            }

            return nuevaCadena;
        }

        #endregion



        #region Conversiones entre rows de DataGridView y vectores de tipo object
        // Para incluir estas dos funciones hemos tenido que añadir la referencia de ensamblado System.Windows.Forms

        public static object[] DataGridViewRowToObjectArray(System.Windows.Forms.DataGridViewRow dgr)
        {
            object[] o = new object[dgr.Cells.Count];
            for (int i = 0; i < dgr.Cells.Count; i++)
                try { o[i] = dgr.Cells[i].Value; }
                catch {; }

            return o;
        }

        public static void ObjectArrayToDataGridViewRow(object[] o, System.Windows.Forms.DataGridViewRow dgr)
        {
            for (Int32 i = 0; i < o.Length; i++)
                try { dgr.Cells[i].Value = o[i]; }
                catch {; }
        }

        #endregion



        /// <summary>
        /// Devuelve el valor de objeto como un string. Si objeto no es convertible devuelve una cadena vacia ("")
        /// </summary>
        /// <param name="objeto">Objeto para convertir</param>
        /// <returns></returns>
        public static string toString(object objeto)
        {
            string aux = "";
            try
            {
                aux = objeto.ToString();
            }
            catch (Exception ex)
            {
                ;
            }
            return aux;
        }

        public static string quitarComillas(string cadena)
        {
            return cadena.Replace("'", "´").Replace("\"", "¨");
        }

        public static int HexToInt(string valor)
        {
            int resultado = -1;
            try
            {
                resultado = Convert.ToInt32(valor, 16);
            }
            catch (Exception comunStringToIntException)
            {
                ;
            }
            return resultado;
        }

        public static string IntToHex(int valor)
        {
            return valor.ToString("X");
        }

    } // end class
} // end namespace
