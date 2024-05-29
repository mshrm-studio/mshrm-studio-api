using MediatR;
using Mshrm.Studio.Shared.Models.Entities.NewFolder;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Shared.Models.Entities
{
    /// <summary>
    /// Inherit from this is the poco
    /// </summary>
    public abstract class Entity : IDomainEvent
    {
        /// <summary>
        /// List of domain events for entity update
        /// </summary>
        [NotMapped]
        public List<INotification> DomainEvents { get; } = new List<INotification>();

        /// <summary>
        /// Add a domain event to be processed
        /// </summary>
        /// <param name="event">The event to process</param>
        public void QueueDomainEvent(INotification @event)
        {
            DomainEvents.Add(@event);
        }
    }
}
