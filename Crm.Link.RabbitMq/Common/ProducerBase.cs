using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Xml.Serialization;

namespace Crm.Link.RabbitMq.Common
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
    public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
    {
        private readonly ILogger<ProducerBase<T>> _logger;
        protected abstract string ExchangeName { get; }
        protected abstract string RoutingKeyName { get; }
        protected abstract string AppId { get; }

        protected List<T> MessageQueue { get; set; } = new();

        protected ProducerBase(
            ConnectionProvider connectionProvider,
            ILogger<RabbitMqClientBase> logger,
            ILogger<ProducerBase<T>> producerBaseLogger) :
            base(connectionProvider, logger) => _logger = producerBaseLogger;

        public virtual void Publish(T @event)
        {
            _ = @event ?? throw new ArgumentNullException(nameof(@event));

            MessageQueue.Add(@event);
            if (Channel is not null)
            {
                foreach (var message in MessageQueue)
                {
                    try
                    {
                        _logger.LogInformation("start");

                        ReadOnlyMemory<byte> body;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            serializer.Serialize(ms, @event);

                            body = new ReadOnlyMemory<byte>(ms.ToArray());
                        }

                        _logger.LogInformation("sending");
                        _logger.LogInformation($"message size: {body.Length}");

                        var properties = Channel.CreateBasicProperties();
                        properties.AppId = AppId;
                        properties.ContentType = "application/xml";
                        properties.DeliveryMode = 1; // Doesn't persist to disk
                        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        Channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body, basicProperties: properties);
                        _logger.LogInformation("published");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Error while publishing");
                    }
                }
                        MessageQueue.Clear();
            }
        }
    }
}
