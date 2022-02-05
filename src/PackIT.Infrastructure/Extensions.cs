using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackIT.Application.Services;
using PackIT.Infrastructure.EF;
using PackIT.Infrastructure.EF.Queries.Handlers;
using PackIT.Infrastructure.Services;

namespace PackIT.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgres(configuration);
            services.AddScoped<PackingListQueryService>();
            services.AddSingleton<IWeatherService, DumbWeatherService>();

            return services;
        }
    }
}
