using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.ContactForms
{
    public class GetContactFormByGuidQueryHandler : IRequestHandler<GetContactFormByGuidQuery, ContactForm>
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContactFormByGuidQueryHandler"/> class.
        /// </summary>
        /// <param name="contactFormRepository"></param>
        /// <param name="tracer"></param>
        public GetContactFormByGuidQueryHandler(IContactFormRepository contactFormRepository, ITracer tracer)
        {
            _contactFormRepository = contactFormRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a contact for
        /// </summary>
        /// <param name="query">The guid identifier</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A contact form</returns>
        /// <exception cref="NotFoundException">Throw if contact form doesn't exist</exception>
        public async Task<ContactForm> Handle(GetContactFormByGuidQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetContactFormAsync_QueryContactFromService").StartActive(true))
            {
                var contactForm = await _contactFormRepository.GetContactFormByGuidAsync(query.GuidId, cancellationToken);
                if (contactForm == null)
                {
                    throw new NotFoundException("Contact form doesn't exist", FailureCode.ContactFormDoesntExist);
                }

                return contactForm;
            }
        }
    }
}
