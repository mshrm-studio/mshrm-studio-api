using AutoMapper;
using Mshrm.Studio.Email.Api.Models.CQRS.Emails.Commands;
using Mshrm.Studio.Email.Api.Models.Dtos;
using Mshrm.Studio.Email.Api.Models.Entities;

namespace Mshrm.Studio.Email.Api.Mapping
{
    /// <summary>
    /// The email mapping profile
    /// </summary>
    public class MshrmStudioEmailMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioEmailMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init mappings
        /// </summary>
        private void Init()
        {
            #region Email Message

            CreateMap<EmailMessage, EmailMessageDto>().ReverseMap();
            CreateMap<EmailDto, CreateEmailCommand>().ReverseMap();

            #endregion
        }
    }
}
