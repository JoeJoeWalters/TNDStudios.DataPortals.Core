using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
