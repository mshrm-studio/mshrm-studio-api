using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Models.Dtos.User;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryUserService
    {
        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid to get the user by</param>
        /// <param name="callersEmail">The requesting users email</param>
        /// <param name="callersRole">The requesting users role</param>
        /// <param name="requestAborted">The request token</param>
        /// <returns>A user</returns>
        Task<MshrmStudioUserResponseDto> GetUserAsync(Guid guid, string callersEmail, RoleType callersRole, CancellationToken requestAborted);

        /// <summary>
        /// Get a user using the email from SSO token. Do NOT call this if the emaail is taken from endpoint (security issue)
        /// </summary>
        /// <param name="email">The email to try get the user by</param>
        /// <param name="requestAborted">The request token</param>
        /// <returns>A user</returns>
        Task<MshrmStudioUserResponseDto> GetUserByEmailAsync(string? email, CancellationToken requestAborted);
    }
}
