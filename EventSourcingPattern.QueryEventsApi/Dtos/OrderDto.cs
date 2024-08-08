namespace EventSourcingPattern.QueryEventsApi.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public virtual List<OrderItemDto> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
