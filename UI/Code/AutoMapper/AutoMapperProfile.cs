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
                        src => src.Culture.Name
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
                        src => CultureInfo.GetCultureInfo(src.Culture)
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
            CreateMap<ApiDefinition, ApiDefinitionModel>()
                .ForMember(
                    item => item.DataConnection,
                    opt => opt.MapFrom(
                        src => new KeyValuePair<Guid, String>(src.DataConnection, "")
                        )
                    )
                .ForMember(
                    item => item.DataDefinition,
                    opt => opt.MapFrom(
                        src => new KeyValuePair<Guid, String>(src.DataDefinition, "")
                        )
                    );

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
            CreateMap<DataConnection, DataConnectionModel>()
                .ForMember(
                    item => item.Definitions,
                    opt => opt.MapFrom(
                        src => src.Definitions.Select(
                            item => new KeyValuePair<Guid, String>(item, "")
                            ).ToList()
                        )
                    );

            // Map from the web view model of the data connection to the domain object
            CreateMap<DataConnectionModel, DataConnection>()
                .ForMember(
                    item => item.Definitions,
                    opt => opt.MapFrom(
                        src => src.Definitions.Select(
                            item => item.Key
                            ).ToList()
                        )
                    );
        }
    }
}
