using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;

namespace TinaORM.Core
{
    public abstract class TinaConfig
    {
        /// <summary>
        /// The connection string to be used in this instance
        /// </summary>
        public abstract string ConnectionString { get; set; }

        /// <summary>
        /// The connection to the database. Should not be singelton.
        /// </summary>
        public abstract DbConnection Connection { get; }

        /// <summary>
        /// A delegate that serializes an object
        /// </summary>
        public abstract ISerializer Serializer { get; set; }

        public abstract void CreateTableIfNotExists();
    }
}
