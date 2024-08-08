using EventSourcingPattern.QueryEventsApi.Dtos;
using EventSourcingPattern.Shared.Events;

namespace EventSourcingPattern.QueryEventsApi.Services
{
    public interface IOrderService
    {
        Task CreateOrder(OrderCreated orderCreated);
        Task<List<OrderDto>> GetOrders();
    }
}
