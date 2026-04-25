using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Enums;

namespace Application.Features.Order.Command
{
    public class CancelOrderCommand : IRequest<Response<bool>>
    {
        public int OrderId { get; set; }

        public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IRestaurantRepositoryAsync _restaurantRepository;
            private readonly IMenuItemRepositoryAsync _menuItemRepository;

            public CancelOrderCommandHandler(IMapper mapper, IOrderRepositoryAsync orderRepository,
                                             IRestaurantRepositoryAsync restaurantRepository,
                                             IMenuItemRepositoryAsync menuItemRepository)
            {
                _mapper = mapper;
                _orderRepository = orderRepository;
                _restaurantRepository = restaurantRepository;
                _menuItemRepository = menuItemRepository;

            }

            public async Task<Response<bool>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var order = await _orderRepository.GetOrderById(command.OrderId);

                    if (order == null)
                        throw new ApiException("The requested order could not be found");

                    if (order.Status == (int)OrderEnum.Canceled)
                        throw new ApiException("Order is already canceled");

                    if (order.Status == (int)OrderEnum.Completed)
                        throw new ApiException("Completed orders cannot be canceled");

                    // RESTORE STOCK
                    foreach (var item in order.OrderItems)
                    {
                        var product = await _menuItemRepository.GetByIdAsync(item.FoodId);

                        if (product != null)
                        {
                            product.StockQuantity += item.Quantity;
                            await _menuItemRepository.UpdateAsync(product);
                        }
                    }

                    //CANCEL ORDER
                    order.Status = (int)OrderEnum.Canceled;
                    await _orderRepository.UpdateAsync(order);

                    ts.Complete();
                }

                return new Response<bool>(true, "Order canceled successfully.");
            }
        }
    }
}

