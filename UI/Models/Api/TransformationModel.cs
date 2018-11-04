using Newtonsoft.Json;

namespace TNDStudios.DataPortals.UI.Models.Api
{
    /// <summary>
    /// Definition of a transformation between two connections
    /// </summary>
    [JsonObject]
    public class TransformationModel : CommonObjectModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransformationModel() : base()
        {
        }
    }
}
