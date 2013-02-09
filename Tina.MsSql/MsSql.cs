namespace TinaORM.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    using TinaORM.Core;

    // NOTE - Tested with Microsoft SQL server 2005 and 2008.

    /// <summary>
    /// A TINA-ORM data adapter that allows Tina to conenct to Microsoft SQL Server databases
    /// </summary>
    public sealed class MsSql : TinaConfig
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
            get { return new SqlConnection(ConnectionString); }
        }

        /// <summary>
        /// Gets or sets the serializer to use for any connections with this adapter
        /// </summary>
        public override ISerializer Serializer { get; set; }

        /// <summary>
        /// Initializes a new isntance of the MsSql class, wihtout a connection string
        /// and with the default JavaScriptSerializer
        /// </summary>
        public MsSql()
        {
            ConnectionString = String.Empty;
            Serializer = new JavascriptSerializer();
        }

        /// <summary>
        /// Initializes a new isntance of the MsSql class, with a connection string
        /// and with the default JavaScriptSerializer 
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        public MsSql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }

        /// <summary>
        /// Creates a new isntance of the MsSql adapter, with a connection string
        /// and with a given data serializer
        /// </summary>
        /// <param name="connectionString">The connection string to use when making
        /// connections with this adapter</param>
        /// <param name="serializer">A serializer to use with connections made with
        /// this adapter</param>
        public MsSql(string connectionString, ISerializer serializer)
        {
            ConnectionString = connectionString;
            Serializer = serializer;
        }

        /// <summary>
        /// Creates a table for the wrappers if it doesn't exist
        /// </summary>
        public override void CreateTableIfNotExists()
        {
            if (!TableExists("Tina"))
            {
                const string CreateTable =
                    @"CREATE TABLE Tina
                                        (
                                            id INT IDENTITY(1,1) PRIMARY KEY, 
                                            Type NVARCHAR(4000), 
                                            Contents NVARCHAR(4000)
                                        );";

                using (var conn = Connection)
                {
                    conn.Open();

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = CreateTable;
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Checks whether the table exists in the curently connected
        /// database
        /// </summary>
        /// <param name="tableName">The table to check for</param>
        /// <returns>A value indicating whether the table exists or not</returns>
        private bool TableExists(string tableName)
        {
            string command =
                String.Format(
                    "select case when exists((select * from information_schema.tables where table_name = '{0}')) then 1 else 0 end",
                    tableName);

            using (var conn = Connection)
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = command;

                bool retVal = ((int) cmd.ExecuteScalar()) == 1;

                conn.Close();

                return retVal;
            }
        }
    }
}
