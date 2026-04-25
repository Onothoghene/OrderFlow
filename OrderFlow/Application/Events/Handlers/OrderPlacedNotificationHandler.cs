using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Events.Handlers
{
    public class OrderPlacedNotificationHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedNotificationHandler> _logger;

        public OrderPlacedNotificationHandler(ILogger<OrderPlacedNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Notification sent for Order {OrderId} to User {UserId}. Total Amount: {TotalAmount}",
                                    notification.OrderId,
                                    notification.UserId,
                                    notification.TotalAmount);

            return Task.CompletedTask;
        }
    }
}