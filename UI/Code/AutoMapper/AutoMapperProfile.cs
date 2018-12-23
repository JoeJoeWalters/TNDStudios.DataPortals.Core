using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.DataPortals.Api;
using TNDStudios.DataPortals.Data;
using TNDStudios.DataPortals.PropertyBag;
using TNDStudios.DataPortals.Repositories;
using TNDStudios.DataPortals.Security;
using TNDStudios.DataPortals.UI.Models.Api;
using TNDStudios.DataPortals.Helpers;

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

            // Base transformation for common object model to and from domain object
            CreateMap<CommonObject, CommonObjectModel>();
            CreateMap<CommonObjectModel, CommonObject>()
                .ForMember(
                    item => item.ParentPackage,
                    opt => opt.Ignore()
                    );

            // Map from the data item definition to the web model
            CreateMap<DataItemDefinition, DataItemDefinitionModel>()
                .ForMember(
                    item => item.Culture,
                    opt => opt.MapFrom(
                        src => (src.Culture == CultureInfo.InvariantCulture) ? "" : src.Culture.Name
                       )
                    )
                .ForMember(
                    item => item.EncodingFormat,
                    opt => opt.MapFrom(
                        src => src.EncodingFormat.WebName
                        )
                    );
            CreateMap<DataItemDefinition, CommonObjectModel>();

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
                        src => (src.Culture == "") ? CultureInfo.InvariantCulture : CultureInfo.GetCultureInfo(src.Culture)
                       )
                    )
                .ForMember(
                    item => item.EncodingFormat,
                    opt => opt.MapFrom(
                        src => Encoding.GetEncoding(src.EncodingFormat)
                        )
                    )
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());

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
            CreateMap<ApiDefinition, CommonObjectModel>();

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
                    )
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());

            // Map from the data connection domain object to a key/value pairing
            CreateMap<DataConnection, KeyValuePair<Guid, String>>()
                .ConstructUsing(api => new KeyValuePair<Guid, string>(api.Id, api.Name));

            // Map from the data connection domain object to the web view model
            CreateMap<DataConnection, DataConnectionModel>()
                .ForMember(item => item.ProviderData, opt => opt.Ignore()); // Manually Mapped

            CreateMap<DataConnection, CommonObjectModel>();

            // Map from the web view model of the data connection to the domain object
            CreateMap<DataConnectionModel, DataConnection>()
                .ForMember(
                    item => item.Credentials,
                    opt => opt.MapFrom(
                        src => src.Credentials.Key
                        )
                    )
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());

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
                    )
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());

            // Map from the transformation domain object to a key/value pairing
            CreateMap<Transformation, KeyValuePair<Guid, String>>()
                .ConstructUsing(trans => new KeyValuePair<Guid, string>(trans.Id, trans.Name));

            // Map from the transformation domain object to the web view model
            CreateMap<Transformation, TransformationModel>();
            CreateMap<Transformation, CommonObjectModel>();

            // Map from the package domain object to the web view model
            CreateMap<Package, PackageModel>();
            CreateMap<Package, CommonObjectModel>();

            // Map from the package web view model to the domain object
            CreateMap<PackageModel, Package>()
                .ForMember(item => item.ApiDefinitions, opt => opt.Ignore())
                .ForMember(item => item.DataDefinitions, opt => opt.Ignore())
                .ForMember(item => item.DataConnections, opt => opt.Ignore())
                .ForMember(item => item.Transformations, opt => opt.Ignore())
                .ForMember(item => item.CredentialsStore, opt => opt.Ignore())
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());

            // Map from the Credentials object to a key/value pairing
            CreateMap<Credentials, KeyValuePair<Guid, String>>()
                .ConstructUsing(cred => new KeyValuePair<Guid, string>(cred.Id, cred.Name));

            // Map from the credentials domain object to the web view model
            CreateMap<Credentials, CredentialsModel>();
            CreateMap<Credential, CredentialModel>();
            CreateMap<Credentials, CommonObjectModel>();
            CreateMap<CredentialsLink, CredentialsLinkModel>();

            // Map from the credentials web view model to the domain object
            CreateMap<CredentialsModel, Credentials>()
                .ForMember(item => item.ParentPackage, opt => opt.Ignore());
            CreateMap<CredentialModel, Credential>();
            CreateMap<CredentialsLinkModel, CredentialsLink>();

            // Map from the permissions domain object to the web view model
            CreateMap<Permissions, PermissionsModel>();

            // Map from the permissions web view model to the domain object
            CreateMap<PermissionsModel, Permissions>();

            // Map from the property bag domain object to the web view model
            CreateMap<PropertyBagItem, PropertyBagItemModel>();
            CreateMap<PropertyBagItemType, PropertyBagItemTypeModel>()
                .ForMember(
                    item => item.PropertyType,
                    opt => opt.MapFrom(
                        src => new KeyValuePair<Int32, String>(
                            (Int32)src.PropertyType,
                            src.PropertyType.GetEnumDescription()
                            )
                        )
                    )
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => src.DataType.ToShortName()
                        )
                    );

            // Map from the property bag web view model to the domain object
            CreateMap<PropertyBagItemModel, PropertyBagItem>();
            CreateMap<PropertyBagItemTypeModel, PropertyBagItemType>()
                .ForMember(
                    item => item.PropertyType,
                    opt => opt.MapFrom(
                        src => src.PropertyType.Key
                        )
                    )
                .ForMember(
                    item => item.DataType,
                    opt => opt.MapFrom(
                        src => src.DataType.FromShortName()
                        )
                    );

            // Map the data provider base class to a model to represent it
            CreateMap<IDataProvider, DataProviderModel>()
                .ForMember(
                    item => item.PropertyBagTypes,
                    opt => opt.MapFrom(
                        src => src.PropertyBagTypes()
                        )
                    );
        }
    }
}
