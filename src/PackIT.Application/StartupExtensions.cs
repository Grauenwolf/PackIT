using Microsoft.Extensions.DependencyInjection;
using PackIT.Application.Commands.Handlers;
using PackIT.Domain.Factories;
using PackIT.Domain.Policies;

namespace PackIT.Application
{
    public static class StartupExtensions
    {
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
