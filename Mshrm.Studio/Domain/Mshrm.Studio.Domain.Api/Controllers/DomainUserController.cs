using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Domain.Api.Models.Dtos.Users;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Dapr;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries;
using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Controllers
{
    /// <summary>
    /// User API 
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    public class DomainUserController : ControllerBase
    {
        private readonly ILogger<DomainUserController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainUserController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public DomainUserController(IMediator mediator, ILogger<DomainUserController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper; 

            _mediator = mediator;
        }

        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid identifier</param>
        /// <returns>A user if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DomainUserDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<DomainUserDto>> GetUserByGuidAsync([FromRoute] Guid guid)
        {
            // Get user
            var user = await _mediator.Send<User>(new GetUserByGuidQuery() { GuidId = guid }, Request.HttpContext.RequestAborted);

            // Map and return
            return _mapper.Map<DomainUserDto>(user);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">The email identifier</param>
        /// <returns>A user if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DomainUserDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("email/{email}")]
        public async Task<ActionResult<DomainUserDto?>> GetUserByEmailAsync([FromRoute] string email)
        {
            // Get user
            var user = await _mediator.Send<User>(new GetUserByEmailQuery() { Email = email }, Request.HttpContext.RequestAborted);

            // Map and return
            return _mapper.Map<DomainUserDto?>(user);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">The id identifier</param>
        /// <returns>A user if exists</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DomainUserDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("id/{id}")]
        public async Task<ActionResult<DomainUserDto?>> GetUserByIdAsync([FromRoute] int id)
        {
            // Get user
            var user = await _mediator.Send<User>(new GetUserByIdQuery() { Id = id }, Request.HttpContext.RequestAborted);

            // Map and return
            return _mapper.Map<DomainUserDto?>(user);
        }

        /// <summary>
        /// Get users paged
        /// </summary>
        /// <param name="search">A search term</param>
        /// <param name="email">An email</param>
        /// <param name="firstName">A first name</param>
        /// <param name="lastName">A last name</param>
        /// <param name="fullName">A full name</param>
        /// <param name="orderProperty">The property to order by</param>
        /// <param name="order">The order to return set in</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="perPage">How many to return in the page</param>
        /// <returns>A page of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResultDto<DomainUserDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("")]
        public async Task<ActionResult<PageResultDto<DomainUserDto>>> GetUsersPagedAsync([FromQuery] string? search, [FromQuery] string? email, [FromQuery] string? firstName, [FromQuery] string? lastName, 
            [FromQuery] string? fullName, [FromQuery] string? orderProperty = "createdDate", [FromQuery] Order order = Order.Descending, [FromQuery] uint pageNumber = 1, [FromQuery] uint perPage = 30)
        {
            // Get users
            var user = await _mediator.Send<PagedResult<User>>(new GetUsersPagedQuery() 
            { 
                SearchTerm = search,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                FullName = fullName
            }, Request.HttpContext.RequestAborted);

            // Map and return
            return _mapper.Map<PageResultDto<DomainUserDto>>(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="dto">The user data</param>
        /// <returns>The new user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DomainUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DomainUserDto), StatusCodes.Status201Created)]
        [Topic("pubsub", "user-created")]
        [Route("")]
        public async Task<ActionResult<DomainUserDto>> CreateUserAsync([FromBody] CreateDomainUserDto dto)
        {
            // Create user
            var user = await _mediator.Send<User>(_mapper.Map<CreateUserCommand>(dto), Request.HttpContext.RequestAborted);

            // Map and return
            return _mapper.Map<DomainUserDto>(user);
        }
    }
}
