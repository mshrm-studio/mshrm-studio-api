using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Context;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.ContactForms;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Api.Repositories
{
    /// <summary>
    /// Repository for contaact forms
    /// </summary>
    public class ContactFormRepository : BaseRepository<ContactForm, MshrmStudioDomainDbContext>, IContactFormRepository
    {
        private readonly IContactFormFactory _contactFormFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ContactFormRepository(MshrmStudioDomainDbContext context, IContactFormFactory contactFormFactory) : base(context)
        {
            _contactFormFactory = contactFormFactory;
        }

        /// <summary>
        /// Gets all items from context - is overrideable
        /// </summary>
        /// <returns>List of items</returns>
        public override IQueryable<ContactForm> GetAll(string? tableName = null)
        {
            return base.GetAll(tableName);
        }

        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="message">The message in the contact form</param>
        /// <param name="contactEmail">A contact email</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        public async Task<ContactForm> CreateContactFormAsync(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<Guid> attachmentGuidIds,
            CancellationToken cancellationToken)
        {
            // Create form
            var contactForm = _contactFormFactory.CreateContactForm(message, contactEmail, firstName, lastName, websiteUrl, attachmentGuidIds);

            // Save
            Add(contactForm);
            await SaveAsync(cancellationToken);

            // Return
            return contactForm;
        }

        /// <summary>
        /// Get a contact form by its guid id
        /// </summary>
        /// <param name="guid">The guid to get the contact form by</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A contact form</returns>
        public async Task<ContactForm?> GetContactFormByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            return await GetAll()
                .Where(x => x.GuidId == guid)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get a page of contact forms
        /// </summary>
        /// <param name="searchTerm">The search term (searches message)</param>
        /// <param name="email">The contact email</param>
        /// <param name="createdFrom">Returns all contact forms after this date</param>
        /// <param name="createdTo">Returns all contact forms before this date</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The sort order of the set</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of contact forms</returns>
        public async Task<PagedResult<ContactForm>> GetContactFormsAsync(string? searchTerm, string? email, DateTime? createdFrom, DateTime? createdTo,
            Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            // Get the set ref
            var contactForms = GetAll();

            // Filter by a search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contactForms = contactForms.Where(x => x.Message.ToLower().Contains(searchTerm.ToLower().Trim()));
            }

            // Filter by user
            if (!string.IsNullOrEmpty(email))
            {
                contactForms = contactForms.Where(x => x.ContactEmail.ToLower() == email.ToLower().Trim());
            }

            // Filter by created from date
            if (createdFrom.HasValue)
            {
                contactForms = contactForms.Where(x => x.CreatedDate >= createdFrom);
            }

            // Filter by created to date
            if (createdTo.HasValue)
            {
                contactForms = contactForms.Where(x => x.CreatedDate <= createdTo);
            }

            // Order 
            contactForms = OrderSet(contactForms, sortOrder);

            // Enumerate page
            var returnPage = await contactForms.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<ContactForm>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = contactForms.Count()
            };
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<ContactForm> OrderSet(IQueryable<ContactForm> set, SortOrder sortOrder)
        {
            return (sortOrder.PropertyName.Trim(), sortOrder.Order) switch
            {
                ("createdDate", Order.Ascending) => set.OrderBy(x => x.CreatedDate),
                ("createdDate", Order.Descending) => set.OrderByDescending(x => x.CreatedDate),
                _ => sortOrder.Order == Order.Descending ? set.OrderBy(x => x.CreatedDate) : set.OrderByDescending(x => x.CreatedDate)
            };
        }

        #endregion
    }
}
