using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
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
        public static DataTable ToDataTable(String contentType, String json, 
            ApiDefinition apiDefinition, DataItemDefinition dataItemDefinition)
        {
            //try
            //{
            // Translate the content type
            switch (contentType.Trim().ToLower())
            {
                case "application/json":

                    JObject jObjectParsed = null;
                    JArray jArrayParsed = null;
                    try
                    {
                        jObjectParsed = JObject.Parse(json);
                    }
                    catch
                    {
                        try
                        {
                            jArrayParsed = JArray.Parse(json);
                        }
                        catch{ }
                    }

                    // Was it an object?
                    if (jObjectParsed != null)
                        return ToDataTable(jObjectParsed, apiDefinition, dataItemDefinition);
                    else if (jArrayParsed != null)
                        return ToDataTable(jArrayParsed, apiDefinition, dataItemDefinition);
                    else
                        return null; // Failed
                        
                case "application/xml":
                case "text/xml":

                    break;
            }
            /*}
            catch (Exception ex)
            {
            }*/

            // Catch all (including errors)
            return null;
        }

        /// <summary>
        /// Take some raw Json text and convert it to a datatable for the provider
        /// to handle e.g. updates or insertions
        /// </summary>
        /// <param name="json">A JObject (Queryable) representation of the json data</param>
        /// <returns>A datatable with the given format</returns>
        public static DataTable ToDataTable(JObject json, ApiDefinition apiDefinition, 
            DataItemDefinition dataItemDefinition)
        {
            // Create the table from the definition
            DataTable result = dataItemDefinition.ToDataTable(); 
            
            // Call the data conversion for the root object
            DataRow row = ToDataRow(json, apiDefinition, dataItemDefinition, result);
            if (row != null)
                result.Rows.Add(row); // Add the row to the results table

            return result;
        }

        /// <summary>
        /// Take some raw Json text and convert it to a datatable for the provider
        /// to handle e.g. updates or insertions
        /// </summary>
        /// <param name="json">A JArray (Queryable) representation of the json data</param>
        /// <returns>A datatable with the given format</returns>
        public static DataTable ToDataTable(JArray json, ApiDefinition apiDefinition,
            DataItemDefinition dataItemDefinition)
        {
            // Create the table from the definition
            DataTable result = dataItemDefinition.ToDataTable();

            // For each item in the array, call the data conversion
            foreach (JObject item in json.Children())
            {
                DataRow row = ToDataRow(item, apiDefinition, dataItemDefinition, result);
                if (row != null)
                    result.Rows.Add(row); // Add the row to the results table
            };

            return result;
        }

        /// <summary>
        /// Convert the JObject to a data row
        /// </summary>
        /// <param name="json"></param>
        public static DataRow ToDataRow(JObject json, ApiDefinition apiDefinition, 
            DataItemDefinition definition, DataTable table)
        {
            DataRow result = table.NewRow(); // Create a new row to populate

            // Loop the properties in the data definition object
            definition.ItemProperties.ForEach(property => 
            {
                // Check and see if there is an alias
                String alias = apiDefinition.Aliases.Where(a => a.Key == property.Name).FirstOrDefault().Value;

                // Check and see if the property exists in the json object
                // with the alias if one is given, but the name if one is not
                JProperty found = (json.Children<JProperty>())
                    .FirstOrDefault(prop => prop.Name == property.Name || 
                                            prop.Name == (alias ?? String.Empty));

                // Found the property?
                if (found != null)
                {
                    Object parsedValue = DataFormatHelper.ReadData(
                            found.Value.ToString(), property, definition);
                    result[property.Name] = parsedValue;
                }
            });

            // Send back the row
            return result;
        }
    }
}
