using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackIT.Data;
using PackIT.Infrastructure.Commands.Handlers;
using PackIT.Infrastructure.EF.Options;
using PackIT.Infrastructure.EF.Queries.Handlers;
using PackIT.Infrastructure.EF.Services;
using PackIT.Infrastructure.Factories;
using PackIT.Infrastructure.Policies;
using PackIT.Infrastructure.Services;

namespace PackIT.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPackingListReadService, PostgresPackingListReadService>();

            var options = configuration.GetSection("Postgres").Get<PostgresOptions>();
            services.AddDbContext<ReadDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));
            services.AddDbContext<WriteDbContext>(ctx => ctx.UseNpgsql(options.ConnectionString));

            IServiceCollection result = services;
            services.AddScoped<PackingListQueryService>();
            services.AddSingleton<IWeatherService, DumbWeatherService>();

            return services;
        }

        public static IServiceCollection AddPackITApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPackingListFactory, PackingListFactory>();
            services.AddScoped<PackingListCommandService>();
            services.AddScoped<CreatePackingListWithItemsService>();

            services.Scan(b => b.FromAssemblies(typeof(IPackingItemsPolicy).Assembly)
                .AddClasses(c => c.AssignableTo<IPackingItemsPolicy>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }
    }
}
