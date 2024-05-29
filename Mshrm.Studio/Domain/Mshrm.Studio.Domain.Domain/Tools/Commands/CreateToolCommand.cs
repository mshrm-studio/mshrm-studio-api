using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.Tools
{
    public class CreateToolCommand: IRequest<Tool>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ToolType ToolType { get; set; }
        public int Rank { get; set; }
        public string Link { get; set; }
        public Guid LogoGuidId { get; set; }
    }
}
