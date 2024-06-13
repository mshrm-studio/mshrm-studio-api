using AutoMapper;
using Dapr;
using Dapr.Client;
using Duende.IdentityServer.EntityFramework.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Auth.Api.Controllers.Bases;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Application.Dtos.ApiResources;
using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Mshrm.Studio.Auth.Domain.ApiResources.Commands;
using Mshrm.Studio.Auth.Domain.ApiResources.Queries;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Domain.Clients.Queries;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using System.Threading;

namespace Mshrm.Studio.Auth.Api.Controllers
{
    /// <summary>
    /// Client API
    /// </summary>
    [ApiController]
    [Route("api/v1/api-scopes")]
    public class ApiScopeController : BaseAuthController
    {
        private readonly ILogger<ApiScopeController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiScopeController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ApiScopeController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<ApiScopeController> logger, IMapper mapper) : base(httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Create a new api scope
        /// </summary>
        /// <param name="model">The new api scope configuration</param>
        /// <returns>The api scope created</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [ProducesResponseType(typeof(ApiScopeResponseDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<ApiScopeResponseDto>> CreateApiScopeAsync([FromBody] CreateApiScopeRequestDto model)
        {
            // Create api scope
            var apiScope = await _mediator.Send<ApiScope>(_mapper.Map<CreateApiScopeCommand>(model), Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<ApiScopeResponseDto>(apiScope));
        }

        /// <summary>
        /// Get a page of api scopes
        /// </summary>
        /// <param name="name">The api scopes name</param>
        /// <param name="searchTerm">A search term</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Api scopes</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(typeof(PageResultDto<ApiScopeResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ApiScopeResponseDto>>> GetApiScopesPagedAsync([FromQuery] string? name, [FromQuery] string? searchTerm, 
            [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get api scopes
            var apiScopes = await _mediator.Send<PagedResult<ApiScope>>(new GetPagedApiScopesQuery() 
            { 
                SearchTerm = searchTerm,
                Name = name, 
                Order = order,
                OrderProperty = orderProperty,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<PageResultDto<ApiScopeResponseDto>>(apiScopes));
        }
    }
}
