using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryContactFormService
    {
        /// <summary>
        /// Get a contact form by Guid ID
        /// </summary>
        /// <param name="guid">The Guid ID of contact form</param>
        /// <param name="callingUser">The person making the request</param>
        /// <param name="role">The person making the requests role</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A contact form</returns>
        Task<ContactFormDto> GetContactFormAsync(Guid guid, DomainUserDto callingUser, RoleType role, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">A search term (content)</param>
        /// <param name="contactEmail">The contacts email</param>
        /// <param name="createdFrom">When the contact form was created from</param>
        /// <param name="createdTo">When the contact form was created to</param>
        /// <param name="callingUser">The user making the request</param>
        /// <param name="callingUsersRole">The user making the requests role</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">Order of results</param>
        /// <param name="stoppingToken">A cancellation token</param>
        /// <returns>A page of contact forms</returns>
        Task<PageResultDtoOfContactFormDto> GetContactFormsAsync(string? searchTerm, string? contactEmail, DateTime? createdFrom, DateTime? createdTo, DomainUserDto callingUser, RoleType callingUsersRole,
            Page page, SortOrder sortOrder, CancellationToken stoppingToken);
    }
}
