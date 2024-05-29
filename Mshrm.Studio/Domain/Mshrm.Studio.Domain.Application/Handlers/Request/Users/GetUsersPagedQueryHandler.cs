using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Users;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Users
{
    public class GetUsersPagedQueryHandler : IRequestHandler<GetUsersPagedQuery, PagedResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tracer"></param>
        public GetUsersPagedQueryHandler(IUserRepository userRepository, ITracer tracer)
        {
            _userRepository = userRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of users
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Paged list of users</returns>
        public async Task<PagedResult<User>> Handle(GetUsersPagedQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetUserPagedAsync_GetUsersPagedQueryHandler").StartActive(true))
            {
                var users = await _userRepository.GetUsersPagedAsync(query.SearchTerm, query.Email, query.FirstName, query.LastName, query.FullName,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                return users;
            }
        }
    }
}
