using MediatR;
using Microsoft.AspNetCore.Identity;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Entities.NewFolder;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Auth.Api.Models.Entities
{
    /// <summary>
    /// Mshrm Studio implementation of identity user
    /// </summary>
    public class MshrmStudioIdentityUser : IdentityUser, IDomainEvent
    {
        /// <summary>
        /// The users refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// When the refresh token expires
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="MshrmStudioIdentityUser"/> class.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="emailConfirmed"></param>
        public MshrmStudioIdentityUser(string email, string userName, bool emailConfirmed)
        {
            Email = email;
            UserName = email;
            RefreshToken = string.Empty;
            RefreshTokenExpiryTime = DateTime.UtcNow;
            EmailConfirmed = emailConfirmed;
        }

        /// <summary>
        /// Set a random GUID Id
        /// </summary>
        public void SetRandomGuidId()
        {
            if(string.IsNullOrEmpty(Email))
            {
                throw new Exception("Guid generation uses Email and must be populated");
            }

            Id = Email.GenerateSeededGuid().ToString();
        }
    }
}
