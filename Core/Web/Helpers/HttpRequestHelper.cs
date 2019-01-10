using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TNDStudios.DataPortals.Web
{
    /// <summary>
    /// Helpers to handler Http Requests and common tasks
    /// </summary>
    public static class HttpRequestHelper
    {
        /// <summary>
        /// Get the body of the request so it can be processed,
        /// also changes the incoming body stream to a reusable memorystream
        /// as the default stream only allows one hit reading, and as the 
        /// CanRead and Async processing accesses the stream twice it needs 
        /// the ability to seek and read more than once
        /// </summary>
        /// <param name="request">The input request pulled from the context</param>
        /// <returns>The content of the request body</returns>
        public static String GetBody(HttpRequest request)
        {
            String content = ""; // The return content

            // Has the body stream been replaced with a seekable memorystream?
            if (!request.Body.CanSeek)
            {
                MemoryStream stream = new MemoryStream(); // Create a new memory stream to replace the standard one
                request.Body.CopyTo(stream); // Copy the origional stream to the memory stream
                request.Body = stream; // Re-assign the stream
            }

            // Reset the stream regardless of it's type
            request.Body.Seek(0, SeekOrigin.Begin); // Reset the position

            // Read the incoming body stream
            var reader = new StreamReader(request.Body);
            content = reader.ReadToEnd(); // Get the stream content
            reader = null; // Can't have a using statement as it kills the stream too

            request.Body.Seek(0, SeekOrigin.Begin); // Reset the position
            return content; // Send the content back
        }
    }
}
