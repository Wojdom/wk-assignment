using System;
using Common.Domain;
using Domain.Order.ValueObjects;

namespace Domain.Order
{
	public interface IOrderRepository : IRepository<Order, OrderId>
    {
	}
}

