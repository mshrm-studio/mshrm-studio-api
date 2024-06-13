using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.ApiResources.Queries
{
    public class GetPagedApiScopesQuery : IRequest<PagedResult<ApiScope>>
    {
        public string? SearchTerm;
        public string? Name { get; set; }
        public Order Order { get; set; }
        public string? OrderProperty { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
