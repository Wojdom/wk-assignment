using System;
using Common.Domain;
using Domain.Common;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart.ValueObjects;

namespace Domain.Order
{
	public class OrderItem : Entity<ItemId>
    {
        public ProductId ProductId { get; private set; }
        public Amount Amount { get; private set; }
        public Money UnitPrice { get; private set; }

        public OrderItem(ProductId productId, Amount amount, Money unitPrice)
		{
            ProductId = productId;
            Amount = amount;
            UnitPrice = unitPrice;
		}
	}
}

