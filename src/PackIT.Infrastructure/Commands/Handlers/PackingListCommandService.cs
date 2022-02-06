using System.Threading.Tasks;
using PackIT.Data.Entities;
using PackIT.Infrastructure.Exceptions;
using PackIT.Infrastructure.Repositories;

namespace PackIT.Infrastructure.Commands.Handlers
{
    public sealed class PackingListCommandService
    {

        private readonly IPackingListRepository _repository;

        public PackingListCommandService(IPackingListRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(AddPackingItem command)
        {
            var packingList = await _repository.GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            var packingItem = new PackingItem(command.Name, command.Quantity);
            packingList.AddItem(packingItem);
            packingList.Version += 1;

            await _repository.UpdateAsync(packingList);
        }

        public async Task HandleAsync(PackItem command)
        {
            var packingList = await _repository.GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            packingList.PackItem(command.Name);
            packingList.Version += 1;

            await _repository.UpdateAsync(packingList);
        }

        public async Task HandleAsync(RemovePackingItem command)
        {
            var packingList = await _repository.GetAsync(command.PackingListId);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.PackingListId);
            }

            packingList.RemoveItem(command.Name);
            packingList.Version += 1;

            await _repository.UpdateAsync(packingList);
        }

        public async Task HandleAsync(RemovePackingList command)
        {
            var packingList = await _repository.GetAsync(command.Id);

            if (packingList is null)
            {
                throw new PackingListNotFoundException(command.Id);
            }

            await _repository.DeleteAsync(packingList);
        }
    }
}
