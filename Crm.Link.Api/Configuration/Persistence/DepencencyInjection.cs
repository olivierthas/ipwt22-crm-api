using Crm.Link.Api.GateAway;

namespace Crm.Link.Api
{
    public static class DepencencyInjection
    {
        public static IServiceCollection UsePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            /*services.AddSingleton(serviceProvider =>
            {
                var uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
                return new ConnectionFactory
                {
                    Uri = uri,
                    DispatchConsumersAsync = true,
                };
            });*/
            services.AddSingleton<TokenProvider>();

            services.AddTransient<IAccountGateAway, AccountGateAway>();
            services.AddTransient<ISessionGateAway, SessionGateAway>();
            return services;
        }
    }
}
