using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT48
{
    public static class DB
    {
        private static DBConexion _conexionEstatica = null;
        private static bool _hayConexionEstatica = false;

        /// <summary>
        /// Establece una conexión estática que se puede usar por defecto.
        /// </summary>
        /// <param name="conexion">Conexión o null para quitar la conexión por defecto actual.</param>
        public static void setConexionEstatica(DBConexion conexion)
        {
            _conexionEstatica = conexion;
            _hayConexionEstatica = conexion != null;
        }



        public static System.Data.DataTable getDataTable(string sql, DBConexion conexion)
        {
            System.Data.DataTable t = new System.Data.DataTable();

            if (conexion.getConexionAbierta())
            {
                try
                {
                    switch (conexion.getTipo())
                    {
                        case DBConexion.Tipo.Access:
                            System.Data.OleDb.OleDbConnection c1 = (System.Data.OleDb.OleDbConnection)conexion.getConexion();
                            System.Data.OleDb.OleDbCommand comando1 = new System.Data.OleDb.OleDbCommand(sql, c1);
                            System.Data.OleDb.OleDbDataReader reader1 = comando1.ExecuteReader();
                            t.Load(reader1);
                            break;

                        case DBConexion.Tipo.SQLServer:
                            System.Data.SqlClient.SqlConnection c2 = (System.Data.SqlClient.SqlConnection)conexion.getConexion();
                            System.Data.SqlClient.SqlCommand comando2 = new System.Data.SqlClient.SqlCommand(sql, c2);
                            System.Data.SqlClient.SqlDataReader reader2 = comando2.ExecuteReader();
                            t.Load(reader2);
                            break;

                            //case DBConexion.Tipo.SQLite:
                            //    System.Data.SQLite.SQLiteConnection c3 = (System.Data.SQLite.SQLiteConnection)conexion.getConexion();
                            //    System.Data.SQLite.SQLiteCommand comando3 = new System.Data.SQLite.SQLiteCommand(sql, c3);
                            //    System.Data.SQLite.SQLiteDataReader reader3 = comando3.ExecuteReader();
                            //    t.Load(reader3);
                            //    break;
                    }
                }
                catch (Exception ex) { throw new Exception("getDataTable.Error: " + ex.Message + "\r\nEjecutando consulta: " + sql); }
            }
            else
            {
                throw new Exception("DB.ejecutaSQL.Error: La conexión no está abierta");
            }

            return t;
        }

        public static System.Data.DataTable getDataTable_CCP(string sql)
        {
            if (_hayConexionEstatica)
            {
                if (_conexionEstatica.getTipo() == DBConexion.Tipo.Access)
                    return getDataTable_CCP_Access(sql);
                else
                    return getDataTable_CCP_SQLServer(sql);
            }
            else
            {
                throw new Exception("No hay conexión estática definida");
            }
        }

        private static System.Data.DataTable getDataTable_CCP_Access(string sql)
        {
            System.Data.DataTable t = null;
            bool _oldAbierta = _conexionEstatica.getConexionAbierta();
            bool _oldRealmenteAbierta = !_oldAbierta ? false :
                _conexionEstatica.getConexionRealmenteAbierta();
            try
            {
                if (!_oldRealmenteAbierta)
                    _conexionEstatica.Abre();

                t = getDataTable(sql, _conexionEstatica);

                if (!_oldAbierta)
                    _conexionEstatica.Cierra();
            }
            catch (Exception ex) { throw new Exception("DB.getDataTable_CCP.Exception: " + ex.Message); }

            return t;
        }

        private static System.Data.DataTable getDataTable_CCP_SQLServer(string sql)
        {
            System.Data.DataTable t = null;

            DBConexion __conexion = null;
            try
            {
                __conexion = DBConexion.getACopy(_conexionEstatica);
                __conexion.Abre();

                t = getDataTable(sql, __conexion);

                __conexion.Cierra();
            }
            catch (Exception ex) { throw new Exception("DB.getDataTable_CCP.Exception: " + ex.Message); }
            finally { __conexion.Dispose(); }

            return t;
        }


        public static int ejecutaSQL(string sql, DBConexion conexion)
        {
            int n = 0;

            if (conexion.getConexionAbierta())
            {
                try
                {
                    switch (conexion.getTipo())
                    {
                        case DBConexion.Tipo.Access:
                            System.Data.OleDb.OleDbConnection c1 = (System.Data.OleDb.OleDbConnection)conexion.getConexion();
                            System.Data.OleDb.OleDbCommand comando1 = new System.Data.OleDb.OleDbCommand(sql, c1);
                            n = comando1.ExecuteNonQuery();
                            break;

                        case DBConexion.Tipo.SQLServer:
                            System.Data.SqlClient.SqlConnection c2 = (System.Data.SqlClient.SqlConnection)conexion.getConexion();
                            System.Data.SqlClient.SqlCommand comando2 = new System.Data.SqlClient.SqlCommand(sql, c2);
                            n = comando2.ExecuteNonQuery();
                            break;
                    }
                }
                catch (Exception ex) { throw new Exception("DB.ejecutaSQL.Error: " + ex.Message + "\r\nEjecutando sentencia: " + sql); }
            }
            else
            {
                throw new Exception("DB.ejecutaSQL.Error: La conexión no está abierta");
            }

            return n;
        }

        public static int ejecutaSQL_CCP(string sql)
        {
            Int32 aux;
            if (_hayConexionEstatica)
            {
                if (_conexionEstatica.getTipo() == DBConexion.Tipo.Access)
                    aux = ejecutaSQL_CCP_Access(sql);
                else
                    aux = ejecutaSQL_CCP_SQLServer(sql);
            }
            else
            {
                throw new Exception("No hay conexión estática definida");
            }

            return aux;
        }

        private static int ejecutaSQL_CCP_Access(string sql)
        {
            int n = 0;
            bool _oldAbierta = _conexionEstatica.getConexionAbierta();
            bool _oldRealmenteAbierta = !_oldAbierta ? false :
                _conexionEstatica.getConexionRealmenteAbierta();
            try
            {
                // if (!_oldAbierta) // 20200129 Sustituyo por si ha habido un problema de conexión;
                if (!_oldRealmenteAbierta)
                    _conexionEstatica.Abre();

                n = ejecutaSQL(sql, _conexionEstatica);

                if (!_oldAbierta)
                    _conexionEstatica.Cierra();
            }
            catch (Exception ex) { throw new Exception("DB.ejecutaSQL_CCP.Exception: " + ex.Message); }

            return n;
        }

        private static int ejecutaSQL_CCP_SQLServer(string sql)
        {
            int n = 0;
            DBConexion __conexion = null;
            try
            {
                __conexion = DBConexion.getACopy(_conexionEstatica);
                __conexion.Abre();

                n = ejecutaSQL(sql, __conexion);

                __conexion.Cierra();
            }
            catch (Exception ex) { throw new Exception("DB.ejecutaSQL_CCP.Exception: " + ex.Message); }
            finally { __conexion.Dispose(); }

            return n;
        }



        public static int ejecutaSQL_fromFile(string fileName, DBConexion conexion)
        {
            int resultado = 0;
            if (conexion.getTipo() == DBConexion.Tipo.SQLServer && conexion.getConexionAbierta())
            {
                System.Data.SqlClient.SqlCommand comando = new System.Data.SqlClient.SqlCommand();
                comando.Connection = (System.Data.SqlClient.SqlConnection)conexion.getConexion();
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = Files.cargaFicheroTXTToString("tablas.sql");
                try
                {
                    comando.ExecuteNonQuery();
                }
                catch (Exception executenonqueryexception)
                {
                    Log.log(executenonqueryexception.Message);
                    resultado = 0;
                }
            }
            return resultado;
        }



        #region Otras funciones

        public static Int32 nextValue_CCP(string campo, string tabla)
        {
            int n = 1;
            if (_hayConexionEstatica)
            {
                bool _oldAbierta = _conexionEstatica.getConexionAbierta();
                bool _oldRealmenteAbierta = !_oldAbierta ? false :
                    _conexionEstatica.getConexionRealmenteAbierta();
                try
                {
                    //if (!_oldAbierta) // 20200129 Cambio esto por la siguiente por si ha habido problemas de conexión y la conexión real se ha cerrado.
                    if (!_oldRealmenteAbierta)
                        _conexionEstatica.Abre();

                    n = nextValue(campo, tabla, _conexionEstatica);

                    if (!_oldAbierta)
                        _conexionEstatica.Cierra();
                }
                catch (Exception ex) { Log.log("DB.nextValue_CCP.Exception: " + ex.Message); }
            }
            else
            {
                throw new Exception("No hay conexión estática definida");
            }

            return n;
        }
        public static Int32 nextValue(string campo, string tabla, DBConexion conexion)
        {
            Int32 n = 1;
            if (conexion.getConexionAbierta())
            {
                string sql = "select max(" + campo + ") from " + tabla;
                System.Data.DataTable t;
                try { t = getDataTable(sql, conexion); }
                catch (Exception ex) { Log.log("DB.nextValue.Exception: " + ex.Message); t = null; }

                if (t != null && t.Rows.Count > 0)
                {
                    n = Conversiones.toInt(t.Rows[0].ItemArray[0]);
                    n++;
                }
            }
            else
            {
                throw new Exception("DB.nextValue.Error: La conexión no está abierta");
            }
            return n;
        }

        #endregion



        #region Funciones con repetición

        private static void delay(Int32 milliseconds)
        {
            try { System.Threading.Thread.Sleep(milliseconds); }
            catch {; }
        }

        /// <summary>
        /// Ejecuta una sentencia SQL, pudiendo llegar a realizar 3 intentos para ejecutarla si diera algún problema. Entre intento e intento aguardaría
        /// 500 ms antes de volver a intentarlo. Cuando hay fallos va montando la cadena de log con lo que se produzca. Si al final no se ha ejecutado
        /// devuelve una exception "ejecutaSQL_conReintento.Exception"
        /// </summary>
        /// <param name="sql">Comando sql que queremos ejecutar</param>
        /// <param name="baseMsgLog">La base de texto que se coloca para el mensaje de error del log</param>
        public static Int32 ejecutaSQL_conReintento(string sql, string baseMsgLog)
        {
            int resultado = 0;

            bool done = false;
            int reintentos = 3;

            while (!done && reintentos > 0)
            {
                try { resultado = DB.ejecutaSQL_CCP(sql); done = true; }
                catch (Exception ex) { Log.log("[R" + (reintentos - 4) + "] " + baseMsgLog + ex.Message); }
                reintentos--;

                if (!done && reintentos > 0)
                    delay(500);
            }

            if (!done)
                throw new Exception("ejecutaSQL_conReintento.Exception");

            return resultado;
        }

        /// <summary>
        /// Trata de obtener una DataTable a partír de una consulta SQL, pudiendo llegar a realizar 3 intentos para ejecutarla si diera algún problema.
        /// Entre intento e intento aguardaría 500 ms antes de volver a intentarlo. Cuando hay fallos va montando la cadena de log con lo que se produzca.
        /// Si al final no se ha ejecutado devuelve una exception "ejecutaSQL_conReintento.Exception"
        /// </summary>
        /// <param name="sql">Consulta SQL</param>
        /// <param name="baseMsgLog">La base de texto que se coloca para el mensaje de error del log</param>
        public static System.Data.DataTable getDataTable_conReintentos(string sql, string baseMsgLog)
        {
            System.Data.DataTable t = null; ;

            bool done = false;
            int reintentos = 3;

            while (!done && reintentos > 0)
            {
                try { t = DB.getDataTable_CCP(sql); done = true; }
                catch (Exception ex) { Log.log("[R" + (reintentos - 4) + "] " + baseMsgLog + ex.Message); }
                reintentos--;

                if (!done && reintentos > 0)
                    delay(500);
            }

            if (!done)
                throw new Exception("getDataTable_conReintentos.Exception");

            return t;
        }

        public static object getValue_conReintento(string sql, string baseMsgLog)
        {
            object r = null;

            bool done = false;
            int reintentos = 3;

            while (!done && reintentos > 0)
            {
                try
                {
                    System.Data.DataTable t = getDataTable_CCP(sql);
                    if (t.Rows.Count > 0)
                        r = t.Rows[0].ItemArray[0];

                    done = true;
                }
                catch (Exception ex) { Log.log("[R" + (reintentos - 4) + "] " + baseMsgLog + ex.Message); }

                reintentos--;

                if (!done && reintentos > 0)
                    delay(500);
            }

            if (!done)
                throw new Exception("getValue_conReintento.Exception");

            return r;
        }

        public static Int32 nextValue_conReintento(string valueName, string tableName, string baseMsgLog)
        {
            Int32 aux = 0;

            bool done = false;
            int reintentos = 3;

            while (!done && reintentos > 0)
            {
                try
                {
                    aux = nextValue_CCP(valueName, tableName);
                    done = true;
                }
                catch (Exception ex) { Log.log("[R" + (reintentos - 4) + "] " + baseMsgLog + ex.Message); }

                reintentos--;

                if (!done && reintentos > 0)
                    delay(500);
            }

            if (!done)
                throw new Exception("DB.nextValue_conReintento.Exception");

            return aux;
        }

        #endregion

    } // end class





    /// <summary>
    /// Clase para controlar la conexión a BD obviando el tipo. Incluye el tipo de BD que es, el archivo o nombre de la base de datos, la cadena de
    /// conexión y la conexión en sí (encapsulada como tipo object)
    /// </summary>
    public class DBConexion
    {
        public enum Tipo { Access, SQLServer };

        private Tipo _tipo;
        private string _fileNameOrDBName;
        private string _cadenaDeConexion;
        private object _conexion;

        private bool _conexionAbierta = false;

        private string _forCopy_user;
        private string _formCopy_password;
        private string _forCopy_catalog;

        private static bool _es64bits = false;
        private static bool _es64bitIniciado = false;

        /// <summary>
        /// Proporciona un objeto conexión pero que está cerrado. Almacena la cadena de conexión y el tipo.
        /// </summary>
        /// <param name="tipo">Tipo de base de datos</param>
        /// <param name="fileNameOrDBName">Nombre del archivo mdb o de SQLite o el nombre de la base de datos SQL Server</param>
        /// <param name="userOrNull">Para SQLServer puede indicar un usuario distinto a sa</param>
        /// <param name="passwordOrNull">Para SQLServer puede indicar una contraseña distinta a 1</param>
        /// <param name="contenidoVariable">Para SQLServer indica el catálogo, en Access indica la contraseña del archivo de la BD</param>
        public DBConexion(Tipo tipo, string fileNameOrDBServerName, string userOrNull, string passwordOrNull, string accessPswOrSQLSDBName)
        {
            dbconexionBuilder(tipo, fileNameOrDBServerName, userOrNull, passwordOrNull, accessPswOrSQLSDBName);
        }

        /// <summary>
        /// Proporciona un objeto conexión pero que está cerrado. Almacena la cadena de conexión y el tipo.
        /// </summary>
        /// <param name="tipo">Tipo de base de datos</param>
        /// <param name="fileNameOrDBName">Nombre del archivo mdb o de SQLite o el nombre de la base de datos SQL Server</param>
        public DBConexion(Tipo tipo, string fileNameOrDbServerName)
        {
            dbconexionBuilder(tipo, fileNameOrDbServerName, null, null, null);
        }

        private DBConexion()
        {
            ;
        }

        public void Dispose()
        {
            if (_conexionAbierta)
                this.Cierra();

            switch (_tipo)
            {
                case Tipo.Access:
                    ((System.Data.OleDb.OleDbConnection)_conexion).Dispose();
                    break;

                case Tipo.SQLServer:
                    ((System.Data.SqlClient.SqlConnection)_conexion).Dispose();
                    break;
            }
            GC.SuppressFinalize(_conexion);
        }

        private void dbconexionBuilder(Tipo tipo, string fileNameOrDBServerName, string userOrNull, string passwordOrNull, string accessPswOrSQLSDBName)
        {
            _tipo = tipo;
            _fileNameOrDBName = fileNameOrDBServerName;

            // Guardamos la siguiente información únicamente por si hay que hacer una copia de la instancia ya que son datos que se usan para 
            // crear la cadena de conexión
            _forCopy_user = userOrNull;
            _formCopy_password = passwordOrNull;
            _forCopy_catalog = accessPswOrSQLSDBName;

            switch (_tipo)
            {
                case Tipo.Access:
                    string _auxPsw = passwordOrNull != null ? "Password=" + passwordOrNull + ";" : "";
                    string _auxUser = userOrNull != null ? "User ID=" + userOrNull + ";" : "";
                    string _auxDBPassword = accessPswOrSQLSDBName != null ? ";Jet OLEDB:Database Password=" + accessPswOrSQLSDBName : "";
                    if (!_es64bitIniciado)
                        try
                        {
                            System.Data.OleDb.OleDbConnection conexion_p = new System.Data.OleDb.OleDbConnection();
                            conexion_p.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + _auxPsw + _auxUser + "Data Source=" + fileNameOrDBServerName + _auxDBPassword;
                            try { 
                                conexion_p.Open(); 
                                _es64bits = true; 
                            }
                            catch (Exception ex) { Log.log("LT48.DBConexion.dbconexionBuilder.Exception1: " + ex.Message); }

                            try { conexion_p.Close(); }
                            catch (Exception ex) { Log.log("LT48.DBConexion.dbconexionBuilder.Exception2: " + ex.Message); }
                        }
                        catch (Exception ex) { Log.log("LT48.DBConexion.dbconexionBuilder.Exception1+2: " + ex.Message); }
                    _es64bitIniciado = true;
                    string provider = !_es64bits ? "Microsoft.Jet.OLEDB.4.0" : "Microsoft.ACE.OLEDB.12.0";
                    _cadenaDeConexion = "Provider=" + provider + ";" + _auxPsw + _auxUser + "Data Source=" + fileNameOrDBServerName + _auxDBPassword;

                    break;

                case Tipo.SQLServer:
                    string __user = userOrNull == null ? "sa" : userOrNull;
                    string __psw = passwordOrNull == null ? "1" : passwordOrNull;
                    _cadenaDeConexion = "Password=" + __psw + ";Persist Security Info=True;User ID=" + __user + ";Initial Catalog=" + accessPswOrSQLSDBName + ";Data Source=" + fileNameOrDBServerName;
                    break;
            }
        }



        public static DBConexion getACopy(DBConexion conexionOriginal)
        {
            DBConexion nueva = new DBConexion();

            nueva._tipo = conexionOriginal._tipo;
            nueva._fileNameOrDBName = conexionOriginal._fileNameOrDBName;

            nueva._cadenaDeConexion = conexionOriginal._cadenaDeConexion;

            nueva._conexionAbierta = false;

            nueva._forCopy_user = conexionOriginal._forCopy_user;
            nueva._formCopy_password = conexionOriginal._formCopy_password;
            nueva._forCopy_catalog = conexionOriginal._forCopy_catalog;

            return nueva;
        }



        public Tipo getTipo() { return _tipo; }
        public string getCadenaDeConexion() { return _cadenaDeConexion; }
        public string getFileNameOrDBName() { return _fileNameOrDBName; }
        public object getConexion() { return _conexion; }

        public bool getConexionAbierta() { return _conexionAbierta; }
        public bool getConexionRealmenteAbierta()
        {
            bool aux = false;
            switch (_tipo)
            {
                case Tipo.Access:
                    aux = ((System.Data.OleDb.OleDbConnection)_conexion).State == System.Data.ConnectionState.Open;
                    break;

                case Tipo.SQLServer:
                    aux = ((System.Data.SqlClient.SqlConnection)_conexion).State == System.Data.ConnectionState.Open;
                    break;
            }
            return aux;
        }

        public DBConexion Copy()
        {
            DBConexion nueva = new DBConexion(_tipo, _fileNameOrDBName, _forCopy_user, _formCopy_password, _forCopy_catalog);
            return nueva;
        }

        public void Open() { Abre(); }
        public void Abre()
        {
            switch (_tipo)
            {
                case Tipo.Access:
                    _conexion = new System.Data.OleDb.OleDbConnection();
                    ((System.Data.OleDb.OleDbConnection)_conexion).ConnectionString = _cadenaDeConexion;

                    try { ((System.Data.OleDb.OleDbConnection)_conexion).Open(); _conexionAbierta = true; }
                    catch (Exception ex) { throw new Exception(ex.Message); }

                    break;

                case Tipo.SQLServer:
                    _conexion = new System.Data.SqlClient.SqlConnection();
                    ((System.Data.SqlClient.SqlConnection)_conexion).ConnectionString = _cadenaDeConexion;

                    try { ((System.Data.SqlClient.SqlConnection)_conexion).Open(); _conexionAbierta = true; }
                    catch (Exception ex) { throw new Exception(ex.Message); }

                    break;

            } // switch
        }

        public void Close() { Cierra(); }
        public void Cierra()
        {
            switch (_tipo)
            {
                case Tipo.Access:
                    try { ((System.Data.OleDb.OleDbConnection)_conexion).Close(); _conexionAbierta = false; }
                    catch (Exception ex) { throw new Exception(ex.Message); }
                    break;

                case Tipo.SQLServer:
                    try { ((System.Data.SqlClient.SqlConnection)_conexion).Close(); _conexionAbierta = false; }
                    catch (Exception ex) { throw new Exception(ex.Message); }
                    break;

            } // switch
        }

    } // end class

} // end namespace
