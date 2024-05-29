using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IUserAuthorizationService
    {
        /// <summary>
        /// Generate a refresh token
        /// </summary>
        /// <param name="token">The token to generate from</param>
        /// <param name="refreshToken">The refresh token to check</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A new valid JWT token</returns>
        Task<TokenDto> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken stoppingToken);

        /// <summary>
        /// Get a token using login credentials
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A token if valid</returns>
        Task<TokenDto> GetTokenAsync(string userName, string password, CancellationToken stoppingToken);

        /// <summary>
        /// Update a password
        /// </summary>
        /// <param name="oldPassword">The old password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>An async task</returns>
        Task UpdatePasswordAsync(string oldPassword, string newPassword);

        /// <summary>
        /// Request a password to be reset
        /// </summary>
        /// <param name="email">The email to request password reset for</param>
        /// <returns>An async task</returns>
        Task RequestPasswordResetAsync(string email);

        /// <summary>
        /// Reset a password using the token sent via email
        /// </summary>
        /// <param name="email">The email to reset the password for</param>
        /// <param name="token">The token to validate</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>An async task</returns>
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);

        /// <summary>
        /// Confirms a new account using a confirmation token
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="confirmationToken">The confirmation token to validate account with</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>A valid token if confirmed</returns>
        Task<TokenDto> ConfirmNewAccountAsync(string email, string confirmationToken, CancellationToken cancellationToken);

        /// <summary>
        /// Resends confirmation token
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>If the confirmation code was resent</returns>
        Task<bool> ResendConfirmationTokenAsync(string email, CancellationToken cancellationToken);
    }
}
