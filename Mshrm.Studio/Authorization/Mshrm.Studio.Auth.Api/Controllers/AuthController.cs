using AutoMapper;
using Dapr;
using Dapr.Client;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Auth.Api.Controllers.Bases;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Application.Dtos.Users;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using System.Threading;

namespace Mshrm.Studio.Auth.Api.Controllers
{
    /// <summary>
    /// Authorization API (token management)
    /// </summary>
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : BaseAuthController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public AuthController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<AuthController> logger, IMapper mapper) : base(httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;

            _mediator = mediator;
        }

        /// <summary>
        /// Updates a users password using the existing one
        /// </summary>
        /// <param name="model">Password update method</param>
        /// <returns>Ok</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("password")]
        public async Task<ActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordRequestDto model)
        {
            // Build commmand
            var command = _mapper.Map<UpdatePasswordCommand>(model);
            command.Email = GetLoggedInUsersUserName();

            await _mediator.Send(command, Request.HttpContext.RequestAborted);

            // Return token
            return Ok();
        }

        /// <summary>
        /// Request a reset token for resetting a users password
        /// </summary>
        /// <param name="model">The reset data</param>
        /// <returns>Ok</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("password/reset/token")]
        public async Task<ActionResult> RequestPasswordResetTokenAsync([FromBody] PasswordResetTokenRequestDto model)
        {
            await _mediator.Send(_mapper.Map<CreatePasswordResetTokenCommand>(model), Request.HttpContext.RequestAborted);

            // Return token
            return Ok();
        }

        /// <summary>
        /// Resets a users password
        /// </summary>
        /// <param name="model">The reset data</param>
        /// <returns>Ok</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("password/reset")]
        public async Task<ActionResult<bool>> ResetPasswordAsync([FromBody] PasswordResetRequestDto model)
        {
            var outcome = await _mediator.Send<bool>(_mapper.Map<ResetPasswordCommand>(model), Request.HttpContext.RequestAborted); 

            // Return token
            return Ok(outcome);
        }

        /// <summary>
        /// Validates a confirmation token for a new user and returns a login token if valid
        /// </summary>
        /// <param name="model">The users email and confirmation token</param>
        /// <returns>A JWT token for login if valid</returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("confirmation/validate")]
        public async Task<ActionResult<bool>> ValidateConfirmationTokenAsync([FromBody] ValidateConfirmationRequestDto model)
        {
            var confirmed = await _mediator.Send<bool>(_mapper.Map<ValidateUserConfirmationCommand>(model), Request.HttpContext.RequestAborted);

            return Ok(confirmed);
        }

        /// <summary>
        /// Resends confirmation token
        /// </summary>
        /// <param name="model">The users email</param>
        /// <returns>If the confirmation code was resent</returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [Route("confirmation")]
        public async Task<ActionResult<bool>> ResendConfirmationTokenAsync([FromBody] ResendConfirmationRequestDto model)
        {
            var resent = await _mediator.Send<bool>(_mapper.Map<ResendUserConfirmationCommand>(model), Request.HttpContext.RequestAborted);

            // Map and return
            return Ok(resent);
        }
    }
}
