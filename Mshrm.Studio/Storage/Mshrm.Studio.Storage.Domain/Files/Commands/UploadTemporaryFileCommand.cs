
using MediatR;
using Mshrm.Studio.Storage.Api.Models.Misc;

namespace Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands
{
    public class UploadTemporaryFileCommand : IRequest<TemporaryFileUpload>
    {
        public Stream Stream { get; set; }
    }
}
