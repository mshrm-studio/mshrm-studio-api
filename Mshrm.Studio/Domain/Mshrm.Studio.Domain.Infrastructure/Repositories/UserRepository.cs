using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Context;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Users;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Api.Repositories
{
    public class UserRepository : BaseRepository<User, MshrmStudioDomainDbContext>, IUserRepository
    {
        private readonly IUserFactory _userFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(MshrmStudioDomainDbContext context, IUserFactory userFactory) : base(context)
        {
            _userFactory = userFactory;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="email">Unique identifier</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="ip">Current IP</param>
        /// <param name="active">The state of the user</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The new user</returns>
        public async Task<User> CreateUserAsync(string email, string firstName, string lastName, string? ip, bool active,
            CancellationToken cancellationToken)
        {
            var newUser = _userFactory.CreateUser(email, firstName, lastName, ip, active);

            Add(newUser);
            await SaveAsync(cancellationToken);

            return newUser;
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="ip">Current IP</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The new user</returns>
        public async Task<User> UpdateUserAsync(int id, string firstName, string lastName, string? ip,
            CancellationToken cancellationToken)
        {
            // Get existing user
            var existingUser = GetAll().FirstOrDefault(x => x.Id == id);

            // Update
            existingUser.UpdateIp(ip);
            existingUser.UpdateName(firstName, lastName);

            Update(existingUser);
            await SaveAsync(cancellationToken);

            return existingUser;
        }

        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public async Task<User?> GetUserAsync(Guid guid, CancellationToken cancellationToken)
        {
            return await GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuidId == guid);
        }

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">The email to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public async Task<User?> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var emailToSearch = email?.ToLower()?.Trim();

            return await GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.ToLower().Trim() == emailToSearch);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">The id to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public async Task<User?> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            return await GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get a page of users
        /// </summary>
        /// <param name="searchTerm">A search term</param>
        /// <param name="email">The email</param>
        /// <param name="firstName">A first name</param>
        /// <param name="lastName">A last name</param>
        /// <param name="fullName">The full name</param>
        /// <param name="page">What page to return</param>
        /// <param name="sortOrder">What order to return results in</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Page of users</returns>
        public async Task<PagedResult<User>> GetUsersPagedAsync(string? searchTerm, string? email, string? firstName, string? lastName, string? fullName, Page page,
            SortOrder sortOrder, CancellationToken cancellationToken)
        {
            // Get the set ref
            var users = GetAll();

            // Filter by a search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var sanitizedSerachTerm = searchTerm.ToLower().Trim();
                users = users.Where(x => x.Email.ToLower().Contains(sanitizedSerachTerm) ||
                        x.FirstName.ToLower().Contains(sanitizedSerachTerm) ||
                        x.LastName.ToLower().Contains(sanitizedSerachTerm) ||
                        (x.FirstName + " " + x.LastName).ToLower().Contains(sanitizedSerachTerm));
            }

            // Filter by email
            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(x => x.Email.ToLower().Contains(email.ToLower()));
            }

            // Filter by first name
            if (!string.IsNullOrEmpty(firstName))
            {
                users = users.Where(x => x.FirstName.ToLower().Contains(firstName.ToLower()));
            }

            // Filter by last name
            if (!string.IsNullOrEmpty(lastName))
            {
                users = users.Where(x => x.LastName.ToLower().Contains(lastName.ToLower()));
            }

            // Filter by full name
            if (!string.IsNullOrEmpty(fullName))
            {
                users = users.Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(fullName.ToLower()));
            }

            // Order 
            users = OrderSet(users, sortOrder);

            // Enumerate page
            var returnPage = await users.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<User>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = users.Count()
            };
        }


        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<User> OrderSet(IQueryable<User> set, SortOrder sortOrder)
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
