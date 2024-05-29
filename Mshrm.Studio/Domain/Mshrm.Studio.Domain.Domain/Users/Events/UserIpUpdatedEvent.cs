using MediatR;

namespace Mshrm.Studio.Domain.Domain.Users.Events
{
    public class UserIpUpdatedEvent : INotification
    {
        /// <summary>
        /// The user created
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The logged IP of the user
        /// </summary>
        public string? Ip { get; private set; }

        /// <summary>
        /// Constructtor
        /// </summary>
        /// <param name="userId">The user created</param>
        /// <param name="email">The users email</param>
        /// <param name="ip">The users current/last logged IP</param>
        public UserIpUpdatedEvent(int userId, string email, string? ip)
        {
            UserId = userId;
            Email = email;
            Ip = ip;
        }
    }
}
