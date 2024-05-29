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
    public class ExchangePricePairCreatedEventHandler : INotificationHandler<ExchangePricePairCreatedEvent>
    {
        private readonly ILogger<ExchangePricePairCreatedEventHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangePricePairCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ExchangePricePairCreatedEventHandler(IMediator mediator, IMapper mapper, ILogger<ExchangePricePairCreatedEventHandler> logger)
        {
            _mediator = mediator;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(ExchangePricePairCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Add the change to history
            await _mediator.Send<ExchangePricingPairHistory>(_mapper.Map<CreateExchangePricingPairHistoryCommand>(notification));

            // Do other stuff...
        }
    }
}
