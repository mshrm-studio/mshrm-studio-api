using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Localization.Api.Models.Entities
{
    public class LocalizationResource : Entity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }

        public string Culture { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string? Comment { get; private set; }
        public LocalizationArea LocalizationArea { get; private set; }

        public LocalizationResource(LocalizationArea localizationArea, string culture, string name, string value, string? comment)
        {
            Culture = culture;
            Name = name;
            Value = value;
            Comment = comment;
            LocalizationArea = localizationArea;
        }

        public void SetupForDefaultImport(int id, string culture)
        {
            Id = id;
            GuidId = Name.GenerateSeededGuid();
            Culture = culture;
        }
    }
}
