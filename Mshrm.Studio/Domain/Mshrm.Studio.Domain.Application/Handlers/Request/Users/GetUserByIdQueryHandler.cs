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
using Mshrm.Studio.Domain.Domain.Users;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Users
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public GetUserByIdQueryHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="query">The id to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public async Task<User> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetUserByIdAsync_CreateUserService").StartActive(true))
            {
                var user = await _userRepository.GetUserAsync(query.Id, cancellationToken);
                if (user == null)
                {
                    throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist);
                }

                return user;
            }
        }
    }
}
