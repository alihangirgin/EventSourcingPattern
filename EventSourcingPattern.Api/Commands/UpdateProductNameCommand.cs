using EventSourcingPattern.Api.Constants;
using EventSourcingPattern.Api.Services;
using EventSourcingPattern.Shared.Events;
using MediatR;

namespace EventSourcingPattern.Api.Commands
{
    public sealed record UpdateProductNameCommand(Guid Id, string Name) : IRequest
    {
        public UpdateProductNameCommand SetId(Guid newId)
        {
            return this with { Id = newId };
        }
    }
    public sealed class UpdateProductNameCommandHandler : IRequestHandler<UpdateProductNameCommand>
    {
        private readonly IEventStoreService _eventStoreService;

        public UpdateProductNameCommandHandler(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task Handle(UpdateProductNameCommand command, CancellationToken cancellationToken)
        {
            ProductNameUpdated productNameUpdated = new()
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Timestamp = DateTime.Now
            };
            await _eventStoreService.AppendEventToStreamAsync(StreamConstants.ProductStream, productNameUpdated, 
                productNameUpdated.GetType().Name);
        }
    }
}
