using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using Order = Mshrm.Studio.Api.Clients.Domain.Order;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryContactFormService : IQueryContactFormService
    {
        private readonly IDomainContactFormClient _client;

        public QueryContactFormService(IDomainContactFormClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Get a contact form by Guid ID
        /// </summary>
        /// <param name="guid">The Guid ID of contact form</param>
        /// <param name="callingUser">The person making the request</param>
        /// <param name="role">The person making the requests role</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A contact form</returns>
        public async Task<ContactFormDto> GetContactFormAsync(Guid guid, DomainUserDto callingUser, RoleType role, CancellationToken cancellationToken)
        {
            return await _client.GetContactFormByGuidAsync(guid);
        }

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">A search term (content)</param>
        /// <param name="contactEmail">The email of the contact</param>
        /// <param name="createdFrom">When the contact form was created from</param>
        /// <param name="createdTo">When the contact form was created to</param>
        /// <param name="callingUser">The user making the request</param>
        /// <param name="callingUsersRole">The user making the requests role</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">Order of results</param>
        /// <param name="stoppingToken">A cancellation token</param>
        /// <returns>A page of contact forms</returns>
        public async Task<PageResultDtoOfContactFormDto> GetContactFormsAsync(string? searchTerm, string? contactEmail, DateTime? createdFrom, DateTime? createdTo, 
            DomainUserDto callingUser, RoleType callingUsersRole, Page page, SortOrder sortOrder, CancellationToken stoppingToken)
        {
            return await _client.GetContactFormsAsync(searchTerm, createdFrom, createdTo, sortOrder.PropertyName, (Order)sortOrder.Order, (int)page.PageNumber, (int)page.PerPage);
        }
    }
}
