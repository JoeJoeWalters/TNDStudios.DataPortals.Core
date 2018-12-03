using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals.Security
{
    public class WebAuthHelper
    {

        /// <summary>
        /// Authenticate the request made to the service
        /// </summary>
        /// <param name="request">The Http Request</param>
        /// <param name="package">The Package To Check Against</param>
        /// <param name="apiDefinition">The Api Definition found</param>
        /// <returns></returns>
        public Permissions AuthenticateRequest(HttpRequest request, Package package, ApiDefinition apiDefinition)
        {
            // Result
            Permissions result = new Permissions();

            // Did we pass in some authentication headers?
            String authenticationHeader = request.Headers.ContainsKey("Authorization") ?
                request.Headers["Authorization"].ToString() : String.Empty;

            // Did we have a header?
            if (authenticationHeader != String.Empty)
            {
                // Split the authentication parts
                String[] authenticationParts = authenticationHeader.Split(' ');

                // We should have 2 parts e.g. ("Basic dsddfsfsfdff2232")
                if (authenticationParts.Length == 2)
                {
                    // Get the authentication type
                    String authenticationType = (authenticationParts[0] ?? String.Empty).ToLower().Trim();
                    switch (authenticationType)
                    {
                        case "basic":

                            // Basic authentication, parse out the username and password
                            String details = Encoding.ASCII.GetString(
                                Convert.FromBase64String(
                                    authenticationParts[1] ?? String.Empty
                                    )
                                );

                            String[] authParts = details.Split(':');
                            if (authParts.Length == 2)
                            {
                                // Get a list of the available credential ids for the Api endpoint to check against
                                List<Guid> credentialIds = apiDefinition.CredentialsLinks
                                    .Select(link => link.Credentials).ToList();

                                // Search the available credentials to see if any contain a match
                                Credentials credentials = package.CredentialsStore.Where(cred =>
                                    credentialIds.Contains(cred.Id) &&
                                    cred.GetValue("username") == authParts[0].Trim() &&
                                    cred.GetValue("password") == authParts[1].Trim()).FirstOrDefault();

                                // Did we find a match?
                                if (credentials != null)
                                {
                                    // Get the permissions associated with these credentials from the 
                                    // links themselves
                                    result = apiDefinition
                                        .CredentialsLinks
                                        .Where(link => link.Credentials == credentials.Id)
                                        .Select(link => link.Permissions)
                                        .FirstOrDefault();
                                }
                            }

                            break;
                    }
                }
            }

            // Return the permissions of the check
            return result;
        }
    }
}
