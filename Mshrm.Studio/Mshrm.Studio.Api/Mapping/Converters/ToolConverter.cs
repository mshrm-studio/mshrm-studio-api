using AutoMapper;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Models.Dtos.Tools;
using System.Runtime.Intrinsics;

namespace Mshrm.Studio.Api.Mapping.Converters
{
    /// <summary>
    /// For the converstion of toolDTO to toolResponseDTO
    /// </summary>
    public class ToolConverter : ITypeConverter<ToolDto, ToolResponseDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolConverter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ToolConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Convert models
        /// </summary>
        /// <param name="parent">The ToolDto</param>
        /// <param name="target">The ToolResponseDto</param>
        /// <param name="context">The request context</param>
        /// <returns>A ToolResponseDto</returns>
        public ToolResponseDto Convert(
            ToolDto parent,
            ToolResponseDto target,
            ResolutionContext context)
        {
            target = new ToolResponseDto();

            // Build the url
            var url = (parent.LogoGuidId.HasValue) ? $"https://{_httpContextAccessor.HttpContext?.Request.Host}/api/v1/files/guid/{parent.LogoGuidId}" : null;

            // Set the rest of the properties
            target.Rank = parent.Rank;
            target.Description = parent.Description;
            target.Name = parent.Name;
            target.ToolType = parent.ToolType;
            target.GuidId = parent.GuidId;
            target.logoGuidId = parent.LogoGuidId;
            target.Link = parent.Link;
            target.LogoUrl = url;

            // Return mapped target
            return target;
        }
    }
}
