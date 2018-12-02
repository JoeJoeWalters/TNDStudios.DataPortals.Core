using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Helpers;
using TNDStudios.DataPortals.Security;

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
        /// List of transformations for this package
        /// </summary>
        public List<Transformation> Transformations { get; set; }

        /// <summary>
        /// List of transformations for this package
        /// </summary>
        public List<Credentials> CredentialsStore { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Package() : base()
        {
            ApiDefinitions = new List<ApiDefinition>();
            DataDefinitions = new List<DataItemDefinition>();
            DataConnections = new List<DataConnection>();
            Transformations = new List<Transformation>();
            CredentialsStore = new List<Credentials>();
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
        /// Get the transformation based on the id given
        /// </summary>
        /// <param name="id">The id of the transformation</param>
        /// <returns>The transformation</returns>
        public Transformation Transformation(Guid id)
            => Transformations.Where(item => item.Id == id).FirstOrDefault();

        /// <summary>
        /// Get the Data connection based on the id given
        /// </summary>
        /// <param name="id">The id of the connection</param>
        /// <returns>The data connection</returns>
        public Credentials Credentials(Guid id)
            => CredentialsStore.Where(item => item.Id == id).FirstOrDefault();

        /// <summary>
        /// Delete an object from the package
        /// </summary>
        /// <typeparam name="T">The type of data to delete</typeparam>
        /// <param name="id">The id of the data to delete</param>
        /// <returns>If the deletion was successful</returns>
        public Boolean Delete<T>(Guid id)
        {
            // What will the result be?
            Boolean result = false;

            // Get the type of data to be saved
            String typeOfData = typeof(T).ToShortName();

            // Based on the type of data, save it to the correct repository element
            switch (typeOfData)
            {
                case "apidefinition":

                    // Get the actual value from the object wrapper
                    ApiDefinition apiDefinition = this.Api(id);

                    // If the type is not null
                    if (apiDefinition != null)
                    {
                        result = this.ApiDefinitions.Remove(apiDefinition);
                    }

                    break;

                case "dataitemdefinition":

                    // Get the actual value from the object wrapper
                    DataItemDefinition dataItemDefinition = this.DataDefinition(id);

                    // If the type is not null
                    if (dataItemDefinition != null)
                    {
                        result = this.DataDefinitions.Remove(dataItemDefinition);
                    }

                    break;

                case "dataconnection":

                    // Get the actual value from the object wrapper
                    DataConnection connection = this.DataConnection(id);

                    // If the type is not null
                    if (connection != null)
                    {
                        result = this.DataConnections.Remove(connection);
                    }

                    break;

                case "credentials":

                    // Get the actual value from the object wrapper
                    Credentials credentials = this.Credentials(id);

                    // If the type is not null
                    if (credentials != null)
                    {
                        result = this.CredentialsStore.Remove(credentials);
                    }

                    break;
            }

            // Send the result back
            return result;
        }

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
                            existingApiDefinition.LastUpdated = DateTime.Now;

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
                            existingDataItemDefinition.LastUpdated = DateTime.Now;

                            // Assign the lists
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
                            existingConnection.LastUpdated = DateTime.Now;                            
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingConnection, typeof(T));
                    }

                    break;

                case "transformation":

                    // Get the actual value from the object wrapper
                    Transformation transformation = (Transformation)Convert.ChangeType(dataToSave, typeof(Transformation));

                    // If the type is not null
                    if (transformation != null)
                    {
                        // Does this transformation already exist?
                        Transformation existingTransformation =
                            (transformation.Id == Guid.Empty) ? null : Transformation(transformation.Id);

                        // No transformation found?
                        if (existingTransformation == null)
                        {
                            // Doesn't exist currently so create a new Id
                            // and assign the object as the "existing" transformation
                            existingTransformation = transformation;
                            existingTransformation.Id = Guid.NewGuid();

                            // Add this new transformation to the repository
                            Transformations.Add(existingTransformation);
                        }
                        else
                        {
                            // Assign the values from the item to save
                            existingTransformation.Description = transformation.Description;
                            existingTransformation.Name = transformation.Name;
                            existingTransformation.LastUpdated = DateTime.Now;
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingTransformation, typeof(T));
                    }

                    break;
                    
                case "credentials":

                    // Get the actual value from the object wrapper
                    Credentials credentials = (Credentials)Convert.ChangeType(dataToSave, typeof(Credentials));

                    // If the type is not null
                    if (credentials != null)
                    {
                        // Does this set of credentials already exist?
                        Credentials existingCredentials =
                            (credentials.Id == Guid.Empty) ? null : this.Credentials(credentials.Id);

                        // Loop the properties and see if any have not been saved before. If not give them an Id
                        credentials.Properties.ForEach(credential => 
                        {
                            if (credential.Id == null)
                                credential.Id = Guid.NewGuid();
                        });

                        // No credentials found?
                        if (existingCredentials == null)
                        {
                            // Doesn't exist currently so create a new Id
                            // and assign the object as the "existing" credentials
                            existingCredentials = credentials;
                            existingCredentials.Id = Guid.NewGuid();

                            // Add this new credentials to the repository
                            CredentialsStore.Add(existingCredentials);
                        }
                        else
                        {
                            // Assign the values from the item to save
                            existingCredentials.Description = credentials.Description;
                            existingCredentials.Name = credentials.Name;
                            existingCredentials.LastUpdated = DateTime.Now;

                            // Assign the lists
                            existingCredentials.Properties = credentials.Properties;
                        }

                        // Convert the data back to the return data type (which is actually the same)
                        dataToSave = (T)Convert.ChangeType(existingCredentials, typeof(T));
                    }

                    break;
            }

            // Return the data that was saved
            return dataToSave;
        }
    }
}
