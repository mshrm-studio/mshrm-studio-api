using AutoMapper;
using Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands;
using Mshrm.Studio.Storage.Api.Models.Dtos.Files;
using Mshrm.Studio.Storage.Api.Models.Dtos.Resources;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Misc;

namespace Mshrm.Studio.Storage.Api.Mapping
{
    /// <summary>
    /// Storage mapping
    /// </summary>
    public class MshrmStudioStorageMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MshrmStudioStorageMappingProfile"/> class.
        /// </summary>
        public MshrmStudioStorageMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init maps
        /// </summary>
        public void Init()
        {
            #region Resources

            CreateMap<Resource, ResourceDto>().ReverseMap();

            #endregion

            #region Files

            CreateMap<TemporaryFileUpload, TemporaryFileUploadDto>().ReverseMap();
            CreateMap<SaveTemporaryFileDto, SaveTemporaryFileCommand>().ReverseMap();
            CreateMap<SaveTemporaryFilesDto, SaveTemporaryFilesCommand>().ReverseMap();

            #endregion
        }
    }
}
