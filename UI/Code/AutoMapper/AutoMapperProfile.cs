using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    )
                .ForMember(
                    item => item.Id,
                    opt => opt.MapFrom(
                        src => src.Id.ToString()
                        )
                    );

            CreateMap<DataItemProperty, DataItemPropertyModel>()
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => src.DataType.ToString()
                        )
                    );

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
                    )
                .ForMember(
                    item => item.Id,
                    opt => opt.MapFrom(
                        src => new Guid(src.Id)
                        )
                    );

            CreateMap<DataItemPropertyModel, DataItemProperty>()
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => Type.GetType(src.DataType)
                        )
                );
        }
    }
}
