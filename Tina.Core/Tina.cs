using System.Data.Common;

namespace TinaORM.Core
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
        private readonly TinaConfig config;

        /// <summary>
        /// Houses a mapping between the curently selected 
        /// wrappers and their deserialized contents
        /// </summary>
        private Dictionary<Wrapper, object> CurentlySelected;

        public Tina(TinaConfig config)
        {
            CurentlySelected = new Dictionary<Wrapper, object>();

            this.config = config;
            config.CreateTableIfNotExists();
        }

        #region Fluent
        
        public static Tina ConnectsTo<T>(string connectionString) where T:TinaConfig, new()
        {
            TinaConfig config = new T();
            config.ConnectionString = connectionString;

            return new Tina(config);
        }

        public static Tina ConnectsTo<T>(string connectionString, ISerializer serializer) where T : TinaConfig, new()
        {
            TinaConfig config = new T();
            config.ConnectionString = connectionString;
            config.Serializer = serializer;

            return new Tina(config);
        }

        #endregion 

        // CRUD

        /// <summary>
        /// Creates a new entry in the database representing the entity passed in
        /// </summary>
        /// <param name="entity">A POCO that is to be persisted in the database</param>        
        public void Store(object entity)
        {
            using (var connection = config.Connection)
            {
                string insertQuery = "INSERT INTO Tina (Type, Contents) VALUES (@Type, @JSON)";

                var insertCommand = connection.CreateCommand(insertQuery);
                insertCommand.Parameters.Add(insertCommand.CreateParameter(
                    DbType.String, 
                    "@Type",
                    entity.GetType().AssemblyQualifiedName));
                insertCommand.Parameters.Add(insertCommand.CreateParameter(
                    DbType.String,
                    "@JSON",
                    config.Serializer.Serialize(entity)));

                connection.Open();
                insertCommand.ExecuteNonQuery();
                connection.Close(); 
            }
        }

        /// <summary>
        /// Querys the table representing persisted instances of type T for all instances
        /// </summary>
        /// <typeparam name="T">Represents the object type queryd for</typeparam>
        /// <returns>All instances of T persisted in the database</returns>
        public IEnumerable<T> Query<T>() where T : class
        {
            var selectedType = typeof (T);
            var selected = new List<Wrapper>();

            using (var connection = config.Connection)
            {
                // Note: Replace * with column names
                string selectQuery = "SELECT * FROM Tina WHERE Type=@Type";

                var selectCommand = connection.CreateCommand(selectQuery);
                selectCommand.Parameters.Add(selectCommand.CreateParameter(
                    DbType.String, 
                    "@Type", 
                    selectedType.AssemblyQualifiedName));

                connection.Open();

                using (var reader = selectCommand.ExecuteReader())
                {
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
                }

                connection.Close(); 
            }

            var deSerialized = from element in selected
                               select config.Serializer.Deserialize<T>(((string)element.Contents).UnEscapeQuotes());

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
            using (var Connection = config.Connection)
            {
                string updateQuery = "UPDATE Tina SET Contents  = @JSON WHERE Id = @Id";

                var updateCommand = Connection.CreateCommand(updateQuery);
                updateCommand.Parameters.Add(updateCommand.CreateParameter(
                    DbType.String,
                    "@JSON",
                    null));

                updateCommand.Parameters.Add(updateCommand.CreateParameter(
                    DbType.Int32,
                    "@Id",
                    null));

                Connection.Open();

                foreach (var pair in CurentlySelected)
                {
                    // Update the stored JSON to the curent object state
                    updateCommand.Parameters["@JSON"].Value = config.Serializer.Serialize(pair.Value);
                    updateCommand.Parameters["@Id"].Value = pair.Key.Id;
                    updateCommand.ExecuteNonQuery();
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
            // Get they wrapper that coresponds to the curent entity
            Wrapper wrapper = CurentlySelected.Where(x => ReferenceEquals(x.Value, entity))
                                                     .ElementAt(0).Key;

            using (var Connection = config.Connection)
            {
                Connection.Open();

                string deleteQuery = "DELETE FROM Tina WHERE Id = @Id";

                var deleteCommand = Connection.CreateCommand(deleteQuery);
                deleteCommand.Parameters.Add(deleteCommand.CreateParameter(
                    DbType.Int32, 
                    "@Id", 
                    wrapper.Id));
                deleteCommand.ExecuteNonQuery();

                Connection.Close();
            }
            
            CurentlySelected.Remove(wrapper);
        }
    }
}