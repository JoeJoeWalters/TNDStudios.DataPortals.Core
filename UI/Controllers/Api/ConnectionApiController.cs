using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.DataPortals.Helpers;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;
using TNDStudios.DataPortals.UI.Models.RequestResponse;
using System.Data;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.UI.Controllers.Api.Helpers;

namespace TNDStudios.DataPortals.UI.Controllers.Api
{
    [ApiController]
    [Route("/api/package/{packageId}/data")]
    public class ConnectionApiController : ApiControllerBase
    {
        /// <summary>
        /// Automapper set by the dependency injection to the constructor
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Reference to the helpers object
        /// </summary>
        private ConnectionApiHelpers helpers;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConnectionApiController(IMapper mapper) : base()
        {
            this.helpers = new ConnectionApiHelpers(); // Create an instance of the helpers class
            this.mapper = mapper; // Assign the mapper from the dependency injection
        }

        /// <summary>
        /// Get a list of the provider types available
        /// </summary>
        /// <returns>The list of available provider types</returns>
        [HttpGet]
        [Route("providers")]
        public ApiResponse<List<KeyValuePair<Int32, String>>> GetProviderTypes()
            => new ApiResponse<List<KeyValuePair<Int32, String>>>()
            {
                Data = typeof(DataProviderType).ToList().Where(item => (item.Key != 0)).ToList(),
                Success = true
            };

        /// <summary>
        /// Get a list (or singular) data connection model 
        /// based on a set of criteria
        /// </summary>
        /// <returns>An API response with a list of data connection models</returns>
        [HttpGet]
        [Route("connection")]
        public ApiResponse<List<CommonObjectModel>> Get([FromRoute]Guid packageId)
        {
            // Create the response object
            ApiResponse<List<CommonObjectModel>> response = new ApiResponse<List<CommonObjectModel>>();

            try
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<List<CommonObjectModel>>(package.DataConnections);
                }

                // Success as we got here
                response.Success = true;
            }
            catch
            {
                response.Data.Clear(); // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }

            // Return the response object
            return response;
        }

        /// <summary>
        /// Test the connection to a data source
        /// </summary>
        /// <param name="request">The data connection model to test</param>
        /// <returns>A success or failure state for the connection</returns>
        [HttpPost]
        [Route("connection/test")]
        public ApiResponse<Boolean> Test([FromRoute]Guid packageId, [FromBody] DataConnectionModel request)
        {
            // Create a default response object
            ApiResponse<Boolean> response = new ApiResponse<Boolean>();

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Map the connection to something we can use
                DataConnection connection = mapper.Map<DataConnection>(request);

                // Create a new factory class and get the provider from the connection given
                IDataProvider provider = (new DataProviderFactory()).Get(package, connection, false);
                if (provider != null)
                {
                    // Can we connect?
                    response.Success = response.Success = provider.Test(connection);
                    provider = null; // Kill the provider now
                }
            }

            // Send the response back
            return response;
        }

        /// <summary>
        /// Query the objects associated with a given provider type and the 
        /// connection details
        /// </summary>
        /// <param name="request">The data connection model to query the objects</param>
        /// <returns>A list of objects associated with this connection / provider</returns>
        [HttpPost]
        [Route("connection/queryobjects")]
        public ApiResponse<List<KeyValuePair<String, String>>> QueryObjects([FromRoute]Guid packageId, [FromBody] DataConnectionModel request)
        {
            // Create a default response object
            ApiResponse<List<KeyValuePair<String, String>>> response =
                new ApiResponse<List<KeyValuePair<String, String>>>()
                {
                    Data = new List<KeyValuePair<String, String>>()
                };

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Map the connection to something we can use
                DataConnection connection = mapper.Map<DataConnection>(request);

                // Create a new factory class and get the provider from the connection given
                IDataProvider provider = (new DataProviderFactory()).Get(package, connection, false);
                if (provider != null)
                {
                    response.Data = provider.ObjectList();
                    response.Success = response.Success = true;
                    provider = null; // Kill the provider now
                }
            }

            // Send the response back
            return response;
        }

        [HttpDelete]
        [Route("connection/{id}")]
        public ApiResponse<Boolean> Delete([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<Boolean> response = new ApiResponse<Boolean>();

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Get the item from the repository to make sure that it is 
                // not attached to other things
                DataConnection connection = package.DataConnection(id);
                response.Success = response.Data = package.Delete<Transformation>(id);
            }

            // Return the response
            return response;
        }

        [HttpGet]
        [Route("connection/{id}")]
        public ApiResponse<DataConnectionModel> Get([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<DataConnectionModel> response = new ApiResponse<DataConnectionModel>();

            try
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Was an id passed in? If not just return everything
                    response.Data = mapper.Map<DataConnectionModel>(
                        package.DataConnections.Where
                            (def => (id == Guid.Empty || def.Id == id)).FirstOrDefault()
                        );

                    // Fill in the blanks
                    response.Data = helpers.PopulateModel(mapper, package, response.Data);
                }

                // Success as we got here
                response.Success = true;
            }
            catch
            {
                response.Data = null; // Clear the data as we don't want to send back partial data
                response.Success = false; // Failed due to hard failure
            }

            // Return the response object
            return response;
        }

        [HttpPost]
        [Route("connection")]
        public ApiResponse<DataConnectionModel> Post([FromRoute]Guid packageId, [FromBody] DataConnectionModel request)
        {
            // Create the response object
            ApiResponse<DataConnectionModel> response = new ApiResponse<DataConnectionModel>();

            // Map the model to a domain object type
            DataConnection savedConnection = mapper.Map<DataConnection>(request);

            // Did the mapping work ok?
            if (savedConnection != null)
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the repository to save the package for us
                    savedConnection = package.Save<DataConnection>(savedConnection);
                }

                // Saved ok?
                if (savedConnection != null)
                {
                    // Map the connection back to a model type and send it back to the user
                    response.Data = mapper.Map<DataConnectionModel>(savedConnection);

                    // Fill in the blanks
                    response.Data = helpers.PopulateModel(mapper, package, response.Data);
                }

                // Nothing died .. Success
                response.Success = true;
            }

            // Send the response back
            return response;
        }

        /// <summary>
        /// Analyse a connection object to give back the result as a model 
        /// for the UI to consume
        /// </summary>
        /// <param name="connection">The connection to analyse</param>
        /// <param name="includeSample">Include sample data with the analysis</param>
        /// <returns>The data item model for the UI to consume</returns>
        private DataItemModel AnalyseConnection([FromRoute]Guid packageId, DataConnection connection)
        {
            DataItemModel result = new DataItemModel(); // The result to return

            // Valid connection?
            if (connection != null)
            {
                // Did we find a package?
                Package package = SessionHandler.PackageRepository.Get(packageId);
                if (package != null)
                {
                    // Get the appropriate provider object to analyse
                    IDataProvider provider = (new DataProviderFactory()).Get(package, connection, false);

                    // Get the results of the analysis of this connection
                    DataItemDefinition definition =
                        provider.Analyse(new AnalyseRequest<object>()
                        {
                            Data = null,
                            Connection = connection
                        });

                    // Did we get a result back?
                    if (definition.ItemProperties.Count != 0)
                    {
                        // Assign the definition to the result
                        result.Definition = mapper.Map<DataItemDefinitionModel>(definition);
                    }
                }
            }

            return result; // Send the result back
        }

        [HttpGet]
        [Route("connection/{id}/analyse")]
        public ApiResponse<DataItemModel> Analyse([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Create the response object
            ApiResponse<DataItemModel> result = new ApiResponse<DataItemModel>() { Success = false };

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Get the connection from the current package by the id given
                DataConnection connection = package.DataConnection(id);

                // Get the analysis result
                result.Data = AnalyseConnection(packageId, connection);

                // Send the resulting analysis back
                result.Success = (result.Data != null && result.Data.Definition != null);
            }

            return result;
        }

        /// <summary>
        /// Query the connection with a data definition
        /// </summary>
        /// <param name="request">The connection request</param>
        /// <returns>The result of the query</returns>
        [HttpPost]
        [Route("connection/{id}/sample")]
        public ApiResponse<DataItemModel> Sample([FromRoute]Guid packageId, [FromRoute]Guid id, [FromBody]DataItemDefinitionModel request)
        {
            // Create the response object
            ApiResponse<DataItemModel> result = new ApiResponse<DataItemModel>() { Success = false };

            // Did we find a package?
            Package package = SessionHandler.PackageRepository.Get(packageId);
            if (package != null)
            {
                // Get the connection from the current package by the id given
                DataConnection connection = package.DataConnection(id);

                // Get the definition from the model provided
                if (request != null && request.IsValid)
                {
                    DataItemDefinition dataDefinition = mapper.Map<DataItemDefinition>(request);

                    // Get the sample result 
                    result.Data = SampleConnection(package, connection, dataDefinition, 10);

                    // Send the resulting analysis back
                    result.Success = (result.Data != null && result.Data.Values != null);
                }
                else
                    result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// Sample the data from a connection object to give back the result as a model 
        /// for the UI to consume
        /// </summary>
        /// <param name="connection">The connection to analyse</param>
        /// <returns>The data item model for the UI to consume</returns>
        private DataItemModel SampleConnection(Package package, DataConnection connection, DataItemDefinition definition, Int32 sampleSize)
        {
            DataItemModel result = new DataItemModel(); // The result to return

            // Valid connection?
            if (connection != null)
            {
                // Get the appropriate provider object to analyse
                IDataProvider provider = (new DataProviderFactory()).Get(package, connection, definition, false);
                if (provider.Connect(definition, connection))
                {
                    result.Values = new DataItemValuesModel(); // Create a new values model

                    // Read the data from the connection to the stream
                    DataTable data = provider.Read("");

                    // Did we get some rows back?
                    Int32 rowId = 0;
                    while (rowId < sampleSize && rowId < data.Rows.Count)
                    {
                        // Get the next row
                        DataRow row = data.Rows[rowId];
                        if (row != null)
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

                        // Move to the next row
                        rowId++;
                    }
                }
            }

            return result; // Send the result back
        }

    }
}