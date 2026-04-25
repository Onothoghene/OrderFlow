using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Events.Handlers
{
    public class OrderPlacedInventoryHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedInventoryHandler> _logger;

        public OrderPlacedInventoryHandler(ILogger<OrderPlacedInventoryHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inventory confirmed for Order {OrderId}. Products: {@ProductIds}",
                                   notification.OrderId,
                                   notification.ProductIds);

            return Task.CompletedTask;
        }
    }
}