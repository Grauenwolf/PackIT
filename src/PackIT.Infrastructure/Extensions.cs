using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackIT.Application.Services;
using PackIT.Domain.Repositories;
using PackIT.Infrastructure.EF.Contexts;
using PackIT.Infrastructure.EF.Options;
using PackIT.Infrastructure.EF.Queries.Handlers;
using PackIT.Infrastructure.EF.Repositories;
using PackIT.Infrastructure.EF.Services;
using PackIT.Infrastructure.Services;

namespace PackIT.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPackingListRepository, PostgresPackingListRepository>();
            services.AddScoped<IPackingListReadService, PostgresPackingListReadService>();

            var options = configuration.GetSection("Postgres").Get<PostgresOptions>();
            services.AddDbContext<ReadDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));
            services.AddDbContext<WriteDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));

            IServiceCollection result = services;
            services.AddScoped<PackingListQueryService>();
            services.AddSingleton<IWeatherService, DumbWeatherService>();

            return services;
        }
    }
}
