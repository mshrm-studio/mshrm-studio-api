using MediatR;
using Mshrm.Studio.Auth.Api.Models.Dtos;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Mshrm.Studio.Auth.Domain.User.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;

namespace Mshrm.Studio.Auth.Api.Handlers
{
    /// <summary>
    /// Service to query users (read only)
    /// </summary>
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, MshrmStudioUser>
    {
        private readonly IIdentityUserService _identityUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByEmailQuery"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        public GetUserByEmailQueryHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        /// <summary>
        /// Get an identity user by email
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>An identity user</returns>
        public async Task<MshrmStudioUser> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            // Get user and exist is found
            var user = await _identityUserService.FindByUserNameAsync(query.Email.ToLower().Trim());
            if (user == null)
                throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);

            // Check the calling user cana see the data
            if (query.RequestingUsersRole != RoleType.Admin && query.RequestingUsersEmail != user.Email)
                throw new ForbidException("Non admin users can only see their own data", FailureCode.CannotViewOtherUsersData);

            // Get user role
            var role = (await _identityUserService.GetRolesAsync(query.Email)).First();

            return new MshrmStudioUser()
            {
                Email = query.Email.ToLower().Trim(),
                Confirmed = user.EmailConfirmed,
                Role = Enum.Parse<RoleType>(role)
            };
        }
    }
}
