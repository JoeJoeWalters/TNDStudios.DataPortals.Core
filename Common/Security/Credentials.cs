using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Get the value of a credential in this collection based on it's name
        /// </summary>
        /// <param name="credentialName">The name of the credential</param>
        /// <returns>The value of the credential</returns>
        public String GetValue(String credentialName)
            => Properties.Where(prop => prop.Name.ToLower() == credentialName.ToLower())
                        .Select(prop => prop.Value).FirstOrDefault();

        /// <summary>
        /// Transform the property bag provided using the template
        /// given to the method
        /// </summary>
        /// <param name="template"></param>
        /// <returns>The transformed template</returns>
        public String Transform(String template)
        {
            // Make sure it's not an empty template
            if (template != String.Empty)
            {
                // For each of the property items, do the transformation
                this.Properties.ForEach(property =>
                {
                    template = template.Replace("{{" + property.Name + "}}", property.Value);
                });
            }

            return template; // Return the transformed template
        }

        /// <summary>
        /// The default constructor
        /// </summary>
        public Credentials()
        {
            Properties = new List<Credential>(); // Default to an empty array
        }
    }
}
