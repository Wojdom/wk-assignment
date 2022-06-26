using System;
using System.Collections.Generic;
using Common.Domain;
using Common.Domain.Enums;
using Domain.Common;
using Domain.DiscountVoucher.ValueObjects;
using Domain.Order.ValueObjects;
using Domain.Product.ValueObjects;
using Domain.ShoppingCart;

namespace Domain.Order
{
	public class Order : AggregateRoot<OrderId>
	{
		private static int _orderCounter = 1;

		public string Remarks { get; set; }

        public int OrderNumber { get; private set; }

        public PaymentMethod PaymentMethod { get; set; }

		public DiscountVoucherId DiscountVoucherId { get; set; }

		public CreationDate CreationDate { get; private set; }

		private ICollection<OrderItem> _items { get; set; }

		public Order(OrderId orderId) : base(orderId)
		{
			OrderNumber = _orderCounter++;
			CreationDate = CreationDate.CreateOrNull(DateTime.Now);
		}

		public void AddItem(ProductId productId, Amount amount, Money unitPrice)
        {
			_items.Add(new OrderItem(productId, amount, unitPrice));
        }

		public Money CalculateTotalPrice()
		{
			decimal totalPrice = 0;
			foreach (var item in _items)
			{
                totalPrice += item.Amount.Value * item.UnitPrice.Value;
			}

			return new Money(totalPrice);
		}
	}
}

