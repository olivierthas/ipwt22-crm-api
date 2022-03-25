using Microsoft.IdentityModel.Protocols;
using RabbitMQ.Client;

namespace Crm.Link.Api
{
    public static class DepencencyInjection
    {
        public static IServiceCollection UsePersistence(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddSingleton(serviceProvider =>
            {
                var uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
                return new ConnectionFactory
                {
                    Uri = uri,
                    DispatchConsumersAsync = true,
                };
            });
            return services;
        }
    }
}
