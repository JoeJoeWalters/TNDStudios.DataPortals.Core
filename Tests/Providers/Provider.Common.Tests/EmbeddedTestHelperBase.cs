using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TNDStudios.DataPortals.Tests
{
    /// <summary>
    /// Base class to allow streaming of resources from the context of 
    /// the current namespace context
    /// </summary>
    public class EmbeddedTestHelperBase
    {        
        /// <summary>
        /// Build a memory stream from the embedded resource to feed to the test scenarios
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A memory stream with the data contained within</returns>
        public virtual Stream GetResourceStream(String embeddedResourceName)
            => GetResourceStream(embeddedResourceName, Assembly.GetCallingAssembly());

        public virtual Stream GetResourceStream(String embeddedResourceName, Assembly assembly)
        {
            String name = FormatResourceName(assembly, embeddedResourceName);
            return assembly.GetManifestResourceStream(
                    name
                    );
        }

        /// <summary>
        /// Cast the stream of a given resource to a string to be passed to other methods
        /// </summary>
        /// <param name="embeddedResourceName">The name of the resource to read</param>
        /// <returns>A string representing the resource data</returns>
        public virtual String GetResourceString(String embeddedResourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (StreamReader reader = new StreamReader(GetResourceStream(embeddedResourceName, assembly)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Get the resource name by deriving it from the assembly
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns></returns>
        public virtual String FormatResourceName(Assembly assembly, string resourceName)
            => assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                            .Replace("\\", ".")
                                                            .Replace("/", ".");
    }
}
