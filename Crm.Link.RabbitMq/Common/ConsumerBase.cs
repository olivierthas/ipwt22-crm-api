using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance;
using RabbitMQ.Client.Events;
using System.Timers;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Crm.Link.RabbitMq.Common
{
    public abstract class ConsumerBase<T> : RabbitMqClientBase where T : IEvent
    {
        private System.Timers.Timer? _timer;
        private Dictionary<string, int> _failCount = new();
        private string _key = "";
        protected readonly ILogger<ConsumerBase<T>> _logger;
        protected abstract string QueueName { get; }
        protected Func<Task>? TimerMethode { get; set; }

        public ConsumerBase(
            ConnectionProvider connectionProvider,
            ILogger<ConsumerBase<T>> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionProvider, logger)
        {
            _logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived(object sender, BasicDeliverEventArgs @event)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            _logger?.LogInformation("hello Mrs T: {Name}", typeof(T).Name);
            try
            {
                /*XmlReader reader = new XmlTextReader(@event.Body.AsStream());
                XmlDocument document = new();
                document.Load(reader);

                // xsd for validation
                XmlSchemaSet xmlSchemaSet = new();
                xmlSchemaSet.Add("", $"{basePath}Resources/AttendeeEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}Resources/SessionEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}Resources/SessionAttendeeEvent.xsd");

                document.Schemas.Add(xmlSchemaSet);
                ValidationEventHandler eventHandler = new(ValidationEventHandler);

                document.Validate(eventHandler);*/

                XmlRootAttribute root = new();
                root.ElementName = SessionEvent.XmlElementName;
                root.IsNullable = true;

                var serializer = new XmlSerializer(typeof(T), root);

                var message = serializer.Deserialize(@event.Body.AsStream());
                if (message == null)
                {
                    _logger.LogError("deserialized message was null!!!!");
                    Channel.BasicAck(@event.DeliveryTag, false);
                }

                _key = ((T)message).UUID_Nr;
                _logger.LogInformation("key for requeu is: {key}", _key);
                await HandleMessage((T)message);
            }
            catch (FieldAccessException fex)
            {
                FaildMessage(_key, @event);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
                SuccessMessage(_key); // well not realy but yeah
                return;
            }

            Channel!.BasicAck(@event.DeliveryTag, false);
            SuccessMessage(_key);
        }

        protected abstract Task HandleMessage(T? messageObject);
        private void ValidationEventHandler(object? sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    _logger.LogError("Error: {Message}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    _logger.LogWarning("Warning {Message}", e.Message);
                    break;
            }
        }

        protected void SetTimer()
        {
            if (_timer == null)
            {
                _logger.LogInformation("creating timer - consumerbase.");
                _timer = new System.Timers.Timer(10000);

                _timer.Elapsed += OnTimedEvent;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
        }

        private void OnTimedEvent(Object? source, ElapsedEventArgs e)
        {

            if (Channel is not null && TimerMethode is not null)
            {
                TimerMethode().GetAwaiter().GetResult();
                _timer?.Stop();
                _timer?.Dispose();
            }
        }

        protected void SuccessMessage(string key)
        {
            if (_failCount.TryGetValue(key, out int value))
            {
                _failCount.Remove(key);
            }
        }

        protected void FaildMessage(string key, BasicDeliverEventArgs @event)
        {
            if (string.IsNullOrEmpty(key))
                Channel!.BasicAck(@event.DeliveryTag, false);

            if (_failCount.TryGetValue(key, out int value))
            {
                if (value <= 5)
                {
                    _failCount[key] += 1;
                    _logger.LogInformation("message failed for: {key} - {count}", key, _failCount[key]);
                    Channel!.BasicNack(@event.DeliveryTag, false, true);
                }
                else
                {
                    Channel!.BasicAck(@event.DeliveryTag, false);
                    _failCount.Remove(key);
                }

                return;
            }
            _failCount.Add(key, 1);
            _logger.LogInformation("message failed adding key: {key}", key);
            Channel!.BasicNack(@event.DeliveryTag, false, true);
        }
    }
}
