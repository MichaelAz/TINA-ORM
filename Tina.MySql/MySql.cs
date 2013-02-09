using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

using TinaORM.Core;


namespace TinaORM.MySql
{
    // NOTE - Tested with MySQL 5.5

    /// <summary>
    /// A TINA-ORM data adapter that allows Tina to conenct to MySQL databases
    /// </summary>
    public sealed class MySql : TinaConfig
    {
        /// <summary>
        /// Gets or sets the connection string for a connection with this adapter
        /// </summary>
        public override string ConnectionString { get; set; }

        /// <summary>
        /// Gets a connection that uses the connection string supplied to this adapter
        /// </summary>
        public override DbConnection Connection
        {
            get { return new MySqlConnection(ConnectionString); }
        }

        /// <summary>
        /// Gets or sets the serializer to use for any connections with this adapter
        /// </summary>
        public override ISerializer Serializer { get; set; }

        /// <summary>
        /// Initializes a new isntance of the MySql class, wihtout a connection string
        /// and with the default JavaScriptSerializer
        /// </summary>
        public MySql()
        {
            ConnectionString = String.Empty;
            Serializer =  new JavascriptSerializer();
        }

        /// <summary>
        /// Initializes a new isntance of the MySql class, with a connection string
        /// and with the default JavaScriptSerializer 
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        public MySql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }

        /// <summary>
        /// Creates a new isntance of the MySql adapter, with a connection string
        /// and with a given data serializer
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        /// <param name="serializer">A serializer to use with connections made with
        /// this adapter</param>
        public MySql(string connectionString, ISerializer serializer)
        {
            ConnectionString = connectionString;
            Serializer = serializer;
        }

        /// <summary>
        /// Creates a table for the wrappers if it doesn't exist
        /// </summary>
        public override void CreateTableIfNotExists()
        {
            const string createTable =
                @"CREATE TABLE IF NOT EXISTS Tina
                                        (
                                            id INT AUTO_INCREMENT UNIQUE PRIMARY KEY, 
                                            Type VARCHAR(4000) CHARSET utf8, 
                                            Contents VARCHAR(4000) CHARSET utf8
                                        );";

            using (var conn = Connection)
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = createTable;
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
