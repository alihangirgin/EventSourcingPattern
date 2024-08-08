using EventSourcingPattern.CommandEventsApi.Constants;
using EventSourcingPattern.CommandEventsApi.Services;
using EventSourcingPattern.Shared.Events;
using MediatR;

namespace EventSourcingPattern.CommandEventsApi.Commands
{
    public sealed record CreateProductCommand(string Name, decimal Price, int Stock) : IRequest;

    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IEventStoreService _eventStoreService;

        public CreateProductCommandHandler(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ProductCreated productCreatedEvent = new()
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Price = command.Price,
                Stock = command.Stock,
                Timestamp = DateTime.Now
            };

            await _eventStoreService.AppendEventToStreamAsync(StreamConstants.ProductStream, productCreatedEvent,
                productCreatedEvent.GetType().Name);
        }
    }
}
