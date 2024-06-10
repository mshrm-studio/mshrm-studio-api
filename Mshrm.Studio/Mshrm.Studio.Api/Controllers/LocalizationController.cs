using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models.Dtos.Localization;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// Localization API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/localizations")]
    public class LocalizationController : MshrmStudioBaseController
    {
        private readonly IMapper _mapper;
        private readonly IQueryLocalizationService _queryLocalizationService;
        private readonly IDeleteLocalizationService _deleteLocalizationService;
        private readonly ICreateLocalizationService _createLocalizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="queryLocalizationService"></param>
        /// <param name="deleteLocalizationService"></param>
        /// <param name="createLocalizationService"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        public LocalizationController(IDomainUserClient domainUserClient, IQueryLocalizationService queryLocalizationService, IDeleteLocalizationService deleteLocalizationService,
            ICreateLocalizationService createLocalizationService, IHttpContextAccessor contextAccessor,
            IMapper mapper) : base(domainUserClient, contextAccessor)
        {
            _queryLocalizationService = queryLocalizationService;
            _deleteLocalizationService = deleteLocalizationService;
            _createLocalizationService = createLocalizationService;

            _mapper = mapper;
        }

        /// <summary>
        /// Create a localization
        /// </summary>
        /// <param name="model">The localization data</param>
        /// <returns>A file if exists</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(typeof(LocalizationResourceResponseDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<LocalizationResourceResponseDto>> CreateLocalizationResourceAsync([FromBody] CreateLocalizationResourceRequestDto model)
        {
            // Create localization
            var localization = await _createLocalizationService.CreateLocalizationResourceAsync(model.LocalizationArea, model.Culture, model.Name, model.Value, model.Comment,
                Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map< LocalizationResourceResponseDto>(localization));
        }

        /// <summary>
        /// Get localization resources
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The resource text to localize</param>
        /// <returns>Localization resources</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(typeof(List<LocalizationResourceResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<List<LocalizationResourceResponseDto>>> GetLocalizationResourcesAsync([FromQuery] LocalizationArea? area, [FromQuery] string? culture, [FromQuery] string? name)
        {
            // Get localizations
            var localizations = await _queryLocalizationService.GetLocalizationResourcesAsync(area, culture, name, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<List<LocalizationResourceResponseDto>>(localizations));
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resource to delete</param>
        /// <returns>True</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("{guidId}")]
        public async Task<ActionResult> DeleteLocalizationResourcesAsync([FromRoute] Guid guidId)
        {
            await _deleteLocalizationService.DeleteLocalizationResourceAsync(guidId, Request.HttpContext.RequestAborted);

            return Ok();
        }

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resource to get</param>
        /// <returns>A localization resource</returns>
        [HttpGet]
        [ProducesResponseType(typeof(LocalizationResourceResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("{guidId}")]
        public async Task<ActionResult<LocalizationResourceResponseDto>> GetLocalizationResourceAsync([FromRoute] Guid guidId)
        {
            var localization = await _queryLocalizationService.GetLocalizationResourceAsync(guidId, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<LocalizationResourceResponseDto>(localization));
        }
    }
}
