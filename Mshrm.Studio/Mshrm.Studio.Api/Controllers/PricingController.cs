using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Models.Dtos.Prices;
using Mshrm.Studio.Api.Services.Api;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Order = Mshrm.Studio.Api.Clients.Pricing.Order;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Pricing API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/prices")]
    public class PricingController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly IQueryPricesService _queryPricesService;
        private readonly IQueryPriceHistoryService _queryPriceHistoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingController"/> class.
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public PricingController(IDomainUserClient domainUserClient, IQueryPricesService queryPricesService, IHttpContextAccessor contextAccessor, IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _queryPricesService = queryPricesService;

            _mapper = mapper;
        }

        /// <summary>
        /// Gets assets
        /// </summary>
        /// <param name="pricingProviderType">The provider type</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="baseAsset">The base asset (conversion to)</param>
        /// <param name="symbols">Symbols to get prices for - all returned if left null</param>
        /// <returns>Latest prices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PriceResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<List<PriceResponseDto>>> GetLatestPricesAsync([FromQuery] PricingProviderType? pricingProviderType,
            [FromQuery] AssetType? assetType, [FromQuery] string baseAsset = "USD", [FromQuery] List<string>? symbols = null)
        {
            var prices = await _queryPricesService.GetLatestPricesAsync(pricingProviderType, assetType, baseAsset, symbols, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<List<PriceResponseDto>>(prices));
        }

        /// <summary>
        /// Gets price history
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
        [ProducesResponseType(typeof(PageResultDto<PriceHistoryResponseDto>), StatusCodes.Status200OK)]
        [Route("history")]
        public async Task<ActionResult<List<PriceDto>>> GetLatestPricesAsync([FromQuery] string assetGuidId, [FromQuery] PricingProviderType? pricingProviderType,
            [FromQuery] string baseAssetGuidId, [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1,
             [FromQuery] uint perPage = 30)
        {
            var priceHistory = _queryPriceHistoryService.GetPagedPriceHistoryAsync(assetGuidId, baseAssetGuidId, pricingProviderType, orderProperty, order, pageNumber, perPage, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<PageResultDto<PriceHistoryResponseDto>>(priceHistory));
        }
    }
}
