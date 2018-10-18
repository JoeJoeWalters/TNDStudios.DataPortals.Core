using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;

namespace TNDStudios.DataPortals.Repositories
{
    /// <summary>
    /// Collection file format for saving or storing the 
    /// definitions for a "Data Portals File"
    /// </summary>
    public class Package : CommonObject
    {
        /// <summary>
        /// List of API definitions for this package
        /// </summary>
        public List<ApiDefinition> ApiDefinitions { get; set; }

        /// <summary>
        /// List of data definitions for this package
        /// </summary>
        public List<DataItemDefinition> DataDefinitions { get; set; }

        /// <summary>
        /// List of data connections for this package
        /// (Essentially provider to connection string)
        /// </summary>
        public List<DataConnection> DataConnections { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Package() : base()
        {
            ApiDefinitions = new List<ApiDefinition>();
            DataDefinitions = new List<DataItemDefinition>();
            DataConnections = new List<DataConnection>();
        }

        /// <summary>
        /// Get the Api Definition based on the Id given
        /// </summary>
        /// <param name="id">The Id of the definition</param>
        /// <returns>The Api Definition</returns>
        public ApiDefinition Api(Guid id)
            => ApiDefinitions.Where(item => item.Id == id).FirstOrDefault();

        /// <summary>
        /// Get the Api Definition based on the name given
        /// </summary>
        /// <param name="name">The name of the definition</param>
        /// <returns>The Api Definition</returns>
        public ApiDefinition Api(String name)
            => ApiDefinitions.Where(item => item.Name == name).FirstOrDefault();

        /// <summary>
        /// Get the Data definition based on the id given
        /// </summary>
        /// <param name="id">The id of the definition</param>
        /// <returns>The data definition</returns>
        public DataItemDefinition DataDefinition(Guid id)
            => DataDefinitions.Where(item => item.Id == id).FirstOrDefault();

        /// <summary>
        /// Get the Data connection based on the id given
        /// </summary>
        /// <param name="id">The id of the connection</param>
        /// <returns>The data connection</returns>
        public DataConnection DataConnection(Guid id)
            => DataConnections.Where(item => item.Id == id).FirstOrDefault();

        /// <summary>
        /// Save a piece of information to the appropriate package element
        /// </summary>
        /// <typeparam name="T">The type of data to be saved</typeparam>
        /// <param name="dataToSave">The data to be saved</param>
        /// <returns>The saved data</returns>
        public T Save<T>(T dataToSave) where T : CommonObject
        {
            // Get the type of data to be saved
            String typeOfData = typeof(T).ToShortName();

            // Based on the type of data, save it to the correct repository element
            switch (typeOfData)
            {
                case "apidefinition":

                    // Get the actual value from the object wrapper
                    ApiDefinition apiDefinition = (ApiDefinition)Convert.ChangeType(dataToSave, typeof(ApiDefinition));

                    // If the type is not null
                    if (apiDefinition != null)
                    {
                        // Does this api definition already exist?
                        ApiDefinition existingApiDefinition =
                            (apiDefinition.Id == Guid.Empty) ? null : this.Api(apiDefinition.Id);

                        // No API Definition found?
                        if (existingApiDefinition == null)
                        {
                            // Doesn't exist currently so create a new Id
                            // and assign the object as the "existing" api definition
                            existingApiDefinition = apiDefinition;
                            existingApiDefinition.Id = Guid.NewGuid();

                            // Add this new api definition to the repository
                            ApiDefinitions.Add(existingApiDefinition);
                        }
                        else
                        {
                            // Assign the values from the item to save
                            existingApiDefinition.Description = apiDefinition.Description;
                            existingApiDefinition.Name = apiDefinition.Name;

                            // Assign the foreign keys
                            existingApiDefinition.DataConnection = apiDefinition.DataConnection;
                            existingApiDefinition.DataDefinition = apiDefinition.DataDefinition;
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingApiDefinition, typeof(T));
                    }

                    break;

                case "dataitemdefinition":
                    
                    // Get the actual value from the object wrapper
                    DataItemDefinition dataItemDefinition = (DataItemDefinition)Convert.ChangeType(dataToSave, typeof(DataItemDefinition));

                    // If the type is not null
                    if (dataItemDefinition != null)
                    {
                        // Does this data definition already exist?
                        DataItemDefinition existingDataItemDefinition =
                            (dataItemDefinition.Id == Guid.Empty) ? null : this.DataDefinition(dataItemDefinition.Id);

                        // No data definition found?
                        if (existingDataItemDefinition == null)
                        {
                            // Doesn't exist currently so create a new Id
                            // and assign the object as the "existing" data definition
                            existingDataItemDefinition = dataItemDefinition;
                            existingDataItemDefinition.Id = Guid.NewGuid();

                            // Add this new data definition to the repository
                            DataDefinitions.Add(existingDataItemDefinition);
                        }
                        else
                        {
                            // Assign the values from the item to save
                            existingDataItemDefinition.Description = dataItemDefinition.Description;
                            existingDataItemDefinition.Name = dataItemDefinition.Name;
                            existingDataItemDefinition.Culture = dataItemDefinition.Culture;
                            existingDataItemDefinition.EncodingFormat = dataItemDefinition.EncodingFormat;

                            // Assign the lists
                            existingDataItemDefinition.Connections = dataItemDefinition.Connections;
                            existingDataItemDefinition.ItemProperties = dataItemDefinition.ItemProperties;
                            existingDataItemDefinition.PropertyBag = dataItemDefinition.PropertyBag;
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingDataItemDefinition, typeof(T));
                    }

                    break;

                case "dataconnection":

                    // Get the actual value from the object wrapper
                    DataConnection connection = (DataConnection)Convert.ChangeType(dataToSave, typeof(DataConnection));

                    // If the type is not null
                    if (connection != null)
                    {
                        // Does this connection already exist?
                        DataConnection existingConnection =
                            (connection.Id == Guid.Empty) ? null : DataConnection(connection.Id);

                        // No connection found?
                        if (existingConnection == null)
                        {
                            // Doesn't exist currently so create a new Id
                            // and assign the object as the "existing" connection
                            existingConnection = connection;
                            existingConnection.Id = Guid.NewGuid();

                            // Add this new connection to the repository
                            DataConnections.Add(existingConnection);
                        }
                        else
                        {
                            // Assign the values from the item to save
                            existingConnection.ConnectionString = connection.ConnectionString;
                            existingConnection.Description = connection.Description;
                            existingConnection.Name = connection.Name;
                            existingConnection.ProviderType = connection.ProviderType;

                            // Assign the definitions
                            existingConnection.Definitions = connection.Definitions;
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingConnection, typeof(T));
                    }

                    break;
            }

            // Return the data that was saved
            return dataToSave;
        }
    }
}
