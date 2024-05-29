using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Models.Events;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using Newtonsoft.Json;
using System;

namespace Mshrm.Studio.Domain.Api.Models.Entity
{
    /// <summary>
    /// A tool 
    /// </summary>
    public class Tool : AuditableEntity, IAggregateRoot
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        public Guid GuidId { get; private set; }

        /// <summary>
        /// The tools name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The type of tool
        /// </summary>
        public ToolType ToolType { get; private set; }

        /// <summary>
        /// A link to the tool
        /// </summary>
        public string Link { get; private set; }

        /// <summary>
        /// AA description
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// The display rank
        /// </summary>
        public int Rank { get; private set; }

        /// <summary>
        /// The logo GUID
        /// </summary>
        public Guid LogoGuidId { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The tools name</param>
        /// <param name="link">A link to more information on the tool</param>
        /// <param name="description">A description of the tool</param>
        /// <param name="rank">Display rank</param>
        /// <param name="toolType"></param>
        /// <param name="logoGuidId">Image</param>
        public Tool(string name, string link, string? description, int rank, ToolType toolType, Guid logoGuidId)
        {
            Name = name;
            Link = link;    
            Description = description;
            Rank = rank;
            ToolType = toolType;
            LogoGuidId = logoGuidId;
             
            // Add event
            base.QueueDomainEvent(new ToolCreatedEvent(Id));
        }

        /// <summary>
        /// Set the name
        /// </summary>
        /// <param name="name">The new name to set</param>
        public void SetName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Set the description
        /// </summary>
        /// <param name="description">The new description</param>
        public void SetDescription(string? description)
        {
            Description = description;
        }

        /// <summary>
        /// Set the new display rank
        /// </summary>
        /// <param name="rank">The new rank</param>
        public void SetRank(int rank)
        {
            Rank = rank;
        }

        /// <summary>
        /// Set a new link
        /// </summary>
        /// <param name="link">The link to set</param>
        public void SetLink(string link)
        {
            Link = Link;
        }

        /// <summary>
        /// Set the new tool type
        /// </summary>
        /// <param name="toolType">The new tool type</param>
        public void SetToolType(ToolType toolType)
        {
            ToolType = toolType;
        }

        /// <summary>
        /// Set the logo
        /// </summary>
        /// <param name="logoGuidId">The new logo guid</param>
        public void SetLogoGuidId(Guid logoGuidId)
        {
            LogoGuidId = logoGuidId;
        }
    }
}
