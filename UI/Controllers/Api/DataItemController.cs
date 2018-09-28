using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Controllers.RequestResponse;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    public class DataItemController : ControllerBase
    {
        [HttpPost]
        [Route("/api/data/analyse/file")]
        public ApiResponse<DataItemModel> AnalyseFile([FromForm] IFormFile Upload)
        {
            // Create the response object
            DataItemModel result = new DataItemModel() { };

            // Get a stream of data out of the uploaded file
            using (MemoryStream stream = new MemoryStream())
            {
                Upload.CopyTo(stream);
                Byte[] fileContent = stream.ToArray();

            }

            // Respond with the analysed data
            return new ApiResponse<DataItemModel>() { Data = result, Success = true };
        }
    }
}