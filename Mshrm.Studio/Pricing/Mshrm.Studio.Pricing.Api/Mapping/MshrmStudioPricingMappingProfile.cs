using AutoMapper;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Currencies;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Currency;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Prices;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Events;
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
            #region Currencies

            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<PagedResult<Currency>, PageResultDto<CurrencyDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();
            CreateMap<UpdateSupportedCurrencyDto, UpdateSupportedCurrencyCommand>().ReverseMap();
            CreateMap<CreateSupportedCurrencyDto, CreateSupportedCurrencyCommand>().ReverseMap();

            #endregion

            #region Prices

            CreateMap<PagedResult<ExchangePricingPair>, PageResultDto<PriceDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();

            CreateMap<ExchangePricingPair, PriceDto>().ReverseMap();

            CreateMap<ExchangePricePairUpdatedEvent, CreateExchangePricingPairHistoryCommand>()
                .ReverseMap();
            CreateMap<ExchangePricePairCreatedEvent, CreateExchangePricingPairHistoryCommand>()
                .ForMember(dest => dest.NewMarketCap, src => src.MapFrom(x => x.ExchangePricingPair.MarketCap))
                .ForMember(dest => dest.PricingProviderType, src => src.MapFrom(x => x.ExchangePricingPair.PricingProviderType))
                .ForMember(dest => dest.NewPrice, src => src.MapFrom(x => x.ExchangePricingPair.Price))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.ExchangePricingPair.Id))
                .ForMember(dest => dest.NewVolume, src => src.MapFrom(x => x.ExchangePricingPair.Volume))
                .ForMember(dest => dest.UpdatedDate, src => src.MapFrom(x => x.ExchangePricingPair.UpdatedDate))
                .ReverseMap();

            #endregion
        }
    }
}
