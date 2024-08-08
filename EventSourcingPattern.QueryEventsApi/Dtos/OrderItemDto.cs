using EventSourcingPattern.QueryEventsApi.Models;

namespace EventSourcingPattern.QueryEventsApi.Dtos
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
