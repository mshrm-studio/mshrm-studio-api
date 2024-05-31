using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Currency;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Prices;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers;
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
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PricesController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public PricesController(IMediator mediator, IDistributedCache cache, ILogger<PricesController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the latest price data for symbols
        /// </summary>
        /// <param name="pricingProviderType">The provider used to import</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="baseCurrency">The base currency</param>
        /// <param name="symbols">Symbols - all used if left empty</param>
        /// <returns>Latest prices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PriceDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<List<PriceDto>>> GetLatestPricesAsync([FromQuery] PricingProviderType? pricingProviderType,
            [FromQuery] CurrencyType? currencyType, [FromQuery] string baseCurrency = "USD", [FromQuery] List<string>? symbols = null)
        {
            var prices = await _mediator.Send<List<ExchangePricingPair>>(new GetLatestPricesQuery()
            {
                Symbols = symbols,
                BaseCurrencySymbol = baseCurrency,
                CurrencyType = currencyType,
                PricingProviderType = pricingProviderType
            });

            return Ok(_mapper.Map<List<PriceDto>>(prices));
        }
    }
}
