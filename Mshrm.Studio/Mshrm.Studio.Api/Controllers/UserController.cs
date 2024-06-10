using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Controllers.Bases;
using Mshrm.Studio.Api.Models;
using Mshrm.Studio.Api.Models.Dtos.User;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Enums;
using System.Security.Claims;

namespace Mshrm.Studio.Api.Controllers
{
    /// <summary>
    /// User API
    /// </summary>
    [ApiController]
    [FormatFilter]
    [Route("api/v1/users")]
    public class UserController : MshrmStudioBaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        private readonly IQueryUserService _readOnlyUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="readOnlyUserService"></param>
        /// <param name="domainUserClient"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public UserController(IQueryUserService readOnlyUserService, IDomainUserClient domainUserClient, IHttpContextAccessor contextAccessor, 
            IMapper mapper, ILogger<UserController> logger) :
            base(domainUserClient, contextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _readOnlyUserService = readOnlyUserService;
        }

        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid identifier</param>
        /// <returns>A user if exists</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(MshrmStudioUserResponseDto), StatusCodes.Status200OK)]
        [Route("guid/{guid}")]
        public async Task<ActionResult<MshrmStudioUserResponseDto?>> GetUserAsync([FromRoute] Guid guid)
        {
            // Get user
            var user = await _readOnlyUserService.GetUserAsync(guid, (await GetUserAsync()).Email, GetUserRole(), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<MshrmStudioUserResponseDto?>(user));
        }

        /// <summary>
        /// Gets the callers user
        /// </summary>
        /// <returns>A user</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(MshrmStudioUserResponseDto), StatusCodes.Status200OK)]
        [Route("")]
        public async Task<ActionResult<MshrmStudioUserResponseDto?>> GetLoggedInUserAsync()
        {
            // Get user
            var user = await _readOnlyUserService.GetUserByEmailAsync(User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.NameIdentifier), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(_mapper.Map<MshrmStudioUserResponseDto?>(user)); // TODO: change to get user from token
        }
    }
}
