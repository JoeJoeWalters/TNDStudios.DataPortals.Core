using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Controllers
{
    /// <summary>
    /// The response to an API call
    /// </summary>
    /// <typeparam name="T">The data that is being passed back</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// The data that is being sent
        /// </summary>
        [JsonProperty]
        public T Data { get; set; }

        /// <summary>
        /// Was the operation successful?
        /// </summary>
        [JsonProperty]
        public Boolean Success { get; set; }

        /// <summary>
        /// List of messages to return to the caller
        /// </summary>
        [JsonProperty]
        public List<String> Messages { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiResponse()
        {
            Success = false; // Not successful by default
            Messages = new List<String>();
        }

        /// <summary>
        /// Constructor with data (also make success as we have data)
        /// </summary>
        public ApiResponse(T data)
        {
            Data = data;
            Success = true;
            Messages = new List<String>();
        }
    }
}
