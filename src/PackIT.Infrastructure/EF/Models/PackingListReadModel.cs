using System;
using System.Collections.Generic;
using System.Linq;
using PackIT.Application.DTO;

namespace PackIT.Infrastructure.EF.Models
{
    public class PackingListReadModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public LocalizationReadModel Localization { get; set; }
        public ICollection<PackingItemReadModel> Items { get; set; }

        public PackingListDto AsDto()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Localization = new LocalizationDto
                {
                    City = Localization?.City,
                    Country = Localization?.Country
                },
                Items = Items?.Select(pi => new PackingItemDto
                {
                    Name = pi.Name,
                    Quantity = pi.Quantity,
                    IsPacked = pi.IsPacked
                })
            };
        }
    }
}
