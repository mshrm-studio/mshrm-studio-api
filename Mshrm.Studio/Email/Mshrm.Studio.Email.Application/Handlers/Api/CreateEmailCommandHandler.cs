using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Email.Api.Models.Pocos.Email;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Email.Api.Models.Entities.Email.Bases;
using Mshrm.Studio.Email.Api.Repositories.Interfaces;
using OpenTracing;
using Mshrm.Studio.Email.Api.Models.CQRS.Emails.Commands;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Mshrm.Studio.Email.Application.Services.Interfaces;
using Mshrm.Studio.Email.Application.Resources;

namespace Mshrm.Studio.Email.Api.Services.Api
{
    public class CreateEmailCommandHandler : IRequestHandler<CreateEmailCommand, EmailMessage>
    {
        private readonly ISendEmailService _sendEmailService;
        private readonly IEmailMessageRepository _emailMessageRepository;

        private readonly IStringLocalizer<EmailResource> _localizer;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmailCommandHandler"/> class.
        /// </summary>
        /// <param name="sendEmailService"></param>
        /// <param name="emailMessageRepository"></param>
        /// <param name="localizer"></param>
        /// <param name="tracer"></param>
        /// <param name="contextAccessor"></param>
        public CreateEmailCommandHandler(ISendEmailService sendEmailService, IEmailMessageRepository emailMessageRepository, IStringLocalizer<EmailResource> localizer,
            ITracer tracer, IHttpContextAccessor contextAccessor)
        {
            _sendEmailService = sendEmailService;
            _emailMessageRepository = emailMessageRepository;

            _localizer = localizer;
            _contextAccessor = contextAccessor;

            _tracer = tracer;
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The email sent</returns>
        public async Task<EmailMessage> Handle(CreateEmailCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("SendEmailAsync_CreateEmailService").StartActive(true))
            {
                // Get the body/subject
                var emailToSend = GetEmail(command.EmailType, command.ReplaceValues);

                // Send the email
                var sent = _sendEmailService.SendEmail(command.ToEmailAddress, emailToSend.Subject, emailToSend.Body, command.Link);
                if (!sent)
                {
                    throw new UnprocessableEntityException("Failed to send email", FailureCode.FailedToSendEmail);
                }

                // Save a copy
                return await _emailMessageRepository.CreateEmailMessageAsync(command.EmailType, command.ToEmailAddress, emailToSend.Subject, emailToSend.Body, command.Link, cancellationToken);
            }
        }

        #region Helpers

        private EmailBase GetEmail(EmailType emailType, List<KeyValuePair<string, string>> replaceValues)
        {
            switch (emailType)
            {
                // Password reset email
                case EmailType.PasswordReset:
                    return new PasswordResetEmail(_localizer, replaceValues);
                // Account confirmation email - send a code
                case EmailType.AccountConfirmationEmail:
                    return new AccountConfirmationEmail(_localizer, replaceValues);
                // Indicates new email type not yet mapped
                default: throw new NotFoundException("No email type has been registered for the email being sent", FailureCode.NoEmailTypeRegistered);
            }
        }

        #endregion
    }
}
