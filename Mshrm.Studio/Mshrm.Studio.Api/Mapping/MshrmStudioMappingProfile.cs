using AutoMapper;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Mapping.Converters;
using Mshrm.Studio.Api.Models.Dtos.ContactForms;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Models.Dtos.Localization;
using Mshrm.Studio.Api.Models.Dtos.Prices;
using Mshrm.Studio.Api.Models.Dtos.Tools;
using Mshrm.Studio.Api.Models.Dtos.User;
using Mshrm.Studio.Shared.Models.Dtos;

namespace Mshrm.Studio.Api.Mapping
{
    /// <summary>
    /// The aggregator mapping profile
    /// </summary>
    public class MshrmStudioMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init mappings
        /// </summary>
        private void Init()
        {
            #region Users

            CreateMap<DomainUserDto, MshrmStudioUserResponseDto>().ReverseMap();

            #endregion

            #region ContactForms

            CreateMap<PageResultDtoOfContactFormDto, PageResultDto<ContactFormDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
                .ReverseMap();
            CreateMap<PageResultDtoOfContactFormDto, PageResultDto<ContactFormResponseDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
                .ReverseMap();
            CreateMap<ContactFormDto, ContactFormResponseDto>().ConvertUsing<ContactFormConverter>();

            #endregion

            #region Assets

            CreateMap<AssetDto, AssetResponseDto>().ConvertUsing<AssetConverter>();
            CreateMap<PageResultDtoOfAssetDto, PageResultDto<AssetResponseDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
                .ReverseMap();
            CreateMap<ProviderAssetDto, ProviderAssetResponseDto>().ReverseMap();

            #endregion

            #region Tools

            CreateMap<ToolDto, ToolResponseDto>().ConvertUsing<ToolConverter>();
            CreateMap<PageResultDtoOfToolDto, PageResultDto<ToolDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
               .ReverseMap();
            CreateMap<PageResultDtoOfToolDto, PageResultDto<ToolResponseDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
              .ReverseMap();

            #endregion

            #region Localization

            CreateMap<LocalizationResourceDto, LocalizationResourceResponseDto>().ReverseMap();

            #endregion

            #region Files

            CreateMap<TemporaryFileUploadDto, TemporaryFileUploadResponseDto>().ReverseMap();

            #endregion

            #region Prices

            CreateMap<PageResultDtoOfContactFormDto, PageResultDto<PriceResponseDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
                .ReverseMap();
            CreateMap<AssetPriceDto, PriceResponseDto>().ReverseMap();
            CreateMap<PageResultDtoOfAssetPriceHistoryDto, PageResultDto<PriceHistoryResponseDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.PageNumber <= 0 ? 1 : x.PageNumber))
                .ReverseMap();
            CreateMap<AssetPriceHistoryDto, PriceHistoryResponseDto>().ReverseMap();

            #endregion
        }
    }
}
