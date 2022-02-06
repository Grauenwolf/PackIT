using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackIT.Data;
using PackIT.Infrastructure.Commands;
using PackIT.Infrastructure.Commands.Factories;
using PackIT.Infrastructure.Commands.Factories.Policies;
using PackIT.Infrastructure.Commands.Models;
using PackIT.Infrastructure.Queries;
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
