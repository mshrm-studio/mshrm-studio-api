using Microsoft.AspNetCore.Identity;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mshrm.Studio.Auth.Application.Services.Interfaces
{
    public interface IIdentityUserService
    {
        /// <summary>
        /// Find users by userName
        /// </summary>
        /// <param name="userName">Username to search by</param>
        /// <returns>Matching user (if any)</returns>
        public Task<MshrmStudioIdentityUser?> FindByUserNameAsync(string userName);

        /// <summary>
        /// Generate token for user (login)
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public Task<Token> RequestTokenAsync(string userName, string password);

        /// <summary>
        /// If the user is locked out or not
        /// </summary>
        /// <param name="userName">The user to check</param>
        /// <returns>If the user is currently locked out or not</returns>
        public Task<bool> IsUserLockedoutAsync(string userName);

        /// <summary>
        /// Update users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>Success/Failure</returns>
        public Task<bool> UpdatePasswordAsync(string email, string oldPassword, string newPassword);

        /// <summary>
        /// Update users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Success/Failure</returns>
        public Task<bool> UpdatePasswordAsync(string email, string password);

        /// <summary>
        /// Creates password reset token (sends to email)
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Success/Failure</returns>
        public Task<string> GetPasswordResetTokenAsync(string email);

        /// <summary>
        /// Creates password reset token (sends to email)
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Success/Failure</returns>
        public Task<string> RequestPasswordResetTokenAsync(string email);

        /// <summary>
        /// Updates a users password without the need to specify the previous - careful with this call
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="password">The new password to set</param>
        /// <returns>If the password update was successful or not</returns>
        public Task<bool> ForceUpdatePasswordAsync(string email, string password);

        /// <summary>
        /// Updates a users roles
        /// </summary>
        /// <param name="emails">The users emails to update roles</param>
        /// <param name="role">The role to update to</param>
        /// <returns>If the operation was successful</returns>
        public Task<bool> UpdateUsersRoleAsync(List<string> emails, RoleType role);

        /// <summary>
        /// Reset a users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Success/Failure</returns>
        public Task<ResetPasswordReport> ResetPasswordAsync(string email, string token, string newPassword);

        /// <summary>
        /// Creates a new identity user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="roles"></param>
        /// <param name="confirmed">If the user is confirmed</param>
        /// <returns></returns>
        public Task<MshrmStudioIdentityUser> CreateIdentityUserAsync(string email, string firstName, string lastName, string password, string? ip, List<RoleType> roles, bool confirmed);

        /// <summary>
        /// Get a token to confirm account
        /// </summary>
        /// <param name="email">The account email the confirmation token is for</param>
        /// <returns>The confirmation token</returns>
        public Task<string?> GenerateAccountConfirmationTokenAsync(string email);

        /// <summary>
        /// Confirm access to an account using a confirmation token
        /// </summary>
        /// <param name="email">The account email the confirmation token is for</param>
        /// <param name="token">The confirmation token</param>
        /// <returns>The confirmation token</returns>
        public Task<bool> ValidateConfirmationTokenAsync(string email, string token);

        /// <summary>
        /// Add role/s to a user
        /// </summary>
        /// <param name="userName">The user to associate role with</param>
        /// <param name="roles">The role/s to associate</param>
        /// <returns>The operation outcome</returns>
        public Task<bool> AddRolesToUserAsync(string userName, params RoleType[] roles);

        /// <summary>
        /// Remove an identity user
        /// </summary>
        /// <param name="email">Email of user to remove</param>
        public Task<bool> RemoveUserAsync(string email);

        /// <summary>
        /// Revokes a users refresh token (deletes from db)
        /// </summary>
        /// <param name="userName">The user to remove from</param>
        /// <returns>Operation outcome</returns>
        public Task<bool> RevokeRefreshTokenAsync(string userName);

        /// <summary>
        /// Build a access token (and refresh)
        /// </summary>
        /// <param name="userName">The user to build it for</param>
        /// <returns>A set of tokens</returns>
        public Task<Token> BuildToken(string userName);

        /// <summary>
        /// Get a list of all platform roles
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoles();

        /// <summary>
        /// Gets a users roles
        /// </summary>
        /// <param name="email">The users email</param>
        /// <returns>The users roles (list of role types)</returns>
        public Task<List<string>> GetRolesAsync(string email);

        /// <summary>
        /// Gets principle from an expired token (validates)
        /// </summary>
        /// <param name="token">The valid or expired token</param>
        /// <returns>A claim principle (validation of token)</returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
