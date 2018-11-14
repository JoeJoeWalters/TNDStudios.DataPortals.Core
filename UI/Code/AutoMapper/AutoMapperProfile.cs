using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.UI.Models.Api;

namespace TNDStudios.DataPortals.UI
{
    /// <summary>
    /// Definition of how objects are to map
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Set up the mappings
        /// </summary>
        public AutoMapperProfile()
        {
            // Map from the data item definition domain object to a key/value pairing
            CreateMap<DataItemDefinition, KeyValuePair<Guid, String>>()
                .ConstructUsing(api => new KeyValuePair<Guid, string>(api.Id, api.Name));

            // Map from the data item definition to the web model
            CreateMap<DataItemDefinition, DataItemDefinitionModel>()
                .ForMember(
                    item => item.Culture,
                    opt => opt.MapFrom(
                        src => (src.Culture == CultureInfo.InvariantCulture) ? "Invariant" : src.Culture.Name
                       )
                    )
                .ForMember(
                    item => item.EncodingFormat,
                    opt => opt.MapFrom(
                        src => src.EncodingFormat.WebName
                        )
                    );

            // Map from the data item definition sub-property to the web view model
            CreateMap<DataItemProperty, DataItemPropertyModel>()
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => src.DataType.ToString()
                        )
                    );

            // Map from the web view model of the data item definition back to the domain object
            CreateMap<DataItemDefinitionModel, DataItemDefinition>()
                .ForMember(
                    item => item.Culture,
                    opt => opt.MapFrom(
                        src => (src.Culture == "Invariant") ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(src.Culture)
                       )
                    )
                .ForMember(
                    item => item.EncodingFormat,
                    opt => opt.MapFrom(
                        src => Encoding.GetEncoding(src.EncodingFormat)
                        )
                    );

            // Map from the web view model sub-property of the data item definition
            // back to the domain object type
            CreateMap<DataItemPropertyModel, DataItemProperty>()
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => Type.GetType(src.DataType)
                        )
                    );

            // Map from the api definition domain object to a key/value pairing
            CreateMap<ApiDefinition, KeyValuePair<Guid, String>>()
                .ConstructUsing(api => new KeyValuePair<Guid, string>(api.Id, api.Name));

            // Map from the api definition domain object to web view model
            CreateMap<ApiDefinition, ApiDefinitionModel>();

            // Map from the web view model of the api definition to the domain object
            CreateMap<ApiDefinitionModel, ApiDefinition>()
                .ForMember(
                    item => item.DataConnection,
                    opt => opt.MapFrom(
                        src => src.DataConnection.Key
                        )
                    )
                .ForMember(
                    item => item.DataDefinition,
                    opt => opt.MapFrom(
                        src => src.DataDefinition.Key
                        )
                    );

            // Map from the data connection domain object to a key/value pairing
            CreateMap<DataConnection, KeyValuePair<Guid, String>>()
                .ConstructUsing(api => new KeyValuePair<Guid, string>(api.Id, api.Name));

            // Map from the data connection domain object to the web view model
            CreateMap<DataConnection, DataConnectionModel>();

            // Map from the web view model of the data connection to the domain object
            CreateMap<DataConnectionModel, DataConnection>();

            // Create a generic key pair to id mapping
            CreateMap<KeyValuePair<Guid, String>, Guid>()
                .ConstructUsing(key => key.Key);

            // Create a generic id to key pair mapping
            CreateMap<Guid, KeyValuePair<Guid, String>>()
                .ConstructUsing(key => new KeyValuePair<Guid, String>(key, ""));
            
            // Map from the web view model of the transformation model to the domain object
            CreateMap<TransformationModel, Transformation>()
                .ForMember(
                    item => item.Source,
                    opt => opt.MapFrom(
                        src => src.Source.Key
                        )
                    )
                .ForMember(
                    item => item.Destination,
                    opt => opt.MapFrom(
                        src => src.Destination.Key
                        )
                    );

            // Map from the transformation domain object to a key/value pairing
            CreateMap<Transformation, KeyValuePair<Guid, String>>()
                .ConstructUsing(trans => new KeyValuePair<Guid, string>(trans.Id, trans.Name));

            // Map from the transformation domain object to the web view model
            CreateMap<Transformation, TransformationModel>();
        }
    }
}
