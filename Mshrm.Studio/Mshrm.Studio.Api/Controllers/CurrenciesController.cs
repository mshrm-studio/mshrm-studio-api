using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos;
using Mshrm.Studio.Api.Models.Dtos.Currencies;
using Mshrm.Studio.Api.Services.Api;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Order = Mshrm.Studio.Shared.Enums.Order;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Currencies API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/currencies")]
    public class CurrenciesController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICreateCurrenciesService _createCurrenciesService;
        private readonly IUpdateCurrenciesService _updateCurrenciesService;
        private readonly IQueryCurrenciesService _queryCurrenciesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="createCurrenciesService"></param>
        /// <param name="updateCurrenciesService"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public CurrenciesController(IDomainUserClient domainUserClient, ICreateCurrenciesService createCurrenciesService, IUpdateCurrenciesService updateCurrenciesService,
            IQueryCurrenciesService queryCurrenciesService, IHttpContextAccessor contextAccessor, IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _createCurrenciesService = createCurrenciesService;
            _updateCurrenciesService = updateCurrenciesService;
            _queryCurrenciesService = queryCurrenciesService;

            _mapper = mapper;
        }

        /// <summary>
        /// Create a new currency
        /// </summary>
        /// <param name="model">The currency to create</param>
        /// <returns>A new currency</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CurrencyResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("")]
        public async Task<ActionResult<CurrencyResponseDto>> CreateCurrencyAsync([FromBody] CreateCurrencyDto model)
        {
            // Create the new currency
            var newCurrency = await _createCurrenciesService.CreateCurrencyAsync(model.Logo, model.Name, model.Symbol, model.SymbolNative, model.Description, model.CurrencyType, 
                model.ProviderType, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<CurrencyResponseDto>(newCurrency));
        }

        /// <summary>
        /// Update a supported currency.
        /// </summary>
        /// <param name="currencyId">The currency to update</param>
        /// <param name="model">The update data</param>
        /// <returns>The updated currency</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(CurrencyResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("{currencyId}")]
        public async Task<ActionResult<CurrencyResponseDto>> UpdateCurrencyAsync([FromRoute] Guid currencyId, [FromBody] UpdateCurrencyDto model)
        {
            // Update
            var updatedCurrency = await _updateCurrenciesService.UpdateCurrencyAsync(currencyId, model.Name, model.Description, model.SymbolNative,
                model.ProviderType, model.CurrencyType, model.Logo, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<CurrencyResponseDto>(updatedCurrency));
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
        [ProducesResponseType(typeof(PageResultDto<CurrencyResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<CurrencyResponseDto>>> GetCurrenciesAsync([FromQuery] string? search, [FromQuery] string? symbol,
             [FromQuery] string? name, [FromQuery] PricingProviderType? pricingProviderType, [FromQuery] CurrencyType? currencyType,
             [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1,
             [FromQuery] uint perPage = 30)
        {
            var currencies = await _queryCurrenciesService.GetCurrenciesAsync(search, symbol, name, pricingProviderType, currencyType, orderProperty, order, pageNumber, perPage, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<PageResultDto<CurrencyResponseDto>>(currencies));
        }

        /// <summary>
        /// Get a currency by Guid
        /// </summary>
        /// <param name="guid">The currency</param>
        /// <returns>A currency if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CurrencyResponseDto), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<CurrencyResponseDto>> GetCurrencyByGuidAsync([FromRoute] Guid guid)
        {
            var currency = await _queryCurrenciesService.GetCurrencyByGuidAsync(guid, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<CurrencyResponseDto>(currency));
        }
    }
}
