using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI.Controllers.Api.Helpers
{
    /// <summary>
    /// Helper functions for the connection Api, no public items
    /// are allowed in the Api itself so located here for testability
    /// </summary>
    public class ConnectionApiHelper
    {
        /// <summary>
        /// Makes sure that any additional items that cannot be automapped
        /// are attached to the model to be returned
        /// </summary>
        /// <param name="model">The model to be populated</param>
        /// <returns>The populated model</returns>
        public static DataConnectionModel PopulateModel(IMapper mapper, Package package, DataConnectionModel model)
        {
            // Post processing to fill in the missing titles
            // as this doesn't really fit well in Automapper due 
            // to the source column type
            model.Credentials =
                        mapper.Map<KeyValuePair<Guid, String>>
                            (package.Credentials(model.Credentials.Key));

            // Add the provider data which can't be automapped
            // As it connects to an enum
            model.ProviderData =
                mapper.Map<DataProviderModel>(
                    (new DataProviderFactory())
                        .Get((DataProviderType)model.ProviderType)
                    );

            return model; // Send the model back (it's byref anyway but ..)
        }
    }
}
