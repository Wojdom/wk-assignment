using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Order.Create;
using MediatR;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        [HttpPost("Create")]
        public async Task<Guid> Create([FromBody]CreateRequest createRequest)
        {
            var result = await _mediator.Send(
                new CreateOrderCommand(createRequest));
            return result;
        }
    }
}
