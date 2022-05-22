using Crm.Link.RabbitMq.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance;
using RabbitMQ.Client.Events;
using System.Resources;
using System.Timers;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Crm.Link.RabbitMq.Common
{
    public abstract class ConsumerBase<T> : RabbitMqClientBase where T : notnull
    {
        private System.Timers.Timer? _timer;
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
                Stream stream = @event.Body.AsStream();

                XmlReader reader = new XmlTextReader(stream);
                XmlDocument document = new();
                document.Load(reader);

                
                _logger.LogInformation("{basePath}Resources/AttendeeEvent.xsd", basePath);
                // xsd for validation
                XmlSchemaSet xmlSchemaSet = new();
                xmlSchemaSet.Add("", $"{basePath}Resources/AttendeeEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}Resources/SessionEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}Resources/SessionAttendeeEvent.xsd");


                document.Schemas.Add(xmlSchemaSet);
                ValidationEventHandler eventHandler = new(ValidationEventHandler);

                document.Validate(eventHandler);

                XmlRootAttribute root = new("SessionEvent");
                root.IsNullable = true;
                var serializer = new XmlSerializer(typeof(T), root);

                T? message = serializer.Deserialize(stream) != null? (T)serializer.Deserialize(stream)! : default;

                await HandelMessage(message);                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");                
                return;
            }
            finally
            {
                _logger?.LogInformation("hello Mrs T: {Name}", typeof(T).Name);
            }

            Channel!.BasicAck(@event.DeliveryTag, false);
        }

        protected abstract Task HandelMessage(T? messageObject);
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
    }
}
