using System;
using System.Collections.Generic;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Credentials object used for supplying connections etc. with the authentication they 
    /// need to connect
    /// </summary>
    public class CredentialsModel : CommonObjectModel
    {
        /// <summary>
        /// Property bag of credential items
        /// </summary>
        public List<CredentialModel> Properties { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public CredentialsModel()
        {
            Properties = new List<CredentialModel>(); // Default to an empty array
        }
    }
}
