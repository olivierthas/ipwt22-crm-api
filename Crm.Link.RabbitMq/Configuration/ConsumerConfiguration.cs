using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Consumer;
using Crm.Link.RabbitMq.Producer;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Crm.Link.RabbitMq.Configuration
{
    public static class ConsumerConfiguration
    {
        public static IServiceCollection StartConsumers(this IServiceCollection services, string connectionstring)
        {
            services.AddSingleton<IConnectionFactory>(serviceProvider =>
            {
                //// var uri = new Uri(connectionstring);
                return new ConnectionFactory
                {
                    //// Uri = uri,
                    HostName = "rabbitmq",
                    DispatchConsumersAsync = true,
                };
            });

            services.AddSingleton<ConnectionProvider>();

            services.AddHostedService<AccountConsumer>();
            services.AddHostedService<SessionConsumer>();
            services.AddHostedService<ContactConsumer>();
            services.AddHostedService<SessionUserConsumer>();

            return services;
        }

        public static IServiceCollection AddPublisher(this IServiceCollection services)
        {
            services.AddSingleton<AccountPublisher>();
            services.AddSingleton<SessionPublisher>();
            services.AddSingleton<ContactPublisher>();
            services.AddSingleton<SessionContactPublisher>();

            return services;
        }
    }
}
