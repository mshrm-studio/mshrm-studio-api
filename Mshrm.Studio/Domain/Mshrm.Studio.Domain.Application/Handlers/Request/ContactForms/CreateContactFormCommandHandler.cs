using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Users;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Extensions;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.ContactForms
{
    public class CreateContactFormCommandHandler : IRequestHandler<CreateContactFormCommand, ContactForm>
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContactFormCommandHandler"/> class.
        /// </summary>
        /// <param name="contactFormRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public CreateContactFormCommandHandler(IContactFormRepository contactFormRepository, IUserRepository userRepository, ITracer tracer)
        {
            _contactFormRepository = contactFormRepository;
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Create a contact form
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        public async Task<ContactForm> Handle(CreateContactFormCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateContactFormAsync_CreateContactFormService").StartActive(true))
            {
                return await _contactFormRepository.CreateContactFormAsync(command.Message, command.ContactEmail, cancellationToken);
            }
        }
    }
}
