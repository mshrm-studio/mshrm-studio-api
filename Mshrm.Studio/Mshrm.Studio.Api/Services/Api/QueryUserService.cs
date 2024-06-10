using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System;
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.User;
using Mshrm.Studio.Shared.Exceptions.HttpAction;

namespace Mshrm.Studio.Api.Services.Api
{
    /// <summary>
    /// For querying users
    /// </summary>
    public class QueryUserService : IQueryUserService
    {
        private readonly IDomainUserClient _domainUserClient;
        private readonly IIdentityUserClient _identityUserClient;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryUserService"/> class.
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="domainUserClient"></param>
        /// <param name="identityUserClient"></param>
        public QueryUserService(IHttpContextAccessor contextAccessor, IDomainUserClient domainUserClient, IIdentityUserClient identityUserClient)
        {
            _domainUserClient = domainUserClient;
            _identityUserClient = identityUserClient;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid to get the user by</param>
        /// <param name="callersEmail">The requesting users email</param>
        /// <param name="callersRole">The requesting users role</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A user</returns>
        public async Task<MshrmStudioUserResponseDto> GetUserAsync(Guid guid, string callersEmail, RoleType callersRole, CancellationToken stoppingToken)
        {
            // Get the user
            var user = await _domainUserClient.GetUserByGuidAsync(guid, stoppingToken);
            if(user != null)
            {
                // Check if the caller if NOT admin that it is their data
                if (callersRole != RoleType.Admin && callersEmail.ToLower().Trim() != user.Email)
                {
                    throw new ForbidException("Non admin users can only see their own data", FailureCode.CannotViewOtherUsersData);
                }

                // get the identity user to aggregate
                var identityUser = await _identityUserClient.GetIdentityUserAsync(user.Email);

                return new MshrmStudioUserResponseDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    FullName = user.FullName,
                    GuidId = user.GuidId,
                    RoleType = identityUser?.Role ?? RoleType.User,
                    Confirmed = identityUser?.Confirmed ?? true,
                };
            }

            throw new NotFoundException("user doesn't exist", FailureCode.UserDoesntExist);
        }

        /// <summary>
        /// Get a user using the email from SSO token. Do NOT call this if the emaail is taken from endpoint (security issue)
        /// </summary>
        /// <param name="email">The email to try get the user by</param>
        /// <param name="stoppingToken">The request token</param>
        /// <returns>A user</returns>
        public async Task<MshrmStudioUserResponseDto> GetUserByEmailAsync(string? email, CancellationToken stoppingToken)
        {
            // Check we have an email
            if (string.IsNullOrEmpty(email))
                throw new UnprocessableEntityException("Email is invalid", FailureCode.EmailIsInvalid);

            // Get user by it
            var user = await _domainUserClient.GetUserByEmailAsync(email.ToLower().Trim(), stoppingToken);
            if (user != null)
            {
                // get the identity user to aggregate
                var identityUser = await _identityUserClient.GetIdentityUserAsync(user.Email);

                return new MshrmStudioUserResponseDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    FullName = user.FullName,
                    GuidId = user.GuidId,
                    RoleType = identityUser?.Role ?? RoleType.User,
                    Confirmed = identityUser?.Confirmed ?? true,
                };
            }

            throw new NotFoundException("user doesn't exist", FailureCode.UserDoesntExist);
        }
    }
}
