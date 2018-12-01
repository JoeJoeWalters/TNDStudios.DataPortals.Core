using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TNDStudios.DataPortals.Security
{
    /// <summary>
    /// Credentials object used for supplying connections etc. with the authentication they 
    /// need to connect
    /// </summary>
    public class Credentials : CommonObject 
    {
        /// <summary>
        /// Property bag of credential items
        /// </summary>
        public List<CredentialItem> Properties { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Credentials()
        {
            Properties = new List<CredentialItem>(); // Default to an empty array
        }
    }

    /// <summary>
    /// A property belonging to the credentials (such as password etc.)
    /// </summary>
    public class CredentialItem : CommonObject
    {
        /// <summary>
        /// The value of the credential item
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// If the value is to be encrypted or viewable
        /// </summary>
        public Boolean Encrypted { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CredentialItem()
        {
            Value = String.Empty; // The value of the property is empty by default
            Encrypted = false; // Not encrypted by default
        }
    }
}
