﻿using Crm.Link.RabbitMq.Common;
using Crm.Link.RabbitMq.Messages;
using Crm.Link.Suitcrm.Tools.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Crm.Link.Suitcrm.Tools.GateAway;
using Crm.Link.UUID;
using Crm.Link.UUID.Model;

namespace Crm.Link.RabbitMq.Consumer
{
    public class AccountConsumer : ConsumerBase<AccountEvent>, IHostedService
    {
        
        protected override string QueueName => "CrmAccount";
        private readonly ILogger<AccountConsumer> _accountLogger;
        private readonly IAccountGateAway _accountGateAway;
        private readonly IUUIDGateAway _uUIDGateAway;

        public AccountConsumer(
            ConnectionProvider connectionProvider,
            ILogger<AccountConsumer> accountLogger,
            ILogger<ConsumerBase<AccountEvent>> consumerLogger,
            ILogger<RabbitMqClientBase> logger,
            IAccountGateAway accountGateAway,
            IUUIDGateAway uUIDGateAway) :
            base(connectionProvider, consumerLogger, logger)
        {
            _accountLogger = accountLogger;
            _accountGateAway = accountGateAway;
            _uUIDGateAway = uUIDGateAway;
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
        protected async override Task HandelMessage(AccountEvent messageObject)
        {
            var crmObject = new AccountModel
            {                
                Name = $"{messageObject.Name}",
                Email = messageObject.Email,                
            };

            ResourceDto response;
            switch (messageObject.Method)
            {
                case MethodEnum.CREATE:
                    var resp = await _accountGateAway.CreateOrUpdate(crmObject);
                    if (resp.IsSuccessStatusCode)
                    {
                        // await _uUIDGateAway.PublishEntity(SourceEnum.CRM.ToString(), EntityTypeEnum.Account,  1);
                    }
                    break;
                case MethodEnum.UPDATE:
                    if (Guid.TryParse(messageObject.UUID_Nr, out Guid id))
                    {
                        response = await _uUIDGateAway.GetResource(id);
                        if (response == null)
                        {
                            _logger.LogError("response UUIDMaster was null - handelMessage - account");
                        }
                        else
                        {
                            crmObject.Id = response.SourceEntityId;
                            var result = await _accountGateAway.CreateOrUpdate(crmObject);

                            if (result.IsSuccessStatusCode)
                            {
                                await _uUIDGateAway.UpdateEntity(response.Uuid.ToString(), SourceEnum.CRM.ToString(), UUID.Model.EntityTypeEnum.Account, messageObject.EntityVersion);
                            }
                        }


                    }
                    break;
                case MethodEnum.DELETE:
                    break;
                default:
                    break;
            }

            // Call UUId Do Stuff
            // 
            // Map Data 
            // Send To crm
            _accountGateAway.CreateOrUpdate(crmObject);
        }
    }
}
