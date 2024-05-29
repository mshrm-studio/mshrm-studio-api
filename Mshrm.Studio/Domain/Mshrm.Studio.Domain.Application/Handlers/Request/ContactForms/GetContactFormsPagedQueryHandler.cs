using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
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
    public class GetContactFormsPagedQueryHandler : IRequestHandler<GetContactFormsPagedQuery, PagedResult<ContactForm>>
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContactFormByGuidQueryHandler"/> class.
        /// </summary>
        /// <param name="contactFormRepository"></param>
        /// <param name="tracer"></param>
        public GetContactFormsPagedQueryHandler(IContactFormRepository contactFormRepository, ITracer tracer)
        {
            _contactFormRepository = contactFormRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of contact form
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">Stopping token</param>
        /// <returns>A page of contact forms</returns>
        public async Task<PagedResult<ContactForm>> Handle(GetContactFormsPagedQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetContactFormsAsync_QueryContactFromService").StartActive(true))
            {
                return await _contactFormRepository.GetContactFormsAsync(query.SearchTerm, query.ContactEmail, query.CreatedFrom, query.CreatedTo, new Page(query.PageNumber, query.PerPage),
                    new SortOrder(query.OrderProperty, query.Order), cancellationToken);
            }
        }
    }
}
