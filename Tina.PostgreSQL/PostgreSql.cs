namespace TinaORM.PostgreSql
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;

    using TinaORM.Core;

    using Npgsql;

    // NOTE - Tested with PostgreSQL 9.2.3

    /// <summary>
    /// A TINA-ORM data adapter that allows Tina to conenct to PostgreSQL databases
    /// </summary>
    public sealed class PostgreSql : TinaConfig
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
            get { return new NpgsqlConnection(ConnectionString); }
        }

        /// <summary>
        /// Gets or sets the serializer to use for any connections with this adapter
        /// </summary>
        public override ISerializer Serializer { get; set; }

        /// <summary>
        /// Initializes a new isntance of the PostgreSql class, wihtout a connection string
        /// and with the default JavaScriptSerializer
        /// </summary>
        public PostgreSql()
        {
            ConnectionString = String.Empty;
            Serializer =  new JavascriptSerializer();
        }

        /// <summary>
        /// Initializes a new isntance of the PostgreSql class, with a connection string
        /// and with the default JavaScriptSerializer 
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        public PostgreSql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }

        /// <summary>
        /// Creates a new isntance of the PostgreSql adapter, with a connection string
        /// and with a given data serializer
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        /// <param name="serializer">A serializer to use with connections made with
        /// this adapter</param>
        public PostgreSql(string connectionString, ISerializer serializer)
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
                                            id SERIAL UNIQUE PRIMARY KEY, 
                                            Type VARCHAR(4000), 
                                            Contents VARCHAR(4000)
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
