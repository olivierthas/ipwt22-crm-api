using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance;
using RabbitMQ.Client.Events;
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
            try
            {
                Stream stream = @event.Body.AsStream();

                XmlReader reader = new XmlTextReader(stream);
                XmlDocument document = new();
                document.Load(reader);


                _logger.LogInformation($"{basePath}/Resources/AttendeeEvent.xsd");
                // xsd for validation
                XmlSchemaSet xmlSchemaSet = new();
                xmlSchemaSet.Add("", $"{basePath}/Resources/AttendeeEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}/Resources/SessionEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}/Resources/SessionAttendeeEvent.xsd");


                document.Schemas.Add(xmlSchemaSet);
                ValidationEventHandler eventHandler = new(ValidationEventHandler);

                document.Validate(eventHandler);

                var serializer = new XmlSerializer(typeof(T));

                T test = (T)serializer.Deserialize(stream)!;

                await HandelMessage(test);                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel!.BasicAck(@event.DeliveryTag, false);
            }
            await Task.CompletedTask;
        }

        protected abstract Task HandelMessage(T messageObject);
        private void ValidationEventHandler(object? sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }

        protected void SetTimer()
        {
            if (_timer == null)
            {
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
