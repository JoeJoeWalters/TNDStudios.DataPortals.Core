using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models.Api
{

    /// <summary>
    /// A property belonging to the credentials (such as password etc.)
    /// </summary>
    public class CredentialModel : CommonObject
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
        public CredentialModel()
        {
            Value = String.Empty; // The value of the property is empty by default
            Encrypted = false; // Not encrypted by default
        }
    }
}
