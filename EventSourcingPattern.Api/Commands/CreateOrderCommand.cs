using EventSourcingPattern.Api.Constants;
using EventSourcingPattern.Api.Services;
using EventSourcingPattern.Shared.Events;
using MediatR;

namespace EventSourcingPattern.Api.Commands
{
    public sealed record OrderItem(Guid ProductId, int Quantity);

    public sealed record CreateOrderCommand(List<OrderItem> OrderItems) : IRequest;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
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
                    ProductId = x.ProductId,
                    Quantity = x.Quantity

                }).ToList()
            };
            await _eventStoreService.AppendEventToStreamAsync(StreamConstants.OrderStream, orderCreated,
                orderCreated.GetType().Name);
        }
    }
}
