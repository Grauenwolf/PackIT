using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PackIT.Shared.Exceptions;
using PackIT.Shared.Services;

namespace PackIT.Shared
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPackITShared(this IServiceCollection services)
        {
            services.AddHostedService<AppInitializer>();
            services.AddScoped<ExceptionMiddleware>();
            return services;
        }

        public static IApplicationBuilder UsePackITShared(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
