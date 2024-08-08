using EventSourcingPattern.Api.Constants;
using EventSourcingPattern.Api.Services;
using EventSourcingPattern.Shared.Events;
using MediatR;

namespace EventSourcingPattern.Api.Commands
{
    public sealed record UpdateProductPriceCommand(Guid Id, decimal Price) : IRequest
    {
        public UpdateProductPriceCommand SetId(Guid id)
        {
            return this with { Id = id };
        }
    }
    public class UpdateProductPriceCommandHandler : IRequestHandler<UpdateProductPriceCommand>
    {
        private readonly IEventStoreService _eventStoreService;

        public UpdateProductPriceCommandHandler(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task Handle(UpdateProductPriceCommand command, CancellationToken cancellationToken)
        {
            ProductPriceUpdated productPriceUpdated = new()
            {
                Id = Guid.NewGuid(),
                Price = command.Price,
                Timestamp = DateTime.Now
            };
            _eventStoreService.AppendEventToStreamAsync(StreamConstants.ProductStream, productPriceUpdated,
                productPriceUpdated.GetType().Name);
        }
    }
}
