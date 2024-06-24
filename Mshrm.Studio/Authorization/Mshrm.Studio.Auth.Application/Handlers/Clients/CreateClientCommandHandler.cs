using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Domain.User.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;

namespace Mshrm.Studio.Auth.Application.Handlers.Clients
{
    /// <summary>
    /// Handles a create new client command
    /// </summary>
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientWithSecret>
    {
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateClientCommandHandler"/> class.
        /// </summary>
        /// <param name="clientRepository"></param>
        public CreateClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>A client</returns>
        public async Task<ClientWithSecret> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var existingClient = await _clientRepository.GetClientByClientIdAsync(command.IdName, cancellationToken);
            if (existingClient != null)
            {
                throw new UnprocessableEntityException("Client id needs to be unique", FailureCode.ClientIdShouldBeUnique);
            }

            (var client, var secret) = await _clientRepository.CreateClientAsync(command.IdName, command.ClientName, command.GrantTypes, command.Scopes, command.RedirectUris);

            return new ClientWithSecret()
            {
                Client = client,
                PlainTextSecret = secret
            };
        }
    }
}
