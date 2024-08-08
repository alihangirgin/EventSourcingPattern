using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.QueryEventsApi.Services;
using MediatR;

namespace EventSourcingPattern.QueryEventsApi.Queries
{
    public sealed record GetOrdersQuery() : IRequest<List<OrderDto>>;
    public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery,List<OrderDto>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrders();
        }
    }
}
