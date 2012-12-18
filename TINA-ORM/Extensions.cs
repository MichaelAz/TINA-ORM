namespace TinaORM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using System.Data.SqlClient;

    /// <summary>
    /// Used to house extension methods used in the DatabaseService
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Escapes quotes on a string (Used for storing JSON in database)
        /// </summary>
        /// <param name="s">The string to escape</param>
        /// <returns>A quote escaped string</returns>
        public static string EscapeQuotes(this string s)
        {
            return s.Replace(@"""", @"""""");
        }

        /// <summary>
        /// Un escapes a strings quotes
        /// </summary>
        /// <param name="s">The string to unescape</param>
        /// <returns>An unescaped string</returns>
        public static string UnEscapeQuotes(this string s)
        {
            return s.Replace(@"""""", @"""");
        }

        /// <summary>
        /// An overload of CreateCommand that accepts a command text
        /// </summary>
        /// <param name="factory">The connection for which the command is created</param>
        /// <param name="query">The command text for the command</param>
        /// <returns>A command for the connection with command text set</returns>
        public static SqlCommand CreateCommand(this SqlConnection connection, string query)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            return command;
        }
    }
}