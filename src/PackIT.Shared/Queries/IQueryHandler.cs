using System.Threading.Tasks;

namespace PackIT.Shared.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }

    //public interface IQueryHandler<in TQuery> where TQuery : class, IQuery
    //{
    //    Task<object> HandleAsync(TQuery query);
    //}

    //public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>, IQueryHandler<TQuery>
    //    where TQuery : class, IQuery<TResult>
    //{
    //    public abstract Task<TResult> HandleAsync(TQuery query);

    //    async Task<object> IQueryHandler<TQuery>.HandleAsync(TQuery query)
    //    {
    //        return await HandleAsync(query);
    //    }
    //}
}
