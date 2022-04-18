using Crm.Link.Api.GateAway;

namespace Crm.Link.Api
{
    public static class DepencencyInjection
    {
        public static IServiceCollection UsePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<TokenProvider>();

            services.AddTransient<IAccountGateAway, AccountGateAway>();
            services.AddTransient<ISessionGateAway, SessionGateAway>();

            return services;
        }
    }
}
