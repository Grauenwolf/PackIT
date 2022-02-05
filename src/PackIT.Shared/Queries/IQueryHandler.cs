using System.Threading.Tasks;
using PackIT.Shared.Queries;

namespace PackIT.Shared.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}