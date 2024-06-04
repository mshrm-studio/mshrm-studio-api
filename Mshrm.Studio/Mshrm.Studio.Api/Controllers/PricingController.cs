using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.Prices;
using Mshrm.Studio.Api.Services.Api;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;

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
    }
}
