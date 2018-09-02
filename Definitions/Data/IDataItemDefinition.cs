using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    public interface IDataItemDefinition
    {
        IEnumerable<IDataItemProperty> Columns { get; set; }
    }
}
