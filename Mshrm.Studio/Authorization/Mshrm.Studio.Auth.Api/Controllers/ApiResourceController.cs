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
    [Route("api/v1/api-resources")]
    public class ApiResourceController : BaseAuthController
    {
        private readonly ILogger<ApiResourceController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResourceController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ApiResourceController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<ApiResourceController> logger, IMapper mapper) : base(httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Create a new api resource
        /// </summary>
        /// <param name="model">The new api resource configuration</param>
        /// <returns>The api resource created</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(CreatedApiResourceResponseDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<CreatedApiResourceResponseDto>> CreateApiResourceAsync([FromBody] CreateApiResourceRequestDto model)
        {
            // Create client
            var client = await _mediator.Send<ApiResourceWithSecret>(_mapper.Map<CreateApiResourceCommand>(model), Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<CreatedApiResourceResponseDto>(client));
        }

        /// <summary>
        /// Get a page of api resources
        /// </summary>
        /// <param name="name">The api resources name</param>
        /// <param name="searchTerm">A search term</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Api resources</returns>
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(PageResultDto<ApiResourceResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ApiResourceResponseDto>>> GetApiResourcesPagedAsync([FromQuery] string? name, [FromQuery] string? searchTerm, 
            [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get clients
            var apiResources = await _mediator.Send<PagedResult<ApiResource>>(new GetPagedApiResourcesQuery() 
            { 
                SearchTerm = searchTerm,
                Name = name, 
                Order = order,
                OrderProperty = orderProperty,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<PageResultDto<ApiResourceResponseDto>>(apiResources));
        }
    }
}
