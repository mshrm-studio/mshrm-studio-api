using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Prices;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Dtos.Prices;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.Text;

namespace Mshrm.Studio.Pricing.Api.Controllers
{
    /// <summary>
    /// Prices API
    /// </summary>
    [ApiController]
    [Route("api/v1/prices")]
    public class PricesController : ControllerBase
    {
        private readonly ILogger<PricesController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PricesController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PricesController(IMediator mediator, ILogger<PricesController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the latest price data for symbols
        /// </summary>
        /// <param name="pricingProviderType">The provider used to import</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="baseAsset">The base asset</param>
        /// <param name="symbols">Symbols - all used if left empty</param>
        /// <returns>Latest prices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PriceDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<List<PriceDto>>> GetLatestPricesAsync([FromQuery] PricingProviderType? pricingProviderType,
            [FromQuery] AssetType? assetType, [FromQuery] string baseAsset = "USD", [FromQuery] List<string>? symbols = null)
        {
            var prices = await _mediator.Send<List<ExchangePricingPair>>(new GetLatestPricesQuery()
            {
                Symbols = symbols,
                BaseAssetSymbol = baseAsset,
                AssetType = assetType,
                PricingProviderType = pricingProviderType
            });

            return Ok(_mapper.Map<List<PriceDto>>(prices));
        }

        /// <summary>
        /// Gets price history paged
        /// </summary>
        /// <param name="pricingProviderType">The provider used to import</param>
        /// <param name="baseAssetGuidId">The base asset</param>
        /// <param name="assetGuidId">Asset to get history for</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Price history</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<PriceHistoryDto>), StatusCodes.Status200OK)]
        [Route("history")]
        public async Task<ActionResult<List<PriceDto>>> GetPriceHistoryAsync([FromQuery] string assetGuidId, [FromQuery] PricingProviderType? pricingProviderType, 
            [FromQuery] string baseAssetGuidId, [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1,
             [FromQuery] uint perPage = 30)
        {
            var prices = await _mediator.Send<PagedResult<ExchangePricingPairHistory>>(new GetPagedPriceHistoryQuery()
            {
                BaseAssetGuidId = assetGuidId,
                AssetGuidId = assetGuidId,
                PricingProviderType = pricingProviderType,
                OrderProperty = orderProperty,
                Order = order,
                PageNumber = pageNumber,
                PerPage = perPage
            });

            return Ok(_mapper.Map<PageResultDto<PriceHistoryDto>>(prices));
        }
    }
}
