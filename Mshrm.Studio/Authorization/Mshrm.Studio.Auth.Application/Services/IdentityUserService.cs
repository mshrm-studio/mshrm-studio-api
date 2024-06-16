using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Linq;
using System.Data;
using Mshrm.Studio.Shared.Models.Options;
using Mshrm.Studio.Shared.Models.Constants;
using MediatR;
using Dapr.Client;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Auth.Domain.Users;
using IdentityModel.Client;

namespace Mshrm.Studio.Auth.Application.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<IdentityUserService> _logger;
        private readonly UserManager<MshrmStudioIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DaprClient _daprClient;
        private readonly IMshrmStudioIdentityUserFactory _mshrmStudioIdentityUserFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">Identity user manager</param>
        /// <param name="roleManager">The role manager</param>
        /// <param name="jwtOptions">JWT options</param>
        /// <param name="logger">Class logger</param>
        public IdentityUserService(UserManager<MshrmStudioIdentityUser> userManager, RoleManager<IdentityRole> roleManager, DaprClient daprClient, IMshrmStudioIdentityUserFactory mshrmStudioIdentityUserFactory,
             IOptions<JwtOptions> jwtOptions, ILogger<IdentityUserService> logger)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _roleManager = roleManager;
            _daprClient = daprClient;
            _mshrmStudioIdentityUserFactory = mshrmStudioIdentityUserFactory;
            _logger = logger;
        }

        /// <summary>
        /// Find users by userName
        /// </summary>
        /// <param name="userName">Username to search by</param>
        /// <returns>Matching user (if any)</returns>
        public async Task<MshrmStudioIdentityUser?> FindByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// If the user is locked out or not
        /// </summary>
        /// <param name="userName">The user to check</param>
        /// <returns>If the user is currently locked out or not</returns>
        public async Task<bool> IsUserLockedoutAsync(string userName)
        {
            // Get existing user
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new DirectoryNotFoundException();

            // Return if the user is lockedout
            return user.LockoutEnd > DateTime.UtcNow;
        }

        /// <summary>
        /// Update users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>Success/Failure</returns>
        public async Task<bool> UpdatePasswordAsync(string email, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Get existing user
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return false;

            // Update password
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result?.Succeeded ?? false;
        }

        /// <summary>
        /// Update users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Success/Failure</returns>
        public async Task<bool> UpdatePasswordAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Get existing user
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return false;

            // Update password by getting reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, password);

            // Return operation outcome
            return result?.Succeeded ?? false;
        }

        /// <summary>
        /// Creates password reset token (sends to email)
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Success/Failure</returns>
        public async Task<string> GetPasswordResetTokenAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return null;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <summary>
        /// Creates password reset token (sends to email)
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Success/Failure</returns>
        public async Task<string> RequestPasswordResetTokenAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return null;

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <summary>
        /// Updates a users password without the need to specify the previous - careful with this call
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="password">The new password to set</param>
        /// <returns>If the password update was successful or not</returns>
        public async Task<bool> ForceUpdatePasswordAsync(string email, string password)
        {
            // Get user to update
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return false;

            // Generate token to reset
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Update the password
            var updatedResult = await _userManager.ResetPasswordAsync(user, resetToken, password);

            // Return
            var errors = updatedResult?.Errors;
            if (errors != null)
                _logger.LogInformation(JsonConvert.SerializeObject(updatedResult?.Errors));

            return updatedResult.Succeeded;
        }

        /// <summary>
        /// Updates a users roles
        /// </summary>
        /// <param name="emails">The users emails to update roles</param>
        /// <param name="role">The role to update to</param>
        /// <returns>If the operation was successful</returns>
        public async Task<bool> UpdateUsersRoleAsync(List<string> emails, RoleType role)
        {
            // Update all the roles
            foreach (var email in emails)
            {
                await UpdateRolesAsync(email, new List<RoleType>() { role });
            }

            // If we reached here, the operation was successful
            return true;
        }

        /// <summary>
        /// Reset a users password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Success/Failure</returns>
        public async Task<ResetPasswordReport> ResetPasswordAsync(string email, string token, string newPassword)
        {
            // Create new report
            var report = new ResetPasswordReport();

            // Basic check - and set to lower
            if (string.IsNullOrEmpty(email))
                return report;

            // See if the user actually exists
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return report;

            // Update the password
            var updatedResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            // Get errors
            report.Errors = updatedResult?.Errors?.ToList();
            if (report.Errors != null)
                _logger.LogInformation(JsonConvert.SerializeObject(updatedResult?.Errors));

            // Mark success
            report.Success = report.Errors == null || !report.Errors.Any();

            return report;
        }

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
        public async Task<MshrmStudioIdentityUser> CreateIdentityUserAsync(string email, string firstName, string lastName, string password, string? ip, List<RoleType> roles, bool confirmed)
        {
            // Create the new user object
            var newUser = _mshrmStudioIdentityUserFactory.CreateMshrmStudioIdentityUser(email, email, confirmed);

            // Try to create the new user
            var createUserResult = await _userManager.CreateAsync(newUser, password);
            if (createUserResult?.Succeeded ?? false)
            {
                var rolesSuccessfullyAdded = await AddRolesToUserAsync(email, roles.ToArray());
                if (rolesSuccessfullyAdded)
                {
                    // Send identity user created event
                    await _daprClient.PublishEventAsync("pubsub", "user-created", new
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Confirmed = confirmed,
                        Ip = ip,
                        Active = true
                    }, CancellationToken.None);

                    return newUser;
                }
            }

            // Build error reason list and log it for out info
            var errors = string.Join(",", createUserResult?.Errors.Select(x => $"[{x.Code}] {x.Description}"));
            _logger.LogError($"Identity user creation failed: {errors}");

            // Return null as nothing was created properly
            return null;
        }

        /// <summary>
        /// Generate a token to confirm account 
        /// </summary>
        /// <param name="email">The account email the confirmation token is for</param>
        /// <returns>The confirmation token</returns>
        public async Task<string?> GenerateAccountConfirmationTokenAsync(string email)
        {
            // Check both user exists and that there are roles to add (not point continuing if not)
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return null;

            // Generate the confirm token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Send email for user confirmation
            await _daprClient.PublishEventAsync("pubsub", "send-email", new
            {
                EmailType = EmailType.AccountConfirmationEmail,
                ToEmailAddress =
                user.Email,
                Link = $"?token={token}"
            });

            return token;
        }

        /// <summary>
        /// Confirm access to an account using a confirmation token
        /// </summary>
        /// <param name="email">The account email the confirmation token is for</param>
        /// <param name="token">The confirmation token</param>
        /// <returns>The confirmation token</returns>
        public async Task<bool> ValidateConfirmationTokenAsync(string email, string token)
        {
            // Check both user exists and that there are roles to add (not point continuing if not)
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return false;

            // Generate the confirm token
            var identityResult = await _userManager.ConfirmEmailAsync(user, token);
            if (identityResult?.Succeeded ?? false)
            {
                return true;
            }

            // If not successful then we log why an return false
            var errors = string.Join(",", identityResult?.Errors.Select(x => $"[{x.Code}] {x.Description}"));
            _logger.LogError($"Identity role addition failed: {errors}");

            return false;
        }

        /// <summary>
        /// Add role/s to a user
        /// </summary>
        /// <param name="userName">The user to associate role with</param>
        /// <param name="roles">The role/s to associate</param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> AddRolesToUserAsync(string userName, params RoleType[] roles)
        {
            // Check both user exists and that there are roles to add (not point continuing if not)
            var user = await FindByUserNameAsync(userName);
            if (user == null || !(roles?.Any() ?? false))
                return false;

            // Add roles and return if all successful
            var identityResult = await _userManager.AddToRolesAsync(user, roles.Select(x => x.ToString()));
            if (identityResult?.Succeeded ?? false)
                return true;

            // If not successful then we log why an return false
            var errors = string.Join(",", identityResult?.Errors.Select(x => $"[{x.Code}] {x.Description}"));
            _logger.LogError($"Identity role addition failed: {errors}");

            return false;
        }

        /// <summary>
        /// Remove an identity user
        /// </summary>
        /// <param name="email">Email of user to remove</param>
        public async Task<bool> RemoveUserAsync(string email)
        {
            // Check both user exists (can't delete one that doesn't)
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return false;

            // Get the result of the deletion (status)
            var identityResult = await _userManager.DeleteAsync(user);

            return identityResult?.Succeeded ?? false;
        }

        /// <summary>
        /// Revokes a users refresh token (deletes from db)
        /// </summary>
        /// <param name="userName">The user to remove from</param>
        /// <returns>Operation outcome</returns>
        public async Task<bool> RevokeRefreshTokenAsync(string userName)
        {
            // Get the user to update
            var user = await FindByUserNameAsync(userName);
            if (user == null)
                return false;

            // Expire the token
            user.RefreshTokenExpiryTime = DateTime.UtcNow;

            // Save and return operation outcome
            var identityResult = await _userManager.UpdateAsync(user);
            return identityResult.Succeeded;
        }

        /// <summary>
        /// Get a list of all platform roles
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoles()
        {
            return _roleManager.Roles
                .Select(x => x.Name)
                .Where(x => x != null)
                .Cast<string>()
                .ToList();
        }

        /// <summary>
        /// Gets a users roles
        /// </summary>
        /// <param name="email">The users email</param>
        /// <returns>The users roles (list of role types)</returns>
        public async Task<List<string>> GetRolesAsync(string email)
        {
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return new List<string>();

            return (List<string>)await _userManager.GetRolesAsync(user) ?? new List<string>();
        }

        #region Helpers

        /// <summary>
        /// Generate authorization claims to add to token
        /// </summary>
        /// <param name="userName">The username/email</param>
        /// <param name="roles">Any roles the user has</param>
        /// <returns>List of token ready claims</returns>
        private List<Claim> GenerateAuthClaims(string userName, List<string>? roles = null)
        {
            // Build claim list from user
            var claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Email, userName)
            };

            // Add role claims if defined
            if (roles?.Any() ?? false)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Return all claims
            return claims;
        }

        /// <summary>
        /// Generates a JWT token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            // Get signing key 
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSigningKey));

            // Build and return token
            return new JwtSecurityToken(

                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    expires: DateTime.UtcNow.AddSeconds(_jwtOptions.Expiry.ParseFrequencyConfig(TimeUnit.Seconds)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        /// <summary>
        /// Update roles for a user
        /// </summary>
        /// <param name="userName">The user</param>
        /// <param name="roles">The new roles</param>
        /// <returns>True if updated</returns>
        private async Task<bool> UpdateRolesAsync(string userName, List<RoleType> roles)
        {
            // Get the user (to check exists)
            var user = await FindByUserNameAsync(userName);
            if (user == null)
                return false;

            // Remove roles
            await RemoveRolesAsync(user);

            // Add new role/s
            await AddRolesToUserAsync(user.UserName, roles.ToArray());

            // Return
            return true;
        }

        private async Task RemoveRolesAsync(MshrmStudioIdentityUser user)
        {
            // Get all users existing roles (so we can remove them)
            var existingRoles = await _userManager.GetRolesAsync(user);

            // Remove existing roles
            foreach (var role in existingRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Replace(" ", string.Empty));
            }
        }

        /// <summary>
        /// Remove a role from a user
        /// </summary>
        /// <param name="email">The user</param>
        /// <param name="role"></param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> RemoveRoleAsync(string email, RoleType role)
        {
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return false;

            return (await _userManager.RemoveFromRoleAsync(user, role.ToString()))?.Succeeded ?? false;
        }

        /// <summary>
        /// Add a role to a user
        /// </summary>
        /// <param name="email">The user</param>
        /// <param name="role"></param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> AddRoleAsync(string email, RoleType role)
        {
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return false;

            return (await _userManager.AddToRoleAsync(user, role.ToString()))?.Succeeded ?? false;
        }

        /// <summary>
        /// Generates a refresh token in Base64
        /// </summary>
        /// <returns>Base64 refresh token</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Generates a refresh token in Base64 and sets in user object (with expiry to +10 days)
        /// </summary>
        /// <returns>Base64 refresh token</returns>
        private async Task<string> GetUserRefreshToken(string userName)
        {
            // Get the user
            var user = await FindByUserNameAsync(userName);

            // Generate a token and set in user obj
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(_jwtOptions.RefreshTokenExpiry.ParseFrequencyConfig(TimeUnit.Seconds));

            // Update the user
            await _userManager.UpdateAsync(user);

            // Return the generated token
            return user.RefreshToken;
        }

        /// <summary>
        /// Gets principle from an expired token (validates)
        /// </summary>
        /// <param name="token">The valid or expired token</param>
        /// <returns>A claim principle (validation of token)</returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtSigningKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateLifetime = false // here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        /// <summary>
        /// Obfuscates an identity users personal information
        /// </summary>
        /// <param name="email">The user to obfuscate</param>
        /// <param name="obfuscationValue">What to put in place of existing data</param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> ObfuscateUserAsync(string email, string obfuscationValue)
        {
            // Basic check
            if (string.IsNullOrEmpty(email))
                return false;

            // Get the user
            var user = await FindByUserNameAsync(email);
            if (user == null)
                return false;

            // Update the personal info
            user.Email = obfuscationValue;
            user.NormalizedEmail = obfuscationValue.ToUpper();
            user.UserName = obfuscationValue.ToUpper();
            user.NormalizedUserName = obfuscationValue.ToUpper();

            // Update the info
            var identityResult = await _userManager.UpdateAsync(user);

            // Return the operation outcome
            return identityResult?.Succeeded ?? false;
        }

        #endregion
    }
}
