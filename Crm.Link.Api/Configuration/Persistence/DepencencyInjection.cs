using Crm.Link.Api.GateAway;
using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Producer;
using RabbitMQ.Client;

namespace Crm.Link.Api
{
    public static class DepencencyInjection
    {
        public static IServiceCollection UsePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory>(serviceProvider =>
            {
                var uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
                return new ConnectionFactory
                {
                    Uri = uri,
                    DispatchConsumersAsync = true,
                };
            });

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<AccountPublisher>();
            services.AddSingleton<SessionPublisher>();
            services.AddSingleton<ConnectionProvider>();

            services.AddTransient<IAccountGateAway, AccountGateAway>();
            services.AddTransient<ISessionGateAway, SessionGateAway>();

            return services;
        }
    }
}
