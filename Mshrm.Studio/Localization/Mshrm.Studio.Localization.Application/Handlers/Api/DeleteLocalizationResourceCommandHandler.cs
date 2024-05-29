using MediatR;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;

namespace Mshrm.Studio.Localization.Api.Services.Api
{
    public class DeleteLocalizationResourceCommandHandler : IRequestHandler<DeleteLocalizationResourceCommand, bool>
    {
        private readonly ILocalizationRepository _localizationResository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteLocalizationResourceCommandHandler"/> class.
        /// </summary>
        /// <param name="localizationRepository"></param>
        /// <param name="tracer"></param>
        public DeleteLocalizationResourceCommandHandler(ILocalizationRepository localizationRepository, ITracer tracer)
        {
            _localizationResository = localizationRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="query">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public async Task<bool> Handle(DeleteLocalizationResourceCommand query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("DeleteLocalizationResourceAsync_DeleteLocalizationService").StartActive(true))
            {
                var existing = await _localizationResository.GetLocalizationResourceReadOnlyAsync(query.GuidId, cancellationToken);
                if (existing == null)
                {
                    throw new NotFoundException("Localization resource doesn't exist", FailureCode.LocalizationResourceDoesntExist);
                }

                await _localizationResository.DeleteLocalizationResourceAsync(query.GuidId, cancellationToken);

                return true;
            }
        }
    }
}
