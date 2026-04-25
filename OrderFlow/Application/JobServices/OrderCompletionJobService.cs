using Application.Enums;
using Application.Helper;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Settings;
using Hangfire;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Application.JobServices
{
    public class OrderCompletionJob : IOrderCompletionJobService
    {
        private readonly IOrderRepositoryAsync _orderRepository;
        private readonly IRestaurantRepositoryAsync _restaurantRepository;

        public OrderCompletionJob(IOrderRepositoryAsync orderRepository, IRestaurantRepositoryAsync restaurantRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
        }

        [AutomaticRetry(Attempts = 3)] // Retry up to 3 times if it fails
        public async Task ProcessPendingOrders()
        {
            var pendingOrders = await _orderRepository.GetPendingOrdersAsync(); // Fetch active orders older than 15 mins

            foreach (var order in pendingOrders)
            {
                // Mark order as completed using the enum
                order.Status = (int)OrderEnum.Completed;
                await _orderRepository.UpdateAsync(order);

                // Make restaurant available again
                var restaurant = await _restaurantRepository.GetByIdAsync(order.RestaurantId);
                if (restaurant != null)
                {
                    restaurant.IsAvailable = true;
                    await _restaurantRepository.UpdateAsync(restaurant);
                }
            }
        }
    }

}
