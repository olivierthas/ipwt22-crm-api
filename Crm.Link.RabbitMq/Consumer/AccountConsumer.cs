using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Producer;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Crm.Link.Suitcrm.Tools.GateAway;

namespace Crm.Link.RabbitMq.Consumer
{
    public class AccountConsumer : ConsumerBase<AttendeeEvent>, IHostedService
    {
        
        protected override string QueueName => "Accounts";
        private readonly ILogger<AccountConsumer> _accountLogger;
        private readonly IAccountGateAway _accountGateAway;

        public AccountConsumer(
            ConnectionProvider connectionProvider,
            ILogger<AccountConsumer> accountLogger,
            ILogger<ConsumerBase<AttendeeEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            IAccountGateAway accountGateAway) :
            base(connectionProvider, consumerLogger, logger)
        {
            _accountLogger = accountLogger;
            _accountGateAway = accountGateAway;
            TimerMethode += async () => await StartAsync(new CancellationToken(false));  
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (Channel is not null)
            {
                try
                {
                    var consumer = new AsyncEventingBasicConsumer(Channel);
                    consumer.Received += OnEventReceived;
                    Channel?.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
                }
                catch (Exception ex)
                {
                    _accountLogger.LogCritical(ex, "Error while consuming message");
                }
            }
            else
            {
                SetTimer();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// need to inject methode from top level class
        /// 
        protected override void HandelMessage(AttendeeEvent messageObject)
        {
            // Call UUId Do Stuff
            // 
            // Map Data
            //
            var test = new AttendeeEvent();
            var crmObject = new AccountModel
            {
                // if already exist add id for update
                // Id = 
                Name = $"{messageObject.Name} {messageObject.LastName}",
                Email = messageObject.Email,
            };
            // Send To crm
            _accountGateAway.CreateOrUpdate(crmObject);
        }
    }
}
