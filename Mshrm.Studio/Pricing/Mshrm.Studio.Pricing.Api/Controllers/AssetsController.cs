using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Api.Extensions;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;
using Mshrm.Studio.Pricing.Api.Models.Dtos.Asset;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Dtos.Providers;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Domain.ProviderAssets;
using Mshrm.Studio.Pricing.Domain.ProviderAssets.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Api.Controllers
{
    /// <summary>
    /// Assets API
    /// </summary>
    [ApiController]
    [Route("api/v1/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly ILogger<AssetsController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetsController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public AssetsController(IMediator mediator, ILogger<AssetsController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a supported asset.
        /// </summary>
        /// <param name="model">The new asset to add</param>
        /// <returns>The new asset added</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("")]
        public async Task<ActionResult<AssetDto>> CreateSupportedAssetAsync([FromBody] CreateSupportedAssetDto model)
        {
            var newSupportedAsset = await _mediator.Send<Asset>(_mapper.Map<CreateSupportedAssetCommand>(model), Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<AssetDto>(newSupportedAsset));
        }

        /// <summary>
        /// Update a supported asset.
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="model">The update data</param>
        /// <returns>The updated asset</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("{assetId}")]
        public async Task<ActionResult<AssetDto>> UpdateSupportedAssetAsync([FromRoute] Guid assetId, [FromBody] UpdateSupportedAssetDto model)
        {
            var command = _mapper.Map<UpdateSupportedAssetCommand>(model);
            command.AssetId = assetId;

            var updatedAsset = await _mediator.Send<Asset>(command, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<AssetDto>(updatedAsset));
        }

        /// <summary>
        /// Get all symbols supported by a provider
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported assets for a pricing provider</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProviderAssetDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("provider/{providerType}")]
        public async Task<ActionResult<List<ProviderAssetDto>>> GetProvidersAssetsAsync([FromRoute] PricingProviderType providerType)
        {
            var providersAssets = await _mediator.Send<List<ProviderAsset>>(new GetProviderAssetsQuery() { ProviderType = providerType }, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<List<ProviderAssetDto>>(providersAssets));
        }

        /// <summary>
        /// Gets assets
        /// </summary>
        /// <param name="search">A search value</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="name">A name</param>
        /// <param name="pricingProviderType">The provider type</param>
        /// <param name="assetType">The type of asset</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Supported assets</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<AssetDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<AssetDto>>> GetSupportedAssetsAsync([FromQuery] string? search, [FromQuery] string? symbol, 
             [FromQuery] string? name, [FromQuery] PricingProviderType? pricingProviderType, [FromQuery] AssetType? assetType,
             [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, 
             [FromQuery] uint perPage = 30)
        {
            var assets = await _mediator.Send<PagedResult<Asset>>(new GetPagedAssetsQuery() 
            { 
                Search = search,
                Symbol = symbol,
                Name = name,
                PricingProviderType = pricingProviderType,
                AssetType = assetType,
                OrderProperty = orderProperty,
                Order = order,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map< PageResultDto<AssetDto>>(assets));
        }
    }
}
