using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Auth.Domain.ApiResources;
using Mshrm.Studio.Auth.Domain.ApiResources.Commands;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Handlers.ApiResources
{
    public class CreateApiScopeCommandHandler : IRequestHandler<CreateApiScopeCommand, ApiScope>
    {
        private readonly IApiScopeRepository _apiResourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateApiScopeCommandHandler"/> class.
        /// </summary>
        /// <param name="apiResourceRepository"></param>
        public CreateApiScopeCommandHandler(IApiScopeRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        /// <summary>
        /// Create a new api scope
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>An api scope</returns>
        public async Task<ApiScope> Handle(CreateApiScopeCommand command, CancellationToken cancellationToken)
        {
            var existingApiScope = await _apiResourceRepository.GetApiScopeByNameAsync(command.Name, cancellationToken);
            if (existingApiScope != null)
            {
                throw new UnprocessableEntityException("API scope name needs to be unique", FailureCode.ApiResourceNameShouldBeUnique);
            }

            return await _apiResourceRepository.CreateApiScopeAsync(command.Name, cancellationToken);
        }
    }
}
