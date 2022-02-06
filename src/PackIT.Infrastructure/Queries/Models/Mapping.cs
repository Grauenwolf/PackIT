using System.Linq;
using PackIT.Data.Entities;

namespace PackIT.Infrastructure.Queries.Models
{
    internal static class Mapping
    {
        public static PackingListDto AsDto(this PackingListReadModel value)
        {
            return new()
            {
                Id = value.Id,
                Name = value.Name,
                Localization = new LocalizationDto
                {
                    City = value.Localization?.City,
                    Country = value.Localization?.Country
                },
                Items = value.Items?.Select(pi => new PackingItemDto
                {
                    Name = pi.Name,
                    Quantity = pi.Quantity,
                    IsPacked = pi.IsPacked
                })
            };
        }
    }
}
