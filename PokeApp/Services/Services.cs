using Refit;
using InjectedServices.Interfaces;
using InjectedServices.Interfaces.Refit;
using InjectedServices.Services;

namespace PokeApp.Services
{
    public static class Services
    {
        public static void AllServices(this IServiceCollection services, string apiConnection)
        {
            services.AddScoped<IPokeService, PokeService>();

            services.AddRefitClient<IPokeAPI>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConnection))
                .SetHandlerLifetime(TimeSpan.FromMinutes(2));
        }
    }
}
