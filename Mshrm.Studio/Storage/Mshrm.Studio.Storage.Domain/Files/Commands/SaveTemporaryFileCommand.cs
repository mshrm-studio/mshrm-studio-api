using MediatR;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Misc;
using Mshrm.Studio.Storage.Domain.Resources.Enums;

namespace Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands
{
    public class SaveTemporaryFileCommand : IRequest<Resource>
    {
        public required string Key { get; set; }
        public required string FileName { get; set; }
        public DirectoryType DirectoryType { get; set; }
        public bool IsPrivate { get; set; } 
    }
}
