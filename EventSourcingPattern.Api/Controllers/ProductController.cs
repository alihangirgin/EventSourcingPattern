using EventSourcingPattern.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingPattern.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody]CreateProductCommand command)
        {
             await _mediator.Send(command);
             return Ok();
        }

        [HttpPatch("{id}/name")]
        public async Task<IActionResult> UpdateProductNameAsync(Guid id, [FromBody]UpdateProductNameCommand command)
        {
            await _mediator.Send(command.SetId(id));
            return Ok();
        }

        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdateProductPriceAsync(Guid id, [FromBody] UpdateProductPriceCommand command)
        {
            await _mediator.Send(command.SetId(id));
            return Ok();
        }
    }
}
