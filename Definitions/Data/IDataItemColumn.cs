using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.DataPortals.Core.Data
{
    public interface IDataItemProperty
    {
        Int32 OridinalPosition { get; set; }
        String Name { get; set; }
        String Description { get; set; }
        Type DataType { get; set; }
    }
}
