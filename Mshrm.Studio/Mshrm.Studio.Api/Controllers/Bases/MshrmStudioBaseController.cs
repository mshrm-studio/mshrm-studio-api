using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Constants;
using System.Security.Claims;

namespace Mshrm.Studio.Api.Controllers.Bases
{
    /// <summary>
    /// Base API
    /// </summary>
    public abstract class MshrmStudioBaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDomainUserClient _domainUserClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MshrmStudioBaseController"/> class.
        /// </summary>
        /// <param name="domainUserClient"></param>
        /// <param name="contextAccessor"></param>
        public MshrmStudioBaseController(IDomainUserClient domainUserClient, IHttpContextAccessor contextAccessor)
        {
            _domainUserClient = domainUserClient;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get the requesting users ID
        /// </summary>
        /// <returns>The requesting users IP</returns>
        protected string? GetIp()
        {
            return _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Get the logged in users role from the JWT claims
        /// </summary>
        /// <returns>A users role</returns>
        protected RoleType GetUserRole()
        {
            var role = Request.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                throw new NotFoundException("Role doesn't exist", FailureCode.RoleDoesntExist);
            }

            return Enum.Parse<RoleType>(role);
        }

        /// <summary>
        /// Get the logged in user from the JWT claims
        /// </summary>
        /// <returns>A user ID</returns>
        /// <exception cref="NotFoundException">Thrown if user is not found</exception>
        protected async Task<DomainUserDto> GetUserAsync()
        {
            // Try get userid from claim
            var rawUserId = Request.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimNames.UserId)?.Value;
            if (string.IsNullOrEmpty(rawUserId))
            {
                // If comes from SSO - get the email
                var email = Request.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                if(!string.IsNullOrEmpty(email))
                {
                    // Get the user from domain by the email
                    var user = await _domainUserClient.GetUserByEmailAsync(email);
                    if(user == null)
                    {
                        throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);
                    }

                    // Return the ID
                    return user;
                }

                throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);
            }
            else
            {
                // Parse 
                var userId = int.Parse(rawUserId);
                var user = await _domainUserClient.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);
                }

                return user;
            }
        }
    }
}
