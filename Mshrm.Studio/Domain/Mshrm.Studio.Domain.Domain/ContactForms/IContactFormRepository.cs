using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository for contact forms
    /// </summary>
    public interface IContactFormRepository
    {
        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="message">The message in the contact form</param>
        /// <param name="contactEmail">A contact email</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        public Task<ContactForm> CreateContactFormAsync(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<Guid> attachmentGuidIds, 
            CancellationToken cancellationToken);

        /// <summary>
        /// Get a contact form by its guid id
        /// </summary>
        /// <param name="guid">The guid to get the contact form by</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A contact form</returns>
        public Task<ContactForm?> GetContactFormByGuidAsync(Guid guid, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">The search term (searches message)</param>
        /// <param name="userId">The users guid (who created the contact form</param>
        /// <param name="createdFrom">Returns all contact forms after this date</param>
        /// <param name="createdTo">Returns all contact forms before this date</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The sort order of the set</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of contact forms</returns>
        public Task<PagedResult<ContactForm>> GetContactFormsAsync(string? searchTerm, string? email, DateTime? createdFrom, DateTime? createdTo,
            Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
