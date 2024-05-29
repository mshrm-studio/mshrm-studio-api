using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Email;

namespace Mshrm.Studio.Api.Services.Api
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IAuthClient _authClient;
        private readonly IEmailClient _emailClient;

        public UserAuthorizationService(IEmailClient emailClient, IAuthClient authClient)
        {
            _authClient = authClient;
            _emailClient = emailClient;
        }

        /// <summary>
        /// Get a token using login credentials
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="password">The password</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A token if valid</returns>
        public async Task<TokenDto> GetTokenAsync(string userName, string password, CancellationToken stoppingToken)
        {
            return await _authClient.GenerateTokenAsync(new LoginDto() { UserName = userName, Password = password }, stoppingToken);
        }

        /// <summary>
        /// Generate a refresh token
        /// </summary>
        /// <param name="token">The token to generate from</param>
        /// <param name="refreshToken">The refresh token to check</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A new valid JWT token</returns>
        public async Task<TokenDto> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken stoppingToken)
        {
            return await _authClient.GenerateTokenFromRefreshTokenAsync(new RefreshTokenDto() { TokenValue = token, RefreshTokenValue = refreshToken }, stoppingToken);
        }

        /// <summary>
        /// Update a password
        /// </summary>
        /// <param name="oldPassword">The old password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>An async task</returns>
        public async Task UpdatePasswordAsync(string oldPassword, string newPassword)
        {
            await _authClient.UpdatePasswordAsync(new UpdatePasswordDto() { OldPassword = oldPassword, NewPassword = newPassword });
        }

        /// <summary>
        /// Request a password to be reset
        /// </summary>
        /// <param name="email">The email to request password reset for</param>
        /// <returns>An async task</returns>
        public async Task RequestPasswordResetAsync(string email)
        {
            await _authClient.RequestPasswordResetTokenAsync(new PasswordResetTokenRequestDto() { Email = email });

            return;
        }

        /// <summary>
        /// Reset a password using the token sent via email
        /// </summary>
        /// <param name="email">The email to reset the password for</param>
        /// <param name="token">The token to validate</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>An async task</returns>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var reset = await _authClient.ResetPasswordAsync(new PasswordResetDto() { Email = email, Token = token, NewPassword = newPassword });

            return reset;
        }

        /// <summary>
        /// Confirms a new account using a confirmation token
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="confirmationToken">The confirmation token to validate account with</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>A valid token if confirmed</returns>
        public async Task<TokenDto> ConfirmNewAccountAsync(string email, string confirmationToken, CancellationToken cancellationToken)
        {
            var token = await _authClient.ValidateConfirmationTokenAsync(new ValidateConfirmationDto() { Email = email, ConfirmationToken = confirmationToken });
           
            return token;
        }

        /// <summary>
        /// Resends confirmation token
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>If the confirmation code was resent</returns>
        public async Task<bool> ResendConfirmationTokenAsync(string email, CancellationToken cancellationToken)
        {
            var reset = await _authClient.ResendConfirmationTokenAsync(new ResendConfirmationDto() { Email = email });

            return reset;
        }
    }
}
