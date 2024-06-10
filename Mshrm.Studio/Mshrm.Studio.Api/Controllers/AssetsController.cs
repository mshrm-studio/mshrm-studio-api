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
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Services.Api;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Order = Mshrm.Studio.Shared.Enums.Order;
using AssetType = Mshrm.Studio.Api.Clients.Pricing.AssetType;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Assets API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/assets")]
    public class AssetsController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICreateAssetService _createAssetService;
        private readonly IUpdateAssetsService _updateAssetService;
        private readonly IQueryAssetService _queryAssetService;
        private readonly IQueryProviderAssetsService _queryProviderAssetsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetsController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="createAssetService"></param>
        /// <param name="updateAssetService"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public AssetsController(IDomainUserClient domainUserClient, ICreateAssetService createAssetService, IUpdateAssetsService updateAssetService,
            IQueryAssetService queryAssetService, IQueryProviderAssetsService queryProviderAssetsService, IHttpContextAccessor contextAccessor, 
            IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _createAssetService = createAssetService;
            _updateAssetService = updateAssetService;
            _queryAssetService = queryAssetService;

            _queryProviderAssetsService = queryProviderAssetsService;

            _mapper = mapper;
        }

        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="model">The asset to create</param>
        /// <returns>A new asset</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AssetResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("")]
        public async Task<ActionResult<AssetResponseDto>> CreateAssetAsync([FromBody] CreateAssetRequestDto model)
        {
            // Create the new asset
            var newAsset = await _createAssetService.CreateAssetAsync(model.Logo, model.Name, model.Symbol, model.SymbolNative, model.Description, model.AssetType, 
                model.ProviderType, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<AssetResponseDto>(newAsset));
        }

        /// <summary>
        /// Update a supported asset.
        /// </summary>
        /// <param name="assetId">The asset to update</param>
        /// <param name="model">The update data</param>
        /// <returns>The updated asset</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(AssetResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("{assetId}")]
        public async Task<ActionResult<AssetResponseDto>> UpdateAssetAsync([FromRoute] Guid assetId, [FromBody] UpdateAssetRequestDto model)
        {
            // Update
            var updatedAsset = await _updateAssetService.UpdateAssetAsync(assetId, model.Name, model.Description, model.SymbolNative,
                model.ProviderType, model.AssetType, model.Logo, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<AssetResponseDto>(updatedAsset));
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
        [ProducesResponseType(typeof(PageResultDto<AssetResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<AssetResponseDto>>> GetAssetsAsync([FromQuery] string? search, [FromQuery] string? symbol,
             [FromQuery] string? name, [FromQuery] PricingProviderType? pricingProviderType, [FromQuery] AssetType? assetType,
             [FromQuery] string orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1,
             [FromQuery] uint perPage = 30)
        {
            var assets = await _queryAssetService.GetAssetsAsync(search, symbol, name, pricingProviderType, assetType, orderProperty, order, pageNumber, perPage, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<PageResultDto<AssetResponseDto>>(assets));
        }

        /// <summary>
        /// Get a asset by Guid
        /// </summary>
        /// <param name="guid">The asset</param>
        /// <returns>A asset if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(AssetResponseDto), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<AssetResponseDto>> GetAssetByGuidAsync([FromRoute] Guid guid)
        {
            var asset = await _queryAssetService.GetAssetByGuidAsync(guid, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<AssetResponseDto>(asset));
        }

        /// <summary>
        /// Get all assets supported by a provider
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported assets for a pricing provider</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProviderAssetResponseDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("provider/{providerType}")]
        public async Task<ActionResult<List<ProviderAssetResponseDto>>> GetProvidersAssetSymbolsAsync([FromRoute] PricingProviderType providerType)
        {
            var assets = await _queryProviderAssetsService.GetProvidersAssetsAsync(providerType);

            return Ok(_mapper.Map<List<ProviderAssetResponseDto>>(assets));
        }
    }
}
