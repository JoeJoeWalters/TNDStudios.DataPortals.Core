using System;
using System.Collections.Generic;
using System.Data;

namespace TNDStudios.DataPortals.Helpers
{
    /// <summary>
    /// Helpers for the SQL Provider where items fall outside the 
    /// standard pattern of the data provider interface
    /// </summary>
    public partial class SQLProviderHelpers
    {
        /// <summary>
        /// Given the datarow with the data in create the referenece that can
        /// access a given table / view
        /// </summary>
        /// <param name="row">The data row containing the table data</param>
        /// <returns>A Key Value Pair with the table name as the value and the key as the fully qualified path</returns>
        public static KeyValuePair<String, String> CreateObjectReference(DataRow row)
            => new KeyValuePair<String, String>(
                $"{(row["TABLE_CATALOG"] ?? "").ToString()}.{(row["TABLE_SCHEMA"] ?? "").ToString()}.{(row["TABLE_NAME"] ?? "").ToString()}",
                row["TABLE_NAME"].ToString());

    }
}
