using System;
using Common.Domain.ValueObjects.Identifiers;

namespace Domain.Order.ValueObjects
{
    public class OrderId : GuidIdValueObject
    {
        public OrderId() : base()
        {
        }

        public OrderId(Guid id) : base(id)
        {
        }

        public static OrderId CreateOrNull(Guid? id) => id is null ? null : new OrderId(id.Value);
    }
}
