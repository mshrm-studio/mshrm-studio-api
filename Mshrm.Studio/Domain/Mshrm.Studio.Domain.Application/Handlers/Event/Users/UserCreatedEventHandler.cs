using Dapr.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Domain.Api.Models.Events;
using Mshrm.Studio.Domain.Domain.Users.Events;

namespace Mshrm.Studio.Domain.Api.Handlers.Event.Users
{
    /// <summary>
    /// Handles user created event
    /// </summary>
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DaprClient _client;
        private readonly ILogger<UserCreatedEventHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public UserCreatedEventHandler(IServiceProvider serviceProvider, DaprClient client, ILogger<UserCreatedEventHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Handle event
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An async task</returns>
        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Fired when a new user is created

            // TODO: add a message to Telegram group - this requires a new MS (Mshrm.Studio.Message.Api)
        }
    }
}
