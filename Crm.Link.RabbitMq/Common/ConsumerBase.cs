using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.HighPerformance;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;
using System.Timers;
using System.Xml;
using System.Xml.Schema;

namespace Crm.Link.RabbitMq.Common
{
    public abstract class ConsumerBase : RabbitMqClientBase
    {
        private System.Timers.Timer? _timer;
        private readonly ILogger<ConsumerBase> _logger;
        protected abstract string QueueName { get; }
        protected Func<Task>? TimerMethode { get; set; }

        public ConsumerBase(
            ConnectionProvider connectionProvider,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqClientBase> logger) :
            base(connectionProvider, logger)
        {
            _logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                Console.WriteLine(basePath);

                XmlReader reader = new XmlTextReader(@event.Body.AsStream());
                XmlDocument document = new();
                document.Load(reader);

                // xsd for validation
                XmlSchemaSet xmlSchemaSet = new();
                xmlSchemaSet.Add("", $"{basePath}/Resources/AttendeeEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}/Resources/SessionEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}/Resources/SessionAttendeeEvent.xsd");
                xmlSchemaSet.Add("", $"{basePath}/Resources/UUID.xsd");


                document.Schemas.Add(xmlSchemaSet);
                ValidationEventHandler eventHandler = new (ValidationEventHandler);

                document.Validate(eventHandler);
                
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                Console.WriteLine(body);
                var message = JsonConvert.DeserializeObject<T>(body); // still need to do something with this message send to crm after mapping.


            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                Channel!.BasicAck(@event.DeliveryTag, false);
            }
        }

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
