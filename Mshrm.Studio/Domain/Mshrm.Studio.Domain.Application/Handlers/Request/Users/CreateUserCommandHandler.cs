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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public CreateUserCommandHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The new user</returns>
        public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateUserAsync_CreateUserService").StartActive(true))
            {
                var email = command.Email.ToLower().Trim();

                // Check the user doesn't already exist
                var existingUser = await _userRepository.GetUserAsync(email, cancellationToken);
                if (existingUser != null)
                {
                    throw new UnprocessableEntityException("User already exists", FailureCode.UserAlreadyExists, nameof(email));
                }

                // Add user and return result
                return await _userRepository.CreateUserAsync(email, command.FirstName, command.LastName, command.Ip, true, cancellationToken);
            }
        }
    }
}
