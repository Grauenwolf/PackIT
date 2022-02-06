using System.Threading.Tasks;

namespace PackIT.Infrastructure.Commands
{
    public interface IPackingListReadService
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
