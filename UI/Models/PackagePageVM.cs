using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TNDStudios.DataPortals.UI.Models
{
    /// <summary>
    /// Class used to defined what is needed for a view that relies on a package
    /// </summary>
    public class PackagePageVM
    {
        /// <summary>
        /// The Id for the current package
        /// </summary>
        public Guid PackageId { get; set; }

        /// <summary>
        /// The Id for the current item being shown (or loaded)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The pattern of the Url for the current page
        /// </summary>
        public String PageUrlPattern { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PackagePageVM()
        {
            PackageId = Guid.Empty;
            Id = Guid.Empty;
            PageUrlPattern = "";
        }

        /// <summary>
        /// Create an instance of the package vm based on the routing etc.
        /// </summary>
        /// <returns></returns>
        public static PackagePageVM Create(Guid packageId, Guid id)
        {
            // Get the calling method
            MethodBase method = (new StackTrace()).GetFrame(1).GetMethod();
            String template = "";

            // Did we find a calling method?
            if (method != null)
            {
                // Get the route attribute of the calling class type
                if (method.ReflectedType != null)
                {
                    // Get the custom attributes of the calling class
                    object[] typeAttributes = method.ReflectedType.GetCustomAttributes(typeof(RouteAttribute), true);

                    // If we have any attributes then assign the template of the route to the template string
                    if (typeAttributes.Count() != 0)
                        template = ((RouteAttribute)typeAttributes[0]).Template;
                }

                // Get the route attribute of the calling method
                object[] attributes = method.GetCustomAttributes(typeof(RouteAttribute), true);
                if (attributes.Count() > 0)
                {
                    // There was a route prefix, add a seperator
                    if (template.Length > 0)
                        template = $"{template}/";

                    // Assign the rest of the route
                    template += ((RouteAttribute)attributes[0]).Template;
                }
            }

            // Create a new page VM to return
            return new PackagePageVM()
            {
                PackageId = packageId,
                Id = id,
                PageUrlPattern = template
                /*.Replace("{packageId}", packageId.ToString())
                .Replace("{id}", id.ToString())*/
            };
        }
    }
}
