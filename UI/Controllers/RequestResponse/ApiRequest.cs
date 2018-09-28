using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Controllers.RequestResponse
{
    /// <summary>
    /// The holder for requests from the web / system
    /// </summary>
    /// <typeparam name="T">The data that is being requested</typeparam>
    [JsonObject]
    public class ApiRequest<T>
    {
        /// <summary>
        /// The data that is being requested (or the request structure)
        /// </summary>
        [JsonProperty]
        public T Data { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiRequest()
        {

        }
    }
}
