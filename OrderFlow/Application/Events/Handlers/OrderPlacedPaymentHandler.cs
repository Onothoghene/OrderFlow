using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Events.Handlers
{
    public class OrderPlacedPaymentHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedPaymentHandler> _logger;

        public OrderPlacedPaymentHandler(ILogger<OrderPlacedPaymentHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Payment processing started for Order {OrderId}, Amount: {TotalAmount}",
                                   notification.OrderId,
                                   notification.TotalAmount);

            return Task.CompletedTask;
        }
    }
}