using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Crm.Link.RabbitMq.Common
{
    public class ConnectionProvider : IDisposable
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger<ConnectionProvider> logger;
        private IConnection? consumerConnection;
        private IConnection? publisherConnection;

        public ConnectionProvider(IConnectionFactory connectionFactory, ILogger<ConnectionProvider> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public void Dispose()
        {
            if (consumerConnection != null)
            {
                consumerConnection.Dispose();
            }

            if (publisherConnection != null)
            {
                publisherConnection.Dispose();
            }
        }

        public IConnection? GetConsumerConnection()
        {
            if (consumerConnection == null || !consumerConnection!.IsOpen)
                return consumerConnection = OpenConnection();

            return consumerConnection;
        }

        public IConnection? GetPublisherConnection()
        {
            if (publisherConnection == null || !publisherConnection!.IsOpen)
                return publisherConnection = OpenConnection();

            return publisherConnection;
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
