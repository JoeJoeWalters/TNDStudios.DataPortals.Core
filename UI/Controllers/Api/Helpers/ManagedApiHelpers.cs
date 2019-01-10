using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using TNDStudios.DataPortals.Json;

namespace TNDStudios.DataPortals.UI.Controllers.Api.Helpers
{
    /// <summary>
    /// Helpers to enable the UI / Service portion of the managed APIs
    /// (Potential for these to be moved to the "Core" namespace later
    /// </summary>
    public class ManagedApiHelpers
    {
        /// <summary>
        /// Handle the datatable injection of aliases in to the column headers
        /// </summary>
        /// <param name="data">The table to have the aliases injected into</param>
        /// <param name="aliases">The aliases to be injected</param>
        public void HandleAliases(DataTable data, List<KeyValuePair<String, String>> aliases)
        {
            // Loop the alias's for this Api and inject them
            aliases.ForEach(pair =>
            {
                // Do we have a column with the correct name
                if (data.Columns.Contains(pair.Key))
                {
                    // Get the column
                    DataColumn column = data.Columns[pair.Key];
                    if (column != null)
                        column.ExtendedProperties["Alias"] = pair.Value;
                }
            });
        }

        /// <summary>
        /// Convert a data table to the correct format for returning to the user
        /// </summary>
        /// <returns></returns>
        public JsonResult DataTableToJsonFormat(DataTable data)
        {
            // Define the way the serialisation should be done
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter>() { new DataRowConverter() }
            };

            // Send back the formatted results
            return new JsonResult(data.Rows, serializerSettings);
        }
    }
}
