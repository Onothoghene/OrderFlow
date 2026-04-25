using Application.DTOs.Orders;
using Application.Events;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.Order.Command
{
    public class AddOrUpdateOrderCommand : IRequest<Response<int>>
    {
        public int? Id { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentOption { get; set; }
        public int PaymentStatus { get; set; }
        public List<OrderItemsIM> OrderItems { get; set; }

        public class AddOrUpdateOrderCommandHandler : IRequestHandler<AddOrUpdateOrderCommand, Response<int>>
        {
            private readonly IMapper _mapper;
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IRestaurantRepositoryAsync _restaurantRepository;
            private readonly ICartItemRepository _cartItemRepository;
            private readonly IAuthenticatedUserService _userService;
            private readonly IPaymentRepositoryAsync _paymentRepository;
            private readonly IMenuItemRepositoryAsync _menuItemRepository;
            private readonly IMediator _mediator;
            private readonly ILogger<AddOrUpdateOrderCommandHandler> _logger;

            public AddOrUpdateOrderCommandHandler(IMapper mapper, IOrderRepositoryAsync orderRepository,
                                                 IRestaurantRepositoryAsync restaurantRepository,
                                                 ICartItemRepository cartItemRepository,
                                                 IAuthenticatedUserService userService,
                                                 IPaymentRepositoryAsync paymentRepository,
                                                 IMenuItemRepositoryAsync menuItemRepository,
                                                 IMediator mediator, ILogger<AddOrUpdateOrderCommandHandler> logger)
            {
                _mapper = mapper;
                _orderRepository = orderRepository;
                _restaurantRepository = restaurantRepository;
                _cartItemRepository = cartItemRepository;
                _userService = userService;
                _paymentRepository = paymentRepository;
                _menuItemRepository = menuItemRepository;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Response<int>> Handle(AddOrUpdateOrderCommand command, CancellationToken cancellationToken)
            {
                Orders order;
                var userId = _userService.UserId;

                _logger.LogInformation("Processing order request for User {UserId}", userId);

                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //PRODUCT TRACKING
                    var productIds = command.OrderItems.Select(x => x.FoodId).ToList();
                    var products = await _menuItemRepository.GetByIdsWithTrackingAsync(productIds);

                    if (products.Count != productIds.Count)
                    {
                        _logger.LogWarning("Product validation failed for User {UserId}", userId);
                        throw new ApiException("One or more products do not exist.");

                    }

                    // VALIDATE STOCK
                    foreach (var item in command.OrderItems)
                    {
                        var product = products.FirstOrDefault(x => x.Id == item.FoodId) ?? throw new ApiException($"Product {item.FoodId} not found");
                        if (product.StockQuantity < item.Quantity)
                        {
                            _logger.LogWarning("Insufficient stock for Product {ProductId}", product.Id);
                            throw new ApiException($"Insufficient stock for {product.Name}");
                        }
                    }

                    //CREATE/UPDATE ORDER
                    if (command.Id.HasValue && command.Id > 0)
                    {
                        order = await _orderRepository.GetByIdAsync(command.Id.Value);

                        if (order == null)
                        {
                            _logger.LogWarning("Order not found: {OrderId}", command.Id.Value);
                            throw new ApiException("Order not found");
                        }

                        _mapper.Map(command, order);

                        try
                        {
                            await _orderRepository.UpdateAsync(order);
                            _logger.LogInformation("Order updated: {OrderId}", order.Id);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            _logger.LogError("Concurrency conflict on Order {OrderId}", order.Id);
                            throw new ApiException("Order conflict detected. Please retry.");
                        }
                    }
                    else
                    {
                        order = _mapper.Map<Orders>(command);
                        await _orderRepository.AddAsync(order);

                        _logger.LogInformation("Order created: {OrderId} for User {UserId}", order.Id, userId);
                    }

                    // UPDATE ORDER ITEMS
                    order.OrderItems = [];
                    foreach (var item in command.OrderItems)
                    {
                        order.OrderItems.Add(new OrderItems
                        {
                            FoodId = item.FoodId,
                            Quantity = item.Quantity,
                            Subtotal = item.Subtotal,
                            OrderId = order.Id
                        });
                    }

                    // DEDUCT STOCK
                    //foreach (var item in command.OrderItems)
                    //{
                    //    var product = products.First(x => x.Id == item.FoodId);
                    //    product.StockQuantity -= item.Quantity;
                    //    await _menuItemRepository.UpdateAsync(product);
                    //}

                    foreach (var item in command.OrderItems)
                    {
                        var product = products.First(x => x.Id == item.FoodId);
                        product.StockQuantity -= item.Quantity;

                        try
                        {
                            await _menuItemRepository.UpdateAsync(product);
                            _logger.LogInformation("Stock updated for Product {ProductId}", product.Id);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            _logger.LogError("Stock conflict for Product {ProductId}", product.Id);
                            throw new ApiException($"Stock was updated by another request for {product.Name}. Try again.");
                        }
                    }

                    // Handle Payment Processing
                    var payment = await _paymentRepository.GetPaymentByOrderIdAsync(order.Id);
                    if (payment != null)
                    {
                        payment.AmountPaid = command.AmountPaid;
                        payment.PaymentOption = command.PaymentOption;
                        payment.PaymentStatus = command.PaymentStatus;

                        await _paymentRepository.UpdateAsync(payment);
                        _logger.LogInformation("Payment updated for Order {OrderId}", order.Id);
                    }
                    else
                    {
                        var newPayment = new Domain.Entities.Payment
                        {
                            OrderId = order.Id,
                            AmountPaid = command.AmountPaid,
                            PaymentOption = command.PaymentOption,
                            PaymentStatus = command.PaymentStatus
                        };
                        await _paymentRepository.AddAsync(newPayment);
                        _logger.LogInformation("Payment created for Order {OrderId}", order.Id);
                    }

                    //Update CART ITEMS
                    //set the items in the cart Ordered to true which signifies that the menu item has been bought at this time 
                    var cartItemIds = command.OrderItems.Select(x => x.FoodId).ToList();
                    var cartItems = await _cartItemRepository.GetCartItemsAsync(userId, cartItemIds);
                    if (cartItems != null)
                    {
                        foreach (var item in cartItems)
                        {
                            item.Ordered = true;

                            await _cartItemRepository.UpdateAsync(item);
                        }
                        _logger.LogInformation("Cart items updated for User {UserId}", userId);
                    }

                    ts.Complete();
                }

                await _mediator.Publish(new OrderPlacedEvent(
                                                    order.Id,
                                                    userId,
                                                    order.TotalAmount,
                                                    [.. command.OrderItems.Select(x => x.FoodId)]
                                                ), cancellationToken);

                _logger.LogInformation("OrderPlacedEvent published for Order {OrderId}", order.Id);


                return new Response<int>(order.Id, "Request processed successfully.");
            }
        }
    }
}
