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
        public List<Credential> Properties { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Credentials()
        {
            Properties = new List<Credential>(); // Default to an empty array
        }
    }
}
