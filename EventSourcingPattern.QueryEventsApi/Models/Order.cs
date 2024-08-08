using EventSourcingPattern.Shared.Events;

namespace EventSourcingPattern.QueryEventsApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
