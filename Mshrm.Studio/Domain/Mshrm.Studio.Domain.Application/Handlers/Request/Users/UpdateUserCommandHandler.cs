using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using OpenTracing;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands;
using MediatR;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Mshrm.Studio.Domain.Domain.Users;
using Mshrm.Studio.Auth.Api.Models.Enums;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public UpdateUserCommandHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Update and existing user
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The updated user</returns>
        public async Task<User> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("UpdateUserCommandHandler").StartActive(true))
            {
                // Check the user doesn't already exist
                var existingUser = await _userRepository.GetUserAsync(command.UserId, cancellationToken);
                if (existingUser == null)
                {
                    throw new NotFoundException("User doesn't exist", FailureCode.UserDoesntExist, nameof(command.Email));
                }

                // Only admins can update other peoples user profiles
                if((command.CallingUsersRoleType == RoleType.User) && (existingUser.Email != command.CallingUsersEmail))
                {
                    throw new ForbidException("Cann view other users data", FailureCode.CannotViewOtherUsersData);
                }

                // Add user and return result
                return await _userRepository.UpdateUserAsync(existingUser.Id, command.FirstName, command.LastName, command.Ip, cancellationToken);
            }
        }
    }
}
