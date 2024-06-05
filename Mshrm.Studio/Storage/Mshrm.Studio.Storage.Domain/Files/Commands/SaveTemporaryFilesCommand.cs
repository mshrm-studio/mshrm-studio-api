using MediatR;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Misc;
using Mshrm.Studio.Storage.Domain.Resources.Enums;

namespace Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands
{
    public class SaveTemporaryFilesCommand : IRequest<List<Resource>>
    {
        public List<SaveTemporaryFileCommand> SaveTemporaryFilesCommands { get; set; }
    }
}
