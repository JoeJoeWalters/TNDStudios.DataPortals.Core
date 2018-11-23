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
        /// The pattern of the Url for the current page
        /// </summary>
        public String PageUrlPattern { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PackagePageVM()
        {
            PackageId = Guid.Empty;
            PageUrlPattern = "";
        }

        /// <summary>
        /// Create an instance of the package vm based on the routing etc.
        /// </summary>
        /// <returns></returns>
        public static PackagePageVM Create(Guid packageId)
        {
            // Get the calling method
            MethodBase method = (new StackTrace()).GetFrame(1).GetMethod();
            
            // Get the route attribute of the calling method
            RouteAttribute attr = (RouteAttribute)method.GetCustomAttributes(typeof(RouteAttribute), true)[0];
            
            // Create a new page VM to return
            return new PackagePageVM()
            {
                PackageId = packageId,
                PageUrlPattern = attr.Template.Replace("{id}", (packageId == Guid.Empty) ? "Index" : packageId.ToString())
            };
        }
    }
}
