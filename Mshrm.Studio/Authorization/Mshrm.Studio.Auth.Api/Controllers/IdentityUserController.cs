using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Auth.Api.Controllers.Bases;
using Mshrm.Studio.Auth.Api.Handlers;
using Mshrm.Studio.Auth.Api.Models.Dtos;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Auth.Domain.User.Queries;
using System.Security.Claims;

namespace Mshrm.Studio.Auth.Api.Controllers
{
    /// <summary>
    /// Identity User API
    /// </summary>
    [ApiController]
    [Route("api/v1/user")]
    public class IdentityUserController : BaseAuthController
    {
        private readonly ILogger<IdentityUserController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public IdentityUserController(IMediator mediator, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<IdentityUserController> logger) : base(httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new user with any role - authenticated to admin
        /// </summary>
        /// <param name="model">The new users information</param>
        /// <returns>A user if created successfully</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status201Created)]
        [Route("any-role")]
        public async Task<ActionResult<IdentityUserResponseDto>> CreateUserAnyRoleAsync([FromBody] CreateUserAnyRoleRequestDto model)
        {
            var user = await _mediator.Send<MshrmStudioIdentityUser>(_mapper.Map<CreateUserAnyRoleCommand>(model), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<IdentityUserResponseDto>(user));
        }

        /// <summary>
        /// Creates a new user with any role - no authentication
        /// </summary>
        /// <param name="model">The new users information</param>
        /// <returns>A user if created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status201Created)]
        [Route("")]
        public async Task<ActionResult<IdentityUserResponseDto>> CreateUserAsync([FromBody] CreateUserRequestDto model)
        {
            var user = await _mediator.Send<MshrmStudioIdentityUser>(_mapper.Map<CreateUserCommand>(model), Request.HttpContext.RequestAborted); 

            // Map and return
            return Ok(_mapper.Map<IdentityUserResponseDto>(user));
        }

        /// <summary>
        /// Creates a new user using the user role ONLY. Only SSO Authenticated users can call this to create accounts from their existing token. Only 1 user can be created per minute per IP address
        /// </summary>
        /// <returns>A user if created successfully</returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status201Created)]
        [Route("sso")]
        public async Task<ActionResult<IdentityUserResponseDto>> CreateUserFromSSOAsync()
        {
            // Create user
            var user = await _mediator.Send<MshrmStudioIdentityUser>(new CreateUserFromSSOCommand()
            {
                IPAddress = GetIp(),
                User = User
            }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<IdentityUserResponseDto>(user));
        }

        /// <summary>
        /// Gets an identity user 
        /// </summary>
        /// <param name="email">The users email</param>
        /// <returns>A user if created successfully</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IdentityUserResponseDto), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("")]
        public async Task<ActionResult<IdentityUserResponseDto>> GetIdentityUserAsync([FromQuery] string email)
        {
            var user = await _mediator.Send<MshrmStudioUser>(new GetUserByEmailQuery()
            {
                Email = email,
                RequestingUsersEmail = GetLoggedInUsersUserName(),
                RequestingUsersRole = GetLoggedInUsersRole() 
            }, Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<IdentityUserResponseDto>(user));
        }
    }
}
