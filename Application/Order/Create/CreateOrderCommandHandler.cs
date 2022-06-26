using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain.ActionContext;
using Common.Domain.ValueObjects;
using Domain.DiscountVoucher;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.Product;
using Domain.ShoppingCart;
using Domain.ShoppingCart.ValueObjects;
using MediatR;

namespace Application.Order.Create
{
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IActionContextProvider _actionContextProvider;
        private readonly IDiscountVoucherRepository _discountVoucherRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository,
            IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository,
            IDiscountVoucherRepository discountVoucherRepository,
            IActionContextProvider actionContextProvider)
		{
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _shoppingCartRepository = shoppingCartRepository ?? throw new ArgumentNullException(nameof(shoppingCartRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _discountVoucherRepository = discountVoucherRepository ?? throw new ArgumentNullException(nameof(discountVoucherRepository));
            _actionContextProvider = actionContextProvider ?? throw new ArgumentNullException(nameof(actionContextProvider));
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var shoppingCartId = ShoppingCartId.CreateOrNull(command.CreateRequest.ShoppingCartId);
            var cart = await _shoppingCartRepository.GetAsync(shoppingCartId);

            var newOrderId = OrderId.CreateOrNull(Guid.NewGuid());
            var order = new Domain.Order.Order(newOrderId)
            {
                Remarks = command.CreateRequest.Remarks,
                PaymentMethod = command.CreateRequest.PaymentMethod
            };

            // validate voucher
            var voucher = await _discountVoucherRepository.GetAsync(cart.DiscountVoucherId);
            if (!voucher.IsUsed && voucher.ExpirationDate.IsAfter(order.CreationDate))
            {
                order.DiscountVoucherId = voucher.Id;
            }

            // create order items
            var cartItems = cart.GetProducts();
            foreach (var cartItem in cartItems)
            {
                var product = await _productRepository.GetAsync(cartItem.ProductId);
                order.AddItem(cartItem.ProductId, cartItem.Amount, product.Price);
            }

            // clear cart
            await _shoppingCartRepository.DeleteAsync(shoppingCartId);

            // save order
            await _orderRepository.InsertAsync(new Domain.Order.Order(newOrderId));

            return newOrderId;
        }
    }
}
