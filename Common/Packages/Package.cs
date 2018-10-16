using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;

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
    }
}
