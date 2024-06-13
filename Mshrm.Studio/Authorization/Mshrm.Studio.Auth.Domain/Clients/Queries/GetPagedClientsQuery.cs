using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Clients.Queries
{
    public class GetPagedClientsQuery : IRequest<PagedResult<Client>>
    {
        public string? SearchTerm;
        public string? ClientName { get; set; }
        public string? ClientId { get; set; }
        public Order Order { get; set; }
        public string? OrderProperty { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
