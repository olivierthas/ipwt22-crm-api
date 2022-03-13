using MediatR;

namespace Crm.Link.RabbitMq.Consumer
{
    public class LogCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
