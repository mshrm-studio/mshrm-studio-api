using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
       /// <summary>
       /// The users guid identifier to remove
       /// </summary>
       public Guid Guid { get; set; }
    }
}
