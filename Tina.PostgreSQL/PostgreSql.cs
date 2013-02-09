using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

using TinaORM.Core;

using Npgsql;

namespace TinaORM.PostgreSql
{
    public sealed class PostgreSql : TinaConfig
    {
        public override string ConnectionString { get; set; }

        public override DbConnection Connection
        {
            get { return new NpgsqlConnection(ConnectionString); }
        }

        public override ISerializer Serializer { get; set; }

        public PostgreSql()
        {
            ConnectionString = String.Empty;
            Serializer =  new JavascriptSerializer();
        }

        public PostgreSql(string connectionString)
        {
            ConnectionString = connectionString;
            Serializer = new JavascriptSerializer();
        }


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
