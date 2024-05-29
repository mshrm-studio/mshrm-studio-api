using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Events;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;

namespace Mshrm.Studio.Pricing.Api.EventHandlers.Users
{
    public class ExchangePricePairUpdatedEventHandler : INotificationHandler<ExchangePricePairUpdatedEvent>
    {
        private readonly ILogger<ExchangePricePairUpdatedEventHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangePricePairCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ExchangePricePairUpdatedEventHandler(IMediator mediator, IMapper mapper, ILogger<ExchangePricePairUpdatedEventHandler> logger)
        {
            _mediator = mediator;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(ExchangePricePairUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Add the change to history
            await _mediator.Send<ExchangePricingPairHistory>(_mapper.Map<CreateExchangePricingPairHistoryCommand>(notification));

            // Do other stuff...
        }
    }
}
