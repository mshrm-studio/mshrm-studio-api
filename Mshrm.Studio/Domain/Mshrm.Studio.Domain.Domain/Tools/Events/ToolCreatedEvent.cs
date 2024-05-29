using MediatR;

namespace Mshrm.Studio.Domain.Api.Models.Events
{
    public class ToolCreatedEvent : INotification
    {
        /// <summary>
        /// The user created
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Constructtor
        /// </summary>
        /// <param name="id">The tools id</param>
        public ToolCreatedEvent(int id)
        {
            Id = id;
        }
    }
}
