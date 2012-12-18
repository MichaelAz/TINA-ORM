namespace TinaORM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Data;
    using System.Data.Sql;
    using System.Data.SqlClient;

    using System.Web;
    using System.Web.Script;
    using System.Web.Script.Serialization;

    /// <summary>
    /// TINA-ORM (This Is Not An ORM) is a NoSQL-ish interface
    ///  to Microsoft SQL server inspired by RavenDB.
    /// </summary>
    public class Tina
    {
        /// <summary>
        /// The connection string to be used in this instance
        /// </summary>
        private readonly string ConnectionString;

        /// <summary>
        /// Houses a mapping between the curently selected 
        /// wrappers and their deserialized contents
        /// </summary>
        private Dictionary<Wrapper, object> CurentlySelected;

        /// <summary>
        /// The serializer to be used when working with JSON
        /// </summary>
        private readonly JavaScriptSerializer Serializer;

        public Tina(string connectionString)
        {
            CurentlySelected = new Dictionary<Wrapper, object>();
            Serializer = new JavaScriptSerializer();

            ConnectionString = connectionString;

            CreateTableIfNotExists();
        }

        // CRUD

        /// <summary>
        /// Creates a new entry in the database representing the entity passed in
        /// </summary>
        /// <param name="entity">A POCO that is to be persisted in the database</param>        
        public void Store(object entity)
        {
            var Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            string insertQuery = String.Format(
                "INSERT INTO Tina VALUES ('{0}','{1}')",
                entity.GetType().AssemblyQualifiedName,
                Serializer.Serialize(entity));

            insertQuery = insertQuery.EscapeQuotes();

            var command = Connection.CreateCommand(insertQuery);

            command.ExecuteNonQuery();
            
            Connection.Close();
        }

        /// <summary>
        /// Querys the table representing persisted instances of type T for all instances
        /// </summary>
        /// <typeparam name="T">Represents the object type queryd for</typeparam>
        /// <returns>All instances of T persisted in the database</returns>
        public IEnumerable<T> Query<T>() where T : class
        {
            var selectedType = typeof (T);

            string selectQuery = String.Format(
                "SELECT * FROM Tina WHERE Type ='{0}'",
                selectedType.AssemblyQualifiedName);

            var selected = new List<Wrapper>();
            using (var Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                var command = Connection.CreateCommand(selectQuery);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    selected.Add(new Wrapper
                    {
                        Id = (int)reader["Id"],
                        Type = selectedType,
                        Contents = reader["Contents"]
                    });
                }

                reader.Close(); 
                Connection.Close();
            } 

            var deSerialized = from element in selected
                               select Serializer.Deserialize<T>(((string)element.Contents).UnEscapeQuotes());

            deSerialized = deSerialized.ToList();

            for (int i = 0; i < selected.Count; i++)
            {
                CurentlySelected.Add(selected.ElementAt(i), deSerialized.ElementAt(i));
            }
            
            return deSerialized;
        }

        /// <summary>
        /// Saves the changes preformed on curently selected objects (UOW pattern)
        /// </summary>
        public void SaveChanges()
        {
            using (var Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                var command = Connection.CreateCommand();

                foreach (var pair in CurentlySelected)
                {
                    pair.Key.Contents = Serializer.Serialize(pair.Value);

                    string updateCommand = String.Format(
                        "UPDATE Tina SET Contents  = '{0}' WHERE Id = '{1}'",
                        ((string)pair.Key.Contents).EscapeQuotes(),
                        pair.Key.Id);

                    command.CommandText = updateCommand;

                    command.ExecuteNonQuery();
                }

                Connection.Close();
            }

            CurentlySelected = new Dictionary<Wrapper, object>();
        }

        /// <summary>
        /// Deletes the entity passed in from the database
        /// </summary>
        /// <param name="entity">The entity to be deleted</param>
        public void Delete(object entity)
        {
            Wrapper wrapper = CurentlySelected.Where(x => ReferenceEquals(x.Value, entity))
                                                     .ElementAt(0).Key;

            string deleteCommand = String.Format("DELETE FROM Tina WHERE Id = '{0}'", wrapper.Id);

            using (var Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                var command = Connection.CreateCommand(deleteCommand);

                command.ExecuteNonQuery();

                Connection.Close();
            }
            
            CurentlySelected.Remove(wrapper);
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

            using (var Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                var cmd = Connection.CreateCommand(command);

                bool retVal = ((int) cmd.ExecuteScalar()) == 1;

                Connection.Close();

                return retVal;
            }
        }

        /// <summary>
        /// Creates a table for the wrappers if it doesn't exist
        /// </summary>
        private void CreateTableIfNotExists()
        {
            if (!TableExists("Tina"))
            {
                const string createTable = @"CREATE TABLE Tina
                                        (
                                            id INT IDENTITY(1,1) PRIMARY KEY, 
                                            Type NVARCHAR(4000), 
                                            Contents NVARCHAR(4000)
                                        );";

                using (var Connection = new SqlConnection(ConnectionString))
                {
                    Connection.Open();

                    var cmd = Connection.CreateCommand();
                    cmd.CommandText = createTable;
                    cmd.ExecuteNonQuery();

                    Connection.Close();
                }
            }
        }
    }
}