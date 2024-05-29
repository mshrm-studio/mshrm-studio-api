using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Models.Events;
using Mshrm.Studio.Domain.Domain.Users.Events;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Domain.Api.Models.Entity
{
    /// <summary>
    /// The domain user
    /// </summary>
    [Index("GuidId", "Email")]
    public class User : AuditableEntity, IAggregateRoot
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        public Guid GuidId { get; private set; }

        /// <summary>
        /// An email identification for a user
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The first name of a user
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// The last name of a user
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// The users current/last logged IP
        /// </summary>
        public string? Ip { get; private set; }

        /// <summary>
        /// If the user is active or not
        /// </summary>
        public bool Active {  get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email">The users email</param>
        /// <param name="active">If the user is active or not</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="ip">The IP of the user</param>
        public User(string email, string firstName, string lastName, string? ip, bool active)
        {
            Active = active;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Ip = ip;

            // Add event
            base.QueueDomainEvent(new UserCreatedEvent(Id, email, ip));
        }

        /// <summary>
        /// Update the IP address
        /// </summary>
        /// <param name="ip">The IP to update</param>
        public void UpdateIp(string ip)
        {
            // See if its changed
            var changed = Ip != ip;

            // Set the new IP
            Ip = ip;

            // Add event
            base.QueueDomainEvent(new UserIpUpdatedEvent(Id, Email, ip));
        }

        /// <summary>
        /// Update a users name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Get the full name of a user
        /// </summary>
        /// <returns>The full name of a user (aggregation of first and last name)</returns>
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

        /// <summary>
        /// Set a users active state
        /// </summary>
        /// <param name="state">The new state to set</param>
        /// <returns>The new active state</returns>
        public bool SetActiveState(bool state)
        {
            Active = state;

            return Active;
        }

        /// <summary>
        /// Set a random GUID Id + actual ID
        /// </summary>
        public void SetIds(int id)
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new Exception("Guid generation uses Email and must be populated");
            }

            Id = id;
            GuidId = Email.GenerateSeededGuid();
        }
    }
}
