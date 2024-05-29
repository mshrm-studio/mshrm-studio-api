using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Models.Entities.NewFolder
{
    public interface IDomainEvent
    {
        /// <summary>
        /// List of domain events for entity update
        /// </summary>
        [NotMapped]
        public List<INotification> DomainEvents { get; }

        /// <summary>
        /// Add a domain event to be processed
        /// </summary>
        /// <param name="event">The event to process</param>
        public void QueueDomainEvent(INotification @event);
    }
}
