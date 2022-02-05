using System.Threading.Tasks;
using PackIT.Shared.Queries;

namespace PackIT.Shared.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}