using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        {
            // Valid row with at least the table name?
            if (row != null &&
                row.Table.Columns.Contains("TABLE_NAME") &&
                (row["TABLE_NAME"].ToString() ?? String.Empty) != String.Empty)
            {
                List<String> items = new List<String>();

                // Process the possible items 
                items.Add(row.Table.Columns.Contains("TABLE_CATALOG") ?
                    (row["TABLE_CATALOG"].ToString() ?? String.Empty) :
                    String.Empty);

                items.Add(row.Table.Columns.Contains("TABLE_SCHEMA") ?
                    (row["TABLE_SCHEMA"].ToString() ?? String.Empty) :
                    String.Empty);

                items.Add(row["TABLE_NAME"].ToString() ?? String.Empty);
                
                return new KeyValuePair<String, String>(
                    items[items.Count - 1],
                    String.Join('.', items.Where(x => x != String.Empty).ToArray())
                    );
            }
            else
                return new KeyValuePair<String, String>(String.Empty, String.Empty);
        }
    }
}
