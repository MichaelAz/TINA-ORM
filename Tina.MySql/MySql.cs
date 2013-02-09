using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

using System.Web.Script.Serialization;

using TinaORM.Core;


namespace TinaORM.MySql
{
    public sealed class MySql : TinaConfig
    {
        public override string ConnectionString { get; set; }

        public override DbConnection Connection
        {
            get { return new MySqlConnection(ConnectionString); }
        }

        public override ISerializer Serializer { get; set; }

        public MySql()
        {
            ConnectionString = String.Empty;
            Serializer =  new JavascriptSerializer();
        }

        public MySql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }

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
