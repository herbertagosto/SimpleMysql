using HCulture;
using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMysql
{
    /// <summary>
    /// Simplify MySQL implementation
    /// </summary>
    public class Mysql : IDisposable
    {
        private MySqlConnection conn;
        private Culture culture;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server name. Sample "127.0.0.1"</param>
        /// <param name="database">Database name</param>
        /// <param name="username">User name</param>
        /// <param name="userpassword">Password</param>
        public Mysql(string server, string database, string username, string userpassword)
        {
            culture = new Culture();

            string connStr = string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", server, database, username, userpassword);
            conn = new MySqlConnection(connStr);
            Open();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string. Sample ""</param>
        public Mysql(string connectionString)
        {
            culture = new Culture();
            conn = new MySqlConnection(connectionString);
            Open();
        }

        #region PUBLIC METHODS
        /// <summary>
        /// Query method. Returns DataTable
        /// </summary>
        /// <param name="query">Sample "SELECT * FROM database.table"</param>
        /// <returns></returns>
        public DataTable Query(string query)
        {
            DataTable ret = new DataTable();
            if (conn.State == ConnectionState.Open)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                new MySqlCommandBuilder(adapter);
                adapter.Fill(ret);
            }
            return ret;
        }

        /// <summary>
        /// Query method. Write to file
        /// </summary>
        /// <param name="query">Sample "SELECT * FROM database.table"</param>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public void Query(string query, string path)
        {
            if (conn.State == ConnectionState.Open)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    var cmd = new MySqlCommand(query, conn);
                    cmd.CommandTimeout = 99999;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] items = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            items[i] = reader.GetString(i);
                        }
                        file.WriteLine(string.Join("|", items));
                    }
                }
            }
        }

        /// <summary>
        /// Non Query method. Use for INSERT,etc. Returns Integer
        /// </summary>
        /// <param name="query">Sample "INSERT INTO table VALUES (value1,value2,value3,...);"</param>
        /// <returns></returns>
        public int ExecNonQuery(string query)
        {
            if (conn.State == ConnectionState.Open)
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }
            return 0;
        }
        #endregion

        #region PRIVATE METHODS
        private void Open()
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        throw new ArgumentException("Cannot connect to server. Contact administrator");
                    case 1045:
                        throw new ArgumentException("Invalid username or password, please try again");
                    default:
                        throw new ArgumentException(ex.Message);
                }
            }
        }
        private void Close()
        {
            conn.Close();
        }
        #endregion





        #region DISPOSE
        /// <summary>
        /// Dispose when class instance was distruct
        /// </summary>
        ~Mysql()
        {
            Dispose();
        }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        /// <summary>
        /// Dispose class
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose classe
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                Close();
                culture.Dispose();
            }

            disposed = true;
        }
        #endregion
    }
}
