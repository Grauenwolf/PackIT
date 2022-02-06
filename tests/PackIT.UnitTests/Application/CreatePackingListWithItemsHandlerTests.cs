using NSubstitute;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Commands;
using PackIT.Infrastructure.Commands.Handlers;
using PackIT.Infrastructure.Consts;
using PackIT.Infrastructure.DTO.External;
using PackIT.Infrastructure.Exceptions;
using PackIT.Infrastructure.Factories;
using PackIT.Infrastructure.Services;
using PackIT.Infrastructure.ValueObjects;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PackIT.UnitTests.Application
{
	public class CreatePackingListWithItemsHandlerTests
	{
		Task Act(CreatePackingListWithItems command)
			=> _commandHandler.CreatePackingListWithItemsAsync(command);

		[Fact]
		public async Task HandleAsync_Throws_PackingListAlreadyExistsException_When_List_With_same_Name_Already_Exists()
		{
			var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10, Gender.Female, default);
			_readService.ExistsByNameAsync(command.Name).Returns(true);

			var exception = await Record.ExceptionAsync(() => Act(command));

			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<PackingListAlreadyExistsException>();
		}

		[Fact]
		public async Task HandleAsync_Throws_MissingLocalizationWeatherException_When_Weather_Is_Not_Returned_From_Service()
		{
			var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10, Gender.Female,
				new LocalizationWriteModel("Warsaw", "Poland"));

			_readService.ExistsByNameAsync(command.Name).Returns(false);
			_weatherService.GetWeatherAsync(Arg.Any<Localization>()).Returns(default(WeatherDto));

			var exception = await Record.ExceptionAsync(() => Act(command));

			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<MissingLocalizationWeatherException>();
		}

		[Fact]
		public async Task HandleAsync_Calls_Repository_On_Success()
		{
			var command = new CreatePackingListWithItems(Guid.NewGuid(), "MyList", 10, Gender.Female,
				new LocalizationWriteModel("Warsaw", "Poland"));

			_readService.ExistsByNameAsync(command.Name).Returns(false);
			_weatherService.GetWeatherAsync(Arg.Any<Localization>()).Returns(new WeatherDto(12));
			_factory.CreateWithDefaultItems(command.Id, command.Name, command.Days, command.Gender,
				Arg.Any<Temperature>(), Arg.Any<Localization>()).Returns(default(PackingList));

			var exception = await Record.ExceptionAsync(() => Act(command));

			exception.ShouldBeNull();
			await _repository.Received(1).AddPackingListAsync(Arg.Any<PackingList>());
		}

		#region ARRANGE

		private readonly CreatePackingListWithItemsService _commandHandler;
		private readonly IPackingListCommandService _repository;
		private readonly IWeatherService _weatherService;
		private readonly IPackingListReadService _readService;
		private readonly IPackingListFactory _factory;

		public CreatePackingListWithItemsHandlerTests()
		{
			_repository = Substitute.For<IPackingListCommandService>();
			_weatherService = Substitute.For<IWeatherService>();
			_readService = Substitute.For<IPackingListReadService>();
			_factory = Substitute.For<IPackingListFactory>();

			_commandHandler = new CreatePackingListWithItemsService(_repository, _factory, _readService, _weatherService);
		}

		#endregion
	}
}