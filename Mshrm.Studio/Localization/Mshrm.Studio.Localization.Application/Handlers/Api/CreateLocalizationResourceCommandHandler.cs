using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Localization.Api.Models.Constants;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Services.Interfaces;
using OpenTracing;

namespace Mshrm.Studio.Localization.Api.Services.Api
{
    public class CreateLocalizationResourceCommandHandler : IRequestHandler<CreateLocalizationResourceCommand, LocalizationResource>
    {
        private readonly ILocalizationRepository _localizationRepository;
        private readonly ICacheService _cacheService;
        private readonly ITracer _tracer;

        private readonly RequestLocalizationOptions _requestLocalizationOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateLocalizationResourceCommandHandler"/> class.
        /// </summary>
        /// <param name="localizationRepository"></param>
        /// <param name="cacheService"></param>
        /// <param name="tracer"></param>
        public CreateLocalizationResourceCommandHandler(ILocalizationRepository localizationRepository, ICacheService cacheService, ITracer tracer, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            _localizationRepository = localizationRepository;
            _cacheService = cacheService;

            _tracer = tracer;
            _requestLocalizationOptions = requestLocalizationOptions.Value;
        }

        /// <summary>
        /// Create a new localization resource
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new localization resource</returns>
        public async Task<LocalizationResource> Handle(CreateLocalizationResourceCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateLocalizationResourceAsync_CreateLocalizationService").StartActive(true))
            {
                var existingResources = await _localizationRepository.GetLocalizationResourcesReadOnlyAsync(command.LocalizationArea, command.Culture, command.Key, cancellationToken);
                if (existingResources.Any())
                {
                    throw new UnprocessableEntityException("Localization resource already exists", FailureCode.LocalizationResourceAlreadyExists);
                }

                // Validate culture
                if(!_requestLocalizationOptions.SupportedCultures.Select(x => x.Name).Contains(command.Culture))
                {
                    throw new BadRequestException("Culture not supported", FailureCode.CultureNotSupported, nameof(command.Culture));
                }

                // Validate name/key
                switch (command.LocalizationArea)
                {
                    case LocalizationArea.Errors:
                        // Validate failure code
                        var isValidFailureCode = Enum.TryParse<FailureCode>(command.Key, true, out var result);
                        if(!isValidFailureCode)
                        {
                            throw new UnprocessableEntityException("Failure code is invalid", FailureCode.FailureCodeIsInvalid, nameof(command.Key));
                        }
                        break;
                    default: throw new UnprocessableEntityException("Localization area not supported", FailureCode.LocalizationAreaNotSupported, nameof(command.LocalizationArea));
                }
                
                // Clear cache so this is now included
                await _cacheService.ClearItemsThatStartWithAsync(MshrmStudioLocalizationConstants.LocalizationResourcesKey);

                return await _localizationRepository.CreateLocalizationResourceAsync(command.LocalizationArea, command.Culture, command.Key, command.Value, command.Comment, cancellationToken);
            }
        }
    }
}
