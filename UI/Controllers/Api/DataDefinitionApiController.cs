using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    public class DataDefinitionApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataDefinitionApiController(IMapper mapper) : base()
        {
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list (or singular) data definition model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data definition models</returns>
        [HttpGet]
        [Route("/api/data/definition")]
        public ApiResponse<List<DataItemDefinitionModel>> Get()
            => Get(Guid.Empty);

        /// <summary>
        /// Delete a data definition
        /// </summary>
        /// <param name="id">The id of the data definition to delete</param>
        /// <returns>If the deletion was successful</returns>
        [HttpDelete]
        [Route("/api/data/definition/{id}")]
        public ApiResponse<Boolean> Delete(Guid id)
        {
            // Create the response object
            ApiResponse<Boolean> response = new ApiResponse<Boolean>();

            // Get the item from the repository to make sure that it is 
            // not attached to other things
            DataItemDefinition dataDefinition = SessionHandler.CurrentPackage.DataDefinition(id);
            response.Success = response.Data =
                SessionHandler.CurrentPackage.Delete<DataItemDefinition>(id);

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("/api/data/definition/{id}")]
        public ApiResponse<List<DataItemDefinitionModel>> Get(Guid id)
        {
            // Create the response object
            ApiResponse<List<DataItemDefinitionModel>> response =
                new ApiResponse<List<DataItemDefinitionModel>>();

            try
            {
                // Was an id passed in? If not just return everything
                response.Data = mapper.Map<List<DataItemDefinitionModel>>(
                    SessionHandler.CurrentPackage.DataDefinitions.Where
                        (def => (id == Guid.Empty || def.Id == id))
                    );
                
                response.Success = true;
            }
            catch(Exception ex)
            {
                response.Data.Clear(); // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }
            // Return the response object
            return response;
        }
        
        [HttpPost]
        [Route("/api/data/definition")]
        public ApiResponse<DataItemDefinitionModel> Post([FromBody] DataItemDefinitionModel request)
        {
            // Create the response object
            ApiResponse<DataItemDefinitionModel> response = new ApiResponse<DataItemDefinitionModel>();

            // Map the model to a domain object type
            DataItemDefinition savedDataItemDefinition = mapper.Map<DataItemDefinition>(request);

            // Did the mapping work ok?
            if (savedDataItemDefinition != null)
            {
                // Get the repository to save the package for us
                savedDataItemDefinition = SessionHandler.CurrentPackage
                        .Save<DataItemDefinition>(savedDataItemDefinition);

                // Saved ok?
                if (savedDataItemDefinition != null)
                {
                    // Map the data definition back to a model type and send it back to the user
                    response.Data = mapper.Map<DataItemDefinitionModel>(savedDataItemDefinition);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }

        /*
        [HttpPost]
        [Route("/api/data/definition/analyse/file")]
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
                        IDataProvider provider = new DelimitedFileProvider();
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
                                    foreach (DataItemProperty property in definition.ItemProperties)
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
        */
    }
}
