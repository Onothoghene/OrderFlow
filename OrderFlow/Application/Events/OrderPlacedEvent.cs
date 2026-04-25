using MediatR;
using System.Collections.Generic;

namespace Application.Events
{
    public class OrderPlacedEvent : INotification
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<int> ProductIds { get; set; }

        public OrderPlacedEvent(int orderId, int userId, decimal totalAmount, List<int> productIds)
        {
            OrderId = orderId;
            UserId = userId;
            TotalAmount = totalAmount;
            ProductIds = productIds;
        }
    }
}