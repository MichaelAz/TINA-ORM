namespace TinaORM
{
    using System;

    /// <summary>
    /// A wrapper for an object to persist to a database
    /// </summary>
    class Wrapper
    {
        /// <summary>
        /// Gets or sets the ID of the object wrapper
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the object within the wrapper
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the object contained within the wrapper
        /// </summary>
        public object Contents { get; set; }
    }
}