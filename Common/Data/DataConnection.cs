using System;
using System.Collections.Generic;
using System.Text;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Security;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Link between a data provider and a connection string that feeds it
    /// </summary>
    public class DataConnection : CommonObject
    {
        /// <summary>
        /// The type of provider for this data connection
        /// </summary>
        public DataProviderType ProviderType { get; set; }

        /// <summary>
        /// The connection string for this connection
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// Which credentials pack to use for this connection
        /// items in the credentials pack can be transposed in to connection strings
        /// </summary>
        public Guid Credentials { get; set; }

        /// <summary>
        /// Returns the connection string formatted with any replacement text injected in to it
        /// </summary>
        /// <returns>The formatted connection string</returns>
        private String connectionStringProcessed;
        public virtual String ConnectionStringProcessed
        {
            get
            {
                // Do we have a package reference to go get the credentials etc.
                if (ParentPackage != null &&
                    Credentials != Guid.Empty &&
                    ConnectionString != String.Empty)
                {
                    // Already processed?
                    if ((connectionStringProcessed ?? String.Empty) == String.Empty)
                    {
                        // Get the credentials package
                        Credentials credentials = ParentPackage.Credentials(this.Credentials);
                        if (credentials != null)
                        {
                            // Transform the connection string
                            this.connectionStringProcessed = credentials.Transform(this.ConnectionString);
                        }
                    }

                    // Send back the processed string
                    return (connectionStringProcessed ?? String.Empty);
                }
                else
                    return (ConnectionString ?? String.Empty);
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataConnection()
        {
            Credentials = Guid.Empty; // No credentials by default
            ProviderType = DataProviderType.Unknown; // No provider type by default
            ConnectionString = String.Empty; // No connection string by default
        }
    }
}
