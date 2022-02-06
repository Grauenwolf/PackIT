using System.Threading.Tasks;

namespace PackIT.Infrastructure.Services
{
    public interface IPackingListReadService
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}