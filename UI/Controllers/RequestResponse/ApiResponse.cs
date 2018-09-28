﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Controllers.RequestResponse
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
        public T Data { get; set; }

        /// <summary>
        /// Was the operation successful?
        /// </summary>
        public Boolean Success { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApiResponse()
        {
            Success = false; // Not successful by default
        }

        /// <summary>
        /// Constructor with data (also make success as we have data)
        /// </summary>
        public ApiResponse(T data)
        {
            Data = data;
            Success = true;
        }
    }
}