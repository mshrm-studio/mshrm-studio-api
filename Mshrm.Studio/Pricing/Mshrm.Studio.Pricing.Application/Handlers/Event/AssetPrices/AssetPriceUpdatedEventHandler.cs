using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPriceHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Events;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;

namespace Mshrm.Studio.Pricing.Api.EventHandlers.AssetPrices
{
    public class AssetPriceUpdatedEventHandler : INotificationHandler<AssetPriceUpdatedEvent>
    {
        private readonly ILogger<AssetPriceUpdatedEventHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPriceCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public AssetPriceUpdatedEventHandler(IMediator mediator, IMapper mapper, ILogger<AssetPriceUpdatedEventHandler> logger)
        {
            _mediator = mediator;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(AssetPriceUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Add the change to history
            await _mediator.Send<AssetPriceHistory>(_mapper.Map<CreateAssetPriceHistoryCommand>(notification));

            // Do other stuff...
        }
    }
}
