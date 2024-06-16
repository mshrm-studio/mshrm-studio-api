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
using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Domain.Clients.Queries;
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
    [Route("api/v1/clients")]
    public class ClientController : BaseAuthController
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ClientController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<ClientController> logger, IMapper mapper) : base(httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="model">The new clients configuration</param>
        /// <returns>The client created</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(typeof(CreatedClientResponseDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<CreatedClientResponseDto>> CreateClientAsync([FromBody] CreateClientRequestDto model)
        {
            // Create client
            var client = await _mediator.Send<ClientWithSecret>(_mapper.Map<CreateClientCommand>(model), Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<CreatedClientResponseDto>(client));
        }

        /// <summary>
        /// Get a page of clients
        /// </summary>
        /// <param name="clientName">The clients name</param>
        /// <param name="clientId">The clients id</param>
        /// <param name="searchTerm">A search term</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>Clients</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(typeof(PageResultDto<ClientResponseDto>), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<PageResultDto<ClientResponseDto>>> GetClientsPagedAsync([FromQuery] string? clientName, string? clientId, [FromQuery] string? searchTerm, 
            [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get clients
            var clients = await _mediator.Send<PagedResult<Client>>(new GetPagedClientsQuery() 
            { 
                SearchTerm = searchTerm,
                ClientName = clientName, 
                ClientId = clientId,
                Order = order,
                OrderProperty = orderProperty,
                PageNumber = pageNumber,
                PerPage = perPage
            }, Request.HttpContext.RequestAborted);

            // Return
            return Ok(_mapper.Map<PageResultDto<ClientResponseDto>>(clients));
        }
    }
}
