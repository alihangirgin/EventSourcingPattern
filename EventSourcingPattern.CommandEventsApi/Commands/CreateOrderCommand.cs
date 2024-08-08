using EventSourcingPattern.CommandEventsApi.Constants;
using EventSourcingPattern.CommandEventsApi.Services;
using EventSourcingPattern.Shared.Events;
using MediatR;

namespace EventSourcingPattern.CommandEventsApi.Commands
{
    public sealed record OrderItem(Guid ProductId, int Quantity);

    public sealed record CreateOrderCommand(List<OrderItem> OrderItems) : IRequest;

    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IEventStoreService _eventStoreService;

        public CreateOrderCommandHandler(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            OrderCreated orderCreated = new()
            {
                Id = Guid.NewGuid(),
                OrderItems = command.OrderItems.Select(x=> new OrderItemMessage()
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.ProductId,
                    Quantity = x.Quantity

                }).ToList()
            };
            await _eventStoreService.AppendEventToStreamAsync(StreamConstants.OrderStream, orderCreated,
                orderCreated.GetType().Name);
        }
    }
}
