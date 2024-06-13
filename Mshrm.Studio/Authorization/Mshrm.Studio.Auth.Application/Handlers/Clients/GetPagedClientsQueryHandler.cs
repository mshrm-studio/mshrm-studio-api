using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Handlers.Clients
{
    public class GetPagedClientsQueryHandler : IRequestHandler<GetPagedClientsQuery, PagedResult<Client>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITracer _tracer;

        public GetPagedClientsQueryHandler(IClientRepository clientRepository, ITracer tracer)
        {
            _clientRepository = clientRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of clients
        /// </summary>
        /// <param name="query">The query data</param>
        /// <returns>A page of clients</returns>
        public async Task<PagedResult<Client>> Handle(GetPagedClientsQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetPagedClientsQueryHandler").StartActive(true))
            {
                var clients = await _clientRepository.GetClientsPagedAsync(query.SearchTerm, query.ClientId, query.ClientName,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                return clients;
            }
        }
    }
}
