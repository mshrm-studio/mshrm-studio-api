using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Queries;
using Mshrm.Studio.Localization.Api.Models.Dtos.LocalizationResources;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Api.Services.Api;
using Mshrm.Studio.Localization.Domain.LocalizationResources.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Services;
using Mshrm.Studio.Shared.Services.Interfaces;
using System;

namespace Mshrm.Studio.Localization.Api.Controllers
{
    /// <summary>
    /// Localization API
    /// </summary>
    [ApiController]
    [Route("api/v1/localization")]
    public class LocalizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<LocalizationController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public LocalizationController(IMediator mediator, ILogger<LocalizationController> logger, IMapper mapper)
        {
            _mediator = mediator;

            _logger = logger;
            _mapper= mapper;
        }

        /// <summary>
        /// Get localization resources
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="key">The resource text to localize</param>
        /// <returns>Localization resources</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<LocalizationResourceDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<List<LocalizationResourceDto>>> GetLocalizationResourcesAsync([FromQuery] LocalizationArea? area, [FromQuery] string? culture, [FromQuery] string? key)
        {
            var localizationResources = await _mediator.Send<List<LocalizationResource>>(new GetLocalizationResourcesQuery()
            { 
                Area = area, 
                Culture= culture, 
                Key = key 
            }, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<List<LocalizationResourceDto>>(localizationResources));
        }

        /// <summary>
        /// Create a localization resource
        /// </summary>
        /// <param name="model">The resource data</param>
        /// <returns>The new localization resource</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LocalizationResourceDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<LocalizationResourceDto>> CreateLocalizationResourcesAsync([FromBody] CreateLocalizationResourceDto model)
        {
            var command = _mapper.Map<CreateLocalizationResourceCommand>(model);

            var localizationResource = await _mediator.Send<LocalizationResource>(command, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<LocalizationResourceDto>(localizationResource));
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resource to delete</param>
        /// <returns>True</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("{guidId}")]
        public async Task<ActionResult<bool>> DeleteLocalizationResourcesAsync([FromRoute] Guid guidId)
        {
            var deleted = await _mediator.Send<bool>(new DeleteLocalizationResourceCommand() { GuidId = guidId }, Request.HttpContext.RequestAborted);

            return Ok(deleted);
        }

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resource to get</param>
        /// <returns>A localization resource</returns>
        [HttpGet]
        [ProducesResponseType(typeof(LocalizationResourceDto), StatusCodes.Status200OK)]
        [Route("{guidId}")]
        public async Task<ActionResult<LocalizationResourceDto>> GetLocalizationResourceAsync([FromRoute] Guid guidId)
        {
            var localizationResource = await _mediator.Send<LocalizationResource>(new GetLocalizationResourceByGuidQuery(){ GuidId = guidId }, Request.HttpContext.RequestAborted);

            return Ok(_mapper.Map<LocalizationResourceDto>(localizationResource));
        }

        /// <summary>
        /// Gets all supported localization cultures
        /// </summary>
        /// <returns>All supported localization cultures</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [Route("supported-cultures")]
        public async Task<ActionResult<List<string>>> GetSupportedLocalizationCulturesAsync()
        {
            var localizationResourceCultures = await _mediator.Send<List<string>>(new GetSupportedLocalizationResourceCulturesQuery(), Request.HttpContext.RequestAborted);

            return Ok(localizationResourceCultures);
        }

        /// <summary>
        /// Gets all keys for localization area
        /// </summary>
        /// <param name="localizationArea">Get all keys for a localization area</param>
        /// <returns>All keys that can be used for a localization area</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [Route("localization-area/{localizationArea}/keys")]
        public async Task<ActionResult<List<string>>> GetLocalizationAreaKeysAsync([FromRoute] LocalizationArea localizationArea)
        {
            var localizationAreaKeys = await _mediator.Send<List<string>>(new GetKeysForLocalizationAreaQuery() { LocalizationArea = localizationArea }, Request.HttpContext.RequestAborted);

            return Ok(localizationAreaKeys);
        }
    }
}
