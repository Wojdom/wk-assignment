using System;
using Common.Domain.ValueObjects;

namespace Domain.Order.ValueObjects
{
	public class CreationDate : DateTimeValueObject
	{
        public CreationDate() : base()
        {
        }

        public CreationDate(DateTime date) : base(date)
        {
        }

        public static CreationDate CreateOrNull(DateTime? date) => date is null ? null : new CreationDate(date.Value);
    }
}

