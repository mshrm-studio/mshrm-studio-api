using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Currencies;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Currency;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Api.Controllers
{
    /// <summary>
    /// Currencies API
    /// </summary>
    [ApiController]
    [Route("api/v1/currencies")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ILogger<CurrenciesController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public CurrenciesController(IMediator mediator, ILogger<CurrenciesController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a supported currency.
        /// </summary>
        /// <param name="model">The new currency to add</param>
        /// <returns>The new currency added</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CurrencyDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("")]
        public async Task<ActionResult<CurrencyDto>> CreateSupportedCurrencyAsync([FromBody] CreateSupportedCurrencyDto model)
        {
            var newSupportedCurrency = await _mediator.Send<Currency>(_mapper.Map<CreateSupportedCurrencyCommand>(model), Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<CurrencyDto>(newSupportedCurrency));
        }

        /// <summary>
        /// Update a supported currency.
        /// </summary>
        /// <param name="currencyId">The currency to update</param>
        /// <param name="model">The update data</param>
        /// <returns>The updated currency</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(CurrencyDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("{currencyId}")]
        public async Task<ActionResult<CurrencyDto>> UpdateSupportedCurrencyAsync([FromRoute] Guid currencyId, [FromBody] UpdateSupportedCurrencyDto model)
        {
            var command = _mapper.Map<UpdateSupportedCurrencyCommand>(model);
            command.CurrencyId = currencyId;

            var updatedCurrency = await _mediator.Send<Currency>(command, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<CurrencyDto>(updatedCurrency));
        }

        /// <summary>
        /// Gets currencies
        /// </summary>
        /// <param name="search">A search value</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="name">A name</param>
        /// <param name="pricingProviderType">The provider type</param>
        /// <param name="currencyType">The type of currency</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Supported currencies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<CurrencyDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<CurrencyDto>>> GetSupportedCurrenciesAsync([FromQuery] string? search, [FromQuery] string? symbol, 
             [FromQuery] string? name, [FromQuery] PricingProviderType? pricingProviderType, [FromQuery] CurrencyType? currencyType,
             [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, 
             [FromQuery] uint perPage = 30)
        {
            var currencies = await _mediator.Send<PagedResult<Currency>>(new GetPagedCurrenciesQuery() 
            { 
                Search = search,
                Symbol = symbol,
                Name = name,
                PricingProviderType = pricingProviderType,
                CurrencyType = currencyType,
                OrderProperty = orderProperty,
                Order = order,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map< PageResultDto<CurrencyDto>>(currencies));
        }
    }
}
