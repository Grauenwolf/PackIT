using PackIT.Data.Entities;
using PackIT.Infrastructure.Factories;
using PackIT.Infrastructure.Policies;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace PackIT.UnitTests.Domain
{
	public class PackingListTests
	{
		[Fact]
		public void AddItem_Throws_PackingItemAlreadyExistsException_When_There_Is_Already_Item_With_The_Same_Name()
		{
			//ARRANGE
			var packingList = GetPackingList();
			packingList.AddItem(new PackingItem("Item 1", 1));

			//ACT
			var exception = Record.Exception(() => packingList.AddItem(new PackingItem("Item 1", 1)));

			//ASSERT
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<PackingItemAlreadyExistsException>();
		}

		[Fact]
		public void AddItem_Adds_PackingItemAdded_Domain_Event_On_Success()
		{
			var packingList = GetPackingList();

			var exception = Record.Exception(() => packingList.AddItem(new PackingItem("Item 1", 1)));

			exception.ShouldBeNull();

		}


		#region ARRANGE

		private PackingList GetPackingList()
		{
			var packingList = _factory.Create(Guid.NewGuid(), "MyList", Localization.Create("Warsaw, Poland"));
			return packingList;
		}

		private readonly IPackingListFactory _factory;

		public PackingListTests()
		{
			_factory = new PackingListFactory(Enumerable.Empty<IPackingItemsPolicy>());
		}

		#endregion

	}
}