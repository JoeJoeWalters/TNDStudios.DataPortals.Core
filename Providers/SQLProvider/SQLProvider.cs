using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TNDStudios.DataPortals.PropertyBag;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// Implementation of a provider that connects to a SQL Source
    /// </summary>
    public class SQLProvider : DataProviderBase, IDataProvider
    {
        /// <summary>
        /// This provider can list objects that can be connected to
        /// </summary>
        public override Boolean CanList => true;

        /// <summary>
        /// The property bag types that can be used to define this connection
        /// </summary>
        public override List<PropertyBagItemType> PropertyBagTypes() =>
            new List<PropertyBagItemType>()
            {
                new PropertyBagItemType(){ DataType = typeof(Int32), DefaultValue = 0, PropertyType = PropertyBagItemTypeEnum.RowsToSkip }
            };

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SQLProvider() : base()
        {
        }
    }
}
