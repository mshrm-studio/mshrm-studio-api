using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Domain.Users
{
    public interface IUserRepository
    {
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
        public Task<User> CreateUserAsync(string email, string firstName, string lastName, string? ip, bool active,
            CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="ip">Current IP</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The new user</returns>
        public Task<User> UpdateUserAsync(int id, string firstName, string lastName, string? ip,
            CancellationToken cancellationToken);

        /// <summary>
        /// Get a user by Guid
        /// </summary>
        /// <param name="guid">The guid to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public Task<User?> GetUserAsync(Guid guid, CancellationToken cancellationToken);

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">The email to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public Task<User?> GetUserAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">The id to get the user by</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A user</returns>
        public Task<User?> GetUserAsync(int id, CancellationToken cancellationToken);

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
        public Task<PagedResult<User>> GetUsersPagedAsync(string? searchTerm, string? email, string? firstName, string? lastName, string? fullName, Page page,
            SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
