using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using OpenTracing;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries;
using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Mshrm.Studio.Domain.Domain.Users;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Users
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public GetUserByEmailQueryHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="query">The email to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public async Task<User> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetUserByEmailUserAsync_CreateUserService").StartActive(true))
            {
                var user = await _userRepository.GetUserAsync(query.Email, cancellationToken);
                if (user == null)
                {
                    throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);
                }

                return user;
            }
        }
    }
}
