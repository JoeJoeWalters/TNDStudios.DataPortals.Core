using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Repositories;

namespace TNDStudios.DataPortals.Security
{
    /// <summary>
    /// Hanlde web authentications
    /// </summary>
    public static class WebAuthHelper
    {
        /// <summary>
        /// Authenticate an incoming request before farming off the result to the 
        /// calling method
        /// </summary>
        /// <param name="packageId">The identifier of the package to be checked</param>
        /// <param name="objectType">The "real" name of the api endpoint</param>
        /// <param name="packageRepository">The package repository to load the definitions from</param>
        /// <param name="request">The Http Request to determine the verbs etc.</param>
        /// <returns>A package that contains the status of the result but also the associated 
        /// items needed to process the request</returns>        
        public static ApiAuthenticationResult AuthenticateApiRequest(Guid packageId,
            String objectType, IPackageRepository packageRepository,
            HttpRequest request)
        {
            // Create a result object (default Http status etc.)
            ApiAuthenticationResult result = new ApiAuthenticationResult();

            try
            {
                // Get the package from the repository
                result.Package = packageRepository.Get(packageId);
                if (result.Package != null)
                {
                    // Get the definition for this object type
                    result.ApiDefinition = result.Package.Api(objectType);
                    if (result.ApiDefinition != null)
                    {
                        // Authenticate this request against the Api Definition
                        result.Permissions = AuthenticateRequest(request, result.Package, result.ApiDefinition);
                        if (result.Permissions == null)
                        {
                            result.StatusCode = HttpStatusCode.Unauthorized;
                            result.StatusDescription = "Could not obtain permissions for the request";
                        }
                        else
                        {
                            // Check permissions here
                            switch (request.Method.Trim().ToUpper())
                            {
                                case "GET":
                                    result.StatusCode = result.Permissions.CanRead ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
                                    break;

                                case "POST":
                                    result.StatusCode = result.Permissions.CanCreate ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
                                    break;

                                case "PATCH":
                                    result.StatusCode = result.Permissions.CanUpdate ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
                                    break;

                                case "DELETE":
                                    result.StatusCode = result.Permissions.CanDelete ? HttpStatusCode.OK : HttpStatusCode.Unauthorized;
                                    break;

                                default:
                                    result.StatusCode = HttpStatusCode.Unauthorized; // Not a recognised verb so deny it
                                    break;
                            }

                            // Not authorised? Give the reason why 
                            result.StatusDescription = result.StatusCode == HttpStatusCode.OK ? String.Empty : "Unauthorized to access this resource with the given verb.";

                            // Go get the data definition if all is ok
                            if (result.StatusCode == HttpStatusCode.OK)
                                result.DataDefinition = result.Package.DataDefinition(result.ApiDefinition.DataDefinition);
                        }
                    }
                    else
                    {
                        result.StatusCode = HttpStatusCode.ServiceUnavailable;
                        result.StatusDescription = $"The associated Api definition for '{objectType}' endpoint could not be found.";
                    }
                }
                else
                {
                    result.StatusCode = HttpStatusCode.ServiceUnavailable;
                    result.StatusDescription = $"There was not package loaded to search for an endpoint of type '{objectType}'";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.StatusDescription = $"Could not perform operation due to '{ex.Message}'";
            }

            // Send the result of the authentication process back with any 
            // required objects attached
            return result;
        }
        /// <summary>
        /// Authenticate the request made to the service
        /// </summary>
        /// <param name="request">The Http Request</param>
        /// <param name="package">The Package To Check Against</param>
        /// <param name="apiDefinition">The Api Definition found</param>
        /// <returns></returns>
        public static Permissions AuthenticateRequest(HttpRequest request, Package package, ApiDefinition apiDefinition)
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
