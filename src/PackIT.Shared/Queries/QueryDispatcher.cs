using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PackIT.Shared.Queries
{
    public sealed class QueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        //public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        //{
        //    using var scope = _serviceProvider.CreateScope();
        //    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        //    var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        //    return await (Task<TResult>)handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
        //        .Invoke(handler, new[] { query });
        //}

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery<TResult>
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();


            var result = await handler.HandleAsync((TQuery)query);
            return (TResult)result;
        }
    }
}
