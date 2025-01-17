﻿using System.Threading.Tasks;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands.Factories;
using PackIT.Infrastructure.Commands.Factories.Policies;
using PackIT.Infrastructure.Services;

namespace PackIT.Infrastructure.Commands.Models
{
    public class CreatePackingListWithItemsService
    {
        private readonly IPackingListCommandService _repository;
        private readonly IPackingListFactory _factory;
        private readonly IPackingListReadService _readService;
        private readonly IWeatherService _weatherService;

        public CreatePackingListWithItemsService(IPackingListCommandService repository, IPackingListFactory factory,
            IPackingListReadService readService, IWeatherService weatherService)
        {
            _repository = repository;
            _factory = factory;
            _readService = readService;
            _weatherService = weatherService;
        }

        public async Task CreatePackingListWithItemsAsync(CreatePackingListWithItems command)
        {
            var (id, name, days, gender, localizationWriteModel) = command;

            if (await _readService.ExistsByNameAsync(name))
            {
                throw new PackingListAlreadyExistsException(name);
            }

            var localization = new Localization(localizationWriteModel.City, localizationWriteModel.Country);
            var weather = await _weatherService.GetWeatherAsync(localization);

            if (weather is null)
            {
                throw new MissingLocalizationWeatherException(localization);
            }

            if (days is 0 or > 100)
            {
                throw new InvalidTravelDaysException(days);
            }

            var packingList = _factory.CreateWithDefaultItems(id, name, days, gender, weather.Temperature,
                localization);


            await _repository.AddPackingListAsync(packingList);
        }
    }
}
