using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Application.Interfaces;

namespace Application.Features.Cart.Command
{
    public class AddOrUpdateCartItemCommand : IRequest<Response<bool>>
    {
        public int Quantity { get; set; }
        public int FoodId { get; set; }

        public class AddOrUpdateCartItemCommandHandler : IRequestHandler<AddOrUpdateCartItemCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly ICartItemRepository _cartItemRepository;
            private readonly IAuthenticatedUserService _userService;
            private readonly IMenuItemRepositoryAsync _menuItemRepository;

            public AddOrUpdateCartItemCommandHandler(IMapper mapper, ICartItemRepository cartItemRepository,
                                                    IAuthenticatedUserService userService,
                                                    IMenuItemRepositoryAsync menuItemRepository)
            {
                _mapper = mapper;
                _cartItemRepository = cartItemRepository;
                _userService = userService;
                _menuItemRepository = menuItemRepository;

            }

            public async Task<Response<bool>> Handle(AddOrUpdateCartItemCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = _userService.UserId;
                    // Get product
                    var product = await _menuItemRepository.GetByIdAsync(command.FoodId);

                    if (product == null)
                        throw new ApiException("Product not found");

                    if (command.Quantity <= 0)
                        throw new ApiException("Quantity must be greater than 0");

                    // Check if the item already exists in the user's cart
                    var item = await _cartItemRepository.GetUserMenuCartAsync(userId, command.FoodId);

                    int existingQuantity = item?.Quantity ?? 0;
                    int newTotalQuantity = existingQuantity + command.Quantity;

                    // STOCK VALIDATION
                    if (product.StockQuantity < newTotalQuantity)
                        throw new ApiException($"Insufficient stock. Available: {product.StockQuantity}");

                    if (item != null)
                    {
                        item.Quantity = newTotalQuantity;
                        await _cartItemRepository.UpdateAsync(item);
                    }
                    else
                    {
                        var data = _mapper.Map<CartItems>(command);
                        await _cartItemRepository.AddAsync(data);
                    }
                    ts.Complete();
                }

                return new Response<bool>(true, "Add item to cart successfully.");
            }
        }
    }
}

