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
    public class CreateApiResourceCommandHandler : IRequestHandler<CreateApiResourceCommand, ApiResourceWithSecret>
    {
        private readonly IApiResourceRepository _apiResourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateApiResourceCommandHandler"/> class.
        /// </summary>
        /// <param name="apiResourceRepository"></param>
        public CreateApiResourceCommandHandler(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        /// <summary>
        /// Create a new api resource
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>An api resource</returns>
        public async Task<ApiResourceWithSecret> Handle(CreateApiResourceCommand command, CancellationToken cancellationToken)
        {
            var existingApiResource = await _apiResourceRepository.GetApiResourceByNameAsync(command.Name, cancellationToken);
            if (existingApiResource != null)
            {
                throw new UnprocessableEntityException("API resource name needs to be unique", FailureCode.ApiResourceNameShouldBeUnique);
            }

            (var apiResource, var secret) = await _apiResourceRepository.CreateApiResourceAsync(command.Name, cancellationToken);

            return new ApiResourceWithSecret()
            {
                ApiResource = apiResource,
                PlainTextSecret = secret
            };
        }
    }
}
