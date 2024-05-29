using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Domain.Infrastructure.Factories
{
    public class ToolFactory : IToolFactory
    {
        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="name">The name of the tool</param>
        /// <param name="description">The tools description</param>
        /// <param name="link">A link to the tool</param>
        /// <param name="logoGuidId">The logos key</param>
        /// <param name="rank">The order in which is shown</param>
        /// <param name="toolType">The type of tool</param>
        /// <returns>The new tool</returns>
        public Tool CreateTool(string name, string? description, string link, int rank, Guid logoGuidId, ToolType toolType)
        {
            return new Tool(name, link, description, rank, toolType, logoGuidId);
        }
    }
}
