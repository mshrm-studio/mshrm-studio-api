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

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public DeleteUserCommandHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>True if complete</returns>
        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("DeleteUserCommandHandler").StartActive(true))
            {
                // Check the user doesn't already exist
                var existingUser = await _userRepository.GetUserAsync(command.Guid, cancellationToken);
                if (existingUser == null)
                {
                    throw new UnprocessableEntityException("User doesn't exist", FailureCode.UserDoesntExist, nameof(command.Guid));
                }

                // Add user and return result
                return await _userRepository.DeleteUserAsync(existingUser.Id, cancellationToken);
            }
        }
    }
}
