using System;
using MediatR;

namespace Application.Order.Create
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public CreateRequest CreateRequest { get; set; }
        
        public CreateOrderCommand(CreateRequest createRequest)
        {
            createRequest = CreateRequest;
        }
    }
}
