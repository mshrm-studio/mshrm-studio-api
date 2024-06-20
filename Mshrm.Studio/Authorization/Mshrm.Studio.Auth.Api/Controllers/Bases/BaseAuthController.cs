using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using System.Security.Claims;

namespace Mshrm.Studio.Auth.Api.Controllers.Bases
{
    /// <summary>
    /// base controller for authentication
    /// </summary>
    public class BaseAuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuthController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public BaseAuthController(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets a username from context user is authenticated (and therefore set)
        /// </summary>
        /// <returns>The logged in users username</returns>
        /// <exception cref="NotFoundException">Thrown if user email claim doesn't exist</exception>
        protected string GetLoggedInUsersUserName()
        {
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;
            if(string.IsNullOrEmpty(email))
            {
                throw new NotFoundException("Email not set in claim", FailureCode.EmailNotFound);
            }

            return email;
        }

        /// <summary>
        /// Get the logged in users role from the JWT claims
        /// </summary>
        /// <returns>A users role</returns>
        protected RoleType? GetLoggedInUsersRole()
        {
            var role = Request.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            return string.IsNullOrEmpty(role) ? RoleType.User : Enum.Parse<RoleType>(role);
        }

        /// <summary>
        /// Get the requesting users ID
        /// </summary>
        /// <returns>The requesting users IP</returns>
        protected string? GetIp()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
    }
}
