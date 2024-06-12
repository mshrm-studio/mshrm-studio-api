using AutoMapper;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPriceHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Events;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Asset;
using Mshrm.Studio.Pricing.Api.Models.Dtos.AssetPrices;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Application.Dtos.AssetPriceHistories;
using Mshrm.Studio.Pricing.Application.Dtos.Providers;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;
using Mshrm.Studio.Pricing.Domain.ProviderAssets;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Api.Mapping
{
    /// <summary>
    /// The pricing mapping profile
    /// </summary>
    public class MshrmStudioPricingMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioPricingMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init mappings
        /// </summary>
        public void Init()
        {
            #region Assets

            CreateMap<Asset, AssetDto>().ReverseMap();
            CreateMap<PagedResult<Asset>, PageResultDto<AssetDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();
            CreateMap<UpdateSupportedAssetDto, UpdateSupportedAssetCommand>().ReverseMap();
            CreateMap<CreateSupportedAssetDto, CreateSupportedAssetCommand>().ReverseMap();
            CreateMap<ProviderAsset, ProviderAssetDto>().ReverseMap();

            #endregion

            #region Prices

            CreateMap<PagedResult<AssetPrice>, PageResultDto<AssetPriceDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();

            CreateMap<PagedResult<AssetPriceHistory>, PageResultDto<AssetPriceHistoryDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();

            CreateMap<AssetPriceHistory, AssetPriceHistoryDto>()
                .ReverseMap();

            CreateMap<AssetPrice, AssetPriceDto>().ReverseMap();

            CreateMap<AssetPriceUpdatedEvent, CreateAssetPriceHistoryCommand>()
                .ReverseMap();
            CreateMap<AssetPriceCreatedEvent, CreateAssetPriceHistoryCommand>()
                .ForMember(dest => dest.NewMarketCap, src => src.MapFrom(x => x.AssetPrice.MarketCap))
                .ForMember(dest => dest.PricingProviderType, src => src.MapFrom(x => x.AssetPrice.PricingProviderType))
                .ForMember(dest => dest.NewPrice, src => src.MapFrom(x => x.AssetPrice.Price))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.AssetPrice.Id))
                .ForMember(dest => dest.NewVolume, src => src.MapFrom(x => x.AssetPrice.Volume))
                .ForMember(dest => dest.UpdatedDate, src => src.MapFrom(x => x.AssetPrice.UpdatedDate))
                .ReverseMap();

            #endregion
        }
    }
}
