using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Web.Script.Serialization;

using TinaORM.Core;


namespace TinaORM.MsSql
{
    public sealed class MsSql : TinaConfig
    {
        public override string ConnectionString { get; set; }

        public override DbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }

        public override ISerializer Serializer { get; set; }

        public MsSql()
        {
            ConnectionString = String.Empty;
            Serializer =  new JavascriptSerializer();
        }

        public MsSql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }

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
                const string createTable =
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
                    cmd.CommandText = createTable;
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
