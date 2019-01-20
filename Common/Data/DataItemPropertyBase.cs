﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TNDStudios.DataPortals.Data
{
    /// <summary>
    /// What type of property is this?
    /// </summary>
    public enum DataItemPropertyType
    {
        Property = 0, // Just a regular property
        Calculated = 1, // A calculated property (one not from the source data)
    }

    /// <summary>
    /// Definition of a property member of a data item
    /// </summary>
    [JsonObject]
    public class DataItemPropertyBase : CommonObject
    {
        /// <summary>
        /// What type of property is this?
        /// </summary>
        [JsonProperty]
        public DataItemPropertyType PropertyType { get; set; }

        /// <summary>
        /// Is this item part of a column identifier?
        /// </summary>
        [JsonProperty]
        public Boolean Key { get; set; }

        /// <summary>
        /// Is the property auto generated? e.g. the primary key is created etc.
        /// </summary>
        [JsonProperty]
        public Boolean AutoGenerated { get; set; }

        /// <summary>
        /// The path for the property (E.g. when using XML/Json etc.)
        /// </summary>
        [JsonProperty]
        public String Path { get; set; }

        /// <summary>
        /// The oridinal position of the property when using flat files etc.
        /// </summary>
        [JsonProperty]
        public Int32 OrdinalPosition { get; set; }

        /// <summary>
        /// The size of the property
        /// </summary>
        [JsonProperty]
        public Int32 Size { get; set; }

        /// <summary>
        /// The pattern of the data (such as the date format etc.)
        /// </summary>
        [JsonProperty]
        public String Pattern { get; set; }

        /// <summary>
        /// Is the field quoted (mainly when it comes from a flat file)
        /// </summary>
        [JsonProperty]
        public Boolean Quoted { get; set; }

        /// <summary>
        /// If this property is a calculated property then use this calculation
        /// </summary>
        [JsonProperty]
        public String Calculation { get; set; }

        /// <summary>
        /// If this data for this property is generated by the provider
        /// i.e. the value should not be inserted, it will be created by the 
        /// provider side. So not in inserts or updates
        /// </summary>
        [JsonProperty]
        public Boolean ProviderGenerated { get; set; }
        
        /// <summary>
        /// The default constructor
        /// </summary>
        public DataItemPropertyBase() : base() => Initialise();

        /// <summary>
        /// Common initialiser to be shared with derived objects
        /// </summary>
        public void Initialise()
        {
            PropertyType = DataItemPropertyType.Property; // Standard type by default
            Key = false; // By default this is not the primary key
            AutoGenerated = false; // Is the property auto generated by the provider / server
            Path = ""; // Empty String by default
            OrdinalPosition = -1; // First item in the array by default
            Size = 0; // The size of the property (width etc.)
            Pattern = ""; // The pattern of the data (such as the date format)
            Quoted = false; // Is the field quoted?
            Calculation = ""; // The column calculation
            ProviderGenerated = false; // Provider generates the value not the user
        }
    }
}
