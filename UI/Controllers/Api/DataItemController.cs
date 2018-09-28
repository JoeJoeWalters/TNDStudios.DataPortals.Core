using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public ApiResponse<DataItemModel> AnalyseFile()
        {
            // Create the response object
            DataItemModel result = new DataItemModel() { };



            // Respond with the analysed data
            return new ApiResponse<DataItemModel>() { Data = result, Success = true };
        }
    }
}