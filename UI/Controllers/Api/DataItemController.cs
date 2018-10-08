using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers
{
    [ApiController]
    public class DataItemController : ControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default constructor with DI items
        /// </summary>
        public DataItemController(IMapper mapper)
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        [HttpPost]
        [Route("/api/data/analyse/file")]
        public ApiResponse<DataItemModel> AnalyseFile(IFormFile upload)
        {
            // Create the response object
            DataItemModel result = new DataItemModel() { };

            // Anything to work with?
            if (upload != null)
            {
                // Get a stream of data out of the uploaded file
                using (MemoryStream stream = new MemoryStream())
                {
                    upload.CopyTo(stream); // Copy the data from the file to a readable stream

                    // Cast the stream to a set of content to work with
                    String content = Encoding.UTF8.GetString(stream.ToArray()) ?? "";

                    // Do we have any content to work with
                    if (content.Length != 0)
                    {
                        IDataProvider provider = new FlatFileProvider();
                        DataItemDefinition definition = provider.Analyse(new AnalyseRequest<object>() { Data = content });
                        if (definition.ItemProperties.Count != 0)
                        {
                            // Assign the definition to the result
                            result.Definition = mapper.Map<DataItemDefinitionModel>(definition);
                            
                            // Connect to the data stream now we have a definition to get the data
                            if (provider.Connect(definition, stream))
                            {
                                result.Values = new DataItemValuesModel(); // Create a new values model

                                // Read the data from the connection to the stream
                                DataTable data = provider.Read("");

                                // Did we get some rows back?
                                foreach (DataRow row in data.Rows)
                                {
                                    // Create a new blank data line to cast the data to
                                    Dictionary<String, String> line = new Dictionary<string, string>();

                                    // Loop the headers to get the values
                                    foreach(DataItemProperty property in definition.ItemProperties)
                                    {
                                        // Cast the data as appropriate and add it to the line
                                        line[property.Name] = 
                                            DataFormatHelper.WriteData(
                                                row[property.Name], 
                                                property, 
                                                definition);
                                    }

                                    // Add the line to the result values
                                    result.Values.Lines.Add(line);
                                }
                            }
                        }
                    }
                }
            }

            // Respond with the analysed data
            return new ApiResponse<DataItemModel>() { Data = result, Success = true };
        }
    }
}