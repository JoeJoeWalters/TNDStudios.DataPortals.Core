using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using TNDStudios.DataPortals.Web.Json;

namespace TNDStudios.DataPortals.Api
{
    /// <summary>
    /// Helpers to enable the UI / Service portion of the managed APIs
    /// (Potential for these to be moved to the "Core" namespace later
    /// </summary>
    public static class ManagedApiHelper
    {
        /// <summary>
        /// Handle the datatable injection of aliases in to the column headers
        /// </summary>
        /// <param name="data">The table to have the aliases injected into</param>
        /// <param name="aliases">The aliases to be injected</param>
        public static void HandleAliases(DataTable data, List<KeyValuePair<String, String>> aliases)
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
        public static JsonResult ToJson(DataTable data)
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

        /// <summary>
        /// Take some raw Json text and convert it to a datatable for the provider
        /// to handle e.g. updates or insertions
        /// </summary>
        /// <param name="json"></param>
        /// <returns>A datatable with the given format</returns>
        public static DataTable ToDataTable(String contentType, String json)
        {
            try
            {
                // Translate the content type
                switch (contentType.Trim().ToLower())
                {
                    case "application/json":

                        return ToDataTable(JObject.Parse(json));

                    case "application/xml":
                    case "text/xml":

                        break;
                }
            }
            catch (Exception ex)
            {
            }

            // Catch all (including errors)
            return null;
        }

        /// <summary>
        /// Take some raw Json text and convert it to a datatable for the provider
        /// to handle e.g. updates or insertions
        /// </summary>
        /// <param name="json">A JObject (Queryable) representation of the json data</param>
        /// <returns>A datatable with the given format</returns>
        public static DataTable ToDataTable(JObject json)
        {
            // Is this an array of objects?
            //Boolean isArray = (json.First is JArray);


            return new DataTable();
        }
    }
}
