using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Crm.Link.RabbitMq.Common
{
    public class ConnectionProvider
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger<ConnectionProvider> logger;

        public ConnectionProvider(IConnectionFactory connectionFactory, ILogger<ConnectionProvider> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }
                
        public IConnection? GetConsumerConnection()
        {
            return OpenConnection();
        }

        public IConnection? GetPublisherConnection()
        {
            
            return OpenConnection();
        }

        private IConnection? OpenConnection()
        {
            try
            {
                return connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {                
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Tried to connect to rabbitMq");
            }

            return null;
        }
    }
}
